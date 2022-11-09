Shader "Unlit/Galaxy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Pixels("Pixels", range(10,10000)) = 100.0
        _Rotation("Rotation",range(0.0, 6.28)) = 0.0
        _Time_speed("Time Speed",range(0.0, 1.0)) = 0.2

        _Dither_size("Dither Size",range(0.0, 10.0)) = 2.0
        _should_dither("Should Dither", float) = 1

        _GradientTex("Texture", 2D) = "white"{}

        _Size("Size",float) = 50.0
        _OCTAVES("OCTAVES", range(0,20)) = 0
        _Seed("Seed",range(1, 10)) = 1
        time("time",float) = 0.0

        tilt("tilt",float) = 4.0
        n_layers("n_layers",float) = 4.0
        layer_height("layer_height",float) = 0.4
        zoom("zoom",float) = 2.0
        n_colors("n_colors",float) = 7.0
        swirl("swirl",float) = -9.0

    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}
        LOD 100

        Pass
        {
            Tags { "LightMode" = "UniversalForward"}

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Pixels;
            float _Rotation;
            float _Time_speed;
            float _Dither_size;
            float _should_dither;

            float _Size;
            int _OCTAVES;
            int _Seed;
            float time;

            float tilt;
            float n_layers;
            float layer_height;
            float zoom;
            float n_colors;
            float swirl;

            sampler2D _GradientTex;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float rand(float2 coord) {
                coord = mod(coord, float2(1.0, 1.0) * round(_Size));
                return frac(sin(dot(coord.xy, float2(12.9898, 78.233))) * 15.5453 * _Seed);
            }

            float noise(float2 coord) {
                float2 i = floor(coord);
                float2 f = frac(coord);

                float a = rand(i);
                float b = rand(i + float2(1.0, 0.0));
                float c = rand(i + float2(0.0, 1.0));
                float d = rand(i + float2(1.0, 1.0));

                float2 cubic = f * f * (3.0 - 2.0 * f);

                return lerp(a, b, cubic.x) + (c - a) * cubic.y * (1.0 - cubic.x) + (d - b) * cubic.x * cubic.y;
            }

            float fbm(float2 coord) {
                float value = 0.0;
                float scale = 0.5;

                for (int i = 0; i < _OCTAVES; i++) {
                    value += noise(coord) * scale;
                    coord *= 2.0;
                    scale *= 0.5;
                }
                return value;
            }

            float2 rotate(float2 coord, float angle) {
                coord -= 0.5;
                //coord *= float2x2(float2(cos(angle),-sin(angle)),float2(sin(angle),cos(angle)));
                coord = mul(coord, float2x2(float2(cos(angle), -sin(angle)), float2(sin(angle), cos(angle))));
                return coord + 0.5;
            }

            bool dither(float2 uv_pixel, float2 uv_real) {
                return mod(uv_pixel.x + uv_real.y, 2.0 / _Pixels) <= 1.0 / _Pixels;
            }

            fixed4 frag (v2f i) : COLOR
            {

                float2 uv = floor(i.uv * _Pixels) / _Pixels;
                bool dith = dither(uv, i.uv);

                // I added a little zooming functionality so I dont have to mess with other values to get correct sizing.
                uv *= zoom;
                uv -= (zoom - 1.0) / 2.0;

                // overall rotation of galaxy
                uv = rotate(uv, _Rotation);
                float2 uv2 = uv; // save a copy of untranslated uv for later

                // this uv is used to determine where the "layers" will be
                uv.y *= tilt;
                uv.y -= (tilt - 1.0) / 2.0;

                float d_to_center = distance(uv, float2(0.5, 0.5));

                // swirl uv around the center, the further from the center the more rotated.
                float rot = swirl * pow(d_to_center, 0.4);
                float2 rotated_uv = rotate(uv, rot + time * _Time_speed);

                // fbm will decide where the layers are
                float f1 = fbm(rotated_uv * _Size);
                // quantize to a few different values, so layers don't blur through each other
                f1 = floor(f1 * n_layers) / n_layers;

                // use the unaltered second uv for the actual galaxy
                // tilt so it looks like it's an angle.
                uv2.y *= tilt;
                uv2.y -= (tilt - 1.0) / 2.0 + f1 * layer_height;

                // now do the same stuff as before, but for the actual galaxy image, not the layers
                float d_to_center2 = distance(uv2, float2(0.5, 0.5));
                float rot2 = swirl * pow(d_to_center2, 0.4);
                float2 rotated_uv2 = rotate(uv2, rot2 + time * _Time_speed);

                // I offset the second fbm by some amount so the don't all use the same noise, try it wihout and the layers are very obvious
                float f2 = fbm(rotated_uv2 * _Size + float2(f1, f1) * 10.0);

                // alpha
                float a = step(f2 + d_to_center2, 0.7);

                // some final steps to choose a nice color
                f2 *= 2.3;
                if (_should_dither && dith) { // dithering
                    f2 *= 0.94;
                }

                f2 = floor(f2 * (n_colors + 1.0)) / n_colors;

                float3 col = tex2D(_GradientTex, float2(f2, 0.0)).rgb;

                
                return fixed4(col, a);

            }
            ENDCG
        }
    }
}
