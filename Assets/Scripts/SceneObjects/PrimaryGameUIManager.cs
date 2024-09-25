using UnityEngine;
using System.Collections;

public class PrimaryGameUIManager : MonoBehaviour
{
    public GameObject FoundItemUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
       // FoundItemUI = transform.GetComponentInChildren<FoundItemManager>().gameObject;
    }

    void Start()
    {
       // FoundItemUI.SetActive(false);
    }
}
