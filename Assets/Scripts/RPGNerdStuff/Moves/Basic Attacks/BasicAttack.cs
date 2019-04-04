using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "New Basic Attack", menuName = "Attacks/Basic Attack")]
public class BasicAttack : Action
{
    public int baseDamage = 20;
    public int subDamage = 20;


    public override int Peek(CharacterStats player, CharacterStats target)
    {
        // Displays if attack is lethal to anyone.
        if (target != null)
        {
            if (target.GetType() == typeof(PlayerStats))
            {
                return player.PeekAttackTarget(target, subDamage, false);

            }
            else
            {
                return player.PeekAttackTarget(target, baseDamage, false);
            }
        }
        else
        {
            return -1;
        }
    }


    public override int PeekPassive(CharacterStats player, CharacterStats target)
    {
        return -1;
    }



    public override void Perform(CharacterStats player, CharacterStats target)
    {
        if (player != null && target != null)
        {
            if (target.GetType() == typeof(PlayerStats))
            {
                player.AttackTarget(target, subDamage, false);
            }
            else
            {
                player.AttackTarget(target, baseDamage, false);

            }
        }
    }
}
