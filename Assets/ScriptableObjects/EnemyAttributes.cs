using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttributes", menuName = "Scriptable Objects/EnemyAttributes")]
public class EnemyAttributes : ScriptableObject
{
    public float enemy_Health;
    public float enemy_Speed;
    public string enemy_Name;
    public EnemyType enemyType;

}
public enum EnemyType {Normal, Boss};
