using UnityEngine;
using System.Collections;

/// <summary>
/// Triggers objects OnTrigger enter or exit or stay
/// </summary>
[RequireComponent(typeof(Collider))]
public class TriggerZone : MonoBehaviour {
    
    public TriggerableComponent[] triggerOnEnter;
    public TriggerableComponent[] triggerOnStay;
    public TriggerableComponent[] triggerOnExit;

    public bool activate;

    public string triggerTag = "Player";

    void OnTriggerEnter(Collider col) {
        if (triggerOnEnter == null) return;

        if (col.CompareTag(triggerTag)) {
            foreach (TriggerableComponent trig in triggerOnEnter) {
                trig.Trigger(activate?1:0);
            }
        }
    }

    void OnTriggerStay(Collider col) {
        if (triggerOnStay == null) return;

        if (col.CompareTag(triggerTag)) {
            foreach (TriggerableComponent trig in triggerOnStay) {
                trig.Trigger(activate ? 1 : 0);
            }
        }
    }

    void OnTriggerExit(Collider col) {
        if (triggerOnExit == null) return;

        if (col.CompareTag(triggerTag)) {
            foreach (TriggerableComponent trig in triggerOnExit) {
                trig.Trigger(activate ? 1 : 0);
            }
        }
    }

}
