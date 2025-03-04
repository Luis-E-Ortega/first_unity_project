using UnityEngine;

public class SpellProjectileManager : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    private ParticleSystem ps;
    private ParticleCollisionEvent[] collisionEvents;

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        if (ps != null)
        {
            collisionEvents = new ParticleCollisionEvent[16];
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Collision detected with: " + other.name);

        if (other.CompareTag("Boss"))
        {
            HealthController bossHealth = other.GetComponent<HealthController>();
            if(bossHealth != null)
            {
                bossHealth.DealDamage(damage);
            }
        }
    }
}
