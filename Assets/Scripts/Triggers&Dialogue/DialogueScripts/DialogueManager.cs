using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    private Queue<Sprite> images;

    private TextMeshProUGUI dialogueText;


    public TextMeshProUGUI shortDialogueText;

    public TextMeshProUGUI longDialogueText;


    //public Animator animator;

    public Image image;

    public float timeDelay = 1.0f;

    private bool coroutineRunning = false;
    private string currentSentence;
    
    public bool inCutscene = false;

    //private Rigidbody2D rb;
    
    private MonoBehaviour[] scriptTriggers;

    public int lineSize = 38;

    public int bigLineSize = 38;
    public int smallLineSize = 0;

    private int currentLine = 0;
    private int lineOffset = 0;

    private int currentChar = 0;

    private Animator windowAnim;

    private bool hasImage = false;


    // Start is called before the first frame update
    void Awake()
    {
        sentences = new Queue<string>();
        images = new Queue<Sprite>();
        scriptTriggers = new MonoBehaviour[0];
        
    }


    void Start()
    {
        windowAnim = shortDialogueText.transform.parent.GetComponent<Animator>();
    }

    public void StartDialogue (Dialogue dialogue)
    {

        inCutscene = true;

        currentLine = 0;

        currentChar = 0;

        lineOffset = 0;

        windowAnim.SetBool("IsOpen", true);

        GameControl.FreezeOverworld();


        sentences = new Queue<string>();
        images = new Queue<Sprite>();
        scriptTriggers = new MonoBehaviour[0];


        //rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;


        //Debug.Log("Starting conversation with " + dialogue.name);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        foreach (Sprite picture in dialogue.face)
        {
            images.Enqueue(picture);
        }

        scriptTriggers = dialogue.scriptTriggers;

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        if (!coroutineRunning)
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }


            if (images.Peek() != null)
            {
                hasImage = true;
                image.sprite = images.Dequeue();
                image.gameObject.SetActive(true);
                shortDialogueText.gameObject.SetActive(true);
                longDialogueText.gameObject.SetActive(false);
                dialogueText = shortDialogueText;
            }
            else
            {
                hasImage = false;
                images.Dequeue();
                image.gameObject.SetActive(false);
                shortDialogueText.gameObject.SetActive(false);
                longDialogueText.gameObject.SetActive(true);
                dialogueText = longDialogueText;
            }
            currentLine = 0;

            currentChar = 0;

            lineOffset = 0;
            string sentence = sentences.Dequeue();
            //Debug.Log(sentence);
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));


        }
        else
        {
            dialogueText.text = "<mspace=1em>" + currentSentence;
            StopAllCoroutines();
            coroutineRunning = false;
        }




    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "<mspace=1em>";
        currentSentence = sentence;
        coroutineRunning = true;
        foreach (char letter in sentence.ToCharArray())
        {
            if (letter != '&' && letter != '%' && letter != '@')
            {
                dialogueText.text += letter;
            }
            if (letter == ' ')
            {
                string subStr = sentence.Substring(currentChar + 1);
                lineOffset = 0;
                bool escape = false;
                foreach (char letter2 in subStr.ToCharArray())
                {
                    if (!escape)
                    {
                        if (letter2 == ' ')
                        {
                            escape = true;
                        }
                        if (currentLine + lineOffset >= lineSize - 2)
                        {
                            dialogueText.text += '\n';
                            currentLine = -1;
                            escape = true;
                        }
                        if (letter != '&' && letter != '%')
                        {
                            lineOffset++;
                        }
                    }
                }
            }
            if (letter == '&')
            {
                yield return new WaitForSeconds(timeDelay);
                currentLine -= 1;

            }
            if (letter == '%')
            {
                dialogueText.text += '\n';
                currentLine = -1;
            }
            currentChar++;
            currentLine++;
            yield return new WaitForSeconds(timeDelay);
        }
        coroutineRunning = false;
    }

    public void EndDialogue()
    {
        //Debug.Log("End of Conversation");
        //animator.SetBool("IsOpen", false);
        //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        inCutscene = false;

        windowAnim.SetBool("IsOpen", false);
        GameControl.UnfreezeOverworld();




        if (scriptTriggers.Length > 0)
        {
            foreach (MonoBehaviour script in scriptTriggers)
            {
                if (script != null)
                {
                    script.enabled = true;
                }
            }
        }

    }


    void Update()
    {
        if (!GameControl.isBattling && (Input.GetKeyDown("z") || Input.GetKeyDown("x")))
        {
            DisplayNextSentence();
        }
    }
}
