using UnityEngine;
using System.Collections.Generic;

public enum Floor {
    MinusOne = 1 << 0,
    Zero = 1 << 1,
    One = 1 << 2,
    Two = 1 << 3,
    Three = 1 << 4,
    Four = 1 << 5,
    Five = 1 << 6,
    Six = 1 << 7,
    Seven = 1 << 8,
    Eight = 1 << 9
}

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
        new Vector3(4,0,4),
        new Vector3(4,2,4),
        new Vector3(4,4,4),
        new Vector3(4,6,4),
        new Vector3(4,8,4),
        new Vector3(4,10,4),
        new Vector3(4,12,4),
        new Vector3(4,14,4),
        new Vector3(4,16,4),
        new Vector3(4,18,4)
        };

    private Vector3 targetFloorPos;

    #region ParentClass

    void Start() {
        animator = GetComponent<Animator>();
        foreach(SlidingDoorStateBehaviour behaviour in animator.GetBehaviours<SlidingDoorStateBehaviour>()) {
            behaviour.AddListener(this);
        }
        currentTargetFloor = currentFloor;
        positionDriver = new TransformAnimator();
        targetFloors = 0;
        transform.position = GetFloorPosition(currentFloor);
        
    }

    // Update is called once per frame
    void Update() {
        if (IsStanding()) {
            if (targetFloors>0 && IsInTargets(currentFloor)) {
                // remove current floor from target floor
                targetFloors &= ~(int)currentFloor;
                if (IsClosed()) {
                    animator.SetTrigger("OpenTrigger");
                    FloorArrived();
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
            Debug.Log(name + " not closed! cancel driving..");
            return;
        }
        if (positionDriver.running) {
            Debug.Log(name + " driving towards floor: " + GetFloorNumber(currentTargetFloor));
            positionDriver.Step(Time.deltaTime);
            // Also move carried objects
            Vector3 delta = positionDriver.position - transform.position;
            foreach (GameObject go in carriedObjects.objects) {
                go.transform.position += delta;
            }
            transform.position += delta;
            
        }
        else {
            if (positionDriver.reachedTarget) {
                positionDriver.Reset();
                Debug.Log(name+" reached target floor: " + GetFloorNumber(currentTargetFloor));
                currentFloor = currentTargetFloor;
                driving = false;
            }
            else {
                Debug.Log(name + " start driving!");
                positionDriver.Init(transform.position, GetFloorPosition(currentTargetFloor), CalculateDrivingTime());
                positionDriver.Start();
            }
        }
    }

    private Vector3 GetFloorPosition(Floor floor) {
        switch (floor) {
            case Floor.MinusOne: return floorPositions[0];
            case Floor.Zero: return floorPositions[1];
            case Floor.One: return floorPositions[2];
            case Floor.Two: return floorPositions[3];
            case Floor.Three: return floorPositions[4];
            case Floor.Four: return floorPositions[5];
            case Floor.Five: return floorPositions[6];
            case Floor.Six: return floorPositions[7];
            case Floor.Seven: return floorPositions[8];
            case Floor.Eight: return floorPositions[9];
            default:
                {
                    Debug.LogError("Invalid Floor: " + floor);
                    return Vector3.zero;
                }
        }
    }

    private float CalculateDrivingTime() {
        return drivingTimeForOneFloor * (Mathf.Abs(GetFloorNumber(currentFloor) - GetFloorNumber(currentTargetFloor)) );
    }

    /// <summary>
    /// negative is up, positive is down, 0 is no driving
    /// </summary>
    /// <returns></returns>
    public int GetDrivingDirection() {
        return GetFloorNumber(currentFloor) - GetFloorNumber(currentTargetFloor);
    }

    private void CalculateNearestTargetFloor(int direction) {
        for (int floors = 0; floors < 9; floors++) {
            Floor nextTargetFloor = MoveFromCurrentFloor(floors, direction);
            if (IsInTargets(nextTargetFloor)) {
                // found nearest target floor
                currentTargetFloor = nextTargetFloor;
                SetTargetFloorPosition(nextTargetFloor);
                break;
            }
        }
    }
    
    private Floor MoveFromCurrentFloor(int floors, int direction) {
        if (direction < 0) {
            return (Floor)((int)currentFloor << floors);
        }
        else if (direction > 0) {
            return (Floor)((int)currentFloor >> floors);
        }

        return currentFloor;
    }

    private void SetTargetFloorPosition(Floor floor) {
        switch (floor) {
            case Floor.MinusOne: targetFloorPos = floorPositions[0]; break;
            case Floor.Zero: targetFloorPos = floorPositions[1]; break;
            case Floor.One: targetFloorPos = floorPositions[2]; break;
            case Floor.Two: targetFloorPos = floorPositions[3]; break;
            case Floor.Three: targetFloorPos = floorPositions[4]; break;
            case Floor.Four: targetFloorPos = floorPositions[5]; break;
            case Floor.Five: targetFloorPos = floorPositions[6]; break;
            case Floor.Six: targetFloorPos = floorPositions[7]; break;
            case Floor.Seven: targetFloorPos = floorPositions[8]; break;
            case Floor.Eight: targetFloorPos = floorPositions[9]; break;
        }
    }

    private bool IsInTargets(Floor floor) {
        return ((int)floor & targetFloors) > 0;
    }

    private int GetFloorNumber(Floor floor) {
        switch (floor) {
            case Floor.MinusOne: return -1;
            case Floor.Zero: return 0;
            case Floor.One: return 1;
            case Floor.Two: return 2;
            case Floor.Three: return 3;
            case Floor.Four: return 4;
            case Floor.Five: return 5;
            case Floor.Six: return 6;
            case Floor.Seven: return 7;
            case Floor.Eight: return 8;
            default: return 0;
        }
    }

    #endregion
    #region API_orders

    public void CallToFloor(Floor floor) {
        targetFloors |= (int)floor;
        if (IsStanding()) {
            driving = true;
            if ((int)currentFloor > (int)floor) {
                foreach (ElevatorListener listener in listeners) {
                    listener.DrivingDown();
                }
            }
            else if ((int)currentFloor < (int)floor) {
                foreach (ElevatorListener listener in listeners) {
                    listener.DrivingUp();
                }
            }
        }
        CalculateNearestTargetFloor((int)currentFloor - (int)floor);
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
        return GetDrivingDirection() == 0;
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
