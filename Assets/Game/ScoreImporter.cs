using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace FoxShooter.Game
{
    [CreateAssetMenu(menuName = "FoxShooter/ScoreImporter", fileName = "ScoreImporter")]
    public class ScoreImporter : ScriptableObject
    {
        private const string Filepath = "scores";
        [SerializeField] public List<LevelDefinition> levels;

        public void LoadScores()
        {
            var saveFile = Resources.Load<TextAsset>(Filepath);
            if (!saveFile)
            {
                return;
            }
            
            foreach (Match pair in Regex.Matches(saveFile.text, "\"(.*?)\"\\s(.*)"))
            {
                // The first group is actually our match here, so we want 3
                if (pair.Groups.Count != 3)
                {
                    continue;
                }
                
                if (float.TryParse(pair.Groups[2].Value, out var floatValue))
                {
                    SetScore(pair.Groups[1].Value, floatValue);
                }
            }
        }

        public void SaveScores()
        {
            var file = new StreamWriter($"Assets/Resources/{Filepath}.txt", false);
            foreach (var level in levels)
            {
                file.WriteLine(FormatSave(level));
            }
            file.Close();            
        }

        private void SetScore(string levelName, float score)
        {
            var level = GetLevelDefinition(levelName);
            if (!level)
            {
                return;
            }
            
            level.highScore = score;
        }

        private LevelDefinition GetLevelDefinition(string levelName)
        {
            return levels.FirstOrDefault(level => level.name == levelName);
        }

        private string FormatSave(LevelDefinition level)
        {
            return $"\"{level.displayName}\" : \"{level.highScore}\"";
        }
    }
}