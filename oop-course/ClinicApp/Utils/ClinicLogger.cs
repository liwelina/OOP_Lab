using System.Text;

namespace ClinicApp.Utils
{
    public class ClinicLogger
    {
        private readonly string _logPath;

        public ClinicLogger(string logPath = "clinic.log")
        {
            _logPath = logPath;
        }

        public void LogInfo(string message)
        {
            WriteLog("INFO ", message);
        }

        public void LogWarning(string message)
        {
            WriteLog("WARN ", message);
        }

        public void LogError(string message)
        {
            WriteLog("ERROR", message);
        }

        public string[] GetLastLines(int n)
        {
            if (n <= 0 || !Exists())
            {
                return Array.Empty<string>();
            }

            string[] lines = File.ReadAllLines(_logPath, Encoding.UTF8);
            int startIndex = lines.Length - n;
            if (startIndex < 0)
            {
                startIndex = 0;
            }

            int resultLength = lines.Length - startIndex;
            string[] result = new string[resultLength];
            for (int i = 0; i < resultLength; i++)
            {
                result[i] = lines[startIndex + i];
            }

            return result;
        }

        public void Clear()
        {
            if (Exists())
            {
                File.Delete(_logPath);
            }
        }

        public bool Exists()
        {
            return File.Exists(_logPath);
        }

        private void WriteLog(string level, string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string line = $"[{timestamp}] [{level}] {message}{Environment.NewLine}";
            File.AppendAllText(_logPath, line, Encoding.UTF8);
        }
    }
}