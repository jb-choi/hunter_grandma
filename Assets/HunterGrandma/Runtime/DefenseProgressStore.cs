using System;
using System.IO;
using UnityEngine;

namespace HunterGrandma
{
    public sealed class DefenseProgressStore
    {
        private readonly string path;
#if UNITY_EDITOR
        public static string EditorValidationPath;
#endif
        public static string DefaultPath
        {
            get
            {
#if UNITY_EDITOR
                if (!string.IsNullOrEmpty(EditorValidationPath)) return EditorValidationPath;
#endif
                return Path.Combine(Application.persistentDataPath, "HunterGrandma", "stage-progress-v1.json");
            }
        }
        public DefenseProgressStore(string path) { this.path = path; }
        public DefenseProgress Load()
        {
            if (!File.Exists(path)) return new DefenseProgress();
            try { return DefenseProgress.FromJson(File.ReadAllText(path)); }
            catch (Exception exception) when (exception is IOException || exception is ArgumentException || exception is FormatException)
            {
                Debug.LogWarning("Hunter Grandma save could not be loaded; trying the backup. " + exception.GetType().Name);
                if (File.Exists(path + ".bak"))
                {
                    try { return DefenseProgress.FromJson(File.ReadAllText(path + ".bak")); }
                    catch (Exception backupException) when (backupException is IOException || backupException is ArgumentException || backupException is FormatException) { }
                }
                File.Copy(path, path + ".invalid-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss"), true);
                return new DefenseProgress();
            }
        }
        public bool Save(DefenseProgress progress)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                string temporary = path + ".tmp";
                File.WriteAllText(temporary, progress.ToJson());
                // Validate before replacing a completed save. File.Replace keeps a recovery copy.
                DefenseProgress.FromJson(File.ReadAllText(temporary));
                if (File.Exists(path)) File.Replace(temporary, path, path + ".bak");
                else File.Move(temporary, path);
                return true;
            }
            catch (Exception exception) when (exception is IOException || exception is UnauthorizedAccessException || exception is FormatException)
            {
                Debug.LogWarning("Hunter Grandma progress could not be saved: " + exception.GetType().Name);
                return false;
            }
        }
    }
}
