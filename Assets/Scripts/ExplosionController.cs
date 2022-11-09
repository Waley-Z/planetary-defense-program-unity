using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public List<Sprite> sprites;
    public float duration = 2f;
    private SpriteRenderer sr;
    float startTime;

    void Awake()
    {
        transform.Rotate(0, 0, Random.Range(0f, 360f));
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float progress = (Time.time - startTime) / duration;
        if (progress > 1)
            Destroy(gameObject);
        else
        {
            int index = Mathf.FloorToInt(progress * 9) + 3;
            Debug.Log(index);
            sr.sprite = sprites[index];
        }
    }
}
