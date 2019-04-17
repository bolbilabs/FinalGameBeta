using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDialogue : MonoBehaviour
{
    public DialogueManager dialogueManager;

    public MonoBehaviour[] finale;

    // Start is called before the first frame update
    void Start()
    {
        Dialogue dialogue = new Dialogue();
        dialogue.sentences = new string[1];
        dialogue.sentences[0] = "#&&&" + GameControl.YourName + "&&&&&&&&&&&& has saved the game.";

        dialogue.clips = new AudioClip[1];
        dialogue.face = new Sprite[1];
        dialogue.scriptTriggers = finale;

        dialogueManager.StartDialogue(dialogue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
