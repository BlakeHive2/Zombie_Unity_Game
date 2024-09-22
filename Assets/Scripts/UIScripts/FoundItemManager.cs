using UnityEngine;
using UnityEngine.UI;

public class FoundItemManager : MonoBehaviour
{
    public Text itemName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    public void SetItemName(string newName)
    {
        itemName.text = newName;
    }
}
