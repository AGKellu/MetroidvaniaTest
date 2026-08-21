using UnityEngine;
using UnityEngine.InputSystem;
public class AimScript : MonoBehaviour
{
    private Vector3 mousePos;
    public bool Aiming;
    public  GameObject UpperBody;
    //[SerializeField] GameObject LowerBody;
    [SerializeField] GameObject AimRay;
    [SerializeField] GameObject Shot;
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
            //UpperBody.transform.rotation = Quaternion.Euler(0, 0, rotZ);
            //transform.rotation = Quaternion.Euler(0, 0, rotZ);
            UpperBody.transform.localRotation = Quaternion.Euler(0, 0, -rotZ);
            if (rotZ > 90 || rotZ < -90)
            {
                UpperBody.GetComponent<SpriteRenderer>().flipY = true;
                UpperBody.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                UpperBody.GetComponent<SpriteRenderer>().flipY = false;
                UpperBody.GetComponent<SpriteRenderer>().flipY = false;
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
    }
    public void EndAim()
    {
        Aiming = false;
        AimRay.SetActive(false);
    }
    public void Shoot()
    {
        GameObject Projectile = Instantiate(Shot, new Vector2(transform.position.x, transform.position.y + .1f), Quaternion.identity);
        Destroy(Projectile, 0.5f);
        Projectile.GetComponent<ProjectileScript>().BelongsTo = PlayerAttack.instance.gameObject;
    }
}
