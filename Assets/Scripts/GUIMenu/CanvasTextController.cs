using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasTextController : MonoBehaviour {
    private string text = "";
    //private string text =
    //"\n" +
    //"You are living in a megaCity at the year 2300. mega Corporations are ruining this city while they are trying to hide that fact with a few social politics.\n" +
    //"\n" +
    //"Your work is a battlefield, the delivery Office is full of orders so your workmates are going more aggressive and competitive day by day.\n" +
    //"\n" +
    //"You only want to live and bring some money to your self economy and avoid the use of cyber improvements for your body and soul.\n" +
    //"\n" +
    //"This night you had a very weird dream, you are trying to remember what happened.\n" +
    //"Someone went inside your house while you was resting for a hard day at the Delivery Office.\n" +
    //"That person put you a injection, you think it was anesthesia...\n" +
    //"Before that, you only remember how that person was cutting and opening your chest....\n" +
    //"\n" +
    //"With that vivid memory you touch your chest and feel a very long scar which go through your entire chest\n" +
    //"\n" +
    //"You decide to go to your friend Dario, who is a very good doctor in this city to examine your body, after the work....\n";
    [SerializeField]
    private TextMeshProUGUI textPlaceHolder;

    [SerializeField]
    public float letterPause;

    [SerializeField]
    private GameObject textStartGame;


    [SerializeField]
    private AudioSource audioSource;

    private GameController gameController;
    private Animator animator;
    private CanvasGroup canvasGroup;
    private AsyncOperation asyncLoad;

    [SerializeField]
    private float speedTimeOff, speedTimeOn;
    // Start is called before the first frame update
    void Start()
    {
        textStartGame.gameObject.SetActive(false);
        gameController = GameController.Instance;
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        //gameController.PauseGame(false);
        //Volvemos a poner la escena del primer nivel como escena siguiente
        if (!gameController.selectedLevel)
        {
            gameController.selectedLevel = gameController.PlayableLevels.levels[0];
        }


        if (gameController.selectedLevel.GetType().Equals(typeof(PlayableLevel)))
        {
            Utils.GetInfoLevelByPlayableLevel();

        }
        InformationScene informationScene = (InformationScene)gameController.selectedLevel;

        text = informationScene.Text;

        Utils.GetPlayableLevelByInfoLevel();


        //switch (gameController.selectedLevel.LevelName)
        //{
        //    case "ShowInfoFirst":
        //        {
        //            gameController.selectedLevel = gameController.PlayableLevels.levels[0];
        //            break;
        //        }
        //    case "ShowInfoSecond":
        //        {
        //            gameController.selectedLevel = gameController.PlayableLevels.levels[1];
        //            break;
        //        }
        //    case "ShowInfoThird":
        //        {
        //            gameController.selectedLevel = gameController.PlayableLevels.levels[2];
        //            break;
        //        }
        //    case "ShowInfoFourth":
        //        {
        //            gameController.selectedLevel = gameController.PlayableLevels.levels[3];
        //            break;
        //        }

        //    default:
        //        throw new Exception("Unexpected Case");
        //}

        asyncLoad = SceneManager.LoadSceneAsync(gameController.selectedLevel.SceneName);
        asyncLoad.allowSceneActivation = false;
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in text.ToCharArray())
        {
            if (asyncLoad.progress >= 0.9f)
            {
                textStartGame.gameObject.SetActive(true);
                //StartCoroutine(Spark());
            }
            textPlaceHolder.text += letter;
            yield return 0;
            yield return new WaitForSeconds(letterPause);
        }
        audioSource.Stop();
        //yield return new WaitForSeconds(1.0f);
        //StartGame();
    }

    public void ButtonSkipText()
    {
        StartGame();
    }

    private void StartGame()
    {
        asyncLoad.allowSceneActivation = true;
    }

    //IEnumerator Spark()
    //{
    //    while (true)
    //    {
    //        textStartGame.GetComponent<TextMeshProUGUI>().enabled = false;
    //        yield return new WaitForSecondsRealtime(speedTimeOff);
    //        textStartGame.GetComponent<TextMeshProUGUI>().enabled = true;
    //        //textStartGame.gameObject.SetActive(true);
    //        yield return new WaitForSecondsRealtime(speedTimeOn);
    //    }

    //}
}
