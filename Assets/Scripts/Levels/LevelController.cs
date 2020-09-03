using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public enum GameMode {
    HistoryMode, ArcadeMode
}
public class LevelController : MonoBehaviour {
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private TextMeshProUGUI TimeValueText, CreditsValueText;
    [SerializeField]
    private GameObject spawnersEnemyParent;
    [SerializeField]
    private KillZoneController killZoneController;

    [SerializeField]
    private HomePositionController homePositionController;

    [SerializeField]
    private GameObject initPosition;

    [SerializeField]
    private Goal goalPosition;

    [SerializeField]
    private PoolManager poolManager;

    [SerializeField]
    private AudioSource musicLevel;

    private PlayerController player;
    //Objects Intantiated USAR ESTOS
    private List<GameObject> enemiesInstancesList;
    private List<GameObject> garbageInstancesList;


    private GameObject timerUntilFirstEnemyInstantiated;
    private GameObject timerUntilFirstGarbageInstantiated;

    private GameController gameController;
    private SoundManager soundManager;

    //Canvas
    private CanvasButton canvasButton;
    private CanvasInputController canvasInputController;


    [SerializeField]
    private InfoLevelInGame infoLevel;

    private PlayableLevel selectedPlayableLevel;

    float timeAtGoal = 0.0f;

    private void Awake()
    {

        gameController = GameController.Instance;
        soundManager = SoundManager.Instance;
        infoLevel.Init();

        gameController.Initialize();

        PlayerShip playerPrefab = null;

        if (gameController.selectedPlayerShip == null)
        {
            playerPrefab = Resources.Load<GameObject>("Prefabs/PlayerShips/NavePlayer").GetComponent<PlayerController>().Player;
        }
        else
        {
            playerPrefab = gameController.selectedPlayerShip;
        }
        gameController.selectedPlayerShip = playerPrefab;

        GameObject playerInstantiated = GameObject.Instantiate(playerPrefab.Prefab);
        playerInstantiated.transform.localPosition = initPosition.transform.position;
        player = playerInstantiated.GetComponent<PlayerController>();
        player.SetPlayerStatsInfo(infoLevel);

        killZoneController.playerController = player;

        enemiesInstancesList = new List<GameObject>();
        garbageInstancesList = new List<GameObject>();

        poolManager.LoadSpawners(spawnersEnemyParent);
        poolManager.LoadInfoLevel(infoLevel);
    }



