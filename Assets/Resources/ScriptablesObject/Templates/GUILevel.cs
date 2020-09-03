using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GUILevelInstance", menuName = "GUILevel")]

public class GUILevel : ScriptableObject {
    public List<Level> levels;
    public Level selectedLevel;

    internal Level GetLevelByName(string name)
    {
        return levels.Find(x => x.LevelName == name);
    }

    internal Level GetLevelBySceneName(string name)
    {
        foreach (var item in levels)
        {
            if (item.SceneName == name)
            {
                return item;
            }
        }
        return null;
    }

    internal Level GetLevelById(int idLevel)
    {
        return levels.Find(x => x.IdLevel == idLevel);
    }

    internal Level GetLevelByLevelName(string v)
    {
        if (levels.Any(x => x.LevelName == v)){
            return levels.Single(x => x.LevelName == v);
        }
        return null;
    }
}
