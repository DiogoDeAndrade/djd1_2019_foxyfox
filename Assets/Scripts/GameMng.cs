using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMng : MonoBehaviour
{
    public static GameMng instance;

    Player currentPlayer;
    string targetMarker;
    bool   goingToNewScene = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
    }

    void Update()
    {
        if (currentPlayer == null)
        {
            currentPlayer = FindObjectOfType<Player>();
        }
    }

    public void GotoScene(string sceneName, string markerName)
    {
        if (goingToNewScene) return;

        goingToNewScene = true;

        targetMarker = markerName;

        SceneManager.sceneLoaded += OnSceneLoaded;

        SceneManager.LoadScene(sceneName);
    }

    void CleanupScene()
    {
        if (currentPlayer == null) return;

        Player[] players = FindObjectsOfType<Player>();

        foreach (var p in players)
        {
            if (p != currentPlayer)
            {
                Debug.Log("Destroying " + p.name);
                Destroy(p.gameObject);
            }
        }

        if (targetMarker != "")
        {
            GameMarker[] markers = FindObjectsOfType<GameMarker>();
            
            foreach (var m in markers)
            {
                if (m.name == targetMarker)
                {
                    currentPlayer.transform.position = m.transform.position;
                }
            }

            targetMarker = "";
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CleanupScene();

        SceneManager.sceneLoaded -= OnSceneLoaded;

        goingToNewScene = false;
    }
}
