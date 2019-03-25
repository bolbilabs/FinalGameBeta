using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerDialogueStart : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;


    // Start is called before the first frame update
    void Start()
    {
        dialogueTrigger.TriggerDialogue();
    }
}