    // Start is called before the first frame update
    void Start()
    {
        canvasButton = canvas.GetComponent<CanvasButton>();
        canvasInputController = canvas.GetComponent<CanvasInputController>();
        gameController.PauseGame(false);


        TimeValueText.gameObject.SetActive(true);
        timerUntilFirstEnemyInstantiated = poolManager.InstantiateTimer();
        timerUntilFirstEnemyInstantiated.name = "timerUntilFirstEnemyInstantiated";

        timerUntilFirstGarbageInstantiated = poolManager.InstantiateTimer();
        timerUntilFirstEnemyInstantiated.name = "timerUntilFirstEnemyInstantiated";


        string name = SceneManager.GetActiveScene().name;
        gameController.selectedLevel = gameController.PlayableLevels.GetLevelBySceneName(name);
        selectedPlayableLevel = (PlayableLevel)gameController.selectedLevel;
        infoLevel.Credits = selectedPlayableLevel.ScoreToKeep;
        CreditsValueText.text = infoLevel.Credits.ToString();

        poolManager.EnemiesOfLevel = selectedPlayableLevel.Enemies;
        poolManager.GarbageOfLevel = selectedPlayableLevel.Obstacles;

        //Inicio Timers
        if (selectedPlayableLevel.Enemies.Count > 0)
        {
            timerUntilFirstEnemyInstantiated.GetComponent<Timer>().WaitTime = selectedPlayableLevel.TimeUntilFirstEnemy;
            timerUntilFirstEnemyInstantiated.GetComponent<Timer>().Reset();
        }
        if (selectedPlayableLevel.Obstacles.Count > 0)
        {
            timerUntilFirstGarbageInstantiated.GetComponent<Timer>().WaitTime = selectedPlayableLevel.TimeUntilFirstGarbage;
            timerUntilFirstGarbageInstantiated.GetComponent<Timer>().Reset();
        }

        //Suscripcion a eventos
        timerUntilFirstEnemyInstantiated.GetComponent<Timer>().OnTimeDone += OnTimeToReleaseEnemies;
        timerUntilFirstGarbageInstantiated.GetComponent<Timer>().OnTimeDone += OnTimeToReleaseGarbage;

        infoLevel.OnPlayerZeroPoints += PlayerDead;

        canvasButton.OnPauseGame += OnPauseGame;
        canvasButton.OnExitLevel += OnExitLevel;
        canvasButton.OnRestartLevel += OnRestartlevel;
        canvasButton.OnGoHomeResponse += OnGoHomeResponse;
        canvasButton.OnNextLevel += OnNextLevel;

        player.OnPlayerMove += OnPlayerMoveSetSpawnerPos;
        player.OnPlayerMove += OnPlayerMoveKillZonePos;
        player.OnCanShootExpansiveWave += canvasButton.OnEnableExpansiveWaveButton;
        player.OnCanShootNeutralizer += canvasButton.OnEnableNeutralizerButton;
        player.OnGetDamage += OnPlayerDamage;
        homePositionController.OnGoHomePosition += OnGoHomePosition;
        goalPosition.OnGoalReached += OnGoalReached;

        infoLevel.Lifes = player.Player.Life + player.Player.LifeImprove;

        canvasButton.OnChangeLife(infoLevel.Lifes);
        canvasButton.SetBackground();
        canvasButton.Info = infoLevel;
        CreditsValueText.gameObject.SetActive(false);

        if (
          (gameController.selectedLevel == gameController.PlayableLevels.GetLevelBySceneName("Level4") && gameController.GameMode == GameMode.ArcadeMode) ||
         (gameController.selectedLevel != gameController.PlayableLevels.GetLevelBySceneName("Level4")))
        {
            CreditsValueText.gameObject.SetActive(true);
        }
    }

    private void OnNextLevel()
    {
        int idNextLevel = gameController.selectedLevel.IdLevel + 1;
        musicLevel.Stop();
        soundManager.StopSounds();
        gameController.PauseGame(false);

        if (idNextLevel <= gameController.PlayableLevels.levels.Count)
        {
            Level nextLevel;

            nextLevel = gameController.PlayableLevels.GetLevelById(idNextLevel);
            gameController.selectedLevel = nextLevel;
            if (gameController.GameMode == GameMode.ArcadeMode)
            {
                SceneManager.LoadScene(nextLevel.SceneName);
            }
            else
            {
                SceneManager.LoadScene("ShowInitialInfo");
            }
        }
        else
        {
            SceneManager.LoadScene("InitialMenu");
        }
    }



    #region Events
    private void OnChangeLifePlayer(float life)
    {
        soundManager.PlayEffectPlayerHit();
        StartCoroutine(nameof(Spark));
    }


    public void OnPlayerDamage()
    {
        infoLevel.Lifes--;
        canvasButton.OnChangeLife(infoLevel.Lifes);

        if (infoLevel.Lifes <= 0)
        {
            PlayerDead();
        }
        else
        {
            OnChangeLifePlayer(infoLevel.Lifes);
        }
    }

    public void OnGoalReached()
    {
        timeAtGoal = Time.timeSinceLevelLoad;
        //TODO CHECK If  infoLevel.Time == timeAtGoal
        ShowWinScreen();
    }

    public void OnPauseGame(bool menuPauseState)
    {
        gameController.PauseGame(menuPauseState);
        if (menuPauseState)
        {
            player.StopSounds();
        }
        else
        {
            player.PlayShipMovingEffect();
        }
    }

