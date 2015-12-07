using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class FixViewToElevatorButtons : MonoBehaviour, Interactable {

    private Camera sourceCamera;
    public Camera targetCamera;

    private MonoBehaviour[] componentsToToggle;

    private TransformAnimator transformAnimator;
    
	// Use this for initialization
	void Start () {
        targetCamera.gameObject.SetActive(false);
        transformAnimator = new TransformAnimator();
	}
	
	// Update is called once per frame
	void Update () {
        if (transformAnimator.Tick(Time.deltaTime)) {
            targetCamera.transform.position = transformAnimator.position;
            targetCamera.transform.rotation = transformAnimator.rotation;
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
            sourceCamera.gameObject.SetActive(true);
            targetCamera.gameObject.SetActive(false);
            
            transformAnimator.Reset();
            transformAnimator.SetToTargetValues(targetCamera.transform);
            Cursor.visible = false;
        }
	}

    public void Init(Camera cam, MonoBehaviour[] componentsToToggle) {
        this.componentsToToggle = componentsToToggle;
        foreach(MonoBehaviour comp in componentsToToggle) {
            comp.enabled = false;
        }

        this.sourceCamera = cam;
        sourceCamera.gameObject.SetActive(false);
        targetCamera.gameObject.SetActive(true);
        
        transformAnimator.Init(sourceCamera.transform, targetCamera.transform, .25f);
        transformAnimator.SetToSourceValues(targetCamera.transform);
    }

    public void Interact(GameObject source) {
        
    }
}
