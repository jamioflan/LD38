using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities {

    public static void shuffle<T>(T[] array)
    {
        int n = array.Length;
        for (int i = 0; i < n; i++)
        {
            int r = i + (int)Random.Range(0, (n - i));
            T t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }

}
