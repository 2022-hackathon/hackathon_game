using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActiveObject : ActiveObject
{
    public override void Active()
    {
        base.Active();
        Debug.Log("Active!");
    }
}
