using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleBehavior : Weapon
{
    [SerializeField] private float radius = 2f;

    private Transform _player;
    private Transform _independentPivot;
    private float currentAngle = 0f;

    private void Start()
    {
        _player = Player.singleton.transform;
        CreateIndependentPivot();
        PositionWeapon();
        base.Initialize();
    }

    private void CreateIndependentPivot()
    {
        // ������� ����������� ����� �������� ��� ������������ ����� � �������
        GameObject pivot = new GameObject("IndependentWeaponPivot");
        _independentPivot = pivot.transform;

        // �� ������ pivot �������� �������� ������!
        _independentPivot.position = _player.position;

        // ������ ������ �������� �������� ����������� ����� ��������
        transform.SetParent(_independentPivot);
    }

    private void PositionWeapon()
    {
        transform.localPosition = new Vector3(radius, 0, 0);
    }

    private void Update()
    {
        // ��������� ������� ����� ��������, ������ �� �������
        _independentPivot.position = _player.position;

        // ������� ����� �������� � ���������� ���������
        _independentPivot.Rotate(0, 0, speed * Time.deltaTime);

        // ���������� ������ �� ������
        Vector3 directionFromCenter = (transform.position - _player.position).normalized;
        transform.right = directionFromCenter;
    }
}
