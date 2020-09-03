using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Prefab(nameof(GameController), true)]
public class GameController : Singleton<GameController> {

    [SerializeField]
    internal PlayerShip selectedPlayerShip;
    [SerializeField]
    internal Level selectedLevel;

    [SerializeField]
    private bool gamePaused;

    [SerializeField]
    private GameMode gameMode;

    public List<Enemy> enemies;

    public List<SpaceGarbage> garbage;

    [SerializeField]
    private bool vibration = false;

    [SerializeField]
    private PersistentData persistentData;

    [SerializeField]
    private GUILevel playableLevels;

    [SerializeField]
    private GUILevel textLevels;

    [SerializeField]
    private GUIPlayerShip playerShipUIInfo;

    public PersistentData PersistentData { get => persistentData; }
    public GUILevel PlayableLevels { get => playableLevels; }
    public GUIPlayerShip PlayerShipUIInfo { get => playerShipUIInfo; }
    public bool Vibration { get => vibration; set => vibration = value; }
    public bool GamePaused { get => gamePaused; set => gamePaused = value; }
    public GUILevel Textlevels { get => textLevels; }
    public bool ReadHowToPlay { get => persistentData.ReadHowToPlay; set => persistentData.ReadHowToPlay = value; }
    public GameMode GameMode { get => gameMode; set => gameMode = value; }

    internal void PauseGame(bool value)
    {
        Time.timeScale = value ? 0 : 1;
        gamePaused = value;
    }

    private void GetPlatform()
    {

    }
    public void SaveInformation()
    {
        string data = JsonUtility.ToJson(PersistentData);
        PlayerPrefs.SetString("UserData", data);
    }

    public void ReadInformation()
    {
        string data = PlayerPrefs.GetString("UserData");
        JsonUtility.FromJsonOverwrite(data, PersistentData);
    }

    public void Initialize()
    {
        if (PersistentData == null)
        {
            persistentData = new PersistentData();
        }
        ReadInformation();
        //gameMode = GameMode.ArcadeMode;
        //persistentData.Credits = 1000;
        //SaveInformation();
        PersistentData.Unlocklevel(0);
    }
    public List<Level> GetLevelsUnlocked()
    {
        List<Level> levelsUnlocked = new List<Level>();
        foreach (int item in PersistentData.LevelsUnlocked)
        {
            Level levelUnlocked = PlayableLevels.GetLevelById(item);
            if (levelUnlocked)
            {
                levelsUnlocked.Add(levelUnlocked);
            }
        }
        return levelsUnlocked;
    }

    public bool LevelUnlocked(Level level)
    {
        return PersistentData.LevelsUnlocked.Contains(level.IdLevel);
    }

    public bool LevelUnlocked(int idLevel)
    {
        return PersistentData.LevelsUnlocked.Contains(idLevel);
    }
}
