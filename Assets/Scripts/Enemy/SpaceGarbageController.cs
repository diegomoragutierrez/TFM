using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceGarbageController : EnemyBaseController {

    private Rigidbody body;

    //private float DeadTime;

    //private bool defeatedByPlayer;

    [SerializeField]
    private SpaceGarbage spaceGarbage;

    SoundManager soundManager;
    public SpaceGarbage SpaceGarbage { get => spaceGarbage; set => spaceGarbage = value; }

    public event Action<SpaceGarbageController> OnDesactivate = delegate { };

    void Start()
    {
        //DeadTime = 0;
        soundManager = SoundManager.Instance;
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Live " + live + " " + "Active " + isActiveAndEnabled);
        if (gameObject.activeInHierarchy)
        {
            Move();
        }
    }


    public void Move()
    {
        transform.localPosition = body.position + Vector3.left * Time.deltaTime * spaceGarbage.Speed;
        transform.eulerAngles += 3.0f * transform.forward;
    }


    public override void Reset()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        OnDesactivate(this);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckCollisions(other.gameObject);
    }

    private void CheckCollisions(GameObject gameObject)
    {
        EnemyShipController enemyShipController = gameObject.GetComponent<EnemyShipController>();
        SpaceGarbageController spaceGarbageController = gameObject.GetComponent<SpaceGarbageController>();
        LaserController laserController = gameObject.GetComponent<LaserController>();
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
        if (laserController)
        {
            laserController.Deactivate();
            Deactivate();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        //defeatedByPlayer = true;
        soundManager.PlayGarbageDefeated();

        Deactivate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollisions(collision.gameObject);
    }
}
