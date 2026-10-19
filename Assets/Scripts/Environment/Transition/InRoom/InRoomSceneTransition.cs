using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class InRoomSceneTransition : MonoBehaviour
{
   public static InRoomSceneTransition instance;
   private TransitionScript.DoorToSpawnAt _doorToSpawnTo;

   [Header("Spawn To In Room")]
   [SerializeField] private SceneField[] scenesToLoad;
   [SerializeField] private SceneField[] scenesToUnload;
   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
      }
   }
   //Once the player bumps into the trigger, load the next part of the room 
   //in the update function, check if player is for enough away to unload the last part of the room
   //either use an index or make a Scene array for every multi part room, load the next scene and do array stuff 
   //put the trigger far back in room 1! the player should never see loading
   //Remember to use loadsceneasync
   /*
   Example: Room 1 is loaded
   Player hits trigger to load room 2 
   room 1 and room 2 is loaded 
   Player hits trigger for room 3
   Room 2 and 3 is loaded
   Room 1 is unloaded
   Player goes back to trigger in room 2
   Room 2 and 1 is loaded
   Room 3 is unloaded 
   etc 
   */
   //public static void SwapSceneFromDoorUse(SceneField myScene, TransitionScript.DoorToSpawnAt doorToSpawnAt)
   //{
   // instance.StartCoroutine(instance.FadeOutThenChangeScene(myScene, doorToSpawnAt));
   //}
   public void SwapSceneFromDoorUse()
   {
      //DisablePlayer();
      LoadScenes();
      UnloadScenes();
      //EnablePlayer();
     //Debug.Log("InRoomTrans");
   }
   //private IEnumerator FadeOutThenChangeScene(SceneField myScene, TransitionScript.DoorToSpawnAt doorToSpawnAt = TransitionScript.DoorToSpawnAt.None)
   //{
      //DisablePlayer();
      /*Scene currentScene = SceneManager.GetActiveScene();
      SceneFadeManager.instance.StartFadeOut();
      while (SceneFadeManager.instance.isFadingOut)
      {
         yield return null;
      }
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
      GameObject BrokenDoor = null;
      foreach (GameObject Door in Doors)
      {
         if (Door.scene.name == myScene.SceneName)
         {
            //check all doors to see if breakable
            //check saveload instance to check breakable[breakabledoorint of door]
            //if saveload breakabledoor[breakabledoorint of door] = true, set door to false
            if (Door.transform.GetChild(0).GetComponent<TransitionScript>().CurrentDoorPosition == doorToSpawnAt)
            {
               SpawnDoor = Door;
            }
            for (int i = 0; i < SaveLoad.instance.BreakableDoors.Length; i++)
            {
               if (i == Door.GetComponent<DoorScript>().BreakableDoorInt && Door.GetComponent<DoorScript>().BreakableDoor)
               {
                  if (SaveLoad.instance.BreakableDoors[i] == true)
                  {

                     BrokenDoor = Door;
                  }
               }
            }
         }
      }
      for (int i = 0; i < Spawners.Length; i++)
      {
         if (Spawners[i].scene.name == myScene.SceneName)
         {
            Debug.Log(Spawners[i]);
            if (BrokenDoor != null)
            {
               Spawners[i].GetComponent<SpawnerScript>().TurnOffDoor(BrokenDoor);

            }
            Spawners[i].GetComponent<SpawnerScript>().PlayerCheck(SpawnDoor, false);
     
         }
      }

      SceneFadeManager.instance.StartFadeIn();
      while (SceneFadeManager.instance.isFadingIn)
      {
         yield return null;
      }

      SceneManager.UnloadSceneAsync(currentScene);*/
     // DisablePlayer();
     // LoadScenes();
     // UnloadScenes();
     // EnablePlayer();
   //}

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
   private void LoadScenes()
   {
      for (int i = 0; i < scenesToLoad.Length; i++)
      {
         bool isSceneLoaded = false;
         for (int j = 0; j < SceneManager.sceneCount; j++)
         {
            Scene loadedScene = SceneManager.GetSceneAt(j);
            if (loadedScene.name == scenesToLoad[i].SceneName)
            {
               isSceneLoaded = true;
               break;
            }
         }
         if (!isSceneLoaded)
         {
            //SceneManager.MoveGameObjectToScene(CameraManager.instance.gameObject, SceneManager.GetSceneByName(scenesToLoad[i].SceneName));
            SceneManager.LoadSceneAsync(scenesToLoad[i], LoadSceneMode.Additive);
         }
      }
   }
   private void UnloadScenes()
   {
      for (int i = 0; i < scenesToUnload.Length; i++)
      {
         for (int j = 0; j < SceneManager.sceneCount; j++)
         {
            Scene loadedScene = SceneManager.GetSceneAt(j);
            if (loadedScene.name == scenesToUnload[i].SceneName)
            {
               
               SceneManager.UnloadSceneAsync(scenesToUnload[i]);
            }
         }
      }
   }
   void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.CompareTag("Player"))
      {
         SwapSceneFromDoorUse();
      }
   }

}
