using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class OrbitLine : MonoBehaviour
{
    [Header("Settings")]
    public Transform sunTarget;
    public int segments = 100;
    
    [Header("Visual")]
    public float lineWidth = 0.01f;

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        
        line.useWorldSpace = true;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;
        line.material = new Material(Shader.Find("Sprites/Default"));

        DrawOrbit();
    }

    void DrawOrbit()
    {
        if (sunTarget == null) return;

        // Hitung jarak planet ke matahari (Radius)
        float radius = Vector3.Distance(transform.position, sunTarget.position);

        // Buat titik-titik melingkar
        float x;
        float z;
        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            line.SetPosition(i, new Vector3(x, 0, z));

            angle += (360f / segments);
        }
    }
}