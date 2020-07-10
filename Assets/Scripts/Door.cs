using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public string sceneName;
    public string markerName;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();

        if (player)
        {
            if (player.ShouldGoToDoor())
            {
                GameMng.instance.GotoScene(sceneName, markerName);
            }
        }
    }
}
