using UnityEngine;

public class HealthController : MonoBehaviour
{
    private int maximumHealth;
    private int currentHealth;

    private void DealDamage(int damage)
    {

    }

    private void HealDamage(int healing)
    {

    }

    private bool IsDead()
    {
        if (currentHealth == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
