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

    public bool isGuarded = false;
    public bool isPermahealed = false;
    public bool isRoared = false;

    public CharacterStats rory;

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
            int randomNumber = Random.Range(0, superlatives.Length - 1);
            superlative = superlatives[randomNumber] + " ";
        }

        autoDialogue.MessageMe(superlative + characterName + " takes " + damage + " damage!");

        if (currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
            autoDialogue.MessageMe(characterName + " is defeated!");
        }

    }


    // Damage the target
    public virtual void AttackTarget(CharacterStats target, int damage, bool ignoreDefense)
    {
        if (!isRoared)
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
                autoDialogue.MessageMe("Attack misses!");


            }
        }
        else
        {
            if (rory.gameObject.activeSelf)
            {
                autoDialogue.MessageMe(characterName + ",& in a state of pure survival,& attacks Rory with all of its strength.");
                rory.TakeDamage(99999, true);
                isRoared = false;
            }
            else
            {
                autoDialogue.MessageMe("Still panicked,&&&% " + characterName + " further mutilates Rory's carcass.");
                isRoared = false;
            }
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

        if (currentHealth - damage <= 0)
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
            target.currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());
            autoDialogue.MessageMe(characterName + " gains " + amount + " health!");

        }
        else
        {
            Debug.Log("Target does not exist?!?");
            autoDialogue.MessageMe("But it failed!");

        }
    }

    public virtual int PeekHeal(CharacterStats target, int amount)
    {
        // Subtract the armor value - Make sure damage doesn't go below 0.
        amount += attack.GetValue();
        amount = Mathf.Clamp(amount, 0, int.MaxValue);

        // Subtract damage from health
        Debug.Log(transform.name + " will have " + (currentPeekDamage + amount) + " health.");

        //if (currentHealth - damage <= 0)
        //{
        //    Debug.Log(transform.name + " is in peril!");
        //}

        currentPeekDamage += amount;

        return currentPeekDamage;
    }

    public virtual void GuardOn()
    {
    }

    public virtual void PermahealOn()
    {
    }

    public virtual void RoryOn(CharacterStats roryStats)
    {
        rory = roryStats;
        autoDialogue.MessageMe(characterName + " begins to fear Rory greatly.");
        isRoared = true;
    }

    public virtual void TurnOver()
    {
    }
}