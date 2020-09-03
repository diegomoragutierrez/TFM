using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasInputController : BaseInputHandler, IPointerUpHandler, IPointerDownHandler {

    private float horizontal, horizontalSpeed, horizontalTarget;
    private float vertical, verticalSpeed, verticalTarget;

    private PlayerController playerController;

    [SerializeField]
    private Joystick joystick;

    private CanvasButton canvasButton;

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

    public Joystick Joystick { get => joystick; }

    protected bool pressed;
    private void Start()
    {
        //TODO: Pensar refactorizacion para evitar buscar por Tag
        canvasButton = GetComponentInChildren<CanvasButton>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {

        GetMoves();
    }

    public void ChangeHorizontalAxis(float horizontalValue)
    {
        horizontalTarget = horizontalValue;
    }

    public void ChangeVerticalAxis(float verticalValue)
    {
        verticalTarget = verticalValue;
    }

    public void ShootNeutralizer()
    {

        canvasButton.DisableNeutralizerButton();

        playerController.ShootNeutralizer();
    }

    public void ThrowExpansiveWaveShoot()
    {

        canvasButton.DisableExpansiveWaveButton();

        playerController.ShootExpansiveWave();
    }

    internal override void GetMoves()
    {

        horizontal = Joystick.Horizontal;
        vertical = Joystick.Vertical;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }
}
