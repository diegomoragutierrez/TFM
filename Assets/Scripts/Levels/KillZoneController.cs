using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZoneController : MonoBehaviour {
    internal PlayerController playerController;
    private void Start()
    {
        transform.position = playerController.gameObject.transform.position - Vector3.right * 10;
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyShipController enemyController = other.gameObject.GetComponent<EnemyShipController>();
        LaserController laserController = other.gameObject.GetComponent<LaserController>();
        SpaceGarbageController spacegarbage = other.gameObject.GetComponent<SpaceGarbageController>();

        if (enemyController)
        {
            enemyController.Deactivate();
        }
        if (laserController)
        {
            laserController.Deactivate();
        }
        if (spacegarbage)
        {
            spacegarbage.Deactivate();
        }
    }

    public void Move(float right)
    {
        transform.position = new Vector3(right, transform.position.y, 0);
    }
}
