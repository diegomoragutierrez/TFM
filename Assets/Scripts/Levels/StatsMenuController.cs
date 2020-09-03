using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenuController : MonoBehaviour {
    //[SerializeField]
    //private CanvasButton buttonReiniciar, buttonSalir;

    [SerializeField]
    private TextMeshProUGUI textPuntuacion, textTiempo, textLost, textWin;

    [SerializeField]
    private GameObject winScreen, lostScreen;

    [SerializeField]
    private Button exitButton;

    private void Awake()
    {
        winScreen.gameObject.SetActive(false);
        lostScreen.gameObject.SetActive(false);
    }
    public void SetValues(int puntuacion)
    {
        winScreen.gameObject.SetActive(true);
        string scoreText = "Score {0} pts";
        string timeText = "Time {0} s";
        textPuntuacion.text = String.Format(scoreText, puntuacion);
        float actualTime = (float)(Math.Round((double)Time.timeSinceLevelLoad, 1));

        textTiempo.text = String.Format(timeText, actualTime.ToString());
    }

    public void ShowLostScreen()
    {
        lostScreen.gameObject.SetActive(true);
        exitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(360, 80);
    }

    internal void SetTextLost(String text)
    {
        textLost.text = text;
    }
}
