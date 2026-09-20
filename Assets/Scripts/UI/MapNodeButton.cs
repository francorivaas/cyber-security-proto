using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapNodeButton : MonoBehaviour
{
    [Header("Referencias")]

    [SerializeField]
    private Button button;

    [SerializeField]
    private TMP_Text levelNameText;

    [SerializeField]
    private Image iconImage;

    [SerializeField]
    private GameObject lockedVisual;

    private int levelIndex;

    private MapManager mapManager;

    public void Setup(
        int index,
        TriviaLevel level,
        bool unlocked,
        MapManager manager
    )
    {
        levelIndex = index;
        mapManager = manager;

        if (levelNameText != null)
        {
            levelNameText.text = level.LevelName;
        }

        if (iconImage != null && level.Icon != null)
        {
            iconImage.sprite = level.Icon;
        }

        if (lockedVisual != null)
        {
            lockedVisual.SetActive(!unlocked);
        }

        button.interactable = unlocked;

        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(OnNodeClicked);
    }

    private void OnNodeClicked()
    {
        mapManager.OpenLevel(levelIndex);
    }
}