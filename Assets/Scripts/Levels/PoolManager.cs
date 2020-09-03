using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour {
    List<Spawner> spawners;
    private GameController gameController;

    List<GameObject> timerGameObjects;
    List<GameObject> timerSpawners;

    [SerializeField]
    private GameObject timerPrefab, timerGameObjectPrefab, explosionGameObject;

    private InfoLevelInGame infoLevel;

    private List<Enemy> enemiesOfLevel;
    private List<SpaceGarbage> garbageOfLevel;

    private int enemiesDefeated;

    public List<Enemy> EnemiesOfLevel { get => enemiesOfLevel; set => enemiesOfLevel = value; }
    public List<SpaceGarbage> GarbageOfLevel { get => garbageOfLevel; set => garbageOfLevel = value; }
    public int EnemiesDefeated { get => enemiesDefeated; set => enemiesDefeated = value; }

    private void Awake()
    {
        gameController = GameController.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        timerGameObjects = new List<GameObject>();
        timerSpawners = new List<GameObject>();

        for (int i = 0; i < spawners.Count * 3; i++)
        {
            GameObject timerGameObjectInstance = Instantiate(timerGameObjectPrefab);
            timerGameObjectInstance.SetActive(false);
            timerGameObjects.Add(timerGameObjectInstance);
        }

        for (int i = 0; i < spawners.Count; i++)
        {
            GameObject timerSpawnInstance = Instantiate(timerPrefab);
            timerSpawnInstance.SetActive(false);
            timerSpawners.Add(timerSpawnInstance);
        }
    }

    private void OnDestroy()
    {
        if (timerGameObjects != null)
        {
            for (int i = 0; i < timerGameObjects.Count; i++)
            {
                if (timerGameObjects[i] != null)
                {
                    timerGameObjects[i].GetComponent<TimerGameObject>().OnTimeDone -= OnCanResetEnemy;
                }
            }
        }

        if (timerSpawners != null && spawners != null)
        {
            for (int i = 0; i < timerSpawners.Count; i++)
            {
                if (timerSpawners[i] != null)
                {
                    if (spawners[i])
                    {
                        timerSpawners[i].GetComponent<Timer>().OnTimeDone -= spawners[i].CanUseAgain;
                    }

                }
            }
        }
    }

    public GameObject GetTimerGameObject()
    {
        GameObject instanceTimer = timerGameObjects.Find(x => !x.activeInHierarchy);
        instanceTimer.GetComponent<TimerGameObject>().ClearEvents();
        return instanceTimer;
    }

    internal void LoadInfoLevel(InfoLevelInGame infoLevel)
    {
        this.infoLevel = infoLevel;
    }

    public GameObject GetTimerSpawners()
    {
        GameObject instanceTimer = timerSpawners.Find(x => !x.activeInHierarchy);

        return instanceTimer;
    }


    public Spawner GetRandomSpawner()
    {
        Spawner spawner = null;
        List<Spawner> spawnersUsable = spawners.FindAll(X => X.CanUse);
        if (spawnersUsable.Count > 0)
        {
            int index = GetRandomInt(0, spawnersUsable.Count);
            spawner = spawnersUsable[index];
        }
        return spawner;
    }

    public GameObject GetExplosion()
    {
        GameObject explosion = Instantiate(explosionGameObject);
        return explosion;
    }

    public List<GameObject> ActiveElements(List<GameObject> items)
    {
        return items.Where(x => x.activeInHierarchy).ToList();
    }

    public void DeactivateTemporallySpawner(Spawner spawner)
    {
        spawner.CanUse = false;

        GameObject timerInstantiated = GetTimerSpawners();
        timerInstantiated.SetActive(true);

        Timer timerUntilRepeatSpawner = timerInstantiated.GetComponent<Timer>();
        timerUntilRepeatSpawner.gameObject.SetActive(true);
        timerUntilRepeatSpawner.name = "timerUntilRepeatSpawner";

        timerUntilRepeatSpawner.WaitTime = 0.1f;
        timerUntilRepeatSpawner.OnTimeDone += spawner.CanUseAgain;
        timerUntilRepeatSpawner.Reset();
    }

    private int GetRandomInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public GameObject InstantiateEnemy(Spawner randomSpawner)
    {
        int randomIndex = 0;

        randomIndex = GetRandomInt(0, enemiesOfLevel.Count - 1);
        GameObject instance = enemiesOfLevel[randomIndex].ShipPrefab;
        GameObject randomEnemyInstantiated = randomSpawner.Spawn(instance);
        EnemyShipController actualEnemyController = randomEnemyInstantiated.GetComponent<EnemyShipController>();

        actualEnemyController.gameObject.SetActive(true);
        actualEnemyController.OnDesactivate += OnDesactivateEnemy;
        return randomEnemyInstantiated;
    }

    public GameObject InstantiateGargabe(Spawner randomSpawner)
    {
        int randomIndex = 0;

        randomIndex = GetRandomInt(0, garbageOfLevel.Count - 1);
        GameObject gargabePrefabInstantiated = randomSpawner.Spawn(garbageOfLevel[randomIndex].Prefab);
        SpaceGarbageController actualGargabeController = gargabePrefabInstantiated.GetComponent<SpaceGarbageController>();

        actualGargabeController.Reset();
        actualGargabeController.OnDesactivate += OnDesactivateGarbage;
        return gargabePrefabInstantiated;
    }

    public GameObject InstantiateTimer()
    {
        return GameObject.Instantiate(timerPrefab);
    }

    private void OnDesactivateEnemy(EnemyShipController obj)
    {

        GameObject timerToAppear = GetTimerGameObject();
        if (obj.HittedByPlayer)
        {
            infoLevel.AddEnemyDefeated(obj);

        }

        TimerGameObject timer = timerToAppear.GetComponent<TimerGameObject>();
        timerToAppear.SetActive(true);
        timer.WaitTime = obj.Enemy.WaitTimeToReset;
        timer.GameObject = obj.gameObject;
        timer.OnTimeDone += OnCanResetEnemy;
        timer.Reset();
    }

    private void OnDesactivateGarbage(SpaceGarbageController obj)
    {

        GameObject timerToAppear = GetTimerGameObject();

        TimerGameObject timer = timerToAppear.GetComponent<TimerGameObject>();
        timerToAppear.SetActive(true);
        timer.WaitTime = obj.SpaceGarbage.WaitTimeToReset;
        timer.GameObject = obj.gameObject;
        timer.OnTimeDone += OnCanResetGarbage;
        timer.Reset();
    }

    private void OnCanResetEnemy(TimerGameObject timer, GameObject obj)
    {

        EnemyShipController enemyShipController = obj.GetComponent<EnemyShipController>();

        //timer.OnTimeDone -= null;
        enemyShipController.Reset();

        Spawner spawner = null;
        if (spawners.Any(x => x.CanUse))
        {
            spawner = GetRandomSpawner();
            if (spawner)
            {
                DeactivateTemporallySpawner(spawner);
                obj.transform.position = spawner.transform.position;
                obj.gameObject.SetActive(true);
            }
        }
        else
        {
            timer.gameObject.SetActive(true);
            timer.WaitTime = obj.GetComponent<EnemyShipController>().Enemy.WaitTimeToReset;
            timer.GameObject = obj.gameObject;
            //TODO Quitar Eventos
            timer.OnTimeDone += OnCanResetEnemy;
            timer.Reset();
        }
    }

    private void OnCanResetGarbage(TimerGameObject timer, GameObject obj)
    {
        SpaceGarbageController spaceGarbageController = obj.GetComponent<SpaceGarbageController>();

        if (!spaceGarbageController)
        {
            Debug.Log("ddf");
        }
        //timer.OnTimeDone -= null;
        spaceGarbageController.Reset();

        Spawner spawner = null;
        if (spawners.Any(x => x.CanUse))
        {
            spawner = GetRandomSpawner();
            if (spawner)
            {
                DeactivateTemporallySpawner(spawner);

                obj.transform.position = spawner.transform.position;
                obj.gameObject.SetActive(true);
            }
        }
        else
        {
            timer.gameObject.SetActive(true);

            timer.WaitTime = obj.GetComponent<SpaceGarbageController>().SpaceGarbage.WaitTimeToReset;
            timer.GameObject = obj.gameObject;
            //TODO Quitar Eventos
            timer.OnTimeDone += OnCanResetGarbage;
            timer.Reset();
        }
    }

    public void LoadSpawners(GameObject spawnersEnemyParent)
    {
        spawners = new List<Spawner>();
        foreach (Transform item in spawnersEnemyParent.GetComponentsInChildren<Transform>())
        {
            Spawner spawner = item.gameObject.GetComponent<Spawner>();
            if (spawner)
            {
                spawners.Add(spawner);
            }
        }
    }


}
