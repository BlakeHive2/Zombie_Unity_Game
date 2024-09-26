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
    }

    void Start()
    {
    }
}
