using UnityEngine;
using System.Collections;

public class ToggleActivation : MonoBehaviour {

    public GameObject[] activateObjects;
    public GameObject[] deactivateObjects;

    public GameObject[] activateCollidersInGameObjects;
    public GameObject[] deactivateCollidersInGameObjects;
    public int depth = 2;

    public string playerTag = "Player";
    public bool triggerOnStart = false;

	
	void Start () {
        if (triggerOnStart) {
            Toggle();
        }
    }

    private void Toggle() {
        foreach (GameObject section in activateObjects) {
            section.SetActive(true);
        }
        foreach (GameObject section in deactivateObjects) {
            section.SetActive(false);
        }
        
        foreach (GameObject section in activateCollidersInGameObjects) {
            ActivateColliders(section.GetComponentsInChildren<Collider>(), 1);
        }
        foreach (GameObject section in deactivateCollidersInGameObjects) {
            DeactivateColliders(section.GetComponentsInChildren<Collider>(), 1);
        }
        
    }
	
    void OnTriggerEnter(Collider col) {
        if (col.CompareTag(playerTag)) {
            Toggle();
        }
    }

    private void DeactivateColliders(Collider[] colliders, int level) {
        if (level > depth) return;
        foreach (Collider collider in colliders) {
            collider.enabled = false;
            DeactivateColliders(collider.GetComponentsInChildren<Collider>(true), ++level);
        }
    }

    private void ActivateColliders(Collider[] colliders, int level) {
        if (level > depth) return;
        foreach (Collider collider in colliders) {
            collider.enabled = true;
            ActivateColliders(collider.GetComponentsInChildren<Collider>(true), ++level);
        }
    }
}
