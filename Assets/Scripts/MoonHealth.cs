using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class MoonHealth : MonoBehaviour
{
    public int maxHP = 3;
    public int remainingHP = 1;
    public UnityEvent OnDeath;
    public float InvulnerabilityTime = 2f;
    public bool _invulnerable = false;


    private void Awake()
    {
        remainingHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        if (!_invulnerable)
        {
            remainingHP -= damage;
            if (remainingHP <= 0)
            {
                remainingHP = 0;
                Death();
            }
            else
            {
                StartCoroutine(InvulnerableCoroutine());
            }
            UIManager.instance.UpdateHealthPercentage((float)remainingHP / maxHP);
        }
    }

    private void Death()
    {
        OnDeath?.Invoke();
        Invoke(nameof(GameManager.instance.EndGame), 5f);
    }

    private IEnumerator InvulnerableCoroutine()
    {
        var anim = GetComponent<Animator>();
        _invulnerable = true;
        anim.SetBool("Invulnerable", true);

        yield return new WaitForSeconds(InvulnerabilityTime);

        anim.SetBool("Invulnerable", false);
        _invulnerable = false;
    }
}
