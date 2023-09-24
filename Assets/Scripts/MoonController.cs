using System;
using System.Collections;
using UnityEngine;

public class MoonController : MonoBehaviour
{
    [Header("Options")]
    public bool isActive = false;

    [Header("Orbit")]
    public Transform orbitTarget;
    public float orbitRadius = 1f;
    public float orbitSpeed = 2f;

    [Header("Charge")]
    public float maxCharge = 3f;
    public float chargeRate = 1f;
    public float chargeTime = .25f;
    public float downTime = .25f;
    public float recoveryTime = .5f;
    [Range(0f, 1f)]
    public float speedDuringCharge = .5f;
    public AnimationCurve chargeDownCurve, chargeUpCurve;

    [Header("References")]
    public GameObject moonHitParticleSystem;

    [HideInInspector]
    public float startingOrbitRadius = 1f;
    public float currentAngle = 0f;

    [SerializeField]
    private bool isAttacking, isChargingAttack, canAttack = true;
    private Collider2D col;
    [SerializeField, Range(0f, 3f)]
    private float currentCharge = 0f;
    private MoonHealth moonHealth;

    private void Awake()
    {
        moonHealth = GetComponent<MoonHealth>();
        if (orbitTarget == null)
        {
            Debug.LogError("Target object not assigned.");
            enabled = false;
            return;
        }
        col = GetComponent<Collider2D>();
        startingOrbitRadius = orbitRadius;
        currentAngle = Mathf.PI / 2;
    }

    private void Start()
    {
        transform.position = (Vector2)orbitTarget.position + Vector2.up * (orbitRadius + GetMoonRadius());
    }

    void Update()
    {
        Attack();
    }

    void FixedUpdate()
    {
        if (!isActive) return;
        Movement();
        Rotation();
    }

    private void Attack()
    {
        if (Input.GetButtonDown("Jump") && !isChargingAttack && !isAttacking)
            isChargingAttack = true;

        if (isChargingAttack && Input.GetButton("Jump"))
        {
            currentCharge += chargeRate * Time.deltaTime;
            currentCharge = Mathf.Clamp(currentCharge, 0, maxCharge);
        }

        if (canAttack && isChargingAttack && Input.GetButtonUp("Jump"))
            StartCoroutine(AttackCoroutine(currentCharge));
    }

    private void Movement()
    {
        //float inputValue = !(isAttacking || isChargingAttack) ? (Input.GetAxis("Horizontal") * -1f) : 0);

        //float inputValue = !isAttacking ? (!isChargingAttack ? (Input.GetAxis("Horizontal") * -1f) : (Input.GetAxis("Horizontal") * (-1f * speedDuringCharge))) : 0f; //Biba la lejivilidad del kodygo
        float inputValue = 0f;
        if (!isAttacking)
            if (!isChargingAttack)
                inputValue = Input.GetAxis("Horizontal") * -1f;
            else
                inputValue = Input.GetAxis("Horizontal") * (-1f * speedDuringCharge);

        currentAngle += inputValue * orbitSpeed * Time.deltaTime; //By using GetAxis we already have acceleration

        float newX = (orbitTarget.position.x + orbitRadius + GetMoonRadius()) * Mathf.Cos(currentAngle);
        float newY = (orbitTarget.position.y + orbitRadius + GetMoonRadius()) * Mathf.Sin(currentAngle);
        Debug.DrawLine(orbitTarget.position, new Vector2(newX, newY), Color.green, Time.deltaTime);

        transform.position = new Vector2(newX, newY);
    }

    public float GetMoonRadius()
    {
        return col.bounds.extents.y;
    }
    private void Rotation()
    {
        transform.up = (transform.position - orbitTarget.position).normalized;
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
        canAttack = false;
        isChargingAttack = false;
        isAttacking = true;
        float elapsed = 0f;
        float lowerLimit = orbitTarget.GetComponent<Collider2D>().bounds.extents.y;

        float startingPos = orbitRadius;
        //-- Go down --
        while (elapsed < chargeTime)
        {
            orbitRadius = Mathf.Lerp(startingPos, lowerLimit, chargeDownCurve.Evaluate(elapsed / chargeTime));
            elapsed = Mathf.Clamp(elapsed + Time.deltaTime, 0, chargeTime);
            yield return new WaitForEndOfFrame();
        }
        //-------------
        isAttacking = false;
        //ToDo: Spawn ground hit particles
        yield return new WaitForSeconds(downTime);
        //ToDo: Screenshake?
        //-- Go up --
        while (elapsed > 0)
        {
            orbitRadius = Mathf.Lerp(startingPos, lowerLimit, chargeUpCurve.Evaluate(elapsed / recoveryTime));
            elapsed = Mathf.Clamp(elapsed - Time.deltaTime, 0, recoveryTime);
            yield return new WaitForEndOfFrame();
        }
        //-------------
        canAttack = true;
        currentCharge = 0f;
        yield return new WaitForEndOfFrame();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttacking)
        {
            if (collision.TryGetComponent(out EnemyBase enemy))
                if (!enemy.TryKill(Mathf.FloorToInt(currentCharge)))
                    moonHealth.TakeDamage(1);
        }
        else
        {
            if (collision.TryGetComponent(out EnemyBase enemy))
                moonHealth.TakeDamage(1);
        }
        //Instantiate(moonHitParticleSystem, collision.ClosestPoint(transform.position), Quaternion.identity);
    }

}