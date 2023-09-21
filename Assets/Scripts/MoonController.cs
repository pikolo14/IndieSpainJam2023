using System;
using System.Collections;
using UnityEngine;

public class MoonController : MonoBehaviour
{
    [Header("Orbit")]
    public Transform orbitTarget;
    public float orbitRadius = 1f;
    public float orbitSpeed = 2f;

    [Header("Charge")]
    public float maxCharge = 3f;
    public float chargeRate = 1f;
    public float chargeTime = 1f;
    //public float[] chargeSteps;
    //public AnimationCurve chargeCurve;

    [HideInInspector]
    public float startingOrbitRadius = 1f;

    private float currentAngle = 0f;
    [SerializeField]
    private bool isAttacking, isChargingAttack;
    private Collider2D col;
    [SerializeField]
    [Range(0f, 3f)]
    private float currentCharge = 0f;

    private void Awake()
    {
        if (orbitTarget == null)
        {
            Debug.LogError("Target object not assigned.");
            enabled = false;
            return;
        }
        col = GetComponent<Collider2D>();
        startingOrbitRadius = orbitRadius;
    }

    private void Start()
    {
        transform.position = (Vector2)orbitTarget.position + Vector2.up * (orbitRadius + GetMoonRadius());
    }

    void Update()
    {
        Attack();
        Movement();
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Jump") && !isChargingAttack)
            isChargingAttack = true;

        if (isChargingAttack && Input.GetButton("Jump"))
        {
            currentCharge += chargeRate * Time.deltaTime;
            currentCharge = Mathf.Clamp(currentCharge, 0, maxCharge);
        }

        if (Input.GetButtonUp("Jump"))
            StartCoroutine(AttackCoroutine(currentCharge));
    }

    private void Movement()
    {
        if (isAttacking || isChargingAttack) return;
        orbitTarget.position = orbitTarget.position;

        currentAngle += (Input.GetAxis("Horizontal") * -1f) * orbitSpeed * Time.deltaTime; //By using GetAxis we already have acceleration

        float newX = (orbitTarget.position.x + orbitRadius + GetMoonRadius()) * Mathf.Cos(currentAngle);
        float newY = (orbitTarget.position.y + orbitRadius + GetMoonRadius()) * Mathf.Sin(currentAngle);

        transform.position = new Vector2(newX, newY);
    }

    public float GetMoonRadius()
    {
        return col.bounds.extents.y;
    }

    private void OnDrawGizmosSelected()
    {
        if (col == null) col = GetComponent<Collider2D>();
        if (col == null || orbitTarget == null) return;
        Gizmos.DrawLine(orbitTarget.position, orbitTarget.position + Vector3.up * (orbitRadius + GetMoonRadius()));
        Gizmos.DrawWireSphere(orbitTarget.position, (orbitTarget.position + Vector3.up * (orbitRadius + GetMoonRadius())).y);
        Gizmos.DrawWireSphere(orbitTarget.position + Vector3.up * (orbitRadius + GetMoonRadius()), GetMoonRadius());

    }

    public IEnumerator AttackCoroutine(float attackCharge)
    {
        isAttacking = true;
        float elapsed = 0f;
        float lowerLimit = orbitTarget.GetComponent<Collider2D>().bounds.extents.y + GetMoonRadius();
        //Go down
        while (elapsed < chargeTime)
        {
            orbitRadius = 
            elapsed += Time.deltaTime;
        }
        //Go up
        isAttacking = false;
        isChargingAttack = false;
        currentCharge = 0f;
        yield return null;
    }

}