using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerGameObject : MonoBehaviour {
    // Start is called before the first frame update
    public event Action<TimerGameObject,GameObject> OnTimeDone = delegate { };
    private GameObject gameObjectToReset;
    public float WaitTime { get; set; }
    public GameObject GameObject { get => gameObjectToReset; set => gameObjectToReset = value; }

    public void Reset()
    {
        StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        float elapsedTime = 0;

        while (elapsedTime <= this.WaitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        OnTimeDone(this, GameObject);
        //GameObject.SetActive(false);
        //Debug.Log("desactivamos TimerGameObject");
        gameObject.SetActive(false);
    }

    internal void ClearEvents()
    {
        OnTimeDone = null;
    }
}