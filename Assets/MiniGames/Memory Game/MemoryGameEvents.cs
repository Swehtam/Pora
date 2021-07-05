using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGameEvents : MonoBehaviour
{
    public delegate void MatchEventHandler(string animalName, string animalDescription, Sprite animalImage);
    public static event MatchEventHandler OnTokenMatch;

    public static void TokenMatch(string animalName, string animalDescription, Sprite animalImage)
    {
        OnTokenMatch?.Invoke(animalName, animalDescription, animalImage);
    }
}
