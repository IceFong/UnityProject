using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : NetworkBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private InputActionReference jumpRef;
    [SerializeField] private InputActionReference walkRef;
    

    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Camera Camera;

    

    //Player movement
    private float horizontalInput;
    private float verticalInput;
    public float PlayerSpeed = 2f;
    private Vector3 moveDirection;
    public Transform orientation;
    public float groundDrag;

    //Ground Check
    public float playerHeight;
    public LayerMask Jumpable;
    bool grounded;
    

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    bool netDoJump;

    private void OnEnable() {
        
        if (actionAsset != null) {
            actionAsset.Enable();
        }

    }

    void Update() {

        grounded = Physics.Raycast(rigidbody.transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Jumpable);

        MyInput();
        SpeedControl();

    }

    public override void FixedUpdateNetwork()
    {
        //Only move own player and not other player
        if (HasStateAuthority == false) return;

        MovePlayer();

        if (grounded)
            rigidbody.drag = groundDrag;
        else 
            rigidbody.drag = 0;

        if (netDoJump) {
            readyToJump = false;
            Jump();
            netDoJump = false;
        }
    
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {  
            Camera = Camera.main;
            FirstPersonCamera fpc = Camera.GetComponent<FirstPersonCamera>();
            fpc.Target = GetComponent<NetworkTransform>().InterpolationTarget;
            fpc.orientation = transform.GetChild(2).transform;
            fpc.cameraPosition = transform.GetChild(3).transform;
        }
    }



    private void MyInput() {
        
        Vector2 input = walkRef.action.ReadValue<Vector2>();

        horizontalInput = input.x;
        verticalInput = input.y;

        if (jumpRef.action.triggered && readyToJump && grounded) {
            netDoJump = true;
            Invoke(nameof(ResetJump), jumpCooldown);
        }

    }

    private void MovePlayer() {

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // rigidbody.AddForce(moveDirection.normalized * PlayerSpeed * 10f, ForceMode.Force);

        if (grounded)
            rigidbody.AddForce(moveDirection.normalized * PlayerSpeed * 10f, ForceMode.Force);
        else 
            rigidbody.AddForce(moveDirection.normalized * PlayerSpeed * 10f * airMultiplier, ForceMode.Force);

    }

    private void SpeedControl() {
        Vector3 flatVel = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);

        if (flatVel.magnitude > PlayerSpeed) {
            Vector3 limitedVel = flatVel.normalized * PlayerSpeed;
            rigidbody.velocity = new Vector3(limitedVel.x, rigidbody.velocity.y, limitedVel.z);
        }
    }

    private void Jump() {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);

        rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() {
        readyToJump = true;
    }

}
