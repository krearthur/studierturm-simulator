using UnityEngine;
using System.Collections;

public class ElevatorButton : MonoBehaviour, ElevatorListener {

    public Elevator elevator;
    public Floor floor;
    public bool doorOpenButton = false;
    public Texture buttonsActiveTex;
    private Texture buttonsInactiveTex;
    private Color buttonsInActiveColor;
    public float blinkInterval = .3f;
    public bool blinking;
    private float currentBlinkTime;
    public bool btnIsOn = false;
    private FixViewToElevatorButtons fixView;
    
    public AudioSource audioSource;

    // Use this for initialization
    void Start() {
        elevator.AddListener(this);
        buttonsInactiveTex = GetComponent<Renderer>().material.mainTexture;
        buttonsInActiveColor = GetComponent<Renderer>().material.GetColor("_EmissionColor");
        fixView = GetComponentInParent<FixViewToElevatorButtons>();
    }

    // Update is called once per frame
    void Update() {
        if (blinking) {
            currentBlinkTime += Time.deltaTime;
            if (currentBlinkTime >= blinkInterval) {
                currentBlinkTime = 0;
                BtnLight(!btnIsOn);
            }
        }
    }

    void OnMouseDown() {
        audioSource.PlayOneShot(audioSource.clip);
        if (!doorOpenButton) {
            BtnLight(true);
            if (elevator.currentFloor == floor && elevator.IsClosed() == false) {
                BtnLight(false);
            }
            fixView.externCancelFixView(this);
            elevator.CallToFloor(floor);
        }
        else {
            if (elevator.OpenDoorsCall()) {
                BtnLight(true);
            }
            fixView.externCancelFixView(this);
        }

    }

    private void BtnLight(bool shouldLit) {
        GetComponent<Renderer>().material.mainTexture = shouldLit ? buttonsActiveTex : buttonsInactiveTex;
        GetComponent<Renderer>().material.SetColor("_EmissionColor", shouldLit ? Color.white : buttonsInActiveColor);

        btnIsOn = shouldLit;
    }

    void ElevatorListener.FloorArrived(Floor currentFloor) {
        if (currentFloor == floor) {
            blinking = true;
        }
    }
    void ElevatorListener.FloorPassed(Floor currentFloor) {
       
    }

    void ElevatorListener.ElevatorOpening() {

    }

    void ElevatorListener.ElevatorClosing() {
        if (doorOpenButton) {
            BtnLight(false);
        }
    }
    void ElevatorListener.ElevatorOpened() {
        blinking = false;
        if (elevator.currentFloor == floor || doorOpenButton) {
            BtnLight(false);
        }
    }

    void ElevatorListener.ElevatorClosed() {
        if (doorOpenButton) {
            BtnLight(false);
        }
    }
    void ElevatorListener.DrivingUp() {

    }
    void ElevatorListener.DrivingDown() {

    }

}
