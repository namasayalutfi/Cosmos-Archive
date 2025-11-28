using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PlanetUIManager : MonoBehaviour
{
    [Header("Camera Reference")]
    public SpectatorCamera camScript;

    [Header("Data Source")]
    public TextAsset jsonFile;

    [Header("Panel References")]
    public GameObject dropDownPanel;
    public GameObject detailPanel;

    [Header("Detail UI Components")]
    public TextMeshProUGUI titleText;
    public Image planetIconImage;
    public TextMeshProUGUI infoText;
    public ScrollRect descriptionScroll;

    private Dictionary<string, PlanetData> planetDatabase = new Dictionary<string, PlanetData>();

    void Start()
    {
        LoadPlanetData();

        if (detailPanel != null) detailPanel.SetActive(false);
        if (dropDownPanel != null) dropDownPanel.SetActive(true);
    }

    void LoadPlanetData()
    {
        if (jsonFile != null)
        {
            PlanetCollection data = JsonUtility.FromJson<PlanetCollection>(jsonFile.text);

            foreach (PlanetData planet in data.planets)
            {
                string cleanName = planet.name.Trim(); 

                if (!planetDatabase.ContainsKey(cleanName))
                {
                    planetDatabase.Add(cleanName, planet);
                    // Debugging: Cek apa yang berhasil masuk
                    Debug.Log("Database Loaded: '" + cleanName + "'"); 
                }
            }
        }
        else
        {
            Debug.LogError("File JSON belum dimasukkan ke Inspector!");
        }
    }

    public void ShowPlanetDetails(string planetName, Sprite icon)
    {
        string searchName = planetName.Trim();

        titleText.text = searchName;
        planetIconImage.sprite = icon;

        Debug.Log("Searching for: '" + searchName + "'");

        // Ambil data lengkap dan format menjadi string
        if (planetDatabase.ContainsKey(planetName))
        {
            PlanetData data = planetDatabase[searchName];

            string content = "";

            content += $"<b>Radius:</b> {data.radius}\n";
            content += $"<b>Jarak:</b> {data.distance}\n";
            content += $"<b>Rotasi:</b> {data.rotationPeriod}\n";
            content += $"<b>Revolusi:</b> {data.revolutionPeriod}\n\n";
            
            content += "<b>Deskripsi:</b>\n";
            content += data.description;

            infoText.text = content;
        }
        else
        {
            infoText.text = "Data tidak ditemukan untuk " + planetName;

            string allKeys = "";
            foreach(var key in planetDatabase.Keys) allKeys += key + ", ";
            Debug.LogError("GAGAL. Yang dicari: '" + searchName + "'. Yang tersedia: " + allKeys);
        }

        // Spectate
        GameObject planetObject = GameObject.Find(searchName);

        if (planetObject != null)
        {
            if (camScript != null) 
                camScript.SetTarget(planetObject.transform);
        }
        else
        {
            Debug.LogWarning("Tidak bisa spectate! Object '" + searchName + "' tidak ditemukan di Hierarchy.");
        }

        // Reset Scroll ke paling atas
        descriptionScroll.verticalNormalizedPosition = 1f;
        dropDownPanel.SetActive(false);
        detailPanel.SetActive(true);
    }

    public void ClosePlanetDetails()
    {
        detailPanel.SetActive(false);
        dropDownPanel.SetActive(true);

        if (camScript != null)
            camScript.ResetToMainView();
    }
}

[System.Serializable]
public class PlanetData
{
    public string name;
    public string radius;           
    public string rotationPeriod;   
    public string revolutionPeriod; 
    public string distance;         
    public string description;
}

[System.Serializable]
public class PlanetCollection
{
    public PlanetData[] planets;
}