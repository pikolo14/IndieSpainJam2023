using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy : EnemyBase
{
    protected override bool _moving { get; set; } = true;
    public float TimeToLive = 5;

    protected void Start()
    {
        StartCoroutine(TimeToLiveCoroutine());
    }

    protected override void Move()
    {
        transform.position += Vector3.up * Speed;
    }
    protected IEnumerator TimeToLiveCoroutine()
    {
        yield return new WaitForSeconds(TimeToLive);
        Destroy(gameObject);
    }
}
