using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StoreItemType {
    PowerUp, Credit
}

public class StoreItem : ScriptableObject{
    [SerializeField]
    private int id;

    [SerializeField]
    private StoreItemType type;

    public StoreItemType Type { get => type; set => type = value; }
}