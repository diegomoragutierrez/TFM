using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipController : EnemyBaseController {
    //private float deadTime;

    private Rigidbody body;
    private SoundManager soundManager;

    [SerializeField]
    private Enemy enemy;

    [SerializeField]
    private GameObject ShotPoint;

    private List<LaserController> poolRedLaser;

    private bool hittedByPlayer, neutralizedByPlayer;

    private bool canShoot;

    private GameObject timerToShoot;

    private AudioSource audioSource;

    public Enemy Enemy { get => enemy; set => enemy = value; }
    public bool HittedByPlayer { get => hittedByPlayer; set => hittedByPlayer = value; }
    public bool NeutralizedByPlayer { get => neutralizedByPlayer; set => neutralizedByPlayer = value; }

    public event Action<EnemyShipController> OnDesactivate = delegate { };
    // Start is called before the first frame update

    private void Awake()
    {
        //timerToAppear = Instantiate(timerPrefab);
        timerToShoot = Instantiate(timerPrefab);
        timerToShoot.name = "TimerToShoot";
        poolRedLaser = new List<LaserController>();
        soundManager = SoundManager.Instance;
        for (int i = 0; i < enemy.MaxSizePoolLasers; i++)
        {
            LaserController lasers = Instantiate(enemy.LaserPrefab).GetComponent<LaserController>();
            lasers.gameObject.SetActive(false);
            poolRedLaser.Add(lasers);
        }
    }
    void Start()
    {
        //deadTime = 0;
        body = GetComponent<Rigidbody>();

        //timerToAppear.SetActive(false);
        //timerToAppear.GetComponent<Timer>().OnTimeDone += OnCanAppear;

        timerToShoot.GetComponent<Timer>().WaitTime = enemy.WaitTimeToShoot;
        timerToShoot.GetComponent<Timer>().OnTimeDone += OnCanShoot;
        timerToShoot.GetComponent<Timer>().Reset();

        soundManager.AddEnemyShipExplodingAudioSource(ref audioSource);
        audioSource.clip = Enemy.SoundAtDestroy;

        for (int i = 0; i < poolRedLaser.Count; i++)
        {
            poolRedLaser[i].gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        //if (timerToAppear != null)
        //    timerToAppear.GetComponent<Timer>().OnTimeDone -= OnCanAppear;

        if (timerToShoot != null)
            timerToShoot.GetComponent<Timer>().OnTimeDone -= OnCanShoot;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Live " + live + " " + "Active " + isActiveAndEnabled);
        if (gameObject.activeInHierarchy)
        {
            if (!hittedByPlayer)
            {
                Move();

                if (canShoot)
                {
                    Shoot();
                }
            }
        }
    }

    private void Shoot()
    {
        LaserController laserInactive = SearchLaser();
        if (laserInactive != null)
        {
            soundManager.PlayEnemyShootEffect();
            laserInactive.Speed = enemy.SpeedLaser;
            laserInactive.gameObject.SetActive(true);
            laserInactive.transform.position = ShotPoint.transform.position;
            canShoot = false;
            timerToShoot.SetActive(true);
            timerToShoot.GetComponent<Timer>().Reset();
        }
    }


    private LaserController SearchLaser()
    {
        LaserController laserInactive = poolRedLaser.Find(x => !x.isActiveAndEnabled);

        return laserInactive;
    }

    private void Move()
    {
        body.MovePosition(body.position + Vector3.left * Time.deltaTime * enemy.Speed);
    }

    public void Deactivate()
    {
        timerToShoot.SetActive(false);
        if (hittedByPlayer)
        {
            soundManager.PlayEnemyShipExploding(audioSource);
        }
        else if (NeutralizedByPlayer)
        {
            soundManager.PlayNeutralizerHitAtEnemyShip();

        }

        OnDesactivate(this);

        gameObject.SetActive(false);
    }

    public void OnCanShoot()
    {
        canShoot = true;
    }

    public override void Reset()
    {
        gameObject.SetActive(true);
        timerToShoot.SetActive(true);
        timerToShoot.GetComponent<Timer>().Reset();
        hittedByPlayer = false;
    }
    //private void OnParticleCollision(GameObject other)
    //{
    //    //Debug.Log("Collision");
    //    defeatedByPlayer = true;
    //    Deactivate();
    //}

    private void OnEnable()
    {
        //Debug.Log("ON Enable");
        timerToShoot.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollisions(collision.gameObject);
    }

    private void CheckCollisions(GameObject collision)
    {
        EnemyShipController enemyShipController = collision.GetComponent<EnemyShipController>();
        SpaceGarbageController spaceGarbageController = collision.GetComponent<SpaceGarbageController>();
        NeutralizerMove neutralizerMove = collision.GetComponent<NeutralizerMove>();
        if (enemyShipController)
        {
            enemyShipController.Deactivate();
            Deactivate();
        }
        if (spaceGarbageController)
        {
            spaceGarbageController.Deactivate();
            Deactivate();
        }
        if (neutralizerMove)
        {
            hittedByPlayer = true;

            Deactivate();
            neutralizerMove.Deactivate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckCollisions(other.gameObject);
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    NeutralizerMove neutralizerMove = other.GetComponent<NeutralizerMove>();

    //    if (neutralizerMove)
    //    {
    //        //soundManager.PlayNeutralizerRayEffectInEnemyShip();
    //    }
    //}
}
