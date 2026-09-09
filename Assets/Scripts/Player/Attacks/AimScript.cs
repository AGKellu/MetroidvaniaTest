//using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
public class AimScript : MonoBehaviour
{
    private Vector3 mousePos;
    public bool Aiming;
    public GameObject UpperBody;
    //[SerializeField] GameObject LowerBody;
    [SerializeField] GameObject AimRay;
    [SerializeField] GameObject Shot;
    [SerializeField] GameObject Line;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Aiming = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Aiming)
        {
            

            mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, Camera.main.nearClipPlane));
            AimRay.transform.position = mousePos;
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            UpperBody.transform.localRotation = Quaternion.Euler(0, 0, -rotZ);
            //UpperBody.transform.LookAt(AimRay.transform);
            Line.GetComponent<LineRenderer>().SetPosition(0, UpperBody.transform.position);
            Line.GetComponent<LineRenderer>().SetPosition(1, AimRay.transform.position);
            if (rotZ < 70)
            {
                PlayerMovement.instance.cameraFollowObject.CallTurn();

                //UpperBody.GetComponent<SpriteRenderer>().flipY = true;
                //UpperBody.GetComponent<SpriteRenderer>().flipX = true;
                PlayerMovement.instance.BackFootOffset = PlayerMovement.instance.BackFootOffset * -1;
            }
            else if (rotZ > 70)
            {
                PlayerMovement.instance.cameraFollowObject.CallTurn();
                PlayerMovement.instance.gameObject.transform.localRotation = Quaternion.Euler(PlayerMovement.instance.gameObject.transform.localRotation.x, PlayerMovement.instance.gameObject.transform.localRotation.y * 180, 0);
                //UpperBody.GetComponent<SpriteRenderer>().flipY = false;
                //UpperBody.GetComponent<SpriteRenderer>().flipY = false;
                PlayerMovement.instance.BackFootOffset = PlayerMovement.instance.BackFootOffset * -1;
            }
            //Debug.Log(AimRay.transform.position);

            //amgle = Mathf.Atan2(AimRay.transform.position.x, AimRay.transform.position.y) * Mathf.Rad2Deg;
            //UpperBody.transform.Rotate(new Vector3(0, 0, amgle));
            //UpperBody.transform.LookAt(AimRay.transform, Vector3.up);
            //UpperBody.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(AimRay.transform.position.x, AimRay.transform.position.y) * 180 / Mathf.PI);
        }
    }
    public void Aim()
    {
        Aiming = true;
        AimRay.SetActive(true);
        Line.SetActive(true);
    }
    public void EndAim()
    {
        Aiming = false;
        AimRay.SetActive(false);
        Line.SetActive(false);
        if (!PlayerMovement.instance.MovingLeft && !PlayerMovement.instance.MovingRight)
        {
            PlayerAttack.instance.PlayerAnim.SetBool("Idle", true);
        }
        else
        {
            PlayerAttack.instance.PlayerAnim.SetBool("Running", true);
        }
    }
    public void Shoot()
    {
        //GameObject Projectile = Instantiate(Shot, new Vector2(UpperBody.transform.position.x - .25f, UpperBody.transform.position.y - .1f), UpperBody.transform.rotation);
        GameObject Projectile = Instantiate(Shot, new Vector2(UpperBody.transform.position.x - .25f, UpperBody.transform.position.y - .1f), Shot.transform.rotation);
        //Destroy(Projectile, 0.5f);
        Projectile.GetComponent<ProjectileScript>().BelongsTo = PlayerAttack.instance.gameObject;
        Projectile.GetComponent<ProjectileScript>().mousePos = mousePos;
        //Projectile.transform.LookAt(AimRay.transform);
        //Projectile.transform.localRotation = Quaternion.Euler(Projectile.transform.localRotation.x, Projectile.transform.localRotation.y, UpperBody.transform.localRotation.z);
        //Debug.Log(Projectile.transform.rotation.z);
        
        /*if (Projectile.transform.localRotation != Quaternion.Euler(0f, 0f, ))
                {
                    Projectile.GetComponent<Rigidbody2D>().linearVelocityX = -5;
                    //Debug.Log(Projectile.GetComponent<Rigidbody2D>().linearVelocityX);
                }
                if (Projectile.transform.localRotation == Quaternion.Euler(0f, 0f, 0f))
                {
                    Projectile.GetComponent<Rigidbody2D>().linearVelocityX = 5;
                }
        */
        
       //Projectile.GetComponent<Rigidbody2D>().linearVelocity = transform.TransformDirection(Vector3.forward * 50);
        //Projectile.GetComponent<Rigidbody2D>().linearVelocityX = 5;
    }
}
