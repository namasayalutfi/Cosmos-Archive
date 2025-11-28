using UnityEngine;
using UnityEngine.UI;

public class PlanetInfoToggle : MonoBehaviour
{
<<<<<<< Updated upstream
    public GameObject scrollView;     // ScrollView section
    public Image arrowImage;          // arrow icon
    public Sprite arrowUp;           
    public Sprite arrowDown;         
=======
    public GameObject scrollView;
    public Image arrowImage;
    public Sprite arrowUp;
    public Sprite arrowDown;
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
=======

        // Jika panel dibuka (expanded), paksa scroll ke paling atas
        if (isExpanded)
        {
            // Ambil komponen ScrollRect dari objek scrollView
            ScrollRect sr = scrollView.GetComponent<ScrollRect>();

            if (sr != null)
            {
                // Set posisi ke 1 (Paling Atas)
                sr.verticalNormalizedPosition = 1f;

                // Opsional: Hentikan inersia/momentum scroll sisa sebelumnya (supaya diam)
                sr.velocity = Vector2.zero;
            }
        }
>>>>>>> Stashed changes
    }
}
