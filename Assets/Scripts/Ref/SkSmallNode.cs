using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkSmallNode : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType;
    [HideInInspector] public int m_CurSkCount = 5;
    Text m_SkCountText;  //스킬 카운트
    RawImage m_CrIconImg;   //캐릭터 아이콘 이미지

    // Start is called before the first frame update
    void Start()
    {
        Transform a_CountBack = transform.Find("CountBack");
        if(a_CountBack != null)
            m_SkCountText = a_CountBack.Find("SkCountText").GetComponent<Text>();
        m_CrIconImg = transform.Find("IconImg").GetComponent<RawImage>();

        //이 버튼을 눌러서 스킬을 사용하게 구현
        Button m_BtnCom = this.GetComponent<Button>();
        if (m_BtnCom != null)
            m_BtnCom.onClick.AddListener(() =>
            {
                if (m_CurSkCount <= 0)
                    return;

                HeroCtrl a_Hero = GameObject.FindObjectOfType<HeroCtrl>();
                if(a_Hero != null)
                {
                    bool a_IsOk = a_Hero.UseSkill(m_SkType);
                    if (a_IsOk == true)
                        m_CurSkCount--;
                }
                Refresh_UI(m_SkType);
            });
        //이 버튼을 눌러서 스킬을 사용하게 구현
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitState(SkillType a_SkType)
    {
        Transform a_CountBack = transform.Find("CountBack");
        if (a_CountBack != null)
            m_SkCountText = a_CountBack.Find("SkCountText").GetComponent<Text>();
        m_CrIconImg = transform.Find("IconImg").GetComponent<RawImage>();

        m_CurSkCount = 5;
        m_SkType = a_SkType;

        if (m_SkCountText != null)
            m_SkCountText.text = m_CurSkCount.ToString() + " / " + 5;
    }

    public void Refresh_UI(SkillType a_SkType)
    {
        if (m_SkCountText != null)
            m_SkCountText.text = m_CurSkCount.ToString() + " / " + 5;

        if(m_CurSkCount <= 0)
        {
            if (m_CrIconImg != null)
                m_CrIconImg.color = new Color32(255, 255, 255, 80);
        }
    }
}
