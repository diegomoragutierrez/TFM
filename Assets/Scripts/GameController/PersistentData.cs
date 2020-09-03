using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

//TODO: Clase para almacenar informacion del player durante el juego, sera la clase que serializaremos en un futuro

[Serializable]
public class PairLevelCredits {
    [SerializeField]
    private int levelId;
    [SerializeField]
    private int credit;

    public PairLevelCredits(int levelId, int credit)
    {
        this.levelId = levelId;
        this.credit = credit;
    }

    public int IdLevel {
        get {
            return levelId;
        }

        set {
            levelId = value;
        }
    }

    public int Credits {
        get {
            return credit;
        }

        set {
            credit = value;
        }
    }
}
[Serializable]
public class PairLevelTime {
    [SerializeField]
    private int levelId;
    [SerializeField]
    private float time;

    public PairLevelTime(int levelId, float time)
    {
        this.levelId = levelId;
        this.time = time;
    }

    public int IdLevel {
        get {
            return levelId;
        }

        set {
            levelId = value;
        }
    }

    public float Time {
        get {
            return time;
        }

        set {
            time = value;
        }
    }
}

[Serializable]
public class PairPowerUpShip {
    [SerializeField]
    private FeatureToImprove typePoweUp;

    [SerializeField]
    private int idShip;

    [SerializeField]
    private float valueImprove;


    public PairPowerUpShip(FeatureToImprove typePowerUp, int idShip, float valueImprove)
    {
        typePoweUp = typePowerUp;
        this.IdShip = idShip;
        this.valueImprove = valueImprove;
    }

    public FeatureToImprove FeaturePowerUp { get => typePoweUp; set => typePoweUp = value; }
    public float ValueImprove { get => valueImprove; set => valueImprove = value; }
    public int IdShip { get => idShip; set => idShip = value; }
}

[Serializable]
public class PairPowerUpQuantity {
    [SerializeField]
    private FeatureToImprove typePoweUp;

    [SerializeField]
    private int quantity;


    [SerializeField]
    private float valueImprove;


    public PairPowerUpQuantity(FeatureToImprove typePowerUp, float valueImprove)
    {
        typePoweUp = typePowerUp;
        quantity = 1;
        this.valueImprove = valueImprove;
    }
    public FeatureToImprove FeaturePoweUp { get => typePoweUp; set => typePoweUp = value; }
    public int Quantity { get => quantity; set => quantity = value; }
    public float ValueImprove { get => valueImprove; set => valueImprove = value; }
}

//TODO Crear lista<PowerUp(su id),cantidad> de powerUp y cantidad
[Serializable]
public class PersistentData {
    [SerializeField]
    private List<int> levelsUnlocked = new List<int>();
    [SerializeField]
    private List<int> shipsUnlocked = new List<int>();

    [SerializeField]
    private int idLastLevel;

    [SerializeField]
    private bool readHowToPlay;

    [SerializeField]
    private int credits;

    [SerializeField]
    private List<PairLevelCredits> CreditsLevel = new List<PairLevelCredits>();

    [SerializeField]
    private List<PairPowerUpQuantity> ownedUpgrades = new List<PairPowerUpQuantity>();

    [SerializeField]
    private List<PairPowerUpShip> upgradesOfShips = new List<PairPowerUpShip>();

    [SerializeField]
    private List<PairLevelTime> times = new List<PairLevelTime>();


    public PersistentData()
    {
    }

    public List<int> LevelsUnlocked {
        get {
            return levelsUnlocked;
        }
    }

    public List<int> ShipsUnlocked {
        get {
            return shipsUnlocked;
        }
    }

    //TODO Añadir mejoras
    public void UnlockShip(PlayerShip playerShip)
    {
        if (!shipsUnlocked.Contains(playerShip.IdShip))
        {
            shipsUnlocked.Add(playerShip.IdShip);
        }
    }

    public void Unlocklevel(Level level)
    {
        if (!levelsUnlocked.Contains(level.IdLevel))
        {
            levelsUnlocked.Add(level.IdLevel);
        }
    }

    public void Unlocklevel(int idLevel)
    {
        if (!levelsUnlocked.Contains(idLevel))
        {
            levelsUnlocked.Add(idLevel);
        }
    }

    public List<PairPowerUpShip> UpgradesOfShips { get => upgradesOfShips; }
    public bool ReadHowToPlay { get => readHowToPlay; set => readHowToPlay = value; }

