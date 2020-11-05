using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreEventArgs : EventArgs 
{
    public int score;
}

public class BreakableEventArgs : EventArgs 
{
    public float x;
    public float y;
    public float z;
}

public static class GameEvents 
{
    public static event EventHandler ResetPlayer;
    public static event EventHandler<ScoreEventArgs> ScoreIncreased;
    public static event EventHandler LevelIncreased;
    public static event EventHandler<BreakableEventArgs> InstantiateBreakable;

    public static void InvokeResetPlayer() {
        ResetPlayer(null, EventArgs.Empty);
    }

    public static void InvokeScoreIncreased(int n) {
        ScoreIncreased(null, new ScoreEventArgs{score = n});
    }

    public static void InvokeLevelIncreased() {
        LevelIncreased(null, EventArgs.Empty);
    }

    public static void InvokeInstantiateBreakable(float a, float b, float c) {
        InstantiateBreakable(null, new BreakableEventArgs{x = a, y=b, z=c});
    }
}