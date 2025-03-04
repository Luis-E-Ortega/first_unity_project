using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using Cainos.PixelArtTopDown_Basic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossBehavior : MonoBehaviour
{
    private ParticleSystem aura;
    private ParticleSystem aoe;
    private ParticleSystemRenderer auraRenderer;
    private ParticleSystemRenderer aoeRenderer;

    private AoECollisionHandler aoeCollision;

    public ElementType elementType;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private HealthController bossHealth;
    [SerializeField] private HealthController playerHealth;
    [SerializeField] private ElementSpawner elementSpawner;
    [SerializeField] private Tilemap grassTilemap;

    [SerializeField] private TopDownCharacterController player;

    [SerializeField] private int fullDamage = 50;
    [SerializeField] private int partialDamage = 10;
  

    void Awake()
    {
        bossHealth = GetComponent<HealthController>();
        Debug.Log("BossBehavior Awake - Got HealthController reference");
        spriteRenderer = GetComponent<SpriteRenderer>();
        aura = transform.Find("BossAura").GetComponent<ParticleSystem>();
        aoe = transform.Find("AoEAttack").GetComponent<ParticleSystem>();
        auraRenderer = aura.GetComponent<ParticleSystemRenderer>();
        aoeRenderer = aoe.GetComponent<ParticleSystemRenderer>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Debug.Log($"BossBehavior OnEnable - Boss Health: {bossHealth.CurrentHealth}");
        StartCoroutine(SwitchElementRoutine());
        Debug.Log("Element routine started successfully");
    }

    public IEnumerator SpawnBoss()
    {
        Debug.Log("SpawnBoss coroutine BEGINNING");
        gameObject.SetActive(true);
        aoe.Stop();
        //Debug.Log($"Initial Boss Health: {bossHealth.CurrentHealth}"); // Should now show 100
        Debug.Log("Boss GameObject activated");
        //StartCoroutine(SwitchElementRoutine());
        //Debug.Log("Element routine started successfully");
        yield break;
    }

    IEnumerator SwitchElementRoutine()
    {
        Debug.Log("SwitchElementRoutine STARTING");
        if (aura == null)
        {
            Debug.LogError("Aura ParticleSystem is null!");
            yield break;
        }

        if (auraRenderer == null)
        {
            Debug.LogError("ParticleSystemRenderer not found!");
            yield break;
        }

        if (aoeRenderer == null)
        {
            Debug.LogError("ParticleSystemRenderer for AoE attack not found!");
            yield break;
        }

        float minTime = 1f;
        float maxTime = 5f;
        float phaseIntervalTime = 2f;
        Debug.Log($"Boss Health: {bossHealth.CurrentHealth}");

        while(bossHealth.CurrentHealth > 0)
        {
            ElementType randElement = elementSpawner.GetRandomElement();
            switch(randElement)
            {
                case ElementType.Fire:
                    auraRenderer.material.color = Color.red;
                    grassTilemap.color = Color.white;
                    yield return new WaitForSeconds(phaseIntervalTime); // 2 second telegraph
                    elementType = ElementType.Fire; // Switch element type
                    spriteRenderer.color = Color.red; // Match color to type
                    aoeRenderer.material.color = Color.red;
                    aoe.Clear();
                    aoe.Play();
                    grassTilemap.color = Color.red;
                    if (player.elementType != ElementType.Fire)
                    {
                        Debug.Log("Dealing full damage!");
                        playerHealth.DealDamage(fullDamage);
                    }
                    else
                    {
                        Debug.Log("Dealing only partial damage!");
                        playerHealth.DealDamage(partialDamage);
                    }
                    break;
                case ElementType.Water:
                    auraRenderer.material.color = Color.blue;
                    grassTilemap.color = Color.white;
                    yield return new WaitForSeconds(phaseIntervalTime);
                    elementType = ElementType.Water;
                    spriteRenderer.color = Color.blue;
                    aoeRenderer.material.color = Color.blue;
                    aoe.Clear();
                    aoe.Play();
                    grassTilemap.color = Color.blue;
                    if (player.elementType != ElementType.Water)
                    {
                        Debug.Log("Dealing full damage!");
                        playerHealth.DealDamage(fullDamage);
                    }
                    else
                    {
                        Debug.Log("Dealing only partial damage!");
                        playerHealth.DealDamage(partialDamage);
                    }
                    break;
                case ElementType.Earth:
                    auraRenderer.material.color = Color.green;
                    grassTilemap.color = Color.white;
                    yield return new WaitForSeconds(phaseIntervalTime);
                    elementType = ElementType.Earth;
                    spriteRenderer.color = Color.green;
                    aoeRenderer.material.color = Color.green;
                    aoe.Clear();
                    aoe.Play();
                    grassTilemap.color = Color.green;
                    if (player.elementType != ElementType.Earth)
                    {
                        Debug.Log("Dealing full damage!");
                        playerHealth.DealDamage(fullDamage);
                    }
                    else
                    {
                        Debug.Log("Dealing only partial damage!");
                        playerHealth.DealDamage(partialDamage);
                    }
                    break;
                case ElementType.Wind:
                    auraRenderer.material.color = Color.white;
                    grassTilemap.color = Color.white;
                    yield return new WaitForSeconds(phaseIntervalTime);
                    elementType = ElementType.Wind;
                    spriteRenderer.color = Color.white;
                    aoeRenderer.material.color = Color.white;
                    aoe.Clear();
                    aoe.Play();
                    grassTilemap.color = Color.grey;
                    if (player.elementType != ElementType.Wind)
                    {
                        Debug.Log("Dealing full damage!");
                        playerHealth.DealDamage(fullDamage);
                    }
                    else
                    {
                        Debug.Log("Dealing only partial damage!");
                        playerHealth.DealDamage(partialDamage);
                    }
                    break;
            }
            yield return new WaitForSeconds(Random.Range(minTime, maxTime)); // Random interval between swaps
        }
    }
}
