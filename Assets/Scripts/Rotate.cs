using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Transform target;
<<<<<<< Updated upstream
    public float baseSpeed; // Ganti nama 'speed' jadi 'baseSpeed' agar jelas

    void Update()
    {
        // PENGAMAN: Cek apakah target ada
        if (target == null) return;

        // Ambil multiplier dari TimeManager
        // Jika TimeManager belum dibuat, default ke 1 agar tidak error
        float globalMultiplier = TimeManager.Instance != null ? TimeManager.Instance.currentMultiplier : 1f;

        // Rumus baru
        float step = baseSpeed * globalMultiplier * Time.deltaTime;

        // Eksekusi Rotasi
=======
    public float baseSpeed;

    void Update()
    {
        // Cek apakah target ada
        if (target == null) return;

        // Ambil multiplier dari TimeManager
        // Jika TimeManager tidak ada, default ke 1 agar tidak error
        float globalMultiplier = TimeManager.Instance != null ? TimeManager.Instance.currentMultiplier : 1f;

        // Rumus kecepatan
        float step = baseSpeed * globalMultiplier * Time.deltaTime;

>>>>>>> Stashed changes
        transform.RotateAround(target.transform.position, target.transform.up, step);
    }
}