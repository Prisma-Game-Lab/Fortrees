using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour {
    [Header("Audio")]
    public AudioSource UIAudioSource;
    [Range(0, 1)] public float ConfirmationSoundVolume = 1f;
    public AudioClip ConfirmationSound;
    [Range(0, 1)] public float SelectionSoundVolume = 1f;
    public AudioClip SelectionSound;

    public void Select()
    {
        UIAudioSource.PlayOneShot(SelectionSound, SelectionSoundVolume);
    }

    public void Press()
    {
        UIAudioSource.PlayOneShot(ConfirmationSound, ConfirmationSoundVolume);
    }


}
