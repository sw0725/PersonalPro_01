using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : RecycleObject
{
    ParticleSystem particleSystem;
    float playLenth = 1.0f;

    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        particleSystem.Play();
        StartCoroutine(LifeOver(playLenth));
    }
}
