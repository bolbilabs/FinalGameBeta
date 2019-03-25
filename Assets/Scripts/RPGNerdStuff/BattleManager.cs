using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    private GameObject[] players;

    [SerializeField]
    private GameObject[] enemies;

    [SerializeField]
    private List<GameObject> fightOrder;

    [SerializeField]
    private List<Object> targetList;

    [SerializeField]
    private Action chosenAction = null;

    //// First index: Player
    //// Second index: Target
    //// Third index: Action
    //[SerializeField]
    //private List<List<Object>> actionParams;
    public Dictionary<int, List<List<Object>>> actionParams = new Dictionary<int, List<List<Object>>>();

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
    void Start()
    {
        // Placeholder for prototype
        players = GameObject.FindGameObjectsWithTag("Player");
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        fightOrder.AddRange(players);
        fightOrder.AddRange(enemies);

        // Determines turn order based on speed
        fightOrder = fightOrder.OrderByDescending(o => o.GetComponent<CharacterStats>().speed.GetValue()).ToList();

        
    }

    // Update is called once per frame
    void Update()
    {
        // Enemy chooses who to attack here. PEAK Displayed.

        // Might have to sort players by ID... but for now:


        //For each player, show the UI. Make the UI for each into a list.

        if (currentPlayer < players.Length)
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

        if (menuState == 0)
        {
            UpdateTopMenu();
        } else if (menuState == 1)
        {
            UpdateSubMenu();
        } else if (menuState == 2)
        {
            UpdateTargetMenu();
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
            }
        }
        else if (Input.GetKeyDown("x")) {
            if (currentPlayer > 0)
            {
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
        UpdateUI();
    }

    void UpdateTargetMenu()
    {
        if (Input.GetKeyDown("z"))
        {
            if (chosenAction != null)
            {
                List<Object> currentPlayerList = new List<Object>();

                currentPlayerList.Add(players[currentPlayer]);

                List<List<Object>> indexList = new List<List<Object>>();

                List<Object> actionList = new List<Object>();

                actionList.Add(chosenAction);

                indexList.Add(currentPlayerList);
                indexList.Add(targetList);
                indexList.Add(actionList);

                actionParams[currentPlayer] = indexList;

                Action checkAction = ((Action)(actionParams[currentPlayer][actionParams[currentPlayer].Count() - 1][0]));

                Debug.Log("Locked in " + checkAction.moveName + "!");

                if (currentPlayer < players.Length - 1)
                {
                    currentPlayer++;
                    goToTopMenu();
                }
                else
                {
                    Debug.Log("All players locked in!");
                }
            }
            else
            {
                Debug.Log("Something's wrong.");
            }
        } else if (Input.GetKeyDown("x"))
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

    void goToSubMenu()
    {
        skipSub = false;
        menuState = 1;
        currentSubAction = 0;
        UpdateUI();

        // Update UI
    }

    void UpdateSubMenu()
    {
        List<Action> possibleActions = new List<Action>();

        if (currentTopAction == 1)
        {
            // Specials
            possibleActions.AddRange(players[currentPlayer].GetComponent<PlayerController>().specialAttack);
        }



        if (possibleActions.Any())
        {

        }
        else
        {
            Debug.Log("Empty.");
        }


    }


    void UpdateUI()
    {
        if (menuState == 0)
        {
            if (currentTopAction == 0)
            {
                Debug.Log("Basic attack.");
            } else if (currentTopAction == 1)
            {
                Debug.Log("Special attacks.");
            } else if (currentTopAction == 2)
            {
                Debug.Log("Various Actions.");
            }
        }
    }

    int mod(int x, int m)
    {
        return (x % m + m) % m;
    }
}
