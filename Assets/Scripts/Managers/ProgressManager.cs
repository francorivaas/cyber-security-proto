using UnityEngine;

public static class ProgressManager
{
    private const string HighestUnlockedLevelKey = "HighestUnlockedLevel";

    public static int GetHighestUnlockedLevel()
    {
        return PlayerPrefs.GetInt(HighestUnlockedLevelKey, 0);
    }

    public static bool IsLevelUnlocked(int levelIndex)
    {
        return levelIndex <= GetHighestUnlockedLevel();
    }

    public static void CompleteLevel(int completedLevelIndex, int totalLevels)
    {
        int highestUnlocked = GetHighestUnlockedLevel();

        int nextLevel = completedLevelIndex + 1;

        if (nextLevel >= totalLevels)
        {
            return;
        }

        if (nextLevel > highestUnlocked)
        {
            PlayerPrefs.SetInt(
                HighestUnlockedLevelKey,
                nextLevel
            );

            PlayerPrefs.Save();
        }
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(HighestUnlockedLevelKey);
        PlayerPrefs.Save();
    }
}