using UnityEngine;
using UnityEngine.InputSystem;

public class SpectatorCamera : MonoBehaviour
{
    [Header("Targeting")]
    public Transform currentTarget; // Planet yang sedang dilihat
    public Vector3 defaultPosition; // Posisi awal kamera (Wide shot)
    public Quaternion defaultRotation; // Rotasi awal kamera

    [Header("Settings")]
    public float sensitivity = 0.3f;
    public float zoomSpeed = 1f; 
    public float maxZoom = 10.0f;

    [Header("Dynamic Zoom Settings")]
    public float minZoomMultiplier = 1.5f; // Jarak minimal = 1.5x Radius Planet, mencegah clipping
    private float calculatedMinZoom = 0.5f; // Variable internal untuk menyimpan hasil hitungan

    [Header("Free Roam Settings")]
    public float panSpeed = 30f;       // Kecepatan gerak WASD
    public float scrollZoomSpeed = 50f; // Kecepatan zoom scroll saat free roam
    public float lookSpeed = 0.2f;

    // Batas Wilayah
    public float boundaryX = 350f;     // Batas kiri-kanan
    public float boundaryZ = 250f;  // Batas atas-bawah
    public float minY = -2;        // Zoom paling dekat ke matahari (Tinggi kamera)
    public float maxY = 200f;

    // Internal variables untuk rotasi
    private float distance = 5.0f;
    private float currentX = 0.0f;
    private float currentY = 0.0f;
    private bool isSpectating = false;

    private float freeRoamYaw = 0.0f;
    private float freeRoamPitch = 60.0f;

    void Start()
    {
        // Simpan posisi awal Main Camera saat game dimulai
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;

        freeRoamPitch = transform.eulerAngles.x;
        freeRoamYaw = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        if (isSpectating && currentTarget != null)
        {
            HandleSpectateMovement();
        }
        else
        {
            HandleFreeRoamMovement();
        }
    }

    // SPECTATE MODE
    void HandleSpectateMovement()
    {
        if (Mouse.current == null) return;

        // Input Mouse Klik Kiri/Kanan untuk putar kamera
        if (Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            
            currentX += mouseDelta.x * sensitivity;
            currentY -= mouseDelta.y * sensitivity;
            
            currentY = Mathf.Clamp(currentY, -80, 80);
        }

        // Input Scroll untuk Zoom
        float scroll = Mouse.current.scroll.ReadValue().y * 0.1f;
        distance -= scroll * zoomSpeed;
        distance = Mathf.Clamp(distance, calculatedMinZoom, maxZoom);

        // Hitung Posisi & Rotasi
        // Hitung posisi relatif terhadap Target (Planet)
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
        
        // Rumus Orbit: Posisi Planet + (Rotasi * Jarak Mundur)
        Vector3 position = rotation * negDistance + currentTarget.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    // FREEROAM
    void HandleFreeRoamMovement()
    {
        // Cek device keyboard & mouse untuk menghindari error
        if (Keyboard.current == null || Mouse.current == null) return;

        // Rotasi kamera (Tahan Klik Kanan)
        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 mouseDelta = Mouse.current.delta.ReadValue();
            
            freeRoamYaw += mouseDelta.x * lookSpeed;
            freeRoamPitch -= mouseDelta.y * lookSpeed;
            
            // Batasi pitch agar tidak jungkir balik
            freeRoamPitch = Mathf.Clamp(freeRoamPitch, 10f, 85f);

            // Terapkan rotasi langsung
            transform.rotation = Quaternion.Euler(freeRoamPitch, freeRoamYaw, 0);
        }

        Vector3 pos = transform.position;
        float dt = Time.deltaTime;

        Vector3 moveInput = Vector3.zero;

        // Gerakan Panning (WASD / Panah)
        if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
            moveInput.z += 1;
        
        if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
            moveInput.z -= 1;
        
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            moveInput.x += 1;
        
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            moveInput.x -= 1;

        // Q (Turun) dan E (Naik) ---
        if (Keyboard.current.eKey.isPressed) 
        {
            pos.y += panSpeed * dt; // Naik
        }
        if (Keyboard.current.qKey.isPressed) 
        {
            pos.y -= panSpeed * dt; // Turun
        }

        // Konversi input WASD menjadi arah relatif terhadap hadap kamera
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // "Ratakan" vektor ke tanah (Y=0) agar kamera tidak 'terbang' ke atas saat tekan W
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = forward * moveInput.z + right * moveInput.x;
        pos += moveDir * panSpeed * dt;

        // Zoom
        float scroll = Mouse.current.scroll.ReadValue().y;
        
        pos.y -= scroll * scrollZoomSpeed * dt; 

        // Batasan (Clamping)
        pos.x = Mathf.Clamp(pos.x, -boundaryX, boundaryX); // Batas Kiri-Kanan Map
        pos.z = Mathf.Clamp(pos.z, -boundaryZ, boundaryZ); // Batas Atas-Bawah Map
        pos.y = Mathf.Clamp(pos.y, minY, maxY);            // Batas Tinggi Kamera

        // Terapkan posisi baru
        transform.position = pos;
    }

    // Fungsi dipanggil saat tombol Planet ditekan
    public void SetTarget(Transform newTarget)
    {
        currentTarget = newTarget;
        isSpectating = true;

        Renderer planetRenderer = newTarget.GetComponent<Renderer>();
        float boundRadius = 0.5f; // Default jika gagal

        if (planetRenderer != null)
        {
            // bounds.extents.magnitude adalah jarak dari pusat ke pinggir terluar
            boundRadius = planetRenderer.bounds.extents.magnitude;
        }
        else
        {
            // Fallback: Gunakan scale (Asumsi bola diameter 1)
            boundRadius = newTarget.localScale.x * 0.5f;
        }

        float cameraClipPlane = Camera.main.nearClipPlane;
        float zoomByMultiplier = boundRadius * minZoomMultiplier;

        float visualRadius = newTarget.localScale.x * 0.5f; 
        float safeDistance = visualRadius + cameraClipPlane + 0.1f;

        calculatedMinZoom = Mathf.Max(zoomByMultiplier, safeDistance);
        distance = calculatedMinZoom * 3.0f;
        
        // Reset sudut kamera agar pas di belakang/samping planet
        currentX = 0; 
        currentY = 20; 
    }

    // Fungsi dipanggil saat tombol X (Close) ditekan
    public void ResetToMainView()
    {
        isSpectating = false;
        currentTarget = null;
        
        // Kembalikan ke posisi awal tata surya
        // transform.position = defaultPosition;
        // transform.rotation = defaultRotation;

        // Reset rotasi ke sudut pandang
        freeRoamPitch = transform.eulerAngles.x;
        freeRoamYaw = transform.eulerAngles.y;
    }
}