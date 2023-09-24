using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CityEnemy : EnemyBase
{
    public List<Sprite> Sprites;
    public GameObject LabelGO;
    protected TextMeshPro _labelText;
    protected SpriteRenderer _renderer;

    protected float _height;

    //TODO: Hacer parones aleatorios

    public override void Initialize(float angle, float orbitalRadius = -1)
    {
        _currentAngle = angle;
        _height = GetComponentInChildren<Renderer>().bounds.size.y;
        _orbitRadius = LevelGlobals.PlanetRadius + _height / 2f +0.03f;

        Locate();
        Orientate();
    }

    protected void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.sprite = Sprites[Random.Range(0,Sprites.Count)];

        _labelText = LabelGO.GetComponentInChildren<TextMeshPro>(true);

        //Defensa de ciudades siempre iguales a la carga maxima
        Deffense = Mathf.FloorToInt(LevelGlobals.Moon.maxCharge);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EnemiesSpawnManager.Instance.IncreaseDifficulty();
    }

    protected void ChangeLabel(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            LabelGO.SetActive(true);
            _labelText.text = text;
        }
        else
            LabelGO.SetActive(false);
    }

    protected override void Move(){}
}
