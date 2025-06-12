using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Vampire Survivors/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    [Header("Основные характеристики")]
    public string enemyName = "Враг";
    public GameObject enemyPrefab;

    [Header("Характеристики")]
    public float baseHealth = 100f;
    public float baseDamage = 10f;
    public float moveSpeed = 3f;

    [Header("Награды")]
    public int experienceReward = 1;
    public float dropChance = 0.1f;

    [Header("Визуал")]
    public Sprite enemyIcon;
    public Color enemyColor = Color.white;

    [Header("Описание")]
    [TextArea(2, 4)]
    public string description;
}

