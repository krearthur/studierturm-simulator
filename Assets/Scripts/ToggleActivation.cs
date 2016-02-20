using UnityEngine;
using System.Collections;

public abstract class TriggerableComponent : MonoBehaviour {
    public abstract void Trigger(int code);
}

/// <summary>
///     Either sets objects active or inactive
/// </summary>
public class ToggleActivation : TriggerableComponent {
    

    public ToggleObjectsNamesProvider gameObjectsNamesProvider;
    
    public string[] rootGameObjectsNames;

    public string[] toggleGameObjectsWithNames;
    private GameObject[] toggleGameObjects;

    public string[] toggleCollidersWithNames;
    private GameObject[] toggleCollidersInGameObjects;
    
    public int hierarchyDepth = 2;
    
	void Start () {

        // Name Providers, concatenate their names arrays with this instance ones
        if (gameObjectsNamesProvider != null) {
            ConcatenateArrays(ref toggleGameObjectsWithNames, gameObjectsNamesProvider.gameObjectsWithNames);
            ConcatenateArrays(ref toggleCollidersWithNames, gameObjectsNamesProvider.collidersWithNames);
        }

        // Search for GameObjects with the names arrays
        if (rootGameObjectsNames.Length > 0) {
            toggleGameObjects = new GameObject[toggleGameObjectsWithNames.Length];
            for (int i = 0; i < toggleGameObjectsWithNames.Length; i++) {
                for(int k = 0; k < rootGameObjectsNames.Length; k++) {
                    toggleGameObjects[i] = GameObject.Find(rootGameObjectsNames[k] + "/" + toggleGameObjectsWithNames[i]);
                }
                
            }

            toggleCollidersInGameObjects = new GameObject[toggleCollidersWithNames.Length];
            for (int i = 0; i < toggleCollidersWithNames.Length; i++) {
                for (int k = 0; k < rootGameObjectsNames.Length; k++) {
                    toggleCollidersInGameObjects[i] = GameObject.Find(rootGameObjectsNames[k] + "/" + toggleCollidersWithNames[i]);
                }
            }
        }
        
    }

    override public void Trigger(int code) {
        bool activate = code > 0 ? true :false;
        
        if (toggleGameObjects != null) {
            foreach (GameObject section in toggleGameObjects) {
                if (section == null) continue;
                section.SetActive(activate);
            }
        }
        if (toggleCollidersInGameObjects != null) {
            foreach (GameObject section in toggleCollidersInGameObjects) {
                if (section == null) continue;
                if (activate) {
                    ActivateColliders(section.GetComponentsInChildren<Collider>(), 1);
                }
                else {
                    DeactivateColliders(section.GetComponentsInChildren<Collider>(), 1);
                }
            }
        }

    }
	

    private void DeactivateColliders(Collider[] colliders, int level) {
        if (level > hierarchyDepth) return;
        foreach (Collider collider in colliders) {
            collider.enabled = false;
            DeactivateColliders(collider.GetComponentsInChildren<Collider>(true), ++level);
        }
    }

    private void ActivateColliders(Collider[] colliders, int level) {
        if (level > hierarchyDepth) return;
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
                result[i] = b[i - a.Length];
            }
        }

        a = result;
    }
}
