using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Engine.UI;
using UnityEngine.Events;

[System.Serializable]
public struct LevelSettings 
{
    public int brokenWoodPieces;
    public int brokenRailsPieces;
    public float gameTime;
    public GameObject scenarioLevel;
    public float spawnTrainEverySeconds;
    public Transform brokenPartsParent;
}

public class GameManager : MonoBehaviour
{
    //[SerializeField] float gameTime;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] UnityEvent onStartLoad;
    [SerializeField] UnityEvent onRestartLoad;
    [SerializeField] UnityEvent onGameOver;
    [SerializeField] UnityEvent onNextLevelLoad;

    //[SerializeField] Transform woodBreakable;
    [SerializeField] GameObject brokenRail;

    [SerializeField] GameObject trainPrefab;


    [SerializeField] LevelSettings[] settings;


    [SerializeField] UnityEvent onGameComplete;


    private float maxLeft = -5f;
    private float maxRight = 10f; 

    private int activeLevel;

    public bool IsGameActive { get; private set; }
    public bool IsGameFinished { get; private set; }
    private float actualTime;

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        timeText.text = settings[0].gameTime.ToString("F0");
    }

    private bool isStartingGame;


    public Transform GetBrokenPartsParent()
    {
        return settings[activeLevel].brokenPartsParent;
    }

    public void StartGame(int level)
    {
        if (IsGameActive || isStartingGame)
            return;

        activeLevel = level;

        StartCoroutine(StartGameRoutine(false));

        
    }

    public void NextLevel()
    {

        if(activeLevel + 1 >= settings.Length)
        {
            //Finish Game
            onGameComplete.Invoke();

            //Clean
            var fixes = FindObjectsOfType<FixInteraction>();
            for (int i = 0; i < fixes.Length; i++)
            {
                if (fixes[i].IsBroken)
                    fixes[i].ForceFix();
            }

            var trains = FindObjectsOfType<Train>();
            for (int i = 0; i < trains.Length; i++)
            {
                Destroy(trains[i].gameObject);
            }
            var wagons = FindObjectsOfType<Wagon>();
            for (int i = 0; i < wagons.Length; i++)
            {
                Destroy(wagons[i].gameObject);
            }
        }
        else
        {
            //Clean
            var fixes = FindObjectsOfType<FixInteraction>();
            for (int i = 0; i < fixes.Length; i++)
            {
                if (fixes[i].IsBroken)
                    fixes[i].ForceFix();
            }

            var trains = FindObjectsOfType<Train>();
            for (int i = 0; i < trains.Length; i++)
            {
                Destroy(trains[i].gameObject);
            }
            var wagons = FindObjectsOfType<Wagon>();
            for (int i = 0; i < wagons.Length; i++)
            {
                Destroy(wagons[i].gameObject);
            }

            //Show next level
            
            StartCoroutine(NextLevelRoutine());

            
        }
    }

    IEnumerator NextLevelRoutine()
    {
        onNextLevelLoad.Invoke();
        yield return new WaitForSeconds(0.5f);

        settings[activeLevel].scenarioLevel.SetActive(false);
        activeLevel++;
        settings[activeLevel].scenarioLevel.SetActive(true);

        StartCoroutine(StartGameRoutine(true));
    }

    void RestartGame()
    {
        if (!IsGameActive)
            return;

        IsGameActive = false;

        StartCoroutine(RestartRoutine());
    }

    IEnumerator RestartRoutine()
    {
        var fixes = FindObjectsOfType<FixInteraction>();
        for (int i = 0; i < fixes.Length; i++)
        {
            if (fixes[i].IsBroken)
                fixes[i].ForceFix();
        }

        var trains = FindObjectsOfType<Train>();
        for(int i = 0; i < trains.Length; i++)
        {
            Destroy(trains[i].gameObject);
        }
        var wagons = FindObjectsOfType<Wagon>();
        for (int i = 0; i < wagons.Length; i++)
        {
            Destroy(wagons[i].gameObject);
        }

        yield return StartCoroutine(StartGameRoutine(true));
    }

    IEnumerator TrainsRoutine()
    {
        while(IsGameActive)
        {
            Instantiate(trainPrefab);
            yield return new WaitForSeconds(settings[activeLevel].spawnTrainEverySeconds);
        } 
    }

    IEnumerator StartGameRoutine(bool restart)
    {
        isStartingGame = true;

        if(!restart)
        {
            yield return new WaitForSeconds(2.5f);

            onStartLoad.Invoke();
        }
        else
        {
            onRestartLoad.Invoke();
        }
        

        yield return new WaitForSeconds(0.5f);

        timeText.text = settings[activeLevel].gameTime.ToString("F0");

        //Wood Broken
        int woodQuantity = settings[activeLevel].brokenWoodPieces;
        int loopTries = 100;
        while(woodQuantity > 0 && loopTries > 0)
        {
            loopTries--;
            int random = Random.Range(0, settings[activeLevel].brokenPartsParent.childCount);

            if (settings[activeLevel].brokenPartsParent.GetChild(random).position.x < maxLeft || settings[activeLevel].brokenPartsParent.GetChild(random).position.x > maxRight)
                continue;

            FixInteraction interaction = settings[activeLevel].brokenPartsParent.GetChild(random).GetComponent<FixInteraction>();
            if(interaction)
            {
                if(!interaction.IsBroken)
                {
                    interaction.Break();
                    woodQuantity--;
                }
            }
        }

        //Rail Broken
        int railsQuantity = settings[activeLevel].brokenRailsPieces;
        if(railsQuantity>0)
        {
            GameObject[] brokenRails = new GameObject[settings[activeLevel].brokenPartsParent.childCount];
            for (int i = 0; i < settings[activeLevel].brokenPartsParent.childCount; i++)
            {
                GameObject r = Instantiate(brokenRail, settings[activeLevel].brokenPartsParent.GetChild(i).position, settings[activeLevel].brokenPartsParent.GetChild(i).rotation) as GameObject;
                brokenRails[i] = r;
            }

            loopTries = 100;
            while (railsQuantity > 0 && loopTries > 0)
            {
                loopTries--;
                int random = Random.Range(0, brokenRails.Length);
                if (brokenRails[random].transform.position.x < maxLeft || brokenRails[random].transform.position.x > maxRight)
                    continue;
                FixInteraction interaction = brokenRails[random].GetComponent<FixInteraction>();
                if (interaction)
                {
                    if (!interaction.IsBroken)
                    {
                        interaction.Break();
                        railsQuantity--;
                    }
                }
            }
        }


        actualTime = settings[activeLevel].gameTime;
        IsGameActive = true;
        isStartingGame = false;
        IsGameFinished = false;

        if(trainRoutineVar != null)
            StopCoroutine(trainRoutineVar);

        //Train Start
        trainRoutineVar = StartCoroutine(TrainsRoutine());
    }

    Coroutine trainRoutineVar;

    private void Update()
    {
        if (!IsGameActive)
            return;

        actualTime -= Time.deltaTime;
        timeText.text = actualTime.ToString("F0");

        if (actualTime <= 0)
        {
            IsGameActive = false;
            IsGameFinished = true;
            timeText.text = "0";

            GameWin();
        }  


    }

    private void GameWin()
    {
        NextLevel();
    }

    public void GameOver()
    {
        if (trainRoutineVar != null)
            StopCoroutine(trainRoutineVar);

        //Restart Game
        RestartGame();
        onGameOver.Invoke();
    }
}
