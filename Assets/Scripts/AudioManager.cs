using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    void Awake()
    {
        // Cek apakah sudah ada AudioManager lain di scene
        if (instance == null)
        {
            // Jika belum ada, jadikan ini yang utama
            instance = this;
            // JANGAN HANCURKAN objek ini saat pindah scene
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Jika sudah ada (misal balik lagi ke menu), hancurkan yang baru 
            // agar musik tidak dobel/tumpang tindih
            Destroy(gameObject);
        }
    }
}