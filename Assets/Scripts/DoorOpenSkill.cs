using UnityEngine;
using System.Collections;

// attach to player
public class DoorOpenSkill : MonoBehaviour {
    
    public ObjectsInTrigger frontObjects;
    public ObjectsInRaycast interactiveRaycastObjects;

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (interactiveRaycastObjects.hasObjects && Input.GetButtonDown("Interact")) {
            foreach (RaycastHit hit in interactiveRaycastObjects.hits) {
                if (hit.collider.CompareTag("Door")) {
                    hit.collider.GetComponent<OpenDoorEvent>().Run();
                }
                else {
                    if(hit.collider.GetComponent<Interactable>() != null) {
                        hit.collider.GetComponent<Interactable>().Interact();
                    }
                }
            }
        }
    }
}
