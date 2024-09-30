using UnityEngine;
using System.Collections;
using Rewired;

//Camera: Left X/Y - Pan Horizontal, Pan Vertical
//Cursor: Right X/Y - Move Horizontal, Move Vertical
//Zoom: Left Trigger
//Click: Right Trigger
public enum ColliderType
{
    kInteractable = 6,
    kUIElement = 7,
    kPickup = 8,
    kDoor = 9

}

[RequireComponent(typeof(CharacterController))]
public class BasicMovement : MonoBehaviour
{
    public GameObject[] allSections;
    public GameObject fadeManager;
    public GameObject foundItemPrompt;
    // The Rewired player id of this character
    int playerId = 0;

    // The movement speed of this character
    public float moveSpeed = 10.0f;

    private Camera playerCamera;

    private Player cursor; // The Rewired Player
    [System.NonSerialized]
    private PlayerMouse mouse;
    public float distanceFromCam = 15f;

    private CharacterController cc;
    private Vector3 moveVector;
    private Vector3 cameraVector;
    private Vector3 zoomVector;
    private bool clicked;
    /// <summary>
    /// Set to false when another UI element is up, like Dialogue
    /// </summary>
    [HideInInspector()]
    public bool canClick = true;

    [Header("Zooming Camera")]
    public float camZoomRangeMin = -15;
    public float camZoomRangeMax = -8;

    [Header("Panning Camera")]
    public float PanCamMin = 2;
    public float PanCamMax = 5;

    string interactableObjName = "";
    GameObject interactableObj;
    string interactableUI = "";

    float newZoom = -14;
    float panZoom = 0;

    void Awake()
    {
        newZoom = camZoomRangeMin;
       
        // Get the Rewired Player object for this player and keep it for the duration of the character's lifetime
        cursor = ReInput.players.GetPlayer(playerId);

        mouse = PlayerMouse.Factory.Create();
        mouse.playerId = playerId;
        
        // If you want to change joystick pointer speed
        mouse.xAxis.actionName = "Move Horizontal";
        mouse.yAxis.actionName = "Move Vertical";
        mouse.wheel.yAxis.actionName = "Zoom";
        mouse.leftButton.actionName = "Click";
        mouse.screenPosition = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        mouse.ScreenPositionChangedEvent += OnScreenPositionChanged;

        // Get the initial position
        OnScreenPositionChanged(mouse.screenPosition);
        //
        //mouse.pointerSpeed = 10f;

        playerCamera = Camera.main;
        // Get the character controller
        cc = GetComponent<CharacterController>();
        cc.detectCollisions = true;
    }

    void Start()
    {
        if (PrimaryGameUIManager.instance.usingMouse)
        {
            cursor.controllers.hasMouse = true; 
        }
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent += OnControllerPreDisconnect;
    }
    // This function will be called when a controller is connected
    // You can get information about the controller that was connected via the args parameter
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        if (args.controllerType == Rewired.ControllerType.Joystick)
        {
            PrimaryGameUIManager.instance.usingMouse = false;
        }
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }

    // This function will be called when a controller is fully disconnected
    // You can get information about the controller that was disconnected via the args parameter
    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {

        PrimaryGameUIManager.instance.usingMouse = true;
        mouse.screenPosition = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        //mouse.screenPosition = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }

    // This function will be called when a controller is about to be disconnected
    // You can get information about the controller that is being disconnected via the args parameter
    // You can use this event to save the controller's maps before it's disconnected
    void OnControllerPreDisconnect(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller is being disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);
    }
    void OnScreenPositionChanged(Vector2 position)
    {

        // Convert from screen space to world space
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, distanceFromCam));

        // Move the pointer object
        transform.position = worldPos;
    }
    void OnDestroy()
    {
        mouse.ScreenPositionChangedEvent -= OnScreenPositionChanged;
        // Unsubscribe from events
        ReInput.ControllerConnectedEvent -= OnControllerConnected;
        ReInput.ControllerDisconnectedEvent -= OnControllerDisconnected;
        ReInput.ControllerPreDisconnectEvent -= OnControllerPreDisconnect;
    }
    void Update()
    {
        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {

        cursor.controllers.hasMouse = PrimaryGameUIManager.instance.usingMouse;
        if (cursor.controllers.GetLastActiveController() != null)
        {
            if (PrimaryGameUIManager.instance.usingMouse == true && cursor.controllers.GetLastActiveController().type == Rewired.ControllerType.Joystick)
            {
                PrimaryGameUIManager.instance.usingMouse = false;
            }
        }

        if (PrimaryGameUIManager.instance.usingMouse == false)
        {
            moveVector.x = cursor.GetAxis("Move Horizontal");
            moveVector.y = cursor.GetAxis("Move Vertical");
            cameraVector.x = cursor.GetAxis("Pan Horizontal");
            cameraVector.y = cursor.GetAxis("Pan Vertical");           
        }
        zoomVector.x = cursor.GetAxis("Zoom");
       // Debug.Log(zoomVector);

        clicked = cursor.GetButtonDown("Click");


    }

    private void ProcessInput()
    {
        // Process movement
        if (moveVector.x != 0.0f || moveVector.y != 0.0f)
        {
            cc.Move(moveVector * moveSpeed * Time.deltaTime);
        }
        //Panning
        if (PrimaryGameUIManager.instance.usingMouse == false)
        {
            panZoom = zoomVector.x * 7;
            if (panZoom <= 0.1)
            {
                panZoom = PanCamMin;
            }
            else if (panZoom > 6)
            {
                panZoom = PanCamMax;
            }

            if (cameraVector.x != 0.0f || cameraVector.y != 0.0f)
            {
                playerCamera.transform.position = new Vector3(cameraVector.x * panZoom, cameraVector.y * panZoom, newZoom);
            }
        }
        //using Mouse
        else
        {
            if (newZoom != camZoomRangeMax)
            {
                //check mouse
            }
        }

        if (canClick)
        {
            //clicking
            if (clicked)
            {
                if (interactableObj != null)
                {
                    interactableObj.GetComponent<InteractableManager>()._ClickedFunction(this);
                }

            }
        }
        

        //ZOOMING
        if (PrimaryGameUIManager.instance.usingMouse == false)
        {
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
        }
        else //use MOUSE
        {
            //zoom in
            if (zoomVector.x > 0)
            {
                newZoom++;
                if (newZoom > camZoomRangeMax)
                {
                    newZoom = camZoomRangeMax;
                }
            }
            //zoom out
            else if (zoomVector.x < 0)
            {
                newZoom--;
                if (newZoom < camZoomRangeMin)
                {
                    newZoom = camZoomRangeMin;
                }
            }
            else
            {
                //do nothing
            }
        }
        distanceFromCam = newZoom * -1;
        playerCamera.transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, newZoom);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //   Debug.Log(other.gameObject.layer);
        if (canClick)
        {
            if (other.gameObject.layer == (int)ColliderType.kInteractable)
            {
                other.gameObject.GetComponent<InteractableManager>()._OnEnterHover();
                interactableObj = other.gameObject;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == (int)ColliderType.kInteractable)
        {
            other.gameObject.GetComponent<InteractableManager>()._OnExitHover();
        }

        if (other.gameObject.layer == (int)ColliderType.kUIElement)
        {
            interactableUI = null;
        }
    }

    public void SetNewCamera(Camera newCamera)
    {
        playerCamera = newCamera;
    }
}

