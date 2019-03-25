using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Action basicAttack;

    public Action[] specialAttack;

    public Action[] items;

    public Action[] assist;

    //public CharacterStats player;
    //public CharacterStats enemy;


    void Update ()
    {
        //if (Input.GetKeyDown("t"))
        //{
        //    Debug.Log("Old Enemy Health: " + enemy.currentHealth);
        //    basicAttack.Perform(player, enemy);
        //    Debug.Log("New Enemy Health: " + enemy.currentHealth);

        //}
    }
}
