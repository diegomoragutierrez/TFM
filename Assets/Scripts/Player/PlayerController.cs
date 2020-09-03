using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private float speedBoost = 1.0f, improvedSpeed;
    BaseInputHandler controller;

    [SerializeField]
    private PlayerShip player;
    [SerializeField]
    private GameObject timerPrefab, shotPlace, shipMesh;
    private SoundManager soundManager;

    private GameObject timerShootNeutralized, timerShootExpansiveWave, timerDamage;

    public bool CanMove { get; set; } = true;
    public bool CanBeDamaged { get; set; } = true;
    public bool CanShootNeutralizer { get; set; } = true;
    public bool CanShootExpansive { get; set; } = true;
    public PlayerShip Player { get => player; }
    public GameObject ShipMesh { get => shipMesh; set => shipMesh = value; }

    public event Action<float> OnPlayerMove = delegate { };
    public event Action OnCanShootExpansiveWave = delegate { };
    public event Action OnCanShootNeutralizer = delegate { };
    public event Action OnGetDamage = delegate { };

    List<GameObject> expansiveWavePool;
    List<GameObject> neutralizerRayPool;

    private AudioSource audioSourceShipExploding, audioSourceShipMoving;

    private InfoLevelInGame infoLevel;

    private int sizePoolExpansiveWave = 3, sizePoolNeutralizeRay = 3;



    private void Awake()
    {
        soundManager = SoundManager.Instance;

        timerShootNeutralized = Instantiate(timerPrefab);
        timerShootNeutralized.SetActive(false);
        timerShootNeutralized.GetComponent<Timer>().OnTimeDone += OnPlayerCanShootNeutralizer;

        timerShootExpansiveWave = Instantiate(timerPrefab);
        timerShootExpansiveWave.SetActive(false);
        timerShootExpansiveWave.GetComponent<Timer>().OnTimeDone += OnPlayerCanShootExpansiveWave;

        timerDamage = Instantiate(timerPrefab);
        timerDamage.SetActive(false);
        timerDamage.GetComponent<Timer>().OnTimeDone += OnCanBeDamaged;


    }

    private void OnDestroy()
    {
        if (timerShootNeutralized != null)
        {
            timerShootNeutralized.GetComponent<Timer>().OnTimeDone -= OnPlayerCanShootNeutralizer;
            Destroy(timerShootNeutralized);
        }
        if (timerShootExpansiveWave != null)
        {
            timerShootExpansiveWave.GetComponent<Timer>().OnTimeDone -= OnPlayerCanShootExpansiveWave;
            Destroy(timerShootExpansiveWave);
        }
        if (timerDamage != null)
        {
            timerDamage.GetComponent<Timer>().OnTimeDone -= OnCanBeDamaged;
            Destroy(timerDamage);
        }
    }


    #region Action Events
    private void OnCanBeDamaged()
    {
        //Debug.Log("Can Be Damaged at " + Time.deltaTime);
        CanBeDamaged = true;

        //Stop Make player intermintent
    }

    private void OnPlayerCanShootNeutralizer()
    {
        OnCanShootNeutralizer();
        CanShootNeutralizer = true;
    }

    private void OnPlayerCanShootExpansiveWave()
    {
        OnCanShootExpansiveWave();
        CanShootExpansive = true;
    }
    #endregion

    private void OnDisable()
    {
        if (infoLevel)
        {
            if (infoLevel.Lifes <= 0)
            {
                CanMove = false;
                StopSounds();

                soundManager.PlayPlayerShipExploding(audioSourceShipExploding);
                soundManager.StopShipMovingEffect(audioSourceShipMoving);

            }
        }
    }

    void Start()
    {
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        if (Application.platform == RuntimePlatform.Android)
        {
            controller = canvas.GetComponent<CanvasInputController>();
        }
        else
        {
            //Comentar para version de pruebas con Android en PC
            //controller = canvas.GetComponent<CanvasInputController>();

            ////Descomentar para version final
            controller = GetComponent<KeyboardInputHandler>();
            canvas.GetComponent<CanvasButton>().HideJoystick();
            canvas.GetComponent<CanvasButton>().HidePauseButton();
        }
        controller.enabled = true;

        soundManager.AddPlayerShipExplodingAudioSource(ref audioSourceShipExploding);
        soundManager.AddPlayerShipMoving(ref audioSourceShipMoving);
        audioSourceShipMoving.clip = Player.SoundMoving;
        audioSourceShipMoving.loop = true;
        audioSourceShipExploding.clip = Player.SoundAtDestroy;

        soundManager.PlayShipMovingEffect(audioSourceShipMoving);
        expansiveWavePool = new List<GameObject>();
        neutralizerRayPool = new List<GameObject>();
        //Generacion Pool Expansive Waves
        for (int i = 0; i < sizePoolExpansiveWave; i++)
        {
            GameObject expansiveWavePrefab = player.ExpansiveWave.prefab;
            expansiveWavePrefab.SetActive(false);
            expansiveWavePool.Add(Instantiate(expansiveWavePrefab));
        }
        //Generacion Pool Neutralizer Rays
        for (int i = 0; i < sizePoolNeutralizeRay; i++)
        {
            GameObject neutralizerRayPrefab = player.NeutralizerRay.prefab;
            neutralizerRayPrefab.SetActive(false);
            neutralizerRayPool.Add(Instantiate(neutralizerRayPrefab));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            Move();
        }
    }

    internal void SetPlayerStatsInfo(InfoLevelInGame infoStats)
    {
        infoLevel = infoStats;
    }

    internal GameObject GetExpansiveWave()
    {
        GameObject expansiveWave = null;
        for (int i = 0; i < expansiveWavePool.Count; i++)
        {
            if (!expansiveWavePool[i].activeInHierarchy)
            {
                expansiveWave = expansiveWavePool[i];
            }
        }
        return expansiveWave;
    }

    internal GameObject GetNeutralizerRay()
    {
        GameObject neutralizerRay = null;
        for (int i = 0; i < neutralizerRayPool.Count; i++)
        {
            if (!neutralizerRayPool[i].activeInHierarchy)
            {
                neutralizerRay = neutralizerRayPool[i];
            }
        }
        return neutralizerRay;
    }
    internal void ShootNeutralizer()
    {
        //Debug.Log("Actual Time " + Time.fixedTime + " Last Time " + lastTimeShooted + " NEXT SHOT " + (lastTimeShooted + shotCadence) + " Cadence " + Constants.INITIAL_CADENCE);
        GameObject neutralizer = GetNeutralizerRay();
        if (CanShootNeutralizer && neutralizer)
        {
            soundManager.PlayNeutralizerRayEffect();
            timerShootNeutralized.GetComponent<Timer>().WaitTime = player.WaitTimeNeutralizer;
            timerShootNeutralized.SetActive(true);
            timerShootNeutralized.GetComponent<Timer>().Reset();
            CanShootNeutralizer = false;
            neutralizer.GetComponent<NeutralizerMove>().TimeToDestroy = player.WaitTimeNeutralizer;
            //neutralizer.GetComponent<NeutralizerMove>().Speed = player.SpeedNeutralizer;
            neutralizer.GetComponent<NeutralizerMove>().Speed = player.Speed + player.SpeedNeutralizer + player.SpeedImprove;
            InstantiateAbility(neutralizer);
        }
    }


    internal void ShootExpansiveWave()
    {
        //Debug.Log("Actual Time " + Time.fixedTime + " Last Time " + lastTimeShooted + " NEXT SHOT " + (lastTimeShooted + shotCadence) + " Cadence " + Constants.INITIAL_CADENCE);

        GameObject expansiveWave = GetExpansiveWave();
        if (CanShootExpansive && expansiveWave)
        {
            soundManager.PlayExpansiveWaveEffect();
            timerShootExpansiveWave.GetComponent<Timer>().WaitTime = player.WaitTimeExpansiveWave;
            timerShootExpansiveWave.SetActive(true);
            timerShootExpansiveWave.GetComponent<Timer>().Reset();
            CanShootExpansive = false;

            InstantiateAbility(expansiveWave);
        }
    }

    private void InstantiateAbility(GameObject prefab)
    {
        prefab.transform.position = shotPlace.transform.position;
        prefab.SetActive(true);
    }

    private void Move()
    {
        float movHorizontal = controller.Horizontal;
        float movVertical = controller.Vertical;
        float finalSpeed = player.Speed + player.SpeedImprove;
        if (movHorizontal != 0.1f || movVertical >= 0.1f)
        {
            transform.Translate(Vector3.right * movHorizontal * finalSpeed * Time.deltaTime);
            OnPlayerMove(Vector3.right.x * movHorizontal * finalSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * movVertical * finalSpeed * Time.deltaTime);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        EnemyShipController enemyController = other.gameObject.GetComponent<EnemyShipController>();
        LaserController laserController = other.gameObject.GetComponent<LaserController>();
        SpaceGarbageController spacegarbage = other.gameObject.GetComponent<SpaceGarbageController>();
        //NeutralizerMove neutralizerMove = other.gameObject.GetComponent<NeutralizerMove>();


        if (enemyController || laserController || spacegarbage)
        {
            if (CanBeDamaged)
            {
                CanBeDamaged = !CanBeDamaged;
                OnGetDamage();

                timerDamage.GetComponent<Timer>().WaitTime = player.WaitTimeUntilNextHit;
                timerDamage.SetActive(true);
                timerDamage.GetComponent<Timer>().Reset();
                //Debug.Log("Damaged at " + Time.fixedTime);
            }

            if (enemyController)
            {
                enemyController.HittedByPlayer = true;
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


    internal void StopSounds()
    {
        soundManager.StopShipMovingEffect(audioSourceShipMoving);
    }

    internal void PlayShipMovingEffect()
    {
        soundManager.PlayShipMovingEffect(audioSourceShipMoving);
    }
}
