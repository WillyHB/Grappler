using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Event Channels/Rumble")]
public class RumbleEventChannel : ScriptableObject
{
    public event Action<float, float, float> Rumble;
    public event Action<float, float, float, float, float> RumbleLinear;
    public event Action<float, float, float, float, float> RumblePulse;

    public void PerformRumble(float lowFreq, float highFreq, float time)
        => Rumble?.Invoke(lowFreq, highFreq, time);
    public void PerformLinearRumble(float lowA, float lowB, float highA, float highB, float time)
        => RumbleLinear?.Invoke(lowA, lowB, highA, highB, time);

    public void PerformPulseRumble(float lowFreq, float highFreq, float burstTime, float timeBetween, float time)
        => RumblePulse?.Invoke(lowFreq, highFreq, burstTime, timeBetween, time);

}
