using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObj : MonoBehaviour
{

    public int coinValue = 10;
    int moveSpeed = 1;
    PolygonCollider2D col;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
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

    public void GetCoin()
    {
        GameManager.Instance.coin += coinValue;
    }
}
