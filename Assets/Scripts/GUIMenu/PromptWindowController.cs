using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PromptWindowController : MonoBehaviour {

    public event Action OnButtonYesClicked = delegate { };
    public event Action OnButtonNoClicked = delegate { };

    [SerializeField]
    private TextMeshProUGUI textPlaceHolder, textButtonYes, textButtonNo;
    SoundManager soundManager;

    [SerializeField]
    private string textToShow;

    private void Awake()
    {
        soundManager = SoundManager.Instance;
    }
    public void ButtonYesClicked()
    {
        soundManager.PlayMusicClick();

        gameObject.SetActive(false);
        OnButtonYesClicked();
    }

    public void ButtonNoClicked()
    {
        soundManager.PlayMusicClick();

        gameObject.SetActive(false);
        OnButtonNoClicked();
    }

    internal void SetText(string text)
    {
        textPlaceHolder.text = text;
    }
}
