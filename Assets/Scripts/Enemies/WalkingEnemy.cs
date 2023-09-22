using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : OrbitalEnemy
{
    protected float _height;

    //TODO: Hacer parones aleatorios

    public override void Initialize(float angle, float orbitalRadius = -1)
    {
        _moving = true;
        _currentAngle = angle;
        _height = GetComponentInChildren<Collider2D>().bounds.size.y;
        _orbitRadius = LevelGlobals.PlanetRadius + _height/2f;

        SetRandomDirection();
        Locate();
        Orientate();
    }
}
