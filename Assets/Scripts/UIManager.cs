using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public enum HealthDisplayType { Text, Discrete, Meter };

    public HealthDisplayType    healthDisplayType;
    public Player               playerRef;
    public HP                   playerHPRef;
    public TextMeshProUGUI      playerHPTextRef;
    public TextMeshProUGUI      playerScoreTextRef;
    public Image[]              hearts;
    public Image                heartFill;

    void Start()
    {
    }

    void Update()
    {
        if ((playerRef == null) || (playerHPRef == null))
        {
            return;
        }

        if (healthDisplayType == HealthDisplayType.Text)
        {
            playerHPTextRef.text = "x" + playerHPRef.hp;
        }
        else if (healthDisplayType == HealthDisplayType.Discrete)
        {
            foreach (Image i in hearts)
            {
                i.enabled = false;
            }
            for (int i  = 0; i < playerHPRef.hp; i++)
            {
                hearts[i].enabled = true;
            }
        }
        else if (healthDisplayType == HealthDisplayType.Meter)
        {
            heartFill.fillAmount = playerHPRef.hp / 3.0f;
        }

        playerScoreTextRef.text = "Score: " + playerRef.GetScore();
    }
}
