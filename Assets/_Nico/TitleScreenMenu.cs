using UnityEngine;

public class TitleScreenMenu : MonoBehaviour
{

    [SerializeField] private GameObject instructionsPanel;


    public void OpenPanel()
    {
        instructionsPanel.SetActive(true);
    }
    public void ClosePanel()
    {
        instructionsPanel.SetActive(false);
    }
}
