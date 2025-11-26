using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanetDataSender : MonoBehaviour
{
    // HAPUS variabel description manual
    // public string planetDescription; <--- Tidak perlu lagi

    private string autoPlanetName;
    private Sprite autoPlanetIcon;
    private PlanetUIManager uiManager;

    void Start()
    {
        uiManager = FindObjectOfType<PlanetUIManager>();

        // Ambil Nama dari Child Text
        TextMeshProUGUI btnText = GetComponentInChildren<TextMeshProUGUI>();
        if (btnText != null) autoPlanetName = btnText.text;

        // Ambil Icon dari Child Image
        Image[] allImages = GetComponentsInChildren<Image>();
        foreach (Image img in allImages)
        {
            if (img.gameObject != this.gameObject)
            {
                autoPlanetIcon = img.sprite;
                break;
            }
        }
    }

    public void OnClickPlanet()
    {
        if (uiManager != null)
        {
            // Panggil fungsi ShowPlanetDetails TANPA deskripsi
            uiManager.ShowPlanetDetails(autoPlanetName, autoPlanetIcon);
        }
    }
}