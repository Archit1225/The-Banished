using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBossAttack", menuName = "Boss/Attack")]
public class Attacks : ScriptableObject
{
    public string AttackName;
    public string animationTrigger;
    public float minRange;
    public float maxRange;
    public float damage;
    public float attackCooldown;
    public AttackTypes attackType;
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float lingering_damage;
    public AudioClip attackAudio;
}
public enum AttackTypes {Ranged, Melee, Lingering};
