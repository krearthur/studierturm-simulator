using UnityEngine;
using System.Collections.Generic;

public class ObjectsInTrigger : MonoBehaviour {

    public List<GameObject> objects;

    void OnTriggerEnter(Collider col) {
        if (objects.Contains(col.gameObject) == false) {
            objects.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col) {
        if (objects.Contains(col.gameObject)) {
            objects.Remove(col.gameObject);
        }
    }

}
