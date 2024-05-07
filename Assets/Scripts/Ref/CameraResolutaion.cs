using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolutaion : MonoBehaviour
{
    //스크린의 월드 좌표
    public static Vector3 m_SceenWMin = new Vector3(-10.0f, -5.0f, 0.0f);
    public static Vector3 m_SceenWMax = new Vector3(10.0f, 5.0f, 0.0f);
    //스크린의 월드 좌표

    // Start is called before the first frame update
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) /
                            ((float)16 / 9);
        float scalewidth = 1f / scaleheight;

        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;

        //------스크린의 월드 좌표 구하기
        Vector3 a_ScMin = new Vector3(0.0f, 0.0f, 0.0f); //ScreenViewPort 좌측하단
        m_SceenWMin = camera.ViewportToWorldPoint(a_ScMin);
        //카메라 화면 좌측하단 코너의 월드 좌표

        Vector3 a_ScMax = new Vector3(1.0f, 1.0f, 0.0f); //ScreenViewPort 우측상단
        m_SceenWMax = camera.ViewportToWorldPoint(a_ScMax);
        //카메라 화면 우측상단 코너의 월드 좌표
        //------스크린의 월드 좌표 구하기
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
