Imports System.IO
Imports System.Text

Public Class Logger
    Private Shared _instance As Logger
    Private ReadOnly _logFilePath As String
    Private ReadOnly _logLock As New Object()

    Private Sub New()
        Try
            ' Create logs directory in application folder
            Dim appDirectory As String = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
            Dim logsDirectory As String = Path.Combine(appDirectory, "Logs")

            If Not Directory.Exists(logsDirectory) Then
                Directory.CreateDirectory(logsDirectory)
            End If

            ' Create log file with date
            Dim logFileName As String = $"FTPSync_{DateTime.Now:yyyy-MM-dd}.log"
            _logFilePath = Path.Combine(logsDirectory, logFileName)
        Catch ex As Exception
            ' Fallback to temp directory if app directory fails
            _logFilePath = Path.Combine(Path.GetTempPath(), $"FTPSync_{DateTime.Now:yyyy-MM-dd}.log")
        End Try
    End Sub

    Public Shared ReadOnly Property Instance As Logger
        Get
            If _instance Is Nothing Then
                _instance = New Logger()
            End If
            Return _instance
        End Get
    End Property

    Private Sub WriteLog(level As String, message As String, Optional ex As Exception = Nothing)
        Try
            SyncLock _logLock
                Dim logEntry As New StringBuilder()
                logEntry.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] [{level}] {message}")

                If ex IsNot Nothing Then
                    logEntry.AppendLine($"Exception: {ex.GetType().Name}")
                    logEntry.AppendLine($"Message: {ex.Message}")
                    logEntry.AppendLine($"Stack Trace: {ex.StackTrace}")

                    If ex.InnerException IsNot Nothing Then
                        logEntry.AppendLine($"Inner Exception: {ex.InnerException.GetType().Name}")
                        logEntry.AppendLine($"Inner Message: {ex.InnerException.Message}")
                    End If
                End If

                File.AppendAllText(_logFilePath, logEntry.ToString())
            End SyncLock
        Catch
            ' Silently fail if logging fails to avoid infinite loops
        End Try
    End Sub

    Public Sub Debug(message As String, Optional ex As Exception = Nothing)
        WriteLog("DEBUG", message, ex)
    End Sub

    Public Sub Info(message As String, Optional ex As Exception = Nothing)
        WriteLog("INFO", message, ex)
    End Sub

    Public Sub Warning(message As String, Optional ex As Exception = Nothing)
        WriteLog("WARNING", message, ex)
    End Sub

    Public Sub [Error](message As String, Optional ex As Exception = Nothing)
        WriteLog("ERROR", message, ex)
    End Sub

    Public Sub Critical(message As String, Optional ex As Exception = Nothing)
        WriteLog("CRITICAL", message, ex)
    End Sub

    Public Function GetLogFilePath() As String
        Return _logFilePath
    End Function

    Public Function GetRecentLogs(maxLines As Integer) As String()
        Try
            SyncLock _logLock
                If File.Exists(_logFilePath) Then
                    Dim allLines As String() = File.ReadAllLines(_logFilePath)
                    If allLines.Length <= maxLines Then
                        Return allLines
                    Else
                        Dim recentLines(maxLines - 1) As String
                        Array.Copy(allLines, allLines.Length - maxLines, recentLines, 0, maxLines)
                        Return recentLines
                    End If
                Else
                    Return {}
                End If
            End SyncLock
        Catch ex As Exception
            Return {$"Error reading log file: {ex.Message}"}
        End Try
    End Function

    Public Sub ClearOldLogs(daysToKeep As Integer)
        Try
            Dim logsDirectory As String = Path.GetDirectoryName(_logFilePath)
            If Directory.Exists(logsDirectory) Then
                Dim cutoffDate As DateTime = DateTime.Now.AddDays(-daysToKeep)

                For Each logFile As String In Directory.GetFiles(logsDirectory, "FTPSync_*.log")
                    If File.GetCreationTime(logFile) < cutoffDate Then
                        File.Delete(logFile)
                        Info($"Deleted old log file: {Path.GetFileName(logFile)}")
                    End If
                Next
            End If
        Catch ex As Exception
            Warning("Failed to clean old log files", ex)
        End Try
    End Sub
End Class