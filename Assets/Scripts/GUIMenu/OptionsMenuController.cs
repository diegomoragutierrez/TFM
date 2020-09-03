using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI textSave, textCancel, textTitle, textLabelToggleMusic, textLabelVibration;
    [SerializeField]
    private Toggle toggleMusic, toggleEffects, toggleVibration;

    private GameController gameController;
    private SoundManager soundManager;


    public event Action OnCloseMenu = delegate { };

    private void Awake()
    {
        gameController = GameController.Instance;
        soundManager = SoundManager.Instance;

    }
    // Start is called before the first frame update

    private void OnEnable()
    {
        toggleVibration.isOn = gameController.Vibration;

        //TODO ASIGNAR VALORES DE SONIDO

        toggleEffects.isOn = soundManager.SoundEffects;
        toggleMusic.isOn = soundManager.SoundMusic;
    }

    //Used in GUI
    public void ButtonCancel()
    {
        soundManager.PlayMusicClick();

        OnCloseMenu();
        gameObject.SetActive(false);

    }
    //Used in GUI
    public void ButtonSave()
    {
        soundManager.PlayMusicClick();

        //TODO: SAVE VALUES
        gameController.Vibration = toggleVibration.isOn;
        //TODO ASIGNAR VALORES DE SONIDO
        soundManager.SoundEffects = toggleEffects.isOn;
        soundManager.SoundMusic = toggleMusic.isOn;
        soundManager.UpdateSounds();
        OnCloseMenu();
        gameObject.SetActive(false);
    }
}
