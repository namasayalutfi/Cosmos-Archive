using UnityEngine;

public class PlanetData : MonoBehaviour
{
    [Header("Informasi Dasar")]
    public string planetName;
    public float radius;
    [TextArea(3, 5)]
    public string description;

    [Header("Data Pergerakan")]
    public string rotationSpeed;
    public string revolutionSpeed;
    public float distanceFromSun;

}