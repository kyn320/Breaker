using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour {

    public static InGameManager instance;

    public WallBehaviour wall;

    void Awake() {
        instance = this;
    }


}
