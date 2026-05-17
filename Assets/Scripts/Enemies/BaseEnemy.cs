using UnityEngine;

public class BaseEnemy : MonoBehaviour,IDamageable
{
    public int health {  get; set; }

    public void Damage(int damage)
    {
        health -= damage;
    }
}
