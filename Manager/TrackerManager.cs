
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TrackerManager : UdonSharpBehaviour
{
    public GameManager manager;
    public Tracker tracker;
    public GameObject trackerUI;
    public TrackerIcon trackerIcon;
    private const float FadeDuration = 3.0f;

    private bool _isEntering = false;
    private float _fill = 0.0f;
    private void Update() {
        if (!_isEntering) return;

        _fill += Time.deltaTime / FadeDuration;
        _fill = Mathf.Clamp01(_fill);
        trackerIcon.image.fillAmount = _fill;
        if (_fill >= 1.0f) {
            ResetIcon();
            tracker.OnReset();
            if (Networking.LocalPlayer == Networking.GetOwner(tracker.gameObject)) {
                manager.policyManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ApplyOnePolicy");
            }
        }
    }

    // [Local]
    public void ResetIcon() {
        _isEntering = false;
        _fill = 0.0f;
        trackerIcon.image.fillAmount = _fill;
    }

    // [All]
    public void OnTrackerEnter() {
        ResetIcon();
        _isEntering = true;
        trackerUI.SetActive(true);
    }
    // [All]
    public void OnTrackerExit() {
        ResetIcon();
        _isEntering = false;
        trackerUI.SetActive(false);
    }
}
