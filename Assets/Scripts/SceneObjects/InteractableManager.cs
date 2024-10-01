using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public enum InteractableType
{
    Basic_Type,
    Dialogue_Type,
    Door_Type,
    InventoryItem_Type,
    Note_Journal_Type,
    Puzzle_Type
} 
/// <summary>
/// Gameobject that can be placed by artist anywhere to set up a new clickable area to interact with.
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class InteractableManager : MonoBehaviour
{
    [Header("MANDATORY SETTINGS:")]
    public InteractableType objectType;
    public bool DoesMoveWithBackground = true;
    public float yOffsetLabel = 1;

    [Header("TOGGLED OBJECTS:")]
    public GameObject[] turnOffObjects;
    public GameObject[] turnOnObjects;

      

    [Header("DOOR OPTIONS:")]
    public GameObject SectionThisLeadsTo;

    [Header("SOUND OPTIONS:")]
    public AudioClip SoundClip;
    public bool DoLoopAudio = false;


    [Header("DIALOGUE OPTIONS:")]
    public DialogueText[] dialogue;

    //
    private AudioSource audioSrc; 
    private GameObject oldPrimary;
    private BasicMovement mvmtManager;

    private GameObject UIForFoundItem;
    private GameObject UIForFoundDocument;
    private GameObject UIForDialogue;
    private GameObject UIForPuzzle;
    private GameObject fromSeciton;

    


    void Awake()
    {
        fromSeciton = gameObject.transform.parent.transform.parent.gameObject;

        SetUpCollider();

        if (DoesMoveWithBackground)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 2.55f);
        }

        SetUpAudio();

        AssignTagAndLayer();

        

        SetUpToggleObjects();
    }

    void Start()
    {
        SetUpUIObjects();
    }

    void SetUpUIObjects()
    {
        UIForFoundItem = PrimaryGameUIManager.instance.FoundItemUI;
        UIForFoundDocument = PrimaryGameUIManager.instance.FoundDocumentUI;
        UIForDialogue = PrimaryGameUIManager.instance.DialogueUI;
    }
     
    void SetUpAudio()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.playOnAwake = false;
        audioSrc.loop = DoLoopAudio;
        audioSrc.outputAudioMixerGroup = Resources.Load<AudioMixerGroup>("GameAudioMixer");
    }

    void SetUpCollider()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        gameObject.GetComponent<CapsuleCollider>().radius = 1;
        gameObject.GetComponent<CapsuleCollider>().height = 10;
        gameObject.GetComponent<CapsuleCollider>().direction = 2;
    }
    void SetUpToggleObjects()
    {
        if (turnOffObjects != null)
        {
            for (int i = 0; i < turnOffObjects.Length; i++)
            {
                turnOffObjects[i].SetActive(true);
            }
        }
        if (turnOnObjects != null)
        {
            for (int i = 0; i < turnOnObjects.Length; i++)
            {
                turnOnObjects[i].SetActive(false);
            }
        }
        
        
    }
    void AssignTagAndLayer()
    {
        gameObject.tag = "Interactable";
        gameObject.layer = (int)ColliderType.kInteractable;
        
    } 

    //once clicked do the function
    public void _ClickedFunction(BasicMovement thisScript)
    {
        mvmtManager = thisScript;
        switch (objectType)
        {
            case InteractableType.Basic_Type:
                //no special UI, just might toggle G/O's, have a label, & make sounds
                break;
            case InteractableType.Dialogue_Type:
                mvmtManager.canClick = false;
                ShowDialoguePopUp();
                break;
            case InteractableType.Door_Type:
                mvmtManager.canClick = false;
                GoThroughDoor();
                break;
            case InteractableType.InventoryItem_Type:
                mvmtManager.canClick = false;
                ShowFoundItemUI();
                break;
            case InteractableType.Note_Journal_Type:
                mvmtManager.canClick = false;
                ShowDocumentUI();
                break;
            case InteractableType.Puzzle_Type:
                mvmtManager.canClick = false;
                ShowPuzzleUI();
                break;
            default:
                break;
        }

        //ALL 
        ToggleGameObjects();
        PlaySound();
    }

    void ToggleGameObjects()
    {
        for (int i = 0; i < turnOffObjects.Length; i ++)
        {
            turnOffObjects[i].SetActive(false);
        }
        for (int i = 0; i < turnOnObjects.Length; i++)
        {
            turnOnObjects[i].SetActive(true);
        }
    }

    public void _restoreClicks()
    {
        mvmtManager.canClick = true;
    }
    void ShowDialoguePopUp()
    {
        //set dialogue and options
        UIForDialogue.SetActive(true);
        UIForDialogue.GetComponent<DialogueManager>().SetDialogue(dialogue, this);
    }

    void ShowFoundItemUI()
    {
        //set name label
        UIForFoundItem.SetActive(true);
        UIForFoundItem.GetComponent<FoundItemManager>().SetItemName(gameObject.name, this);
    }
    void ShowDocumentUI()
    {
        UIForFoundDocument.SetActive(true);
        UIForFoundDocument.GetComponent<DocumentManager>().SetDialogue(dialogue, this);
    }

    void ShowPuzzleUI()
    {
        
    }

    void PlaySound()
    {
        if (SoundClip != null)
        {
            audioSrc.clip = SoundClip;
            audioSrc.Play();
        }
        
    }

    void GoThroughDoor()
    {
        mvmtManager.fadeManager.GetComponent<MusicManager>().FadeFromTo(fromSeciton, SectionThisLeadsTo);
        mvmtManager.SetNewCamera(SectionThisLeadsTo.transform.GetChild(0).GetComponent<Camera>());
    }

    public void _OnEnterHover()
    { 
    }
    public void _OnExitHover()
    { 
    }

    public void _ClickedAddToInventory()
    {
        gameObject.SetActive(false);
    }

}
