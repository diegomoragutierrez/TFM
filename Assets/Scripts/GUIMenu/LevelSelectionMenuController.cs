using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionMenuController : MonoBehaviour {
    [SerializeField]
    private GUILevel levelInfo;

    [SerializeField]
    private GameObject seleccionNaveMenu, gameMenu;

    [SerializeField]
    private TextMeshProUGUI textMeshTimeToBeat, textMeshCreditsToKeep, textMeshBestTime;

    [SerializeField]
    private Button buttonStart;

    [SerializeField]
    private Button[] buttonsLevel;


    [SerializeField]
    private Material background;


    private GameController gameController;
    private SoundManager soundManager;

    private Animator animator;
    private CanvasGroup canvasGroup;

    private CanvasController canvasController;


    private void Awake()
    {
        gameController = GameController.Instance;
        soundManager = SoundManager.Instance;

        canvasController = GetComponentInParent<CanvasController>();
        canvasController.SetBackground(background);

    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        soundManager.PlaySelectionlevelMusicGame();

        buttonStart.interactable = false;

        for (int i = 0; i < buttonsLevel.Length; i++)
        {
            if (gameController.LevelUnlocked(i + 1))
            {
                buttonsLevel[i].interactable = true;
            }
            else
            {
                buttonsLevel[i].interactable = false;
            }
        }
        buttonsLevel[0].interactable = true;
    }



    //Used in GUI
    public void SelectLevel(int id)
    {
        soundManager.PlayMusicClick();

        gameController.selectedLevel = levelInfo.levels[id];
        LoadInfoLevel();
        buttonStart.interactable = true;
    }

    private void LoadInfoLevel()
    {
        string textTimeToBeat = "Time Trial";
        string textCredits = "Credits";

        string textBestTime = "Best Time";
        PlayableLevel selectedLevel = (PlayableLevel)gameController.selectedLevel;
        textMeshTimeToBeat.text = string.Format(textTimeToBeat + " {0}", selectedLevel.TimeToBeat, 1);
        textMeshTimeToBeat.text = string.Format(textTimeToBeat + " {0}", selectedLevel.TimeToBeat, 1);

        textMeshCreditsToKeep.text = string.Format(textCredits + " {0}", selectedLevel.ScoreToKeep);

        float bestTime = 0.0f;
        float bestTimeOfLevel = gameController.PersistentData.GetBestTime(selectedLevel);

        if (bestTimeOfLevel > 0)
        {
            bestTime = bestTimeOfLevel;
        }

        textMeshBestTime.text = string.Format(textBestTime + " {0}", (float)(Math.Round((double)bestTime, 1)));
    }

    //Used in GUI
    public void StartLevel()
    {
        soundManager.PlayMusicClick();
        soundManager.StopSelectionlevelMusic();
        UtilsCanvas.ChangeCanvas(seleccionNaveMenu, gameObject);
        seleccionNaveMenu.GetComponent<ShipSelectionMenu>().StatusButtonContinue(true);

        Hide();
    }

    private void Hide()
    {
        animator.SetBool("Shown", false);

        canvasGroup.interactable = false;
    }

    //Used in GUI
    public void Back()
    {
        soundManager.PlayMusicClick();
        soundManager.StopSelectionlevelMusic();

        UtilsCanvas.ChangeCanvas(gameMenu, gameObject);
        Hide();
    }
}
