
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class Howtoplay : UdonSharpBehaviour
{
    public Image image;
    public Sprite[] sprites;
    
    private const int MAX_PAGE = 3;
    private int _curPage = 0;
    private LANGUAGE _langType = 0;

    public void OnNext() {
        if (_curPage < MAX_PAGE - 1) {
            _curPage++;
            ChangeImage();
        }
    }
    public void OnPrev() {
        if (_curPage > 0) {
            _curPage--;
            ChangeImage();
        }
    }
    public void ChangeLanguage(LANGUAGE type) {
        _langType = type;
        ChangeImage();
    }
    private void ChangeImage() {
        image.sprite = sprites[_curPage + MAX_PAGE * (int)_langType];
    }
}
