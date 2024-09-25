using UnityEngine;
using System.Collections;

public class PrimaryGameUIManager : MonoBehaviour
{
    private GameObject FoundItemUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        FoundItemUI = transform.GetComponentInChildren<FoundItemManager>().gameObject;
    }

    void Start()
    {
        StartCoroutine(turnOffUI());

    }

    IEnumerator turnOffUI()
    {
        yield return new WaitForSeconds(1);
        FoundItemUI.SetActive(false);
    }
}
