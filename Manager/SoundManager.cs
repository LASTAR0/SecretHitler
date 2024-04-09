
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SoundManager : UdonSharpBehaviour
{
    public AudioSource lobbyTheme;
    public AudioSource mainTheme;
    public AudioSource sndVictory;
    public AudioSource sndDefeat;
    public AudioSource sndCredit;

    private float _volume;
    private float _delta;
    private float _fadeDuration = 2.0f;
    private bool _isChanging;

    private void Update() {
        if (_isChanging)
        {
            _volume -= Time.deltaTime / _fadeDuration;
            _volume = Mathf.Clamp01(_volume);

            lobbyTheme.volume = _volume;

            if (_volume <= 0f)
            {
                _isChanging = false;
                lobbyTheme.Stop();
                mainTheme.gameObject.SetActive(true);
            }
        }
    }
    public void ChangeTheme() {
        _volume = lobbyTheme.volume;
        _isChanging = true;
    }
    public void PlayVictory() {
        mainTheme.Stop();
        sndVictory.Play();
        SendCustomEventDelayedSeconds("PlayCredit" , 23.0f);
    }
    public void PlayDefeat() {
        mainTheme.Stop();
        sndDefeat.Play();
        SendCustomEventDelayedSeconds("PlayCredit", 8.0f);
    }
    public void PlayCredit() {
        sndCredit.Play();
    }
}
