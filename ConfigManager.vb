Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Runtime.InteropServices

Public Class ConfigManager
    Private Const CONFIG_FILE As String = "config.ini"
    Private Const ENCRYPTION_KEY As String = "FTPSync2025Key!@#"
    Private configPath As String
    Private configData As Dictionary(Of String, Dictionary(Of String, String))

    ' Windows API declarations for INI file handling
    <DllImport("kernel32.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function WritePrivateProfileString(
        lpApplicationName As String,
        lpKeyName As String,
        lpString As String,
        lpFileName As String) As Boolean
    End Function

    <DllImport("kernel32.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function GetPrivateProfileString(
        lpApplicationName As String,
        lpKeyName As String,
        lpDefault As String,
        lpReturnedString As StringBuilder,
        nSize As Integer,
        lpFileName As String) As Integer
    End Function

    Public Sub New()
        Try
            ' Get the application executable directory
            Dim appDirectory As String = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
            configPath = Path.Combine(appDirectory, CONFIG_FILE)
            configData = New Dictionary(Of String, Dictionary(Of String, String))()
            LoadConfiguration()
        Catch ex As Exception
            ' If initialization fails, use temp directory
            configPath = Path.Combine(Path.GetTempPath(), CONFIG_FILE)
            configData = New Dictionary(Of String, Dictionary(Of String, String))()
        End Try
    End Sub

    Public Sub LoadConfiguration()
        Try
            configData.Clear()

            If Not File.Exists(configPath) Then
                ' Create default configuration file if it doesn't exist
                CreateDefaultConfig()
                Return
            End If

            ' Load all sections and keys from INI file
            LoadSection("FTP")
            LoadSection("Sync")
        Catch ex As Exception
            ' If loading fails, create a new default config
            CreateDefaultConfig()
        End Try
    End Sub

    Private Sub LoadSection(sectionName As String)
        Try
            If String.IsNullOrEmpty(sectionName) Then
                Return
            End If

            If Not configData.ContainsKey(sectionName) Then
                configData(sectionName) = New Dictionary(Of String, String)()
            End If

            ' Get all key names in the section
            Dim keys As String() = GetSectionKeys(sectionName)

            For Each key As String In keys
                If Not String.IsNullOrEmpty(key) Then
                    Dim value As String = GetValue(sectionName, key, "")
                    configData(sectionName)(key) = value
                End If
            Next
        Catch ex As Exception
            ' Log error but continue
            Console.WriteLine($"Error loading section {sectionName}: {ex.Message}")
        End Try
    End Sub

    Private Function GetSectionKeys(sectionName As String) As String()
        Try
            ' This is a simplified version - in a full implementation,
            ' you would read all keys from the section
            Select Case sectionName.ToUpper()
                Case "FTP"
                    Return {"Server", "Username", "Password"}
                Case "SYNC"
                    Return {"LocalFolder", "RemoteFolder", "Direction"}
                Case Else
                    Return {}
            End Select
        Catch
            Return {}
        End Try
    End Function

    Private Sub CreateDefaultConfig()
        Try
            configData.Clear()

            ' FTP Section
            configData("FTP") = New Dictionary(Of String, String) From {
                {"Server", ""},
                {"Username", ""},
                {"Password", ""}
            }

            ' Sync Section
            configData("Sync") = New Dictionary(Of String, String) From {
                {"LocalFolder", ""},
                {"RemoteFolder", ""},
                {"Direction", "LocalToRemote"}
            }

            SaveConfiguration()
        Catch ex As Exception
            Throw New Exception($"Failed to create default configuration: {ex.Message}", ex)
        End Try
    End Sub

    Public Function GetValue(section As String, key As String, defaultValue As String) As String
        Try
            If String.IsNullOrEmpty(section) OrElse String.IsNullOrEmpty(key) Then
                Return defaultValue
            End If

            Dim buffer As New StringBuilder(256)
            Dim result As Integer = GetPrivateProfileString(section, key, defaultValue, buffer, buffer.Capacity, configPath)

            If result > 0 Then
                Return buffer.ToString().Substring(0, result)
            Else
                Return defaultValue
            End If
        Catch
            Return defaultValue
        End Try
    End Function

    Public Sub SetValue(section As String, key As String, value As String)
        Try
            If String.IsNullOrEmpty(section) OrElse String.IsNullOrEmpty(key) Then
                Return
            End If

            If value Is Nothing Then
                value = ""
            End If

            ' Update in-memory dictionary
            If Not configData.ContainsKey(section) Then
                configData(section) = New Dictionary(Of String, String)()
            End If
            configData(section)(key) = value

            ' Write to INI file
            WritePrivateProfileString(section, key, value, configPath)
        Catch ex As Exception
            Throw New Exception($"Error setting configuration value [{section}].[{key}]: {ex.Message}", ex)
        End Try
    End Sub

    Public Function GetDecryptedValue(section As String, key As String, defaultValue As String) As String
        Try
            If String.IsNullOrEmpty(section) OrElse String.IsNullOrEmpty(key) Then
                Return defaultValue
            End If

            Dim encryptedValue As String = GetValue(section, key, "")
            If String.IsNullOrEmpty(encryptedValue) Then
                Return defaultValue
            End If

            Return DecryptString(encryptedValue)
        Catch
            Return defaultValue
        End Try
    End Function

    Public Sub SetEncryptedValue(section As String, key As String, value As String)
        Try
            If String.IsNullOrEmpty(section) OrElse String.IsNullOrEmpty(key) Then
                Return
            End If

            If value Is Nothing Then
                value = ""
            End If

            Dim encryptedValue As String = EncryptString(value)
            SetValue(section, key, encryptedValue)
        Catch ex As Exception
            Throw New Exception($"Error setting encrypted configuration value [{section}].[{key}]: {ex.Message}", ex)
        End Try
    End Sub

    Private Function EncryptString(plainText As String) As String
        If String.IsNullOrEmpty(plainText) Then
            Return ""
        End If

        Try
            Dim key() As Byte = Encoding.UTF8.GetBytes(ENCRYPTION_KEY.PadRight(32).Substring(0, 32))
            Dim iv() As Byte = Encoding.UTF8.GetBytes("1234567890123456") ' Simple IV for demo

            Using aes As Aes = Aes.Create()
                aes.Key = key
                aes.IV = iv
                aes.Mode = CipherMode.CBC
                aes.Padding = PaddingMode.PKCS7

                Using encryptor As ICryptoTransform = aes.CreateEncryptor()
                    Dim plainBytes() As Byte = Encoding.UTF8.GetBytes(plainText)
                    Dim encryptedBytes() As Byte = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length)
                    Return Convert.ToBase64String(encryptedBytes)
                End Using
            End Using
        Catch ex As Exception
            ' If encryption fails, return original text (not recommended for production)
            Console.WriteLine($"Encryption failed: {ex.Message}")
            Return plainText
        End Try
    End Function

    Private Function DecryptString(encryptedText As String) As String
        If String.IsNullOrEmpty(encryptedText) Then
            Return ""
        End If

        Try
            Dim key() As Byte = Encoding.UTF8.GetBytes(ENCRYPTION_KEY.PadRight(32).Substring(0, 32))
            Dim iv() As Byte = Encoding.UTF8.GetBytes("1234567890123456") ' Same IV used for encryption

            Using aes As Aes = Aes.Create()
                aes.Key = key
                aes.IV = iv
                aes.Mode = CipherMode.CBC
                aes.Padding = PaddingMode.PKCS7

                Using decryptor As ICryptoTransform = aes.CreateDecryptor()
                    Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedText)
                    Dim decryptedBytes() As Byte = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length)
                    Return Encoding.UTF8.GetString(decryptedBytes)
                End Using
            End Using
        Catch ex As Exception
            ' If decryption fails, return encrypted text
            Console.WriteLine($"Decryption failed: {ex.Message}")
            Return encryptedText
        End Try
    End Function

    Public Sub SaveConfiguration()
        Try
            ' Ensure the configuration file exists
            If Not File.Exists(configPath) Then
                Dim dir As String = Path.GetDirectoryName(configPath)
                If Not Directory.Exists(dir) Then
                    Directory.CreateDirectory(dir)
                End If
                File.Create(configPath).Close()
            End If

            ' Write all configuration data to INI file
            For Each section In configData
                For Each kvp In section.Value
                    WritePrivateProfileString(section.Key, kvp.Key, kvp.Value, configPath)
                Next
            Next
        Catch ex As Exception
            Throw New Exception($"Error saving configuration to {configPath}: {ex.Message}", ex)
        End Try
    End Sub

    Public Function ConfigFileExists() As Boolean
        Try
            Return File.Exists(configPath)
        Catch
            Return False
        End Try
    End Function

    Public Function GetConfigFilePath() As String
        Return configPath
    End Function

    Public Sub BackupConfiguration(backupPath As String)
        Try
            If String.IsNullOrEmpty(backupPath) Then
                Throw New ArgumentException("Backup path cannot be empty")
            End If

            If File.Exists(configPath) Then
                File.Copy(configPath, backupPath, True)
            End If
        Catch ex As Exception
            Throw New Exception($"Error backing up configuration: {ex.Message}", ex)
        End Try
    End Sub

    Public Sub RestoreConfiguration(backupPath As String)
        Try
            If String.IsNullOrEmpty(backupPath) Then
                Throw New ArgumentException("Backup path cannot be empty")
            End If

            If File.Exists(backupPath) Then
                File.Copy(backupPath, configPath, True)
                LoadConfiguration()
            Else
                Throw New FileNotFoundException($"Backup file not found: {backupPath}")
            End If
        Catch ex As Exception
            Throw New Exception($"Error restoring configuration: {ex.Message}", ex)
        End Try
    End Sub
End Class