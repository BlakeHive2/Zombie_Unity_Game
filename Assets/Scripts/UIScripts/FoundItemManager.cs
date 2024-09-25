using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class FoundItemManager : MonoBehaviour
{
    public Text itemName;
    private Player cursor; // The Rewired Player
    bool clicked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cursor = ReInput.players.GetPlayer(0);

    }
    void Update()
    {
        GetInput();
        ProcessInput();
    }
    private void GetInput()
    {
        clicked = cursor.GetButtonDown("Click");
    }
    private void ProcessInput()
    {
        if (clicked)
        {
            Debug.Log("FIGRE YES OR NO");
            gameObject.SetActive(false);
        }
    }

    public void SetItemName(string newName)
    {
        itemName.text = newName;
    }
}
