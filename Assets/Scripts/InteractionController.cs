using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

// attach to player
public class InteractionController : MonoBehaviour {
    
    public ObjectsInTrigger frontObjects;
    public ObjectsInRaycast interactiveRaycastObjects;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (interactiveRaycastObjects.hasObjects && Input.GetButtonDown("Action")) {
            RaycastHit hit = interactiveRaycastObjects.hits;
            if (hit.collider.CompareTag("ButtonsPanel")) {
                hit.collider.GetComponent<FixViewToElevatorButtons>().Init(GetComponentInChildren<Camera>(), 
                    new MonoBehaviour[] { GetComponent<FirstPersonController>(), this, frontObjects, interactiveRaycastObjects });
            }
            else {
                if(hit.collider.GetComponent<Interactable>() != null) {
                    hit.collider.GetComponent<Interactable>().Interact(gameObject);
                }
            }
        }
    }
}
