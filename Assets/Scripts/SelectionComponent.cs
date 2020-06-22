using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionComponent : MonoBehaviour {
    private void Start() {
        GetComponent<Renderer>().material.color = Color.white;
    }
    private void OnDestroy() {
        GetComponent<Renderer>().material.color = Color.red;
    }
}

