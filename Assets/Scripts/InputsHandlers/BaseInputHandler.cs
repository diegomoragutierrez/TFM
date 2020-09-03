using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInputHandler : MonoBehaviour {
    // Start is called before the first frame update

    public abstract float Horizontal { get; }
    public abstract float Vertical { get; }

    internal abstract void GetMoves();

}
