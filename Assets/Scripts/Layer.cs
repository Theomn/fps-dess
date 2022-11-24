using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Layer
{
    public static readonly int playerLayer = LayerMask.NameToLayer("Player");
    public static readonly int enemyLayer = LayerMask.NameToLayer("Enemy");
    public static readonly int groundLayer = LayerMask.NameToLayer("Ground");
}
