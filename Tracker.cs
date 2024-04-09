
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Tracker : UdonSharpBehaviour
{
    private Vector3 originPos;

    public void OnReset() {
        transform.position = originPos;
        transform.rotation = Quaternion.identity;
    }
    private void Start() {
        originPos = transform.position;
    }
}
