using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameEvents 
{
    public static event EventHandler ResetPlayer;

    public static void InvokeResetPlayer() {
        ResetPlayer(null, EventArgs.Empty);
    }
}