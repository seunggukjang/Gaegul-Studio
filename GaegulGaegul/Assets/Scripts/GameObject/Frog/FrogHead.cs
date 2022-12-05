using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogHead : MonoBehaviour
{
    LayerMask frogHeadMask;
    LayerMask jumpOffPlatformMask;

    private void Start()
    {
        frogHeadMask = LayerMask.NameToLayer("FrogHead");
        jumpOffPlatformMask = LayerMask.NameToLayer("JumpOffPlatform");
    }
    public void CollideOff()
    {
        Physics2D.IgnoreLayerCollision(frogHeadMask, jumpOffPlatformMask, true);
    }
    public void CollideOn()
    {
        Physics2D.IgnoreLayerCollision(frogHeadMask, jumpOffPlatformMask, false);
    }
}
