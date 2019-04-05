using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
	public float moveSpeed;
	public Animator anim;
	private bool playerMoving;
	private Vector2 lastMove;

    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator>();
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
