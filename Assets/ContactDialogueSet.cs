using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactDialogueSet : MonoBehaviour
{
    public AudioClip clip;
    public int chooseIt;
    public FadeOut fadeOut;
    private bool once = false;

    public StartScene startScene;

    //private string[] dialogue1 =
    //    {
    //    "Now " + GameObject.FindWithTag("Player").GetComponent<PlayerStats>().characterName + " is all alone.",
    //    "#Or so you may think."
    //    };
    //private string[] dialogue2 =
    //    {
    //    "There is a way to save " + GameObject.FindWithTag("Player").GetComponent<PlayerStats>().characterName + ".",
    //    "Something that couldn't have been done before."
    //    };
    //private string[] dialogue3 =
    //    {
    //    "#&&&.&&&.&&&.",
    //    "#As it turns out, there's another party member still alive..."
    //    };


    //private string[] finalDialogue =
    //{
    //    "#" + GameControl.YourName + ".",
    //    "#Yeah, you. I'm speaking to you.",
    //    "You happen to have an \"Options\" menu of your own",
    //    "If you hit the escape button.",
    //    "In your Options menu, you have only one move...",
    //    "\"Save.\"",
    //    "This means you save the game.",
    //    "By saving the game before a fatal blow, you prevent your last party member from falling...",
    //    "#But it comes with a price.",
    //    "Feel free to use it once. Just once.",
    //    "You will most likely be crippled by it, but you'll save " + GameObject.FindWithTag("Player").GetComponent<PlayerStats>().characterName + ".",
    //    "Use it a second time, and... well.",
    //    "#I can't guarentee that you will make it.",
    //    "But it's up to you... Will you save " + GameObject.FindWithTag("Player").GetComponent<PlayerStats>().characterName + " or see this through?",
    //    "#It's your move, " + GameControl.YourName + "."
    //};


    // Start is called before the first frame update
    void Start()
    {
        //Dialogue dialogue = new Dialogue();
        //if (chooseIt == 0)
        //{
        //    dialogue.face = new Sprite[dialogue1.Length];
        //    dialogue.clips = new AudioClip[dialogue1.Length];
        //    for (int i = 0; i < dialogue1.Length; i++)
        //    {
        //        dialogue.clips[i] = clip;
        //    }
        //    dialogue.sentences = dialogue1;
        //} else if (chooseIt == 1)
        //{
        //    dialogue.face = new Sprite[dialogue2.Length];
        //    dialogue.clips = new AudioClip[dialogue2.Length];
        //    for (int i = 0; i < dialogue2.Length; i++)
        //    {
        //        dialogue.clips[i] = clip;
        //    }
        //    dialogue.sentences = dialogue2;
        //}
        //else if (chooseIt == 2)
        //{
        //    dialogue.face = new Sprite[dialogue3.Length];
        //    dialogue.clips = new AudioClip[dialogue3.Length];
        //    for (int i = 0; i < dialogue3.Length; i++)
        //    {
        //        dialogue.clips[i] = clip;
        //    }
        //    dialogue.sentences = dialogue3;
        //} else if (chooseIt == 3)
        //{
        //    fadeOut.enabled = true;
        //}


    }


    // Update is called once per frame
    void Update()
    {
        //if (!once && fadeOut.effectFinished)
        //{
        //    once = true;
        //    Dialogue dialogue = new Dialogue();
        //    dialogue.face = new Sprite[finalDialogue.Length];
        //    dialogue.clips = new AudioClip[finalDialogue.Length];
        //    for (int i = 0; i < finalDialogue.Length; i++)
        //    {
        //        dialogue.clips[i] = clip;
        //    }
        //    dialogue.sentences = finalDialogue;
        //    dialogue.scriptTriggers = new MonoBehaviour[1];
        //    dialogue.scriptTriggers[0] = startScene;
        //}
    }
}
