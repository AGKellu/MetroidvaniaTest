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
        DisablePlayer();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneFadeManager.instance.StartFadeOut();
        
        while (SceneFadeManager.instance.isFadingOut)
        {
            yield return null;
        }
        //yield return null;
        _doorToSpawnTo = doorToSpawnAt;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(myScene.SceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        GameObject Player = GameObject.FindWithTag("Player");
        SceneManager.MoveGameObjectToScene(Player, SceneManager.GetSceneByName(myScene.SceneName));
        GameObject[] Spawners = GameObject.FindGameObjectsWithTag("Spawner");
        GameObject[] Doors = GameObject.FindGameObjectsWithTag("Door");
        GameObject SpawnDoor = null;
        foreach (GameObject Door in Doors)
        {
            if (Door.scene.name == myScene.SceneName)
            {
                if (Door.transform.GetChild(0).GetComponent<TransitionScript>().CurrentDoorPosition == doorToSpawnAt)
                {
                    Debug.Log(Door.name);
                    SpawnDoor = Door;
                    //Debug.Log(Door.name);
                }
                //Debug.Log(Door.name);
            }
        }
        
        for (int i = 0; i < Spawners.Length; i++)
        {
            //Debug.Log(Spawners[i].scene.name);
            if (Spawners[i].scene.name == myScene.SceneName)
            {
                Debug.Log(Spawners[i]);
                Spawners[i].GetComponent<SpawnerScript>().PlayerCheck(SpawnDoor, false);
            }
        }
        
        //SceneManager.LoadScene(myScene);
       // PlayerMovement.instance.Grounded = true;
        //PlayerMovement.instance.canWalk = true;
        //PlayerMovement.instance.gameObject.transform.position = ;
        //TransitionScript[] Scripts = GameObject.FindGameObjects
        /*GameObject[] Doors = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject Door in Doors)
        {
            Debug.Log(Door.name);
        }*/
        //no need to find the doors nowposition, since its weird and i hate it (the bug was created due to there being a ledge, that stopped gravity scale)
        SceneFadeManager.instance.StartFadeIn();
        while (SceneFadeManager.instance.isFadingIn)
        {
            yield return null;
        }
        SceneManager.UnloadSceneAsync(currentScene);
        EnablePlayer();
    }

    private void EnablePlayer()
    {
        //GameObject Player = GameObject.FindGameObjectWithTag("Player");
        //Player.GetComponent<PlayerMovement>().enabled = true;
        InputSystem.actions.FindActionMap("Move").Enable();
        PlayerMovement.instance.ableToMove = true;
        PlayerMovement.instance.MovingRight = false;
        PlayerMovement.instance.MovingLeft = false;
        PlayerMovement.instance.gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector3(0, 0, 0);
        PlayerMovement.instance.gameObject.GetComponent<Animator>().SetBool("Running", false);
        PlayerMovement.instance.gameObject.GetComponent<Animator>().SetBool("Idle", true);
        InputSystem.actions.FindActionMap("Attacks").Enable();
    }
    private void DisablePlayer()
    {
        //GameObject Player = GameObject.FindGameObjectWithTag("Player");
        //Player.GetComponent<PlayerMovement>().enabled = false;
        //Debug.Log(playerInput.currentActionMap);
        InputSystem.actions.FindActionMap("Move").Disable();
        PlayerMovement.instance.ableToMove = false;
        InputSystem.actions.FindActionMap("Attacks").Disable();
    }
    
}
