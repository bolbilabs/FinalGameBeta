using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Explode Attack", menuName = "Attacks/Explode Attack")]
public class ExplodeAttack : Action
{
    public int enemyDamage = 100;
    public int selfDamage = 20;



    public override int Peek(CharacterStats player, CharacterStats target)
    {
        // Displays if attack is lethal to anyone.
        if (target != null)
        {
            return (player.PeekAttackTarget(target, enemyDamage, false));
        }
        else
        {
            return -1;
        }
    }


    public override int PeekPassive(CharacterStats player, CharacterStats target)
    {
        if (player != null)
        {
            return player.PeekDamage(selfDamage, false);
        }
        else
        {
            return -1;
        }
    }

    public override void Perform(CharacterStats player, CharacterStats target)
    {
        if (player != null && target != null)
        {
            player.AttackTarget(target, enemyDamage, false);
        }
    }


    public override void PerformPassive(CharacterStats player, CharacterStats target)
    {
        if (player != null && target != null)
        {
            player.TakeDamage(selfDamage, false);
        }
    }
}
