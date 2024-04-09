
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PropManager : UdonSharpBehaviour
{
    public GameManager manager;
    public Bullet[] bullets;
    public Magnifying[] magnifyings;

    [UdonSynced, HideInInspector] public bool[] isbulletEnabled;
    [UdonSynced, HideInInspector] public bool[] isMagnifyingEnabled;

    // [Master]
    public void EnableBullet(int index, bool bl) {
        isbulletEnabled[index] = bl;
        Debug.Log($"isBulletEnabled : {isbulletEnabled[index]}");
        RequestSerialization();
    }
    public void EnableMagnifying(int index, bool bl) {
        isMagnifyingEnabled[index] = bl;
        RequestSerialization();
    }

    public override void OnPreSerialization() { Sync(); }
    public override void OnDeserialization() { Sync(); }
    private void Sync() {
        for (int i = 0; i < bullets.Length; ++i) {
            Debug.Log($"bullet activeSelf : {bullets[i].gameObject.activeSelf}");
            if (bullets[i].gameObject.activeSelf != isbulletEnabled[i]) {
                bullets[i].gameObject.SetActive(isbulletEnabled[i]);
            }
        }
        for (int i = 0; i < magnifyings.Length; ++i) {
            if (magnifyings[i].gameObject.activeSelf != isMagnifyingEnabled[i]) {
                magnifyings[i].gameObject.SetActive(isMagnifyingEnabled[i]);
            }
        }
    }
    private void Start() {
        isbulletEnabled = new bool[2];
        isMagnifyingEnabled = new bool[2];
    }
}
