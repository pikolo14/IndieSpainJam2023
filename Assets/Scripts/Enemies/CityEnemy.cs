using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class CityEnemy : EnemyBase
{
    public List<Sprite> Sprites;
    public GameObject LabelGO;
    protected TMP_Text _labelText;
    protected SpriteRenderer _renderer;

    protected float _height;


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
        _labelText = LabelGO.GetComponentInChildren<TMP_Text>(true);

        //Defensa de ciudades siempre iguales a la carga maxima
        Deffense = Mathf.FloorToInt(LevelGlobals.Moon.maxCharge);
    }

    public void SetLabelQuantity(int quantity)
    {
        _labelText.text = quantity.ToString();
        LabelGO.SetActive(quantity > 0);
    }
    protected override void Die()
    {
        EnemiesSpawnManager.Instance.DestroyCity(this);
        base.Die();
    }

    protected override void Move(){}
}
