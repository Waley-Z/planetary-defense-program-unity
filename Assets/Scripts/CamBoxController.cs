using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamBoxController : MonoBehaviour
{
    BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        float downBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float upBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;

        boxCollider.size = new Vector2(rightBorder - leftBorder - 0.5f, upBorder - downBorder - 0.5f);
    }
}
