using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New AOE Attack", menuName = "Attacks/AOE Attack")]
public class AOEAll : Action
{
    public int baseDamage = 20;


    public override int Peek(CharacterStats player, CharacterStats target)
    {
        // Displays if attack is lethal to anyone.
        if (target != null)
        {
            return player.PeekAttackTarget(target, baseDamage, false);
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
            player.AttackTarget(target, baseDamage, false);
        }
    }
}
