using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class Action : ScriptableObject
{
    public string moveName = "New Action";
    public bool consumable = false;

    // This is for the BattleManager to interpret targets.
    public bool targetsAllEnemies = false;
    public bool targetsAllAllies = false;
    // Passively targets self. Like self-damage or healing.
    public bool targetsSelfPassive = false;
    // You can't actively target yourself. Usually true.
    public bool cannotTargetSelf = true;
    // It's a self-move. Like Hope.
    public bool onlyTargetsSelf = false;

    // Info text
    [TextArea(3, 10)]
    public string description;

    // Flavor text
    [TextArea(3, 10)]
    public string[] messages;

    public virtual void Peek (CharacterStats player, CharacterStats[] target)
    {
        // Displays if attack is lethal to anyone.
    }

    public virtual void Perform (CharacterStats player, CharacterStats[] target)
    {
        // Broadcast message

        // Perform 
    }
}
