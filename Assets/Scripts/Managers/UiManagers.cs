using UnityEngine;

public class UiManagers : MonoBehaviour
{
    public static UiManagers Instance;
    [SerializeField] private GameObject settingsPanel;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ActiveSettingsPanel(bool active)
    {
        settingsPanel.SetActive(active);
    }
}
