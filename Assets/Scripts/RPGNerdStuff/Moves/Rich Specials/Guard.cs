using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "New Guard", menuName = "Attacks/Guard")]
public class Guard : Action
{
    public int percentage = 50;

    public override int Peek(CharacterStats player, CharacterStats target)
    {
        // Displays if attack is lethal to anyone.
        if (target != null)
        {
            return player.PeekDamage(99999, true);
        }
        else
        {
            return -1;
        }
    }


    public override int PeekPassive(CharacterStats player, CharacterStats target)
    {
        target.currentPeekDamage = target.currentHealth;
        return target.currentHealth;
    }



    public override void Perform(CharacterStats player, CharacterStats target)
    {
        // Displays if attack is lethal to anyone.
        if (target != null)
        {
            // Displays if attack is lethal to anyone.
            if (target != null)
            {
                target.GuardOn();
            }
        }
    }

    public override void PerformPassive(CharacterStats player, CharacterStats target)
    {
        
    }
}
