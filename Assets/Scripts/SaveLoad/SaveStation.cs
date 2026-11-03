using UnityEngine;

public class SaveStation : MonoBehaviour
{
    [SerializeField] private GameObject SaveCanvas;
    private AudioSource AudioSource;
    public Vector3 nowPosition;
    [SerializeField] private AudioClip SaveRoomOST;
    private Animation Anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioSource = gameObject.GetComponent<AudioSource>();
        Anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PlayClip(AudioClip clip)
    {
        if (AudioSource.isPlaying)
        {

        AudioSource.Pause();
        }
        else
        {
            AudioSource.Stop();
        }
        AudioSource.clip = clip;
        AudioSource.Play();
        Anim.Play("SaveLoad");
        Invoke(nameof(EventOnEnd), clip.length);


    }
    void EventOnEnd()
    {
        AudioSource.clip = SaveRoomOST;
        AudioSource.Play();
        Debug.Log("audio clip ended!");
    }
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            SaveCanvas.SetActive(true);
            PlayerMovement.instance.AvailableJumps = 0;
            PlayerMovement.instance.OnSaveStation = true;
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            SaveCanvas.SetActive(false);
            if (PlayerMovement.instance.Unlockables[2])
            {
                PlayerMovement.instance.AvailableJumps = 2;
            }
            else
            {

                PlayerMovement.instance.AvailableJumps = 1;
            }
            PlayerMovement.instance.OnSaveStation = false;
        }
    }
}
