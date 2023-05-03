using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyModifier : MonoBehaviour
{
    public static DifficultyModifier instance;

    public bool enemiesHaveMoreHealth = false;
    public bool playerHasLessHealth = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}


