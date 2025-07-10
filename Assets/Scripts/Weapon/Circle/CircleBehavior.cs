using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBehavior : Weapon
{
    [Header("Orbit Settings")]
    [SerializeField] private float orbitRadius = 0.1f;
    [SerializeField] private float orbitSpeed = 90f; // ������� � �������
    [SerializeField] private bool clockwise = true;

    [Header("Starting Position")]
    [SerializeField] private float startAngle = 0f;

    private float currentAngle;

    private void Start()
    {
        base.Initialize();
    }

    private void Update()
    {
        Vector3 offset = new Vector3(
            Mathf.Cos(currentAngle * Mathf.Deg2Rad) * orbitRadius,
            Mathf.Sin(currentAngle * Mathf.Deg2Rad) * orbitRadius,
            0f
        );

        // ������������� ������� ������������ ������
        transform.position = player.transform.position + offset;

        // ��������� ���� ��� ���������� �����
        float direction = clockwise ? -1f : 1f;
        currentAngle += orbitSpeed * direction * Time.deltaTime;

        // ����������� ���� � �������� 0-360
        if (currentAngle >= 360f) currentAngle -= 360f;
        if (currentAngle < 0f) currentAngle += 360f;
    }
}
