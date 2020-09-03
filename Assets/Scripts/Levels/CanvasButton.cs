using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasButton : MonoBehaviour {
    [SerializeField]
    private GameObject lifes;
    [SerializeField]
    private Image neutralizerRayImage, expansiveWaveImage, neutralizerButton, expansiveWaveButton;
    [SerializeField]
    private GameObject pauseMenu, pauseButton;
    [SerializeField]
    private GameObject statsMenu;
    [SerializeField]
    private GameObject goHomeMenu, optionsMenu;

    [SerializeField]
    private Transform lifePlaceHolder;

    [SerializeField]
    private GameObject backgroundLevel, joystickPlaceHolder;

    private Joystick joystick;
    private StatsMenuController statsMenuController;
    private InfoLevelInGame info;
    private GameController gameController;
    public InfoLevelInGame Info { get => info; set => info = value; }

    public event Action OnRestartLevel = delegate { };
    public event Action OnExitLevel = delegate { };
    public event Action<bool> OnPauseGame = delegate { };
    public event Action OnNextLevel = delegate { };
    public event Action<bool> OnGoHomeResponse = delegate { };
    public event Action<int> OnChangeLifePlayer = delegate { };

    private Color[] colours = new Color[] { Color.magenta, Color.red, Color.yellow };
    List<GameObject> lifebars = new List<GameObject>();

    private int sizeLifebars = 5;
    private int maxbarUsed;

    internal void HideJoystick()
    {
        neutralizerButton.raycastTarget = false;
        joystickPlaceHolder.gameObject.SetActive(false);
        expansiveWaveButton.raycastTarget = false;
        joystick.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Awake()
    {
        joystick = GetComponentInParent<CanvasInputController>().Joystick;
        statsMenuController = statsMenu.GetComponent<StatsMenuController>();
        gameController = GameController.Instance;
        for (int i = 0; i < sizeLifebars; i++)
        {
            GameObject lifeBar = Instantiate(lifePlaceHolder.gameObject, lifePlaceHolder, false);
            lifeBar.gameObject.SetActive(false);
            lifebars.Add(lifeBar);
        }
    }

    internal void SetBackground()
    {
        backgroundLevel.GetComponent<MeshRenderer>().material = ((PlayableLevel)gameController.selectedLevel).Background;
    }

    public void OnEnableExpansiveWaveButton()
    {
        if (Application.platform == RuntimePlatform.Android || joystick.gameObject.activeInHierarchy)
        {
            expansiveWaveButton.raycastTarget = true;
        }

        expansiveWaveImage.color = Color.white;
    }

    public void OnEnableNeutralizerButton()
    {
        if (Application.platform == RuntimePlatform.Android || joystick.gameObject.activeInHierarchy)
        {
            neutralizerButton.raycastTarget = true;
        }

        neutralizerRayImage.color = Color.white;
    }

    public void DisableExpansiveWaveButton()
    {
        expansiveWaveButton.raycastTarget = false;
        expansiveWaveImage.color = Color.black;
    }

    public void DisableNeutralizerButton()
    {
        neutralizerButton.raycastTarget = false;
        neutralizerRayImage.color = Color.black;
    }

    public void ShowButtons(bool state)
    {
        neutralizerButton.gameObject.SetActive(state);
        expansiveWaveButton.gameObject.SetActive(state);
    }

    public void ShowStats()
    {
        GetComponent<CanvasGroup>().interactable = false;

        ShowButtons(false);
        statsMenu.gameObject.SetActive(true);
    }

    public void SetValuesStats(int score)
    {
        statsMenu.GetComponent<StatsMenuController>().SetValues(score);
    }

    public void ShowGoHomeMenu(bool state)
    {
        goHomeMenu.gameObject.SetActive(state);
    }

    public void ResponseGoHome(bool state)
    {
        OnGoHomeResponse(state);
    }

    public void ButtonOptions()
    {
        optionsMenu.gameObject.SetActive(true);
    }
    #region UI Inputs

    public void PauseGame()
    {
        pauseMenu.gameObject.SetActive(!pauseMenu.activeInHierarchy);
        ShowButtons(!pauseMenu.activeInHierarchy);
        OnPauseGame(pauseMenu.activeInHierarchy);
    }

    public void ResumeGame()
    {
        pauseMenu.gameObject.SetActive(false);
        ShowButtons(true);
        OnPauseGame(false);
    }


    public void RestartLevel()
    {
        OnRestartLevel();
    }

    public void ExitLevel()
    {
        OnExitLevel();
    }

    public void NextLevel()
    {
        OnNextLevel();
    }

    public void OnChangeLife(float life)
    {
        int lifesInstances = 0;
        int colorInstances = 0;
        Color color = lifes.GetComponent<Image>().color;
        DeleteLifeBarChildrens();



        /* TODO Para añadir vidas
         * 
         * Instanciamos vidas si numVida % 3 == 0
         * habilitamos nueva barra y ponemos vidas ashi
         * 
         * Necesitamos coger la barra actual
         * Comprobar si tiene mas de 3 hijos activos (vidas)
         * Si tiene deshabilitamos 1
         *  comprobamos si vidas <= 0
         * Si no tiene, deshabilitamos barra
         * 
         * 
         * */
        int lifebar = 0;
        lifebars[lifebar].gameObject.SetActive(true);
        for (int i = 0; i < life; i++)
        {
            GameObject lifeItem = Instantiate(lifes, lifebars[lifebar].transform, false);
            lifeItem.SetActive(true);

            lifeItem.GetComponent<Image>().color = color;

            lifesInstances++;
            if (lifesInstances % 3 == 0)
            {
                color = GetNewColor(colorInstances);
                colorInstances++;
                if (colorInstances >= colours.Length)
                {
                    colorInstances = 0;
                }
                lifesInstances = 0;
                lifebar++;
                lifebars[lifebar].gameObject.SetActive(true);
            }
        }
    }

    private Color GetNewColor(int colorInstances)
    {
        return colours[colorInstances];
    }

    internal void HidePauseButton()
    {
        pauseButton.gameObject.SetActive(false);
    }

    private void DeleteLifeBarChildrens()
    {
        //TODO borrar hijos de todas las barras
        if (lifePlaceHolder.childCount > 0)
        {
            foreach (Transform child in lifePlaceHolder.GetComponentInChildren<Transform>())
            {
                if (child.gameObject != lifePlaceHolder.gameObject)
                {
                    foreach (Transform item in child)
                    {
                        item.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    internal void ShowLostScreen()
    {

        string textLost = "";
        if (info.Credits <= 0)
        {
            textLost = "You was too aggressive and lost the job :(";
        }
        else if (info.Lifes <= 0)
        {
            textLost = "Your Ship was destroyed during the journey :(";
        }
        else if (info.Time <= 0)
        {
            textLost = "You was too slow :(";
        }
        statsMenuController.SetTextLost(textLost);
        statsMenuController.ShowLostScreen();
    }
    #endregion
}
