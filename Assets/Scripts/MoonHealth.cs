using UnityEngine;
using UnityEngine.Events;

public class MoonHealth : MonoBehaviour
{
    public int maxHP = 3;
    public int remainingHP = 1;
    public UnityEvent OnDeath;

    private void Awake()
    {
        remainingHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        remainingHP -= damage;
        if (remainingHP <= 0)
        {
            remainingHP = 0;
            Death();
        }
        //Update UI
        UIManager.instance.UpdateHealth(remainingHP);
    }

    private void Death()
    {
        OnDeath?.Invoke();
    }
}
