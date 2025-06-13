using UnityEngine;

[CreateAssetMenu(fileName = "New Wave Data", menuName = "Vampire Survivors/Wave Data")]
public class WaveDataSO : ScriptableObject
{
    [Header("�������� ���������")]
    public string waveName = "�����";
    public int totalEnemies = 50;

    [Header("����� � �����")]
    public EnemyData[] possibleEnemies;

    [Header("��������")]
    [TextArea(3, 5)]
    public string description;
}

