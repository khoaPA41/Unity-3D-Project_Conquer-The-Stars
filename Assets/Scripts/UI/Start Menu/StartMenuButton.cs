using UnityEngine;

public class StartMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject startMenuUI;


    public void ActiveSettings(bool active)
    {
        UiManagers.Instance.ActiveSettingsPanel(active);
    }
}
