
using UdonSharp;
using Unity.Mathematics;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Badge : UdonSharpBehaviour
{
    private Vector3 resetPos;
    private Quaternion resetRot;

    public void OnResetPos() {
        transform.position = resetPos;
        transform.rotation = resetRot;
    }
    public void OnEnableBadge() {
        gameObject.SetActive(true);
    }
    public void OnDisableBadge() {
        gameObject.SetActive(false);
    }
    private void Start() {
        resetPos = transform.position;
        resetRot = transform.rotation;
    }
}
