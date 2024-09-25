using UnityEngine;
using System.Collections;
using Rewired;

public class PrimaryGameUIManager : MonoBehaviour
{

    public static PrimaryGameUIManager instance;
    [Header("MOUSE/CONTROL SETTINGS:")]
    public bool useMouse = false;

    [Header("Mandatory UI Connections:")]
    public GameObject FoundItemUI;
    public GameObject DialogueUI;
    public GameObject FoundDocumentUI;

    private void SingletonFunction()
    {
        if (PrimaryGameUIManager.instance == null)
        {
            PrimaryGameUIManager.instance = this;

            DontDestroyOnLoad(transform.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Awake()
    {
        SingletonFunction();

        if (useMouse)
        {
            PlayerMouse mouse = PlayerMouse.Factory.Create();

            // Set the owner
            mouse.playerId = 0;

            // Set up Actions for each axis and button
            mouse.xAxis.actionName = "Move Horizontal";
            mouse.yAxis.actionName = "Move Vertical";
            mouse.wheel.yAxis.actionName = "Zoom";
            mouse.leftButton.actionName = "Click";
            //mouse.rightButton.actionName = rightButtonAction;
            //mouse.middleButton.actionName = middleButtonAction;
        }
    }

    void Start()
    {
    }
}
