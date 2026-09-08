using ConquerTheStars.Managers;
using UnityEngine;

public class ButtonFeatures : MonoBehaviour
{
    public void NewGame()
    {
        GameManager.Instance.StartNewGame();
    }
    public void Continue()
    {
        GameManager.Instance.ContinueGame();
    }

    public void Settings(bool active)
    {
        UiManagers.Instance.ActiveSettingsPanel(active);
    }

    public void ExitGame()
    {
        GameManager.Instance.Exit();
    }

    public void ExitTitle()
    {
        GameManager.Instance.ExitToTitle();
    }
}
