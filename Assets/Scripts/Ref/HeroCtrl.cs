using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroCtrl : MonoBehaviour
{
    float m_MaxHP = 200.0f;
    float m_CurHP = 200.0f;
    public Image m_HPSdBar = null; //using UnityEngine.UI; 필요

    float h = 0.0f;
    float v = 0.0f;

    float moveSpeed = 7.0f;
    Vector3 moveDir = Vector3.zero;

    public GameObject m_BulletObj = null;
    public GameObject m_ShootPos  = null;
    float m_ShootCool = 0.0f;  //총알 발사 주기 계산용 변수

    //----------------주인공이 지형 밖으로 나갈 수 없도록 막기 위한 변수 
    Vector3 HalfSize = Vector3.zero;
    Vector3 m_CacCurPos = Vector3.zero;

    float a_LmtBdLeft = 0;
    float a_LmtBdTop = 0;
    float a_LmtBdRight = 0;
    float a_LmtBdBottom = 0;
    //----------------주인공이 지형 밖으로 나갈 수 없도록 막기 위한 변수 

    //------- Wolf 스킬
    public GameObject m_Wolf_Obj = null;
    //------- Wolf 스킬

    //------- 쉴드 스킬
    float m_SdOnTime = 0.0f;
    float m_SdDuration = 12.0f;
    public GameObject ShieldObj = null;
    //------- 쉴드 스킬

    //------- 유도탄 스킬
    [HideInInspector] public bool IsHoming = false;
    float m_Homing_OnTime = 0.0f;
    float m_HomingDur = 12.0f;
    //------- 유도탄 스킬

    //------- Double Shoot
    [HideInInspector] public float m_Double_OnTime = 0.0f;
    float m_Double_Dur = 12.0f;
    //------- Double Shoot

    //------- Sub Hero
    int Sub_Count = 0;
    float m_Sub_OnTime = 0.0f;
    float m_Sub_Dur = 12.0f;
    public GameObject Sub_Obj = null;
    public GameObject Sub_Parent = null;
    //------- Sub Hero

    // Start is called before the first frame update
    void Start()
    {
        //----- 캐릭터의 가로 반사이즈, 세로 반사이즈 구하기
        //월드에 그려진 스프라이트 사이즈 얻어오기
        SpriteRenderer sprRend = gameObject.GetComponentInChildren<SpriteRenderer>();
        HalfSize.x = sprRend.bounds.size.x / 2.0f - 0.23f; //여백이 커서 조금 줄임
        HalfSize.y = sprRend.bounds.size.y / 2.0f - 0.05f;
        HalfSize.z = 1.0f;
        //----- 캐릭터의 가로 반사이즈, 세로 반사이즈 구하기
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if(h != 0.0f || v != 0.0f)
        {
            moveDir = new Vector3(h, v, 0);
            if (1.0f < moveDir.magnitude)
                moveDir.Normalize();
            transform.position +=
                moveDir * moveSpeed * Time.deltaTime;
        } //if(h != 0.0f || v != 0.0f)

        LimitMove();

        FireUpdate();  //총알 발사 함수

        Update_Skill();

    } //void Update()

    void LimitMove()
    {
        m_CacCurPos = transform.position;

        a_LmtBdLeft  = CameraResolutaion.m_SceenWMin.x + HalfSize.x;
        a_LmtBdTop   = CameraResolutaion.m_SceenWMin.y + HalfSize.y;
        a_LmtBdRight  = CameraResolutaion.m_SceenWMax.x - HalfSize.x;
        a_LmtBdBottom = CameraResolutaion.m_SceenWMax.y - HalfSize.y;

        if (m_CacCurPos.x < a_LmtBdLeft)
            m_CacCurPos.x = a_LmtBdLeft;

        if (a_LmtBdRight < m_CacCurPos.x)
            m_CacCurPos.x = a_LmtBdRight;

        if (m_CacCurPos.y < a_LmtBdTop)
            m_CacCurPos.y = a_LmtBdTop;

        if (a_LmtBdBottom < m_CacCurPos.y)
            m_CacCurPos.y = a_LmtBdBottom;

        transform.position = m_CacCurPos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Monster")
        {
            TakeDamage(50.0f);
            //Destroy(col.gameObject);

            MonsterCtrl a_RefMon = col.gameObject.GetComponent<MonsterCtrl>();
            if (a_RefMon != null)
                a_RefMon.TakeDamage(700);
        }
        else if(col.gameObject.name.Contains("CoinItem") == true)
        {
            Game_Mgr.Inst.AddGold();
            Destroy(col.gameObject);
        }
        else if(col.tag == "EnemyBullet")
        {
            TakeDamage(20.0f);
            Destroy(col.gameObject);
        }
    }

    void TakeDamage(float a_Value)
    {
        if (m_CurHP <= 0.0f)    
            return;

        if (0.0f < m_SdOnTime)  //쉴드 스킬 발동 중 일 때 
            return;

        Game_Mgr.Inst.DamageTxt(-a_Value, transform.position, Color.blue);

        m_CurHP = m_CurHP - a_Value;
        if (m_CurHP < 0.0f)
            m_CurHP = 0.0f;

        if (m_HPSdBar != null)
            m_HPSdBar.fillAmount = m_CurHP / m_MaxHP;

        if(m_CurHP <= 0.0f)
        {
            Time.timeScale = 0.0f; //일시정지
            Game_Mgr.Inst.GameOverFunc();
        }
    }

    public bool UseSkill(SkillType a_SkType)
    {
        if (m_CurHP <= 0)  //이렇게 하면 사망처리시 스킬 사용 불가
            return false;

        if (a_SkType < 0 || SkillType.SkCount <= a_SkType) 
            return false;

        if(a_SkType == SkillType.Skill_0)
        {
            m_CurHP += m_MaxHP * 0.5f;
            Game_Mgr.Inst.DamageTxt(m_MaxHP * 0.5f,
                                    transform.position, new Color(0.18f, 0.5f, 0.34f));
            if (m_MaxHP < m_CurHP)
                m_CurHP = m_MaxHP;
            if (m_HPSdBar != null)
                m_HPSdBar.fillAmount = m_CurHP / m_MaxHP;
        }//if(a_SkType == SkillType.Skill_0)
        else if(a_SkType == SkillType.Skill_1)
        {
            GameObject obj = Instantiate(m_Wolf_Obj) as GameObject;
            obj.transform.position =
                    new Vector3(CameraResolutaion.m_SceenWMin.x - 1.0f, 0.0f, 0.0f);

        }//else if(a_SkType == SkillType.Skill_1)
        else if(a_SkType == SkillType.Skill_2)
        {
            if (0.0f < m_SdOnTime)
                return false;

            m_SdOnTime = m_SdDuration;

            Game_Mgr.Inst.SkillTimeFunc(a_SkType, m_SdOnTime, m_SdDuration);
        } //else if(a_SkType == SkillType.Skill_2)
        else if(a_SkType == SkillType.Skill_3)
        {
            if (0.0f < m_Homing_OnTime)
                return false;

            m_Homing_OnTime = m_HomingDur;

            Game_Mgr.Inst.SkillTimeFunc(a_SkType, m_Homing_OnTime, m_HomingDur);
        } //else if(a_SkType == SkillType.Skill_3)
        else if(a_SkType == SkillType.Skill_4)
        { //더블샷
            if (0.0f < m_Double_OnTime)
                return false;

            m_Double_OnTime = m_Double_Dur;

            Game_Mgr.Inst.SkillTimeFunc(a_SkType, m_Double_OnTime, m_Double_Dur);
        } //else if(a_SkType == SkillType.Skill_4)
        else if(a_SkType == SkillType.Skill_5)
        { //소환수 공격
            if (0.0f < m_Sub_OnTime)
                return false;

            Sub_Count = 3;
            m_Sub_OnTime = m_Sub_Dur;

            for(int ii = 0; ii < Sub_Count; ii++)
            {
                GameObject obj = Instantiate(Sub_Obj) as GameObject;
                obj.transform.SetParent(Sub_Parent.transform);
                SubHero_Ctrl sub = obj.GetComponent<SubHero_Ctrl>();
                sub.SubHeroSpawn(this.gameObject, (360 / Sub_Count) * ii, m_Sub_OnTime);
            }

            Game_Mgr.Inst.SkillTimeFunc(a_SkType, m_Sub_OnTime, m_Sub_Dur);

        } //else if(a_SkType == SkillType.Skill_5)
        else
        {
            Debug.Log("미등록 스킬");
            return false;
        }

        return true;

    } //public bool UseSkill(SkillType a_SkType)

    void Update_Skill()  //스킬 상태 업데이트
    {
        //----- 쉴드 상태 업데이트
        if(0.0f < m_SdOnTime)
        {
            m_SdOnTime -= Time.deltaTime;
            if (ShieldObj != null && ShieldObj.activeSelf == false)
                ShieldObj.SetActive(true);
        }
        else
        {
            if (ShieldObj != null && ShieldObj.activeSelf == true)
                ShieldObj.SetActive(false);
        }
        //----- 쉴드 상태 업데이트

        //----- 유도탄 상태 업데이트
        if(0.0f < m_Homing_OnTime)
        {
            m_Homing_OnTime -= Time.deltaTime;

            if (m_Homing_OnTime <= 0.0f)
                m_Homing_OnTime = 0.0f;

            IsHoming = true;
        }
        else
        {
            IsHoming = false;
        }
        //----- 유도탄 상태 업데이트

        //----- Double Shoot
        if(0.0f < m_Double_OnTime)
        {
            m_Double_OnTime -= Time.deltaTime;

            if(m_Double_OnTime <= 0.0f) 
                m_Double_OnTime = 0.0f;
        }
        //----- Double Shoot

        //----- Sub Hero 업데이트
        if(0.0f < m_Sub_OnTime)
        {
            m_Sub_OnTime -= Time.deltaTime;

            if (m_Sub_OnTime <= 0.0f)
                m_Sub_OnTime = 0.0f;
        }
        //----- Sub Hero 업데이트

    } //void Update_Skill()

    void FireUpdate()
    {
        if (0.0f < m_ShootCool)
            m_ShootCool -= Time.deltaTime;

        BulletCtrl a_BulletSc = null;

        //if (Input.GetKeyDown(KeyCode.Space) == true)
        if (m_ShootCool <= 0.0f)
        {
            if (0.0f < m_Double_OnTime) //더블샷
            {
                Vector3 a_Pos;
                GameObject a_CloneObj;
                for (int ii = 0; ii < 2; ii++)
                {
                    a_CloneObj = (GameObject)Instantiate(m_BulletObj);
                    a_Pos = m_ShootPos.transform.position;
                    a_Pos.y += 0.2f - (ii * 0.4f);
                    a_CloneObj.transform.position = a_Pos;
                    a_BulletSc = a_CloneObj.GetComponent<BulletCtrl>();
                    if (a_BulletSc != null)
                        a_BulletSc.IsHoming = IsHoming;
                } //for (int ii = 0; ii < 2; ii++)
            }
            else   //일반총알
            {
                GameObject a_CloneObj = (GameObject)Instantiate(m_BulletObj);
                a_CloneObj.transform.position = m_ShootPos.transform.position;
                a_BulletSc = a_CloneObj.GetComponent<BulletCtrl>();
                if (a_BulletSc != null)
                    a_BulletSc.IsHoming = IsHoming;
            } //일반총알

            m_ShootCool = 0.15f;  //공격속도 0.15f
        }

    } //void FireUpdate()

} //public class HeroCtrl
