using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] private CinemachineCamera[] allCams;
    [Header("Controls for lerping the Y damping during player jump/fall")]
    [SerializeField] private float fallPanAmount = .25f;
    [SerializeField] private float fallYPanTime = .35f;
    public float fallSpeedYDampingChangeThreshold = -15f;
    public bool isLerpingYDamping { get; private set; }
    public bool lerpedFromPlayerFalling { get; set; }
    private Coroutine lerpYPanCoroutine;
    private Coroutine panCameraCoroutine;
    private CinemachineCamera currentCam;
    private CinemachinePositionComposer positionComposer;
    private float normYPanAmount;
    private Vector2 startingTrackedObjectOffset;
    private CinemachineCamera UpDownCam;
    [SerializeField] private float lookFrames = 0f;
    [SerializeField] private bool movingUp;
    [SerializeField] private bool movingDown;
    [SerializeField] private float panSpeed = 0f;
    //[SerializeField] private float lookCapUp;
    //[SerializeField] private float lookCapDown;
    
    private InputAction panCamUp;
    private InputAction panCamDown;
    private bool Moving;
    private GameObject FollowTarget;
    private bool Working;
    [SerializeField] private CinemachineCamera ShatterCam;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
        for (int i = 0; i < allCams.Length; i++)
        {
            allCams[i].Follow = GameObject.FindGameObjectWithTag("Follower").transform;
            //allCams[i]
            //allCams[i].Follow = FollowTarget.transform;
            if (allCams[i].gameObject.name.Contains("Follow"))
            {
                currentCam = allCams[i];
                positionComposer = currentCam.GetComponent<CinemachinePositionComposer>();

                FollowTarget = GameObject.FindGameObjectWithTag("Follower");
                currentCam.Follow = FollowTarget.transform;
            }
            
        }
        //normYPanAmount = positionComposer.YDamping;
        startingTrackedObjectOffset = positionComposer.TargetOffset;
        
    }
    void Start()
    {
        //Debug.Log("Exits");
        panCamUp = InputSystem.actions.FindAction("Camera/MoveUp");
        panCamUp.performed += ctx => StartCamPan(true);
        panCamUp.canceled += ctx => StopMovingUpDown();
        panCamDown = InputSystem.actions.FindAction("Camera/MoveDown");
        panCamDown.performed += ctx => StartCamPan(false);
        panCamDown.canceled += ctx => StopMovingUpDown();
        FollowTarget = GameObject.FindGameObjectWithTag("Follower");
    }
    public void Shake(Vector3 direction)
    {
        //Debug.Log(currentCam);
        PlayerMovement.instance.GetComponent<CinemachineImpulseSource>().GenerateImpulseWithVelocity(direction);
//        currentCam.gameObject.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
    }
    public IEnumerator Shatter(float Frames)
    {
        ShatterCam.Priority = 2;
        Time.timeScale = .25f;
        yield return new WaitForSeconds((Frames* 2) / 60);
        Time.timeScale = 1;
    }
    private void StartCamPan(bool Direction)
    {
        if (Direction && !movingDown && PlayerMovement.instance.Grounded && !PlayerMovement.instance.MovingRight && !PlayerMovement.instance.MovingLeft)
        {
            
            movingUp = true;
        }
        else if (!Direction && !movingUp && PlayerMovement.instance.Grounded && !PlayerMovement.instance.MovingRight && !PlayerMovement.instance.MovingLeft)
        {
            movingDown = true;
        }
    }
    public void StartMovingUp()
    {
        //Moving = true;
        Working = true;
        
        //Debug.Log(allCams[0].gameObject.name);
        for (int i = 0; i< allCams.Length; i++)
        {
            if (allCams[i].gameObject.name.Contains("Up"))
            {
                //allCams[i].Priority++;
                //lookCapUp = allCams[i].transform.position.y + 3;
                currentCam = allCams[i];
                currentCam.Priority = 1;
                currentCam.Follow = null;
                //lookCapUp = currentCam.transform.position.y + 3;
                /*if (PlayerMovement.instance.gameObject.transform.rotation == Quaternion.Euler(0f, 180f, 0f))
                {
                    //Debug.Log("Move To the left");
                    currentCam.transform.position = new Vector3(FollowTarget.transform.position.x - .5f, currentCam.transform.position.y + (panSpeed * Time.deltaTime), currentCam.transform.position.z);
                }
                else if (PlayerMovement.instance.gameObject.transform.rotation == Quaternion.Euler(0f, 0f, 0f))
                {
                    //Debug.Log("Move to the right");
                    currentCam.transform.position = new Vector3(FollowTarget.transform.position.x + .5f, currentCam.transform.position.y + (panSpeed * Time.deltaTime), currentCam.transform.position.z);
                }*/
                //currentCam.transform.position = FollowTarget.transform.position + (FollowTarget.transform.right * 3);
                //Debug.Log(currentCam.gameObject.name);
            }
            else
            {
                
                allCams[i].Priority =0;
            }
            //Debug.Log(allCams[i].gameObject.name);
        }
    }

    public void StartMovingDown()
    {
        //Moving = true;
        Working = true;
        for (int i = 0; i < allCams.Length; i++)
        {
            if (allCams[i].gameObject.name.Contains("Up"))
            {
                //allCams[i].Priority++;
                //lookCapDown = allCams[i].transform.position.y - 3;
                currentCam = allCams[i];
                currentCam.Priority = 1;
                currentCam.Follow = null;
                //lookCapDown = currentCam.transform.position.y - 3;

                //currentCam.transform.position = FollowTarget.transform.position +(FollowTarget.transform.right * 3);
                //Debug.Log(currentCam.gameObject.name);
            }
            else
            {
                allCams[i].Priority = 0;
            }
        }
    }
    
    void Move(bool movingUp)
    {
        if (movingUp)
        {
            if (PlayerMovement.instance.gameObject.transform.rotation == Quaternion.Euler(0f, 180f, 0f))
        {
            //Debug.Log("Move To the left");
            currentCam.transform.position = new Vector3(FollowTarget.transform.position.x, currentCam.transform.position.y + (panSpeed * Time.deltaTime), currentCam.transform.position.z);
        }
        else if (PlayerMovement.instance.gameObject.transform.rotation == Quaternion.Euler(0f, 0f, 0f))
        {
            //Debug.Log("Move to the right");
            currentCam.transform.position = new Vector3(FollowTarget.transform.position.x, currentCam.transform.position.y + (panSpeed * Time.deltaTime), currentCam.transform.position.z);
        }
        }
        

        else
        {
            if (PlayerMovement.instance.gameObject.transform.rotation == Quaternion.Euler(0f, 180f, 0f))
                {
                    //Debug.Log("Move To the left");
                    currentCam.transform.position = new Vector3(FollowTarget.transform.position.x, currentCam.transform.position.y - (panSpeed * Time.deltaTime), currentCam.transform.position.z);
                }
                else if (PlayerMovement.instance.gameObject.transform.rotation == Quaternion.Euler(0f, 0f, 0f))
                {
                    //Debug.Log("Move to the right");
                    currentCam.transform.position = new Vector3(FollowTarget.transform.position.x, currentCam.transform.position.y - (panSpeed * Time.deltaTime), currentCam.transform.position.z);
                }
        }
    }

    public void StopMovingUpDown()
    {
        lookFrames = 0f;
        //direction true = up
        //direction false = down
        Moving = false;
        Working = false;
        if (movingUp)
        {
            //lookCapUp = 0f;
            movingUp = false;
            for (int i = 0; i< allCams.Length; i++)
            {
                if (allCams[i].gameObject.name.Contains("Follow"))
                {
                    allCams[i].Priority= 1;
                    currentCam = allCams[i];
                    //Debug.Log(currentCam.gameObject.name);
                }
                else if (allCams[i].gameObject.name.Contains("Up"))
                {
                    allCams[i].Priority = 0;
                    //currentCam.Follow = GameObject.FindGameObjectWithTag("Follower").transform;
                    //Debug.Log("Please");
                    //allCams[i].Follow = GameObject.FindGameObjectWithTag("Follower").transform;
                    //allCams[i].position
                    //allCams[i].Follow = FollowTarget.transform;
                    //allCams[i].ForceCameraPosition(new Vector3(allCams[i].transform.position.x, -.75f, allCams[i].transform.position.z), Quaternion.identity);
                   // allCams[i].transform.position = new Vector3(allCams[i].transform.position.x, -.75f, allCams[i].transform.position.z);
                    allCams[i].transform.localPosition = new Vector3(allCams[i].transform.position.x, -0.75f, -10);
                    
                }
                
            }
        }
        else if (movingDown)
        {
            //lookCapDown = 0f;
            movingDown = false;
            for (int i = 0; i < allCams.Length; i++)
            {
                if (allCams[i].gameObject.name.Contains("Up"))
                {
                    allCams[i].Priority = 0;
                    //Debug.Log("Please");
                    //allCams[i].transform.position = new Vector3(allCams[i].transform.position.x,-.75f, allCams[i].transform.position.z);
                    //allCams[i].Follow = FollowTarget.transform;
                    allCams[i].transform.localPosition = new Vector3(allCams[i].transform.position.x,-0.75f,-10);
                    //allCams[i].ForceCameraPosition(new Vector3(allCams[i].transform.position.x, -.75f, allCams[i].transform.position.z), Quaternion.identity);
                }
                else if (allCams[i].gameObject.name.Contains("Follow"))
                {
                    allCams[i].Priority=1;
                    currentCam = allCams[i];
                    //Debug.Log(currentCam.gameObject.name);
                }
            }
        }

    }
    private void Update()
    {
        if (movingUp)
        {
            lookFrames++;
            if (lookFrames >= 5)
            {
                StartMovingUp();
                //Debug.Log(currentCam.transform.position + (PlayerMovement.instance.transform.forward * 3));
            }
        }
        else if (movingDown)
        {
            lookFrames++;
            if (lookFrames >= 5)
            {
                StartMovingDown();
                //Debug.Log(currentCam.transform.position + (PlayerMovement.instance.transform.forward * 3));
            }
        }
        if (Working)
        {
            if (movingUp)
            {
                Move(true);
            }
            else if (movingDown)
            {
                Move(false);
            }
        }
    }

    #region Lerp The Y Damping
    public void LerpYDamping(bool isPlayerFalling)
    {
        lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }
    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        isLerpingYDamping = true;

        float startDampAmount = positionComposer.Damping.y;
        float endDampAmount = 0f;

        if (isPlayerFalling)
        {
            endDampAmount = fallPanAmount;
            lerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = normYPanAmount;
        }

        float elapsedTime = 0f;
        while (elapsedTime < fallYPanTime)
        {
            elapsedTime += Time.deltaTime;
            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, (elapsedTime / fallYPanTime));
            positionComposer.Damping.y = lerpedPanAmount;
            yield return null;
        }
        isLerpingYDamping = false;
    }
    #endregion

   /* #region Pan Camera

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }
    
    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        if (!panToStartingPos)
        {
            switch(panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.right;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.left;
                    break;
                default:
                    break;

            }

            endPos *= panDistance;
            startingPos = startingTrackedObjectOffset;
            endPos += startingPos;
        }
        else
        {
            startingPos = positionComposer.TargetOffset;
            endPos = startingTrackedObjectOffset;
        }
        float elapsedTime = 0f;
        while (elapsedTime < panTime)
        {
            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            positionComposer.TargetOffset = panLerp;
            yield return null;
        }
       
    }
    #endregion
    */
}
