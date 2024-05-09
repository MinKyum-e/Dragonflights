using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCtrl: MonoBehaviour
{
    public float moveSpeed = 1.3f;
    public int HP = 10;
    public int curHP;
    public int Atk = 2;
    public int curAtk;
    public GameObject Enemy;
    public PolygonCollider2D enemycol;

    // Start is called before the first frame update
    void Start()
    {
        Enemy = GetComponent<GameObject>();
        enemycol = GetComponent<PolygonCollider2D>();
        curHP = HP;
        curAtk = Atk;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceY = moveSpeed * Time.deltaTime;
        transform.Translate(0, -distanceY, 0);

        if(curHP <= 0)
        {
            Destroy(Enemy);
        }
    }


    void OnTriggerEnter2D(Collider2D col) //뭔가 충돌 되었을 때 발생하는 함수
    {
       
        if (col.gameObject.name.Contains("Bullet") == true)
        {
            Hit(curAtk);
            Destroy(col.gameObject);
        }

    }




    public void Hit(int Damage)
    {
        curHP -= Damage;
    }
}
