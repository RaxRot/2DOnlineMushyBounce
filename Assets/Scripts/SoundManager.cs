using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private AudioSource bumpSound;

    private void OnEnable()
    {
        Egg.onBumb += PlayBumpSound;
    }

    private void OnDisable()
    {
        Egg.onBumb -= PlayBumpSound;
    }

    public void PlayBumpSound()
    {
        bumpSound.Play();
    }
}
