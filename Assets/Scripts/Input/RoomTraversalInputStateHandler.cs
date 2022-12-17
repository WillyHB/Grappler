using System;
using UnityEngine;
using UnityEngine.InputSystem;


[CreateAssetMenu(menuName = "Input/InputHandlers/RoomTraversal")]
public class RoomTraversalInputStateHandler : InputStateHandler
{
    public InputActionAsset Actions;

    public bool TraversingRoom;

    public override InputState HandleInputState(InputState state)
    {
        if (TraversingRoom)
        {
            state.MoveDirection = 0;
            state.SwimDirection = 0;
            state.GrappleLength = 0;

            state.CanJump = false;
            state.CanGrapple = false;
            state.CanShoot = false;

            state.IsJumping = false;
            state.IsCrouching = false;
            state.IsWalking = false;
        }

        return state;
    }
}

