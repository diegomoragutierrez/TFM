using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenuController : MonoBehaviour {
    [SerializeField]
    private GameObject guidePcMenu, guideAndroidMenu, windowHowToPlay;
    SoundManager soundManager;

    private CanvasController canvasController;

    private GuideHowToPlayMenuController guideHowToPlayController;
    // Start is called before the first frame update
    void Start()
    {
        canvasController = GetComponentInParent<CanvasController>();
        transform.SetAsLastSibling();
        ShowControlsInfoForPlatform();
        soundManager = SoundManager.Instance;

        windowHowToPlay = Instantiate(windowHowToPlay, transform, false);
        windowHowToPlay.gameObject.SetActive(false);
        windowHowToPlay.transform.SetAsLastSibling();
        guideHowToPlayController = windowHowToPlay.GetComponent<GuideHowToPlayMenuController>();
        guideHowToPlayController.OnButtoBackClicked += ButtonBackHowToPlay;
        guideHowToPlayController.OnCloseGuideMenu += ButtonClose;
    }


    private void OnDestroy()
    {
        if (guideHowToPlayController)
        {
            guideHowToPlayController.OnButtoBackClicked += ButtonBackHowToPlay;
        }
    }
    private void OnEnable()
    {
        ShowControlsInfoForPlatform();
    }
    private void ShowControlsInfoForPlatform()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                {
                    guideAndroidMenu.SetActive(true);
                    break;
                }
            case RuntimePlatform.WebGLPlayer:
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                {
                    guidePcMenu.SetActive(true);

                    break;
                }
            default:
                throw new System.Exception("Unexpected Case");
        }
    }

    public void ButtonClose()
    {
        soundManager.PlayMusicClick();

        canvasController.SetBlockRaycasts(true);

        gameObject.SetActive(false);
    }

    public void ButtonHowToPlay()
    {
        soundManager.PlayMusicClick();
        windowHowToPlay.transform.SetAsLastSibling();
        windowHowToPlay.gameObject.SetActive(true);
    }

    public void ButtonBack()
    {
        soundManager.PlayMusicClick();

        ShowControlsInfoForPlatform();
    }

    public void ButtonBackHowToPlay()
    {
        soundManager.PlayMusicClick();

        windowHowToPlay.gameObject.SetActive(false);
    }
}
