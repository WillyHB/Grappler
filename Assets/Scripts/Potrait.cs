using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Character Potrait")]
public class Potrait : ScriptableObject
{
    public Sprite Poker;
    public Sprite CloseEyes;
    public Sprite Happy;
    public Sprite Sad;
    public Sprite Confused;
    public Sprite Shocked;
    public Sprite Flabbergasted;
    public Sprite Angry;
    public Sprite Fear;

    public enum Emotions
    {
        Poker,
        Happy,
        Sad,
        Angry,
        Fear,
        Confused,
        Shocked,
        Flabbergasted
    }
}
