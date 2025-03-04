using System.Collections;
using Cainos.PixelArtTopDown_Basic;
using UnityEngine.UI;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private int currentHealth;
    public int CurrentHealth {get {return currentHealth; } }

    private ParticleSystem playerImmunityAura;
    private float phaseIntervalTime = 2f;
    private bool _isImmune; // For future possible implementation of immunity mechanics

    [SerializeField] private int maximumHealth;
    [SerializeField] private bool isPlayer; // To set as player character in inspector
    [SerializeField] private bool isBoss; // To set as boss in inspector
    [SerializeField] Slider playerHealthSlider;
    [SerializeField] Slider bossHealthSlider;
    public void DealDamage(int damage)
    {
        if (isImmune) return;
        
        if (currentHealth - damage <= 0)
        {
            if(isPlayer && playerHealthSlider != null)
                playerHealthSlider.value = 0;
            else if(isBoss && bossHealthSlider != null)
                bossHealthSlider.value = 0;
            OnDeath();
        }
        else
        {
            currentHealth -= damage;
            if (isPlayer && playerHealthSlider != null)
            {
                playerHealthSlider.value = currentHealth;
                Debug.Log($"Current player health is: {currentHealth}");
            }
            if (isBoss && bossHealthSlider != null)
            {
                bossHealthSlider.value = currentHealth;
                Debug.Log($"Current boss health is: {currentHealth}");
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

        if (isPlayer && playerHealthSlider != null)
        {
            playerImmunityAura = transform.Find("PlayerAura").GetComponent<ParticleSystem>();
            if (playerHealthSlider != null)
            {
                playerHealthSlider.maxValue = maximumHealth;
                playerHealthSlider.value = currentHealth;
            }
            
        }
        else if (isBoss && bossHealthSlider != null)
        {
            bossHealthSlider.gameObject.SetActive(false); // Start boss health bar hidden
            bossHealthSlider.maxValue = maximumHealth;
            bossHealthSlider.value = currentHealth;
        }
    }
    // Method to show boss health bar
    public void ShowBossUI()
    {
        if (isBoss && bossHealthSlider != null)
        {
            bossHealthSlider.gameObject.SetActive(true);
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
        if (isBoss)
        {
            GameManager.Instance.ShowGameWin();
        }
    }
}