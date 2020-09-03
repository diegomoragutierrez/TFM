using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyShipInstance", menuName = "EnemyShip")]
public class Enemy : ScriptableObject {
    [SerializeField]
    private int id;

    [SerializeField]
    private float speed;

    [SerializeField]
    private int waitTimeToReset;

    [SerializeField]
    private GameObject shipPrefab;

    [SerializeField]
    private GameObject laserPrefab;

    [SerializeField]
    private float speedLaser;

    [SerializeField]
    private int waitTimeToShoot;

    [SerializeField]
    private int maxSizePoolLasers;

    [SerializeField]
    private int pointsWhenDestroyed;

    [SerializeField]
    private AudioClip soundAtDestroy;

    [SerializeField]
    private AudioClip soundEngine;




    public int Id { get => id; set => id = value; }
    public float Speed { get => speed; set => speed = value; }
    public int WaitTimeToReset { get => waitTimeToReset; set => waitTimeToReset = value; }
    public GameObject ShipPrefab { get => shipPrefab; set => shipPrefab = value; }
    public GameObject LaserPrefab { get => laserPrefab; set => laserPrefab = value; }
    public float SpeedLaser { get => speedLaser; set => speedLaser = value; }
    public int WaitTimeToShoot { get => waitTimeToShoot; set => waitTimeToShoot = value; }
    public int MaxSizePoolLasers { get => maxSizePoolLasers; set => maxSizePoolLasers = value; }
    public int PointsWhenDestroyed { get => pointsWhenDestroyed; set => pointsWhenDestroyed = value; }
    public AudioClip SoundAtDestroy { get => soundAtDestroy; set => soundAtDestroy = value; }
    public AudioClip SoundEngine { get => soundEngine; set => soundEngine = value; }
}

