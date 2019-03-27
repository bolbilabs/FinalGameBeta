using UnityEngine;

/* Contains all the stats for a player character/enemy. */

public class CharacterStats : MonoBehaviour
{

    public string characterName;

    public Stat maxHealth;          // Maximum amount of health

    public int currentHealth { get; protected set; }   // Current amount of health

    public int currentPeekDamage;

    public Stat attack;
    public Stat defense;
    public Stat speed;
    public Stat accuracy;

    // TODO: Status Conditions

    //public event System.Action OnHealthReachedZero;

    // Start with max HP.
    public virtual void Awake()
    {
        currentHealth = maxHealth.GetValue();
    }

    public virtual void Start()
    {

    }

    // Damage the character
    public virtual void TakeDamage(int damage, bool ignoreDefense)
    {
        // Subtract the armor value - Make sure damage doesn't go below 0.
        if (!ignoreDefense)
        {
            damage -= defense.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);
        }

        // Subtract damage from health
        currentHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        // TODO: If we hit 0. Die. Death status condition?
        //if (currentHealth <= 0)
        //{
        //    if (OnHealthReachedZero != null)
        //    {
        //        OnHealthReachedZero();
        //    }
        //}
    }


    // Damage the target
    public virtual void AttackTarget(CharacterStats target, int damage, bool ignoreDefense)
    {
        if (Random.Range(0, 100) < accuracy.GetValue() && target != null)
        {
            // Adds attack stat boost.
            damage += attack.GetValue();
            // Clamps to make sure not below zero.
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            target.TakeDamage(damage, ignoreDefense);
        }
        else
        {
            Debug.Log("Attack misses!");
        }
    }

    // Checks to see what the health of the target will be.
    public virtual int PeekAttackTarget(CharacterStats target, int damage, bool ignoreDefense)
    {
        if (target != null)
        {
            // Adds attack stat boost.
            damage += attack.GetValue();
            // Clamps to make sure not below zero.
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            return target.PeekDamage(damage, ignoreDefense);
        }
        else
        {
            return -1;
        }
    }

    // Check the future damage of character.
    public virtual int PeekDamage(int damage, bool ignoreDefense)
    {
        if (!ignoreDefense)
        {
            // Subtract the armor value - Make sure damage doesn't go below 0.
            damage -= defense.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);
        }

        // Subtract damage from health
        Debug.Log(transform.name + " will have " + (currentPeekDamage - damage) + " health.");

        if (currentHealth - damage <=  0)
        {
            Debug.Log(transform.name + " is in peril!");
        }

        currentPeekDamage -= damage;

        return currentPeekDamage;
    }

    // Heal the target.
    public virtual void Heal(CharacterStats target, int amount)
    {
        if (target != null)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());
        }
        else
        {
            Debug.Log("Target does not exist?!?");
        }
    }

}