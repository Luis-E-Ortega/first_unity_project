using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class ElementSpawner : MonoBehaviour
{
    // For setting which object to generate in inspector
    [SerializeField] private GameObject elementPrefab;
    public BossBehavior bossBehavior; // Reference for boss spawning

    // For setting how many objects to spawn 
    [SerializeField] public int initialOrbCount;
    [SerializeField] private ElementData elementData;
    private Dictionary<ElementType, ElementData.ElementProperties> elementProperties;

    public static int activeOrbs = 0;

    private void Awake()
    {
        elementProperties = elementData.GetElementProperties();
    }

    void Start()
    {
        SpawnElements();
    }
    void Update()
    {
        
    }

    private void SpawnElements()
    {
        activeOrbs = initialOrbCount;

        for (int i = 0; i < initialOrbCount; i++)
        {
            Vector3 position = GetValidRandomPosition();
            SpawnElementAtPosition(position);
        }
    }

    private Vector3 GetValidRandomPosition()
    {   
        // Set min/max x and y values
        float minValueX = 20f;
        float maxValueX = 35f;
        float minValueY = 3f;
        float maxValueY = 15f;

        while (true)
        {
            float randomX = Random.Range(minValueX, maxValueX);
            float randomY = Random.Range(minValueY, maxValueY);
            Vector3 position = new(randomX, randomY, 0);

            if (Physics2D.OverlapCircle(position, 0.6f) == null)
            {
                return position;
            }
        }
    }

    private void SpawnElementAtPosition(Vector3 position)
    {
         // Spawn elements randomly on map
        GameObject newElement = Instantiate(elementPrefab, position, Quaternion.identity);
        ElementPickUp elementComponent = newElement.GetComponent<ElementPickUp>();
        SpriteRenderer spriteRenderer = newElement.GetComponent<SpriteRenderer>();

        // Reference for spawning boss
        elementComponent.bossBehavior = bossBehavior;


        // Get random element
        ElementType randomType = GetRandomElement();

        // Apply properties from ElementData
        elementComponent.elementType = randomType;
        spriteRenderer.color = elementProperties[randomType].Color;
    }
    public ElementType GetRandomElement()
    {
        // Get all the possible element types as an array
        ElementType[] elements = (ElementType[])System.Enum.GetValues(typeof(ElementType));

        return elements[Random.Range(0, elements.Length)];
    }
}
