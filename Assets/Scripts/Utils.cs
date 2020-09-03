using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {


    public static void GetPlayableLevelByInfoLevel()
    {
        GameController gameController = GameController.Instance;

        switch (gameController.selectedLevel.LevelName)
        {
            case "ShowInfoFirst":
                {
                    gameController.selectedLevel = gameController.PlayableLevels.GetLevelByLevelName("Level1");
                    break;
                }
            case "ShowInfoSecond":
                {
                    gameController.selectedLevel = gameController.PlayableLevels.levels[1];
                    break;
                }
            case "ShowInfoThird":
                {
                    gameController.selectedLevel = gameController.PlayableLevels.levels[2];
                    break;
                }
            case "ShowInfoFourth":
                {
                    gameController.selectedLevel = gameController.PlayableLevels.levels[3];
                    break;
                }

            default:
                throw new Exception("Unexpected Case");
        }
    }

    public static void GetInfoLevelByPlayableLevel()
    {
        GameController gameController = GameController.Instance;

        switch (gameController.selectedLevel.LevelName)
        {
            case "Level1":
                {
                    gameController.selectedLevel = gameController.Textlevels.GetLevelByName("ShowInfoFirst");
                    break;
                }
            case "Level2":
                {
                    gameController.selectedLevel = gameController.Textlevels.GetLevelByName("ShowInfoSecond");
                    break;
                }
            case "Level3":
                {
                    gameController.selectedLevel = gameController.Textlevels.GetLevelByName("ShowInfoThird");
                    break;
                }
            case "Level4":
                {
                    gameController.selectedLevel = gameController.Textlevels.GetLevelByName("ShowInfoFourth");
                    break;
                }
            default:
                {
                    Debug.Log("R");
                    //throw new System.Exception("Unexpected Case");
                    break;
                }
        }
    }
}
