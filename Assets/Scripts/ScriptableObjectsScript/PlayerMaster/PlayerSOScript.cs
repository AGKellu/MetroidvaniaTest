using UnityEngine;
using UnityEngine.InputSystem;
[CreateAssetMenu(fileName = "Player", menuName= "ScriptableObjects/Player/Main")]
public class PlayerSOScript : ScriptableObject
{
    public float speed;
    public float JumpForce;
    public float JumpSpeed;
    public int JumpCount;
    public int Health;
    public int maxHealth;
    public float Mana;
    public float ManaMax;
    public ScriptableObjectScript currentAttack;
    public ScriptableObjectScript Spell1;
    public ScriptableObjectScript Normal;
   // public Vector3 currentTransform;
    public Vector3 currentRotation;


}
