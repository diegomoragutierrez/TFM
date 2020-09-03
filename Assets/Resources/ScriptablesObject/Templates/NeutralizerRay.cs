using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeutralizerRayInstance", menuName = "NeutralizerRay")]
public class NeutralizerRay : ScriptableObject
{
    public int id;

    public GameObject prefab;

    public Material material;
}
