using UnityEngine;

public class AsteroidRotate : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.one * Time.deltaTime * 10f); 
    }
}
