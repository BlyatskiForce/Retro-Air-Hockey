using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public int framerateRange = 60;

    public int AverageFPS { get; private set; }
    public int HighestFPS { get; private set; }
    public int LowestFPS { get; private set; }

    private int[] fpsBuffer;
    private int fpsBufferIndex;

    private void Update()
    {
        if (fpsBuffer == null || fpsBuffer.Length != framerateRange)
            IntalizeBuffer();

        UpdateBuffer();
        CalculateFPS();
        AverageFPS = (int)(1f / Time.unscaledDeltaTime);
    }

    private void UpdateBuffer()
    {
        fpsBuffer[fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
        if (fpsBufferIndex >= framerateRange)
            fpsBufferIndex = 0;
    }

    private void CalculateFPS()
    {
        int sum = 0;
        int highest = 0;
        int lowest = int.MaxValue;
        for (int i = 0; i < framerateRange; i++)
        {
            int fps = fpsBuffer[i];
            sum += fpsBuffer[i];
            if (fps > highest)
            {
                highest = fps;
            }
            if (fps < lowest)
            {
                lowest = fps;
            }
        }
        AverageFPS = sum / framerateRange;
        LowestFPS = lowest;
        HighestFPS = highest;
    }

    private void IntalizeBuffer()
    {
        if (framerateRange <= 0)
            framerateRange = 1;

        fpsBuffer = new int[framerateRange];
        fpsBufferIndex = 0;
    }
}
