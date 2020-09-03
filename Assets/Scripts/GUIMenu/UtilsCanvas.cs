using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilsCanvas : MonoBehaviour {

    private Canvas canvas;


    public static void ChangeCanvas(GameObject newMenu, GameObject currentMenu)
    {
        GameObject nuevoMenu = GameObject.Instantiate(newMenu, currentMenu.transform.parent);
        Destroy(currentMenu);
        nuevoMenu.SetActive(true);
        newMenu.transform.SetAsLastSibling();
    }
}
