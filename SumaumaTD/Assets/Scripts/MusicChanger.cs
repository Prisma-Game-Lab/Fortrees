using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour {
    public AudioSource MusicSource;
    public AudioClip NewMusic;
    public bool loop = false;

	// Use this for initialization
	void OnEnable () {
        MusicSource.clip = NewMusic;
        MusicSource.loop = loop;
        MusicSource.Stop();
        MusicSource.Play();
	}
}
