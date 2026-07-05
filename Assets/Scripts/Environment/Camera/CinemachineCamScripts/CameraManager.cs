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
            if (allCams[i].gameObject.name.Contains("Follow"))
            {
                currentCam = allCams[i];
                positionComposer = currentCam.GetComponent<CinemachinePositionComposer>();

                FollowTarget = GameObject.FindGameObjectWithTag("Follower");
                currentCam.Follow = FollowTarget.transform;
            }
        }
        startingTrackedObjectOffset = positionComposer.TargetOffset;
    }
    void Start()
    {
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
        PlayerMovement.instance.GetComponent<CinemachineImpulseSource>().GenerateImpulseWithVelocity(direction);
    }
    public IEnumerator Shatter(float Frames)
    {
        ShatterCam.Priority = 2;
        Time.timeScale = .50f;
        yield return new WaitForSecondsRealtime(60 / 60);
        Time.timeScale = 1;
    }
    private void StartCamPan(bool Direction)
    {

        if (Direction && !movingDown && PlayerMovement.instance.Grounded && !PlayerMovement.instance.MovingRight && !PlayerMovement.instance.MovingLeft && PlayerMovement.instance.CanMoveCam)
        {
            
            movingUp = true;
        }
        else if (!Direction && !movingUp && PlayerMovement.instance.Grounded && !PlayerMovement.instance.MovingRight && !PlayerMovement.instance.MovingLeft && PlayerMovement.instance.CanMoveCam)
        {
            movingDown = true;
        }
    }
    public void StartMovingUp()
    {
        Working = true;
        for (int i = 0; i< allCams.Length; i++)
        {
            if (allCams[i].gameObject.name.Contains("Up"))
            {
                currentCam = allCams[i];
                currentCam.Priority = 1;
                currentCam.Follow = null;
            }
            else
            {
                
                allCams[i].Priority =0;
            }
        }
    }

    public void StartMovingDown()
    {
        Working = true;
        for (int i = 0; i < allCams.Length; i++)
        {
            if (allCams[i].gameObject.name.Contains("Up"))
            {
                currentCam = allCams[i];
                currentCam.Priority = 1;
                currentCam.Follow = null;
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
            currentCam.transform.position = new Vector3(FollowTarget.transform.position.x, currentCam.transform.position.y + (panSpeed * Time.deltaTime), currentCam.transform.position.z);
        }
        else if (PlayerMovement.instance.gameObject.transform.rotation == Quaternion.Euler(0f, 0f, 0f))
        {
            currentCam.transform.position = new Vector3(FollowTarget.transform.position.x, currentCam.transform.position.y + (panSpeed * Time.deltaTime), currentCam.transform.position.z);
        }
        }
        else
        {
            if (PlayerMovement.instance.gameObject.transform.rotation == Quaternion.Euler(0f, 180f, 0f))
                {
                    currentCam.transform.position = new Vector3(FollowTarget.transform.position.x, currentCam.transform.position.y - (panSpeed * Time.deltaTime), currentCam.transform.position.z);
                }
                else if (PlayerMovement.instance.gameObject.transform.rotation == Quaternion.Euler(0f, 0f, 0f))
                {
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
            movingUp = false;
            for (int i = 0; i< allCams.Length; i++)
            {
                if (allCams[i].gameObject.name.Contains("Follow"))
                {
                    allCams[i].Priority= 1;
                    currentCam = allCams[i];
                }
                else if (allCams[i].gameObject.name.Contains("Up"))
                {
                    allCams[i].Priority = 0;
                    allCams[i].transform.localPosition = new Vector3(allCams[i].transform.position.x, -0.75f, -10);
                }
            }
        }
        else if (movingDown)
        {
            movingDown = false;
            for (int i = 0; i < allCams.Length; i++)
            {
                if (allCams[i].gameObject.name.Contains("Up"))
                {
                    allCams[i].Priority = 0;
                    allCams[i].transform.localPosition = new Vector3(allCams[i].transform.position.x,-0.75f,-10);
                }
                else if (allCams[i].gameObject.name.Contains("Follow"))
                {
                    allCams[i].Priority=1;
                    currentCam = allCams[i];
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
            }
        }
        else if (movingDown)
        {
            lookFrames++;
            if (lookFrames >= 5)
            {
                StartMovingDown();
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
