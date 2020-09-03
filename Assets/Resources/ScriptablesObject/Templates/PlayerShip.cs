
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerShipInstance", menuName = "PlayerShip")]
public class PlayerShip : ScriptableObject {
    [SerializeField]
    private int idShip;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private float life;
    [SerializeField]
    private string shipName;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxSpeedImprove;
    [SerializeField]
    private float maxLifeImprove;
    [SerializeField]
    private float speedNeutralizer;
    [SerializeField]
    private float waitTimeNeutralizer;
    [SerializeField]
    private float waitTimeExpansiveWave;
    [SerializeField]
    private float waitTimeUntilNextHit;
    [SerializeField]
    private ExpansiveWave expansiveWave;
    [SerializeField]
    private NeutralizerRay neutralizerRay;

    [SerializeField]
    private AudioClip soundAtDestroy, soundMoving;

    private GameController gameController;

    public int IdShip { get => idShip; }
    public float WaitTimeExpansiveWave { get => waitTimeExpansiveWave; }
    public float WaitTimeUntilNextHit { get => waitTimeUntilNextHit; }
    public float WaitTimeNeutralizer { get => waitTimeNeutralizer; }
    public float Speed { get => speed; }
    public string ShipName { get => shipName; }
    public float Life { get => life; }
    public GameObject Prefab { get => prefab; }
    public NeutralizerRay NeutralizerRay { get => neutralizerRay; }
    public ExpansiveWave ExpansiveWave { get => expansiveWave; }
    public AudioClip SoundAtDestroy { get => soundAtDestroy; set => soundAtDestroy = value; }
    public AudioClip SoundMoving { get => soundMoving; set => soundMoving = value; }
    public float SpeedNeutralizer { get => speedNeutralizer; set => speedNeutralizer = value; }
    public float SpeedImprove { get => speedImprove; set => speedImprove = value; }
    public float LifeImprove { get => lifeImprove; set => lifeImprove = value; }
    public float MaxLifeImprove { get => maxLifeImprove; set => maxLifeImprove = value; }
    public float MaxSpeedImprove { get => maxSpeedImprove; set => maxSpeedImprove = value; }

    [SerializeField]
    private float speedImprove;

    [SerializeField]
    private float lifeImprove;

    //private bool initilized = false;

    internal void AddPowerUps()
    {
        System.Collections.IList list = System.Enum.GetValues(typeof(FeatureToImprove));
        for (int i = 0; i < list.Count; i++)
        {
            FeatureToImprove feature = (FeatureToImprove)list[i];
            //gameController.PersistentData.GetImproveValuesUpgradesOfShipAndFeature(this, feature);
            AddPowerUp(feature);
        }
    }

    internal void AddPowerUp(FeatureToImprove feature)
    {
        float valueOfImprove = gameController.PersistentData.GetImproveValuesUpgradesOfShipAndFeature(this, feature);
        switch (feature)
        {
            case FeatureToImprove.Life:
                {
                    lifeImprove = valueOfImprove;
                    break;
                }
            case FeatureToImprove.Speed:
                {
                    speedImprove = valueOfImprove;
                    break;
                }

            default:
                throw new System.Exception("Unexpected Case");
        }
    }

    public void Initialize()
    {
        gameController = GameController.Instance;
        AddPowerUps();
        //initilized = true;
    }
}
