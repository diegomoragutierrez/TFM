using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseController : MonoBehaviour {

    [SerializeField]
    protected GameObject timerPrefab;

    protected GameObject timerToAppear;
     
    public abstract void Reset();





}
