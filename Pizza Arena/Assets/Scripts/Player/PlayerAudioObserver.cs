using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioObserver : PlayerObserver
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip meleeAttackClip, damagedClip;
    public override void Notify(PlayerController sender)
    {
        switch (sender.GetState())
        {
            case PlayerController.State.MELEEATTACK:
                PlayAudioClip(meleeAttackClip);
                break;
            case PlayerController.State.DAMAGED:
                PlayAudioClip(damagedClip);
                break;
        }
    }

    private void PlayAudioClip(AudioClip clip)
    {
        if (source.isPlaying)
            source.Stop();

        source.clip = clip;
        source.Play();
    }
}
