using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static List<T> Shuffle<T>(List<T> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            int index = Random.Range(0, list.Count);

            if(index == i)
                continue;

            T temp = list[i];

            list[i] = list[index];
            list[index] = temp;
        }

        return list;
    }
}
