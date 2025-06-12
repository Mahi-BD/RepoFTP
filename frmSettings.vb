Imports System.IO

Public Class frmSettings
    Private logger As Logger = Logger.Instance
    Private configFilePath As String

    Public Sub New()
        InitializeComponent()
        configFilePath = Path.Combine(Application.StartupPath, "config.ini")
    End Sub

    Private Sub frmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadSettings()
    End Sub

    Private Sub LoadSettings()
        Try
            logger.Debug("Loading settings from config.ini...")
            
            Dim cleanFolders As String = "bin,obj,packages,.vs,Debug,Release"
            Dim forceDelete As String = "True"
            
            If File.Exists(configFilePath) Then
                Dim lines As String() = File.ReadAllLines(configFilePath)
                For Each line As String In lines
                    If line.StartsWith("CleanFolders=") Then
                        cleanFolders = line.Substring("CleanFolders=".Length)
                    ElseIf line.StartsWith("ForceDelete=") Then
                        forceDelete = line.Substring("ForceDelete=".Length)
                    End If
                Next
            End If
            
            txtCleanFolders.Text = cleanFolders
            chkForceDelete.Checked = Boolean.Parse(forceDelete)
            
            logger.Debug("Settings loaded successfully")
        Catch ex As Exception
            logger.Error("Error loading settings", ex)
            MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SaveSettings()
        Try
            logger.Debug("Saving settings to config.ini...")
            
            Dim configLines As New List(Of String)()
            
            ' Read existing config file to preserve other settings
            If File.Exists(configFilePath) Then
                Dim existingLines As String() = File.ReadAllLines(configFilePath)
                For Each line As String In existingLines
                    ' Skip the settings we're about to update
                    If Not line.StartsWith("CleanFolders=") AndAlso Not line.StartsWith("ForceDelete=") Then
                        configLines.Add(line)
                    End If
                Next
            End If
            
            ' Add our updated settings
            configLines.Add($"CleanFolders={txtCleanFolders.Text.Trim()}")
            configLines.Add($"ForceDelete={chkForceDelete.Checked}")
            
            File.WriteAllLines(configFilePath, configLines.ToArray())
            logger.Info("Settings saved successfully to config.ini")
        Catch ex As Exception
            logger.Error("Error saving settings", ex)
            MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveSettings()
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        If MessageBox.Show("Reset to default clean folder settings?", "Confirm Reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            txtCleanFolders.Text = "bin,obj,packages,.vs,Debug,Release"
            chkForceDelete.Checked = True
        End If
    End Sub

    Private Sub btnAddFolder_Click(sender As Object, e As EventArgs) Handles btnAddFolder.Click
        Using dialog As New FolderBrowserDialog()
            dialog.Description = "Select folder to add to clean list"
            If dialog.ShowDialog() = DialogResult.OK Then
                Dim folderName As String = System.IO.Path.GetFileName(dialog.SelectedPath)
                If Not String.IsNullOrEmpty(folderName) Then
                    Dim currentFolders As String = txtCleanFolders.Text.Trim()
                    If String.IsNullOrEmpty(currentFolders) Then
                        txtCleanFolders.Text = folderName
                    Else
                        txtCleanFolders.Text = currentFolders & "," & folderName
                    End If
                End If
            End If
        End Using
    End Sub
End Class