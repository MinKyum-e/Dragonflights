using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Result : MonoBehaviour
{
    public TMP_Text distance;
    public TMP_Text gold;
    public TMP_Text score;
    public TMP_Text mygold;

    void Start()
    {
        distance.text = PlayerPrefs.GetInt("Distance", 0).ToString() + "M";
        gold.text = PlayerPrefs.GetInt("CurGold", 0).ToString();
        mygold.text = PlayerPrefs.GetInt("Gold", 0).ToString();
        score.text = PlayerPrefs.GetInt("Score", 0).ToString();
    }


}
