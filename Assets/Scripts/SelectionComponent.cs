using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionComponent : MonoBehaviour {
    private void Start() {
        GetComponent<Renderer>().material.SetColor("_UnitColor", Color.blue);
    }
    private void OnDestroy() {
        GetComponent<Renderer>().material.SetColor("_UnitColor", Color.red);
    }
}

