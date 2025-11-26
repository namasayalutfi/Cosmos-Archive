using UnityEngine;
using TMPro; // Jika ingin update text UI langsung dari sini

public class TimeManager : MonoBehaviour
{
    // Singleton pattern agar mudah diakses oleh script Rotate
    public static TimeManager Instance;

    [Header("Settings")]
    public float speedStep = 0.1f; // Penambahan 10% (0.1)
    public float minSpeed = 0f;    // Agar tidak mundur (negatif)
    public float maxSpeed = 5f;    // Batas maksimum kecepatan

    [Header("State")]
    public float currentMultiplier = 1.0f;
    private float savedSpeedBeforePause = 1.0f; // Menyimpan speed saat dipause
    private bool isPaused = false;

    // UI References (Optional, sesuai request UI kamu)
    public TextMeshProUGUI speedTextLabel; 

    private void Awake()
    {
        // Setup Singleton
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        UpdateUI();
    }

    // Fungsi untuk Tombol Speed Up
    public void SpeedUp()
    {
        if (isPaused) Unpause(); // Jika sedang pause, otomatis jalan lagi

        currentMultiplier += speedStep;
        
        // Clamp agar tidak melebihi batas
        currentMultiplier = Mathf.Clamp(currentMultiplier, minSpeed, maxSpeed);
        
        UpdateUI();
    }

    // Fungsi untuk Tombol Speed Down
    public void SpeedDown()
    {
        if (isPaused) Unpause();

        currentMultiplier -= speedStep;
        
        // Mencegah speed menjadi negatif
        if (currentMultiplier < minSpeed) currentMultiplier = minSpeed;

        UpdateUI();
    }

    // Fungsi untuk Tombol Pause
    public void TogglePause()
    {
        if (isPaused)
        {
            Unpause();
        }
        else
        {
            // Pause game
            isPaused = true;
            savedSpeedBeforePause = currentMultiplier; // Simpan speed terakhir
            currentMultiplier = 0; // Stop semua pergerakan
        }
        UpdateUI();
    }

    private void Unpause()
    {
        isPaused = false;
        // Kembalikan ke speed sebelum pause, atau default 1 jika error
        currentMultiplier = (savedSpeedBeforePause > 0) ? savedSpeedBeforePause : 1.0f;
    }

    private void UpdateUI()
    {
        if (speedTextLabel != null)
        {
            if (isPaused)
                speedTextLabel.text = "PAUSED";
            else
                speedTextLabel.text = $"Speed: {currentMultiplier:F1}x"; // Tampilkan 1 desimal (contoh: 1.2x)
        }
    }
}