    public void OnGoHomePosition()
    {
        canvasButton.ShowGoHomeMenu(true);
        gameController.PauseGame(true);
    }

    public void OnGoHomeResponse(bool status)
    {
        canvasButton.ShowGoHomeMenu(false);
        if (status)
        {
            Debug.Log("SALIMOS NIVEL");
            SceneManager.LoadScene(Scenes.SCENE_MENU_INICIAL);
            //salir del nivel
        }
        else
        {
            //Reset position
            player.transform.position = initPosition.transform.position;
            player.transform.rotation = initPosition.transform.rotation;
        }
        gameController.PauseGame(false);
    }
    #endregion


    private void OnDestroy()
    {
        if (canvasButton != null)
        {
            canvasButton.OnPauseGame -= OnPauseGame;
            canvasButton.OnGoHomeResponse -= OnGoHomeResponse;
            canvasButton.OnRestartLevel -= OnRestartlevel;
            canvasButton.OnGoHomeResponse -= OnGoHomeResponse;
            canvasButton.OnNextLevel -= OnNextLevel;

        }
        if (player != null && canvasButton != null)
        {
            player.OnCanShootExpansiveWave -= canvasButton.OnEnableExpansiveWaveButton;
            player.OnCanShootNeutralizer -= canvasButton.OnEnableNeutralizerButton;
            player.OnGetDamage -= OnPlayerDamage;
            player.OnPlayerMove -= OnPlayerMoveSetSpawnerPos;
        }
        if (timerUntilFirstEnemyInstantiated != null)
        {
            timerUntilFirstEnemyInstantiated.GetComponent<Timer>().OnTimeDone -= OnTimeToReleaseEnemies;
        }
        if (homePositionController != null)
        {
            homePositionController.OnGoHomePosition -= OnGoHomePosition;
        }
        if (timerUntilFirstGarbageInstantiated != null)
        {
            timerUntilFirstGarbageInstantiated.GetComponent<Timer>().OnTimeDone -= OnTimeToReleaseGarbage;
        }

        if (goalPosition != null)
        {
            goalPosition.OnGoalReached -= OnGoalReached;
        }
    }

    #region Spawners

    private void OnPlayerMoveSetSpawnerPos(float moveRight)
    {
        foreach (Transform item in spawnersEnemyParent.GetComponentInChildren<Transform>())
        {
            if (item.gameObject != spawnersEnemyParent.gameObject)
            {
                Spawner spawner = item.gameObject.GetComponent<Spawner>();
                if (player.transform.position.x > -18)
                {
                    spawner.Move(item.gameObject.transform.position.x + moveRight);
                }
            }
        }
    }

    private void OnPlayerMoveKillZonePos(float moveRight)
    {
        killZoneController.Move(killZoneController.gameObject.transform.position.x + moveRight);
    }



    #endregion

    // Update is called once per frame
    void Update()
    {
        if (!gameController.GamePaused)
        {
            //Debug.Log(Time.realtimeSinceStartup);
            float actualTime = selectedPlayableLevel.TimeToBeat - Time.timeSinceLevelLoad;
            actualTime = (float)(Math.Round((double)actualTime, 1));
            if (actualTime <= 0)
            {
                actualTime = 0.0f;
                PlayerDead();
            }
            infoLevel.Time = actualTime;
            TimeValueText.text = string.Format("Time {0}", infoLevel.Time);
            SetCredits();
        }
    }

    private void SetCredits()
    {
        //playerScore =;
        string creditsText = "Credits";
        CreditsValueText.text = String.Format(creditsText + " {0}", infoLevel.Credits);
    }

    public void PlayerDead()
    {
        if (player)
        {

            player.gameObject.SetActive(false);
            player.StopSounds();
            //player.CanMove = false;
            //player.StopSounds();
            musicLevel.Stop();
            canvas.GetComponentInChildren<OffsetScroller>().Stop();
            if (infoLevel.Lifes <= 0)
            {
                //Instantiate explosion
                GameObject explosion = poolManager.GetExplosion();
                explosion.transform.position = player.transform.position;
            }
            Invoke(@"ShowInfoDead", 1.5f);
        }

    }

