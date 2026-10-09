using UnityEngine;

public class StartMenuButton : MonoBehaviour
{
    [SerializeField] private GameObject startMenuUI;
    [SerializeField] private GameObject continueButtonUI;



    public void Start()
    {
        CheckHasSave();
    }

    public void ActiveSettings(bool active)
    {
        UiManagers.Instance.ActiveSettingsPanel(active);
    }

    private void CheckHasSave()
    {
        continueButtonUI.SetActive(false);
        if (!SaveManagers.Instance.HasSaveData()) return;
        continueButtonUI.SetActive(true);
    }
}