    public int Credits { get => credits; set => credits = value; }
    public int IdLastLevel { get => idLastLevel; set => idLastLevel = value; }

    public void AddCredits(Level level, int credits)
    {
        PairLevelCredits pair = new PairLevelCredits(level.IdLevel, credits);
        if (CreditsLevel.Any(x => x.IdLevel == level.IdLevel))
        {
            PairLevelCredits oldPair = CreditsLevel.Single(x => x.IdLevel == level.IdLevel);

            if (credits > oldPair.Credits)
            {
                CreditsLevel.Remove(oldPair);
                CreditsLevel.Add(pair);

            }
        }
        else
        {
            CreditsLevel.Add(pair);
        }
    }

    public void AddTime(Level level, float time)
    {
        PairLevelTime pair = new PairLevelTime(level.IdLevel, time);
        if (times.Any(x => x.IdLevel == level.IdLevel))
        {
            PairLevelTime oldPair = times.Single(x => x.IdLevel == level.IdLevel);

            if (time > oldPair.Time)
            {
                times.Remove(oldPair);
                times.Add(pair);
            }
        }
        else
        {
            times.Add(pair);
        }
    }
    public float GetBestTime(Level level)
    {
        float time = 0;
        if (times.Any(x => x.IdLevel == level.IdLevel))
        {
            PairLevelTime pair = times.Single(x => x.IdLevel == level.IdLevel);
            time = pair.Time;
        }
        return time;
    }

    internal void AddUpgradeToUser(PowerUpItem itemToBuy)
    {
        PairPowerUpQuantity pairPowerUpQuantities = ownedUpgrades.SingleOrDefault(x => x.FeaturePoweUp == itemToBuy.Feature && x.ValueImprove == itemToBuy.QuantityOfUpgrade);
        if (pairPowerUpQuantities != null)
        {
            pairPowerUpQuantities.Quantity++;
        }
        else
        {
            PairPowerUpQuantity newPair = new PairPowerUpQuantity(itemToBuy.Feature, itemToBuy.QuantityOfUpgrade);
            ownedUpgrades.Add(newPair);
        }
    }

    internal void RemoveUpgradeFromUser(PowerUpItem itemToBuy)
    {
        PairPowerUpQuantity pairPowerUpQuantities = ownedUpgrades.SingleOrDefault(x => x.FeaturePoweUp == itemToBuy.Feature && x.ValueImprove == itemToBuy.QuantityOfUpgrade);
        if (pairPowerUpQuantities != null)
        {
            pairPowerUpQuantities.Quantity--;
        }
    }

    internal int GetQuantityUpgrade(PowerUpItem itemToBuy)
    {
        PairPowerUpQuantity pairPowerUpQuantities = ownedUpgrades.SingleOrDefault(x => x.FeaturePoweUp == itemToBuy.Feature && x.ValueImprove == itemToBuy.QuantityOfUpgrade);
        if (pairPowerUpQuantities != null)
        {
            return pairPowerUpQuantities.Quantity;
        }
        return 0;
    }

    internal void AddUpgradeToShip(PlayerShip selectedPlayerShip, PowerUpItem powerUp)
    {
        PairPowerUpShip pairPowerUpShip = UpgradesOfShips.SingleOrDefault(x => x.FeaturePowerUp == powerUp.Feature && x.IdShip == selectedPlayerShip.IdShip);
        if (pairPowerUpShip != null)
        {
            pairPowerUpShip.ValueImprove += powerUp.QuantityOfUpgrade;
        }
        else
        {
            PairPowerUpShip pair = new PairPowerUpShip(powerUp.Feature, selectedPlayerShip.IdShip, powerUp.QuantityOfUpgrade);
            UpgradesOfShips.Add(pair);
        }
    }


    internal float GetImproveValuesUpgradesOfShipAndFeature(PlayerShip ship, FeatureToImprove feature)
    {
        float upgradeValue = 0;
        List<PairPowerUpShip> powerUpShip = UpgradesOfShips.FindAll(x => x.IdShip == ship.IdShip && x.FeaturePowerUp == feature);
        if (powerUpShip != null)
        {
            foreach (var item in powerUpShip)
            {
                upgradeValue += item.ValueImprove;
            }
        }
        return upgradeValue;
    }

}

