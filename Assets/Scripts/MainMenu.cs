using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI highScoreTextRef;

    private void Start()
    {
        GameMng mng = FindObjectOfType<GameMng>();

        highScoreTextRef.text = "Highscore: " + mng.highScore;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
