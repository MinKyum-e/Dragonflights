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
    public PolygonCollider2D enemycol;
    public GameObject coin;
    // Start is called before the first frame update
    void Start()
    {
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
           GameObject Coin = Instantiate(coin);
           Coin.transform.position = this.transform.position;
           Destroy(gameObject);
           //GameManager.Instance.score += 10;
        }
    }


    void OnTriggerEnter2D(Collider2D col) //뭔가 충돌 되었을 때 발생하는 함수
    {
       
        if (col.gameObject.name.Contains("bullet") == true)
        {
            Debug.Log("아야!");
            Hit(col.gameObject.GetComponent<bullet>().damage);
            
            Destroy(col.gameObject);
        }

    }




    public void Hit(int Damage)
    {
        curHP -= Damage;
    }
}
