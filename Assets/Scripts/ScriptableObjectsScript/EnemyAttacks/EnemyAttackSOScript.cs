using UnityEngine;
[CreateAssetMenu(fileName = "Attacks", menuName = "ScriptableObjects/Attacks/EnemyAttack")]
public class EnemyAttackSOScript : ScriptableObject
{
    public string name;
    public int AttackFrames;
    public int Damage;
    public bool Using;
    public float KnockBack;
    public Vector2 HitDirection;
}
