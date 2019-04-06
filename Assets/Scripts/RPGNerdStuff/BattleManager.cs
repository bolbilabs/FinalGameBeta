using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class BattleManager : MonoBehaviour
{
    public Animator animator;
    public Animator healthBarAnim;


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

    private int buffer = 0;

    public int deathparam = 0;



    [SerializeField]
    private bool enemyLine = true;

    [SerializeField]
    private bool skipSub = false;

    public List<GameObject> players;

    public List<GameObject> enemies;

    [SerializeField]
    private List<GameObject> fightOrder;

    [SerializeField]
    private List<Object> targetList;

    [SerializeField]
    private List<Object> peekList;

    [SerializeField]
    private Action chosenAction = null;

    private static BattleManager instance;

    //// First index: Player
    //// Second index: Target
    //// Third index: Action
    //[SerializeField]
    //private List<List<Object>> actionParams;
    public Dictionary<GameObject, List<List<Object>>> actionParams = new Dictionary<GameObject, List<List<Object>>>();
    public Dictionary<GameObject, List<List<Object>>> peekParams = new Dictionary<GameObject, List<List<Object>>>();

    public List<int> skipIndex = new List<int>();


    public TextMeshProUGUI battleText;

    public AutoDialogue autoDialogue;

    int width = 640;
    int height = 480;

    public GameObject currentDial = null;



    List<Action> possibleActions = new List<Action>();

    public List<GameObject> healthBlocks = new List<GameObject>();



    // This function is called to start battles.
    // The parameter here is for non-random encounters
    // Well, actually. Do we want to fight directly on map (could be stylish) or change scenes for battle?
    public void Init ()
    {
        //players = GameObject.FindGameObjectsWithTag("Player");
        //players = GameObject.FindGameObjectsWithTag("Enemy");


        if (GameControl.enemies != null && GameControl.enemies.Count > 0)
        {
            foreach (GameObject enemy in GameControl.enemies)
            {
                GameObject currentEnemy = Instantiate(enemy);
                currentEnemy.transform.SetParent(GameObject.FindWithTag("Enemies").transform);
                //enemies.Add(currentEnemy);
            }
        }
        
    }

    public static BattleManager GetInstance()
    {
        return instance;
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Screen.SetResolution(width, height, true, 60);

        Init();
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
        int skip = 0;
        foreach (GameObject player in players)
        {
            if (!player.activeSelf)
            {
                skipIndex.Add(skip);
            }
            skip++;
        }

        players.Clear();
        enemies.Clear();
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        //dialList.AddRange(GameObject.FindGameObjectsWithTag("Dials"));



        healthBlocks.Clear();

        int i = 0;
        int offset = 0;
        foreach (GameObject player in players)
        {
            //Debug.Log(player.name);

            if (player.activeSelf)
            {
                foreach (int skipper in skipIndex)
                {
                    if (i == skipper)
                    {
                        offset++;
                    }
                }
                //Debug.Log(i + offset);
                GameObject.FindGameObjectWithTag("HealthBlock").transform.GetChild(i+offset).gameObject.SetActive(true);
                healthBlocks.Add(GameObject.FindGameObjectWithTag("HealthBlock").transform.GetChild(i+offset).gameObject);
                PlayerStats currStats = player.GetComponent<PlayerStats>();
                healthBlocks[i].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().SetText(currStats.characterName + "\n" + currStats.currentHealth + "/" + currStats.maxHealth.GetValue());
                healthBlocks[i].transform.GetChild(0).GetChild(1).GetChild(0).GetChild(3).GetComponent<SpriteRenderer>().sprite = currStats.bodySprite;
            }
            i++;
        }

        fightOrder.Clear();
        fightOrder.AddRange(players);
        fightOrder.AddRange(enemies);

        // Determines turn order based on speed
        fightOrder = fightOrder.OrderByDescending(o => o.GetComponent<CharacterStats>().speed.GetValue()).ToList();

        actionParams = new Dictionary<GameObject, List<List<Object>>>();
        peekParams = new Dictionary<GameObject, List<List<Object>>>();

        foreach (GameObject player in players)
        {
            player.GetComponent<CharacterStats>().TurnOver();
        }

        // Simple enemy AI.
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<CharacterStats>().TurnOver();

            Action enemyAction = enemy.GetComponent<EnemyController>().moves[Random.Range(0, enemy.GetComponent<EnemyController>().moves.Length)];

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

                GameObject target = enabledPlayers[Random.Range(0, enabledPlayers.Count)];
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
        Transform t = healthBlocks[currentPlayer].transform.GetChild(0).transform;

        //currentDial = new GameObject();

        foreach(Transform tr in t)
           {
            if (tr.tag == "Dials")
            {
                currentDial = tr.gameObject;
            }
        }


        skipSub = false;
        menuState = 0;
        currentTopAction = 0;

        animator = currentDial.GetComponent<Animator>();
        healthBarAnim = healthBlocks[currentPlayer].GetComponent<Animator>();

        healthBarAnim.SetInteger("State", 1);



        UpdateUI();
        currentDial.GetComponent<DialChildren>().ForceUpdate();


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
                healthBarAnim.SetInteger("State", 0);
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
        } else if (Input.GetKey("down") || Input.GetKey("right"))
        {
            //Debug.Log("Base: " + animator.GetLayerIndex("Normal"));

            if (animator.GetInteger("Direction") == 0 && buffer == 0)
            {
                currentTopAction++;
                currentTopAction = mod(currentTopAction, numTopActions);
                animator.SetInteger("Direction", -1);
                currentDial.GetComponent<DialChildren>().dialChildren[0].SetActive(false);

                UpdateUI();
            }
            else if (animator.GetInteger("Direction") == 0 && buffer != 0)
            {
                buffer = 0;
            } 
            else
            {
                buffer = -1;
            }
        } else if (Input.GetKey("up") || Input.GetKey("left"))
        {
            if (animator.GetInteger("Direction") == 0 && buffer == 0)
            {
                currentTopAction--;
                currentTopAction = mod(currentTopAction, numTopActions);
                animator.SetInteger("Direction", 1);
                UpdateUI();
            }
            else if (animator.GetInteger("Direction") == 0 && buffer != 0)
            {
                buffer = 0;
            }
            else
            {
                buffer = 1;
            }
        }

        //if (buffer < 0 && animator.GetInteger("Direction") == 0)
        //{
        //    currentTopAction++;
        //    currentTopAction = mod(currentTopAction, numTopActions);
        //    animator.SetInteger("Direction", -1);
        //    UpdateUI();
        //    buffer = 0;
        //} else if (buffer > 0 && animator.GetInteger("Direction") == 0)
        //{
        //    currentTopAction--;
        //    currentTopAction = mod(currentTopAction, numTopActions);
        //    animator.SetInteger("Direction", 1);
        //    UpdateUI();
        //    buffer = 0;
        //}
    }

    void goToTargetMenu()
    {
        menuState = 2;
        currentTarget = 0;
        enemyLine = true;

        healthBarAnim.SetInteger("State", 2);



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
                healthBarAnim.SetInteger("State", 1);

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
            battleText.SetText("Everyone's... gone.");

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
        currentDial.GetComponent<DialChildren>().ForceUpdate();

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
                currentDial.GetComponent<DialChildren>().ForceUpdate();
            }
            else if (Input.GetKey("down") || Input.GetKey("right"))
            {
                if (animator.GetInteger("Direction") == 0 && buffer == 0)
                {
                    currentSubAction++;
                    currentSubAction = mod(currentSubAction, possibleActions.Count());

                    animator.SetInteger("Direction", -1);
                    currentDial.GetComponent<DialChildren>().dialChildren[0].SetActive(false);

                    UpdateUI();
                }
                else if (animator.GetInteger("Direction") == 0 && buffer != 0)
                {
                    buffer = 0;
                }
                else
                {
                    buffer = -1;
                }
            } else if (Input.GetKey("up") || Input.GetKey("left"))
            {
                if (animator.GetInteger("Direction") == 0 && buffer == 0)
                {
                    currentSubAction--;
                    currentSubAction = mod(currentSubAction, possibleActions.Count());
                    animator.SetInteger("Direction", 1);

                    UpdateUI();
                }
                else if (animator.GetInteger("Direction") == 0 && buffer != 0)
                {
                    buffer = 0;
                }
                else
                {
                    buffer = 1;
                }
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
                currentDial.GetComponent<DialChildren>().ForceUpdate();
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

    public void OnAnimationEvent (int direction)
    {

    }


    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }




}
