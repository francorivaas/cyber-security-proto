using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "TriviaDatabase",
    menuName = "Cybersecurity Trivia/Database"
)]
public class TriviaDatabase : ScriptableObject
{
    [SerializeField]
    private List<TriviaLevel> levels = new List<TriviaLevel>();

    public List<TriviaLevel> Levels => levels;

    public int LevelCount => levels.Count;

    public TriviaLevel GetLevel(int index)
    {
        if (index < 0 || index >= levels.Count)
        {
            Debug.LogError($"Level index {index} is invalid.");
            return null;
        }

        return levels[index];
    }
}