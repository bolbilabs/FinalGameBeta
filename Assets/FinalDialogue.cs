using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDialogue : MonoBehaviour
{
    public DialogueManager dialogueManager;

    public MonoBehaviour[] finale;

    Dialogue dialogue = new Dialogue();

    int pizza = 0;

    bool once = false;

    // Start is called before the first frame update
    void Awake()
    {
        dialogue.scriptTriggers = finale;


        dialogue.sentences = new string[1];
        dialogue.sentences[0] = "#&&&" + GameControl.YourName + "&&&&& has saved the game.";

        dialogue.clips = new AudioClip[1];
        dialogue.face = new Sprite[1];


    }

    // Update is called once per frame
    void Update()
    {
        if (!once && pizza > 100)
        {
            once = true;
            dialogueManager.StartDialogue(dialogue);
        }
        pizza++;
    }
}
