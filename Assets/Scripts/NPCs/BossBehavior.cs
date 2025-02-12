using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public void SpawnBoss()
    {
        gameObject.SetActive(true);
    }
}
