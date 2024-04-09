
using System.ComponentModel;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class PolicyUI : UdonSharpBehaviour
{
    public GameObject objPolicy, objRefuse;
    public Image imgPolicyFill, imgRefuseFill;
    public Image[] imgCountTen, imgCountOne;
    public Material[] matCount;
    [UdonSynced,HideInInspector] public bool isUIShow = true;
    [UdonSynced,HideInInspector] public bool isRefuseShow = false;
    [UdonSynced,HideInInspector] public int curCount = 17;

    public void Sync() {
        objPolicy.SetActive(isUIShow);
        objRefuse.SetActive(isRefuseShow);
        ChangeCount();
    }
    // [Master]
    public void Reset() {
        if (Networking.LocalPlayer.isMaster) {
            isUIShow = true;
            isRefuseShow = false;
            RequestSerialization();
        }
    }

    // [Master]
    public void ShowUI() {
        isUIShow = true;
        RequestSerialization();
    }
    public void HideUI() {
        isUIShow = false;
        RequestSerialization();
    }
    public void ShowRefuse() {
        isRefuseShow = true;
        RequestSerialization();
    }
    public void HideRefuse() {
        isRefuseShow = false;
        RequestSerialization();
    }
    // [Master]
    public void SetCount(int num) {
        curCount = num;
        RequestSerialization();
    }
    public void ChangeCount() {
        int ten = (int)(curCount / 10);
        int one = (int)(curCount % 10);
        imgCountTen[0].material = matCount[ten];
        imgCountTen[1].material = matCount[ten];
        imgCountOne[0].material = matCount[one];
        imgCountOne[1].material = matCount[one];
    }
    public override void OnDeserialization() { Sync(); }
    public override void OnPreSerialization() { Sync(); }
}
