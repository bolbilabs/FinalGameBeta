﻿using UnityEngine;
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
        mapPosition = GameObject.FindWithTag("Players").transform.GetChild(0).position;

        SceneManager.LoadScene("BattleScene");
    }

    public static void EndBattle()
    {
        enemies = new List<GameObject>();
        SceneManager.LoadScene("Overworld");

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
                if (disabledEnemy.ContainsKey(i))
                {
                    if (disabledEnemy[i])
                    {
                        GameObject.FindWithTag("MapEnemies").transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}