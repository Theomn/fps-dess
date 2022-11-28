using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layer
{
    public static readonly int player = LayerMask.NameToLayer("Player");
    public static readonly int enemy = LayerMask.NameToLayer("Enemy");
    public static readonly int ground = LayerMask.NameToLayer("Ground");
}
