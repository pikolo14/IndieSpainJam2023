using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static LevelGlobals;


public class CameraController : MonoBehaviour
{
    public float MoveDamping = 0.1f;  // Controls the smoothness of camera follow
    public float RotationDamping = 0.1f;  // Controls the smoothness of camera follow
    public float OffsetRadial = 0;

    private float _offsetZ;

    private void Awake()
    {
        _offsetZ = transform.position.z - Moon.transform.position.z;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition;
        //El objetivo es la posicion de la luna a la altura en reposo
        targetPosition = GetPolarPosition(Moon.currentAngle, MoonOrbitRadius + OffsetRadial);

        //Interpolar para hacer el acercamiento suave
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, MoveDamping);
        smoothedPosition.z = _offsetZ;
        transform.position = smoothedPosition;

        Vector3 targetUp = (Moon.transform.position - Moon.orbitTarget.position).normalized;
        Vector3 smoothedUp = Vector3.Lerp(transform.up, targetUp, RotationDamping);
        transform.up = smoothedUp;
    }
}
