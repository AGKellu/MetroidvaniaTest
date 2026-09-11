using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public string AbilityUnlocked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void UnlockAbility(string Ability, GameObject Player)
    {
        if (Ability == "NormalShot")
        {

            Player.GetComponent<PlayerAttack>().ableToAttack = true;
        }
        if (Ability == "LongBeam")
        {
            Player.GetComponent<PlayerAttack>().EffectiveRange = 1.25f;
        }
    }
    void OnTriggerEnter2D(Collider2D Coll)
    {
        //if (Coll.gameObject.name == "NormalShot")
        //{
        if (Coll.gameObject.CompareTag("Player"))
        {
            
        UnlockAbility(AbilityUnlocked, Coll.gameObject);
        Destroy(gameObject);
        }
        //}
        //if (Coll.gameObject.name == "LongBeam")
        //{
          //  Coll.gameObject.GetComponent<PlayerAttack>().EffectiveRange = 1.25f;
        //}
    }
}
