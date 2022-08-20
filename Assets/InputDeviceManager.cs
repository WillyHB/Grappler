using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputDevices
{
    MnK,
    Controller,
    Unknown
}

public class InputDeviceManager : MonoBehaviour
{
    private void Start()
    {
        GetComponent<PlayerInput>().onControlsChanged += ControlsChanged;
    }

    private void ControlsChanged(PlayerInput playerInput)
    {

        CurrentDeviceType = playerInput.currentControlScheme switch
        {
            "Keyboard&Mouse" => InputDevices.MnK,
            "Gamepad" => InputDevices.Controller,
            _ => InputDevices.Unknown
        };

        DeviceChanged?.Invoke(CurrentDeviceType);
    }

    public static event Action<InputDevices> DeviceChanged;

    public static InputDevices CurrentDeviceType;
}
