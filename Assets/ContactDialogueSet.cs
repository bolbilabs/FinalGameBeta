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
    public DialogueManager dialogueManager;

    Dialogue dialogue = new Dialogue();



    bool finished = false;


    private string[] dialogue1 =
        {
        "Now " + GameControl.finalCharacter + " is all alone.",
        "#Or so you may think."
        };
    private string[] dialogue2 =
        {
        "There is a way to save " + GameControl.finalCharacter + ".",
        "Something that couldn't have been done before."
        };
    private string[] dialogue3 =
        {
        "#&&&.&&&.&&&.",
        "#As it turns out, there's another party member still alive..."
        };


    private string[] finalDialogue =
    {
        "#" + GameControl.YourName + ".",
        "#Yeah, you. I'm speaking to you.",
        "You happen to have an \"Options\" menu of your own.",
        "If you hit the escape button during your final fight.",
        "In your Options menu, you have only one move...",
        "\"Save.\"",
        "This means you save the game.",
        "By saving the game before a fatal blow, you prevent your last party member from falling...",
        "#But it comes with a price.",
        "#I can't guarantee that you will make it.",
        "But it's up to you... Will you save " + GameControl.finalCharacter + " or see this through?",
        "#It's your move, " + GameControl.YourName + "."
    };


    // Start is called before the first frame update
    void Start()
    {

        dialogue1[0] = "Now " + GameControl.finalCharacter + " is all alone.";

        dialogue2[0] = "There is a way to save " + GameControl.finalCharacter + ".";

        finalDialogue[0] = "#&&&&&" + GameControl.YourName + ".";

        finalDialogue[10] = "But it's up to you... Will you save " + GameControl.finalCharacter + " or see this through?";
        finalDialogue[11] = "#It's your move, " + GameControl.YourName + ".";




        if (chooseIt == 0)
        {
            dialogue.face = new Sprite[dialogue1.Length];
            dialogue.clips = new AudioClip[dialogue1.Length];
            for (int i = 0; i < dialogue1.Length; i++)
            {
                dialogue.clips[i] = clip;
            }
            dialogue.sentences = dialogue1;
            dialogue.scriptTriggers = new MonoBehaviour[0];

        }
        else if (chooseIt == 1)
        {
            dialogue.face = new Sprite[dialogue2.Length];
            dialogue.clips = new AudioClip[dialogue2.Length];
            for (int i = 0; i < dialogue2.Length; i++)
            {
                dialogue.clips[i] = clip;
            }
            dialogue.sentences = dialogue2;
            dialogue.scriptTriggers = new MonoBehaviour[0];

        }
        else if (chooseIt == 2)
        {
            dialogue.face = new Sprite[dialogue3.Length];
            dialogue.clips = new AudioClip[dialogue3.Length];
            for (int i = 0; i < dialogue3.Length; i++)
            {
                dialogue.clips[i] = clip;
            }
            dialogue.sentences = dialogue3;
            dialogue.scriptTriggers = new MonoBehaviour[0];

        }
        else if (chooseIt == 3)
        {
            dialogue.face = new Sprite[finalDialogue.Length];
            dialogue.clips = new AudioClip[finalDialogue.Length];
            for (int i = 0; i < finalDialogue.Length; i++)
            {
                dialogue.clips[i] = clip;
            }
            dialogue.sentences = finalDialogue;
            dialogue.scriptTriggers = new MonoBehaviour[1];
            dialogue.scriptTriggers[0] = startScene;
        }


    }


    // Update is called once per frame
    void Update()
    {
        if (!once && fadeOut.effectFinished && chooseIt == 3)
        {
            once = true;
            dialogueManager.StartDialogue(dialogue);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!finished && collision.tag == "Leader")
        {
            finished = true;

            if (chooseIt == 3)
            {
                fadeOut.enabled = true;
            }
            else
            {
                dialogueManager.StartDialogue(dialogue);
            }
        }
    }
}
