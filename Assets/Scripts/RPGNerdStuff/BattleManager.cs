using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public bool inBattle = true;

    public bool executing = false;

    public int currentPlayer = 0;

    // (0, top), (1, sub), (2, target)
    public int menuState = 0;

    public int currentTopAction = 0;

    public int currentSubAction = 0;

    public int currentTarget = 0;

    // Will change this number
    private int numTopActions = 3;

    private int numSubAction = 0;


    [SerializeField]
    private bool enemyLine = true;

    [SerializeField]
    private bool skipSub = false;

    [SerializeField]
    private List<GameObject> players;

    [SerializeField]
    private List<GameObject> enemies;

    [SerializeField]
    private List<GameObject> fightOrder;

    [SerializeField]
    private List<Object> targetList;

    [SerializeField]
    private List<Object> peekList;

    [SerializeField]
    private Action chosenAction = null;

    //// First index: Player
    //// Second index: Target
    //// Third index: Action
    //[SerializeField]
    //private List<List<Object>> actionParams;
    public Dictionary<GameObject, List<List<Object>>> actionParams = new Dictionary<GameObject, List<List<Object>>>();
    public Dictionary<GameObject, List<List<Object>>> peekParams = new Dictionary<GameObject, List<List<Object>>>();


    public TextMeshProUGUI battleText;

    public AutoDialogue autoDialogue;



    List<Action> possibleActions = new List<Action>();


    // This function is called to start battles.
    // The parameter here is for non-random encounters
    // Well, actually. Do we want to fight directly on map (could be stylish) or change scenes for battle?
    public void Init (GameObject[] pregenEnemies)
    {
        if (pregenEnemies == null)
        {
            // Randomly generate enemies from a beautiful selection of prefabs
        }
        else
        { }

            //players = GameObject.FindGameObjectsWithTag("Player");
            //players = GameObject.FindGameObjectsWithTag("Enemy");

    }

    // Start is called before the first frame update
    public void Start()
    {
        executing = false;
        currentPlayer = 0;
        //currentPlayer = 0;
        //currentSubAction = 0;
        //currentTopAction = 0;
        //currentTarget = 0;
        //menuState = 0;

        // Placeholder for prototype
        players.Clear();
        enemies.Clear();
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));

        fightOrder.Clear();
        fightOrder.AddRange(players);
        fightOrder.AddRange(enemies);

        // Determines turn order based on speed
        fightOrder = fightOrder.OrderByDescending(o => o.GetComponent<CharacterStats>().speed.GetValue()).ToList();

        actionParams = new Dictionary<GameObject, List<List<Object>>>();
        peekParams = new Dictionary<GameObject, List<List<Object>>>();

        // Simple enemy AI.
        foreach (GameObject enemy in enemies)
        {
            Action enemyAction = enemy.GetComponent<EnemyController>().moves[Random.Range(0, enemy.GetComponent<EnemyController>().moves.Length - 1)];

            if (enemyAction.onlyTargetsEnemies)
            {
                List<GameObject> enabledPlayers = new List<GameObject>();

                foreach (GameObject player in players)
                {
                    if (player.activeSelf)
                    {
                        enabledPlayers.Add(player);
                    }
                }

                GameObject target = enabledPlayers[Random.Range(0, enabledPlayers.Count-1)];
                List<Object> buildList = new List<Object>();
                buildList.Add(enemy);
                buildList.Add(target);
                buildList.Add(enemyAction);

                actionParams[enemy] = new List<List<Object>>();

                actionParams[enemy].Add(buildList);
            }

           
        }

        battleText.SetText("The party reels in anticipation.");

        foreach (GameObject current in fightOrder)
        {
            current.GetComponent<CharacterStats>().currentPeekDamage = current.GetComponent<CharacterStats>().currentHealth;
        }



        goToTopMenu();
    }

    // Update is called once per frame
    public void Update()
    {
        // Enemy chooses who to attack here. PEAK Displayed.

        // Might have to sort players by ID... but for now:


        //For each player, show the UI. Make the UI for each into a list.

        if (currentPlayer < players.Count())
        {
            UpdatePlayer(players[currentPlayer]);
        }
    }

    // These UpdatePlayers are the player states
    void UpdatePlayer (GameObject player)
    {
        PlayerController playerController = player.GetComponent<PlayerController>();

        // actionParams.Count
        // Treating List as a Deque

        // Select Top Action


        if (!executing)
        {
            if (menuState == 0)
            {
                UpdateTopMenu();
            }
            else if (menuState == 1)
            {
                UpdateSubMenu();
            }
            else if (menuState == 2)
            {
                UpdateTargetMenu();
            }
        }
        else
        {
            ExecuteActions();
        }



        // Select Sub Action (Ignore if Basic Attack)





        /* Select target */



        // Regular Targeting
        //if (!targetsAllEnemies && !targetsAllAllies && !onlyTargetSelf) {

        //}

        // Special Case Targeting
    }

    void goToTopMenu ()
    {
        skipSub = false;
        menuState = 0;
        currentTopAction = 0;
        UpdateUI();

        // Update UI
    }

    void UpdateTopMenu()
    {


        if (Input.GetKeyDown("z"))
        {
            if (currentTopAction == 0)
            {
                skipSub = true;
                chosenAction = players[currentPlayer].GetComponent<PlayerController>().basicAttack;
                goToTargetMenu();
            } else if (currentTopAction == 1)
            {
                skipSub = false;
                goToSubMenu();
            }
        }
        else if (Input.GetKeyDown("x")) {
            if (currentPlayer > 0)
            {
                actionParams[players[currentPlayer-1]] = new List<List<Object>>();
                peekParams[players[currentPlayer -1]] = new List<List<Object>>();
                currentPlayer--;
                goToTopMenu();
                //if (actionParams.Siz > 0)
                //{
                //    // if actionParams is dictionary, then no need to remove.
                //    //actionParams.RemoveAt(actionParams.Count() - 1);
                //}
            }
            else
            {
                Debug.Log("Nevermore");
            }
        } else if (Input.GetKeyDown("down") || Input.GetKeyDown("right"))
        {
            currentTopAction++;
            currentTopAction = mod(currentTopAction, numTopActions);
            UpdateUI();
        } else if (Input.GetKeyDown("up") || Input.GetKeyDown("left"))
        {
            currentTopAction--;
            currentTopAction = mod(currentTopAction, numTopActions);
            UpdateUI();
        }


    }

    void goToTargetMenu()
    {
        menuState = 2;
        currentTarget = 0;
        enemyLine = true;


        if (players.Count() <= 1 && chosenAction.cannotTargetSelf && (chosenAction.onlyTargetsAllies || (chosenAction.targetsAllAllies && !chosenAction.targetsAllEnemies)))
        {
            currentTarget = -1;
        }

        if (chosenAction.onlyTargetsAllies)
        {
            if (currentTarget == currentPlayer)
            {
                // Maybe a find next open for polish? Oh well.
                currentTarget++;
                currentTarget = mod(currentTarget, players.Count());
            }
            enemyLine = false;
        } else if (chosenAction.onlyTargetsSelf)
        {
            enemyLine = false;
            currentTarget = currentPlayer;
        }


        UpdateUI();
    }

    void UpdateTargetMenu()
    {
        if (currentTarget != -1)
        {
            if (Input.GetKeyDown("z"))
            {
                if (chosenAction != null)
                {

                    actionParams[players[currentPlayer]] =  new List<List<Object>>();
                    peekParams[players[currentPlayer]] = new List<List<Object>>();


                    if (targetList.Any())
                    {
                        foreach (Object targetZ in targetList)
                        {

                            Object currentPlayerList = players[currentPlayer];

                            List<Object> indexList = new List<Object>();

                            Object actionList = chosenAction;

                            indexList.Add(currentPlayerList);
                            indexList.Add(targetZ);
                            indexList.Add(actionList);


                            actionParams[players[currentPlayer]].Add(indexList);
                        }
                    }

                    if (peekList.Any())
                    {
                        foreach (Object peekZ in peekList)
                        {
                            Object currentPlayerList = players[currentPlayer];

                            List<Object> indexList = new List<Object>();

                            Object actionList = chosenAction;

                            indexList.Add(currentPlayerList);
                            indexList.Add(peekZ);
                            indexList.Add(actionList);


                            peekParams[players[currentPlayer]].Add(indexList);
                        }
                    }




                    Action checkAction = ((Action)(actionParams[players[currentPlayer]][0][2]));

                    Debug.Log("Locked in " + checkAction.moveName + "!");

                    if (currentPlayer < players.Count() - 1)
                    {
                        currentPlayer++;
                        goToTopMenu();
                    }
                    else
                    {
                        Debug.Log("All players locked in!");

                        // Execute

                        foreach (GameObject enemy in enemies)
                        {
                            GameObject targetY = (GameObject)enemy;

                            Color color = targetY.GetComponent<SpriteRenderer>().color;
                            color.r = 255; color.g = 255; color.b = 255;
                            targetY.GetComponent<SpriteRenderer>().color = color;
                        }

                        goToExecuteActions();

                    }
                }
                else
                {
                    Debug.Log("Something's wrong.");
                }
            }
            else if (Input.GetKeyDown("x"))
            {
                if (skipSub)
                {
                    goToTopMenu();
                    actionParams[players[currentPlayer]] = new List<List<Object>>();
                    peekParams[players[currentPlayer]] = new List<List<Object>>();

                }
                else
                {
                    goToSubMenu();
                    actionParams[players[currentPlayer]] = new List<List<Object>>();
                    peekParams[players[currentPlayer]] = new List<List<Object>>();

                }
            }
            else if (Input.GetKeyDown("down"))
            {
                if (!chosenAction.onlyTargetsAllies && !chosenAction.onlyTargetsEnemies && !chosenAction.onlyTargetsSelf)
                {
                    enemyLine = !enemyLine;
                    if (enemyLine)
                    {
                        currentTarget = Mathf.Clamp(currentTarget, 0, enemies.Count() - 1);
                    }
                    else
                    {
                        currentTarget = Mathf.Clamp(currentTarget, 0, players.Count() - 1);
                    }
                    if (currentTarget == currentPlayer && !enemyLine && chosenAction.cannotTargetSelf)
                    {
                        currentTarget++;
                        currentTarget = mod(currentTarget, players.Count());
                        if (currentTarget == currentPlayer)
                        {
                            enemyLine = true;
                        }
                    }
                    UpdateUI();
                }
            }
            else if (Input.GetKeyDown("up"))
            {
                if (!chosenAction.onlyTargetsAllies && !chosenAction.onlyTargetsEnemies && !chosenAction.onlyTargetsSelf)
                {
                    enemyLine = !enemyLine;
                    if (enemyLine)
                    {
                        currentTarget = Mathf.Clamp(currentTarget, 0, enemies.Count() - 1);
                    }
                    else
                    {
                        currentTarget = Mathf.Clamp(currentTarget, 0, players.Count() - 1);
                    }
                    if (currentTarget == currentPlayer && !enemyLine && chosenAction.cannotTargetSelf)
                    {
                        currentTarget--;
                        currentTarget = mod(currentTarget, players.Count());
                        if (currentTarget == currentPlayer)
                        {
                            enemyLine = true;
                        }
                    }
                    UpdateUI();
                }
            }
            else if (Input.GetKeyDown("left"))
            {
                currentTarget--;

                if (enemyLine)
                {
                    currentTarget = mod(currentTarget, enemies.Count());
                }
                else
                {
                    currentTarget = mod(currentTarget, players.Count());
                }
                if (currentTarget == currentPlayer && !enemyLine && chosenAction.cannotTargetSelf)
                {
                    currentTarget--;
                    currentTarget = mod(currentTarget, players.Count());
                    if (currentTarget == currentPlayer)
                    {
                        enemyLine = true;
                    }
                }
                UpdateUI();
            }
            else if (Input.GetKeyDown("right"))
            {
                currentTarget++;
                if (enemyLine)
                {
                    currentTarget = mod(currentTarget, enemies.Count());
                }
                else
                {
                    currentTarget = mod(currentTarget, players.Count());
                }
                if (currentTarget == currentPlayer && !enemyLine && chosenAction.cannotTargetSelf)
                {
                    currentTarget++;
                    currentTarget = mod(currentTarget, players.Count());
                    if (currentTarget == currentPlayer)
                    {
                        enemyLine = true;
                    }
                }
                UpdateUI();
            }
        }
        else
        {
            // Ally-only move, but all alone.
            Debug.Log("Everyone's... gone.");
            if (Input.GetKeyDown("x"))
            {
                if (skipSub)
                {
                    goToTopMenu();
                }
                else
                {
                    goToSubMenu();
                }
            }
        }
    }

    void goToSubMenu()
    {
        skipSub = false;
        menuState = 1;
        currentSubAction = 0;

        possibleActions.Clear();
        possibleActions.AddRange(players[currentPlayer].GetComponent<PlayerController>().specialAttack);

        UpdateUI();

        // Update UI
    }

    void UpdateSubMenu()
    {

        //if (currentTopAction == 1)
        //{
        //    // Specials
        //    possibleActions.AddRange(players[currentPlayer].GetComponent<PlayerController>().specialAttack);
        //}


        if (possibleActions.Any())
        {
            if (Input.GetKeyDown("z"))
            {
                chosenAction = possibleActions[currentSubAction];
                goToTargetMenu();
            } else if (Input.GetKeyDown("x"))
            {
                menuState = 0;
                UpdateUI();
            } else if (Input.GetKeyDown("down") || Input.GetKeyDown("right"))
            {
                currentSubAction++;
                currentSubAction = mod(currentSubAction, possibleActions.Count());
                UpdateUI();
            } else if (Input.GetKeyDown("up") || Input.GetKeyDown("left"))
            {
                currentSubAction--;
                currentSubAction = mod(currentSubAction, possibleActions.Count());
                UpdateUI();
            }
        }
        else
        {
            //Debug.Log("Empty.");
            battleText.SetText("No options to select. Press \"X\" to return.");
            if (Input.GetKeyDown("x"))
            {
                menuState = 0;
                UpdateUI();
            }
        }


    }


    void UpdateUI()
    {
        foreach (GameObject current in fightOrder)
        {
            current.GetComponent<CharacterStats>().currentPeekDamage = current.GetComponent<CharacterStats>().currentHealth;
        }

        foreach (GameObject enemy in enemies)
        {
            GameObject targetY = (GameObject)enemy;

            Color color = targetY.GetComponent<SpriteRenderer>().color;
            color.r = 255; color.g = 255; color.b = 255;
            targetY.GetComponent<SpriteRenderer>().color = color;
        }

        if (menuState == 0)
        {
            if (currentTopAction == 0)
            {
                //Debug.Log("Basic attack.");
                battleText.SetText(players[currentPlayer].GetComponent<PlayerController>().basicAttack.description);

            } else if (currentTopAction == 1)
            {
                //Debug.Log("Special attacks.");
                battleText.SetText("Special moves.");
            }
            else if (currentTopAction == 2)
            {
                //Debug.Log("Various Actions.");
                battleText.SetText("Various actions.");

            }
        } else if (menuState == 1)
        {
            if (possibleActions.Any())
            {
                //Debug.Log(possibleActions[currentSubAction].description);
                battleText.SetText(possibleActions[currentSubAction].description);

            }
        } else if (menuState == 2)
        {
            UpdateTargets();

            foreach (GameObject enemy in enemies)
            {
                GameObject targetY = (GameObject)enemy;

                Color color = targetY.GetComponent<SpriteRenderer>().color;
                if (!targetList.Contains(enemy))
                {
                    color.r = 0;
                    color.g = 0;
                    color.b = 0;
                }
                else
                {
                    color.r = 0;
                    color.g = 255;
                    color.b = 0;
                }
                targetY.GetComponent<SpriteRenderer>().color = color;

            }


            // Check total peek damage

            foreach (GameObject current in fightOrder)
            {
                current.GetComponent<CharacterStats>().currentPeekDamage = current.GetComponent<CharacterStats>().currentHealth;
            }

            // FOR ENEMY ATTACKS




            // OTHER PLAYER ATTACKS
            if (actionParams.Values.ToList().Any())
            {
                foreach (List<List<Object>> listObjects in actionParams.Values.ToList())
                {
                    foreach (List<Object> objects in listObjects)
                    {
                        GameObject casterObj = (GameObject) objects[0];
                        GameObject targetObj = (GameObject)objects[1];


                        CharacterStats casterStats = casterObj.GetComponent<CharacterStats>();
                        CharacterStats targetStats = targetObj.GetComponent<CharacterStats>();
                        Action action = (Action)objects[2];

                        action.Peek(casterStats, targetStats);
                    }
                }
            }


            if (peekParams.Values.ToList().Any())
            {
                foreach (List<List<Object>> listObjects in peekParams.Values.ToList())
                {
                    foreach (List<Object> objects in listObjects)
                    {
                        GameObject casterObj = (GameObject)objects[0];
                        GameObject targetObj = (GameObject)objects[1];


                        CharacterStats casterStats = casterObj.GetComponent<CharacterStats>();
                        CharacterStats targetStats = targetObj.GetComponent<CharacterStats>();
                        Action action = (Action)objects[2];

                        action.PeekPassive(casterStats, targetStats);
                    }
                }
            }

            // CURRENT ACTIVE AND PASSIVE ATTACKS!! 
            if (peekList.Any())
            {

                foreach (Object targetObj in peekList)
                {
                    GameObject targetGameObj = (GameObject)targetObj;



                    CharacterStats targetStats = targetGameObj.GetComponent<CharacterStats>();
                    CharacterStats casterStats = players[currentPlayer].GetComponent<CharacterStats>();

                    Debug.Log("Passiving:" + targetStats.characterName);


                    chosenAction.PeekPassive(casterStats, targetStats);
                    chosenAction.PeekPassive(casterStats, targetStats);


                    //int deduct = casterStats.currentHealth - chosenAction.PeekPassive(casterStats, targetStats);


                    //testString += targetStats.characterName + " for " + deduct;

                    //if (chosenAction.Peek(casterStats, targetStats) <= 0)
                    //{
                    //    testString += " mortal";
                    //}

                    //testString += " damage!";

                    //Debug.Log(testString);
                }
            }
            if (targetList.Any()) { 

            foreach (Object targetObj in targetList)
                {
                    GameObject targetGameObj = (GameObject)targetObj;

                    CharacterStats targetStats = targetGameObj.GetComponent<CharacterStats>();
                    CharacterStats casterStats = players[currentPlayer].GetComponent<CharacterStats>();


                    Debug.Log("Targeting:" + targetStats.characterName);


                    chosenAction.Peek(casterStats, targetStats);

                    //int deduct = casterStats.currentHealth - chosenAction.Peek(casterStats, targetStats);


                    //testString += targetStats.characterName + " for " + deduct;

                    //if (chosenAction.Peek(casterStats, targetStats) <= 0)
                    //{
                    //    testString += " mortal";
                    //}

                    //testString += " damage!";

                    //Debug.Log(testString);
                }
            }
        }
    }


    void UpdateTargets()
    {
        targetList.Clear();
        peekList.Clear();
        if (!chosenAction.targetsAllEnemies && !chosenAction.targetsAllAllies && !chosenAction.onlyTargetsSelf)
        {
            battleText.SetText("Select a target!");
            if (enemyLine)
            {
                targetList.Add(enemies[currentTarget]);
                //peekList.Add(enemies[currentTarget]);

            }
            else
            {
                targetList.Add(players[currentTarget]);
                //peekList.Add(enemies[currentTarget]);
            }
        }
        else
        {
            battleText.SetText("Lock in targets!");

        }
        if (chosenAction.targetsAllEnemies)
        {
            targetList.AddRange(enemies);

            //peekList.AddRange(enemies);
        }
        if (chosenAction.targetsAllAllies)
        {
            targetList.AddRange(players);
            targetList.Remove(players[currentPlayer]);

            //peekList.AddRange(players);
            //peekList.Remove(players[currentPlayer]);

        }
        if (chosenAction.onlyTargetsSelf)
        {
            targetList.Add(players[currentPlayer]);
            battleText.SetText("Lock in target!");


            //peekList.Add(players[currentPlayer]);
        }


        if (chosenAction.targetsAllAlliesPassive)
        {
            peekList.AddRange(players);
            peekList.Remove(players[currentPlayer]);
        }

        if (chosenAction.targetsAllEnemiesPassive)
        {
            peekList.AddRange(enemies);
        }

        if (chosenAction.targetsSelfPassive)
        {
            peekList.Add(players[currentPlayer]);
        }
    }


    public void goToExecuteActions()
    {
        executing = true;
        battleText.SetText("");
        autoDialogue.StartDialogue(fightOrder, actionParams, peekParams);
    }

    public void ExecuteActions()
    {

        //Action checkAction = ((Action)(actionParams[players[currentPlayer]][0][2]));



    }


    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
