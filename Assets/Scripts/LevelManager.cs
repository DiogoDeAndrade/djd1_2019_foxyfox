using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Player             playerPrefab;
    [SerializeField] Transform          playerRespawnPosition;
    [SerializeField] CameraController   cameraControllerRef;

    Player player;

    private void Start()
    {
        SpawnPlayer();
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCR(1.0f));
    }

    IEnumerator RespawnCR(float respawnTime)
    {
        yield return new WaitForSeconds(respawnTime);

        SpawnPlayer();
    }
    
    void SpawnPlayer()
    { 
        player = Instantiate(playerPrefab, playerRespawnPosition.position, playerRespawnPosition.rotation);

        cameraControllerRef.SetFollowTarget(player.transform);

        HP playerHP = player.GetComponent<HP>();

        UIManager uiManager = FindObjectOfType<UIManager>();
        uiManager.playerRef = player;
        uiManager.playerHPRef = playerHP;

        playerHP.onDead += BackToMainMenu;
    }

    void BackToMainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
