using Fusion;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{

    public float PlayerSpeed = 2f;
    public Camera Camera;
    
    // private Rigidbody rb;
    private CharacterController _controller;

    private void Awake() {
        _controller = GetComponent<CharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        //Only move own player and not other player
        if (HasStateAuthority == false) return;

        var cameraRotationY = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
        Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;
        // Vector3 move = PlayerSpeed * Runner.DeltaTime * new Vector3(Input.GetAxis("Horizontal"), 0 , Input.GetAxis("Vertical"));
    
        _controller.Move(move);

        if (move != Vector3.zero) {
            gameObject.transform.forward = move;
            // print(move);
        }
    
    }

    public override void Spawned()
    {
        if (!HasStateAuthority) return;

        Camera = Camera.main;
        Camera.GetComponent<FirstPersonCamera>().Target = GetComponent<NetworkTransform>().InterpolationTarget;
        // rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }
    
    
}
