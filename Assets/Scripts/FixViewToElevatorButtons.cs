using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class FixViewToElevatorButtons : MonoBehaviour, Interactable {

    private Camera sourceCamera;
    public Transform targetCameraTransform;

    private MonoBehaviour[] behavioursToToggle;
    private Collider[] collidersToToggle;
    private ElevatorButton[] buttons;

    private TransformAnimator transformAnimator = new TransformAnimator();
    
	// Use this for initialization
	void Start () {
        targetCameraTransform.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (transformAnimator.Tick(Time.deltaTime)) {
			sourceCamera.transform.position = transformAnimator.position;
			sourceCamera.transform.rotation = transformAnimator.rotation;
        }else if (transformAnimator.reachedTarget) {
            // click button mode
            if (Input.GetButtonDown("Cancel")) {
                transformAnimator.StartBackwards();
            }
            Cursor.visible = true;
            
        }else if (transformAnimator.movingBackToSource) {
            // back to player mode
            foreach (MonoBehaviour comp in behavioursToToggle) {
                comp.enabled = true;
            }
            foreach (Collider col in collidersToToggle) {
                col.enabled = true;
            }
            this.GetComponent<Collider>().enabled = true;
            transformAnimator.Reset();
            Cursor.visible = false;
        }
	}

    public void Init(Camera cam, MonoBehaviour[] behaviours, Collider[] colliders) {
        this.GetComponent<Collider>().enabled = false;
        this.behavioursToToggle = behaviours;
        this.collidersToToggle = colliders;
        foreach(MonoBehaviour comp in behaviours) {
            comp.enabled = false;
        }
        foreach(Collider col in colliders) {
            col.enabled = false;
        }

        this.sourceCamera = cam;
        transformAnimator.Init(sourceCamera.transform, targetCameraTransform, .25f);
    }

    public void Interact(GameObject source) {
        
    }

    public void externCancelFixView(MonoBehaviour source) {
        transformAnimator.StartBackwards();
    }
}
