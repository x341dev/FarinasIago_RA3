using UnityEngine;

public class UIButtonActions : MonoBehaviour
{
    public void OnPauseButtonPressed()
    {
        GameEvents.RequestPauseToggle();
    }

    public void OnNextLevelButtonPressed()
    {
        GameEvents.RequestNextLevel();
    }

    public void OnExitToMainMenuButtonPressed()
    {
        GameEvents.RequestExitToMainMenu();
    }
}
