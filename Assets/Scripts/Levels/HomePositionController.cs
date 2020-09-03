using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePositionController : MonoBehaviour
{
    public event Action OnGoHomePosition = delegate { };
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        EnemyShipController enemyController = other.gameObject.GetComponent<EnemyShipController>();
        LaserController laserController = other.gameObject.GetComponent<LaserController>();
        SpaceGarbageController spacegarbage = other.gameObject.GetComponent<SpaceGarbageController>();

        if (other.GetComponent<PlayerController>())
        {
            //Mostrar ventana
            OnGoHomePosition();
        }
        if(enemyController){

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
}
