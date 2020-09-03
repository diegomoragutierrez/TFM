using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    // Start is called before the first frame update
    public event Action<GameObject> OnPassThrougLimitSpawner = delegate { };
    private bool canUse = true;
    private bool move = true;

    [SerializeField]
    private Transform spawnPoint;

    public bool CanUse { get => canUse; set => canUse = value; }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "EndMovingSpawner")
        {
            OnStopMoveSpawner(this.gameObject);
            //OnPassThrougLimitSpawner(this.gameObject);
        }
    }

    private void OnStopMoveSpawner(GameObject spawner)
    {
        spawner.GetComponent<Spawner>().move = false;
    }

    public void Move(float right)
    {
        if (move)
        {
            transform.position = new Vector3(right, transform.position.y, 0);
        }
    }

    public GameObject Spawn(GameObject enemyPrefab)
    {
        GameObject instatiatedObject = Instantiate(enemyPrefab);
        instatiatedObject.transform.position = spawnPoint.transform.position;
        instatiatedObject.transform.rotation = Quaternion.identity;
        //instatiatedObject.transform.rotation = spawnPoint.transform.rotation;
        return instatiatedObject;
    }


    public void CanUseAgain()
    {
        canUse = true;
    }
}

