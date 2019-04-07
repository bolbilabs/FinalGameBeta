using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialChildren : MonoBehaviour
{
    public List<GameObject> dialChildren;
    public Animator animator;

    BattleManager battleManager;

    public bool spaceBool = false; 

    private string[] topMenuVars = { "Attack!", "Specials!", "Options!" };

    public void Start()
    {
        battleManager = BattleManager.GetInstance();
    }

    Color altColor = new Color(0, 255, 100);
    Color inactiveColor = new Color(0,132,119);
    Color activeColor = new Color(0, 255, 230);


    public void OnAnimationEvent(int direction)
    {
        //spaceBool = true;
        //if (battleManager != null)
        //{
        //    Debug.Log("TEST TIMG: " + battleManager.currentPlayer);
        //}
        //dialChildren.RemoveAt(0);


        //animator.SetInteger("Direction", 0);


        //if (battleManager.menuState == 0)
            //{
            //        dialChildren[0].GetComponentInChildren<TextMeshProUGUI>().SetText(topMenuVars[mod(battleManager.currentTopAction + 2, topMenuVars.Length)]);
            //        dialChildren[1].GetComponentInChildren<TextMeshProUGUI>().SetText(topMenuVars[mod(battleManager.currentTopAction + 1, topMenuVars.Length)]);
            //        dialChildren[2].GetComponentInChildren<TextMeshProUGUI>().SetText(topMenuVars[battleManager.currentTopAction]);
            //        dialChildren[3].GetComponentInChildren<TextMeshProUGUI>().SetText(topMenuVars[mod(battleManager.currentTopAction - 1, topMenuVars.Length)]);
            //        dialChildren[4].GetComponentInChildren<TextMeshProUGUI>().SetText(topMenuVars[mod(battleManager.currentTopAction - 2, topMenuVars.Length)]);
            //} else if (battleManager.menuState == 1)
            //{
            //    Action[] specialAttacks = battleManager.players[battleManager.currentPlayer].GetComponent<PlayerController>().specialAttack;

            //    dialChildren[0].GetComponentInChildren<TextMeshProUGUI>().SetText(specialAttacks[mod(battleManager.currentSubAction + 2, specialAttacks.Length)].moveName);
            //    dialChildren[1].GetComponentInChildren<TextMeshProUGUI>().SetText(specialAttacks[mod(battleManager.currentSubAction + 1, specialAttacks.Length)].moveName);
            //    dialChildren[2].GetComponentInChildren<TextMeshProUGUI>().SetText(specialAttacks[battleManager.currentSubAction].moveName);
            //    dialChildren[3].GetComponentInChildren<TextMeshProUGUI>().SetText(specialAttacks[mod(battleManager.currentSubAction - 1, specialAttacks.Length)].moveName);
            //    dialChildren[4].GetComponentInChildren<TextMeshProUGUI>().SetText(specialAttacks[mod(battleManager.currentSubAction - 2, specialAttacks.Length)].moveName);
            //}



        //spaceBool = true;
    }

    public void LateUpdate()
    {
        //if (spaceBool)
        //{
        //    animator.SetInteger("Direction", 0);
        //    spaceBool = false;
        //}
    }

    public void FixedUpdate()
    {
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        //AnimatorClipInfo[] myAnimatorClip = animator.GetCurrentAnimatorClipInfo(0);
        //float myTime = myAnimatorClip[0].clip.length * animationState.normalizedTime;

        if (spaceBool || (animationState.normalizedTime >= 1 && animator.GetInteger("Direction") != 0))
        {
            dialChildren[0].SetActive(true);

            animator.SetInteger("Direction", 0);


            if (battleManager.menuState == 0)
            {
                dialChildren[0].GetComponentInChildren<TextMeshProUGUI>().SetText(topMenuVars[mod(battleManager.currentTopAction - 2, topMenuVars.Length)]);
                dialChildren[1].GetComponentInChildren<TextMeshProUGUI>().SetText(topMenuVars[mod(battleManager.currentTopAction - 1, topMenuVars.Length)]);
                dialChildren[2].GetComponentInChildren<TextMeshProUGUI>().SetText(topMenuVars[battleManager.currentTopAction]);
                dialChildren[3].GetComponentInChildren<TextMeshProUGUI>().SetText(topMenuVars[mod(battleManager.currentTopAction + 1, topMenuVars.Length)]);
                dialChildren[4].GetComponentInChildren<TextMeshProUGUI>().SetText(topMenuVars[mod(battleManager.currentTopAction + 2, topMenuVars.Length)]);

                //dialChildren[2].GetComponent<Image>().color = activeColor;

            }
            else if (battleManager.menuState == 1)
            {
                Action[] specialAttacks = null;

                if (battleManager.currentTopAction == 1)
                {
                    specialAttacks = battleManager.players[battleManager.currentPlayer].GetComponent<PlayerController>().specialAttack;
                }
                else if (battleManager.currentTopAction == 2)
                {
                    specialAttacks = battleManager.players[battleManager.currentPlayer].GetComponent<PlayerController>().assist;
                }

                if (specialAttacks != null)
                {
                    dialChildren[0].GetComponentInChildren<TextMeshProUGUI>().SetText(specialAttacks[mod(battleManager.currentSubAction - 2, specialAttacks.Length)].moveName);
                    dialChildren[1].GetComponentInChildren<TextMeshProUGUI>().SetText(specialAttacks[mod(battleManager.currentSubAction - 1, specialAttacks.Length)].moveName);
                    dialChildren[2].GetComponentInChildren<TextMeshProUGUI>().SetText(specialAttacks[battleManager.currentSubAction].moveName);
                    dialChildren[3].GetComponentInChildren<TextMeshProUGUI>().SetText(specialAttacks[mod(battleManager.currentSubAction + 1, specialAttacks.Length)].moveName);
                    dialChildren[4].GetComponentInChildren<TextMeshProUGUI>().SetText(specialAttacks[mod(battleManager.currentSubAction + 2, specialAttacks.Length)].moveName);
                }

                //dialChildren[2].GetComponent<Image>().color = activeColor;

            } else if (battleManager.menuState == 2)
            {
                if (battleManager.currentTopAction == 0) {
                    dialChildren[2].GetComponentInChildren<TextMeshProUGUI>().SetText(topMenuVars[0]);
                }
            }
            spaceBool = false;



        }

    }

    public void ForceUpdate()
    {
        spaceBool = true;
    }

    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
