using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDialogue : MonoBehaviour
{
    public Dialogue dialogue;

    public DialogueManager dialogueManager;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Leader")
        {
            dialogueManager.StartDialogue(dialogue);
        }
    }
}
