using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TriviaManager : MonoBehaviour
{
    [Header("Database")]

    [SerializeField]
    private TriviaDatabase database;

    [Header("Configuración")]

    [SerializeField]
    private int questionsPerLevel = 5;

    [SerializeField]
    private int startingLives = 3;

    [SerializeField]
    private float delayBetweenQuestions = 1f;

    [SerializeField]
    private float delayBeforeReturningToMap = 1.5f;

    [Header("UI - Header")]

    [SerializeField]
    private TMP_Text levelTitleText;

    [SerializeField]
    private TMP_Text questionCounterText;

    [SerializeField]
    private TMP_Text livesText;

    [Header("UI - Pregunta")]

    [SerializeField]
    private TMP_Text questionText;

    [Header("UI - Respuestas")]

    [SerializeField]
    private Button[] answerButtons;

    [SerializeField]
    private TMP_Text[] answerTexts;

    [Header("UI - Feedback")]

    [SerializeField]
    private TMP_Text feedbackText;

    [Header("Scenes")]

    [SerializeField]
    private string mapSceneName = "MapScene";

    private TriviaLevel currentLevel;

    private List<TriviaQuestion> currentQuestions =
        new List<TriviaQuestion>();

    private int currentQuestionIndex;

    private int currentLives;

    private bool acceptingInput;

    private void Start()
    {
        StartLevel();
    }

    private void StartLevel()
    {
        int levelIndex =
            GameSession.SelectedLevelIndex;

        currentLevel =
            database.GetLevel(levelIndex);

        if (currentLevel == null)
        {
            Debug.LogError(
                "Could not load selected level."
            );

            return;
        }

        currentLives = startingLives;

        currentQuestionIndex = 0;

        if (levelTitleText != null)
        {
            levelTitleText.text =
                currentLevel.LevelName;
        }

        SelectQuestions();

        UpdateLivesUI();

        ShowQuestion();
    }

    private void SelectQuestions()
    {
        currentQuestions.Clear();

        List<TriviaQuestion> availableQuestions =
            new List<TriviaQuestion>(
                currentLevel.Questions
            );

        Shuffle(availableQuestions);

        int amount = Mathf.Min(
            questionsPerLevel,
            availableQuestions.Count
        );

        for (int i = 0; i < amount; i++)
        {
            currentQuestions.Add(
                availableQuestions[i]
            );
        }

        if (currentQuestions.Count < questionsPerLevel)
        {
            Debug.LogWarning(
                $"Level {currentLevel.LevelName} has fewer than {questionsPerLevel} questions."
            );
        }
    }

    private void ShowQuestion()
    {
        if (currentQuestionIndex >= currentQuestions.Count)
        {
            LevelCompleted();
            return;
        }

        acceptingInput = true;

        if (feedbackText != null)
        {
            feedbackText.text = "";
        }

        TriviaQuestion question =
            currentQuestions[currentQuestionIndex];

        questionText.text =
            question.QuestionText;

        questionCounterText.text =
            $"Pregunta {currentQuestionIndex + 1}/{currentQuestions.Count}";

        SetupAnswers(question);
    }

    private void SetupAnswers(
        TriviaQuestion question
    )
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i >= question.Answers.Length)
            {
                answerButtons[i].gameObject.SetActive(false);
                continue;
            }

            answerButtons[i].gameObject.SetActive(true);

            answerTexts[i].text =
                question.Answers[i];

            int answerIndex = i;

            answerButtons[i].onClick.RemoveAllListeners();

            answerButtons[i].onClick.AddListener(
                () => SelectAnswer(answerIndex)
            );
        }
    }

    private void SelectAnswer(int selectedIndex)
    {
        if (!acceptingInput)
        {
            return;
        }

        acceptingInput = false;

        TriviaQuestion question =
            currentQuestions[currentQuestionIndex];

        bool correct =
            selectedIndex ==
            question.CorrectAnswerIndex;

        if (correct)
        {
            CorrectAnswer();
        }
        else
        {
            WrongAnswer();
        }
    }

    private void CorrectAnswer()
    {
        if (feedbackText != null)
        {
            feedbackText.text =
                "¡Correcto!";
        }

        StartCoroutine(
            ContinueAfterAnswer()
        );
    }

    private void WrongAnswer()
    {
        currentLives--;

        UpdateLivesUI();

        if (feedbackText != null)
        {
            feedbackText.text =
                "Respuesta incorrecta";
        }

        if (currentLives <= 0)
        {
            StartCoroutine(
                GameOver()
            );

            return;
        }

        StartCoroutine(
            ContinueAfterAnswer()
        );
    }

    private IEnumerator ContinueAfterAnswer()
    {
        yield return new WaitForSeconds(
            delayBetweenQuestions
        );

        currentQuestionIndex++;

        ShowQuestion();
    }

    private void UpdateLivesUI()
    {
        if (livesText == null)
        {
            return;
        }

        string hearts = "";

        for (int i = 0; i < currentLives; i++)
        {
            hearts += "♥ ";
        }

        livesText.text = hearts;
    }

    private void LevelCompleted()
    {
        acceptingInput = false;

        ProgressManager.CompleteLevel(
            GameSession.SelectedLevelIndex,
            database.LevelCount
        );

        if (feedbackText != null)
        {
            feedbackText.text =
                "¡Nivel completado!";
        }

        StartCoroutine(
            ReturnToMap()
        );
    }

    private IEnumerator GameOver()
    {
        acceptingInput = false;

        if (feedbackText != null)
        {
            feedbackText.text =
                "Sin vidas";
        }

        yield return new WaitForSeconds(
            delayBeforeReturningToMap
        );

        SceneManager.LoadScene(
            mapSceneName
        );
    }

    private IEnumerator ReturnToMap()
    {
        yield return new WaitForSeconds(
            delayBeforeReturningToMap
        );

        SceneManager.LoadScene(
            mapSceneName
        );
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex =
                Random.Range(i, list.Count);

            T temp = list[i];

            list[i] = list[randomIndex];

            list[randomIndex] = temp;
        }
    }
}