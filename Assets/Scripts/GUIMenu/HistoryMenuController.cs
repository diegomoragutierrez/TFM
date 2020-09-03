using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HistoryMenuController : MonoBehaviour {
    [SerializeField]
    private TextMeshProUGUI textNewGame, textContinue, textback;
    SoundManager soundManager;

    [SerializeField]
    private GameObject seleccionNaveMenu;
    // Start is called before the first frame update
    private GameController gameController;
    GameMenuController gameMenuController;

    public event Action OnCloseMenu = delegate { };
    public event Action OnStartGameHistoryMode = delegate { };
    void Start()
    {
        gameMenuController = GetComponentInParent<GameMenuController>();
        gameController = GameController.Instance;
        soundManager = SoundManager.Instance;

        if (gameController.PersistentData.IdLastLevel >= 1)
        {
            textNewGame.gameObject.SetActive(false);
        }
        else
        {
            textContinue.gameObject.SetActive(false);
        }

        if (gameController.PersistentData.IdLastLevel + 1 > gameController.PlayableLevels.levels.Count)
        {
            textContinue.gameObject.SetActive(false);

        }
    }

    public void ButtonContinue()
    {
        soundManager.PlayMusicClick();

        gameController.selectedLevel = gameController.PlayableLevels.GetLevelById(gameController.PersistentData.IdLastLevel + 1);
        StartGame();
    }

    public void ButtonNewGame()
    {
        soundManager.PlayMusicClick();

        gameController.selectedLevel = gameController.PlayableLevels.GetLevelBySceneName("Level1");
        StartGame();
    }

    public void ButtonBack()
    {
        soundManager.PlayMusicClick();

        gameObject.SetActive(false);
        OnCloseMenu();
    }


    private void StartGame()
    {
        gameObject.SetActive(false);
        OnStartGameHistoryMode();
    }
}
