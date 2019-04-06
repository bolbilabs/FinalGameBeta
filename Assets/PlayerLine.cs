using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PlayerLine : MonoBehaviour
{
    GameObject leader;
    List<GameObject> followers = new List<GameObject>();
    
    List<GameObject> sortingList = new List<GameObject>();

    List<AnimObject> animList = new List<AnimObject>();

    public int spacingSize = 4;

    void Awake()
    {
        GameObject gameController = GameObject.FindWithTag("GameController");

        for (int i = 0; i < gameController.transform.childCount; i++)
        {
            if (!gameController.transform.GetChild(i).gameObject.activeSelf)
            {
                transform.GetChild(transform.childCount - 1).gameObject.SetActive(false);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        leader = transform.GetChild(0).gameObject;

        if (transform.childCount > 1)
        {
            for (int i = 1; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf)
                {
                    followers.Add(transform.GetChild(i).gameObject);
                }
            }
        }

        sortingList.Add(leader);
        sortingList.AddRange(followers);


    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //if (!leader.activeSelf)
        //{
        //    foreach (GameObject follower in followers)
        //    {
        //        if (follower.activeSelf)
        //        {
        //            leader = follower;
        //            followers.RemoveAt(followers.IndexOf(follower));
        //        }
        //    }
        //}


        Animator currentAnim = leader.GetComponent<PlayerMovements>().anim;

        if (animList.Count < 1 || Vector3.Distance(leader.transform.position, animList[animList.Count - 1].position) > 0.5f)
        {
            animList.Add(new AnimObject(leader.transform.position, currentAnim.GetFloat("MoveX"), currentAnim.GetFloat("MoveY"), currentAnim.GetBool("PlayerMoving"),
                                            currentAnim.GetFloat("LastX"), currentAnim.GetFloat("LastY")));

            for (int i = 0; i < followers.Count; i++)
            {
                if (!followers[i].activeSelf)
                {
                    followers.RemoveAt(i);
                }
                if (animList.Count > spacingSize + (spacingSize * i))
                {
                    AnimObject currentAnimObj = animList[animList.Count - (spacingSize + spacingSize * i)];

                    followers[i].transform.position = currentAnimObj.position;
                    Animator followAnim = followers[i].GetComponent<Animator>();

                    followAnim.SetFloat("MoveX", currentAnimObj.MoveX);
                    followAnim.SetFloat("MoveY", currentAnimObj.MoveY);
                    followAnim.SetBool("PlayerMoving", currentAnimObj.PlayerMoving);
                    followAnim.SetFloat("LastX", currentAnimObj.LastX);
                    followAnim.SetFloat("LastY", currentAnimObj.LastY);

                }
            }

            if (animList.Count > spacingSize + (spacingSize * (followers.Count - 1)))
            {
                animList.RemoveAt(0);
            }


            sortingList = sortingList.OrderByDescending(o => o.transform.position.y).ToList();

            for (int i = 0; i < sortingList.Count; i++)
            {
                sortingList[i].GetComponent<SpriteRenderer>().sortingOrder = i;
            }
        }
        else
        {
            for (int i = 0; i < followers.Count; i++)
            {
                Animator followAnim = followers[i].GetComponent<Animator>();

                followAnim.SetFloat("MoveX", 0f);
                followAnim.SetFloat("MoveY", 0f);
                followAnim.SetBool("PlayerMoving",false);
            }
            }

    }

    private class AnimObject {
        public Vector3 position; public float MoveX; public float MoveY; public bool PlayerMoving; public float LastX; public float LastY;

        public AnimObject(Vector3 position, float MoveX, float MoveY, bool PlayerMoving, float LastX, float LastY)
        {
            this.position = position;
            this.MoveX = MoveX;
            this.MoveY = MoveY;
            this.PlayerMoving = PlayerMoving;
            this.LastX = LastX;
            this.LastY = LastY;
        }
    }
}
