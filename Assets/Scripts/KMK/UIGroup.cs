using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIGroup : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text maxScoreText;
    public TMP_Text coinText;
    public TMP_Text distanceText;

    private void Awake()
    {
        scoreText = transform.Find("Score").GetComponent<TMP_Text>();
        maxScoreText = transform.Find("MaxScore").GetComponent<TMP_Text>();
        coinText = transform.Find("Coin").GetComponent<TMP_Text>();
        distanceText = transform.Find("Distance").GetComponent<TMP_Text>();
    }
    private void Update()
    {
        distanceText.text = GameManager.Instance.distance.ToString() + "M";
    }
}
