using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerDialogueStart : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;

    public DialogueManager dialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager.StartDialogue(dialogueTrigger.dialogue);
    }
}
