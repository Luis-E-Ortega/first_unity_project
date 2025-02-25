using UnityEngine;

public class HealthController : MonoBehaviour
{
    private int currentHealth;
    public int CurrentHealth {get {return currentHealth; } }

    [SerializeField] private int maximumHealth;
    [SerializeField] private bool isPlayer; // To set as player character in inspector


    public void DealDamage(int damage)
    {
        if (currentHealth - damage <= 0)
        {
            OnDeath();
        }
        else
        {
            currentHealth -= damage;
        }
    }

    public void HealDamage(int healing)
    {
        if((currentHealth + healing) <= maximumHealth)
        {
            currentHealth += healing;
        }
        else
        {
            currentHealth = maximumHealth;
        }
    }

    private bool IsDead()
    {
        return currentHealth <= 0;
    }

    void Start()
    {
        Debug.Log($"Health Controller initialized with health: {currentHealth}");
    }
    void Awake()
    {
        currentHealth = maximumHealth;
        Debug.Log($"HealthController Start - currentHealth set to: {currentHealth}");
    }

    private void OnDeath()
    {
        if (isPlayer)
        {
            GameManager.Instance.ShowGameOver();
        }
        else
        {
            // Boss death logic
        }
    }
}
