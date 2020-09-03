using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Borders : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField]
    private Transform borderUppper, borderBottom;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(gameObject.name);
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController)
        {
            Vector3 newPos = Vector3.zero;
            //Estamos abajo
            if (borderUppper)
            {
                newPos = new Vector3(playerController.gameObject.transform.position.x, borderUppper.position.y, 0) - Vector3.up;
            }
            //Estamos arriba
            else if (borderBottom)
            {
                newPos = new Vector3(playerController.gameObject.transform.position.x, borderBottom.position.y, 0) + Vector3.up;
            }
            playerController.gameObject.transform.position = newPos;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Choque Neutralizer");
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player)
        {
            player.GetComponent<Rigidbody>().AddForce(-player.GetComponent<Rigidbody>().velocity);
            //enemy.ExpansiveMove();
            //gameObject.SetActive(false);
        }
    }
}
