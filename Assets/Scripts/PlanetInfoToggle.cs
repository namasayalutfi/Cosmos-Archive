using UnityEngine;
using UnityEngine.UI;

public class PlanetInfoToggle : MonoBehaviour
{
    public GameObject scrollView;     // ScrollView Object
    public Image arrowImage;          // arrow icon
    public Sprite arrowUp;
    public Sprite arrowDown;

    private bool isExpanded = false;

    public void TogglePanel()
    {
        Debug.Log("TOGGLE PANEL: " + isExpanded);
        isExpanded = !isExpanded;

        // 1. Show / hide content
        scrollView.SetActive(isExpanded);

        // 2. Change icon
        arrowImage.sprite = isExpanded ? arrowUp : arrowDown;

        // 3. Force Unity UI to rebuild layout (Agar border membesar)
        LayoutRebuilder.ForceRebuildLayoutImmediate(scrollView.transform.parent as RectTransform);

        // --- TAMBAHAN BARU ---
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
    }
}