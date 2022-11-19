using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadRumbleManager : MonoBehaviour
{
    private bool isRumbling;
    public RumbleEventChannel channel;

    public enum RumblePattern
    {
        Rumble,
        Linear,
        Pulse
    }

    private RumblePattern rumblePattern;

    private float lowA, lowB;
    private float highA, highB;
    private float timer;
    private float time;

    private float  burstTime, timeBetween, burstTimer;

    private bool burstRumble;

    public void StopRumble()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0, 0);
        }

        isRumbling = false;

        lowA = 0;
        lowB = 0;
        timer = 0;
        burstTime = 0;
        timeBetween = 0;
        burstTimer = 0;
    }

    public void PauseRumble()
    {
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0, 0);
        }

        isRumbling = false;
    }

    private void Start()
    {
        channel.Rumble += Rumble;
        channel.RumbleLinear += RumbleLinear;
        channel.RumblePulse += PulseRumble;
        channel.Stop += StopRumble;
    }

    private void OnDisable()
    {
        channel.Rumble -= Rumble;
        channel.RumbleLinear -= RumbleLinear;
        channel.RumblePulse -= PulseRumble;
        channel.Stop -= StopRumble;

        StopRumble();
    }

    public void Update()
    {
        if (isRumbling)
        {
            if (Gamepad.current != null)
            {
                timer += Time.deltaTime;

                if (timer > time && time > -1)
                {
                    StopRumble();
                }

                else
                {
                    switch (rumblePattern)
                    {
                        case RumblePattern.Linear:
                            Gamepad.current.SetMotorSpeeds(Mathf.Lerp(lowA, lowB, time), Mathf.Lerp(highA, highB, time));
                            break;

                        case RumblePattern.Pulse:
                            burstTimer += Time.deltaTime;

                            if (burstRumble)
                            {
                                if (burstTimer >= burstTime)
                                {
                                    burstRumble = false;
                                    burstTimer = 0;
                                    PauseRumble();
                                }
                            }

                            else
                            {
                                if (burstTimer >= timeBetween)
                                {
                                    burstRumble = true;
                                    burstTimer = 0;
                                    Gamepad.current.SetMotorSpeeds(lowA, lowB);
                                }
                            }

                            break;
                    }
                }

            }

            else
            {
                StopRumble();
            }

        }

    }

    private void Rumble(float lowFreq, float highFreq, float t)
    {
        isRumbling = true;

        time = t;
        lowA = lowFreq;
        highA = highFreq;

        if (InputDeviceManager.CurrentDeviceType == InputDevices.Controller)
        {
            Gamepad.current.SetMotorSpeeds(lowFreq, highFreq);
        }
    }

    private void RumbleLinear(float lowStart, float lowEnd, float highStart, float highEnd, float t)
    {
        isRumbling = true;
        lowA = lowStart;
        lowB = lowEnd;
        highA = highStart;
        highB = highEnd;
        time = t;
    }



    private void PulseRumble(float lowFreq, float highFreq, float _burstTime, float _timeBetween, float t)
    {
        isRumbling = true;

        lowA = lowFreq;
        highA = highFreq;

        burstTime = _burstTime;
        timeBetween = _timeBetween;
        time = t;
    }
}
