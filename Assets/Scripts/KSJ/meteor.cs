using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meteor : MonoBehaviour
{
    float speed = 5.0f;
    float cooltime = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 temp = gameObject.transform.position;
        temp.x = Random.Range(-2.5f, 2.5f);
        gameObject.transform.position = temp;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = gameObject.transform.position;
        temp.y -= speed * Time.deltaTime;
        gameObject.transform.position = temp;

        cooltimeUpdate();
    }

    void cooltimeUpdate() 
    {
        cooltime -= Time.deltaTime;
        if (cooltime < 0f) {
            gameObject.transform.position = new Vector3 (Random.Range(-2.5f,2.5f), 6, 0);
            cooltime = 4.0f;
        }
    }
}
