using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCtrl : MonoBehaviour
{
    public static string g_SceneName = ""; //이동을 원하는 씬 이름

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void FadeOutFinish()
    {
        if (g_SceneName == "")
            return;

        UnityEngine.SceneManagement.SceneManager.LoadScene(g_SceneName);
    }

    public void FadeInFinish()
    {
        this.gameObject.SetActive(false);
    }
}
