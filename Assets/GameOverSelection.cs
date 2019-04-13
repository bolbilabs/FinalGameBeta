using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverSelection : MonoBehaviour
{
    private bool quitSelected = false;
    private bool optionSelected = false;
    public DialogueManager dialogueManager;
    public Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!optionSelected)
        {
            if (Input.GetKeyDown("z"))
            {
                if (!quitSelected)
                {
                    optionSelected = true;
                    //gameObject.SetActive(false);
                    for (int i = 0; i < gameObject.transform.childCount; i++)
                    {
                        gameObject.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    dialogueManager.StartDialogue(dialogue);
                }
                else if (quitSelected)
                {
                    optionSelected = true;
                    Application.Quit();
                }
            }

            if (Input.GetKeyDown("left") || Input.GetKeyDown("right") || Input.GetKeyDown("up") || Input.GetKeyDown("down"))
            {
                quitSelected = !quitSelected;

                if (quitSelected)
                {
                    transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                    transform.GetChild(1).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;

                    transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = new Color32(97, 225, 108, 255);
                    transform.GetChild(2).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
                }
                else
                {
                    transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = new Color32(255, 255, 255, 255);
                    transform.GetChild(2).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Normal;

                    transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color32(97, 225, 108, 255);
                    transform.GetChild(1).GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Underline;
                }
            }
        }
    }
}
