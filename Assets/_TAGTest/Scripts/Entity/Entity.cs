using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Stats")]

    [SerializeField, Min(0.0f)]
    protected float startHealth = 10.0f;

    [SerializeField, Min(0.0f)]
    protected float maxHealth = 10.0f;
    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    [SerializeField]
    protected float currentHealth = 10.0f;
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
    }

    [Header("Movement Stats")]
    [SerializeField]
    protected bool canMove = true;

    [SerializeField, Min(0.0f)]
    protected float speed = 10;

    [Header("Attack Stats")]
    [SerializeField]
    protected bool canAttack = true;

    [SerializeField]
    protected float attackSpeed = 1.0f;

    public virtual void Move(Vector3 direction)
    {
        Debug.LogWarning("No Movement Logic Implemented in base Entity class");
    }

    public virtual void LookAt(Vector3 direction)
    {
        Debug.LogWarning("No LookAt Logic Implemented in base Entity class");
    }

    public virtual void Attack()
    {
        Debug.LogWarning("No Attack Logic Implemented in base Entity class");
    }

    public void TakeDamage(float amount)
    {
        if (amount < 0)
        {
            //Can't take negative Damage
            return;
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            OnDeath();
        }
        else
        {
            OnTakeDamage();
        }
    }

    protected virtual void OnTakeDamage()
    {
        //If you want to do some animation stuff or what not. It's independent of the actual damage taking process
    }

    protected virtual void OnDeath()
    {
        //If you want to do some animation stuff or what not.
        canMove = false;
        canAttack = false;
    }


}
