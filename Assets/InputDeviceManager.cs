using System;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputDeviceManager
{
    public enum InputDevices
    {
        Keyboard,
        Controller
    }
    // Start is called before the first frame update
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Start()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private static void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        
    }

    public static event Action<InputDevices> DeviceChanged;

    public static InputDevices CurrentDeviceType;
    public static InputDevice CurrentDevice;
}
