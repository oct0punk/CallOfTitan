using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public static ControlBase control { get; private set; }

    public static void ChangeControl(bool zqsd)
    {
        control = zqsd ? new ZQSD() : new WASD();
    }
}

public class ControlBase
{
    public virtual float HorizontalAxis()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) return -1.0f;
        if (Input.GetKey(KeyCode.RightArrow)) return 1.0f;
        return 0.0f;
    }
    public virtual float VerticalAxis()
    {
        if (Input.GetKey(KeyCode.UpArrow)) return 1.0f;
        if (Input.GetKey(KeyCode.DownArrow)) return -1.0f;
        return 0.0f;
    }
}

class ZQSD : ControlBase
{
    public override float HorizontalAxis()
    {
        if (Input.GetKey(KeyCode.Q)) return -1.0f;
        if (Input.GetKey(KeyCode.D)) return 1.0f;
        return base.HorizontalAxis();
    }

    public override float VerticalAxis()
    {
        if (Input.GetKey(KeyCode.Z)) return 1.0f;
        if (Input.GetKey(KeyCode.S)) return -1.0f;
        return base.VerticalAxis();
    }
}

class WASD: ControlBase
{
    public override float HorizontalAxis()
    {
        if (Input.GetKey(KeyCode.A)) return -1.0f;
        if (Input.GetKey(KeyCode.D)) return 1.0f;
        return base.HorizontalAxis();
    }

    public override float VerticalAxis()
    {
        if (Input.GetKey(KeyCode.W)) return 1.0f;
        if (Input.GetKey(KeyCode.S)) return -1.0f;
        return base.VerticalAxis();
    }
}