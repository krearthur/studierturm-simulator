using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class FixViewToElevatorButtons : MonoBehaviour, Interactable {

    private Camera sourceCamera;
    public Transform targetCameraTransform;

    private MonoBehaviour[] componentsToToggle;

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
                transformAnimator.InvertedStart();
            }
            Cursor.visible = true;
            
        }else if (transformAnimator.movingBackToSource) {
            // back to player mode
            foreach (MonoBehaviour comp in componentsToToggle) {
                comp.enabled = true;
            }
            this.GetComponent<Collider>().enabled = true;
            transformAnimator.Reset();
            Cursor.visible = false;
        }
	}

    public void Init(Camera cam, MonoBehaviour[] componentsToToggle) {
        this.GetComponent<Collider>().enabled = false;
        this.componentsToToggle = componentsToToggle;
        foreach(MonoBehaviour comp in componentsToToggle) {
            comp.enabled = false;
        }

        this.sourceCamera = cam;
        transformAnimator.Init(sourceCamera.transform, targetCameraTransform, .25f);
    }

    public void Interact(GameObject source) {
        
    }
}
