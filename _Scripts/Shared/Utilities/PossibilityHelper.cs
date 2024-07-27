using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibilityHelper
{
    public int GetAmongItems(float[] items)
    {
        int output = -1;
        float ramdonFloat = UnityEngine.Random.Range(0f, 1f);

        for (int i = 0; i < items.Length; ++i)
        {
            float min = i == 0 ? 0 : items[i - 1];
            float max = items[i];
            if (ramdonFloat > min && ramdonFloat <= max)
            {
                output = i;
                break;
            }
        }

        return output;
    }

    public bool GetChance(float possibility)
    {
        return UnityEngine.Random.Range(0f, 1f) <= possibility;
    }
}
