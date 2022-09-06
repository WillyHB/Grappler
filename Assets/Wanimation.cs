using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
[CreateAssetMenu(menuName ="Animation")]
public class Wanimation : ScriptableObject
{
    public Sprite[] Frames;
    public float FrameLength;
    public bool Loop;
    public Wanimation FollowUpAnimation;

}

[CustomEditor(typeof(Wanimation))]
public class WanimationEditor : Editor
{
    private Wanimation wanimation;

    public override VisualElement CreateInspectorGUI()
    {
        wanimation = target as Wanimation;

        return base.CreateInspectorGUI();
    }

    public override void OnInspectorGUI()
    {

        GUI.DrawTexture(new Rect(0, 300, 100, 100), wanimation.Frames[0].texture,);
        base.OnInspectorGUI();


    }
}


