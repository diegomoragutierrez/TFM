using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuideHowToPlayMenuController : MonoBehaviour {
    [SerializeField]
    private TextAsset howToPlayText;

    [SerializeField]
    private TextMeshProUGUI text;

    private GameController gameController;

    public event Action OnCloseGuideMenu = delegate{};
    public event Action OnButtonOkClicked = delegate{};
    public event Action OnButtoBackClicked = delegate{};

    [SerializeField]
    private GameObject buttonBack, buttonOk, butttonClose;
    // Start is called before the first frame update
    void Start()
    {
        //TODO Dependedera del idioma
        gameController = GameController.Instance;
        gameController.ReadHowToPlay = true;
        gameController.SaveInformation();

        text.text = howToPlayText.text;
        CanvasController canvasController = GetComponentInParent<CanvasController>();
        if (canvasController.GetComponentInChildren<ShipSelectionMenu>())
        {
            buttonOk.gameObject.SetActive(true);

            butttonClose.gameObject.SetActive(false);
            buttonBack.gameObject.SetActive(false);
        }
        else
        {
            butttonClose.gameObject.SetActive(true);
            buttonBack.gameObject.SetActive(true);

            buttonOk.gameObject.SetActive(false);
        }
    }
    

    public void CloseButton()
    {
        gameObject.SetActive(false);
        OnCloseGuideMenu();
    }

    public void OkButton()
    {
        gameObject.SetActive(false);
        OnButtonOkClicked();
    }

    public void BackButton()
    {
        OnButtoBackClicked();
    }
}
