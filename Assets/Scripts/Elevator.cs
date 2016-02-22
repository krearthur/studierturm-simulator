using UnityEngine;
using System.Collections.Generic;

public class Elevator : MonoBehaviour, SlidingDoorListener {

    private int targetFloors;
    public Floor currentFloor = Floor.Eight;
    private Floor currentTargetFloor;
    private bool driving;
    private List<ElevatorListener> listeners = new List<ElevatorListener>();
    private Animator animator;
    private TransformAnimator positionDriver;
    public float drivingTimeForOneFloor = 1.5f;
    public ObjectsInTrigger carriedObjects;

    public Collider doorColliders;
    
    public Vector3[] floorPositions = new Vector3[10] {
        new Vector3(0,0,0), // floor -1
        new Vector3(0,2,0), // floor 0
        new Vector3(0,4,0), // floor 1
        new Vector3(0,6,0), // ...
        new Vector3(0,8,0),
        new Vector3(0,10,0),
        new Vector3(0,12,0),
        new Vector3(0,14,0),
        new Vector3(0,16,0),
        new Vector3(0,18,0)
        };

    new public AudioSource audio;

    private Vector3 targetFloorPos;

    #region ParentClass

    void Start() {
        animator = GetComponent<Animator>();
        foreach(SlidingDoorStateBehaviour behaviour in animator.GetBehaviours<SlidingDoorStateBehaviour>()) {
            behaviour.AddListener(this);
        }

        for(int i=0; i<floorPositions.Length; i++) {
            floorPositions[i].y = floorPositions[0].y + i * GameController.floorHeight;
        }

        currentTargetFloor = currentFloor;
        positionDriver = new TransformAnimator();
        targetFloors = 0;
        transform.position = GetFloorPosition(currentFloor);
        
    }

    // Update is called once per frame
    void Update() {
        if (IsStanding()) {
            if (IsInTargets(currentFloor)) { // just arrived
             
                // remove current floor from target floor
                targetFloors &= ~(int)currentFloor;


                if (IsClosed()) {
                    animator.SetTrigger("OpenTrigger");
                    FloorArrived();
                }
            }
            else if(targetFloors > 0) { // any floors left in the pool of target floors?
                CalculateNearestTargetFloor();
                driving = true;
                
                if (GetDrivingDirection() > 0) {
                    foreach (ElevatorListener listener in listeners) {
                        listener.DrivingDown();
                    }
                }
                else if (GetDrivingDirection() < 0) {
                    foreach (ElevatorListener listener in listeners) {
                        listener.DrivingUp();
                    }
                }
            }
        }
        else if (driving) {
            DoDriving();
        }
    }

    #endregion
    
    #region ConvenientFunctions 

    private void DoDriving() {
        if (IsClosed() == false) {
            //Debug.Log(name + " not closed! cancel driving..");
            return;
        }
        if (positionDriver.running) {
            //Debug.Log(name + " driving towards floor: " + FloorUtility.Number(currentTargetFloor));
            positionDriver.Step(Time.deltaTime);
            // Also move carried objects
            Vector3 delta = positionDriver.position - transform.position;
            foreach (GameObject go in carriedObjects.objects) {
                go.transform.position += delta;
            }
            transform.position += delta;

            Floor tmp = currentFloor;
            currentFloor = FloorUtility.FloorForNumber(1 + (int)((transform.position.y - floorPositions[2].y) / GameController.floorHeight));
            if(tmp != currentFloor) {
                // passed floor
                foreach(ElevatorListener li in listeners) {
                    li.FloorPassed(currentFloor);
                }
            }
        }
        else {
            if (positionDriver.reachedTarget) {
                positionDriver.Reset();
                //Debug.Log(name + " reached target floor: " + FloorUtility.Number(currentTargetFloor));
                currentFloor = currentTargetFloor;
                audio.Stop();
                driving = false;
            }
            else {
                //Debug.Log(name + " start driving!");
                positionDriver.Init(transform.position, GetFloorPosition(currentTargetFloor), CalculateDrivingTime());
                positionDriver.Start();
                audio.Play();
            }
        }
    }

