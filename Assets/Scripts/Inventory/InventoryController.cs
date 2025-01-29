using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public bool inv_showing = false;

    private void Start()
    {
        Debug.Log("Start method called");  
    }
    private void Update()
    {
        Debug.Log("Update is running");  
        OpenInventory();
    }
    public void OpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("I key was pressed");
            Debug.Log("Panel active state: " + inventoryPanel.gameObject.activeSelf);
            inv_showing = !inv_showing;
            inventoryPanel.SetActive(inv_showing);
        }
    }

}
