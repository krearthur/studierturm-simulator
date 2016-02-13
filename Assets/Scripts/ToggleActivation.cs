using UnityEngine;
using System.Collections;

/// <summary>
///     Takes array of root GameObjects Names and 
///     activates their sub GameObjects or colliders in sub GameObjects
///     that match the list of given names, configured in the inspector of this MonoBehaviour.
///     Note: if name providers are set, the names are added to the list, not replaced!
/// </summary>
public class ToggleActivation : MonoBehaviour {
    
    public ToggleActivation copyInverseToggle;

    public ToggleObjectsNamesProvider namesProviderActivation;
    public ToggleObjectsNamesProvider namesProviderDeactivation;

    public string[] rootsActivate;
    public string[] rootsDeactivate;

    public string[] activateWithNames;
    private GameObject[] activateObjects;

    public string[] activateCollidersWithNames;
    private GameObject[] activateCollidersInGameObjects;

    public string[] deactivateWithNames;
    private GameObject[] deactivateObjects;

    public string[] deactivateCollidersWithNames;
    private GameObject[] deactivateCollidersInGameObjects;

    public int depth = 2;

    public string playerTag = "Player";
    public bool triggerOnStart = false;

	
	void Start () {

        // Copy inverse roots, name providers and names arrays from other ToggleActivation instances
        if(copyInverseToggle != null) {
            rootsActivate = copyInverseToggle.rootsDeactivate;
            rootsDeactivate = copyInverseToggle.rootsActivate;

            activateWithNames = copyInverseToggle.deactivateWithNames;
            activateCollidersWithNames = copyInverseToggle.deactivateCollidersWithNames;

            deactivateWithNames = copyInverseToggle.activateWithNames;
            deactivateCollidersWithNames = copyInverseToggle.activateCollidersWithNames;

            namesProviderActivation = copyInverseToggle.namesProviderDeactivation;
            namesProviderDeactivation = copyInverseToggle.namesProviderActivation;
        }

        // Name Providers, concatenate their names arrays with this instance ones
        if (namesProviderActivation != null) {
            ConcatenateArrays(ref activateWithNames, namesProviderActivation.gameObjectsWithNames);
            ConcatenateArrays(ref activateCollidersWithNames, namesProviderActivation.collidersWithNames);
        }
        if (namesProviderDeactivation != null) {
            ConcatenateArrays(ref deactivateWithNames, namesProviderDeactivation.gameObjectsWithNames);
            ConcatenateArrays(ref deactivateCollidersWithNames, namesProviderDeactivation.collidersWithNames);
        }

        // Search for GameObjects with the names arrays
        if (rootsActivate.Length > 0) {
            activateObjects = new GameObject[activateWithNames.Length];
            for (int i = 0; i < activateWithNames.Length; i++) {
                for(int k = 0; k < rootsActivate.Length; k++) {
                    activateObjects[i] = GameObject.Find(rootsActivate[k] + "/" + activateWithNames[i]);
                }
                
            }

            activateCollidersInGameObjects = new GameObject[activateCollidersWithNames.Length];
            for (int i = 0; i < activateCollidersWithNames.Length; i++) {
                for (int k = 0; k < rootsActivate.Length; k++) {
                    activateCollidersInGameObjects[i] = GameObject.Find(rootsActivate[k] + "/" + activateCollidersWithNames[i]);
                }
            }
        }

        if (rootsDeactivate.Length > 0) {
            deactivateObjects = new GameObject[deactivateWithNames.Length];
            for (int i = 0; i < deactivateWithNames.Length; i++) {
                for (int k = 0; k < rootsDeactivate.Length; k++) {
                    deactivateObjects[i] = GameObject.Find(rootsDeactivate[k] + "/" + deactivateWithNames[i]);
                }
            }

            deactivateCollidersInGameObjects = new GameObject[deactivateCollidersInGameObjects.Length];
            for (int i = 0; i < deactivateCollidersWithNames.Length; i++) {
                for (int k = 0; k < rootsDeactivate.Length; k++) {
                    deactivateCollidersInGameObjects[i] = GameObject.Find(rootsDeactivate[k] + "/" + deactivateCollidersWithNames[i]);
                }
            }
        }
        
        if (triggerOnStart) {
            Trigger();
        }
    }

    public void Trigger() {
        foreach (GameObject section in activateObjects) {
            if (section == null) continue;
            section.SetActive(true);
        }
        foreach (GameObject section in deactivateObjects) {
            if (section == null) continue;
            section.SetActive(false);
        }
        
        foreach (GameObject section in activateCollidersInGameObjects) {
            if (section == null) continue;
            ActivateColliders(section.GetComponentsInChildren<Collider>(), 1);
        }
        foreach (GameObject section in deactivateCollidersInGameObjects) {
            if (section == null) continue;
            DeactivateColliders(section.GetComponentsInChildren<Collider>(), 1);
        }
        
    }
	
    void OnTriggerEnter(Collider col) {
        if (col.CompareTag(playerTag)) {
            Trigger();
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

    private void ConcatenateArrays(ref string[] a, string[] b) {
        string[] result = new string[a.Length + b.Length];
        for(int i =0; i< result.Length; i++) {
            if (i < a.Length) {
                result[i] = a[i];
            }
            else {
                result[i] = b[i];
            }
        }

        a = result;
    }
}
