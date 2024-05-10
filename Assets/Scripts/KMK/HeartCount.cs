using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartCount : MonoBehaviour
{
    public GameObject[] hearts;
    private int heart_idx = 1;
    public void HeartChange()
    {
        if (heart_idx >= 0)
        {
            hearts[heart_idx].SetActive(false);
            heart_idx--;
        }

    }
}