    private Vector3 GetFloorPosition(Floor floor) {
        return floorPositions[FloorUtility.Index(floor)];
    }

    private float CalculateDrivingTime() {
        return drivingTimeForOneFloor * (Mathf.Abs(FloorUtility.Number(currentFloor) - FloorUtility.Number(currentTargetFloor)) );
    }

    /// <summary>
    /// negative is up, positive is down, 0 is no driving
    /// </summary>
    /// <returns></returns>
    public int GetDrivingDirection() {
        return FloorUtility.Number(currentFloor) - FloorUtility.Number(currentTargetFloor);
    }

    private void CalculateNearestTargetFloor() {
        int direction = GetDrivingDirection();

        int curFlorNum = FloorUtility.Number(currentFloor);
        int maxUp = 8 - curFlorNum;
        int maxDown = curFlorNum - 1;

        for (int floorStep = 0; floorStep < 8; floorStep++) {
            // strategy: take the first match
            Floor nextFloor = currentFloor;

            if ( (!driving || direction < 0) && floorStep < maxUp) {
                nextFloor = FloorUtility.FloorForNumber(curFlorNum + floorStep);
            }
            if ( (!driving || direction > 0) && floorStep < maxDown) {
                nextFloor = FloorUtility.FloorForNumber(curFlorNum - floorStep);
            }
           
            

            if (IsInTargets(nextFloor)) {
                // found nearest target floor
                currentTargetFloor = nextFloor;
                SetTargetFloorPosition(nextFloor);
                break;
            }

        }
    }
    

    private void SetTargetFloorPosition(Floor floor) {
        targetFloorPos = floorPositions[FloorUtility.Index(floor)];
    }

    public bool IsInTargets(Floor floor) {
        return ((int)floor & targetFloors) > 0;
    }


    #endregion
    #region API_orders

    public void CallToFloor(Floor floor) {
        targetFloors |= (int)floor;
        if (IsStanding()) {
            currentTargetFloor = floor;
        }
        else {
            CalculateNearestTargetFloor();
        }

    }

    public void AddListener(ElevatorListener lis) {
        listeners.Add(lis);
    }

    public void InterruptClosing(float normTime) {
        animator.Play("Opening", 0, normTime);
    }

    /// <summary>
    /// Requests to open the doors. 
    /// </summary>
    /// <returns>True if request gets redirected. False if request gets ignored due to current elevator status.</returns>
    public bool OpenDoorsCall() {
        if (IsStanding()) {
            if (IsClosed() || IsClosing()) {
                animator.SetTrigger("OpenTrigger");
                return true;
            }
        }
        return false;
    }

    public Floor GetCurrentFloor() {
        return currentFloor;
    }

    #endregion
    #region API_queries

    public bool IsStanding() {
        return !driving;
    }

    public bool IsClosed() {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Closed");
    }
    public bool IsClosing() {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Closing");
    }
    public bool IsOpen() {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Open");
    }
    public bool IsOpening() {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Opening");
    }

    public bool IsDrivingUp() {
        return GetDrivingDirection() < 0;
    }

    public bool IsDrivingDown() {
        return GetDrivingDirection() > 0;
    }

    #endregion
    #region Events

    private void FloorArrived() {
        foreach (ElevatorListener listener in listeners) {
            listener.FloorArrived(currentFloor);
        }
    }

    void SlidingDoorListener.DoorOpening() {
        doorColliders.isTrigger = true;
        foreach (ElevatorListener listener in listeners) {
            listener.ElevatorOpening();
        }
    }

    void SlidingDoorListener.DoorClosing() {
        doorColliders.isTrigger = false;
        foreach (ElevatorListener listener in listeners) {
            listener.ElevatorClosing();
        }
    }

    void SlidingDoorListener.DoorClosed() {
        foreach (ElevatorListener listener in listeners) {
            listener.ElevatorClosed();
        }
    }

    void SlidingDoorListener.DoorOpened() {
        foreach (ElevatorListener listener in listeners) {
            listener.ElevatorOpened();
        }
    }

    #endregion
}
