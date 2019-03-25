using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Base class for all stats: health, armor, damage etc. */
/* NOTE: This represents a single stat for a player character or enemy.
    This means it's the stat with equipment/item modifiers */

[System.Serializable]
public class Stat
{

    public int baseValue;   // Starting value

    // Keep a list of all the modifiers on this stat
    private List<int> modifiers = new List<int>();

    // Add all modifiers together and return the result
    public int GetValue()
    {
        int finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }

    // Returns JUST the base value. For special attacks that ignore modifiers.
    public int GetValueNoMods()
    {
        return baseValue;
    }

    // Add a new modifier to the list
    public void AddModifier(int modifier)
    {
        if (modifier != 0)
            modifiers.Add(modifier);
    }

    // Remove a modifier from the list
    public void RemoveModifier(int modifier)
    {
        if (modifier != 0)
            modifiers.Remove(modifier);
    }

}