using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CameraFollowObject : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform _playerTransform;
    [Header("Flip Rotation Stats")]
    [SerializeField] private float flipYRotationTime = .5f;
    private Coroutine turnCoroutine;
    private PlayerMovement playerMovement;
    private bool isFacingRight;

    private void Awake()
    {
        playerMovement = _playerTransform.gameObject.GetComponent<PlayerMovement>();
        isFacingRight = playerMovement.MovingRight;
    }
    private void Update()
    {
        transform.position = _playerTransform.position;
    }

    public void CallTurn()
    {
        turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
            yield return null;
        }
    }
    
    private float DetermineEndRotation()
    {
        isFacingRight = !playerMovement.MovingRight;

        if (isFacingRight)
        {
            return 100f;
        }
        else
        {
            return 0f;
        }
    }
}
