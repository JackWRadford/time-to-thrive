using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static System.Action SaveInitiated;

    public static void OnSaveInitiated()
    {
        SaveInitiated?.Invoke();
    }
}
