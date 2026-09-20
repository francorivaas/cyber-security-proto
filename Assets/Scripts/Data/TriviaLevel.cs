using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewTriviaLevel",
    menuName = "Cybersecurity Trivia/Level"
)]
public class TriviaLevel : ScriptableObject
{
    [Header("Información")]

    [SerializeField]
    private string levelName;

    [TextArea(2, 4)]
    [SerializeField]
    private string description;

    [SerializeField]
    private Sprite icon;

    [Header("Preguntas")]

    [SerializeField]
    private List<TriviaQuestion> questions = new List<TriviaQuestion>();

    public string LevelName => levelName;

    public string Description => description;

    public Sprite Icon => icon;

    public List<TriviaQuestion> Questions => questions;
}