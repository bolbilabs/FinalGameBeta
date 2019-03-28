using UnityEngine;

/* Contains all the stats for a player character/enemy. */

public class CharacterStats : MonoBehaviour
{
    public AutoDialogue autoDialogue;

    public string characterName;

    public Stat maxHealth;          // Maximum amount of health

    public int currentHealth;   // Current amount of health

    public int currentPeekDamage;

    public Stat attack;
    public Stat defense;
    public Stat speed;
    public Stat accuracy;

    public string[] superlatives;

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

        string superlative = "";

        if (damage >= 100)
        {
            Random random = new Random();
            int randomNumber = Random.Range(0, superlatives.Length-1);
            superlative = superlatives[randomNumber] + " ";
        }

        autoDialogue.sentences.Insert(0, superlative + characterName + " takes " + damage + " damage!");

        if (currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
            autoDialogue.sentences.Insert(1, characterName + " is defeated!");
        }
    }


    // Damage the target
    public virtual void AttackTarget(CharacterStats target, int damage, bool ignoreDefense)
    {
        if (Random.Range(0, 100) < accuracy.GetValue() && target.gameObject.activeSelf)
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
            autoDialogue.sentences.Insert(0, "Attack misses!");

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
        if (target.gameObject.activeSelf)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());
        }
        else
        {
            Debug.Log("Target does not exist?!?");
            autoDialogue.sentences.Insert(0, "But it failed!");

        }
    }

}