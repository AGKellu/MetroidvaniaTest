using UnityEngine;
[CreateAssetMenu(fileName = "Attacks", menuName = "ScriptableObjects/Attacks/PlayerAttack")]
public class ScriptableObjectScript : ScriptableObject
{
    public string name;
    public int AttackFrames;
    public int Damage;
    public bool Using;
    public float KnockBack;
    public Vector2 HitDirection;
    public int ManaGain;
    public bool holdable;
    //public int ManaCost;
    //public bool Spell;
}