    private void ShowInfoDead()
    {
        soundManager.PlayMusicDead();

        gameController.PauseGame(true);
        canvasButton.ShowStats();
        canvasButton.ShowLostScreen();
    }

    private void ShowWinScreen()
    {
        TimeValueText.gameObject.SetActive(false);

        player.CanMove = false;
        canvasButton.ShowStats();

        gameController.PauseGame(true);
        player.StopSounds();
        musicLevel.Stop();
        //Put music win
        soundManager.PlayMusicWin();

        //TODO Quitar cuando se acaben pruebas
        if (!gameController.selectedLevel)
        {
            gameController.selectedLevel = gameController.PlayableLevels.levels[0];
        }


        int credits = infoLevel.Credits;
        canvasButton.SetValuesStats(credits);
        if (gameController.GameMode == GameMode.HistoryMode)
        {
            gameController.PersistentData.IdLastLevel = gameController.selectedLevel.IdLevel;
        }

        gameController.PersistentData.AddCredits(gameController.selectedLevel, credits);
        gameController.PersistentData.Credits += credits;
        gameController.PersistentData.AddTime(gameController.selectedLevel, timeAtGoal);
        if (gameController.GameMode == GameMode.ArcadeMode)
        {

            int idActualLevel = gameController.selectedLevel.IdLevel;
            if (idActualLevel + 1 <= gameController.PlayableLevels.levels.Count)
            {
                gameController.PersistentData.Unlocklevel(idActualLevel + 1);
            }
        }
        gameController.SaveInformation();
    }





    public void OnExitLevel()
    {
        soundManager.StopSounds();
        player.StopSounds();
        SceneManager.LoadScene(Scenes.SCENE_MENU_INICIAL);
    }

    public void OnRestartlevel()
    {
        soundManager.StopSounds();

        //Debug.Log("Restart");
        SceneManager.LoadScene(gameController.selectedLevel.SceneName);
    }

    IEnumerator Spark()
    {
        while (!player.CanBeDamaged)
        {
            player.ShipMesh.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            player.ShipMesh.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    private void OnTimeToReleaseEnemies()
    {

        //Si añadimos tantos spawners como MaxEnemiesInScreen => OK
        for (int enemiesInScreen = 0; enemiesInScreen < selectedPlayableLevel.MaxEnemiesInScreen; enemiesInScreen++)
        {
            ReleaseEnemy();
        }
    }

    private void ReleaseEnemy()
    {

        Spawner randomSpawner = poolManager.GetRandomSpawner();
        if (randomSpawner)
        {
            poolManager.DeactivateTemporallySpawner(randomSpawner);

            if (poolManager.ActiveElements(enemiesInstancesList).Count < selectedPlayableLevel.MaxEnemiesInScreen)
            {
                GameObject enemyInstance = poolManager.InstantiateEnemy(randomSpawner);
                enemiesInstancesList.Add(enemyInstance);
            }
        }
        else
        {
            Invoke("ReleaseEnemy", 0.3f);

        }
    }

    private void OnTimeToReleaseGarbage()
    {
        for (int i = 0; i < poolManager.GarbageOfLevel.Count; i++)
        {
            ReleaseGarbage();
        }
    }

    private void ReleaseGarbage()
    {
        Spawner randomSpawner = poolManager.GetRandomSpawner();
        if (randomSpawner)
        {
            poolManager.DeactivateTemporallySpawner(randomSpawner);

            if (poolManager.ActiveElements(garbageInstancesList).Count < selectedPlayableLevel.MaxEnemiesInScreen && randomSpawner)
            {
                GameObject gargabePrefabInstantiated = poolManager.InstantiateGargabe(randomSpawner);
                garbageInstancesList.Add(gargabePrefabInstantiated);
            }
        }
        else
        {
            Invoke("ReleaseGarbage", 0.3f);
        }
    }
}
