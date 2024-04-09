
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class LobbyUI : UdonSharpBehaviour
{
    public GameManager manager;
    public Text masterName;
    public GameObject objReady;
    public GameObject objProgress;
    public Text count, notEnough;
    public GameObject gPlay;
    public BoxCollider trigBoxPlay;
    
    private const int MIN_COUNT = 5;
    [UdonSynced] private bool _sync = false;

    public void Sync() {
        if (manager.isPlaying) {
            objProgress.SetActive(true);
            objReady.SetActive(false);
        } else {
            PlayerManager pm = manager.playerManager;
            objProgress.SetActive(false);
            objReady.SetActive(true);
            count.text = pm.cntPlayers.ToString() + "/10";

            if (pm.cntPlayers >= MIN_COUNT) {
                notEnough.enabled = false;
                if (Networking.LocalPlayer.isMaster) {
                    gPlay.SetActive(true);
                    trigBoxPlay.enabled = true;
                }
            } else {
                notEnough.enabled = true;
                if (Networking.LocalPlayer.isMaster) {
                    gPlay.SetActive(false);
                    trigBoxPlay.enabled = false;
                }
            }
        }
    }

    public override void OnPreSerialization() { Sync(); }
    public override void OnDeserialization() { Sync(); }

}
