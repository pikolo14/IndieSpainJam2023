using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : EnemyBase
{
    public float TimeToLive = 5;

    protected void Start()
    {
        _moving = true;
        StartCoroutine(TimeToLiveCoroutine());
    } 

    protected override void Move()
    {
        transform.position += transform.up * Speed;
    }
    protected IEnumerator TimeToLiveCoroutine()
    {
        yield return new WaitForSeconds(TimeToLive);
        Destroy(gameObject);
    }
}
