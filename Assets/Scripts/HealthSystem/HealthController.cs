using System.Collections;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private int currentHealth;
    public int CurrentHealth {get {return currentHealth; } }

    private ParticleSystem playerImmunityAura;
    private float phaseIntervalTime = 2f;
    private bool _isImmune;

    [SerializeField] private int maximumHealth;
    [SerializeField] private bool isPlayer; // To set as player character in inspector


    public void DealDamage(int damage)
    {
        if (isImmune) return;
        
        if (currentHealth - damage <= 0)
        {
            OnDeath();
        }
        else
        {
            currentHealth -= damage;
            if (isPlayer)
            {
                Debug.Log($"Current player health is: {currentHealth}");
            }
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
    public bool isImmune
    {
        get { return _isImmune; }
        set { _isImmune = value; }
    }
    void Awake()
    {
        currentHealth = maximumHealth;
        Debug.Log($"HealthController Start - currentHealth set to: {currentHealth}");
        if (isPlayer)
        {
            playerImmunityAura = transform.Find("PlayerAura").GetComponent<ParticleSystem>();
        }
    }

    private IEnumerator ImmunityEffect()
    {
        if (playerImmunityAura == null) yield break;

        playerImmunityAura.Clear();
        playerImmunityAura.Play();
        isImmune = true;
        yield return new WaitForSeconds(phaseIntervalTime);
        isImmune = false;
        playerImmunityAura.Clear();
        playerImmunityAura.Stop();
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