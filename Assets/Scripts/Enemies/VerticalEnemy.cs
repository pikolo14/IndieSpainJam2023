using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalEnemy : EnemyBase
{
    public float LaunchDelay = 3;
    public float TimeToLiveLaunched = 5;
    protected GameObject _fire;
    protected float _height;
    protected Tweener _shakeTween;
    public GameObject SmokeParticles;


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

        var parts = Instantiate(SmokeParticles, null);
        parts.transform.position = transform.position - transform.up * _height / 2f;
        parts.transform.rotation = transform.rotation;
    }

    protected override void Move()
    {
        Vector3 direction = transform.position - LevelGlobals.PlanetTransform.position;
        transform.position += direction.normalized * Speed * Time.deltaTime;
    }

    protected IEnumerator WaitForLaunch()
    {
        //Vibracion durante lanzamiento
        _shakeTween = transform.DOShakePosition(LaunchDelay-0.6f,0.05f,30,45,false,false).SetEase(Ease.InQuad);
        DOVirtual.Float(0.1f, 1, LaunchDelay, (value) => _shakeTween.timeScale = value).SetEase(Ease.InQuad);

        yield return new WaitForSeconds(LaunchDelay);

        _moving = true;
        _fire.SetActive(true);
        AudioManager.self.PlayAdditively(SoundId.RocketLaunch);
        StartCoroutine(TimeToLiveCorotine());
    }

    protected IEnumerator TimeToLiveCorotine()
    {
        yield return new WaitForSeconds(TimeToLiveLaunched);
        Destroy(gameObject);
    }
}
