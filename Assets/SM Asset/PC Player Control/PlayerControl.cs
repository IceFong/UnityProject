using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : NetworkBehaviour
{
    
    [SerializeField] private InputActionReference jumpButton;
    [SerializeField] private InputActionReference walkRef;
    // [SerializeField] private InputActionReference jumpButton;
    // [SerializeField] private InputActionReference jumpButton;
    // [SerializeField] private InputActionReference jumpButton;
    [SerializeField] private InputActionAsset actionAsset;

    [SerializeField] private Rigidbody rigidbody;
    public GameObject Camera;
    // public Transform ikHead;
    // public Transform avatar;

    [SerializeField] private float PlayerSpeed = 2f;
    [SerializeField] private float JumpStrength = 200f;

    public const int MAX_JUMP = 2;
    private int jumpCount = 0;


    private void OnEnable() {
        
        if (actionAsset != null) {
            actionAsset.Enable();
        }

    }

    public override void FixedUpdateNetwork()
    {
        //Only move own player and not other player
        if (HasStateAuthority == false) return;

        if (jumpButton.action.triggered && jumpCount > 0) {
            print("jump");
            rigidbody.AddForce( Vector3.up * JumpStrength );
            jumpCount--;
        }

        //camera forward and right vectors:
        var forward = Camera.transform.forward;
        var right = Camera.transform.right;
        //reading the input:
        Vector2 moveAmount = walkRef.action.ReadValue<Vector2>();
        float horizontalAxis = moveAmount.x;
        float verticalAxis = moveAmount.y;
        //project forward and right vectors on the horizontal plane (y = 0)
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        //this is the direction in the world space we want to move:
        var desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;
        
        //now we can apply the movement:
        transform.Translate(PlayerSpeed * Runner.DeltaTime * desiredMoveDirection);
        // transform.rotation = Camera.transform.rotation;
        // avatar.SetPositionAndRotation(Camera.transform.position, Camera.transform.rotation);
        // ikHead.SetPositionAndRotation(Camera.transform.position, Camera.transform.rotation);
    
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            // Vector3 camPos = transform.position;
            // camPos.y += 0.5f;

            // GameObject newCam = Instantiate(Camera, camPos, transform.rotation);
            // newCam.transform.parent = gameObject.transform.GetChild(0).GetChild(0);
            Camera.GetComponent<FirstPersonCamera>().Target = GetComponent<NetworkRigidbody>().InterpolationTarget;
            // Camera = newCam;
            // print("spawned");
            // GetComponentInChildren<FirstPersonCamera>().Target = GetComponent<NetworkTransform>().InterpolationTarget;        
            // Camera = gameObject.AddComponent<Camera>();
            // Camera.AddComponent<FirstPersonCamera>().Target = GetComponent<NetworkTransform>().InterpolationTarget;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Jumpable")) {
            jumpCount = MAX_JUMP;
        }
    }

    // void Update() {

    // }

}
