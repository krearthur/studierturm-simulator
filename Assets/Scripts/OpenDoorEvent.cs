using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// Gets attached to an door object and handles the open event.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class OpenDoorEvent : MonoBehaviour, Interactable {

    private AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;

    private bool opening = false;
    private bool isOpen = false;
    private bool closing = false;
    private bool running = false;
    private Vector3 defaultRotation;
    public Vector3 newRotation;

    private float openAngle = 85f;
    private float calculatedOpenAngle;
    private float animationDuration = 0.5f;
    private float currentAnimationTime = 0f;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        this.enabled = false;
        defaultRotation = GetComponent<Transform>().eulerAngles;
        newRotation = defaultRotation;
        calculatedOpenAngle = Mathf.Repeat(newRotation.z + openAngle, 360);
    }

    private void Run() {
        if (!running) {
            
			if(!isOpen){
                currentAnimationTime = 0;
                opening = true;
				closing = false;
			}
            else{
				closing = true;
				opening = false;
			}
			GetComponent<Collider>().isTrigger = true;

            running = true;
            this.enabled = true;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (opening) {
            if(currentAnimationTime >= animationDuration) {
                opening = false;
                isOpen = true;
                running = false;
				GetComponent<Collider>().isTrigger = false;
                audioSource.PlayOneShot(openSound);
            }
            else {
                currentAnimationTime += Time.deltaTime;
                if (currentAnimationTime > animationDuration) currentAnimationTime = animationDuration;
                newRotation.z = Mathf.LerpAngle(defaultRotation.z, calculatedOpenAngle, currentAnimationTime / animationDuration);
                transform.eulerAngles = newRotation;
            }
            
        }
        else if (closing) {
            if (currentAnimationTime <= 0) {
                audioSource.PlayOneShot(closeSound);
                GetComponent<Collider>().isTrigger = false;
                isOpen = false;
                running = false;
                this.enabled = false;
            }
            else {
                currentAnimationTime -= Time.deltaTime;
                if (currentAnimationTime < 0) currentAnimationTime = 0;
                newRotation.z = Mathf.LerpAngle(defaultRotation.z, calculatedOpenAngle, currentAnimationTime / animationDuration);
                transform.eulerAngles = newRotation;
            }
        }

	}

    public void Interact(GameObject source) {
        Run();
    }
}
