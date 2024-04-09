
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ParticleGroup : UdonSharpBehaviour
{
    public ParticleSystem[] particles;
    public AudioSource[] audios;

    public void Active() {
        foreach (ParticleSystem p in particles) { p.Play(); }
        foreach (AudioSource s in audios) { s.Play(); }
    }
}
