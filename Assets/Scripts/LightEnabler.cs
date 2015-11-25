using UnityEngine;
using System.Collections;

public class LightEnabler : MonoBehaviour {
    

    void OnTriggerEnter(Collider col) {
        col.GetComponentInChildren<Light>().enabled = true;
    }

    void OnTriggerExit(Collider col) {
        col.GetComponentInChildren<Light>().enabled = false;
    }
}
