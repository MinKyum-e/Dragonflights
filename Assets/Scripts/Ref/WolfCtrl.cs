using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfCtrl : MonoBehaviour
{
    float m_MoveSpeed = 7.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * Time.deltaTime * m_MoveSpeed;

        if(CameraResolutaion.m_SceenWMax.x + 0.5f < transform.position.x)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col) //뭔가 충돌 되었을 때 발생하는 함수
    {
        if(col.tag == "Monster")
        {
            MonsterCtrl a_Enemy = col.gameObject.GetComponent<MonsterCtrl>();
            if (a_Enemy != null)
                a_Enemy.TakeDamage(700);
        }
        else if(col.tag == "EnemyBullet")
        {
            Destroy(col.gameObject);
        }

    }//void OnTriggerEnter2D(Collider2D col)
}
