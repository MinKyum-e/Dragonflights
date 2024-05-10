using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class bullet : MonoBehaviour
{
    float speed = 10f;
    public int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 1.3f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = gameObject.transform.position;
        temp.y += speed * Time.deltaTime;
        gameObject.transform.position = temp;
    }

}
