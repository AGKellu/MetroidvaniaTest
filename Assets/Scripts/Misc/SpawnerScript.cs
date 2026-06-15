using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using System.Diagnostics;
public class SpawnerScript : MonoBehaviour
{
    public GameObject[] GOsInScene;
    public Vector3[] GOPositions;
    public PlayerSOScript Vals;
    public GameObject LeftDoor;
    public GameObject LeftTrans;
    public GameObject RightDoor;
    public GameObject RightTrans;
    //public GameObject Camera;
    //public GameObject Player;
    //public GameObject EyeBat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        //Spawn();
        //SpawnPlayerOneTime();
        //if (GameObject.FindGameObjectWithTag("Player"))
        //{
        //   Debug.Log("There is another pplayer jere\nYOu should never see this!");
        // }
        //  Spawn();
        //PlayerCheck();
       // SceneManager.activeSceneChanged += ChangePlayer;
    }
   // void OnEnable()
   // {
      //  SceneManager.sceneLoaded += ChangePlayer;
    //}
    //void OnDisable()
    //{
     //   SceneManager.sceneLoaded -= ChangePlayer;
   // }
   // private void ChangePlayer(Scene current, LoadSceneMode mode)
   // {
      //  Debug.Log("Send Player to " + current.name);
        
        //Debug.Log(current.name + " has changed to " + newScene.name);
    //}
    public void PlayerCheck(GameObject Player)
    {
        //GameObject Player = GameObject.FindGameObjectWithTag("Player");
        //GameObject Camera = GameObject.FindGameObjectWithTag("MainCamera");
        //Player.GetComponent<PlayerAttack>().Camera = Camera;
        //SceneManager.MoveGameObjectToScene(Player, gameObject.scene);
        //Debug.Log("Please exist");
        //Debug.Log(Player.GetComponent<PlayerAttack>().Camera);
        //Debug.Log(Player.GetComponent<PlayerMovement>().Camera);
        //Player.GetComponent<PlayerMovement>().Camera = null;
        //Player.GetComponent<PlayerMovement>().Camera = Camera;
        //Player.GetComponent<PlayerAttack>().Camera = Camera;
        if (Player.transform.localScale == new Vector3(1, 1, 1))
        {
            Player.transform.position = LeftTrans.GetComponent<TransitionScript>().nowPosition;
            LeftDoor.GetComponent<DoorScript>().Close();
        }
        else if (Player.transform.localScale == new Vector3(-1, 1, 1))
        {
            Player.transform.position = RightTrans.GetComponent<TransitionScript>().nowPosition;
            RightDoor.GetComponent<DoorScript>().Close();
        }
        Player.transform.localScale = Vals.currentRotation;
    }
    public void Spawn()
    {
        /*int countLoaded = SceneManager.sceneCount;
        Scene[] loadedScenes = new Scene[countLoaded];
        for (int i= 0; i<countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i);
            if (loadedScenes[i] !=gameObject.scene)
        {
            Debug.Log("Unload " + loadedScenes[i].name);
        }
        }*/
        //GetArray of scenes, if gameObject doesnt belong to scene x, unload scene x
        
        for (int i = 0; i< GOsInScene.Length; i++)
        {
            GameObject GO = Instantiate(GOsInScene[i], GOPositions[i], Quaternion.identity);
            if (GO.scene != gameObject.scene)
            {
                SceneManager.MoveGameObjectToScene(GO, gameObject.scene);
            }
           
        }
    }
    
}
