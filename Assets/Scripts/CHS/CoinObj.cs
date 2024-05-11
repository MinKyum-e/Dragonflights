using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CoinObj : MonoBehaviour
{


    public int coinValue = 10;
    float moveSpeed = 1.5f;
    PolygonCollider2D col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        speedUp();
        float distanceY = moveSpeed * Time.deltaTime;
        transform.Translate(0, -distanceY, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       /* if (collision.gameObject.name.Contains("coin") == true)
        {
            Destroy(col.gameObject);
        }*/
    }

    private void speedUp()
    {
        moveSpeed = Mathf.Min(moveSpeed + Time.deltaTime * 3f, 10f);
    }

    public void GetCoin()
    {
        GameManager.Instance.coin += coinValue;
    }
}
