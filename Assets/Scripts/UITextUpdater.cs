using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public enum Value_Type
{
    COIN,
    MAX_SCORE,
    SCORE
};

public class UITextUpdater : MonoBehaviour
{
    TMP_Text txt;
    public Value_Type type;
    private void Awake()
    {
        txt = GetComponent<TMP_Text>();
    }
    private void UITextUpdate()
    {
        switch (type)
        {
            case Value_Type.COIN:
                txt.text = GameManager.Instance.coin.ToString();
                break;
            case Value_Type.SCORE:
                txt.text = GameManager.Instance.score.ToString();
                break;
            case Value_Type.MAX_SCORE:
                txt.text = GameManager.Instance.max_score.ToString();
                break;
        }

    }

}
