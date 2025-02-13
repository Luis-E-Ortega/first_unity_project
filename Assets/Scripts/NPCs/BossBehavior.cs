using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    private ParticleSystem aura;
    void Start()
    {
        gameObject.SetActive(false);
        aura = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        
    }

    public void SpawnBoss()
    {
        gameObject.SetActive(true);
    }
}
