using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFieldController : MonoBehaviour
{
    public GameObject preferredEnemy;

    private GameObject player;

    public float distanceToPlayer = 10;

    public float movementSpeed = 5f;

    private GameObject cam;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Players").transform.GetChild(0).gameObject;
        cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < distanceToPlayer)
        {
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.GetComponent<Rigidbody2D>().MovePosition(transform.position + direction * movementSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.collider.gameObject.layer);
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Players")) {
            List<GameObject> passThis = new List<GameObject>();

            passThis.Add(preferredEnemy);


            for (int i = 0; i < Random.Range(0,3); i++)
            {
                passThis.Add(GameControl.allEnemiesPool[Random.Range(0, GameControl.allEnemiesPool.Count)]);
            }

            GameControl.disabledEnemy.Add(transform.GetSiblingIndex(), true);

            GameControl.StartBattle(passThis);
        }
    }
}
