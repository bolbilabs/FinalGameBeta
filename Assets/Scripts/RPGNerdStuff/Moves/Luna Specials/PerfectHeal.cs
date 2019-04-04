using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "New Perfect Heal", menuName = "Attacks/Perfect Heal")]
public class PerfectHeal : Action
{

    public override int Peek(CharacterStats player, CharacterStats target)
    {
        // Displays if attack is lethal to anyone.
        if (target != null)
        {
            return target.maxHealth.GetValue();
        }
        else
        {
            return -1;
        }
    }


    public override int PeekPassive(CharacterStats player, CharacterStats target)
    {
        // Displays if attack is lethal to anyone.
        if (player != null)
        {
            return player.PeekDamage(99999,true);
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
            target.PermahealOn();
            player.Heal(target, 9999);
        }
    }

    public override void PerformPassive(CharacterStats player, CharacterStats target)
    {
        player.TakeDamage(99999, true);
    }
}
