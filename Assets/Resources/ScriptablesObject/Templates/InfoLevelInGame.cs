using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "InfoLevelInGameInstance", menuName = "InfoLevelInGame")]

public class InfoLevelInGame : ScriptableObject {
    [SerializeField]
    private int credits;

    [SerializeField]
    private float lifes;

    [SerializeField]
    private float time;

    [SerializeField]
    private List<EnemyShipController> enemyDefeated;

    private GameController gameController;
    private PlayableLevel selectedPlayableLevel;

    public int Credits { get => credits; set => credits = value; }
    public List<EnemyShipController> EnemyDefeated { get => enemyDefeated; set => enemyDefeated = value; }
    public float Lifes { get => lifes; set => lifes = value; }
    public float Time { get => time; set => time = value; }

    public event Action OnPlayerZeroPoints = delegate { };
    public void Init()
    {
        lifes = 0;
        time = 0;
        credits = 0;
        gameController = GameController.Instance;
        if (!gameController.selectedLevel)
        {
            gameController.selectedLevel = gameController.PlayableLevels.levels[0];
        }
        selectedPlayableLevel = (PlayableLevel)gameController.selectedLevel;
        credits = selectedPlayableLevel.ScoreToKeep;

        if (enemyDefeated == null)
        {
            enemyDefeated = new List<EnemyShipController>();
        }
    }

    public void AddEnemyDefeated(EnemyShipController enemy)
    {
        enemyDefeated.Add(enemy);
        if (
            (gameController.selectedLevel == gameController.PlayableLevels.GetLevelBySceneName("Level4") && gameController.GameMode == GameMode.ArcadeMode) ||
           ( gameController.selectedLevel != gameController.PlayableLevels.GetLevelBySceneName("Level4")))
        {
            credits -= enemy.Enemy.PointsWhenDestroyed;

            if (credits <= 0)
            {
                OnPlayerZeroPoints();
                credits = 0;
            }

        }
    }
}
