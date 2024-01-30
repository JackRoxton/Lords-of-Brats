using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance;
    public AudioSource Music;
    public AudioClip Song;
    private void Awake() {
        Instance = this;
        /*Music.clip = Song;
        Music.time = 0;
        Music.Play();*/
    }
    public void SwitchSong(AudioClip nuSong, float nuSongStartTime) {
        Music.Stop();
        Music.time = nuSongStartTime;
        Music.clip = nuSong;
        Music.Play();
    }
}
