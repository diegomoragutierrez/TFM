using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputHandler : BaseInputHandler {
    float horizontal, vertical;
    Vector3 direction;
    PlayerController playerController;
    private CanvasButton canvasButton;

    private GameController gameController;
    public override float Horizontal {
        get {
            return horizontal;
        }
    }

    public override float Vertical {
        get {
            return vertical;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
        canvasButton = canvas.GetComponentInChildren<CanvasButton>();
        gameController = GameController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        GetMoves();
        GetInputs();
    }

    internal override void GetMoves()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    internal void GetInputs()
    {
        if (!gameController.GamePaused)
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Debug.Log("Mouse 0");
                ShootNeutralizer();
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                //Debug.Log("Mouse 1");
                ThrowExpansiveWaveShoot();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("Mouse 1");
            canvasButton.PauseGame();
        }
    }

    private void ShootNeutralizer()
    {
        canvasButton.DisableNeutralizerButton();

        playerController.ShootNeutralizer();
    }

    private void ThrowExpansiveWaveShoot()
    {
        playerController.ShootExpansiveWave();
        canvasButton.DisableExpansiveWaveButton();
    }
}
