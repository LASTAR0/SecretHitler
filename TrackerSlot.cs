
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TrackerSlot : UdonSharpBehaviour
{
    public void OnTriggerEnter(Collider other) {
        if (!other.name.Contains("Tracker")) {
            return;
        }
        Tracker tracker = other.GetComponent<Tracker>();
        if (tracker == null) return;

        if (Networking.GetOwner(other.gameObject) != Networking.LocalPlayer) return;

        VRC_Pickup pickup = other.GetComponent<VRC_Pickup>();
        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        pickup.Drop();
        tracker.transform.position = transform.position;
        tracker.transform.rotation = transform.rotation;
        rigidbody.velocity = Vector3.zero;
    }
}
