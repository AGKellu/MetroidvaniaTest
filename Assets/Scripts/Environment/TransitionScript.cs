using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
//using System.ComponentModel;
public class TransitionScript : MonoBehaviour
{
    [SerializeField] private string RoomToLoad;
    [SerializeField] private GameObject RealPlayer;
    [SerializeField] private PlayerSOScript Values;
    public IEnumerator Transition(GameObject player)
    {
        Debug.Log("Transition to next Scene");
        string ActiveScene = SceneManager.GetActiveScene().name;
        Debug.Log(ActiveScene);
        //GameObject player = GameObject.FindGameObjectWIthTag("Player");
        player.GetComponent<PlayerMovement>().ableToMove = false;
        yield return null;
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(RoomToLoad, LoadSceneMode.Additive);
        while (!loadScene.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(RoomToLoad));
        player.GetComponent<PlayerAttack>().Transition();
        player.GetComponent<PlayerMovement>().Transition();
        Destroy(player, 0);
        GameObject newPlayer = Instantiate(RealPlayer, Values.currentTransform, transform.rotation);
        newPlayer.transform.localScale = Values.currentRotation;
        //Make Player Values onto Value SO, Destroy Player, then Instaniate New Player at transform
        //GameObject RealCam = GameObject.FindGameObjectWithTag("Camera");
        //RealCam.setActive(false);

        Debug.Log(RoomToLoad + " has been loaded!\nSend player in front of leave trigger depending on what direction theyre looking");

        //player.GetComponent<PlayerMovement>().ableToMove = true;
        //player.GetComponent<PlayerMovement>().Grounded = true;
        //abletomove = false, fade scene to black, load, send player in front of leave trigger, unload, unfade from black, abletomove = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Transition(other.gameObject));
        }
    }
}
