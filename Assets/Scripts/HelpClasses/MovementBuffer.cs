using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementBuffer
{
    Vector2 vectorInput;
    float bufferTime;

    public bool BufferHasTimeOut => Time.time > bufferTime;

    public MovementBuffer(Vector2 input, float bufferLength)
    {
        vectorInput = input;
        bufferTime = Time.time + bufferLength;
    }

    public Vector2 GetBuffer()
    {
        Vector2 buffer = vectorInput;
        vectorInput = Vector2.zero;

        return buffer;
    }
}
