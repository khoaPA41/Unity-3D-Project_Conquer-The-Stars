using UnityEngine;

public class StartMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject backButton;

    public void ActiveSettings(bool active)
    {
        startMenuUI.SetActive(!active);
        backButton.SetActive(active);
        UiManagers.Instance.ActiveSettingsPanel(active);
    }
}
