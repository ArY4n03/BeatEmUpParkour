using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController target;
    [SerializeField] private float distance = 3f;
    [SerializeField] private Vector2 FramingPoint;
    private float yRotation = 0f;
    private float xRotation = 0f;
    private Vector3 focusPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xRotation -= target.lookValue.y;
        yRotation += target.lookValue.x;

        xRotation = Mathf.Clamp(xRotation, -15f, 15f);
        focusPoint = target.transform.position + new Vector3(FramingPoint.x, FramingPoint.y, 0);
        transform.position = focusPoint - Quaternion.Euler(xRotation, yRotation, 0) * new Vector3(0, 0, distance);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

    }

    public Quaternion planerRotation => Quaternion.Euler(0, yRotation, 0);
}
