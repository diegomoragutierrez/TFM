using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "LevelInstance", menuName = "Level")]
public class Level : ScriptableObject {
    //ID of the Level
    [SerializeField]
    protected int idLevel;
    //Id of scene in Build

    //Name of the scene in Build
    [SerializeField]
    protected string sceneName;

    //Name of the level on the game
    [SerializeField]
    protected string levelName;



    public int IdLevel { get => idLevel; }
    //public int SceneId1 { get => SceneId; }
    public string SceneName { get => sceneName; }
    public string LevelName { get => levelName; }

}
