using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGroup : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text maxScoreText;
    public TMP_Text coinText;

    private void Awake()
    {
        scoreText = transform.Find("Score").GetComponent<TMP_Text>();
        maxScoreText = transform.Find("MaxScore").GetComponent<TMP_Text>();
        coinText = transform.Find("Coin").GetComponent<TMP_Text>();
    }
}
