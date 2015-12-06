using UnityEngine;
using System.Collections;

public class ToggleActivation : MonoBehaviour {

    public GameObject[] activateObjects;
    public GameObject[] deactivateObjects;
    public string playerTag = "Player";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col) {
        if (col.CompareTag(playerTag)) {
            foreach (GameObject section in activateObjects) { 
                section.SetActive(true);
            }
            foreach (GameObject section in deactivateObjects) {
                section.SetActive(false);
            }
        }
    }
}
