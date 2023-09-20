using UnityEngine;

public class Orbitante : MonoBehaviour
{
    public Transform target;
    public float orbitRadius = 1f;
    public float orbitSpeed = 2f;
    public AnimationCurve curve;

    private Vector2 orbitCenter;
    private float currentAngle = 0f;

    void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target object not assigned.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        orbitCenter = target.position;

        currentAngle += Input.GetAxis("Horizontal") * orbitSpeed * Time.deltaTime;

        float newX = orbitCenter.x + orbitRadius * Mathf.Cos(currentAngle);
        float newY = orbitCenter.y + orbitRadius * Mathf.Sin(currentAngle);

        transform.position = new Vector2(newX, newY);
    }
}