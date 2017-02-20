using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSounds : MonoBehaviour {
    [Header("Audio")]
    public AudioSource UIAudioSource;
    public AudioClip ConfirmationSound;
    public AudioClip SelectionSound;

    public void Select()
    {
        UIAudioSource.PlayOneShot(SelectionSound);
    }

    public void Press()
    {
        UIAudioSource.PlayOneShot(ConfirmationSound);
    }


}
