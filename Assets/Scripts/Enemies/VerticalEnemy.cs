using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalEnemy : EnemyBase
{
    public float LaunchDelay = 3;
    public float TimeToLiveLaunched = 5;
    protected float _height;
    

    public override void Initialize(float angle, float orbitalRadius = -1)
    {
        _currentAngle = angle;
        _height = GetComponent<Collider2D>().bounds.size.y;
        _orbitRadius = LevelGlobals.PlanetRadius + _height/2f;

        Locate();
        Orientate();

        StartCoroutine(WaitForLaunch());
    }

    protected override void Move()
    {
        Vector3 direction = transform.position - LevelGlobals.PlanetTransform.position;
        transform.position += direction.normalized * Speed;
    }

    protected IEnumerator WaitForLaunch()
    {
        yield return new WaitForSeconds(LaunchDelay);
        _moving = true;
        StartCoroutine(TimeToLiveCorotine());
    }

    protected IEnumerator TimeToLiveCorotine()
    {
        yield return new WaitForSeconds(TimeToLiveLaunched);
        Destroy(gameObject);
    }
}
