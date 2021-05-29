using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioObserver : Observer
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip attackClip, dieClip;
    public override void Notify(Enemy sender)
    {
        switch (sender.GetState())
        {
            case Enemy.State.ATTACKINGMELEE:
                PlayThisClip(attackClip);
                break;
            case Enemy.State.DYING:
                PlayThisClip(dieClip);
                break;
        }
    }

    private void PlayThisClip(AudioClip clip)
    {
        if (source.isPlaying)
            source.Stop();

        source.clip = clip;
        source.Play();
    }
}
