using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
	public float moveSpeed;
	public Animator anim;
	private bool playerMoving;
	private Vector2 lastMove;

    public List<GameObject> players = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator>();

        GameObject gameController = GameObject.FindWithTag("GameController").gameObject;


        //string characterName;

        for (int i = 0; i < gameController.transform.childCount; i++)
        {
            if (gameController.transform.GetChild(i).gameObject.activeSelf)
            {
                players.Add(gameController.transform.GetChild(i).gameObject);
            }
            //characterName = gameController.transform.GetChild(i).gameObject.GetComponent<PlayerStats>().characterName;

        }

        for (int i = 0; i < players.Count; i++)
        {
            string currentName = players[i].GetComponent<PlayerStats>().characterName;

            if (currentName == "Paul")
            {
                GameObject.FindWithTag("Players").transform.GetChild(i).GetComponent<Animator>().SetBool("isPaul", true);
            }
            else if (currentName == "Luna")
            {
                GameObject.FindWithTag("Players").transform.GetChild(i).GetComponent<Animator>().SetBool("isLuna", true);
            }
            else if (currentName == "Rich")
            {
                GameObject.FindWithTag("Players").transform.GetChild(i).GetComponent<Animator>().SetBool("isRich", true);
            }
            else if (currentName == "Rory")
            {
                GameObject.FindWithTag("Players").transform.GetChild(i).GetComponent<Animator>().SetBool("isRory", true);
            }
        }

        for (int i = 0; i < GameObject.FindWithTag("Players").transform.childCount; i++)
        {
            if (!GameObject.FindWithTag("Players").transform.GetChild(i).GetComponent<Animator>().GetBool("isPaul") &&
                !GameObject.FindWithTag("Players").transform.GetChild(i).GetComponent<Animator>().GetBool("isLuna") &&
                !GameObject.FindWithTag("Players").transform.GetChild(i).GetComponent<Animator>().GetBool("isRich") &&
                !GameObject.FindWithTag("Players").transform.GetChild(i).GetComponent<Animator>().GetBool("isRory"))
            {
                GameObject.FindWithTag("Players").transform.GetChild(i).gameObject.SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
		playerMoving = false;
        transform.GetComponent<Rigidbody2D>().velocity = new Vector3(0f,0f,0f);

        float x = 0f;
        float y = 0f;

        anim.SetFloat("MoveX", 0f);
        anim.SetFloat("MoveY", 0f);
        anim.SetBool("PlayerMoving", false);

        if (Input.GetAxisRaw("Horizontal") > 0.5f || Input.GetAxisRaw("Horizontal") < -0.5f)
		{
            x = Input.GetAxisRaw("Horizontal");

            //Vector3 inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);


            //transform.Translate (new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
			playerMoving = true;
			lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
		}

		if(Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
		{
            y = Input.GetAxisRaw("Vertical");

            //transform.Translate (new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
            playerMoving = true;
			lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
		}

        Vector3 inputVector = new Vector3(x, y, 0);

        //inputVector.Normalize();
        inputVector = Vector3.ClampMagnitude(inputVector, 1);

        transform.GetComponent<Rigidbody2D>().velocity = inputVector * moveSpeed;

		anim.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
		anim.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
		anim.SetBool("PlayerMoving", playerMoving);
		anim.SetFloat("LastX", lastMove.x);
		anim.SetFloat("LastY", lastMove.y);
    }
}
