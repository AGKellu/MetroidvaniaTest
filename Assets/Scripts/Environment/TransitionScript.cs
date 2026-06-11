using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using System.ComponentModel;
//using System.ComponentModel;
public class TransitionScript : MonoBehaviour
{
    [SerializeField] private string RoomToLoad;
    [SerializeField] private GameObject RealPlayer;
    [SerializeField] private PlayerSOScript Values;
    [SerializeField] private Vector3 nowPosition;
    private bool changing = false;
    public IEnumerator Transition(GameObject player)
    {
        changing = true;
        Debug.Log("Transition to next Scene");
        string ActiveScene = SceneManager.GetActiveScene().name;
        //Debug.Log(ActiveScene);
        //GameObject player = GameObject.FindGameObjectWIthTag("Player");
        player.GetComponent<PlayerMovement>().ableToMove = false;
        //player.GetComponent<PlayerAttack>().Transition();
        //player.GetComponent<PlayerMovement>().Transition();
        //Destroy(player, 0);
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(RoomToLoad, LoadSceneMode.Additive);
        while (!loadScene.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(RoomToLoad));
        GameObject UI = GameObject.FindGameObjectWithTag("Canvas");
        SceneManager.MoveGameObjectToScene(UI, SceneManager.GetActiveScene());
        SceneManager.MoveGameObjectToScene(player, SceneManager.GetActiveScene());
        player.transform.position = nowPosition;
        //player.GetComponent<PlayerAttack>().Camera = GameObject.FindGameObjectWithTag("RealCamera");
        AsyncOperation unloadScene = SceneManager.UnloadSceneAsync(ActiveScene);
        while (!unloadScene.isDone)
        {
            yield return null;
        }
        //GameObject newPlayer = Instantiate(RealPlayer, Values.currentTransform + new Vector3(-2.9f, 0, 0), transform.rotation);

        //newPlayer.transform.localScale = Values.currentRotation;
        //GameObject RealCam = GameObject.FindGameObjectWithTag("RealCamera");
        //RealCam.GetComponent<CameraFollow>().Player = newPlayer;
        //Make Player Values onto Value SO, Destroy Player, then Instaniate New Player at transform
        
        Debug.Log(RoomToLoad + " has been loaded!\nSend player in front of leave trigger depending on what direction theyre looking");
        //yield return null;
        //player.GetComponent<PlayerMovement>().ableToMove = true;
        //player.GetComponent<PlayerMovement>().Grounded = true;
        //abletomove = false, fade scene to black, load, send player in front of leave trigger, unload, unfade from black, abletomove = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!changing)
        {
            if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Transition(other.gameObject));
        }
        }
        
    }
}
