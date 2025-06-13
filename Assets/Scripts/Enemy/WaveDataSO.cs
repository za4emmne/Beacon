using UnityEngine;

[CreateAssetMenu(fileName = "New Wave Data", menuName = "Vampire Survivors/Wave Data")]
public class WaveDataSO : ScriptableObject
{
    [Header("Основные настройки")]
    public string waveName = "Волна";
    public int totalEnemies = 50;

    [Header("Враги в волне")]
    public EnemyData[] possibleEnemies;

    [Header("Описание")]
    [TextArea(3, 5)]
    public string description;
}

