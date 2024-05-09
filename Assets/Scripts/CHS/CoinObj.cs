using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObj : MonoBehaviour
{

    public int coinValue = 10;
    PolygonCollider2D col;
    // Start is called before the first frame update
    void Start()
    {
     col = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetCoin()
    {
        GameManager.Instance.coin += coinValue;
    }
}