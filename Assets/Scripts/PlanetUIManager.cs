using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic; // Perlu ini untuk Dictionary

public class PlanetUIManager : MonoBehaviour
{
    [Header("Data Source")]
    public TextAsset jsonFile; // Drag file PlanetData.json kesini

    [Header("Panel References")]
    public GameObject dropDownPanel;
    public GameObject detailPanel;

    [Header("Detail UI Components")]
    public TextMeshProUGUI titleText;
    public Image planetIconImage;
    public TextMeshProUGUI infoText;
    public ScrollRect descriptionScroll;

    // Database sementara untuk pencarian cepat (Nama Planet -> Deskripsi)
    private Dictionary<string, string> planetDatabase = new Dictionary<string, string>();

    void Start()
    {
        LoadPlanetData();
        // --- TAMBAHAN BARU ---
        // 1. Pastikan Panel Detail mati saat game mulai
        if (detailPanel != null)
            detailPanel.SetActive(false);

        // 2. Pastikan Panel Menu/Dropdown nyala saat game mulai
        if (dropDownPanel != null)
            dropDownPanel.SetActive(true);
    }

    void LoadPlanetData()
    {
        if (jsonFile != null)
        {
            // Parsing JSON ke Class C#
            PlanetCollection data = JsonUtility.FromJson<PlanetCollection>(jsonFile.text);

            // Masukkan ke Dictionary agar mudah dicari
            foreach (PlanetData planet in data.planets)
            {
                // Kita simpan Nama sebagai Kunci, Deskripsi sebagai Nilai
                if (!planetDatabase.ContainsKey(planet.name))
                {
                    planetDatabase.Add(planet.name, planet.description);
                }
            }
        }
        else
        {
            Debug.LogError("File JSON belum dimasukkan ke Inspector PlanetUIManager!");
        }
    }

    // Perhatikan: Parameter 'description' DIHAPUS, karena Manager yang akan mencarinya
    public void ShowPlanetDetails(string planetName, Sprite icon)
    {
        // 1. Set Judul & Icon
        titleText.text = planetName;
        planetIconImage.sprite = icon;

        // 2. Cari Deskripsi dari JSON Database
        if (planetDatabase.ContainsKey(planetName))
        {
            infoText.text = planetDatabase[planetName];
        }
        else
        {
            infoText.text = "Deskripsi untuk " + planetName + " tidak ditemukan di JSON.";
            Debug.LogWarning("Pastikan 'name' di JSON sama persis dengan Text di Button: " + planetName);
        }

        // 3. Reset Scroll & Swap Panel
        descriptionScroll.verticalNormalizedPosition = 1f;
        dropDownPanel.SetActive(false);
        detailPanel.SetActive(true);
    }

    public void ClosePlanetDetails()
    {
        detailPanel.SetActive(false);
        dropDownPanel.SetActive(true);
    }
}

[System.Serializable]
public class PlanetData
{
    public string name;
    public string description;
}

[System.Serializable]
public class PlanetCollection
{
    public PlanetData[] planets;
}