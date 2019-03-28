using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
    This component is derived from CharacterStats. It adds two things:
        - Gaining modifiers when equipping items
        - Restarting the game when dying
*/

public class PlayerStats : CharacterStats
{
    public Stat maxHope;

    public int hopeFlux;

    public int hope;


    // Use this for initialization
    public override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    public override void Start()
    {

        base.Start();

        //EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
    }


    // Damage the character
    public override void TakeDamage(int damage, bool ignoreDefense)
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


        //if (autoDialogue.sentences.Count >= autoDialogue.actionOrder.Count)
        //{
            autoDialogue.sentences.Insert((autoDialogue.sentences.Count - autoDialogue.actionOrder.Count), characterName + " takes " + damage + " damage!");

        //}
        //else
        //{
        //    autoDialogue.sentences.Insert(0, characterName + " takes " + damage + " damage!");
        //}


        // TODO: If we hit 0. Die. Death status condition?
        if (currentHealth <= 0)
        {
            this.gameObject.SetActive(false);

            //if (autoDialogue.sentences.Count >= autoDialogue.actionOrder.Count)
            //{

                autoDialogue.sentences.Insert((autoDialogue.sentences.Count - autoDialogue.actionOrder.Count), "Placeholder for character death.");
            //}

        }
    }


    // Damage the target
    public override void AttackTarget(CharacterStats target, int damage, bool ignoreDefense)
    {
        if (target.gameObject.activeSelf && Random.Range(0, 100) < accuracy.GetValue())
        {
            // Adds attack stat boost.
            damage += attack.GetValue() + (int)(hope * 0.1);
            // Clamps to make sure not below zero.
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            target.TakeDamage(damage, ignoreDefense);
        }
        else
        {
            Debug.Log("Attack misses!");
            autoDialogue.sentences.Insert(0, "But it totally missed!");
            AdjustHope(this, -hopeFlux * 2);
        }
    }

    // Checks to see what the health of the target will be.
    public override int PeekAttackTarget(CharacterStats target, int damage, bool ignoreDefense)
    {
        // Adds attack stat boost.
        damage += attack.GetValue() + (int)(hope * 0.1);
        // Clamps to make sure not below zero.
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        return target.PeekDamage(damage, ignoreDefense);
    }

    // Check the future damage of character.
    public override int PeekDamage(int damage, bool ignoreDefense)
    {
        if (!ignoreDefense)
        {
            // Subtract the armor value - Make sure damage doesn't go below 0.
            damage -= defense.GetValue() * (int) ((hope+500)/500);
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
    public override void Heal(CharacterStats target, int amount)
    {
        if (target != null)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth.GetValue());
        }
        else
        {
            Debug.Log("No matter how hard you try, you can never bring 'em back...");
            autoDialogue.sentences.Insert(0, "No matter how hard you try, you can never bring 'em back...");

            AdjustHope(this, -hopeFlux * 4);
        }
    }

    public void AdjustHope(PlayerStats target, int value)
    {
        target.hope += value;
    }



}