using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGameEvents : MonoBehaviour
{
    public delegate void MatchEventHandler(string animalName);
    public static event MatchEventHandler OnTokenMatch;

    public static void TokenMatch(string animalName)
    {
        OnTokenMatch?.Invoke(animalName);
    }
}
