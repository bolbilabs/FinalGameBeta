using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameControl : MonoBehaviour
{
    public static GameControl instance;         //A reference to our game control script so we can access it statically.

    public static List<GameObject> playerOrder = new List<GameObject>();

    public static List<GameObject> enemies = new List<GameObject>();

    public static Vector3 mapPosition;

    public List<GameObject> enemyPool;

    public static List<GameObject> allEnemiesPool;

    public static Dictionary<int, bool> disabledEnemy = new Dictionary<int, bool>();

    public static bool isFrozen = false;

    public static bool isBattling = false;

    public static int numDeaths = 3;
    public static bool downCut = false;
    public static bool downCut2 = false;
    public static GameObject characterOut;

    public static bool noMasEnemies = false;

    public static bool finalBattle = false;

    public static Action hopingHero;


    void Awake()
    {
        //If we don't currently have a game control...
        if (instance == null)
        {
            //...set this one to be it...
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //...otherwise...
        else if (instance != this)
            //...destroy this one because it is a duplicate.
            Destroy(gameObject);

        if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByName("Overworld")))
        {
            for (int i = 0; i < GameObject.FindWithTag("Players").transform.childCount; i++)
            {
                GameObject.FindWithTag("Players").transform.GetChild(i).position = mapPosition;
            }
        }

        playerOrder = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
            {
                playerOrder.Add(transform.GetChild(i).gameObject);
            }
        }

        allEnemiesPool = enemyPool;

        Camera.main.GetComponent<SimpleBlit>().TransitionMaterial.SetFloat("_Fade", 0.0f);

    }

    public void Update()
    {
        if (Input.GetKeyDown("j"))
        {
            StartBattle(enemyPool);
        }

        if (Input.GetKeyDown("k"))
        {
            EndBattle();
        }
    }

    public static void StartBattle(List<GameObject> enemiez)
    {
        enemies = enemiez;

        FreezeOverworld();

        Camera.main.gameObject.GetComponent<FadeOutGoop>().enabled = true;

        isBattling = true;


        //SceneManager.LoadScene("BattleScene");
    }

    public static void EndBattle()
    {
        enemies = new List<GameObject>();
        Camera.main.gameObject.GetComponent<FadeOut>().enabled = true;

        isBattling = false;

        //SceneManager.LoadScene("Overworld");
    }


    public static void EndBattleDeath()
    {
        enemies = new List<GameObject>();
        SceneManager.LoadScene("Overworld");
    }


    public static void FreezeOverworld()
    {
        mapPosition = GameObject.FindWithTag("Players").transform.GetChild(0).position;

        GameObject.FindWithTag("Players").transform.GetChild(0).GetComponent<PlayerMovements>().enabled = false;

        GameObject.FindWithTag("Players").transform.GetChild(0).GetComponent<Animator>().enabled = false;

        GameObject.FindWithTag("Players").transform.GetChild(0).GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);

        for (int i = 0; i < GameObject.FindGameObjectWithTag("MapEnemies").transform.childCount; i++)
        {
            GameObject.FindGameObjectWithTag("MapEnemies").transform.GetChild(i).GetComponent<EnemyFieldController>().enabled = false;
        }

        isFrozen = true;

    }

    public static void UnfreezeOverworld()
    {
        mapPosition = GameObject.FindWithTag("Players").transform.GetChild(0).position;

        GameObject.FindWithTag("Players").transform.GetChild(0).GetComponent<PlayerMovements>().enabled = true;

        GameObject.FindWithTag("Players").transform.GetChild(0).GetComponent<Animator>().enabled = true;
        
        for (int i = 0; i < GameObject.FindGameObjectWithTag("MapEnemies").transform.childCount; i++)
        {
            GameObject.FindGameObjectWithTag("MapEnemies").transform.GetChild(i).GetComponent<EnemyFieldController>().enabled = true;
        }

        isFrozen = false;
    }


    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Level Loaded");
        Debug.Log(scene.name);
        Debug.Log(mode);


        if (scene == SceneManager.GetSceneByName("Overworld"))
        {
            for (int i = 0; i < GameObject.FindWithTag("MapEnemies").transform.childCount; i++)
            {
                if (noMasEnemies || disabledEnemy.ContainsKey(i))
                {
                    if (noMasEnemies || disabledEnemy[i])
                    {
                        GameObject.FindWithTag("MapEnemies").transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }

            if (downCut)
            {
                GameObject downPlayer = GameObject.FindWithTag("KOChar");
                Color charColor = downPlayer.GetComponent<SpriteRenderer>().color;
                charColor.a = 1;

                downPlayer.GetComponent<SpriteRenderer>().color = charColor;

                string currentName = characterOut.GetComponent<PlayerStats>().characterName;

                Animator currentAnim = downPlayer.GetComponent<Animator>();
                
                if (currentName == "Paul")
                {
                    currentAnim.SetBool("isPaul", true);
                }
                else if (currentName == "Luna")
                {
                    currentAnim.SetBool("isLuna", true);

                }
                else if (currentName == "Rich")
                {
                    currentAnim.SetBool("isRich", true);
                }
                else if (currentName == "Rory")
                {
                    currentAnim.SetBool("isRory", true);
                }

                currentAnim.SetBool("IsKO", true);

                downPlayer.transform.position = mapPosition;
                downCut = false;
                downCut2 = true;
            }
        } else if (scene == SceneManager.GetSceneByName("Aftermath"))
        {
            GameObject downPlayer = GameObject.FindWithTag("KOChar");

            string currentName = characterOut.GetComponent<PlayerStats>().characterName;

            Animator currentAnim = downPlayer.GetComponent<Animator>();

            if (currentName == "Paul")
            {
                currentAnim.SetBool("isPaul", true);
            }
            else if (currentName == "Luna")
            {
                currentAnim.SetBool("isLuna", true);

            }
            else if (currentName == "Rich")
            {
                currentAnim.SetBool("isRich", true);
            }
            else if (currentName == "Rory")
            {
                currentAnim.SetBool("isRory", true);
            }

            currentAnim.SetBool("IsKO", true);
        }
    }
}