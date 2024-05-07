using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigBox : MonoBehaviour
{
    public Button m_Ok_Btn = null;
    public Button m_Close_Btn = null;
    public InputField IDInputField = null;

    // Start is called before the first frame update
    void Start()
    {
        if (m_Ok_Btn != null)
            m_Ok_Btn.onClick.AddListener(OKBtnClick);

        if (m_Close_Btn != null)
            m_Close_Btn.onClick.AddListener(CloseBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OKBtnClick()
    {
        string a_NickStr = IDInputField.text.Trim();
        if(string.IsNullOrEmpty(a_NickStr) == true)
        {
            Debug.Log("별명은 빈칸 없이 입력해 주셔야 합니다.");
            return;
        }

        if((2 <= a_NickStr.Length && a_NickStr.Length < 20) == false)
        {
            Debug.Log("별명은 2글자 이상 20글자 이하로 작성해 주세요.");
            return;
        }

        GameObject a_TextObj = GameObject.Find("UserInfo_Text");
        if(a_TextObj != null)
        {
            Text a_refText = a_TextObj.GetComponent<Text>();
            if (a_refText != null)
                a_refText.text = "내정보 : 별명(" + a_NickStr + ")";
        }//if(a_TextObj != null)

        PlayerPrefs.SetString("NickName", a_NickStr);

        Time.timeScale = 1.0f;
        this.gameObject.SetActive(false);
    }

    void CloseBtnClick()
    {
        Time.timeScale = 1.0f;
        this.gameObject.SetActive(false);
    }
}
