using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


[CreateAssetMenu(fileName = "New Action", menuName = "Attacks/Action")]
public class Action : ScriptableObject
{
    public string moveName = "New Action";
    public bool consumable = false;

    // This is for the BattleManager to interpret targets.
    public bool targetsAllEnemiesPassive = false;
    public bool targetsAllAlliesPassive = false;

    // Move targets all of a type
    public bool targetsAllEnemies = false;
    public bool targetsAllAllies = false;

    // Can only select target from this type
    public bool onlyTargetsEnemies = false;
    public bool onlyTargetsAllies = false;

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
    public string message;

    public int priority = 0;


    public Dialogue hopeDialogue;


    public virtual int Peek (CharacterStats player, CharacterStats target)
    {
        // Displays if attack is lethal to active target.
        return -1;
    }

    public virtual int PeekPassive(CharacterStats player, CharacterStats target)
    {
        // Displays if attack is lethal to passive target.
        return -1;
    }

    public virtual void Perform (CharacterStats player, CharacterStats target)
    {
        // Broadcast message But it's hopeless!

        // Perform 
        if (moveName == "Hope")
        {
            if (!GameControl.finalBattle)
            {
                GameObject.FindWithTag("BattleManager").GetComponent<AutoDialogue>().MessageMe("But it's hopeless!");
            }
            else
            {
                GameControl.hopingHero = this;
                GameControl.characterOut = player.gameObject;
                GameObject.FindWithTag("Enemy").GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 25);
                GameObject.FindWithTag("BattleManager").GetComponent<AutoDialogue>().MessageMe("~...");
            }
        }

    }

    public virtual void PerformPassive(CharacterStats player, CharacterStats target)
    {
        // Broadcast message

        // Perform 
    }
}
