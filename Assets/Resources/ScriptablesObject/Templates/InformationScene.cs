using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InformationSceneInstance", menuName = "InformationScene")]
public class InformationScene : Level {

    [SerializeField]
    private TextAsset fileText;
    public string Text { get => fileText.text; }

}
