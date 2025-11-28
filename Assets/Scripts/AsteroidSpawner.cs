using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Settings")]
    public GameObject asteroidPrefab;
    public Transform sunTarget;
    public int asteroidCount = 500;   // Jumlah asteroid

    [Header("Belt Dimensions")]
    public float minRadius = 20f;     // Jarak lingkar dalam (setelah Mars)
    public float maxRadius = 30f;     // Jarak lingkar luar (sebelum Jupiter)
    public float beltHeight = 1f;     // Ketebalan sabuk (atas-bawah)

    [Header("Movement")]
    public float minSpeed = 2f;       // Kecepatan orbit minimal
    public float maxSpeed = 5f;       // Kecepatan orbit maksimal

    void Start()
    {
        SpawnAsteroidBelt();
    }

    void SpawnAsteroidBelt()
    {
        for (int i = 0; i < asteroidCount; i++)
        {
            // Posisi acak dalam bentuk cincin (Donut shape)
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad; // Konversi derajat ke radian
            float distance = Random.Range(minRadius, maxRadius);  // Jarak acak antara batas dalam dan luar
            float height = Random.Range(-beltHeight, beltHeight); // Variasi naik

            float x = Mathf.Cos(angle) * distance;
            float z = Mathf.Sin(angle) * distance;
            Vector3 spawnPos = new Vector3(x, height, z);

            // Spawn Asteroid
            GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity, transform);

            // Variasi Ukuran
            float randomScale = Random.Range(0.5f, 1.5f);
            asteroid.transform.localScale *= randomScale;

            // Variasi Rotasi Diri
            asteroid.transform.rotation = Random.rotation;

            // Logic Revolusi dengan script rotate
            Rotate rotationScript = asteroid.AddComponent<Rotate>();
            
            rotationScript.target = sunTarget;
            
            // Kecepatan acak
            rotationScript.baseSpeed = Random.Range(minSpeed, maxSpeed); 
        }
    }
}