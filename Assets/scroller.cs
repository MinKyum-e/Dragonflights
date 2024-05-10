using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroller : MonoBehaviour
{
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();  
    }

    // Update is called once per frame
    void Update()
    {
        sr.material.mainTextureOffset += Vector2.up * Time.deltaTime * 0.5f;
    }
}
