using Cainos.PixelArtTopDown_Basic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AoECollisionHandler : MonoBehaviour
{
    private BossBehavior boss;
    private HealthController playerHealth;
    private TopDownCharacterController player;
    [SerializeField] private int damageFull = 100;
    [SerializeField] private int damagePartial = 20;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TopDownCharacterController>();
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossBehavior>();

        if (player != null)
        {
            playerHealth = player.GetComponent<HealthController>();
        }
        
        if (player == null || boss == null || playerHealth == null)
        {
            Debug.LogError("Missing required references in AoECollisionHandler");
        }
    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            if (player.elementType == boss.elementType)
            {
                Debug.Log("Detected player collision with reduced damage!");
                playerHealth.DealDamage(damagePartial);
                Debug.Log($"Current player health is: {playerHealth.CurrentHealth}");
            }
            else
            {
                Debug.Log("Detected FULL DAMAGE");
                playerHealth.DealDamage(damageFull);
                Debug.Log($"Current player hp after full damage: {playerHealth.CurrentHealth}");
            }
        }
    }

}

