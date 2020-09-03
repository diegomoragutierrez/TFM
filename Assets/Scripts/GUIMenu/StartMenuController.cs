using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartMenuController : MonoBehaviour {
    [SerializeField]
    private GameObject mapMenuPrefab, creditMenu, controlsMenu;
    [SerializeField]
    private TextMeshProUGUI startText, creditsText;

    private GameController gameController;
    private SoundManager soundManager;

    private Animator animator;
    private CanvasGroup canvasGroup;

    public event Action OnGearButtonClicked = delegate { };
    private CanvasController canvasController;
    private CreditMenuController creditMenuController;
    [SerializeField]
    private Material background;

    private void Awake()
    {
        gameController = GameController.Instance;
        soundManager = SoundManager.Instance;
        canvasController = GetComponentInParent<CanvasController>();
        canvasController.SetBackground(background);

        creditMenuController = canvasController.GetComponentInChildren<CreditMenuController>(true);
        //if (!creditMenuInstance)
        //{
        //    //creditMenuController = Instantiate(creditMenu.GetComponent<CreditMenuController>(), canvasController.transform, false);
        //    creditMenuController.name = "creditMenuController";
        //}
        //else
        //{
        //    creditMenuController = creditMenuInstance;
        //}

        creditMenuController.transform.SetAsLastSibling();

        creditMenuController.gameObject.SetActive(false);
        creditMenuController.OnCloseCreditsMenu += OnCloseCreditsMenu;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameController.Initialize();

        gameController.PauseGame(false);
        soundManager.PlayIntroMusicGame();

        startText.text = "Start";
        creditsText.text = "Credits";
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        animator.SetBool("Shown", true);
    }

    public void StartButton()
    {

        soundManager.PlayMusicClick();
        soundManager.StopIntroMusic();
        UtilsCanvas.ChangeCanvas(mapMenuPrefab, gameObject);
        Hide();
    }

    private void Hide()
    {
        animator.SetBool("Shown", false);

        canvasGroup.interactable = false;
    }

    public void CreditsButtons()
    {

        soundManager.PlayMusicClick();
        creditMenuController.transform.SetAsLastSibling();
        creditMenuController.gameObject.SetActive(true);
        canvasController.SetBlockRaycasts(false);
    }

    internal void OnCloseCreditsMenu()
    {
        canvasController.SetBlockRaycasts(true);

    }
    public void ControlsButton()
    {
        soundManager.PlayMusicClick();
        controlsMenu.gameObject.SetActive(true);
        controlsMenu.transform.SetAsLastSibling();
        canvasController.SetBlockRaycasts(false);
    }

    private void OnDestroy()
    {
        if (creditMenuController)
        {
            creditMenuController.OnCloseCreditsMenu -= OnCloseCreditsMenu;
        }
    }
}
