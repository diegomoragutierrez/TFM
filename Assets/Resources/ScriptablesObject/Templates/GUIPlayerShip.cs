using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GUIPlayerShipInstance", menuName = "GUIPlayerShip")]

public class GUIPlayerShip : ScriptableObject {
    [SerializeField]
    private List<PlayerShip> playerShips;
    public PlayerShip selectedShip;

    public List<PlayerShip> PlayerShips { get => playerShips; set => playerShips = value; }

    internal PlayerShip GetShipByName(string name)
    {
        return PlayerShips.Find(x => x.ShipName == name);
    }
}
