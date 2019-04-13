using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavesParty : MonoBehaviour
{
    private GameObject leader;
    private Dialogue leaves = new Dialogue();
    public DialogueManager end;



    // Start is called before the first frame update
    void Start()
    {
        leader = GameObject.FindWithTag("Leader");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameControl.downCut2 && Vector2.Distance(gameObject.transform.position, leader.transform.position) > 600)
        {
            string[] text = { "#" + GameControl.characterOut.GetComponent<PlayerStats>().characterName + "&&& has left the party." };
            leaves.sentences = text;
            leaves.face = new Sprite[1];
            leaves.clips = new AudioClip[1];
            leaves.scriptTriggers = new MonoBehaviour[0];
            end.StartDialogue(leaves);
            GameControl.downCut2 = false;
            gameObject.SetActive(false);
        }
    }
}
