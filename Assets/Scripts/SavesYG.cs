using System.Collections.Generic;

namespace YG
{
    public partial class SavesYG
    {
        public int bestScore;
        public int bestLevel;
        public int coins;
        public float bestTime;
        public int totalKill;
        public float totalTime;

        public List<string> unlockedCharacters = new List<string>(); // Список ключей купленных персонажей
        public string selectedCharacter; // Ключ выбранного персонажа
    }
}
