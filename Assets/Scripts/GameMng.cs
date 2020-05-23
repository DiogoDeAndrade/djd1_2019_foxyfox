using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMng : MonoBehaviour
{
    public static GameMng instance;

    public int highScore;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape)) && (Input.GetKey(KeyCode.LeftShift)))
        {
            Application.Quit();
        }
    }
}
