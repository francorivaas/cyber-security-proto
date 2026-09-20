using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{
    [Header("Database")]

    [SerializeField]
    private TriviaDatabase database;

    [Header("Nodes")]

    [SerializeField]
    private List<MapNodeButton> nodes = new List<MapNodeButton>();

    [Header("Scenes")]

    [SerializeField]
    private string triviaSceneName = "TriviaScene";

    private void Start()
    {
        SetupNodes();
    }

    private void SetupNodes()
    {
        if (database == null)
        {
            Debug.LogError("TriviaDatabase not assigned.");
            return;
        }

        int amount = Mathf.Min(
            nodes.Count,
            database.LevelCount
        );

        for (int i = 0; i < amount; i++)
        {
            TriviaLevel level = database.GetLevel(i);

            bool unlocked =
                ProgressManager.IsLevelUnlocked(i);

            nodes[i].Setup(
                i,
                level,
                unlocked,
                this
            );
        }
    }

    public void OpenLevel(int levelIndex)
    {
        if (!ProgressManager.IsLevelUnlocked(levelIndex))
        {
            return;
        }

        GameSession.SelectedLevelIndex = levelIndex;

        SceneManager.LoadScene(triviaSceneName);
    }

    public void ResetProgress()
    {
        ProgressManager.ResetProgress();

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }
}