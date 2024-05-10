using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class Heart : MonoBehaviour
{
    int maxheart = 5;
    int heart;

    public GameObject heartUI;

    // Start is called before the first frame update
    void Start()
    {
        heart = maxheart;
        float starting_x = (maxheart-1) * -0.25f;
        for (int i = 0; i < maxheart; i++)
        {
            Instantiate(heartUI, new Vector3(starting_x, -4.5f, 0), Quaternion.identity, GameObject.Find("HeartGroup").transform);
            starting_x += 0.5f;
        }
    }
    private void Update()
    {
        if(heart == 0)
        {
            GameManager.Instance.gameState = GameState.GAME_OVER;
        }
    }

    public void HeartBreak() 
    {
        Destroy(transform.GetChild(heart - 1).gameObject);
        heart--;
        if (heart <= 0) {
            //대충 게임오버
        }
    }
}
