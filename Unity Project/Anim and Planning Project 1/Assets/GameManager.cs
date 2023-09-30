using Assets.Physics;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton pattern 
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }


    //Static Objects
    public static List<AABBCollider> AABBs = new List<AABBCollider>();
    public static List<LineCollider> lines = new List<LineCollider>();
    public static List<CircleCollider> circles = new List<CircleCollider>();

    //Dynamic Objects
    public static List<Pinball> Pinballs = new List<Pinball>();
    public static List<Flipper> flippers = new List<Flipper>();
    public static Launcher launcher;


    //Register Methods
    public static int RegisterAABB(AABBCollider b)
    {
        AABBs.Add(b);
        return AABBs.Count - 1;

    }
    public static int RegisterCircle(CircleCollider c)
    {
        circles.Add(c);
        return circles.Count - 1;

    }
    public static int RegisterLine(LineCollider l)
    {
        lines.Add(l);
        return lines.Count - 1;

    }
    public static int RegisterPinball(Pinball p)
    {
        Pinballs.Add(p);
        return Pinballs.Count - 1;

    }
    public static void RegisterPlunger(Launcher l)
    {
        launcher = l;
    }


    //Important Game References
    public Transform PinballSpawnPoint;
    public Transform PinballDeathLine;
    public GameObject ScoreDisplay;
    public GameObject BallsRemainingDisplay;
    public GameObject PinballPrefab;
    public GameObject EntitiesContainer;
    public GameObject largeMessageUIObj;
    public static float killYLevel = 0.0f;

    //Game State variables
    public static bool PlayingGame = false;
    private static bool BallWaiting = false;
    private static int CurrentActivePinballs = 0;
    private static int PendingBalls = 0;
    private static float Score = 0;


    //Game State Management Functions
    public static bool StartNewGame()
    {
        //Start a new game
        if (PlayingGame == true) { return false; }

        //Reset Game Stats
        PlayingGame = true;
        Score = 0;
        PendingBalls = 3;

        //turn off text
        Instance.largeMessageUIObj.SetActive(false);

        //Update Displays
        UpdateBallsRemainingDisplay();
        UpdateScoreDisplay();

        //Spawn the starting pinball
        SpawnPinball();
        return true;
    
    }
    public static bool SpawnPinball()
    {
        //Do we already have a ball waiting in the launcher
        if (BallWaiting || !PlayingGame) { return false; }

        //Do we still have balls left?
        if (PendingBalls > 0)
        {
            //Otherwise create a new pinball
            CurrentActivePinballs++;
            PendingBalls--;
            UpdateBallsRemainingDisplay();

            //Create the new pinball
            GameObject pendingPinballObj = Instantiate(Instance.PinballPrefab, new Vector3(Instance.PinballSpawnPoint.position.x, Instance.PinballSpawnPoint.position.y, 0.0f), Quaternion.identity);
            pendingPinballObj.transform.SetParent(Instance.EntitiesContainer.transform, true);

            //Set flags and return
            BallWaiting = true;
            return true;
        }

        return false;
    }
    public static bool BallLaunched()
    {
        BallWaiting = false;
        return true;
    }
    public static bool FinishGame()
    {
        if (PlayingGame == false) { return false; }

        Debug.Log("Game is over");
        PlayingGame = false;
        
        //Set Gameover message
        SetLargeTextMsg("Game Over!\n High Score: " + Score);

        return true;
    }
    public static void PinballDie(Pinball p)
    {
        //Called when a pinball 
        //remove the pinball
        CurrentActivePinballs -= 1;
        Pinballs.Remove(p);        

        if (CurrentActivePinballs == 0 && PendingBalls == 0)
        {
            //Game Over
            FinishGame();
        }
    }
    public static void IncreaseScore(float score)
    {
        Score += score;
        UpdateScoreDisplay();

    }


    //UI Helper Methods
    public static void UpdateScoreDisplay()
    {
        Instance.ScoreDisplay.GetComponent<TextMeshProUGUI>().text = "Score: " + Score;
    }
    public static void UpdateBallsRemainingDisplay()
    {
        Instance.BallsRemainingDisplay.GetComponent<TextMeshProUGUI>().text = "Remaining Balls: " + PendingBalls;
    }
    public static void SetLargeTextMsg(string msg)
    {
        Instance.largeMessageUIObj.GetComponent<TextMeshProUGUI>().text = msg;
        Instance.largeMessageUIObj.SetActive(true);
    }


    //Gameobject Instance Methods
    public void Awake()
    {
        //Singleton pattern checks
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        //Set the kill level Y
        killYLevel = PinballDeathLine.transform.position.y;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SpawnPinball();
        }
    }
    public void StartButtonCall()
    {
        GameManager.StartNewGame();
    }

}
