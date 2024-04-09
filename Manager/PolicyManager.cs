
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;


public class PolicyManager : UdonSharpBehaviour
{
    public GameManager manager;
    public PolicyCard[] cards;
    public Material[] matPolicy;
    public PolicyUI policyUI;
    //public Image policyFill;
    
    private const int MAX_ALL = 17;
    private const int MAX_LIBERAL = 6;
    private const int MAX_FASCIST = 11;
    private const float FadeDuration = 3.0f;
    // 0 = Fascist, 1 = Liberal
    private DataList deck = new DataList();
    private DataList trash = new DataList();
    public bool _isPresidentTurn = false;

    private bool _isEntering = false;
    private float _fill = 0.0f;
    // 0 = Deck, 1 = Refuse
    private int _type = 0;

    // [Local] Fill Function
    private void Update() {
        if (!_isEntering) return;

        _fill += Time.deltaTime / FadeDuration;
        _fill = Mathf.Clamp01(_fill);
        if (_type == 0) { policyUI.imgPolicyFill.fillAmount = _fill; }
        else { policyUI.imgRefuseFill.fillAmount = _fill; }
        
        if (_fill >= 1.0f) {
            ResetFill();
            if (_type == 0) {
                policyUI.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "HideUI");
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ShowPolicyCards");
            } else {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "ThrowCards");
            }
        }
    }
    public void ResetFill() {
        _isEntering = false;
        _fill = 0.0f;
        policyUI.imgPolicyFill.fillAmount = _fill;
        policyUI.imgRefuseFill.fillAmount = _fill;
    }
    public void EnterHandPolicy() {
        _type = 0;
        _isEntering = true;
    }
    public void EnterHandRefuse() {
        _type = 1;
        _isEntering = true;
    }



    // [Master] Policy Deck Setting
    public void ReadyDeck() {
        for (int i = 0; i < MAX_ALL; ++i) {
            if (i < 11) {
                deck.Add((int)POLICY.FASCIST);
            } else {
                deck.Add((int)POLICY.LIBERAL);                          
            }
        }
        Shuffle();
    }
    public void Shuffle() {
        for (int i = deck.Count - 1; i > 0; i--) {
            int rand = Random.Range(0, i + 1);
            DataToken temp = deck[i];
            deck[i] = deck[rand];
            deck[rand] = temp;
        }
        for (int i = 0; i < deck.Count; ++i) {
            Debug.Log(deck[i]);
        }
        Debug.Log($"deck count : {deck.Count}");
    }


    public void ShowPolicyCards() {
        _isPresidentTurn = true;
        if (deck.Count < 3) {
            // 덱에 3개 이하가 남음, 리셋시켜야함
            for (int i = 0; i < deck.Count; ++i) {
                trash.Add(deck[i]);
                Debug.Log($"trash 에 {deck[i]} 추가");
            }
            deck.Clear();
            Debug.Log($"deck count : {deck.Count}");
            deck = trash.DeepClone();
            trash.Clear();
            Shuffle();
        }
        foreach(PolicyCard c in cards) {
            if (deck[0] == (int)POLICY.FASCIST) {
                c.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChangeMaterialFascist");
            } else {
                c.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ChangeMaterialLiberal");
            }
            deck.RemoveAt(0);
        }
    }

    // [Master]
    public void SelectCard(PolicyCard card, int type) {
        if (_isPresidentTurn) {
            _isPresidentTurn = false;
            trash.Add(type);
            card.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HideCard");
            if (manager.briefingManager.numFascist >= 5) {
                policyUI.ShowRefuse();
            }
        } else {
            card.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HideCard");
            foreach (PolicyCard c in cards) {
                if (c.type != -1) {
                    trash.Add(c.type);
                    Debug.Log($"trash count : {trash.Count}");
                    c.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HideCard");
                }
            }
            if (type == (int)POLICY.FASCIST) { manager.briefingManager.AddPolicyFascist(); } 
            else { manager.briefingManager.AddPolicyLiberal(); }
            policyUI.ShowUI();
            policyUI.SetCount(deck.Count);
            if (manager.briefingManager.numFascist >= 5) {
                policyUI.HideRefuse();
            }
        }
    }
    // [Master]
    public void ThrowCards() {
        foreach(PolicyCard c in cards) {
            if (c.type != -1) {
                trash.Add(c.type);
                c.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HideCard");
            }
        }
        policyUI.ShowUI();
        policyUI.HideRefuse();
        policyUI.SetCount(deck.Count);
    }

    // [Master] Open Policy Card, Just 1
    public void ApplyOnePolicy() {
        if (deck.Count < 0) {
            Debug.Log($"PolicyManager, ApplyOnePolicy, deck Count < {deck.Count}");
            return;
        }
        if (deck[0] == (int)POLICY.FASCIST) {
            manager.briefingManager.AddPolicyFascist();
            //Debug.Log("Add Fascist Policy");
        } else {
            manager.briefingManager.AddPolicyLiberal();
            //Debug.Log("Add Liberal Policy");
        }
        deck.RemoveAt(0);
        policyUI.SetCount(deck.Count);
    }
}
