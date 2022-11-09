Shader "Unlit/BlackholeRing"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	    _Pixels("Pixels", range(10,300)) = 100.0
	    _Rotation("Rotation",range(0.0, 6.28)) = 0.0    		    
    	_Light_origin("Light origin", Vector) = (0.39,0.39,0.39,0.39)
	    _Time_speed("Time Speed",range(-1.0, 1.0)) = 0.2

        _Disk_width("Disk Width", range(0.0, 0.15)) = 0.1
        _should_dither("Should Dither", float) = 1
	    _Ring_perspective("Ring Perspective",float) = 4.0
        _GradientTex("Texture", 2D) = "white"{}

	    _Size("Size",float) = 50.0
	    _OCTAVES("OCTAVES", range(0,20)) = 0
	    _Seed("Seed",range(1, 10)) = 1
	    time("time",float) = 0.0
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
        LOD 100

        Pass
        {
			Tags { "LightMode"="UniversalForward"}

			CULL Off
			ZWrite Off // don't write to depth buffer 
         	Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
        	
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "../cginc/hlmod.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Pixels;
            float _Rotation;
			float2 _Light_origin;    	
			float _Time_speed;
            float _Disk_width;
            float _Ring_perspective;
            bool _should_dither;
            float _Size;
            int _OCTAVES;
            int _Seed;
			float time;
            sampler2D _GradientTex;
			struct Input
	        {
	            float2 uv_MainTex;
	        };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
			float rand(float2 coord) {
				coord = mod(coord, float2(2.0,1.0)*round(_Size));
				return frac(sin(dot(coord.xy ,float2(12.9898,78.233))) * 15.5453 * _Seed);
			}

			float noise(float2 coord){
				float2 i = floor(coord);
				float2 f = frac(coord);
				
				float a = rand(i);
				float b = rand(i + float2(1.0, 0.0));
				float c = rand(i + float2(0.0, 1.0));
				float d = rand(i + float2(1.0, 1.0));

				float2 cubic = f * f * (3.0 - 2.0 * f);

				return lerp(a, b, cubic.x) + (c - a) * cubic.y * (1.0 - cubic.x) + (d - b) * cubic.x * cubic.y;
			}

			float fbm(float2 coord){
				float value = 0.0;
				float scale = 0.5;

				for(int i = 0; i < _OCTAVES ; i++){
					value += noise(coord) * scale;
					coord *= 2.0;
					scale *= 0.5;
				}
				return value;
			}


			// by Leukbaars from https://www.shadertoy.com/view/4tK3zR
			float circleNoise(float2 uv) {
			    float uv_y = floor(uv.y);
			    uv.x += uv_y*.31;
			    float2 f = frac(uv);
				float h = rand(float2(floor(uv.x),floor(uv_y)));
			    float m = (length(f-0.25-(h*0.5)));
			    float r = h*0.25;
			    return smoothstep(0.0, r, m*0.75);
			}

			bool dither(float2 uv_pixel, float2 uv_real) {
				return mod(uv_pixel.x+uv_real.y,2.0/_Pixels) <= 1.0 / _Pixels;
			}

			float2 spherify(float2 uv) {
				float2 centered= uv *2.0-1.0;
				float z = sqrt(1.0 - dot(centered.xy, centered.xy));
				float2 sphere = centered/(z + 1.0);
				return sphere * 0.5+0.5;
			}

			float2 rotate(float2 coord, float angle){
				coord -= 0.5;
				//coord *= float2x2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle)));
            	coord = mul(coord,float2x2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle))));
				return coord + 0.5;
			}

			fixed4 frag(v2f i) : COLOR {
                float2 uv = floor(i.uv * _Pixels) / _Pixels;

                bool dith = dither(i.uv, uv);

                uv = rotate(uv, _Rotation);

                float2 uv2 = uv;

                // compress uv along the x axis, or the accretion disk will look to stretched out
                uv.x -= 0.5;
                uv.x *= 1.3;
                uv.x += 0.5;

                // add a bit of movement to the accretion disk by wobbling it, completely optional and can be disabled.
                //uv = rotate(uv, sin(time * _Time_speed * 2.0) * 0.01);

                // l_origin will be used to determine how to color the pixels
                float2 l_origin = float2(0.5, 0.5);
                // d_width will be the final width of the accretion disk
                float d_width = _Disk_width;

                // here we distort the uvs to achieve the shape of the accretion disk
                if (uv.y < 0.5) {
                    // if we are in the top half of the image, then add to the uv.y based on how close we are to the center
                    uv.y += smoothstep(distance(float2(0.5, 0.5), uv), 0.5, 0.2);
                    // and also the ring width has to be adjusted or it will look to stretched out
                    d_width += smoothstep(distance(float2(0.5, 0.5), uv), 0.5, 0.3);

                    // another optional thing that changes the color distribution, I like it, but can be disabled.
                    l_origin.y -= smoothstep(distance(float2(0.5, 0.5), uv), 0.5, 0.2);
                }
                // we don't check for exactly uv.y > 0.5 because we want a small area where the ring
                // is unaffected by stretching, the middle part that goes over the black hole.
                else if (uv.y > 0.53) {

                    // same steps as before, but uv.y and light is stretched the other way, the disk width is slightly smaller here for visual effect.
                    uv.y -= smoothstep(distance(float2(0.5, 0.5), uv), 0.4, 0.17);
                    d_width += smoothstep(distance(float2(0.5, 0.5), uv), 0.5, 0.2);
                    l_origin.y += smoothstep(distance(float2(0.5, 0.5), uv), 0.5, 0.2);
                }

                // get distance to light origin based on unaltered uv's we saved earlier, some math to account for perspective
                float light_d = distance(uv2 * float2(1.0, _Ring_perspective), l_origin * float2(1.0, _Ring_perspective)) * 0.3;

                // center is used to determine ring position
                float2 uv_center = uv - float2(0.0, 0.5);

                // tilt ring
                uv_center *= float2(1.0, _Ring_perspective);
                float center_d = distance(uv_center, float2(0.5, 0.0));

                // cut out 2 circles of different sizes and only intersection of the 2.
                // this actually makes the disk
                float disk = smoothstep(0.1 - d_width * 2.0, 0.5 - d_width, center_d);
                disk *= smoothstep(center_d - d_width, center_d, 0.4);

                // rotate noise in the disk
                uv_center = rotate(uv_center + float2(0, 0.5), time * _Time_speed * 3.0);
                // some noise
                disk *= pow(fbm(uv_center * _Size), 0.5);

                // apply dithering
                if (dith || _should_dither != 1) {
                    disk *= 1.2;
                }

                // apply some colors based on final value
                float posterized = floor((disk + light_d) * 4.0) / 4.0;
                float3 col;
                col = tex2D(_GradientTex, float2(posterized, uv.y)).rgb;

                // this can be toggled on to achieve a more "realistic" blacak hole, with red and blue shifting. This was just me messing around so can probably be more optimized and done cleaner
                //	col.rgb *= 1.0 - pow(uv.x, 1.0);
                //	col.gb *= 1.0 - pow(uv.x, 2.0);
                //	col.b *= 3.0 - pow(uv.x, 4.0);
                //	col.gb *= 2.0 - pow(uv.x, 2.0);
                //	col.rgb *= pow(uv.x, 0.15);

                float disk_a = step(0.15, disk);
                return fixed4(col, disk_a);
			}
            
            ENDCG
        }
    }
}
