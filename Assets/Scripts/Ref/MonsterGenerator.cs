﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour
{
    public GameObject[] MonPrefab;

    float m_SpDelta = 0.0f;    //스폰 주기 계산용 변수
    float m_DiffSpawn = 1.0f;  //난이도에 따른 몬스터 스폰주기 변수

    [HideInInspector] public static float m_SpBossTimer = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_SpBossTimer = 20.0f;
    }

    // Update is called once per frame
    void Update()
    {
        m_SpDelta -= Time.deltaTime;
        if(m_SpDelta < 0.0f)
        {
            GameObject go = null;
            int dice = Random.Range(1, 11);     // 1 ~ 10 랜덤값 발생
            if (2 < dice)
            {
                go = Instantiate(MonPrefab[0]) as GameObject; //좀비 스폰
            }
            else
            {
                go = Instantiate(MonPrefab[1]) as GameObject; //미사일 스폰
            }

            float py = Random.Range(-3.0f, 3.0f);
            go.transform.position =
                new Vector3(CameraResolutaion.m_SceenWMax.x + 1.0f, py, 0);

            m_SpDelta = m_DiffSpawn;
        }

        if(0.0f < m_SpBossTimer)
        {
            m_SpBossTimer -= Time.deltaTime;
            if(m_SpBossTimer <= 0.0f)
            {
                GameObject go = Instantiate(MonPrefab[2]) as GameObject;
                float py = Random.Range(-3.0f, 3.0f); //-3.0f  ~ 3.0f 랜덤값 발생
                go.transform.position =
                            new Vector3(CameraResolutaion.m_SceenWMax.x + 1.0f, py, 0.0f);
            }
        } //if(0.0f < m_SpBossTimer)

    } //void Update()
}
