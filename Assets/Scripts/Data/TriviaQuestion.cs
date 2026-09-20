using UnityEngine;

[CreateAssetMenu(
    fileName = "NewTriviaQuestion",
    menuName = "Cybersecurity Trivia/Question"
)]
public class TriviaQuestion : ScriptableObject
{
    [Header("Pregunta")]

    [TextArea(3, 6)]
    [SerializeField]
    private string questionText;

    [Header("Respuestas")]

    [SerializeField]
    private string[] answers = new string[4];

    [Header("Respuesta correcta")]

    [SerializeField]
    private int correctAnswerIndex;

    [Header("Explicación opcional")]

    [TextArea(2, 5)]
    [SerializeField]
    private string explanation;

    public string QuestionText => questionText;

    public string[] Answers => answers;

    public int CorrectAnswerIndex => correctAnswerIndex;

    public string Explanation => explanation;
}