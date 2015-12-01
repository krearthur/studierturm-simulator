using UnityEngine;
using System.Collections;

public class ToggleActivation : MonoBehaviour {

    public GameObject[] targetObjects;
    public bool onTriggerSetActiveTo = false;
    public string playerTag = "Player";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col) {
        if (col.CompareTag(playerTag)) {
            foreach (GameObject section in targetObjects) { 
                section.SetActive(onTriggerSetActiveTo);
            }   
        }
    }
}
