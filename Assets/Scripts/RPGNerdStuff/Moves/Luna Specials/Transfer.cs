using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "New Transfer Health", menuName = "Attacks/Transfer Health")]
public class Transfer : Action
{
    public int baseDamage = 20;
    public int passiveDamage = 20;
    public bool takeFromAlly = false;

    public override int Peek(CharacterStats player, CharacterStats target)
    {
        // Displays if attack is lethal to anyone.
        if (target != null)
        {
            if (!takeFromAlly)
            {
                return player.PeekHeal(target, baseDamage);
            }
            else
            {
                return player.PeekHeal(player, baseDamage);
            }
        }
        else
        {
            return -1;
        }
    }


    public override int PeekPassive(CharacterStats player, CharacterStats target)
    {
        // Displays if attack is lethal to anyone.
        if (target != null)
        {
            if (!takeFromAlly)
            {
                return player.PeekAttackTarget(player, passiveDamage, false);
            }
            else
            {
                return player.PeekAttackTarget(target, passiveDamage, false);
            }
        }
        else
        {
            return -1;
        }
    }



    public override void Perform(CharacterStats player, CharacterStats target)
    {
        // Displays if attack is lethal to anyone.
        if (target != null)
        {
            if (!takeFromAlly)
            {
                player.Heal(target, baseDamage);
            }
            else
            {
                player.AttackTarget(target, passiveDamage, false);
            }
        }
    }

    public override void PerformPassive(CharacterStats player, CharacterStats target)
    {
        // Displays if attack is lethal to anyone.
        if (target != null)
        {
            if (!takeFromAlly)
            {
                player.AttackTarget(player, passiveDamage, false);

            }
            else
            {
                player.Heal(player, baseDamage);

            }
        }
    }
}
