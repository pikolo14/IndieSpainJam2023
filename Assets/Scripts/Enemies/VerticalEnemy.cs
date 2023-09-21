using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalEnemy : EnemyBase
{
    public float LaunchDelay = 3;
    public float TimeToLiveLaunched = 5;

    protected void Start()
    {
        StartCoroutine(WaitForLaunch());
    }

    protected override void Move() 
    {
        Vector3 direction = transform.position - MoonTransform.position;
        transform.position += direction.normalized * Speed;
    }

    protected IEnumerator WaitForLaunch()
    {
        yield return new WaitForSeconds(LaunchDelay);
        _moving = true;
        StartCoroutine(TimeToLiveCoroutine());
    }

    protected IEnumerator TimeToLiveCoroutine()
    {
        yield return new WaitForSeconds(TimeToLiveLaunched);
        Destroy(gameObject);
    }
}
