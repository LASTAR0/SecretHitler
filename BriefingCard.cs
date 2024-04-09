
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class BriefingCard : UdonSharpBehaviour
{
    public void Enable() {
        gameObject.SetActive(true);
    }
}
