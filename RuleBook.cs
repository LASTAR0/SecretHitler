
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RuleBook : UdonSharpBehaviour
{
    public GameObject[] obj;
    public MeshRenderer[] meshes;
    public Material[] mats;

    private const int MAX_PAGE = 7;
    private bool _isPickup = false;
    private int _curPage = 0;
    private LANGUAGE _langType = 0;

    public override void OnPickup() {
        obj[0].SetActive(true);
        obj[1].SetActive(false);
    }
    public override void OnDrop() {
        obj[0].SetActive(false);
        obj[1].SetActive(true);
    }
    public override void OnPickupUseDown() {
        _curPage++;
        
        if (_curPage >= MAX_PAGE)
            _curPage = 0;

        ChangeMaterial();
    }
    public void ChangeLanguage(LANGUAGE type) {
        _langType = type;
        ChangeMaterial();
    }
    private void ChangeMaterial() {
        meshes[0].material = mats[_curPage + MAX_PAGE * (int)_langType];
        meshes[1].material = mats[_curPage + MAX_PAGE * (int)_langType];
    }
}
