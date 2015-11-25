using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(AudioSource))]
public class CallElevator : MonoBehaviour, SlidingDoorListener, ElevatorListener, Interactable {

    public Floor floorOfCallButton;
    public Elevator elevator;
    public Animator animator;

    public AudioClip openingSound;
    public AudioClip closingSound;
    public AudioClip buttonClickSound;

    public Texture activeButtons;
    public Renderer statusButton;

    private Texture passiveButtons;
    private Renderer rend;
    private AudioSource audio;

    public Collider doorColliders;

    private bool blinking;
    public float blinkInterval = .3f;
    private float currentBlinkTime;
    private bool interruptToOpen;
    private bool waitingForElevator;

    void Start() {
        rend = GetComponent<Renderer>();
        audio = GetComponent<AudioSource>();
        foreach (SlidingDoorStateBehaviour behaviour in animator.GetBehaviours<SlidingDoorStateBehaviour>()) {
            behaviour.AddListener(this);
        }
        passiveButtons = rend.material.mainTexture;
        currentBlinkTime = 0;
        elevator.AddListener(this);
    }

    void Update() {
        if (blinking) {
            currentBlinkTime += Time.deltaTime;
            if (currentBlinkTime >= blinkInterval) {
                currentBlinkTime = 0;
                statusButton.material.mainTexture = 
                    statusButton.material.mainTexture == activeButtons ? passiveButtons : activeButtons;
            }
        }
    }
    

    public void InterruptClosing() {
        float normTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        animator.Play("Opening", 0, 1 - normTime);
        elevator.InterruptClosing(1-normTime);
        interruptToOpen = true;
    }

    void SlidingDoorListener.DoorClosing() {
        doorColliders.isTrigger = false;
        audio.PlayOneShot(closingSound);
    }

    void SlidingDoorListener.DoorOpening() {

        waitingForElevator = false;
        audio.PlayOneShot(openingSound);
    }

    void SlidingDoorListener.DoorOpened() {
        doorColliders.isTrigger = true;

    }

    void SlidingDoorListener.DoorClosed() {
        rend.material.mainTexture = passiveButtons;
        blinking = false;
        statusButton.material.mainTexture = passiveButtons;
    }


    void ElevatorListener.ElevatorOpening() {
        if (interruptToOpen) {
            interruptToOpen = false;
        }
        else {
            animator.SetTrigger("Open");
        }
        
    }

    void ElevatorListener.ElevatorClosing() {

    }

    void ElevatorListener.FloorArrived(Floor arrived) {
        
        statusButton.material.mainTexture = activeButtons;
        if (floorOfCallButton == arrived) {
            blinking = true;
        }
    }

    void ElevatorListener.FloorPassed(Floor passed) {
        
    }

    void ElevatorListener.ElevatorOpened() {
        
    }

    void ElevatorListener.ElevatorClosed() {
        
    }

    void ElevatorListener.DrivingUp() {
        
    }

    void ElevatorListener.DrivingDown() {
        
    }

    public void Interact() {
        audio.PlayOneShot(buttonClickSound);
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Closing")) {
            InterruptClosing();
        }
        else if (!waitingForElevator && animator.GetCurrentAnimatorStateInfo(0).IsName("Closed")) {
            rend.material.mainTexture = activeButtons;
            elevator.CallToFloor(floorOfCallButton);
            waitingForElevator = true;
        }
    }
}
