using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingMenu : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField]
    private GameObject buttonContinue;

    [SerializeField]
    private TextMeshProUGUI textLoading;

    private GameController gameController;
    private Animator animator;
    private CanvasGroup canvasGroup;
    private AsyncOperation asyncLoad;

    private SoundManager soundManager;
    void Start()
    {
        gameController = GameController.Instance;
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
        buttonContinue.gameObject.SetActive(false);
        soundManager = SoundManager.Instance;
        if (!gameController.selectedLevel)
        {
            gameController.selectedLevel = gameController.PlayableLevels.levels[0];
        }
        if (gameController.GameMode == GameMode.HistoryMode)
        {
            Utils.GetInfoLevelByPlayableLevel();

            //switch (gameController.selectedLevel.LevelName)
            //{
            //    case "Level1":
            //        {
            //            gameController.selectedLevel = gameController.Textlevels.GetLevelByName("ShowInfoFirst");
            //            break;
            //        }
            //    case "Level2":
            //        {
            //            gameController.selectedLevel = gameController.Textlevels.GetLevelByName("ShowInfoSecond");
            //            break;
            //        }
            //    case "Level3":
            //        {
            //            gameController.selectedLevel = gameController.Textlevels.GetLevelByName("ShowInfoThird");
            //            break;
            //        }
            //    case "Level4":
            //        {
            //            gameController.selectedLevel = gameController.Textlevels.GetLevelByName("ShowInfoFourth");
            //            break;
            //        }
            //    default:
            //        {
            //            Debug.Log("R");
            //            //throw new System.Exception("Unexpected Case");
            //            break;
            //}
            //}
        }

        StartCoroutine(LoadYourAsyncScene());
    }

    //Used in GUI
    public void LoadScene()
    {
        soundManager.PlayClickAlternative();
        asyncLoad.allowSceneActivation = true;
    }

    IEnumerator LoadYourAsyncScene()
    {
        yield return new WaitForSeconds(3.0f);

        asyncLoad = SceneManager.LoadSceneAsync(gameController.selectedLevel.SceneName);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            //textProgress.text = progress * 100f + "%";
            //Debug.Log(progress);
            if (asyncLoad.progress >= 0.9f)
            {
                buttonContinue.SetActive(true);
                textLoading.gameObject.SetActive(false);
            }
            textLoading.GetComponent<Animator>().SetBool("Shown", false);
            yield return new WaitForSeconds(0.8f);
            textLoading.GetComponent<Animator>().SetBool("Shown", true);
            yield return new WaitForSeconds(0.8f);
            yield return null;
        }
    }
}


