using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ElementData", menuName = "Game/Element Data")]
public class ElementData : ScriptableObject
{
    // References for images
    public Sprite waterSprite;
    public Sprite fireSprite;
    public Sprite earthSprite;
    public Sprite windSprite;

    // References for sounds
    public AudioClip waterSound;
    public AudioClip fireSound;
    public AudioClip earthSound;
    public AudioClip windSound;

    

    [System.Serializable]
    public struct ElementProperties
    {
        public Color Color; // Color for element
        public Sprite Sprite; // Image for element in inventory
        public float Power; // Power value for combat
        public AudioClip SpellSound; // Sound effect for spells
    }

    public Dictionary<ElementType, ElementProperties> GetElementProperties()
    {
        return new Dictionary<ElementType, ElementProperties>()
        {
            {
                ElementType.Water,
                new ElementProperties
                {
                    Color = Color.blue,
                    Sprite = waterSprite,
                    Power = 1,
                    SpellSound = waterSound // Reference to water sound effect
                }
            },
            {
                ElementType.Fire,
                new ElementProperties
                {
                    Color = Color.red,
                    Sprite = fireSprite,
                    Power = 1
                }
            },
            {
                ElementType.Earth,
                new ElementProperties
                {
                    Color = Color.green,
                    Sprite = earthSprite,
                    Power = 1
                }
            },
            {
                ElementType.Wind,
                new ElementProperties
                {
                    Color = Color.grey,
                    Sprite = windSprite,
                    Power = 1
                }
            }
        };
    }
    
}
