using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace MyKitchenApp.Interfaces
{
    public interface ILoggingService
    {
        /// <summary>
        /// 
        /// </summary>
        event EventHandler<string> LogMessageReceived;

        /// <summary>
        /// Logt eine Nachricht, samt weiterer Informationen.
        /// </summary>
        /// <param name="message">Die Nachricht, welche geloggt werden soll.</param>
        /// <param name="properties">Zusätzliche Informationen für den Entwickler.</param>
        void LogSimpleDebug(string message,
            Dictionary<string, string> properties = null);

        /// <summary>
        /// Logt eine Nachricht in die Debug-Ausgabe.
        /// </summary>
        /// <param name="message">Die Nachricht, welche geloggt werden soll.</param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        void LogDebug(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logt eine Nachricht, samt weiterer Informationen.
        /// </summary>
        /// <param name="message">Die Nachricht, welche geloggt werden soll.</param>
        /// <param name="properties">Zusätzliche Informationen für den Entwickler.</param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        void LogDebug(string message,
            Dictionary<string, string> properties,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logt eine Nachricht, samt weiterer Informationen.
        /// </summary>
        /// <param name="message">Die Nachricht, welche geloggt werden soll.</param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        void LogMessage(string message,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logt eine Nachricht, samt weiterer Informationen.
        /// </summary>
        /// <param name="message">Die Nachricht, welche geloggt werden soll.</param>
        /// <param name="properties">Zusätzliche Informationen für den Entwickler.</param>
        /// <param name="memberName"></param>
        /// <param name="sourceFilePath"></param>
        /// <param name="sourceLineNumber"></param>
        void LogMessage(string message,
            Dictionary<string, string> properties,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        /// <summary>
        /// Logt eine Exception
        /// </summary>
        /// <param name="ex">Die Exception</param>
        void LogException(Exception ex);

        /// <summary>
        /// Logt eine Exception
        /// </summary>
        /// <param name="ex">Die Exception</param>
        /// <param name="properties">Zusätzliche Informationen für den Entwickler.</param>
        /// <param name="attachments">Zusätzliche Anhänge wie Dateien, etc.</param>
        void LogException(Exception ex,
            Dictionary<string, string> properties = null,
            params object[] attachments);

        /// <summary>
        /// Logt eine Exception
        /// </summary>
        /// <param name="ex">Die Exception</param>
        /// <param name="message">Die Nachricht, welche geloggt werden soll.</param>
        /// <param name="properties">Zusätzliche Informationen für den Entwickler.</param>
        /// <param name="attachments">Zusätzliche Anhänge wie Dateien, etc.</param>
        void LogException(Exception ex,
            string message = null,
            Dictionary<string, string> properties = null,
            params object[] attachments);

        /// <summary>
        /// Setzt den aktuellen Benutzer für die Logging-Session
        /// </summary>
        /// <param name="userId">Die Benutzer-ID</param>
        void SetLogUser(string userId);

        /// <summary>
        /// Gibt den Pfad zur lokalen Log-File zurück
        /// </summary>
        /// <returns></returns>
        string GetLogFilePath();

        /// <summary>
        /// Gibt TRUE zurück, wenn eine Log-Datei lokal angelegt wurden, andernfalls FALSE.
        /// </summary>
        /// <returns></returns>
        bool HasLogFile();

        /// <summary>
        /// Packt die Dateien der letzten Tage und gibt den Pfad zur ZIP-Datei zurück
        /// </summary>
        /// <returns></returns>
        string GetLogPackagePath();
    }
}