using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayableLevelInstance", menuName = "PlayableLevel")]
public class PlayableLevel : Level {

    [SerializeField]
    private int difficulty;

    [SerializeField]
    private List<Enemy> enemies;

    [SerializeField]
    private List<SpaceGarbage> obstacles;


    [SerializeField]
    private Material background;

    [SerializeField]
    private float timeUntilFirstEnemy;

    [SerializeField]
    private float timeUntilFirstGarbage;

    [SerializeField]
    private int maxEnemiesInScreen;

    [SerializeField]
    private float timeToBeat;

    [SerializeField]
    private int scoreToKeep;

    public int Difficulty { get => difficulty; }
    public List<Enemy> Enemies { get => enemies; }
    public List<SpaceGarbage> Obstacles { get => obstacles; }
    public float TimeUntilFirstEnemy { get => timeUntilFirstEnemy; }
    public float TimeUntilFirstGarbage { get => timeUntilFirstGarbage; }
    public int MaxEnemiesInScreen { get => maxEnemiesInScreen; }
    public float TimeToBeat { get => timeToBeat; }
    public int ScoreToKeep { get => scoreToKeep; }
    public Material Background { get => background; set => background = value; }
}
