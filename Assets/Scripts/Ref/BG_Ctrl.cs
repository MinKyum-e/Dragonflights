using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Ctrl : MonoBehaviour
{
    float ScrollSpeed = 0.2f;
    float Offset;
    MeshRenderer m_Render;

    // Start is called before the first frame update
    void Start()
    {
        m_Render = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Offset += Time.deltaTime * ScrollSpeed;
        m_Render.material.mainTextureOffset = new Vector2(Offset, 0);
    }
}
