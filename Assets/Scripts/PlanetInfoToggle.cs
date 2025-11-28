using UnityEngine;
using UnityEngine.UI;

public class PlanetInfoToggle : MonoBehaviour
{
    public GameObject scrollView;     // ScrollView section
    public Image arrowImage;          // arrow icon
    public Sprite arrowUp;           
    public Sprite arrowDown;         

    private bool isExpanded = false;

    public void TogglePanel()
    {
        Debug.Log("TOGGLE PANEL: " + isExpanded);
        isExpanded = !isExpanded;

        // Show / hide content
        scrollView.SetActive(isExpanded);

        // Change icon
        arrowImage.sprite = isExpanded ? arrowUp : arrowDown;

        // Force Unity UI to rebuild layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollView.transform.parent as RectTransform);
    }
}
