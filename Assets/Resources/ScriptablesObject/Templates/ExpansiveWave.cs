using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExpansiveWaveInstance", menuName = "ExpansiveWave")]
public class ExpansiveWave : ScriptableObject
{
    public int id;

    public GameObject prefab;

    public Material material;
}
