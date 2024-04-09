
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public enum LANGUAGE {
    ENG = 0,
    KOR = 1,
}
public class LanguageManager : UdonSharpBehaviour
{
    public Howtoplay[] howtoplays;
    public RuleBook[] ruleBooks;
    public Briefing briefing;
    public Image[] icons;

    private LANGUAGE _curType = 0;

    public void ChangeEnglish() {
        if (_curType == LANGUAGE.ENG)
            return;
        _curType = LANGUAGE.ENG;
        icons[(int)LANGUAGE.ENG].color = new Color(1f, 1f, 1f, 1f);
        icons[(int)LANGUAGE.KOR].color = new Color(1f, 1f, 1f, 0.4f);
        foreach (Howtoplay h in howtoplays) 
            h.ChangeLanguage(LANGUAGE.ENG);
        foreach (RuleBook b in ruleBooks) 
            b.ChangeLanguage(LANGUAGE.ENG);
        briefing.ChangeLanguage(LANGUAGE.ENG);
    }
    public void ChangeKorean() {
        if (_curType == LANGUAGE.KOR)
            return;
        _curType = LANGUAGE.KOR;
        icons[(int)LANGUAGE.ENG].color = new Color(1f, 1f, 1f, 0.4f);
        icons[(int)LANGUAGE.KOR].color = new Color(1f, 1f, 1f, 1.0f);
        foreach (Howtoplay h in howtoplays) 
            h.ChangeLanguage(LANGUAGE.KOR);
        foreach (RuleBook b in ruleBooks) 
            b.ChangeLanguage(LANGUAGE.KOR);
        briefing.ChangeLanguage(LANGUAGE.KOR);
    }
}
