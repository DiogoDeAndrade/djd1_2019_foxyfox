using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene1 : MonoBehaviour
{
    Coroutine cutsceneCR;

    void Start()
    {
        cutsceneCR = StartCoroutine(RunCutsceneCR());        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopCoroutine(cutsceneCR);
            EndCutscene();
        }
    }

    IEnumerator RunCutsceneCR()
    {
        yield return new WaitForSeconds(0.1f);

        Player player = FindObjectOfType<Player>();
        player.enableInput = false;
        player.ResetMovement();

        yield return new WaitForSeconds(0.25f);

        player.SetHAxis(1.0f);

        yield return new WaitForSeconds(0.5f);

        player.SetHAxis(-1.0f);

        yield return new WaitForSeconds(1.0f);

        player.SetHAxis(1.0f);

        yield return new WaitForSeconds(0.1f);

        player.SetHAxis(0.0f);

        float startTime = Time.time;
        while ((Time.time - startTime) < 2.0f)
        {
            if (Input.GetButtonDown("Jump")) break;

            yield return null;
        }

        player.SetJump(true);

        yield return new WaitForSeconds(0.5f);

        EndCutscene();
    }

    /*    IEnumerator RunCutsceneCR()
        {
            yield return new WaitForSecondsOrKeypress(0.1f, KeyCode.Escape);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndCutscene();
                yield break;
            }

            Player player = FindObjectOfType<Player>();
            player.enableInput = false;
            player.ResetMovement();

            yield return new WaitForSecondsOrKeypress(0.25f, KeyCode.Escape);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndCutscene();
                yield break;
            }

            player.SetHAxis(1.0f);

            yield return new WaitForSecondsOrKeypress(0.5f, KeyCode.Escape);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndCutscene();
                yield break;
            }

            player.SetHAxis(-1.0f);

            yield return new WaitForSecondsOrKeypress(1.0f, KeyCode.Escape);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndCutscene();
                yield break;
            }

            player.SetHAxis(1.0f);

            yield return new WaitForSecondsOrKeypress(0.1f, KeyCode.Escape);

            player.SetHAxis(0.0f);

            yield return new WaitForSecondsOrKeypress(2.0f, KeyCode.Escape);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndCutscene();
                yield break;
            }

            player.SetJump(true);

            yield return new WaitForSecondsOrKeypress(0.5f, KeyCode.Escape);

            EndCutscene();
        }*/

    void EndCutscene()
    {
        Player player = FindObjectOfType<Player>();

        player.enableInput = true;

        player.transform.position = GetComponent<LevelManager>().playerRespawnPosition.position;
    }
}
