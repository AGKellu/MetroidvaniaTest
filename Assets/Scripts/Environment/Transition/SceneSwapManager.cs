using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class SceneSwapManager : MonoBehaviour
{
    public static SceneSwapManager instance;
    private TransitionScript.DoorToSpawnAt _doorToSpawnTo;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static void SwapSceneFromDoorUse(SceneField myScene, TransitionScript.DoorToSpawnAt doorToSpawnAt)
    {
        instance.StartCoroutine(instance.FadeOutThenChangeScene(myScene, doorToSpawnAt));
    }

    private IEnumerator FadeOutThenChangeScene(SceneField myScene, TransitionScript.DoorToSpawnAt doorToSpawnAt = TransitionScript.DoorToSpawnAt.None)
    {
        SceneFadeManager.instance.StartFadeOut();
        DisablePlayer();
        while (SceneFadeManager.instance.isFadingOut)
        {
            yield return null;
        }
        //yield return null;
        _doorToSpawnTo = doorToSpawnAt;
        SceneManager.LoadScene(myScene);
        SceneFadeManager.instance.StartFadeIn();
        while (SceneFadeManager.instance.isFadingIn)
        {
            yield return null;
        }
        EnablePlayer();
    }

    private void EnablePlayer()
    {
        //GameObject Player = GameObject.FindGameObjectWithTag("Player");
        //Player.GetComponent<PlayerMovement>().enabled = true;
        InputSystem.actions.FindActionMap("Move").Enable();
        InputSystem.actions.FindActionMap("Attacks").Enable();
    }
    private void DisablePlayer()
    {
        //GameObject Player = GameObject.FindGameObjectWithTag("Player");
        //Player.GetComponent<PlayerMovement>().enabled = false;
        //Debug.Log(playerInput.currentActionMap);
        InputSystem.actions.FindActionMap("Move").Disable();
        InputSystem.actions.FindActionMap("Attacks").Disable();
    }
    
}
