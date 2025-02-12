using System.ComponentModel;
using UnityEngine;

public class ElementSpawner : MonoBehaviour
{
    // For setting which object to generate in inspector
    [SerializeField] private GameObject elementPrefab;
    // For setting how many objects to spawn 
    [SerializeField] public int numberOfOrbs;
    
    void Start()
    {
        // Set min/max x and y values
        float minValueX = 20f;
        float maxValueX = 35f;
        float minValueY = 3f;
        float maxValueY = 15f;

        for (int i = 0; i < numberOfOrbs; i++)
        {
            bool validPositionFound = false;
            while (!validPositionFound)
            {
                // Use min/max values for random values within range
                float randomX = Random.Range(minValueX, maxValueX);
                float randomY = Random.Range(minValueY, maxValueY);
                Vector3 position = new(randomX, randomY, 0);

                // Randomize value for type
                int randomType = Random.Range(1, 5);

                // Checks to see if space is already taken
                if (Physics2D.OverlapCircle(position, 0.6f) == null)
                {
                    validPositionFound = true;

                    // Spawn elements randomly on map
                    GameObject newElement = Instantiate(elementPrefab, position, Quaternion.identity);
                    ElementPickUp elementComponent = newElement.GetComponent<ElementPickUp>();
                    SpriteRenderer spriteRenderer = newElement.GetComponent<SpriteRenderer>();
                    switch (randomType)
                    {
                        case 1: // Water type
                            elementComponent.elementType = ElementType.Water;
                            spriteRenderer.color = Color.blue;
                            break;
                        case 2: // Fire type
                            elementComponent.elementType = ElementType.Fire;
                            spriteRenderer.color = Color.red;
                            break;
                        case 3: // Earth type
                            elementComponent.elementType = ElementType.Earth;
                            spriteRenderer.color = Color.green;
                            break;
                        case 4: // Wind type
                            elementComponent.elementType = ElementType.Wind;
                            spriteRenderer.color = Color.gray;
                            break;
                    }
                }
            }
        }

    }
    void Update()
    {
        
    }
}
