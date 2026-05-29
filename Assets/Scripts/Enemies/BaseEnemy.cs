using UnityEngine;

public class BaseEnemy : MonoBehaviour,IDamageable
{
    protected EnemyData enemyStats;
    
    public int health {  get; set; }


    public void Initialize(EnemyData _stats)
    {
        enemyStats = _stats;
        health = enemyStats.maxHealth;
    }


    public void Damage(int damage)
    {
        health -= damage;
    }

    protected virtual void Attack()
    {

    }

    protected virtual void Guard()
    {

    }
}
