using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : OrbitalEnemy
{
    public float Height;


    protected new void Start()
    {
        SetRandomDirection();

        //Orbitar a ras de suelo
        _orbitRadius = EarthRadius + Height/2f;
    }
}
