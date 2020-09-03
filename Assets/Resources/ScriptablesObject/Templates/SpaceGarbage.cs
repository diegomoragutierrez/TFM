using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpacialGarbageInstance", menuName = "SpacialGarbage")]
public class SpaceGarbage : ScriptableObject {
    [SerializeField]
    private int id;
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private int waitTimeToReset;

    public int WaitTimeToReset { get => waitTimeToReset; }
    public GameObject Prefab { get => prefab; }
    public float Speed { get => speed; }
    public int Id { get => id; }


}
