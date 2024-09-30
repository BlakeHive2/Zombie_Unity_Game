using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class FoundItemManager : MonoBehaviour
{
    public Text itemName;

    private InteractableManager interactable;
    private Player cursor; // The Rewired Player
    private bool clicked;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cursor = ReInput.players.GetPlayer(0);

    }
    void OnEnable()
    {

    }
    void OnDisable()
    {
        interactable._restoreClicks();
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

    public void SetItemName(string newName, InteractableManager newInteractable)
    {
        itemName.text = newName;
        interactable = newInteractable;
    }
}
