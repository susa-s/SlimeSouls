using UnityEngine;

public class RespawnCube : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;
    [SerializeField] private float rotationSpeed = 1.5f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
        float newYRotation = Time.time * rotationSpeed;
        transform.rotation *= Quaternion.Euler(0, newYRotation, 0);
    }
}
