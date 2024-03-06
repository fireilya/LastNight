using System.Collections;
using System.Collections.Generic;
using Assets.scripts.Enum;
using UnityEngine;

public class MoveMentData
{
    public MoveMentData(params (Keys key, KeyCode code)[] codes)
    {
        Movement = new Dictionary<Keys, KeyCode>();
        foreach (var c in codes)
        {
            Movement[c.key]=c.code;
        }

        MoveKeys = new KeyCode[]
        {
            Movement[Keys.Forward],
            Movement[Keys.Back],
            Movement[Keys.Right],
            Movement[Keys.Left]
        };
    }

    public Dictionary<Keys, KeyCode> Movement { get; set; }
    public KeyCode[] MoveKeys { get; set; }
}
