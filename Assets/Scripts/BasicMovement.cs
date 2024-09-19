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

    // The Rewired player id of this character
    public int playerId = 0;

    // The movement speed of this character
    public float moveSpeed = 3.0f;

    private Camera playerCamera;
    private Player cursor; // The Rewired Player
    private CharacterController cc;
    private Vector3 moveVector;
    private Vector3 cameraVector;
    private bool clicked;
    private bool zoomed;
    
    public float camZoomRangeMin = -10;
    public float camZoomRangeMax = -1;
    public float currZoom = 0;

    void Awake()
    {
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        cursor = ReInput.players.GetPlayer(playerId);

        playerCamera = Camera.main;
        // Get the character controller
        cc = GetComponent<CharacterController>();

        StartCoroutine(DragCameraUpdate());
    }

    void Update()
    {
        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {
        // Get the input from the Rewired Player. All controllers that the Player owns will contribute, so it doesn't matter
        // whether the input is coming from a joystick, the keyboard, mouse, or a custom controller.

        moveVector.x = cursor.GetAxis("Move Horizontal"); // get input by name or action id
        moveVector.y = cursor.GetAxis("Move Vertical");

        cameraVector.x = cursor.GetAxis("Pan Horizontal");
        cameraVector.y = cursor.GetAxis("Pan Vertical");

        clicked = cursor.GetButtonDown("Click");
        zoomed = cursor.GetButtonDown("Zoom");
    }

    private void ProcessInput()
    {
        // Process movement
        if (moveVector.x != 0.0f || moveVector.y != 0.0f)
        {
            cc.Move(moveVector * moveSpeed * Time.deltaTime);
        }

        // Process click
        if (clicked)
        {
            //Debug.Log("click");
        }
        if (zoomed)
        {
            if (currZoom >= camZoomRangeMax)
            {
                currZoom = camZoomRangeMin;
            }
            else if (currZoom < camZoomRangeMax)
            {
                currZoom += 0.5f;
            }

            playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, currZoom);
        }
    }

    Vector3 GetMousePositionInWorld()
    {
        Vector3 screenPosition = cameraVector;

        // If you're using a perspective camera for parallax, 
        // be sure to assign a depth to this point.
        screenPosition.z = currZoom;
        Debug.Log(currZoom);
       
        return playerCamera.ScreenToWorldPoint(screenPosition);
    }
    private IEnumerator DragCameraUpdate()
    {
        Vector3 initialMousePosition = GetMousePositionInWorld();

        while (zoomed == false)
        {
            Vector3 currentMousePosition = GetMousePositionInWorld();
            Vector3 travel = currentMousePosition - initialMousePosition;

            // Remove any vertical travel to lock the motion to the horizontal plane.
            travel.y = 0; 

            playerCamera.transform.Translate(-travel, Space.World);

            yield return null;
        }
    }
}

