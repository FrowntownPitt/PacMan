﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MazeDirections{

    public const int Count = 4;

    private static IntVector2[] vectors =
    {
        new IntVector2(0,1),
        new IntVector2(1,0),
        new IntVector2(0,-1),
        new IntVector2(-1,0)
    };

    public static IntVector2 ToIntVector2(this MazeDirection direction)
    {
        return vectors[(int)direction];
    }

    public static MazeDirection RandomValue
    {
        get
        {
            return (MazeDirection)Random.Range(0, Count);
        }
    }
}
