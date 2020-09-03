using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralizerMove : MonoBehaviour {
    // Start is called before the first frame update
    Rigidbody body;
    [SerializeField]
    private float timeToDestroy = 0.5f;
    [SerializeField]
    private GameObject timerPrefab;
    private GameObject timerToDestroy;

    private NeutralizerRay neutralizerRay;

    public float TimeToDestroy { get => timeToDestroy; set => timeToDestroy = value; }
    public float Speed { get; set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        timerToDestroy = Instantiate(timerPrefab);
    }


    private void OnEnable()
    {
        timerToDestroy.GetComponent<Timer>().OnTimeDone += OnTimeToDestroy;
        timerToDestroy.GetComponent<Timer>().WaitTime = TimeToDestroy;
        timerToDestroy.SetActive(true);
        timerToDestroy.GetComponent<Timer>().Reset();
    }
    private void OnDestroy()
    {
        if (timerToDestroy != null)
        {
            timerToDestroy.GetComponent<Timer>().OnTimeDone -= OnTimeToDestroy;
        }
    }

 

    // Update is called once per frame
    void Update()
    {
        if (isActiveAndEnabled)
        {
            body.MovePosition(body.position + Vector3.right * Time.deltaTime * Speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Choque Neutralizer");
        EnemyShipController enemy = collision.gameObject.GetComponent<EnemyShipController>();
        if (enemy)
        {
            enemy.Deactivate();
            gameObject.SetActive(false);
        }
    }

    public void OnTimeToDestroy()
    {
        gameObject.SetActive(false);
        timerToDestroy.GetComponent<Timer>().OnTimeDone -= OnTimeToDestroy;
    }

    public void Deactivate()
    {
        OnTimeToDestroy();
    }
}
