
using System;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterControllerPrototype _cc;
    [SerializeField] private Ball _prefabBall;
    private Vector3 _forward;
    [Networked] private TickTimer delay {get; set;}
    [SerializeField] private PhysxBall _prefabPhysxBall;



    private void CheckButton(NetworkInputData data) { 
        if ((data.buttons & NetworkInputData.MOUSEBUTTON1) != 0) {
            delay = TickTimer.CreateFromSeconds(Runner, 0.1f);
            Runner.Spawn(_prefabBall,
                transform.position + _forward, Quaternion.LookRotation(_forward),
                Object.InputAuthority, (runner, o) => {
                    // Initialize the Ball before synchronizing
                    o.GetComponent<Ball>().Init();
                });
        }
        else if ((data.buttons & NetworkInputData.MOUSEBUTTON2) != 0) {
            delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
            Runner.Spawn(_prefabPhysxBall,
                transform.position + _forward,
                Quaternion.LookRotation(_forward),
                Object.InputAuthority,
                (runner, o) =>
                {
                    o.GetComponent<PhysxBall>().Init(10 * _forward);
                });
        }
    }

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterControllerPrototype>();
        _forward = transform.forward;
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * Runner.DeltaTime * data.direction);

            if (data.direction.sqrMagnitude > 0) {
                _forward = data.direction;
            }

            if (delay.ExpiredOrNotRunning(Runner)) {
                CheckButton(data);
            }   

        }
    }
}