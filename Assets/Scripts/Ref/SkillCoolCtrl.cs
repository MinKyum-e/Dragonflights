using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolCtrl : MonoBehaviour
{
    [HideInInspector] public SkillType m_SkType;
    public Sprite[] m_IconImg = null;
    float Skill_Time = 0.0f;
    float Skill_Delay = 0.0f;
    public Image Time_Image = null;
    public Image Icon_Image = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Skill_Time -= Time.deltaTime;
        Time_Image.fillAmount = Skill_Time / Skill_Delay;

        if (Skill_Time <= 0.0f)
            Destroy(gameObject);
    }

    public void InitState(SkillType a_SkType, float a_Time, float a_Delay)
    {
        m_SkType = a_SkType;
        Icon_Image.sprite = m_IconImg[(int)m_SkType];

        Skill_Time  = a_Time;
        Skill_Delay = a_Delay;
    }
}
