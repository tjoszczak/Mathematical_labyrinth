using System.Collections.Generic;
using System.IO;
using System.Windows;
using LabiryntMatematyczny.Models; 

namespace LabiryntMatematyczny.Services
{
    public class LevelManager
    {
        public List<Wall> Walls { get; private set; } = new List<Wall>();
        public Point StartPosition { get; private set; }
        public Rect EndZone { get; private set; }

        private int tileSize = 40; 
        private List<string> levelFiles;
        public int CurrentLevelIndex { get; private set; } = 0;

        public LevelManager()
        {
            levelFiles = new List<string>
            {
                "Assets/Levels/level1.txt",
                "Assets/Levels/level2.txt",
                "Assets/Levels/level3.txt",
                "Assets/Levels/level4.txt", 
                "Assets/Levels/level5.txt"  
            };
        }

        public bool LoadLevel(int levelIndex)
        {
            if (levelIndex < 0 || levelIndex >= levelFiles.Count)
            {
                return false;
            }

            CurrentLevelIndex = levelIndex;
            string filePath = levelFiles[levelIndex];

            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Nie znaleziono pliku poziomu: {filePath}");
                return false;
            }

            Walls.Clear();
            string[] lines = File.ReadAllLines(filePath);

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    char tile = lines[y][x];

                    if (tile == '#') // Ściana
                    {
                        Walls.Add(new Wall(x * tileSize, y * tileSize, tileSize));
                    }
                    else if (tile == 'S') // Start
                    {
                        StartPosition = new Point(x * tileSize, y * tileSize);
                    }
                    else if (tile == 'E') // End (Meta)
                    {
                        EndZone = new Rect(x * tileSize, y * tileSize, tileSize, tileSize);
                    }
                }
            }
            return true;
        }

        public bool LoadNextLevel()
        {
            return LoadLevel(CurrentLevelIndex + 1);
        }

        public bool IsLastLevel()
        {
            return CurrentLevelIndex >= levelFiles.Count - 1;
        }
    }
}