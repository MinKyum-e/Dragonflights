using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossAttState
{
    BS_SLEEP,
    BS_NORMAL_ACT,
    BS_FEVER_ACT,
}

public enum MonType //MonsterType
{
    MT_Zombi,
    MT_Missile,
    MT_Boss,
}

public class MonsterCtrl : MonoBehaviour
{
    public MonType m_MonType = MonType.MT_Zombi;

    float m_Speed = 4.0f;   //이동 속도
    Vector3 m_CurPos;       //위치 계산용 변수
    Vector3 m_SpawnPos;     //스폰 위치

    float m_CacPosY = 0.0f;  //사인 함수에 들어갈 누적 각도 계산용 변수
    float m_Rand_Y = 0.0f;   //랜덤한 진폭값 저장용 변수

    float m_MaxHP = 200.0f;  //최대 체력치
    float m_CurHP = 200.0f;  //현재 체력
    public Image m_HPSdBar = null;

    public GameObject m_CoinItem = null;
    HeroCtrl m_Hero = null;         //몬스터가 추적하게 될 몬스터 객체 변수(주인공 별칭)

    //------ 총알 발사 관련 변수 선언
    public GameObject m_ShootPos = null;
    public GameObject m_BulletObj = null;
    float shoot_Time  = 0.0f;    //총알 발사 주기 계산용 변수
    float shoot_Delay = 1.5f;    //총알 쿨 타임
    float BulletMvSpeed = 10.0f; //총알 속도
    //------ 총알 발사 관련 변수 선언

    Vector3 m_DirVec;            //이동 방향 계산용 변수

    //------ 보스의 공격 패턴 관련 변수
    BossAttState m_AcvState = BossAttState.BS_FEVER_ACT;    //피버 공격
    int m_ShootCount = 0;
    //------ 보스의 공격 패턴 관련 변수

    // Start is called before the first frame update
    void Start()
    {
        m_Hero = GameObject.FindObjectOfType<HeroCtrl>();

        m_SpawnPos = this.transform.position;
        m_Rand_Y = Random.Range(0.2f, 2.6f);  //Sin함수의 랜덤 진폭

        if(m_MonType == MonType.MT_Missile)
        {
            m_MaxHP *= 2.0f;    //미사일은 2.0배 에너지로...
            m_CurHP = m_MaxHP;
        }
        else if(m_MonType == MonType.MT_Boss)
        {
            m_MaxHP = 3000.0f;  //최대 체력치
            m_CurHP = m_MaxHP;  //현재 체력

            shoot_Time = 2.0f;
            m_AcvState = BossAttState.BS_FEVER_ACT; //피버 공격
        } //else if(m_MonType == MonType.MT_Boss)
    }

    // Update is called once per frame
    void Update()
    {
        if (m_MonType == MonType.MT_Zombi)
            Zombi_AI_Update();
        else if (m_MonType == MonType.MT_Missile)
            Missile_AI_Update();
        else if (m_MonType == MonType.MT_Boss)
            Boss_AI_Update();

        if (transform.position.x < CameraResolutaion.m_SceenWMin.x - 2.0f)
            Destroy(gameObject);  //왼쪽 화면을 벗어나는 즉시 제거
    }

    void Zombi_AI_Update()
    {
        m_CurPos = transform.position;
        m_CurPos.x += (-1.0f * Time.deltaTime * m_Speed);
        m_CacPosY += Time.deltaTime * (m_Speed / 2.2f);
        m_CurPos.y = m_SpawnPos.y + Mathf.Sin(m_CacPosY) * m_Rand_Y;
        transform.position = m_CurPos;

        //------ 총알 발사 
        if (m_BulletObj == null)
            return;

        shoot_Time += Time.deltaTime;
        if(shoot_Delay <= shoot_Time)
        {
            GameObject a_NewObj = (GameObject)Instantiate(m_BulletObj);
            BulletCtrl a_BulletSc = a_NewObj.GetComponent<BulletCtrl>();
            a_BulletSc.BulletSpawn(m_ShootPos.transform.position,
                                    Vector3.left, BulletMvSpeed);

            shoot_Time = 0.0f;
        }
        //------ 총알 발사 
    }

    void Missile_AI_Update()
    {
        m_CurPos = transform.position;
        Vector3 a_CacVec = m_Hero.transform.position - transform.position;

        //m_DirVec = Vector3.left;
        m_DirVec = a_CacVec;    //몬스터의 이동 방향 벡터

        if (a_CacVec.x < -3.5f) //미사일이 주인공과의 거리가 우측방향으로 3.5m 이상이면... 
            m_DirVec.y = 0.0f;

        m_DirVec.Normalize();
        m_DirVec.x = -1.0f;
        m_DirVec.z = 0.0f;

        m_CurPos = m_CurPos + (m_DirVec * Time.deltaTime * m_Speed);
        transform.position = m_CurPos;
    }

