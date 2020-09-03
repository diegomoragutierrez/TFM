using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private GameObject player;
    private Vector3 offset;
    [SerializeField]
    private GameObject startMovingCamera;
    [SerializeField]
    private GameObject endMovingCamera;
    private Vector3 velocity;

    [SerializeField]
    private float SmothTime;
    [SerializeField]
    private float maxSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (player.transform.position.x > startMovingCamera.transform.position.x && player.transform.position.x < endMovingCamera.transform.position.x)
        {
            Vector3 newPosition = Vector3.SmoothDamp(transform.position, new Vector3(player.transform.position.x + offset.x, transform.position.y, transform.position.z), ref velocity, SmothTime, maxSpeed);
            transform.position = newPosition;
            //Debug.Log("PosCamara " + transform.position);
            //transform.position = new Vector3(player.transform.position.x + offset.x, transform.position.y, transform.position.z);
        }
    }
}
