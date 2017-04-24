using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    public static Game instance;

    public enum GameState
    {
        IN_MENUS,
        IN_LEVEL,
        IN_INGAME_MENUS,
        IN_ENDGAME_MENUS,
        SHIFTING,
    }

    public GameState state = GameState.IN_MENUS;
    public float timeInState = 0.0f;
    public float unscaledTimeInState = 0.0f;
    public float shiftTime = 3.0f;

    public static Player thePlayer = null;
    public static SkillTree_Manager theSkillTreeManager = null;
    public static Boss theBoss = null;

    public GameObject menuHUD;
    public GameObject menuStart;
    public GameObject menuSkills;
    public GameObject menuEndGame;
    public GameObject menuEndSuccess;
    public GameObject menuEndFailure;
    public AudioSource mainMusic;
    public AudioSource bossMusic;


    public Enemy[] enemies;
    public Boss boss;
    bool flipBoss = false;

    bool gameEndSuccess = false;

	// Use this for initialization
	void Start ()
    {
        instance = this;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timeInState += Time.deltaTime;
        unscaledTimeInState += Time.unscaledDeltaTime;
        switch (state)
        {
            case GameState.IN_MENUS:              { ExecuteState_InMenus();          break; }
            case GameState.IN_LEVEL:              { ExecuteState_InLevel();          break; }
            case GameState.IN_INGAME_MENUS:       { ExecuteState_InInGameMenus();    break; }
            case GameState.IN_ENDGAME_MENUS:      { ExecuteState_InEndGameMenus();   break; }
            case GameState.SHIFTING:              { ExecuteState_Shifting();         break; }
        }
	}

    private void SwitchToState(GameState newState)
    {
        if (state != newState)
        {
            switch (state)
            { 
                case GameState.IN_MENUS:              { ExitState_InMenus();          break; }
                case GameState.IN_LEVEL:              { ExitState_InLevel();          break; }
                case GameState.IN_INGAME_MENUS:       { ExitState_InInGameMenus();    break; }
                case GameState.IN_ENDGAME_MENUS:      { ExitState_InEndGameMenus();   break; }
                case GameState.SHIFTING:              { ExitState_Shifting();         break; }
            }
            switch (newState)
            {
                case GameState.IN_MENUS:              { EnterState_InMenus();         break; }
                case GameState.IN_LEVEL:              { EnterState_InLevel();         break; }
                case GameState.IN_INGAME_MENUS:       { EnterState_InInGameMenus();   break; }
                case GameState.IN_ENDGAME_MENUS:      { EnterState_InEndGameMenus();  break; }
                case GameState.SHIFTING:              { EnterState_Shifting();        break; }
            }

            state = newState;
            timeInState = 0.0f;
            unscaledTimeInState = 0.0f;
        }
    }

    private void EnterState_InMenus()
    {
        menuStart.SetActive(true);
        menuHUD.SetActive(false);
        Time.timeScale = 0.0f;
    }

    private void EnterState_InLevel()
    {
        menuHUD.SetActive(true);
        Time.timeScale = 1.0f;
    }

    private void EnterState_InInGameMenus()
    {
        menuSkills.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void EnterState_InEndGameMenus()
    {
        menuEndGame.SetActive(true);
        (gameEndSuccess ? menuEndSuccess : menuEndFailure).SetActive(true);
        (gameEndSuccess ? menuEndFailure : menuEndSuccess).SetActive(false);
    }

    private void EnterState_Shifting() {}

    private void ExecuteState_InMenus() {}

    private void ExecuteState_InLevel()
    {
        // Is the player dead?
        if (thePlayer.health <= 0.0f)
        {
            SwitchToState(GameState.IN_ENDGAME_MENUS);
            gameEndSuccess = false;
            return;
        }

        // Check if we should open the skill tree menu
        // Be careful that a Tab press to close the skills menu doesn't immediately
        // open it again
        if (unscaledTimeInState > 0.2f && Input.GetAxisRaw("Tab") > 0.0f)
        {
            SwitchToState(GameState.IN_INGAME_MENUS);
        }
    }

    private void ExecuteState_InInGameMenus()
    {
        // If we somehow get in here just as we died, return to game
        if (thePlayer.health <= 0.0f)
        {
            SwitchToState(GameState.IN_LEVEL);
            return;
        }

        // Check if we should return to game
        // Allow Esc or Tab to close the menu, but be careful that the Tab press that opens
        // the menu doesn't close it again instantly.
        if (Input.GetAxisRaw("Cancel") > 0.0f || (unscaledTimeInState > 0.2f && Input.GetAxisRaw("Tab") > 0.0f))
        {
            SwitchToState(GameState.IN_LEVEL);
        }
    }

    private void ExecuteState_InEndGameMenus()
    {
        if (Input.GetAxisRaw("Play Again") > 0.0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void ExecuteState_Shifting()
    {
        if (timeInState >= shiftTime)
        {
            StartNextLevel();
        }
    }

    private void ExitState_InMenus()
    {
        menuStart.SetActive(false);
    }

    private void ExitState_InLevel()
    {
        menuHUD.SetActive(false);
    }

    private void ExitState_InInGameMenus()
    {
        menuSkills.SetActive(false);
    }

    private void ExitState_InEndGameMenus()
    {
        menuEndGame.SetActive(false);
        menuEndSuccess.SetActive(false);
        menuEndFailure.SetActive(false);
    }

    private void ExitState_Shifting() {}

    public void OnPressStart()
    {
        SwitchToState(GameState.IN_LEVEL);

        // Give the player max health again
        thePlayer.health = thePlayer.maxHealth;

        this.mainMusic.Play();

        RoomController.instance.beginLevel();
        generateMonsters(thePlayer.upgrades.Count);
    }

    public void StartNextLevel()
    {

        flipBoss = !flipBoss;

        SwitchToState(GameState.IN_LEVEL);

        // Give the player max health again
        thePlayer.health = thePlayer.maxHealth;

        this.bossMusic.Stop();
        this.mainMusic.Play();

        // Set new start and end nodes

        // Tell generator to work on the next setup
        RoomController.instance.advanceLevel();
        generateMonsters(thePlayer.upgrades.Count);

    }


    public void generateMonsters(int level)
    {
        if (theBoss != null)
        {
            Transform[] forms = theBoss.gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform transform in forms)
            {
                Destroy(transform.gameObject);
            }
            Destroy(theBoss.gameObject);
        }

        foreach (RoomShape shape in RoomController.instance.roomShapes)
        {
            int XPForRoom = (level + 1) * 10 * shape.getWidth() * shape.getHeight();
            int attempts = 3;
            while(XPForRoom > 0 && attempts > 0)
            {
                int iRet = Random.Range(0, enemies.Length);
                Enemy enemy = enemies[iRet];
                if (enemy.XP > XPForRoom)
                {
                    attempts--;
                    continue;
                }

                XPForRoom -= enemy.XP;

                Enemy spawn = Instantiate<Enemy>(enemy);
                spawn.gameObject.SetActive(false);
                shape.dungeonPiece.pendingEnemies.Add(spawn);

            }
        }

        Boss bspawn = Instantiate<Boss>(boss);
        if(flipBoss)
        {
            bspawn.transform.position = RoomController.instance.goldRoomA.dungeonPiece.transform.position + new Vector3(2f, 2f, -3.5f);
            RoomController.instance.goldRoomA.dungeonPiece.boss = bspawn;
            bspawn.eyes[0].currentRoom = RoomController.instance.goldRoomA.dungeonPiece;
        }
        else
        {
            bspawn.transform.position = RoomController.instance.goldRoomB.dungeonPiece.transform.position + new Vector3(2f, 2f, -3.5f);
            RoomController.instance.goldRoomB.dungeonPiece.boss = bspawn;
            bspawn.eyes[0].currentRoom = RoomController.instance.goldRoomB.dungeonPiece;
        }
        theBoss = bspawn;
    }
}
