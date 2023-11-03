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
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    public Transform orientation;

    private void OnEnable() {
        
        if (actionAsset != null) {
            actionAsset.Enable();
        }

    }

    void Update() {
        MyInput();
        SpeedControl();
    }

    public override void FixedUpdateNetwork()
    {
        //Only move own player and not other player
        if (HasStateAuthority == false) return;

        if (jumpButton.action.triggered && jumpCount > 0) {
            rigidbody.AddForce( Vector3.up * JumpStrength );
            jumpCount--;
        }

        MovePlayer();

        // //camera forward and right vectors:
        // var forward = Camera.transform.forward;
        // var right = Camera.transform.right;
        // //reading the input:
        // Vector2 moveAmount = walkRef.action.ReadValue<Vector2>();
        // float horizontalAxis = moveAmount.x;
        // float verticalAxis = moveAmount.y;
        // //project forward and right vectors on the horizontal plane (y = 0)
        // forward.y = 0f;
        // right.y = 0f;
        // forward.Normalize();
        // right.Normalize();
        // //this is the direction in the world space we want to move:
        // var desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;
        
        // //now we can apply the movement:
        // transform.Translate(PlayerSpeed * Runner.DeltaTime * desiredMoveDirection);
        // // transform.rotation = Camera.transform.rotation;
        // // avatar.SetPositionAndRotation(Camera.transform.position, Camera.transform.rotation);
        // // ikHead.SetPositionAndRotation(Camera.transform.position, Camera.transform.rotation);
    
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {   
           Camera.GetComponent<FirstPersonCamera>().Target = GetComponent<NetworkRigidbody>().InterpolationTarget;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Jumpable")) {
            jumpCount = MAX_JUMP;
        }
    }

    private void MyInput() {
        
        Vector2 input = walkRef.action.ReadValue<Vector2>();

        horizontalInput = input.x;
        verticalInput = input.y;

    }

    private void MovePlayer() {

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rigidbody.AddForce(moveDirection.normalized * PlayerSpeed * 10f, ForceMode.Force);

    }

    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);

        if (flatVel.magnitude > PlayerSpeed) {
            Vector3 limitedVel = flatVel.normalized * PlayerSpeed;
            rigidbody.velocity = new Vector3(limitedVel.x, rigidbody.velocity.y, limitedVel.z);
        }
    }

}
