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

    public Sprite playerMiniPortrait;

    public Stat maxHope;

    public int hopeFlux;

    public int hope;


    public Sprite bodySprite;


    public PlayerStats rich;


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
        if (!isGuarded && !isPermahealed)
        {
            // Subtract the armor value - Make sure damage doesn't go below 0.
            if (!ignoreDefense)
            {
                damage -= defense.GetValue();
                damage = Mathf.Clamp(damage, 0, int.MaxValue);
            }

            // Subtract damage from health
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, int.MaxValue);

            Debug.Log(transform.name + " takes " + damage + " damage.");


            //if (autoDialogue.sentences.Count >= autoDialogue.actionOrder.Count)
            //{
            Debug.Log(autoDialogue);

            autoDialogue.MessageMe(characterName + " takes " + damage + " damage!");

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

                autoDialogue.MessageMe("Placeholder for character death.");
                BattleManager.GetInstance().deathparam++;

                //BattleManager.GetInstance().healthBlocks.RemoveAt(BattleManager.GetInstance().currentPlayer);

                if (BattleManager.GetInstance().players.Contains(this.gameObject))
                {
                    BattleManager.GetInstance().healthBlocks[BattleManager.GetInstance().players.IndexOf(this.gameObject)].GetComponent<Animator>().SetInteger("State", 0);
                }

                //}
            }
        }
        else if (isGuarded)
        {
            //Youch!
            autoDialogue.MessageMe("In a sudden act of selflessness&.&.&.&&& Rich blocks the attack from hitting " + characterName + "!");
            rich.TakeDamage(99999, true);
            isGuarded = false;
        } else if (isPermahealed)
        {
            // Immunity.
            autoDialogue.MessageMe("&&&.&.&.&&&%Luna's leftover light prevents the attack from damaging " + characterName + ".");
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
            autoDialogue.MessageMe("But it totally missed!");
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
            target.currentHealth += amount;
            target.currentHealth = Mathf.Clamp(target.currentHealth, 0, target.maxHealth.GetValue());
            autoDialogue.MessageMe(target.characterName + " receives " + amount + " health!");
        }
        else
        {
            Debug.Log("No matter how hard you try, you can never bring 'em back...");
            autoDialogue.MessageMe("No matter how hard you try, you can never bring 'em back...");

            AdjustHope(this, -hopeFlux * 4);
        }
    }

    // Peek Heal the target.
    public override int PeekHeal(CharacterStats target, int amount)
    {
        // Subtract the armor value - Make sure damage doesn't go below 0.
        amount += attack.GetValue() + (int)(Mathf.Clamp(hope * 0.1f, 0, int.MaxValue));
        amount = Mathf.Clamp(amount, 0, int.MaxValue);

        // Subtract damage from health
        Debug.Log(target.characterName + " will have " + (target.currentPeekDamage + amount) + " health.");

        //if (currentHealth - damage <= 0)
        //{
        //    Debug.Log(transform.name + " is in peril!");
        //}

        target.currentPeekDamage += amount;
        target.currentPeekDamage = Mathf.Clamp(target.currentPeekDamage, 0, target.maxHealth.GetValue());


        return currentPeekDamage;
    }

    public void AdjustHope(PlayerStats target, int value)
    {
        target.hope += value;
    }

    public override void GuardOn()
    {
        autoDialogue.MessageMe("Rich appears concerned.&&&% He stares pensively at " + characterName + "...");
        isGuarded = true;
    }

    public override void PermahealOn()
    {
        autoDialogue.MessageMe("Luna glows brilliantly&.&.&.&&& Suddenly, a bright light shrouds " + characterName + ".");
        isPermahealed = true;
    }


    public override void RoryOn(CharacterStats roryStats)
    {
        if (!isRoared)
        {
            // Player Dialogue.
            autoDialogue.MessageMe("He-HEY.");
            autoDialogue.MessageMe("You can't hurt my friends any longer... Yeah. That's right!");
            autoDialogue.MessageMe("I won't let you! You hear?");

            //if (!isRoared)
            //{
            //    if (rich.gameObject.activeSelf)
            //    {
            //        autoDialogue.MessageMe("Rich was taken aback by Rory's newfound demeanor.");
            //        autoDialogue.MessageMe("Rory?&&&% Is that even you?");
            //        autoDialogue.MessageMe("Rory leered fiercely at Rich.");
            //    }
            //    //
            //    isRoared = true;
            //}

            isRoared = true;
            //autoDialogue.MessageMe("Thank you for showing me how to be brave, Rich. This one's for you.");

            //autoDialogue.MessageMe("Rory expelled a battlecry as he slowly approached his enemy.");
            //autoDialogue.MessageMe("His eyes were of that of a lion's, waiting to claim its pray.");
            //autoDialogue.MessageMe("Bright. Brave. He found a fire inside of him that burned hotter than a Luchacabra's muscles.");
            autoDialogue.MessageMe("Pure fear inundated the enemy line.");
        }
    }

    public override void TurnOver()
    {
        if (isGuarded)
        {
            autoDialogue.MessageMe(characterName + " notices Rich staring. Rich conspicuously looks the other way and whistles.");
            isGuarded = false;
        }

        if (isPermahealed)
        {
            autoDialogue.MessageMe("The last of Luna's light...&&&&&% has faded.");
            isPermahealed = false;
        }

        if (isRoared)
        {
            isRoared = false;
        }
    }


}