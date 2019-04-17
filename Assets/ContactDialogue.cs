using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDialogue : MonoBehaviour
{
    public Dialogue dialogue;

    public DialogueManager dialogueManager;

    bool finished = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!finished && collision.tag == "Leader")
        {
            finished = true;
            dialogueManager.StartDialogue(dialogue);
        }
    }
}
