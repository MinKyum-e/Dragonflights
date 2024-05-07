using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title_Mgr : MonoBehaviour
{
    public Button m_Start_Btn;
    public Image m_FadePanel;

    // Start is called before the first frame update
    void Start()
    {
        if (m_Start_Btn != null)
            m_Start_Btn.onClick.AddListener(StartBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartBtnClick()
    {
        // Debug.Log("Start버튼 클릭");
        // UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
        FadeCtrl.g_SceneName = "LobbyScene";
        m_FadePanel.gameObject.SetActive(true);
    }
}
