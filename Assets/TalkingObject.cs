using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingObject : MonoBehaviour
{
    public DialogueTrigger dialogueTrigger;

    public DialogueManager dialogueManager;

    bool triggered;



    // Start is called before the first frame update
    void Start()
    {
    }


    public void Update()
    {
        if (triggered)
        {
            if (!GameControl.isFrozen && (Input.GetKeyDown("z") && !FindObjectOfType<DialogueManager>().inCutscene))
            {
                //dialogueTrigger.TriggerDialogue();
                dialogueManager.StartDialogue(dialogueTrigger.dialogue);

            }
        }
        //Debug.Log(GameControl.isFrozen);
    }



    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered");
        if (collision.gameObject.CompareTag("Leader"))
        {
            triggered = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {

        Debug.Log("Exited");
        if (collision.gameObject.CompareTag("Leader"))
        {
            triggered = false;
        }
    }
}
