using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GroupSound {
    private int count = 0;
    private static GroupSound _instance;
    public AudioSource[] AudioSources = new AudioSource[3];

    [HideInInspector] public static GroupSound GetInstance
    {
        get { return _instance ?? (_instance = new GroupSound()); }
    }

    private Dictionary<AudioClip, int> _audioVsQnt = new Dictionary<AudioClip, int>();
    public Dictionary<AudioClip, AudioSource> _audioDict = new Dictionary<AudioClip, AudioSource>();

    public GroupSound()
    {
        _instance = this;
    }

    public void EnemyEnter(AudioClip audio)
    {
        if (!_audioVsQnt.ContainsKey(audio))
        {
            _audioVsQnt.Add(audio, 0);
        }

        if (_audioVsQnt[audio] == 0)
        {
            InitAudio(audio);
        }

        _audioVsQnt[audio]++;
    }


    public void EnemyLeft(AudioClip audio)
    {
        _audioVsQnt[audio]--;

        Debug.Log("Enemies with " + audio + " left: " + _audioVsQnt[audio]);

        if(_audioVsQnt[audio] == 0) {
            StopAudio(audio);
        }
    }

    private void InitAudio(AudioClip audio)
    {
        if (!_audioDict.ContainsKey(audio))
        {
            _audioDict.Add(audio, AudioSources[count++]);
        }

        _audioDict[audio].clip = audio;
        _audioDict[audio].loop = true;
        _audioDict[audio].Play();
    }

    private void StopAudio(AudioClip audio)
    {
        Debug.Log("Parou o audio " + audio);
        _audioDict[audio].Stop();
    }
}
