
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TrackerLastSlot : UdonSharpBehaviour
{
    public TrackerManager trackerManager;
    public void OnTriggerEnter(Collider other) {
        if (!isTracker(other)) return;

        VRC_Pickup pickup = other.GetComponent<VRC_Pickup>();
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        pickup.Drop();
        other.transform.position = transform.position;
        other.transform.rotation = transform.rotation;
        rigidbody.velocity = Vector3.zero;

        trackerManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OnTrackerEnter");
    }
    public void OnTriggerExit(Collider other) {
        if (!isTracker(other)) return;
        trackerManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "OnTrackerExit");
    }

    private bool isTracker(Collider other) {
        if (!other.name.Contains("Tracker")) return false;
        Tracker tracker = other.GetComponent<Tracker>();
        if (tracker == null) return false;
        if (Networking.GetOwner(other.gameObject) != Networking.LocalPlayer) return false;
        return true;
    }
}
