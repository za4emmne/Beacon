using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Vampire Survivors/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    [Header("�������� ��������������")]
    public string enemyName = "����";
    public GameObject enemyPrefab;

    [Header("��������������")]
    public float baseHealth = 100f;
    public float baseDamage = 10f;
    public float moveSpeed = 3f;

    [Header("�������")]
    public int experienceReward = 1;
    public float dropChance = 0.1f;

    [Header("������")]
    public Sprite enemyIcon;
    public Color enemyColor = Color.white;

    [Header("��������")]
    [TextArea(2, 4)]
    public string description;
}

