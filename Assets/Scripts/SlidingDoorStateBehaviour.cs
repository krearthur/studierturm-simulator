using UnityEngine;
using System.Collections.Generic;

public class SlidingDoorStateBehaviour : StateMachineBehaviour {

    private List<SlidingDoorListener> listeners = new List<SlidingDoorListener>();

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        foreach(SlidingDoorListener li in listeners) {
            if (stateInfo.IsName("Opening")) {
                li.DoorOpening();
            }
            else if (stateInfo.IsName("Open")) {
                li.DoorOpened();
            }
            else if (stateInfo.IsName("Closing")) {
                li.DoorClosing();
            }
            else if (stateInfo.IsName("Closed")) {
                li.DoorClosed();
            }
        }
    }


	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

    public void AddListener(SlidingDoorListener li) {
        listeners.Add(li);
    }
}
