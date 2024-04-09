
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PolicyTrigBox : UdonSharpBehaviour
{
    public PolicyManager policyManager;
    private void OnTriggerEnter(Collider other) {
        if (policyManager.manager.player.candidate == (int)CANDIDATE.NOTHING) 
            return;
        if (other.name.Contains("HandBox")) {
            HandBox box = other.GetComponent<HandBox>();
            if (box == null) return;
            policyManager.EnterHandPolicy();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (policyManager.manager.player.candidate == (int)CANDIDATE.NOTHING) 
            return;
        if (other.name.Contains("HandBox")) {
            HandBox box = other.GetComponent<HandBox>();
            if (box == null) return;
            policyManager.ResetFill();
        }
    }
}
