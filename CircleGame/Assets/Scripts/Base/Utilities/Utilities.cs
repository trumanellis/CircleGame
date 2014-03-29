using UnityEngine;
using System;
using System.Collections;

public class Utilities {
    private static System.Random random;
    public static int RandomSeed { set { random = new System.Random(value); } }

    public static bool RandomBool() {
        return random == null ? (UnityEngine.Random.Range(0, 101) % 2 == 0) : (random.Next(0, 101) % 2 == 0);
    }
}
