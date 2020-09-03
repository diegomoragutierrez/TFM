using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {
    [SerializeField]
    private float speed;

    private Rigidbody body;

    public float Speed { get => speed; set => speed = value; }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        body.MovePosition(body.position + Vector3.left * Time.deltaTime * Speed);
    }

    internal void Deactivate()
    {
        gameObject.SetActive(false);
    }
    private void OnParticleCollision(GameObject other)
    {
        Deactivate();
    }

    private void OnCollisionEnter(Collision other)
    {
        CheckCollisions(other.gameObject);
    }

    private void CheckCollisions(GameObject other)
    {
        LaserController laser = other.GetComponent<LaserController>();
        SpaceGarbageController spaceGarbage = other.GetComponent<SpaceGarbageController>();
        EnemyShipController enemyShipController = other.GetComponent<EnemyShipController>();

        if (laser)
        {
            laser.Deactivate();
            Deactivate();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        CheckCollisions(other.gameObject);

    }
}
