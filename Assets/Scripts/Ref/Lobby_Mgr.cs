using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_Mgr : MonoBehaviour
{
    public Image m_FadePanel = null;
    Animator m_RefAnimator = null;

    public Button m_Start_Btn;
    public Button m_LogOut_Btn;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f; //일시정지 풀어주기

        if (m_FadePanel != null)
            m_FadePanel.gameObject.SetActive(true);

        m_RefAnimator = m_FadePanel.gameObject.GetComponent<Animator>();

        if (m_Start_Btn != null)
            m_Start_Btn.onClick.AddListener(StartBtnClick);

        if (m_LogOut_Btn != null)
            m_LogOut_Btn.onClick.AddListener(LogOutBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartBtnClick()
    {
        // UnityEngine.SceneManagement.SceneManager.LoadScene("InGame");

        FadeCtrl.g_SceneName = "InGame";
        if (m_FadePanel != null)
            m_FadePanel.gameObject.SetActive(true);

        if (m_RefAnimator != null)
            m_RefAnimator.Play("FadeOut");
    }

    void LogOutBtnClick()
    {
        // UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");

        FadeCtrl.g_SceneName = "TitleScene";
        if (m_FadePanel != null)
            m_FadePanel.gameObject.SetActive(true);

        if (m_RefAnimator != null)
            m_RefAnimator.Play("FadeOut");
    }
}
