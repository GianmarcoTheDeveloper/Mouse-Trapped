using UnityEngine;

public class BaseEnemy : MonoBehaviour,IDamageable
{
    [SerializeField] private float maxHealth;
    public int health {  get; set; }

    public void Damage(int damage)
    {
        health -= damage;
    }
}
