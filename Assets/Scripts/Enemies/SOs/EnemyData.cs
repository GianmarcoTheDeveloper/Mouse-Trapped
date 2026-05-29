using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] public int maxHealth;
    [SerializeField] public int damage;
    [SerializeField] public float moveSpeed; 
}
