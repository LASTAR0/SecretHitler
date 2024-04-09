
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class Briefing : UdonSharpBehaviour
{
    public Material[] matFascist;
    public Material[] matLiberal;
    public MeshRenderer[] meshs;

    private const int MAX_NUM = 3;
    private LANGUAGE _langType = 0;

    public void ChangeMaterial(int index) {
        meshs[0].material = matFascist[index + MAX_NUM * (int)_langType];
        meshs[1].material = matLiberal[(int)_langType];
    }
    public void ChangeLanguage(LANGUAGE type) {
        _langType = type;
    }

}
