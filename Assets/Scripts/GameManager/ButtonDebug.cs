using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonDebug : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    void Start()
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        RectTransform rt = GetComponent<RectTransform>();
        
        Debug.Log("Canvas Render Mode: " + canvas.renderMode);
        Debug.Log("Canvas World Camera: " + canvas.worldCamera);
        Debug.Log("Button Rect World Corners:");
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("Corner " + i + ": " + corners[i]);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("POINTER ENTERED BUTTON");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("POINTER EXITED BUTTON");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("BUTTON CLICKED");
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Debug.Log("Raw Mouse Position: " + mousePos);
            
            // Try to convert to canvas space
            RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            Vector2 pointInCanvas;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, 
                mousePos, 
                null, // null for overlay, your camera for screen space camera
                out pointInCanvas);
                
            Debug.Log("Point in Canvas Space: " + pointInCanvas);
        }
    }
}