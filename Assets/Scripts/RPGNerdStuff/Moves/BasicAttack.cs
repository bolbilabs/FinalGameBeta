using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "New Basic Attack", menuName = "Attacks/Basic Attack")]
public class BasicAttack : Action
{
    public int baseDamage = 20;


    public override void Peek(CharacterStats player, CharacterStats[] target)
    {
        // Displays if attack is lethal to anyone.
        if (target.Length > 0)
        {
            if (player.PeekAttackTarget(target[0], baseDamage, false) <= 0)
            {
                // Show skull at target
            }
        }
    }

    public override void Perform(CharacterStats player, CharacterStats[] target)
    {
        if (target.Length > 0)
        {
            player.AttackTarget(target[0], baseDamage, false);
        }
    }
}
