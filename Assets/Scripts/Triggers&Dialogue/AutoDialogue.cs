using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoDialogue : MonoBehaviour
{
    public List<string> sentences;
    //private Queue<Sprite> images;
    public List<string> bonusSentences;

    public TextMeshProUGUI dialogueText;

    public BattleManager battleManager;

    //public Animator animator;

    //public Image image;
    
    private float timeDelay = 0.05f;

    public float timeDelayNorm = 0.03f;
    public float timeDelayAlt = 0.007f;

    public float messagePause = 0.007f;


    private bool coroutineRunning = false;
    private string currentSentence;

    public bool inCutscene = false;

    //private Rigidbody2D rb;

    private MonoBehaviour[] scriptTriggers;

    public int lineSize = 38;

    private int currentLine = 0;
    private int lineOffset = 0;

    private int currentChar = 0;



    // List of characters, list of actions, list of triple objects
    public List<List<List<Object>>> actionOrder = new List<List<List<Object>>>();
    public List<List<List<Object>>> passiveOrder = new List<List<List<Object>>>();



    // Start is called before the first frame update
    void Awake()
    {
        sentences = new List<string>();
        //images = new Queue<Sprite>();
        scriptTriggers = new MonoBehaviour[0];

    }


    void Start()
    {
    }

    public void StartDialogue(List<GameObject> fightOrder, Dictionary<GameObject, List<List<Object>>> actionParams, Dictionary<GameObject, List<List<Object>>> peekParams)
    {
        //animator.SetBool("IsOpen", true);

        inCutscene = true;

        currentLine = 0;

        currentChar = 0;

        lineOffset = 0;


        //sentences = new List<string>();
        //images = new Queue<Sprite>();
        //scriptTriggers = new MonoBehaviour[0];

        actionOrder.Clear();
        passiveOrder.Clear();

        foreach (GameObject gObj in fightOrder)
        {
            //List<Object> comboList = new List<Object>();

            //comboList.AddRange(actionParams[gObj]);

            // Might be problematic down the line... We'll see.
            if (actionParams.ContainsKey(gObj))
            {
                actionOrder.Add(actionParams[gObj]);
            }
            else
            {
                actionOrder.Add(null);
            }

            if (peekParams.ContainsKey(gObj))
            {
                passiveOrder.Add(peekParams[gObj]);
            }
            else
            {
                passiveOrder.Add(null);
            }
        }
        

        foreach (List<List<Object>> objects in actionOrder)
        {
            //// First action's message has move description.
            //GameObject casterObj = (GameObject)(objects[0][0]);

            //if (casterObj.activeSelf)
            //{
                Action action = (Action)objects[0][2];

                sentences.Add(action.message);
            //}
        }


        //rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;


        //Debug.Log("Starting conversation with " + dialogue.name);

        //sentences.Clear();

        //foreach (string sentence in dialogue)
        //{
        //    sentences.Enqueue(sentence);

        //}

        //foreach (Sprite picture in dialogue.face)
        //{
        //    images.Enqueue(picture);
        //}

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        //// TODO: End fight after all enemies defeated.
        //bool foundEnabled = false;
        //foreach (GameObject enemies in battleManager.enemies)
        //{
        //    if (enemies.activeSelf)
        //    {
        //        foundEnabled = true;
        //    }
        //}

        //if (!foundEnabled && sentences.Count <= actionOrder[0].Count + passiveOrder[0].Count)
        //{
        //    EndDialogue();
        //}
        //Debug.Log("-----");
        //Debug.Log("sentences" + sentences.Count);
        //Debug.Log("bonus" + bonusSentences.Count);
        //Debug.Log("actionparams" + actionOrder.Count);
        //Debug.Log("passiveorder" + passiveOrder.Count);
        //Debug.Log("-----");


        if (sentences.Count > 0 && bonusSentences.Count <= 0)
        {
            if (!coroutineRunning)
            {
                string sentence = null;

           

                    //if (sentences.Count == 0)
                    //{
                    //    EndDialogue();
                    //    return;
                    //}


                    //if (images.Peek() != null)
                    //{
                    //    image.sprite = images.Dequeue();
                    //}
                    //else
                    //{
                    //    images.Dequeue();
                    //}
                    currentLine = 0;

                    currentChar = 0;

                    lineOffset = 0;
                    //string sentence = sentences.Dequeue();



                    if (actionOrder.Count > 0)
                    {
                        //List<List<Object>> objectsList = actionOrder[0];

                        //foreach (List<Object> objects in objectsList)
                        //{
                        //GameObject casterObj = (GameObject)objects[0];
                        //if (casterObj.activeSelf)
                        //{

                        GameObject casterObj = (GameObject)actionOrder[0][0][0];
                        if (casterObj.activeSelf)
                        {
                            sentence = sentences[0];
                        }
                        //}
                        //else
                        //{
                        //    actionOrder.RemoveAt(0);
                        //    passiveOrder.RemoveAt(0);
                        //}
                        //}
                    }
                    else
                    {
                        if (sentences.Count > 0)
                        {
                            sentence = sentences[0];
                        }
                    }

                    sentences.RemoveAt(0);



            

                bool foundActiveEnemy = false;
                foreach (GameObject checkEnemy in BattleManager.GetInstance().enemies)
                {
                    if (checkEnemy.activeSelf)
                    {
                        foundActiveEnemy = true;
                    }
                }
                if (foundActiveEnemy)
                {
                    //Debug.Log(sentence);
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(sentence));
                }
                else
                {
                    //MessageMe("&&&Y&&&O&&&U&&& &&&W&&&I&&&N&&&!&&&!&&&!&&&");
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence("&&&Y&&&O&&&U&&& &&&W&&&I&&&N&&&!&&&!&&&!&&&&&&&@"));
                }


            }
            else
            {
                dialogueText.text = "<mspace=1em>" + currentSentence;
                StopAllCoroutines();
                coroutineRunning = false;
            }
        } else if (bonusSentences.Count > 0)
        {
            //Debug.Log("BA" + bonusSentences.Count);
            string sentence = bonusSentences[0];
            bonusSentences.RemoveAt(0);
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }
        else
        {
            foreach (GameObject player in battleManager.players)
            {
                if (player.activeSelf)
                {
                    player.GetComponent<CharacterStats>().TurnOver();
                }
            }

            foreach (GameObject enemy in battleManager.enemies)
            {
                if (enemy.activeSelf)
                {
                    enemy.GetComponent<CharacterStats>().TurnOver();
                }
            }

            if (bonusSentences.Count > 0)
            {
                //Debug.Log("BA" + bonusSentences.Count);
                string sentence = bonusSentences[0];
                bonusSentences.RemoveAt(0);
                StopAllCoroutines();
                StartCoroutine(TypeSentence(sentence));
            }
            else
            {
                bool foundActiveEnemy = false;
                foreach (GameObject checkEnemy in BattleManager.GetInstance().enemies)
                {
                    if (checkEnemy.activeSelf)
                    {
                        foundActiveEnemy = true;
                    }
                }


                if (!foundActiveEnemy)
                {
                    //MessageMe("&&&Y&&&O&&&U&&& &&&W&&&I&&&N&&&!&&&!&&&!&&&");
                    StopAllCoroutines();
                    StartCoroutine(TypeSentence("&&&Y&&&O&&&U&&& &&&W&&&I&&&N&&&!&&&!&&&!&&&&&&&@"));
                }
                else
                {
                    EndDialogue();
                }
            }
        }

    }

    IEnumerator TypeSentence(string sentence)
    {
        if (sentence != null)
        {
            dialogueText.text = "<mspace=1em>";
            currentSentence = sentence;
            Debug.Log(currentSentence);
            coroutineRunning = true;
            currentChar = 0;
            currentLine = 0;
            foreach (char letter in sentence.ToCharArray())
            {
                if (letter != '&' && letter != '%' && letter != '@')
                {
                    dialogueText.text += letter;
                }
                if (letter == ' ')
                {
                    //Debug.Log(currentChar);

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
                if (letter == '@')
                {
                    GameControl.EndBattle();
                    yield return new WaitForSeconds(10);
                }
                currentChar++;
                currentLine++;
                yield return new WaitForSeconds(timeDelay);
            }
            coroutineRunning = false;
            yield return new WaitForSeconds((timeDelay * messagePause));
            if (bonusSentences.Count <= 0 && sentences.Count < actionOrder.Count)
            {
                if (actionOrder.Count > 0)
                {
                    List<List<Object>> objectsList = actionOrder[0];
                    actionOrder.RemoveAt(0);

                    if (objectsList != null)
                    {
                        foreach (List<Object> objects in objectsList)
                        {
                            GameObject casterObj = (GameObject)objects[0];
                            GameObject targetObj = (GameObject)objects[1];


                            CharacterStats casterStats = casterObj.GetComponent<CharacterStats>();
                            CharacterStats targetStats = targetObj.GetComponent<CharacterStats>();
                            Action action = (Action)objects[2];

                            int deathCount = 0;
                            foreach (GameObject player in battleManager.players)
                            {
                                if (!player.activeSelf)
                                {
                                    deathCount++;
                                }
                            }

                            if (casterObj.activeSelf)
                            {
                                if (battleManager.players.Contains(casterObj))
                                {
                                    battleManager.healthBlocks[battleManager.players.IndexOf(casterObj)].GetComponent<Animator>().SetInteger("State", 0);
                                }
                                action.Perform(casterStats, targetStats);

                                foreach (GameObject player in battleManager.players)
                                {
                                    CharacterStats currStats = player.GetComponent<CharacterStats>();
                                    battleManager.healthBlocks[battleManager.players.IndexOf(player)].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(currStats.characterName + "\n" + currStats.currentHealth + "/" + currStats.maxHealth.GetValue());
                                    battleManager.healthBlocks[battleManager.players.IndexOf(player)].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Slider>().value = (float)(((float)currStats.currentHealth) / ((float)currStats.maxHealth.GetValue()));

                                }

                            }
                        }
                    }
                }

                if (passiveOrder.Count > 0)
                {
                    List<List<Object>> objectsList = passiveOrder[0];
                    passiveOrder.RemoveAt(0);

                    if (objectsList != null)
                    {

                        foreach (List<Object> objects in objectsList)
                        {
                            GameObject casterObj = (GameObject)objects[0];
                            GameObject targetObj = (GameObject)objects[1];


                            CharacterStats casterStats = casterObj.GetComponent<CharacterStats>();
                            CharacterStats targetStats = targetObj.GetComponent<CharacterStats>();
                            Action action = (Action)objects[2];
                            //Debug.Log("TESTING!");
                            action.PerformPassive(casterStats, targetStats);

                            foreach (GameObject player in battleManager.players)
                            {
                                CharacterStats currStats = player.GetComponent<CharacterStats>();

                                battleManager.healthBlocks[battleManager.players.IndexOf(player)].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(currStats.characterName + "\n" + currStats.currentHealth + "/" + currStats.maxHealth.GetValue());
                                battleManager.healthBlocks[battleManager.players.IndexOf(player)].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetComponent<Slider>().value = (float)(((float)currStats.currentHealth) / ((float)currStats.maxHealth.GetValue()));

                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (actionOrder.Count > 0)
            {
                actionOrder.RemoveAt(0);
            }
            if (passiveOrder.Count> 0)
            {
                passiveOrder.RemoveAt(0);
            }
        }


        RecheckActive();


        //Debug.Log(actionOrder.Count);
        //Debug.Log(passiveOrder.Count);
        //Debug.Log(sentences.Count);
        //Debug.Log("");

        DisplayNextSentence();
    }

    public void EndDialogue()
    {
        
         



        
        //Debug.Log("End of Conversation");
        //animator.SetBool("IsOpen", false);
        //rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        inCutscene = false;
        battleManager.Start();


        //if (scriptTriggers.Length > 0)
        //{
        //    foreach (MonoBehaviour script in scriptTriggers)
        //    {
        //        if (script != null)
        //        {
        //            script.enabled = true;
        //        }
        //    }
        //}

    }

    void RecheckActive ()
    {
        if (sentences.Count == actionOrder.Count && sentences.Count == passiveOrder.Count)
        {
            for (int i = 0; i < actionOrder.Count; i++)
            {
                GameObject casterObject = (GameObject)actionOrder[i][0][0];

                if (!casterObject.activeSelf)
                {
                    actionOrder.RemoveAt(i);
                    passiveOrder.RemoveAt(i);
                    sentences.RemoveAt(i);
                }
            }
        }

        if (sentences.Count > actionOrder.Count)
        {
            for (int i = 0; i < actionOrder.Count; i++)
            {
                GameObject casterObject = (GameObject)actionOrder[i][0][0];

                if (!casterObject.activeSelf)
                {
                    sentences.RemoveAt(i + (sentences.Count-actionOrder.Count));
                }
            }
        }
    }

    void Update()
    {
        if (inCutscene)
        {
            timeDelay = timeDelayNorm;
            if (Input.GetKey("z") || Input.GetKey("x"))
            {
                //DisplayNextSentence();
                timeDelay = timeDelayAlt;
                if (Input.GetKey("z") && Input.GetKey("x"))
                {
                   timeDelay = 0.0001f;
                }
            }
        }
    }

    public void MessageMe (string sentenceAdd)
    {
        bonusSentences.Add(sentenceAdd);
    }
}
