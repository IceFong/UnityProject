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
    
    private Camera Camera;
    [SerializeField] private float PlayerSpeed = 2f;
    [SerializeField] private float JumpStrength = 200f;


    private void OnEnable() {
        
        if (actionAsset != null) {
            actionAsset.Enable();
        }

    }

    // void Update() {
    //     if (jumpButton.action.triggered) {
    //         print("jump");
    //         rigidbody.AddForce( Vector3.up * 500 );
    //     }

    //     Vector2 moveAmount = walkRef.action.ReadValue<Vector2>();
    //     float x = moveAmount.x * Time.deltaTime;
    //     float y = moveAmount.y * Time.deltaTime;

    //     transform.Translate( x, 0, y );
    // }

    public override void FixedUpdateNetwork()
    {
        //Only move own player and not other player
        if (HasStateAuthority == false) return;

        // var cameraRotationY = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
        
    

        // if (move != Vector3.zero) {
        //     gameObject.transform.forward = move;
        //     // print(move);
        // }

        if (jumpButton.action.triggered) {
            print("jump");
            rigidbody.AddForce( Vector3.up * JumpStrength );
            
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
    
    }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Camera = gameObject.AddComponent<Camera>();
            Camera.AddComponent<FirstPersonCamera>().Target = GetComponent<NetworkTransform>().InterpolationTarget;
            // Camera.GetComponent<FirstPersonCamera>().Target = GetComponent<NetworkTransform>().InterpolationTarget;
        }
    }

}
