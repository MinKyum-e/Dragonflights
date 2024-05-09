using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class player : MonoBehaviour
{
    int hp = 2;

    float mousePositionX = 0;
    float speed = 300;

    public GameObject m_BulletObj;
    float m_ShootCool = 0.12f;

    // Start is called before the first frame update
    void Start()
    {
        Camera Camera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        }
        if (Input.GetMouseButton(0)) {
            Vector3 temp = gameObject.transform.position;
            temp.x += speed * Time.deltaTime * (Camera.main.ScreenToWorldPoint(Input.mousePosition).x - mousePositionX);
            if (temp.x < 2.66 && temp.x > -2.66)
            {
                gameObject.transform.position = temp;
            }
            mousePositionX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        }

        FireUpdate();

    }

    void FireUpdate() 
    {
        if (m_ShootCool > 0.0f) {
           m_ShootCool -= Time.deltaTime;
        }

        if (m_ShootCool <= 0.0f) {
            GameObject a_CloneObj;
            a_CloneObj = (GameObject)Instantiate(m_BulletObj);
            a_CloneObj.transform.position = gameObject.transform.position + new Vector3 (0,0.3f,0);
            m_ShootCool += 0.12f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("meteor") == true)
        {
            hp -= 1;
            Debug.Log(hp);
        }
        else if (collision.gameObject.name.Contains("coin") == true)
        {
            Debug.Log("ÄÚÀÎÈ¹µæ");
            //GameManager.Instance.coin += ¾ó¸¶
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.name.Contains("monster") == true)
        {
            hp -= 1;
            Debug.Log(hp);
        }
    }
}
