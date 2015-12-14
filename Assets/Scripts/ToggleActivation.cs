using UnityEngine;
using System.Collections;

public class ToggleActivation : MonoBehaviour {

    public GameObject[] activateObjects;
    public GameObject[] deactivateObjects;

    public GameObject[] activateCollidersInGameObjects;
    public GameObject[] deactivateCollidersInGameObjects;

    public ToggleActivation invertFromToggle;
    public ToggleActivation copyFromToggle;

    public string playerTag = "Player";
    public bool triggerOnStart = false;

	// Use this for initialization
	void Start () {
	    if(invertFromToggle != null) {
            activateObjects = invertFromToggle.deactivateObjects;
            deactivateObjects = invertFromToggle.activateObjects;
            activateCollidersInGameObjects = invertFromToggle.deactivateCollidersInGameObjects;
            deactivateCollidersInGameObjects = invertFromToggle.activateCollidersInGameObjects;
        }
        if(copyFromToggle != null) {
            activateObjects = invertFromToggle.activateObjects;
            deactivateObjects = invertFromToggle.deactivateObjects;
            activateCollidersInGameObjects = invertFromToggle.activateCollidersInGameObjects;
            deactivateCollidersInGameObjects = invertFromToggle.deactivateCollidersInGameObjects;
        }
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
            ActivateColliders(section.GetComponents<Collider>());
        }
        foreach (GameObject section in deactivateCollidersInGameObjects) {
            DeactivateColliders(section.GetComponents<Collider>());
        }
        
        
    }
	
    void OnTriggerEnter(Collider col) {
        if (col.CompareTag(playerTag)) {
            Toggle();
        }
    }

    private void DeactivateColliders(Collider[] colliders) {
        foreach (Collider collider in colliders) {
            collider.enabled = false;
            DeactivateColliders(collider.GetComponentsInChildren<Collider>(false));
        }
    }

    private void ActivateColliders(Collider[] colliders) {
        foreach (Collider collider in colliders) {
            collider.enabled = false;
            ActivateColliders(collider.GetComponentsInChildren<Collider>(true));
        }
    }
}
