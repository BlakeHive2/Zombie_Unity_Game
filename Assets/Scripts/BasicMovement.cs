using UnityEngine;
using System.Collections;
using Rewired;

//Camera: Left X/Y - Pan Horizontal, Pan Vertical
//Cursor: Right X/Y - Move Horizontal, Move Vertical
//Zoom: Left Trigger
//Click: Right Trigger

[RequireComponent(typeof(CharacterController))]
public class BasicMovement : MonoBehaviour
{
    public GameObject foundItemPrompt;
    public GameObject visualCursorObj;
    // The Rewired player id of this character
    public int playerId = 0;

    // The movement speed of this character
    public float moveSpeed = 3.0f;

    private Camera playerCamera;
    private Player cursor; // The Rewired Player
    private CharacterController cc;
    private Vector3 moveVector;
    private Vector3 cameraVector;
    private Vector3 zoomVector;
    private bool clicked;
    
    [Header("Zooming Camera")]
    public float camZoomRangeMin = -14;
    public float camZoomRangeMax = -8;

    [Header("Panning Camera")]
    public float PanCamMin = 2;
    public float PanCamMax = 5;

    string interactableObj = "";
    float newZoom = -14;
    float panZoom = 0;
    void Awake()
    {
        newZoom = camZoomRangeMin;
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        cursor = ReInput.players.GetPlayer(playerId);

        playerCamera = Camera.main;
        // Get the character controller
        cc = GetComponent<CharacterController>();
        cc.detectCollisions = true;
        
    }

    void Update()
    {
        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {

        moveVector.x = cursor.GetAxis("Move Horizontal");
        moveVector.y = cursor.GetAxis("Move Vertical");

        cameraVector.x = cursor.GetAxis("Pan Horizontal");
        cameraVector.y = cursor.GetAxis("Pan Vertical");

        clicked = cursor.GetButtonDown("Click");
        zoomVector.x = cursor.GetAxis("Zoom");
    }

    private void ProcessInput()
    {
        // Process movement
        if (moveVector.x != 0.0f || moveVector.y != 0.0f)
        {
            cc.Move(moveVector * moveSpeed * Time.deltaTime);
            //visualCursorObj.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        panZoom = zoomVector.x * 7;
        if (panZoom <= 0.1)
        {
            panZoom = PanCamMin;
        }
        else if (panZoom > 6)
        {
            panZoom = PanCamMax;
        }

        //Debug.Log(panZoom);

        if (cameraVector.x != 0.0f || cameraVector.y != 0.0f)
        {           
            playerCamera.transform.position = new Vector3(cameraVector.x * panZoom, cameraVector.y * panZoom, newZoom);
        }
        
        if (clicked)
        { 
            if (interactableObj.Length > 2)
            {
                foundItemPrompt.SetActive(true);
                foundItemPrompt.GetComponent<FoundItemManager>().SetItemName(interactableObj);
            }
        }

        if (zoomVector.x != 0)
        {
            newZoom = (15 + (-8 * zoomVector.x)) * -1;
            if (newZoom > camZoomRangeMax)
            {
                newZoom = camZoomRangeMax;
            }
        }
        else
        {
            newZoom = camZoomRangeMin;
        }
        playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, newZoom);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        interactableObj = other.gameObject.name;

        Debug.Log(interactableObj);
    }
    private void OnTriggerExit(Collider other)
    {
        interactableObj = "";
    }

    Vector3 GetCamPositionInWorld()
    {
        Vector3 screenPosition = cameraVector;

        // If you're using a perspective camera for parallax, 
        // be sure to assign a depth to this point.
        // screenPosition.z = currZoom;
       
        return playerCamera.ScreenToWorldPoint(screenPosition);
    }
     
}

