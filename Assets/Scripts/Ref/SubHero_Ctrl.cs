﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubHero_Ctrl : MonoBehaviour
{
    HeroCtrl m_RefHero = null;

    float angle = 0.0f;
    float radius = 1.0f;
    float speed = 100.0f;

    GameObject parent_Obj = null;
    Vector3 parent_Pos = Vector3.zero;

    float m_LifeTime = 0.0f;

    //----- 공격 관련 변수
    public GameObject m_BulletObj = null;
    float m_AttSpeed = 0.5f;   //공격 속도 (공속)
    float m_ShootCool = 0.0f;  //총알 발사 주기 계산용 변수

    GameObject a_CloneObj = null;
    BulletCtrl a_BulletSc = null;

    bool IsHoming = false;
    bool IsDouble = false;
    //----- 공격 관련 변수

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(m_RefHero != null)
        {
            IsHoming = m_RefHero.IsHoming;
            if (0.0f < m_RefHero.m_Double_OnTime)
                IsDouble = true;
            else //if(m_RefHero.m_Double_OnTime <= 0)
                IsDouble = false;
        }//if(m_RefHero != null)

        angle += Time.deltaTime * speed;
        if (360.0f < angle)
            angle -= 360.0f;

        parent_Pos = parent_Obj.transform.position;

        transform.position = parent_Pos +
                new Vector3(radius * Mathf.Cos(angle * Mathf.Deg2Rad),
                            radius * Mathf.Sin(angle * Mathf.Deg2Rad));

        FireUpdate();

        m_LifeTime -= Time.deltaTime;
        if (m_LifeTime <= 0.0f)
            Destroy(this.gameObject);

    } //void Update()

    public void SubHeroSpawn(GameObject a_Paren, float a_Angle, float a_LifeTime)
    {
        parent_Obj = a_Paren;
        m_RefHero  = a_Paren.GetComponent<HeroCtrl>();
        angle      = a_Angle;
        m_LifeTime = a_LifeTime;
    }

    void FireUpdate()
    {
        if (0.0f < m_ShootCool)
            m_ShootCool -= Time.deltaTime;

        if (m_ShootCool <= 0.0f)
        {
            if (IsDouble == true) //더블샷
            {
                Vector3 a_Pos;
                for (int ii = 0; ii < 2; ii++)
                {
                    a_CloneObj = (GameObject)Instantiate(m_BulletObj);
                    a_Pos = transform.position;
                    a_Pos.y += 0.2f - (ii * 0.4f);
                    a_CloneObj.transform.position = a_Pos;
                    a_BulletSc = a_CloneObj.GetComponent<BulletCtrl>();
                    if (a_BulletSc != null)
                        a_BulletSc.IsHoming = IsHoming;
                } //for (int ii = 0; ii < 2; ii++)
            }
            else   //일반총알
            {
                a_CloneObj = (GameObject)Instantiate(m_BulletObj);
                a_CloneObj.transform.position = transform.position;
                a_BulletSc = a_CloneObj.GetComponent<BulletCtrl>();
                if (a_BulletSc != null)
                    a_BulletSc.IsHoming = IsHoming;
            } //일반총알

            m_ShootCool = m_AttSpeed;  //공격속도 0.15f
        }//if (m_ShootCool <= 0.0f)

    } //void FireUpdate()

} //public class SubHero_Ctrl : MonoBehaviour
