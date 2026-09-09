//using System.Threading.Tasks.Dataflow;
//using System.Numerics;
//using System.Threading.Tasks.Dataflow;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public GameObject BelongsTo;
    public bool OpensFire;
    public bool OpensIce;
    public Vector3 mousePos;
    private Camera mainCam;
    public float force;
    private Vector3 MoveVector;

    void Start()
    {
        Destroy(gameObject, 2);
        //Debug.Log(mousePos);
        /*mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        GetComponent<Rigidbody2D>().linearVelocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot * 90);
        */
        //        GetComponent<Rigidbody2D>().linearVelocity = transform.forward * 5;
        MoveVector = (mousePos - transform.position).normalized * force;
    }
    void Update()
    {
        float step = force * Time.deltaTime;
        //transform.position = Vector3.MoveTowards(transform.position, mousePos, step);
        transform.position += MoveVector *  3* Time.deltaTime;
        //Debug.Log(transform.position);
    }

    void OnTriggerEnter2D(Collider2D attackHitbox)
    {
        //GameObject Player = GameObject.FindGameObjectWithTag("Player");
        /*if (attackHitbox.gameObject.CompareTag("Player") && !BelongsTo == attackHitbox.gameObject)
        {
            attackHitbox.gameObject.GetComponent<PlayerAttack>().TakeDamage(BelongsTo.GetComponent<EnemyAttack>().EnemyCurrentAttack.Damage);
            Destroy(gameObject, 0);
        }*/
        if (attackHitbox.gameObject.CompareTag("Player") && !BelongsTo.CompareTag("Player"))
        {
            PlayerAttack.instance.TakeDamage(BelongsTo.GetComponent<EnemyAttack>().EnemyCurrentAttack.Damage);
            Destroy(gameObject, 0);
        }
        else if (attackHitbox.gameObject.CompareTag("Hurtbox") && !BelongsTo.CompareTag("Enemy"))
        {
            attackHitbox.gameObject.transform.parent.gameObject.GetComponent<EnemyAttack>().TakeDamage();
        }
        else if (attackHitbox.gameObject.CompareTag("Door") && BelongsTo.CompareTag("Player"))
        {

            attackHitbox.gameObject.GetComponent<DoorScript>().OpenDoor(OpensFire, OpensIce);
            Destroy(gameObject, 0);
        }
    }
    void OnTriggerExit2D(Collider2D rangeCircle)
    {
        if (rangeCircle.gameObject.name == "RangeCircle")
        {
            //Debug.Log("Leaving effective range.\nDestroy.");
            Destroy(gameObject);
        }
    }
}