    void Boss_AI_Update()
    {
        m_CurPos = this.transform.position;
        if (7.0f < m_CurPos.x)
            m_CurPos.x = m_CurPos.x + (-1.0f * Time.deltaTime * m_Speed);

        this.transform.position = m_CurPos;

        //--- 공격 패턴 만들기
        if (m_BulletObj == null)
            return;

        if(m_AcvState == BossAttState.BS_NORMAL_ACT) //기본 공격
        {
            shoot_Time -= Time.deltaTime;
            if(shoot_Time <= 0.0f)
            {
                Vector3 a_TargetV =
                        m_Hero.transform.position - this.transform.position;
                a_TargetV.Normalize();
                GameObject a_NewObj = (GameObject)Instantiate(m_BulletObj);
                //오브젝트의 클론(복사체) 생성 함수
                BulletCtrl a_BulletSc = a_NewObj.GetComponent<BulletCtrl>();
                a_BulletSc.BulletSpawn(m_ShootPos.transform.position, a_TargetV, BulletMvSpeed);

                float angle = Mathf.Atan2(a_TargetV.y, a_TargetV.x) * Mathf.Rad2Deg;
                angle += 180.0f;
                a_NewObj.transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);

                m_ShootCount++;
                if (m_ShootCount < 7)   //일반공격 7번까지의 공격 주기
                    shoot_Time = 0.7f;
                else //궁극기로 넘어갈 때 2.0초 딜레이 후에
                {
                    m_ShootCount = 0;
                    shoot_Time = 2.0f;
                    m_AcvState = BossAttState.BS_FEVER_ACT;
                }

            }// if(shoot_Time <= 0.0f)

        } //if(m_AcvState == BossAttState.BS_NORMAL_ACT) //기본 공격
        else if(m_AcvState == BossAttState.BS_FEVER_ACT) //피버 공격
        {
            shoot_Time -= Time.deltaTime;
            if(shoot_Time <= 0.0f)
            {
                float Radius = 100.0f;
                Vector3 a_TargetV = Vector3.zero;
                GameObject a_NewObj = null;
                BulletCtrl a_BulletSc = null;
                for (float Angle = 0.0f; Angle < 360.0f; Angle += 15.0f)
                {
                    a_TargetV.x = Radius * Mathf.Cos(Angle * Mathf.Deg2Rad);
                    a_TargetV.y = Radius * Mathf.Sin(Angle * Mathf.Deg2Rad);
                    a_TargetV.Normalize();
                    a_NewObj = (GameObject)Instantiate(m_BulletObj);
                    a_BulletSc = a_NewObj.GetComponent<BulletCtrl>();
                    a_BulletSc.BulletSpawn(this.transform.position, a_TargetV, BulletMvSpeed);
                    float angle = Mathf.Atan2(a_TargetV.y, a_TargetV.x) * Mathf.Rad2Deg;
                    angle += 180.0f;
                    a_NewObj.transform.eulerAngles = new Vector3(0.0f, 0.0f, angle);

                }//for (float Angle = 0.0f; Angle < 360.0f; Angle += 15.0f)

                m_ShootCount++;
                if (m_ShootCount < 3)   //궁극기 3번까지의 공격 주기
                    shoot_Time = 1.0f;
                else   //궁극기에서 일반공격으로 넘어갈 때 1.5초 딜레이 후에
                {
                    m_ShootCount = 0;
                    shoot_Time = 1.5f;
                    m_AcvState = BossAttState.BS_NORMAL_ACT;
                }

            } //if(shoot_Time <= 0.0f)
        } //else if(m_AcvState == BossAttState.BS_FEVER_ACT) //피버 공격

    } //void Boss_AI_Update()

    void OnTriggerEnter2D(Collider2D col)
    {   //몬스터에 뭔가 충돌 되었을 때 발생되는 함수
        if (col.tag == "AllyBullet")
        {
            TakeDamage(80.0f);
            Destroy(col.gameObject); //몬스터에 충돌된 총알 삭제 코드
        }
    }

    public void TakeDamage(float a_Value)
    {
        if (m_CurHP <= 0.0f) //이렇게 하면 사망 처리는 한번만 될 것이다.
            return;

        Vector3 a_CacPos = transform.position;

        float a_CacDmg = a_Value;
        if (m_CurHP <= a_Value)
            a_CacDmg = m_CurHP;

       Game_Mgr.Inst.DamageTxt(-a_CacDmg, a_CacPos, Color.red);

        m_CurHP = m_CurHP - a_Value;
        if (m_CurHP < 0.0f)
            m_CurHP = 0.0f;

        if (m_HPSdBar != null)
            m_HPSdBar.fillAmount = m_CurHP / m_MaxHP;

        if (m_CurHP <= 0.0f) //몬스터 사망 처리
        {
            //-- 보상
            if (m_MonType == MonType.MT_Boss)
                Game_Mgr.Inst.AddScore(200);
            else
                Game_Mgr.Inst.AddScore(10);

            Game_Mgr.Inst.AddExpLevel();

            //---골드보상
            int dice = Random.Range(0, 10); //0 ~ 9 랜덤값 발생
            if(m_MonType == MonType.MT_Boss)
            {
                dice = 0;   //보스일 때 드롭 확율 100%
            }

            if(dice < 3)  //30% 확률
            if (m_CoinItem != null)
            {
                GameObject a_CoinObj = (GameObject)Instantiate(m_CoinItem);
                a_CoinObj.transform.position = transform.position;
                CoinCtrl a_CoinCtrl = a_CoinObj.GetComponent<CoinCtrl>();
                if (a_CoinCtrl != null)
                    a_CoinCtrl.m_RefHero = m_Hero;
            }
            //---골드보상
            //-- 보상

            if(m_MonType == MonType.MT_Boss)
            {
                MonsterGenerator.m_SpBossTimer = Random.Range(25.0f, 30.0f);
                //다음번 스폰 주기 설정
            }//if(m_MonType == MonType.MT_Boss)

            Destroy(gameObject); //<--몬스터 GameObject 제거됨
        }//if(m_CurHP <= 0.0f)
    } //public void TakeDamage(float a_Value)
}
