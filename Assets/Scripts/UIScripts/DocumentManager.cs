using UnityEngine;
using UnityEngine.UI;
using System;
using Rewired;


public class DocumentManager : MonoBehaviour
{
    public DialogueText[] dialogue;
    public Text primaryText;

    private Player cursor;
    private bool clicked;
    int dialogIndex = 0;
    int subDialogIndex = 0;
    private InteractableManager interactable;
    //clicing a/r mouse btn moves to next string or turns off if last string
    //disable all other interactions until completed

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
            //first we go thru 0 and tab thru subs as long as we are under/= to sub
            if (subDialogIndex < dialogue[dialogIndex].dialogue.Length - 1)
            {
                subDialogIndex++;
            }
            else //next we are out of sub strings in the first round, so move up or close
            {
                subDialogIndex = 0;
                dialogIndex++;
                if (dialogIndex >= dialogue.Length)
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
            primaryText.text = dialogue[dialogIndex].dialogue[subDialogIndex];
        }
    }

    public void SetDialogue(DialogueText[] newDialogue, InteractableManager thisInteractable)
    {
        dialogue = newDialogue;
        primaryText.text = dialogue[0].dialogue[0];
        interactable = thisInteractable;
        subDialogIndex = 0;
        dialogIndex = 0;
    }

    void OnEnable()
    {

    }
    void OnDisable()
    {
        interactable._restoreClicks();
    }

}
