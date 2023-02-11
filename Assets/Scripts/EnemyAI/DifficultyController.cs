using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    public float easyDifficulty = 0.5f;
    public float mediumDifficulty = 1.0f;
    public float hardDifficulty = 1.5f;

    //implement a public 'Dropdown' class

    public void SetDifficulty(int difficulty)
    {
        float difficultyMultiplier = 0f; 
        switch (difficulty)
        {
            case 0:
                difficultyMultiplier = easyDifficulty;
                break;
            case 1:
                difficultyMultiplier = mediumDifficulty;
                break;
            case 2:
                difficultyMultiplier = hardDifficulty;
                break;
            default:
                difficultyMultiplier = mediumDifficulty;
                break;
        }
        // Get a reference to the enemy AI script (AI controller)
        // if statement to check ai is != null

            // Modify the AI's parameters based on the difficulty multiplier
            // <enemyAI obj>.<AI parameters> = <difficultyMultiplier>;
            // Added new enemy combat features to AIController (gernade spam, movement??)
    }


    // Start is called before the first frame update
    void Start()
    {
        // Reference to the UI element via GameObject and gets reference to Dropdowm component
        // A trigger when a value of Dropdown changes which call 'SetDifficulty' is passed as event handler.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
