Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading.Tasks
Imports System.Runtime.InteropServices
Imports FluentFTP
Imports FluentFTP.Exceptions

Public Class frmMain
    Private configManager As ConfigManager
    Private cancelTokenSource As Threading.CancellationTokenSource
    Private syncInProgress As Boolean = False
    Private logger As Logger = Logger.Instance
    Private isFormLoading As Boolean = True

    Public Sub New()
        InitializeComponent()
        Try
            logger.Info("Application starting...")
            configManager = New ConfigManager()
            logger.Info("ConfigManager initialized successfully")
        Catch ex As Exception
            logger.Critical("Error initializing application", ex)
            MessageBox.Show($"Error initializing application: {ex.Message}", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        logger.Info("Form loading...")
        isFormLoading = True

        If configManager IsNot Nothing Then
            LoadConfiguration()
        End If
        UpdateSyncDirectionLabel()

        logger.ClearOldLogs(7)
        isFormLoading = False
        logger.Info("Application loaded successfully")
    End Sub

    Private Sub LoadConfiguration()
        Try
            logger.Debug("Loading configuration...")

            If configManager Is Nothing Then
                configManager = New ConfigManager()
                logger.Warning("ConfigManager was null, created new instance")
            End If

            If txtFtpServer IsNot Nothing Then
                Dim serverValue As String = configManager.GetValue("FTP", "Server", "")
                txtFtpServer.Text = serverValue
                logger.Debug($"Loaded FTP Server: {If(String.IsNullOrEmpty(serverValue), "(empty)", serverValue)}")
            End If

            If txtFtpUsername IsNot Nothing Then
                Dim usernameValue As String = configManager.GetValue("FTP", "Username", "")
                txtFtpUsername.Text = usernameValue
                logger.Debug($"Loaded FTP Username: {If(String.IsNullOrEmpty(usernameValue), "(empty)", usernameValue)}")
            End If

            If txtFtpPassword IsNot Nothing Then
                Dim passwordValue As String = configManager.GetDecryptedValue("FTP", "Password", "")
                txtFtpPassword.Text = passwordValue
                logger.Debug($"Loaded FTP Password: {If(String.IsNullOrEmpty(passwordValue), "(empty)", "***")}")
            End If

            If txtLocalFolder IsNot Nothing Then
                Dim localFolderValue As String = configManager.GetValue("Sync", "LocalFolder", "")
                txtLocalFolder.Text = localFolderValue
                logger.Debug($"Loaded Local Folder: {If(String.IsNullOrEmpty(localFolderValue), "(empty)", localFolderValue)}")
            End If

            If txtRemoteFolder IsNot Nothing Then
                Dim remoteFolderValue As String = configManager.GetValue("Sync", "RemoteFolder", "")
                txtRemoteFolder.Text = remoteFolderValue
                logger.Debug($"Loaded Remote Folder: {If(String.IsNullOrEmpty(remoteFolderValue), "(empty)", remoteFolderValue)}")
            End If

            Dim direction As String = configManager.GetValue("Sync", "Direction", "LocalToRemote")
            If rbLocalToRemote IsNot Nothing Then
                rbLocalToRemote.Checked = (direction = "LocalToRemote")
            End If
            If rbRemoteToLocal IsNot Nothing Then
                rbRemoteToLocal.Checked = (direction = "RemoteToLocal")
            End If

            logger.Debug($"Loaded Sync Direction: {direction}")
            logger.Info("Configuration loaded successfully")
        Catch ex As Exception
            logger.Error("Error loading configuration", ex)
            MessageBox.Show($"Error loading configuration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SaveConfiguration()
        Try
            logger.Debug("Saving configuration...")

            If configManager Is Nothing Then
                configManager = New ConfigManager()
                logger.Warning("ConfigManager was null, created new instance")
            End If

            Dim serverText As String = If(txtFtpServer?.Text, "")
            Dim usernameText As String = If(txtFtpUsername?.Text, "")
            Dim passwordText As String = If(txtFtpPassword?.Text, "")
            Dim localFolderText As String = If(txtLocalFolder?.Text, "")
            Dim remoteFolderText As String = If(txtRemoteFolder?.Text, "")

            configManager.SetValue("FTP", "Server", serverText)
            configManager.SetValue("FTP", "Username", usernameText)
            configManager.SetEncryptedValue("FTP", "Password", passwordText)
            configManager.SetValue("Sync", "LocalFolder", localFolderText)
            configManager.SetValue("Sync", "RemoteFolder", remoteFolderText)

            Dim direction As String = If(rbLocalToRemote?.Checked = True, "LocalToRemote", "RemoteToLocal")
            configManager.SetValue("Sync", "Direction", direction)

            configManager.SaveConfiguration()
            logger.Info("Configuration saved successfully")
        Catch ex As Exception
            logger.Error("Error saving configuration", ex)
            MessageBox.Show($"Error saving configuration: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnBrowseLocal_Click(sender As Object, e As EventArgs) Handles btnBrowseLocal.Click
        Using dialog As New FolderBrowserDialog()
            dialog.Description = "Select Local Folder"
            If txtLocalFolder IsNot Nothing Then
                dialog.SelectedPath = txtLocalFolder.Text
            End If

            If dialog.ShowDialog() = DialogResult.OK Then
                If txtLocalFolder IsNot Nothing Then
                    txtLocalFolder.Text = dialog.SelectedPath
                    logger.Info($"User selected local folder: {dialog.SelectedPath}")
                    SaveConfiguration()
                End If
            End If
        End Using
    End Sub

    Private Sub btnTestConnection_Click(sender As Object, e As EventArgs) Handles btnTestConnection.Click
        Dim serverText As String = If(txtFtpServer?.Text, "")
        Dim usernameText As String = If(txtFtpUsername?.Text, "")

        If String.IsNullOrWhiteSpace(serverText) OrElse String.IsNullOrWhiteSpace(usernameText) Then
            logger.Warning("Test connection attempted with missing server or username")
            MessageBox.Show("Please enter FTP server and username.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        logger.Info("User initiated FTP connection test")
        If btnTestConnection IsNot Nothing Then
            btnTestConnection.Enabled = False
        End If
        If lblStatus IsNot Nothing Then
            lblStatus.Text = "Testing connection..."
        End If

        Task.Run(Async Function()
                     Try
                         Dim testResult As Boolean = Await TestFtpConnection()

                         Me.Invoke(Sub()
                                       If testResult Then
                                           If lblStatus IsNot Nothing Then
                                               lblStatus.Text = "Connection successful!"
                                               lblStatus.ForeColor = Color.Green
                                           End If
                                           logger.Info("FTP connection test successful - User notified")
                                           MessageBox.Show("FTP connection successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                       Else
                                           If lblStatus IsNot Nothing Then
                                               lblStatus.Text = "Connection failed!"
                                               lblStatus.ForeColor = Color.Red
                                           End If
                                           logger.Warning("FTP connection test failed - User notified")
                                           MessageBox.Show("FTP connection failed. Please check your credentials and view logs for details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                       End If
                                       If btnTestConnection IsNot Nothing Then
                                           btnTestConnection.Enabled = True
                                       End If
                                   End Sub)
                     Catch ex As Exception
                         logger.Error("Unexpected error during connection test", ex)
                         Me.Invoke(Sub()
                                       If lblStatus IsNot Nothing Then
                                           lblStatus.Text = "Connection error!"
                                           lblStatus.ForeColor = Color.Red
                                       End If
                                       MessageBox.Show($"Connection error: {ex.Message}" & vbCrLf & vbCrLf & "Check logs for detailed information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                       If btnTestConnection IsNot Nothing Then
                                           btnTestConnection.Enabled = True
                                       End If
                                   End Sub)
                     End Try
                 End Function)
    End Sub

    Private Sub btnViewLogs_Click(sender As Object, e As EventArgs) Handles btnViewLogs.Click
        Try
            logger.Info("User opened log viewer")

            Dim recentLogs As String() = logger.GetRecentLogs(50)

            If recentLogs.Length > 0 Then
                Dim logText As String = String.Join(Environment.NewLine, recentLogs)

                Dim logForm As New Form()
                logForm.Text = "FTP Sync - Log Viewer"
                logForm.Size = New Size(800, 600)
                logForm.StartPosition = FormStartPosition.CenterParent
                logForm.ShowIcon = False

                Dim txtLog As New TextBox()
                txtLog.Multiline = True
                txtLog.ReadOnly = True
                txtLog.ScrollBars = ScrollBars.Both
                txtLog.Font = New Font("Consolas", 9)
                txtLog.Dock = DockStyle.Fill
                txtLog.Text = logText
                txtLog.WordWrap = False

                Dim panel As New Panel()
                panel.Dock = DockStyle.Bottom
                panel.Height = 40

                Dim btnClose As New Button()
                btnClose.Text = "Close"
                btnClose.Size = New Size(75, 23)
                btnClose.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
                btnClose.Location = New Point(logForm.Width - 90, 8)
                AddHandler btnClose.Click, Sub() logForm.Close()

                Dim btnBrowseFtp As New Button()
                btnBrowseFtp.Text = "Browse FTP"
                btnBrowseFtp.Size = New Size(100, 23)
                btnBrowseFtp.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
                btnBrowseFtp.Location = New Point(logForm.Width - 200, 8)
                AddHandler btnBrowseFtp.Click, Sub()
                                                   logForm.Hide()
                                                   BrowseFtpDirectories()
                                                   logForm.Show()
                                               End Sub

                Dim lblPath As New Label()
                lblPath.Text = $"Log file: {logger.GetLogFilePath()}"
                lblPath.Dock = DockStyle.Left
                lblPath.TextAlign = ContentAlignment.MiddleLeft
                lblPath.AutoSize = False
                lblPath.Width = 400

                panel.Controls.Add(btnClose)
                panel.Controls.Add(btnBrowseFtp)
                panel.Controls.Add(lblPath)
                logForm.Controls.Add(txtLog)
                logForm.Controls.Add(panel)

                txtLog.SelectionStart = txtLog.Text.Length
                txtLog.ScrollToCaret()

                logForm.ShowDialog()
            Else
                MessageBox.Show("No log entries found.", "Log Viewer", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            logger.Error("Failed to open log viewer", ex)
            MessageBox.Show($"Error opening log viewer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BrowseFtpDirectories()
        Try
            logger.Info("User initiated FTP directory browser")

            Task.Run(Async Function()
                         Try
                             Dim serverText As String = If(txtFtpServer?.Text, "")
                             Dim usernameText As String = If(txtFtpUsername?.Text, "")
                             Dim passwordText As String = If(txtFtpPassword?.Text, "")

                             If String.IsNullOrWhiteSpace(serverText) OrElse String.IsNullOrWhiteSpace(usernameText) Then
                                 Me.Invoke(Sub() MessageBox.Show("Please enter FTP server and username first.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning))
                                 Return
                             End If

                             Dim host As String = ""
                             Dim port As Integer = 21
                             ParseFtpUrl(serverText, host, port)

                             Using client As New AsyncFtpClient()
                                 client.Host = host
                                 client.Port = port
                                 client.Credentials = New System.Net.NetworkCredential(usernameText, passwordText)
                                 client.Config.ConnectTimeout = 15000
                                 client.Config.DataConnectionType = FtpDataConnectionType.AutoPassive

                                 Await client.AutoConnect()
                                 logger.Info("Connected to FTP server for browsing")

                                 Dim items = Await client.GetListing("/", FtpListOption.NoPath)

                                 Dim directoryInfo As New StringBuilder()
                                 directoryInfo.AppendLine("FTP Server Directory Structure:")
                                 directoryInfo.AppendLine("=" & New String("="c, 40))
                                 directoryInfo.AppendLine()

                                 Dim fileCount As Integer = 0
                                 Dim dirCount As Integer = 0

                                 For Each item In items
                                     Select Case item.Type
                                         Case FtpObjectType.File
                                             directoryInfo.AppendLine($"📄 {item.Name} ({If(item.Size >= 0, item.Size.ToString() & " bytes", "unknown size")})")
                                             fileCount += 1
                                         Case FtpObjectType.Directory
                                             directoryInfo.AppendLine($"📁 {item.Name}/")
                                             dirCount += 1

                                             ' Try to peek into subdirectory
                                             Try
                                                 Dim subItems = Await client.GetListing($"/{item.Name}", FtpListOption.NoPath)
                                                 Dim subFiles = subItems.Where(Function(si) si.Type = FtpObjectType.File).Count()
                                                 Dim subDirs = subItems.Where(Function(si) si.Type = FtpObjectType.Directory).Count()
                                                 If subFiles > 0 OrElse subDirs > 0 Then
                                                     directoryInfo.AppendLine($"   └─ Contains: {subFiles} files, {subDirs} folders")
                                                 End If
                                             Catch
                                                 directoryInfo.AppendLine($"   └─ (Cannot access)")
                                             End Try
                                         Case Else
                                             directoryInfo.AppendLine($"❓ {item.Name} (Unknown type)")
                                     End Select
                                 Next

                                 directoryInfo.AppendLine()
                                 directoryInfo.AppendLine($"Summary: {fileCount} files, {dirCount} directories")
                                 directoryInfo.AppendLine()
                                 directoryInfo.AppendLine("To download files from a specific directory:")
                                 directoryInfo.AppendLine("Enter the full path in Remote Folder field, like:")
                                 For Each dirItem In items.Where(Function(i) i.Type = FtpObjectType.Directory).Take(3)
                                     directoryInfo.AppendLine($"  /{dirItem.Name}/")
                                 Next

                                 Await client.Disconnect()

                                 Me.Invoke(Sub()
                                               Dim browseForm As New Form()
                                               browseForm.Text = "FTP Directory Browser"
                                               browseForm.Size = New Size(600, 500)
                                               browseForm.StartPosition = FormStartPosition.CenterParent

                                               Dim txtBrowse As New TextBox()
                                               txtBrowse.Multiline = True
                                               txtBrowse.ReadOnly = True
                                               txtBrowse.ScrollBars = ScrollBars.Both
                                               txtBrowse.Font = New Font("Consolas", 9)
                                               txtBrowse.Dock = DockStyle.Fill
                                               txtBrowse.Text = directoryInfo.ToString()

                                               Dim btnCloseBrowse As New Button()
                                               btnCloseBrowse.Text = "Close"
                                               btnCloseBrowse.Dock = DockStyle.Bottom
                                               btnCloseBrowse.Height = 30
                                               AddHandler btnCloseBrowse.Click, Sub() browseForm.Close()

                                               browseForm.Controls.Add(txtBrowse)
                                               browseForm.Controls.Add(btnCloseBrowse)
                                               browseForm.ShowDialog()
                                           End Sub)
                             End Using

                         Catch ex As Exception
                             logger.Error("Error browsing FTP directories", ex)
                             Me.Invoke(Sub() MessageBox.Show($"Error browsing FTP directories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error))
                         End Try
                     End Function)

        Catch ex As Exception
            logger.Error("Failed to start FTP directory browser", ex)
            MessageBox.Show($"Error starting FTP browser: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Async Function TestFtpConnection() As Task(Of Boolean)
        Try
            logger.Info("Testing FTP connection...")
            Dim host As String = ""
            Dim port As Integer = 21

            Dim serverText As String = If(txtFtpServer?.Text, "")
            Dim usernameText As String = If(txtFtpUsername?.Text, "")
            Dim passwordText As String = If(txtFtpPassword?.Text, "")

            ParseFtpUrl(serverText, host, port)
            logger.Debug($"Parsed server - Host: {host}, Port: {port}")

            logger.Info($"Attempting to connect to {host}:{port} with username: {usernameText}")

            Using client As New AsyncFtpClient()
                client.Host = host
                client.Port = port
                client.Credentials = New System.Net.NetworkCredential(usernameText, passwordText)

                logger.Debug("Configuring FTP client...")
                client.Config.ConnectTimeout = 15000
                client.Config.DataConnectionType = FtpDataConnectionType.AutoPassive
                client.Config.PassiveMaxAttempts = 5
                client.Config.DataConnectionConnectTimeout = 15000
                client.Config.DataConnectionReadTimeout = 15000

                logger.Debug("Attempting AutoConnect...")
                Await client.AutoConnect()

                Dim isConnected As Boolean = client.IsConnected
                logger.Info($"Connection result: {If(isConnected, "SUCCESS", "FAILED")}")

                If isConnected Then
                    logger.Debug("Testing basic FTP commands...")

                    Try
                        ' Try to get working directory
                        Dim workingDir As String = Await client.GetWorkingDirectory()
                        logger.Debug($"Current working directory: {workingDir}")
                    Catch ex As Exception
                        logger.Warning("Could not get working directory", ex)
                    End Try

                    logger.Debug("Disconnecting from FTP server...")
                    Await client.Disconnect()
                    logger.Info("Successfully disconnected from FTP server")
                End If

                Return isConnected
            End Using
        Catch ex As Exception
            logger.Error($"FTP connection test failed", ex)
            Return False
        End Try
    End Function

    Private Sub rbDirection_CheckedChanged(sender As Object, e As EventArgs) Handles rbLocalToRemote.CheckedChanged, rbRemoteToLocal.CheckedChanged
        UpdateSyncDirectionLabel()

        If Not isFormLoading Then
            SaveConfiguration()
        End If
    End Sub

    Private Sub UpdateSyncDirectionLabel()
        If rbLocalToRemote IsNot Nothing AndAlso rbRemoteToLocal IsNot Nothing AndAlso lblSyncDirection IsNot Nothing Then
            If rbLocalToRemote.Checked Then
                lblSyncDirection.Text = "Local → Remote"
                lblSyncDirection.ForeColor = Color.Blue
            Else
                lblSyncDirection.Text = "Remote → Local"
                lblSyncDirection.ForeColor = Color.DarkGreen
            End If
        End If
    End Sub

    Private Sub btnSync_Click(sender As Object, e As EventArgs) Handles btnSync.Click
        If syncInProgress Then
            If cancelTokenSource IsNot Nothing Then
                cancelTokenSource.Cancel()
            End If
            Return
        End If

        If Not ValidateInputs() Then
            Return
        End If

        SaveConfiguration()
        StartSync()
    End Sub

    Private Function ValidateInputs() As Boolean
        Dim serverText As String = If(txtFtpServer?.Text, "")
        Dim usernameText As String = If(txtFtpUsername?.Text, "")
        Dim localFolderText As String = If(txtLocalFolder?.Text, "")

        If String.IsNullOrWhiteSpace(serverText) Then
            MessageBox.Show("Please enter FTP server address." & vbCrLf & vbCrLf &
                          "Examples:" & vbCrLf &
                          "• 192.168.1.100" & vbCrLf &
                          "• ftp.example.com:2121" & vbCrLf &
                          "• ftp://192.168.31.132:2122",
                          "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            If txtFtpServer IsNot Nothing Then
                txtFtpServer.Focus()
            End If
            Return False
        End If

        If String.IsNullOrWhiteSpace(usernameText) Then
            MessageBox.Show("Please enter FTP username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            If txtFtpUsername IsNot Nothing Then
                txtFtpUsername.Focus()
            End If
            Return False
        End If

        If String.IsNullOrWhiteSpace(localFolderText) Then
            MessageBox.Show("Please select local folder.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            If btnBrowseLocal IsNot Nothing Then
                btnBrowseLocal.Focus()
            End If
            Return False
        End If

        If Not Directory.Exists(localFolderText) Then
            MessageBox.Show("Selected local folder does not exist.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            If btnBrowseLocal IsNot Nothing Then
                btnBrowseLocal.Focus()
            End If
            Return False
        End If

        Return True
    End Function

    Private Sub StartSync()
        Dim syncDirection As String = If(rbLocalToRemote?.Checked = True, "Local to Remote", "Remote to Local")
        logger.Info($"Starting synchronization - Direction: {syncDirection}")
        syncInProgress = True

        If btnSync IsNot Nothing Then
            btnSync.Text = "Cancel Sync"
            btnSync.BackColor = Color.IndianRed
        End If
        If progressBar IsNot Nothing Then
            progressBar.Value = 0
            progressBar.Visible = True
        End If
        If lblProgress IsNot Nothing Then
            lblProgress.Visible = True
            lblProgress.Text = "Preparing sync..."
        End If
        If lblStatus IsNot Nothing Then
            lblStatus.Text = "Synchronization in progress..."
            lblStatus.ForeColor = Color.Blue
        End If

        cancelTokenSource = New Threading.CancellationTokenSource()

        Task.Run(Async Function()
                     Try
                         If rbLocalToRemote?.Checked = True Then
                             logger.Info("Starting Local to Remote synchronization")
                             Await SyncLocalToRemote(cancelTokenSource.Token)
                         Else
                             logger.Info("Starting Remote to Local synchronization")
                             Await SyncRemoteToLocal(cancelTokenSource.Token)
                         End If

                         logger.Info("Synchronization completed successfully")
                         Me.Invoke(Sub()
                                       If lblStatus IsNot Nothing Then
                                           lblStatus.Text = "Synchronization completed successfully!"
                                           lblStatus.ForeColor = Color.Green
                                       End If
                                       MessageBox.Show("Synchronization completed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                   End Sub)
                     Catch ex As OperationCanceledException
                         logger.Warning("Synchronization was cancelled by user")
                         Me.Invoke(Sub()
                                       If lblStatus IsNot Nothing Then
                                           lblStatus.Text = "Synchronization cancelled."
                                           lblStatus.ForeColor = Color.Orange
                                       End If
                                   End Sub)
                     Catch ex As Exception
                         logger.Error("Synchronization failed", ex)
                         Me.Invoke(Sub()
                                       If lblStatus IsNot Nothing Then
                                           lblStatus.Text = "Synchronization failed!"
                                           lblStatus.ForeColor = Color.Red
                                       End If
                                       MessageBox.Show($"Synchronization error: {ex.Message}" & vbCrLf & vbCrLf & "Check logs for detailed information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                   End Sub)
                     Finally
                         logger.Debug("Cleaning up synchronization UI state")
                         Me.Invoke(Sub()
                                       syncInProgress = False
                                       If btnSync IsNot Nothing Then
                                           btnSync.Text = "Start Sync"
                                           btnSync.BackColor = Color.LimeGreen
                                       End If
                                       If progressBar IsNot Nothing Then
                                           progressBar.Visible = False
                                       End If
                                       If lblProgress IsNot Nothing Then
                                           lblProgress.Visible = False
                                       End If
                                   End Sub)
                     End Try
                 End Function)
    End Sub

    Private Sub ParseFtpUrl(serverInput As String, ByRef host As String, ByRef port As Integer)
        Try
            logger.Debug($"Parsing FTP URL: '{serverInput}'")

            If String.IsNullOrWhiteSpace(serverInput) Then
                host = ""
                port = 21
                logger.Warning("Server input is empty or whitespace")
                Return
            End If

            serverInput = serverInput.Trim()

            If serverInput.ToLower().StartsWith("ftp://") Then
                logger.Debug("Parsing as FTP URL")
                Dim uri As New Uri(serverInput)
                host = uri.Host
                port = If(uri.Port = -1, 21, uri.Port)
                logger.Debug($"URL parsed - Host: {host}, Port: {port}")
            Else
                If serverInput.Contains(":"c) Then
                    logger.Debug("Parsing as host:port format")
                    Dim parts() As String = serverInput.Split(":"c)
                    If parts.Length = 2 Then
                        host = parts(0).Trim()
                        If Not Integer.TryParse(parts(1).Trim(), port) Then
                            port = 21
                            logger.Warning($"Failed to parse port '{parts(1)}', using default port 21")
                        End If
                        logger.Debug($"Host:Port parsed - Host: {host}, Port: {port}")
                    Else
                        host = serverInput
                        port = 21
                        logger.Warning($"Invalid host:port format, using as hostname with default port")
                    End If
                Else
                    logger.Debug("Parsing as plain hostname/IP")
                    host = serverInput
                    port = 21
                End If
            End If

            If String.IsNullOrWhiteSpace(host) Then
                logger.Error("Parsed host is empty")
                Throw New ArgumentException("Invalid server address")
            End If

            logger.Info($"Successfully parsed server - Host: {host}, Port: {port}")

        Catch ex As Exception
            logger.Error($"URL parsing failed for '{serverInput}', using as hostname with default port", ex)
            host = serverInput
            port = 21
        End Try
    End Sub

    Private Async Function SyncLocalToRemote(cancellationToken As Threading.CancellationToken) As Task
        Dim host As String = ""
        Dim port As Integer = 21

        Dim serverText As String = If(txtFtpServer?.Text, "")
        Dim usernameText As String = If(txtFtpUsername?.Text, "")
        Dim passwordText As String = If(txtFtpPassword?.Text, "")
        Dim localFolderText As String = If(txtLocalFolder?.Text, "")
        Dim remoteFolderText As String = If(txtRemoteFolder?.Text, "")

        ParseFtpUrl(serverText, host, port)
        logger.Info($"Starting Local to Remote sync - Target: {host}:{port}")

        Using client As New AsyncFtpClient()
            client.Host = host
            client.Port = port
            client.Credentials = New System.Net.NetworkCredential(usernameText, passwordText)

            ' Enhanced configuration for problematic servers
            client.Config.ConnectTimeout = 30000
            client.Config.DataConnectionType = FtpDataConnectionType.AutoPassive
            client.Config.PassiveMaxAttempts = 5
            client.Config.DataConnectionConnectTimeout = 20000
            client.Config.DataConnectionReadTimeout = 20000
            client.Config.ValidateAnyCertificate = True
            client.Config.SocketKeepAlive = True

            logger.Debug("Connecting to FTP server for sync...")
            Await client.AutoConnect(cancellationToken)
            logger.Info("Successfully connected to FTP server for sync")

            ' Create base remote directory if specified
            If Not String.IsNullOrEmpty(remoteFolderText) Then
                logger.Debug($"Creating remote base directory: {remoteFolderText}")
                Await client.CreateDirectory(remoteFolderText, True, cancellationToken)
                logger.Info($"Remote base directory ensured: {remoteFolderText}")
            End If

            logger.Debug($"Scanning local directory recursively: {localFolderText}")
            Dim localFiles As String() = Directory.GetFiles(localFolderText, "*", SearchOption.AllDirectories)
            Dim totalFiles As Integer = localFiles.Length
            logger.Info($"Found {totalFiles} files to upload (including subdirectories)")

            If totalFiles = 0 Then
                logger.Warning("No files found in local directory")
                Me.Invoke(Sub()
                              If lblProgress IsNot Nothing Then
                                  lblProgress.Text = "No files to upload"
                              End If
                          End Sub)
                Return
            End If

            Dim processedFiles As Integer = 0
            Dim successCount As Integer = 0
            Dim errorCount As Integer = 0

            For Each localFile As String In localFiles
                If cancellationToken.IsCancellationRequested Then
                    logger.Warning("Upload cancelled by user")
                    Throw New OperationCanceledException()
                End If

                Try
                    ' Get relative path from local base folder
                    Dim relativePath As String = Path.GetRelativePath(localFolderText, localFile)

                    ' Convert Windows path separators to Unix/FTP format
                    Dim unixRelativePath As String = relativePath.Replace("\"c, "/"c)

                    ' Construct full remote path
                    Dim remotePath As String
                    If String.IsNullOrEmpty(remoteFolderText) Then
                        remotePath = unixRelativePath
                    Else
                        remotePath = $"{remoteFolderText.TrimEnd("/"c)}/{unixRelativePath}"
                    End If

                    logger.Debug($"Uploading file {processedFiles + 1}/{totalFiles}: {localFile} -> {remotePath}")
                    Me.Invoke(Sub()
                                  If lblProgress IsNot Nothing Then
                                      lblProgress.Text = $"Uploading: {relativePath} ({processedFiles + 1}/{totalFiles})"
                                  End If
                                  If progressBar IsNot Nothing Then
                                      progressBar.Value = CInt((processedFiles / totalFiles) * 100)
                                  End If
                              End Sub)

                    ' Ensure remote directory structure exists
                    Dim remoteDir As String = ""
                    If remotePath.Contains("/") Then
                        Dim lastSlashIndex As Integer = remotePath.LastIndexOf("/"c)
                        remoteDir = remotePath.Substring(0, lastSlashIndex)

                        If Not String.IsNullOrEmpty(remoteDir) Then
                            Try
                                logger.Debug($"Creating remote directory structure: {remoteDir}")
                                Await client.CreateDirectory(remoteDir, True, cancellationToken)
                                logger.Debug($"Remote directory created/verified: {remoteDir}")
                            Catch ex As Exception
                                logger.Warning($"Could not create remote directory {remoteDir}: {ex.Message}")
                            End Try
                        End If
                    End If

                    ' Multiple upload attempts with different settings
                    Dim uploadSuccessful As Boolean = False
                    Dim originalTransferMode = client.Config.DataConnectionType

                    ' Attempt 1: Use current settings
                    Try
                        Await client.UploadFile(localFile, remotePath, FtpRemoteExists.Overwrite, True, token:=cancellationToken)
                        uploadSuccessful = True
                        logger.Debug($"Successfully uploaded: {remotePath}")
                    Catch ex As Exception
                        logger.Debug($"Upload attempt 1 failed for {Path.GetFileName(localFile)}: {ex.Message}")
                    End Try

                    ' Attempt 2: Try with AutoActive if first failed
                    If Not uploadSuccessful Then
                        Try
                            client.Config.DataConnectionType = FtpDataConnectionType.AutoActive
                            Await client.UploadFile(localFile, remotePath, FtpRemoteExists.Overwrite, True, token:=cancellationToken)
                            uploadSuccessful = True
                            logger.Debug($"Successfully uploaded with AutoActive: {remotePath}")
                        Catch ex As Exception
                            logger.Debug($"Upload attempt 2 failed for {Path.GetFileName(localFile)}: {ex.Message}")
                        Finally
                            client.Config.DataConnectionType = originalTransferMode
                        End Try
                    End If

                    ' Attempt 3: Try with PORT mode if still failed
                    If Not uploadSuccessful Then
                        Try
                            client.Config.DataConnectionType = FtpDataConnectionType.PORT
                            Await client.UploadFile(localFile, remotePath, FtpRemoteExists.Overwrite, True, token:=cancellationToken)
                            uploadSuccessful = True
                            logger.Debug($"Successfully uploaded with PORT: {remotePath}")
                        Catch ex As Exception
                            logger.Debug($"Upload attempt 3 failed for {Path.GetFileName(localFile)}: {ex.Message}")
                        Finally
                            client.Config.DataConnectionType = originalTransferMode
                        End Try
                    End If

                    If uploadSuccessful Then
                        successCount += 1
                        logger.Debug($"Upload completed successfully: {localFile} -> {remotePath}")
                    Else
                        errorCount += 1
                        logger.Error($"All upload attempts failed for: {localFile}")
                    End If

                Catch ex As Exception When Not TypeOf ex Is OperationCanceledException
                    errorCount += 1
                    logger.Error($"Failed to upload {localFile}", ex)
                End Try

                processedFiles += 1
            Next

            logger.Info($"Disconnecting from FTP server...")
            Await client.Disconnect(cancellationToken)
            logger.Info($"Upload completed - Success: {successCount}, Errors: {errorCount}, Total: {processedFiles}")

            Me.Invoke(Sub()
                          If progressBar IsNot Nothing Then
                              progressBar.Value = 100
                          End If
                          If lblProgress IsNot Nothing Then
                              lblProgress.Text = $"Completed: {successCount} uploaded, {errorCount} errors"
                          End If
                      End Sub)
        End Using
    End Function

    Private Async Function SyncRemoteToLocal(cancellationToken As Threading.CancellationToken) As Task
        Dim host As String = ""
        Dim port As Integer = 21

        Dim serverText As String = If(txtFtpServer?.Text, "")
        Dim usernameText As String = If(txtFtpUsername?.Text, "")
        Dim passwordText As String = If(txtFtpPassword?.Text, "")
        Dim localFolderText As String = If(txtLocalFolder?.Text, "")
        Dim remoteFolderText As String = If(txtRemoteFolder?.Text, "")

        ParseFtpUrl(serverText, host, port)
        logger.Info($"Starting Remote to Local sync - Source: {host}:{port}")

        Using client As New AsyncFtpClient()
            client.Host = host
            client.Port = port
            client.Credentials = New System.Net.NetworkCredential(usernameText, passwordText)

            ' Enhanced configuration for problematic servers
            client.Config.ConnectTimeout = 30000
            client.Config.DataConnectionType = FtpDataConnectionType.AutoPassive
            client.Config.PassiveMaxAttempts = 5
            client.Config.DataConnectionConnectTimeout = 20000
            client.Config.DataConnectionReadTimeout = 20000
            client.Config.ValidateAnyCertificate = True
            client.Config.SocketKeepAlive = True

            logger.Debug("Connecting to FTP server for download sync...")
            Await client.AutoConnect(cancellationToken)
            logger.Info("Successfully connected to FTP server for download sync")

            Dim remoteDirectory As String = If(String.IsNullOrEmpty(remoteFolderText), "/", remoteFolderText)

            ' Check if remote path is a specific file
            If Not String.IsNullOrEmpty(remoteFolderText) AndAlso remoteFolderText.Contains(".") Then
                logger.Debug($"Remote path appears to be a file: {remoteFolderText}")
                Try
                    If Await client.FileExists(remoteFolderText, cancellationToken) Then
                        ' Process single file download
                        Await ProcessSingleFileDownload(client, remoteFolderText, localFolderText, cancellationToken)

                        ' Disconnect and return
                        Await client.Disconnect(cancellationToken)
                        Return
                    Else
                        Throw New FileNotFoundException($"Specified file not found: {remoteFolderText}")
                    End If
                Catch ex As Exception
                    logger.Error($"Error checking specific file: {remoteFolderText}", ex)
                    Throw New Exception($"Cannot access specified file: {remoteFolderText}. Error: {ex.Message}")
                End Try
            Else
                ' Use recursive download to maintain folder structure
                logger.Info($"Starting recursive download from: {remoteDirectory}")

                Me.Invoke(Sub()
                              If lblProgress IsNot Nothing Then
                                  lblProgress.Text = "Scanning remote directory structure..."
                              End If
                          End Sub)

                ' Get all files recursively with their paths
                Dim allRemoteFiles = Await GetAllFilesRecursively(client, remoteDirectory, cancellationToken)

                Dim totalFiles As Integer = allRemoteFiles.Count
                logger.Info($"Found {totalFiles} files in directory structure")

                If totalFiles = 0 Then
                    logger.Warning("No files found in remote directory structure")
                    Me.Invoke(Sub()
                                  If lblProgress IsNot Nothing Then
                                      lblProgress.Text = "No files found to download"
                                  End If
                              End Sub)
                    Return
                End If

                Dim processedFiles As Integer = 0
                Dim successCount As Integer = 0
                Dim errorCount As Integer = 0

                ' Download each file maintaining directory structure
                For Each fileInfo In allRemoteFiles
                    If cancellationToken.IsCancellationRequested Then
                        logger.Warning("Download cancelled by user")
                        Throw New OperationCanceledException()
                    End If

                    Try
                        Dim remotePath As String = fileInfo("RemotePath").ToString()
                        Dim relativePath As String = fileInfo("RelativePath").ToString()
                        Dim fileName As String = Path.GetFileName(relativePath)

                        ' Create local path maintaining directory structure
                        Dim localPath As String = Path.Combine(localFolderText, relativePath.Replace("/"c, "\"c))

                        ' Ensure local directory exists
                        Dim localDir As String = Path.GetDirectoryName(localPath)
                        If Not Directory.Exists(localDir) Then
                            Directory.CreateDirectory(localDir)
                            logger.Debug($"Created local directory: {localDir}")
                        End If

                        ' Files will be overwritten if they exist

                        logger.Debug($"Downloading file {processedFiles + 1}/{totalFiles}: {remotePath}")
                        Me.Invoke(Sub()
                                      If lblProgress IsNot Nothing Then
                                          lblProgress.Text = $"Downloading: {relativePath} ({processedFiles + 1}/{totalFiles})"
                                      End If
                                      If progressBar IsNot Nothing Then
                                          progressBar.Value = CInt((processedFiles / totalFiles) * 100)
                                      End If
                                  End Sub)

                        ' Try download with multiple transfer modes
                        Dim downloadSuccessful As Boolean = False
                        Dim originalTransferMode = client.Config.DataConnectionType

                        ' Attempt 1: Current settings
                        Try
                            Await client.DownloadFile(localPath, remotePath, FtpLocalExists.Overwrite, token:=cancellationToken)
                            downloadSuccessful = True
                            logger.Debug($"Successfully downloaded: {remotePath} -> {localPath}")
                        Catch ex As Exception
                            logger.Debug($"Download attempt 1 failed for {fileName}: {ex.Message}")
                        End Try

                        ' Attempt 2: AutoActive mode
                        If Not downloadSuccessful Then
                            Try
                                client.Config.DataConnectionType = FtpDataConnectionType.AutoActive
                                Await client.DownloadFile(localPath, remotePath, FtpLocalExists.Overwrite, token:=cancellationToken)
                                downloadSuccessful = True
                                logger.Debug($"Successfully downloaded with AutoActive: {remotePath}")
                            Catch ex As Exception
                                logger.Debug($"Download attempt 2 failed for {fileName}: {ex.Message}")
                            Finally
                                client.Config.DataConnectionType = originalTransferMode
                            End Try
                        End If

                        ' Attempt 3: PORT mode
                        If Not downloadSuccessful Then
                            Try
                                client.Config.DataConnectionType = FtpDataConnectionType.PORT
                                Await client.DownloadFile(localPath, remotePath, FtpLocalExists.Overwrite, token:=cancellationToken)
                                downloadSuccessful = True
                                logger.Debug($"Successfully downloaded with PORT: {remotePath}")
                            Catch ex As Exception
                                logger.Debug($"Download attempt 3 failed for {fileName}: {ex.Message}")
                            Finally
                                client.Config.DataConnectionType = originalTransferMode
                            End Try
                        End If

                        If downloadSuccessful Then
                            successCount += 1
                            logger.Debug($"Download completed: {localPath}")
                        Else
                            errorCount += 1
                            logger.Error($"All download attempts failed for: {remotePath}")
                        End If

                    Catch ex As Exception When Not TypeOf ex Is OperationCanceledException
                        errorCount += 1
                        logger.Error($"Error downloading file", ex)
                    End Try

                    processedFiles += 1
                Next

                logger.Info($"Disconnecting from FTP server...")
                Await client.Disconnect(cancellationToken)
                logger.Info($"Download completed - Success: {successCount}, Errors: {errorCount}, Total: {processedFiles}")

                Me.Invoke(Sub()
                              If progressBar IsNot Nothing Then
                                  progressBar.Value = 100
                              End If
                              If lblProgress IsNot Nothing Then
                                  lblProgress.Text = $"Completed: {successCount} downloaded, {errorCount} errors"
                              End If
                          End Sub)
            End If
        End Using
    End Function

    ' Helper method to get all files recursively maintaining path structure
    Private Async Function GetAllFilesRecursively(client As AsyncFtpClient, directoryPath As String, cancellationToken As Threading.CancellationToken, Optional basePath As String = "") As Task(Of List(Of Dictionary(Of String, String)))
        Dim allFiles As New List(Of Dictionary(Of String, String))()

        Try
            logger.Debug($"Scanning directory: {directoryPath}")

            ' Get listing for current directory
            Dim items = Await client.GetListing(directoryPath, FtpListOption.Modify, cancellationToken)

            For Each item In items
                If cancellationToken.IsCancellationRequested Then
                    Return allFiles
                End If

                If item.Type = FtpObjectType.File Then
                    ' Calculate relative path from base directory
                    Dim relativePath As String
                    If String.IsNullOrEmpty(basePath) Then
                        relativePath = item.Name
                    Else
                        relativePath = $"{basePath}/{item.Name}"
                    End If

                    ' Full remote path
                    Dim fullRemotePath As String = If(String.IsNullOrEmpty(item.FullName),
                                                    $"{directoryPath.TrimEnd("/"c)}/{item.Name}",
                                                    item.FullName)

                    ' Add file info
                    Dim fileInfo As New Dictionary(Of String, String) From {
                        {"RemotePath", fullRemotePath},
                        {"RelativePath", relativePath},
                        {"FileName", item.Name}
                    }

                    allFiles.Add(fileInfo)
                    logger.Debug($"Added file: {relativePath}")

                ElseIf item.Type = FtpObjectType.Directory Then
                    ' Recursively scan subdirectories
                    Try
                        Dim subDirPath As String = $"{directoryPath.TrimEnd("/"c)}/{item.Name}"
                        Dim subBasePath As String = If(String.IsNullOrEmpty(basePath), item.Name, $"{basePath}/{item.Name}")

                        logger.Debug($"Entering subdirectory: {subDirPath}")
                        Dim subFiles = Await GetAllFilesRecursively(client, subDirPath, cancellationToken, subBasePath)
                        allFiles.AddRange(subFiles)

                    Catch ex As Exception
                        logger.Warning($"Could not access subdirectory {item.Name}: {ex.Message}")
                    End Try
                End If
            Next

        Catch ex As Exception
            logger.Warning($"Error scanning directory {directoryPath}: {ex.Message}")
        End Try

        Return allFiles
    End Function

    ' Helper method to process single file downloads
    Private Async Function ProcessSingleFileDownload(client As AsyncFtpClient, remoteFilePath As String, localFolderText As String, cancellationToken As Threading.CancellationToken) As Task
        Try
            Dim fileName As String = Path.GetFileName(remoteFilePath)
            Dim localPath As String = Path.Combine(localFolderText, fileName)

            ' Files will be overwritten if they exist

            logger.Debug($"Downloading single file: {remoteFilePath}")
            Me.Invoke(Sub()
                          If lblProgress IsNot Nothing Then
                              lblProgress.Text = $"Downloading: {fileName}"
                          End If
                          If progressBar IsNot Nothing Then
                              progressBar.Value = 50
                          End If
                      End Sub)

            ' Try download with multiple transfer modes
            Dim downloadSuccessful As Boolean = False
            Dim originalTransferMode = client.Config.DataConnectionType

            ' Attempt 1: Current settings
            Try
                Await client.DownloadFile(localPath, remoteFilePath, FtpLocalExists.Overwrite, token:=cancellationToken)
                downloadSuccessful = True
                logger.Debug($"Successfully downloaded: {remoteFilePath}")
            Catch ex As Exception
                logger.Debug($"Download attempt 1 failed for {fileName}: {ex.Message}")
            End Try

            ' Attempt 2: AutoActive mode
            If Not downloadSuccessful Then
                Try
                    client.Config.DataConnectionType = FtpDataConnectionType.AutoActive
                    Await client.DownloadFile(localPath, remoteFilePath, FtpLocalExists.Overwrite, token:=cancellationToken)
                    downloadSuccessful = True
                    logger.Debug($"Successfully downloaded with AutoActive: {remoteFilePath}")
                Catch ex As Exception
                    logger.Debug($"Download attempt 2 failed for {fileName}: {ex.Message}")
                Finally
                    client.Config.DataConnectionType = originalTransferMode
                End Try
            End If

            ' Attempt 3: PORT mode
            If Not downloadSuccessful Then
                Try
                    client.Config.DataConnectionType = FtpDataConnectionType.PORT
                    Await client.DownloadFile(localPath, remoteFilePath, FtpLocalExists.Overwrite, token:=cancellationToken)
                    downloadSuccessful = True
                    logger.Debug($"Successfully downloaded with PORT: {remoteFilePath}")
                Catch ex As Exception
                    logger.Debug($"Download attempt 3 failed for {fileName}: {ex.Message}")
                Finally
                    client.Config.DataConnectionType = originalTransferMode
                End Try
            End If

            If downloadSuccessful Then
                logger.Info($"Single file download completed: {localPath}")
                Me.Invoke(Sub()
                              If lblProgress IsNot Nothing Then
                                  lblProgress.Text = $"Completed: 1 downloaded, 0 errors"
                              End If
                              If progressBar IsNot Nothing Then
                                  progressBar.Value = 100
                              End If
                          End Sub)
            Else
                logger.Error($"All download attempts failed for: {remoteFilePath}")
                Me.Invoke(Sub()
                              If lblProgress IsNot Nothing Then
                                  lblProgress.Text = $"Completed: 0 downloaded, 1 error"
                              End If
                              If progressBar IsNot Nothing Then
                                  progressBar.Value = 100
                              End If
                          End Sub)
                Throw New Exception($"Failed to download file: {remoteFilePath}")
            End If

        Catch ex As Exception
            logger.Error($"Error in single file download", ex)
            Throw
        End Try
    End Function

    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If syncInProgress Then
            Dim result As DialogResult = MessageBox.Show("Synchronization is in progress. Do you want to cancel and exit?",
                                                        "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.No Then
                e.Cancel = True
                Return
            End If

            If cancelTokenSource IsNot Nothing Then
                cancelTokenSource.Cancel()
            End If
        End If

        SaveConfiguration()
        logger.Info("Application closing")
    End Sub

    Private Sub txtConfiguration_TextChanged(sender As Object, e As EventArgs) Handles txtFtpServer.TextChanged, txtFtpUsername.TextChanged, txtFtpPassword.TextChanged, txtLocalFolder.TextChanged, txtRemoteFolder.TextChanged
        If configManager Is Nothing OrElse Not Me.IsHandleCreated OrElse isFormLoading Then
            Return
        End If

        Static saveTimer As Timer
        If saveTimer IsNot Nothing Then
            saveTimer.Stop()
            saveTimer.Dispose()
        End If

        saveTimer = New Timer()
        saveTimer.Interval = 1000
        AddHandler saveTimer.Tick, Sub()
                                       saveTimer.Stop()
                                       saveTimer.Dispose()
                                       If Not isFormLoading Then
                                           SaveConfiguration()
                                       End If
                                   End Sub
        saveTimer.Start()
    End Sub

    Private Sub settingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles settingsToolStripMenuItem.Click
        Try
            logger.Info("User opened settings dialog")
            Using settingsForm As New frmSettings()
                If settingsForm.ShowDialog() = DialogResult.OK Then
                    logger.Info("Settings saved successfully")
                End If
            End Using
        Catch ex As Exception
            logger.Error("Error opening settings dialog", ex)
            MessageBox.Show($"Error opening settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCleanFolders_Click(sender As Object, e As EventArgs) Handles btnCleanFolders.Click
        Try
            logger.Info("User initiated clean folders operation")
            
            If String.IsNullOrWhiteSpace(txtLocalFolder?.Text) Then
                MessageBox.Show("Please select a local folder first.", "No Folder Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim localFolderPath As String = txtLocalFolder.Text.Trim()
            If Not Directory.Exists(localFolderPath) Then
                MessageBox.Show("Selected local folder does not exist.", "Folder Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ' Get clean folder settings from config.ini
            Dim cleanFoldersString As String = "bin,obj,packages,.vs,Debug,Release"
            Dim forceDeleteString As String = "True"
            
            Dim configFilePath As String = Path.Combine(Application.StartupPath, "config.ini")
            If File.Exists(configFilePath) Then
                Dim lines As String() = File.ReadAllLines(configFilePath)
                For Each line As String In lines
                    If line.StartsWith("CleanFolders=") Then
                        cleanFoldersString = line.Substring("CleanFolders=".Length)
                    ElseIf line.StartsWith("ForceDelete=") Then
                        forceDeleteString = line.Substring("ForceDelete=".Length)
                    End If
                Next
            End If
            
            Dim forceDelete As Boolean = Boolean.Parse(forceDeleteString)
            
            ' Parse and clean the folder names
            Dim tempFolders As String() = cleanFoldersString.Split(","c)
            Dim cleanFoldersList As New List(Of String)()
            For Each folder As String In tempFolders
                Dim trimmedFolder As String = folder.Trim()
                If Not String.IsNullOrEmpty(trimmedFolder) Then
                    cleanFoldersList.Add(trimmedFolder)
                End If
            Next
            Dim foldersToProcess As String() = New String(cleanFoldersList.Count - 1) {}
            cleanFoldersList.CopyTo(foldersToProcess)

            If foldersToProcess.Length = 0 Then
                MessageBox.Show("No clean folders configured. Please configure clean folders in Settings.", "No Clean Folders", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            ' Show confirmation dialog
            Dim foldersToClean As New List(Of String)()
            For Each folderName In foldersToProcess
                Dim folderPath As String = Path.Combine(localFolderPath, folderName)
                If Directory.Exists(folderPath) Then
                    foldersToClean.Add(folderName)
                End If
            Next

            If foldersToClean.Count = 0 Then
                MessageBox.Show("No folders found to clean in the selected directory.", "No Folders to Clean", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim message As String = $"This will delete all files and subfolders inside the following folders in:{vbCrLf}{localFolderPath}{vbCrLf}{vbCrLf}"
            message &= String.Join(", ", foldersToClean) & vbCrLf & vbCrLf
            message &= "The folders themselves will remain. Continue?"

            If MessageBox.Show(message, "Confirm Clean Operation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                Dim folderArray(foldersToClean.Count - 1) As String
                foldersToClean.CopyTo(folderArray)
                CleanFolders(localFolderPath, folderArray, forceDelete)
            End If

        Catch ex As Exception
            logger.Error("Error in clean folders operation", ex)
            MessageBox.Show($"Error in clean operation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub CleanFolders(basePath As String, folderNames As String(), forceDelete As Boolean)
        Try
            logger.Info($"Starting clean operation in: {basePath}")
            logger.Info($"Folders to clean: {String.Join(", ", folderNames)}")
            logger.Info($"Force delete enabled: {forceDelete}")

            Dim totalCleaned As Integer = 0
            Dim totalErrors As Integer = 0

            For Each folderName In folderNames
                Dim folderPath As String = Path.Combine(basePath, folderName)
                
                If Directory.Exists(folderPath) Then
                    logger.Debug($"Cleaning folder: {folderPath}")
                    Dim result = CleanDirectoryContents(folderPath, forceDelete)
                    totalCleaned += result.FilesDeleted
                    totalErrors += result.Errors
                    logger.Debug($"Cleaned {result.FilesDeleted} files from {folderName}, {result.Errors} errors")
                Else
                    logger.Debug($"Folder not found, skipping: {folderPath}")
                End If
            Next

            logger.Info($"Clean operation completed - Files deleted: {totalCleaned}, Errors: {totalErrors}")
            
            Dim resultMessage As String = $"Clean operation completed!{vbCrLf}Files deleted: {totalCleaned}"
            If totalErrors > 0 Then
                resultMessage &= $"{vbCrLf}Errors: {totalErrors} (check logs for details)"
            End If

            MessageBox.Show(resultMessage, "Clean Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            logger.Error("Error during clean operation", ex)
            MessageBox.Show($"Error during clean operation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function CleanDirectoryContents(directoryPath As String, forceDelete As Boolean) As (FilesDeleted As Integer, Errors As Integer)
        Dim filesDeleted As Integer = 0
        Dim errors As Integer = 0

        Try
            ' Delete all files in the directory
            Dim files As String() = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories)
            
            For Each filePath In files
                Try
                    If forceDelete Then
                        ' Remove read-only attribute if present
                        Dim fileInfo As New FileInfo(filePath)
                        If fileInfo.IsReadOnly Then
                            fileInfo.IsReadOnly = False
                        End If
                    End If
                    
                    File.Delete(filePath)
                    filesDeleted += 1
                    logger.Debug($"Deleted file: {filePath}")
                    
                Catch ex As Exception
                    errors += 1
                    logger.Warning($"Failed to delete file: {filePath} - {ex.Message}")
                End Try
            Next

            ' Delete all subdirectories (after files are deleted)
            Dim subdirectories As String() = Directory.GetDirectories(directoryPath, "*", SearchOption.TopDirectoryOnly)
            
            For Each subDir In subdirectories
                Try
                    If forceDelete Then
                        ' Remove read-only attributes from directory and all subdirectories
                        RemoveReadOnlyAttributes(subDir)
                    End If
                    
                    Directory.Delete(subDir, True) ' Recursive delete
                    logger.Debug($"Deleted directory: {subDir}")
                    
                Catch ex As Exception
                    errors += 1
                    logger.Warning($"Failed to delete directory: {subDir} - {ex.Message}")
                End Try
            Next

        Catch ex As Exception
            errors += 1
            logger.Error($"Error cleaning directory {directoryPath}", ex)
        End Try

        Return (filesDeleted, errors)
    End Function

    Private Sub RemoveReadOnlyAttributes(directoryPath As String)
        Try
            ' Remove read-only attribute from the directory itself
            Dim dirInfo As New DirectoryInfo(directoryPath)
            If (dirInfo.Attributes And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                dirInfo.Attributes = dirInfo.Attributes And Not FileAttributes.ReadOnly
            End If

            ' Remove read-only attributes from all files
            For Each file In Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories)
                Try
                    Dim fileInfo As New FileInfo(file)
                    If fileInfo.IsReadOnly Then
                        fileInfo.IsReadOnly = False
                    End If
                Catch
                    ' Continue if we can't modify this file
                End Try
            Next

            ' Remove read-only attributes from all subdirectories
            For Each subDir In Directory.GetDirectories(directoryPath, "*", SearchOption.AllDirectories)
                Try
                    Dim subDirInfo As New DirectoryInfo(subDir)
                    If (subDirInfo.Attributes And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                        subDirInfo.Attributes = subDirInfo.Attributes And Not FileAttributes.ReadOnly
                    End If
                Catch
                    ' Continue if we can't modify this directory
                End Try
            Next

        Catch ex As Exception
            logger.Warning($"Could not remove read-only attributes from {directoryPath}: {ex.Message}")
        End Try
    End Sub
End Class