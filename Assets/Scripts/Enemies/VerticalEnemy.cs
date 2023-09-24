using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalEnemy : EnemyBase
{
    public float LaunchDelay = 3;
    public float TimeToLiveLaunched = 5;
    protected GameObject _fire;
    protected float _height;


    public override void Initialize(float angle, float orbitalRadius = -1)
    {
        _currentAngle = angle;
        _height = GetComponent<Collider2D>().bounds.size.y;
        _orbitRadius = LevelGlobals.PlanetRadius + _height/2f;
        _fire = transform.GetChild(0).gameObject;
        _fire.SetActive(false);

        Locate();
        Orientate();

        StartCoroutine(WaitForLaunch());
    }

    protected override void Move()
    {
        Vector3 direction = transform.position - LevelGlobals.PlanetTransform.position;
        transform.position += direction.normalized * Speed * Time.deltaTime;
    }

    protected IEnumerator WaitForLaunch()
    {
        yield return new WaitForSeconds(LaunchDelay);
        _moving = true;
        _fire.SetActive(true);
        StartCoroutine(TimeToLiveCorotine());
    }

    protected IEnumerator TimeToLiveCorotine()
    {
        yield return new WaitForSeconds(TimeToLiveLaunched);
        Destroy(gameObject);
    }
}
