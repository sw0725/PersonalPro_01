using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType 
{
    EnemyBlue = 0,
    HitEffect
}

public class Factory : Singltrun<Factory>
{
    EnemyBluePool enemyBlue;
    HitEffectPool hitEffect;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        enemyBlue = GetComponentInChildren<EnemyBluePool>();
        if (enemyBlue != null) enemyBlue.Initialized();
        hitEffect = GetComponentInChildren<HitEffectPool>();
        if (hitEffect != null) hitEffect.Initialized();
    }
     
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null) 
    {
        GameObject result = null;
        switch (type)
        {
            case PoolObjectType.EnemyBlue:
                result = enemyBlue.GetObject(position, euler).gameObject;
                break;
            case PoolObjectType.HitEffect:
                result = hitEffect.GetObject(position, euler).gameObject;
                break;
        }
        return result;
    }

    public EnemyBace GetEnemyBlue()
    {
        return enemyBlue.GetObject();
    }

    public EnemyBace GetEnemyBlue(Vector3 position, float angle = 0.0f)
    {
        return enemyBlue.GetObject(position, angle * Vector3.forward);
    }
    public HitEffect GetHitEffect()
    {
        return hitEffect.GetObject();
    }

    public HitEffect GetHitEffect(Vector3 position, float angle = 0.0f)
    {
        return hitEffect.GetObject(position, angle * Vector3.forward);
    }
}
