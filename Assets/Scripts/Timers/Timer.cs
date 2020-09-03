using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {
    // Start is called before the first frame update
    public event Action OnTimeDone = delegate { };

    public float WaitTime { get; set; }


    public void Reset()
    {
        StartCoroutine(TimerCoroutine());
    }

    IEnumerator TimerCoroutine()
    {
        float elapsedTime = 0;

        while (elapsedTime <= WaitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        OnTimeDone();
        gameObject.SetActive(false);
    }
}
