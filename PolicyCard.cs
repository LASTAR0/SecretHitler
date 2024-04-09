
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PolicyCard : UdonSharpBehaviour
{
    public PolicyManager policyManager;
    public MeshRenderer mesh;
    public int type = -1;

    public override void Interact() {
        gameObject.SetActive(false);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "OnClick");
    }
    public void ChangeMaterialFascist() {
        gameObject.SetActive(true);
        type = (int)POLICY.FASCIST;
        mesh.material = policyManager.matPolicy[type];
    }
    public void ChangeMaterialLiberal() {
        gameObject.SetActive(true);
        type = (int)POLICY.LIBERAL;
        mesh.material = policyManager.matPolicy[type];
    }
    // [Master]
    public void OnClick() {
        int temp = type;
        type = -1;
        policyManager.SelectCard(this, temp);
        
    }
    
    public void HideCard() {
        gameObject.SetActive(false);
    }
}
