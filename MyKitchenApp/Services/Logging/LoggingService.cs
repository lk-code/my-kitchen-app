using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Configuration;
using MyKitchenApp.Interfaces;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace MyKitchenApp.Services.Logging
{
    public class LoggingService : ILoggingService, IInitialize
    {
        #region Konstanten

        /// <summary>
        /// Anzahl der Tage, die die Log-Dateien gespeichert werden sollen.
        /// </summary>
        private const int MAX_DAYS = 7;
        /// <summary>
        /// Anzahl der Tage, die die Log-Dateien gespeichert werden sollen.
        /// </summary>
        private const string LOG_SEPERATOR = "------------------------------";
        /// <summary>
        /// 
        /// </summary>
        private const string LOG_FILE_NAME = "app-log_{0}.log";
        /// <summary>
        /// 
        /// </summary>
        private const string LOG_FILE_REGEX = @"^app-log_[0-9]{2}-[0-9]{2}-[0-9]{4}\.log$";
        /// <summary>
        /// 
        /// </summary>
        private const string LOG_ARCHIVE = "app-log.zip";

        #endregion

        #region Events

        #endregion

        #region Private Elemente

        public event EventHandler<string> LogMessageReceived;

        private readonly IConfiguration _configuration;

        private readonly string _logFileName;
        private bool _isDebug = false;
        private StreamWriter _logStreamWriter = null;

        #endregion

        #region Properties

        #endregion

        #region Konstruktoren

        public LoggingService(IConfiguration configuration)
        {
            this._configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            this._logFileName = string.Format(LOG_FILE_NAME, DateTime.Now.ToString("MM-dd-yyyy"));
        }

        ~LoggingService()
        {
            this._logStreamWriter.Close();
            this._logStreamWriter.Dispose();
            this._logStreamWriter = null;
        }

        #endregion

        #region Worker

        public void Initialize()
        {
#if DEBUG
            this._isDebug = true;
#endif

            this.InitializeAppCenter();
            this.InitializeLogFile();
        }

        #region AppCenter

        /// <summary>
        /// Setzt den aktuellen Benutzer für die Logging-Session
        /// </summary>
        /// <param name="userId">Die Benutzer-ID</param>
        public void SetLogUser(string userId)
        {
            AppCenter.SetUserId(userId);
        }

        /// <summary>
        /// Initialisiert CppCenter
        /// </summary>
        private void InitializeAppCenter()
        {
            string appCenterId = this.GetAppCenterId();

            AppCenter.Start(appCenterId,
                typeof(Analytics),
                typeof(Crashes));
        }

        /// <summary>
        /// Gibt die AppCenter-Secret zurück.
        /// </summary>
        /// <returns></returns>
        private string GetAppCenterId()
        {
            Settings.AppCenter appCenterSettings = this._configuration.GetRequiredSection("AppCenter").Get<Settings.AppCenter>();

            if (!string.IsNullOrEmpty(appCenterSettings.Secret))
            {
                return appCenterSettings.Secret;
            }

            // build appcenter secrets string
            StringBuilder stringBuilder = new StringBuilder();
            if (!string.IsNullOrEmpty(appCenterSettings.UWP))
            {
                stringBuilder.AppendFormat("uwp={0};", appCenterSettings.UWP);
            }
            if (!string.IsNullOrEmpty(appCenterSettings.iOS))
            {
                stringBuilder.AppendFormat("ios={0};", appCenterSettings.iOS);
            }
            if (!string.IsNullOrEmpty(appCenterSettings.macOS))
            {
                stringBuilder.AppendFormat("macos={0};", appCenterSettings.macOS);
            }
            if (!string.IsNullOrEmpty(appCenterSettings.Android))
            {
                stringBuilder.AppendFormat("android={0};", appCenterSettings.Android);
            }

            string appCenterSecret = stringBuilder.ToString();
            return appCenterSecret;
        }

        #endregion

        #region Log File

        /// <summary>
        /// Gibt den Pfad zur lokalen Log-File zurück
        /// </summary>
        /// <returns></returns>
        public string GetLogFilePath()
        {
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), this._logFileName);

            return fileName;
        }

        /// <summary>
        /// Gibt TRUE zurück, wenn eine Log-Datei lokal angelegt wurden, andernfalls FALSE.
        /// </summary>
        /// <returns></returns>
        public bool HasLogFile()
        {
            string fileName = this.GetLogFilePath();

            return File.Exists(fileName);
        }

        private void InitializeLogFile()
        {
            // clean up
            string[] fileEntries = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));

            List<string> logFiles = new List<string>();
            foreach (string fileEntry in fileEntries)
            {
                if (this.IsLogFile(fileEntry))
                {
                    logFiles.Add(Path.GetFileName(fileEntry));
                }
            }

            // Aktuelle Log-Dateinamen der letzten Tage
            List<string> currentLogFileNames = new List<string>();
            for (int i = 0; i < MAX_DAYS; i++)
            {
                DateTime fileDateTime = DateTime.Now.AddDays((i * -1));
                string file = string.Format(LOG_FILE_NAME, fileDateTime.ToString("MM-dd-yyyy"));

                currentLogFileNames.Add(file);
            }

            foreach (string logFileName in logFiles)
            {
                if (currentLogFileNames.Contains(logFileName) != true)
                {
                    // delete file
                    string fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), logFileName);
                    File.Delete(fullPath);
                }
            }

            // create todays log file
            this._logStreamWriter = File.AppendText(this.GetLogFilePath());
        }

        /// <summary>
        /// Gibt TRUE zurück, wenn es sich bei dem Datei-Pfad um eine Log-Datei handelt.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private bool IsLogFile(string file)
        {
            Regex regex = new Regex(LOG_FILE_REGEX);
            string fileName = Path.GetFileName(file);

            return regex.IsMatch(fileName);
        }

        /// <summary>
        /// Packt die Dateien der letzten Tage und gibt den Pfad zur ZIP-Datei zurück
        /// </summary>
        /// <returns></returns>
        public string GetLogPackagePath()
        {
            string archiveName = string.Empty;

            try
            {
                archiveName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), LOG_ARCHIVE);
                File.Delete(archiveName);

                using (ZipArchive zip = ZipFile.Open(archiveName, ZipArchiveMode.Create))
                {
                    string[] fileEntries = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
                    foreach (string fileEntry in fileEntries)
                    {
                        if (this.IsLogFile(fileEntry) == true)
                        {
                            zip.CreateEntryFromFile(fileEntry, Path.GetFileName(fileEntry), CompressionLevel.Optimal);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this.LogException(exception);
            }

            return archiveName;
        }

        #endregion

        public void LogMessage(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.LogMessage(message,
                null,
                memberName,
                sourceFilePath,
                sourceLineNumber);
        }

        public void LogMessage(string message,
            Dictionary<string, string> properties,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.Log(message,
                null,
                memberName,
                sourceFilePath,
                sourceLineNumber,
                true);
        }

        /// <summary>
        /// Logt eine Exception
        /// </summary>
        /// <param name="ex">Die Exception</param>
        public void LogException(Exception ex)
        {
            this.LogException(ex, null);
        }

        /// <summary>
        /// Logt eine Exception
        /// </summary>
        /// <param name="ex">Die Exception</param>
        /// <param name="message">Die Nachricht, welche geloggt werden soll.</param>
        /// <param name="properties">Zusätzliche Informationen für den Entwickler.</param>
        /// <param name="attachments">Zusätzliche Anhänge wie Dateien, etc.</param>
        public void LogException(Exception ex,
            string message = null,
            Dictionary<string, string> properties = null,
            params object[] attachments)
        {
            if (!string.IsNullOrEmpty(message)
                && !string.IsNullOrWhiteSpace(message))
            {
                if (properties == null)
                {
                    properties = new Dictionary<string, string>();
                }

                properties.Add("development_message", message);
            }

            this.LogException(ex, properties, attachments);
        }

        /// <summary>
        /// Logt eine Exception
        /// </summary>
        /// <param name="ex">Die Exception</param>
        /// <param name="properties">Zusätzliche Informationen für den Entwickler.</param>
        /// <param name="attachments">Zusätzliche Anhänge wie Dateien, etc.</param>
        public void LogException(Exception ex,
            Dictionary<string, string> properties = null,
            params object[] attachments)
        {
            if (this._isDebug != true)
            {
                Crashes.TrackError(ex, properties, (attachments as ErrorAttachmentLog[]));
            }

            this.Log(ex.ToString(),
                properties,
                sendToAppCenter: true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public void LogDebug(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.LogDebug(message,
                null,
                memberName,
                sourceFilePath,
                sourceLineNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        public void LogDebug(string message,
            Dictionary<string, string> properties,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            this.Log(message,
                properties,
                memberName,
                sourceFilePath,
                sourceLineNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        public void LogSimpleDebug(string message,
            Dictionary<string, string> properties = null)
        {
            this.Log(message,
                properties);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        /// <param name="sendToAppCenter"></param>
        private void Log(string message,
            Dictionary<string, string> properties = null,
            string memberName = "",
            string sourceFilePath = "",
            int sourceLineNumber = 0,
            bool sendToAppCenter = false)
        {
            if (properties == null)
            {
                properties = new Dictionary<string, string>();
            }

            if (!string.IsNullOrEmpty(memberName.Trim()))
                properties.Add("_memberName", memberName);
            if (!string.IsNullOrEmpty(sourceFilePath.Trim()))
                properties.Add("_sourceFilePath", sourceFilePath);
            if (sourceLineNumber > 0)
                properties.Add("_sourceLineNumber", sourceLineNumber.ToString());

            this.GenerateAndWriteMessage(message,
                properties,
                sendToAppCenter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        /// <param name="sendToAppCenter"></param>
        private void GenerateAndWriteMessage(string message,
            Dictionary<string, string> properties,
            bool sendToAppCenter = false)
        {
            if (this._isDebug != true
                && sendToAppCenter == true)
            {
                Analytics.TrackEvent(message, properties);
            }

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(message);
            if (properties.Count > 0)
            {
                //stringBuilder.AppendLine(">");
                foreach (var property in properties)
                {
                    stringBuilder.AppendLine($"{property.Key}:\t{property.Value}");
                }
            }

            this.WriteDebug(stringBuilder.ToString());
        }

        /// <summary>
        /// Logt eine Nachricht in die Debug-Ausgabe.
        /// </summary>
        /// <param name="message"></param>
        private void WriteDebug(string message)
        {
            if (this._isDebug == true)
            {
                Debug.WriteLine($"DEBUG:\t{message}");
            }
            this.LogMessageReceived?.Invoke(this, message);

            this._logStreamWriter.Write($"{DateTime.UtcNow.ToLongTimeString()}\t{message}");
            this._logStreamWriter.WriteLine(LOG_SEPERATOR);

            try
            {
                this._logStreamWriter.Flush();
            }
            catch (Exception)
            {

            }
        }

        #endregion
    }
}
