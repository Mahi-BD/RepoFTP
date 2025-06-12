<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        splitContainer1 = New SplitContainer()
        leftPanel = New Panel()
        grpFtpSettings = New GroupBox()
        lblServerHint = New Label()
        btnTestConnection = New Button()
        txtFtpPassword = New TextBox()
        txtFtpUsername = New TextBox()
        txtFtpServer = New TextBox()
        lblFtpPassword = New Label()
        lblFtpUsername = New Label()
        lblFtpServer = New Label()
        rightPanel = New Panel()
        grpSync = New GroupBox()
        pnlSyncControls = New Panel()
        progressBar = New ProgressBar()
        lblProgress = New Label()
        btnSync = New Button()
        btnViewLogs = New Button()
        btnCleanFolders = New Button()
        grpSyncSettings = New GroupBox()
        splitContainer2 = New SplitContainer()
        pnlSyncTop = New Panel()
        tlpSyncTop = New TableLayoutPanel()
        lblSyncDirection = New Label()
        grpDirection = New GroupBox()
        rbRemoteToLocal = New RadioButton()
        rbLocalToRemote = New RadioButton()
        lblLocalFolder = New Label()
        txtLocalFolder = New TextBox()
        btnBrowseLocal = New Button()
        lblRemoteFolder = New Label()
        txtRemoteFolder = New TextBox()
        pnlSyncBottom = New Panel()
        lblStatus = New Label()
        menuStrip1 = New MenuStrip()
        fileToolStripMenuItem = New ToolStripMenuItem()
        settingsToolStripMenuItem = New ToolStripMenuItem()
        toolStripSeparator1 = New ToolStripSeparator()
        exitToolStripMenuItem = New ToolStripMenuItem()
        helpToolStripMenuItem = New ToolStripMenuItem()
        aboutToolStripMenuItem = New ToolStripMenuItem()
        statusStrip = New StatusStrip()
        toolStripStatusLabel = New ToolStripStatusLabel()
        folderBrowserDialog = New FolderBrowserDialog()
        toolTip = New ToolTip(components)
        CType(splitContainer1, ComponentModel.ISupportInitialize).BeginInit()
        splitContainer1.Panel1.SuspendLayout()
        splitContainer1.Panel2.SuspendLayout()
        splitContainer1.SuspendLayout()
        leftPanel.SuspendLayout()
        grpFtpSettings.SuspendLayout()
        rightPanel.SuspendLayout()
        grpSync.SuspendLayout()
        pnlSyncControls.SuspendLayout()
        grpSyncSettings.SuspendLayout()
        CType(splitContainer2, ComponentModel.ISupportInitialize).BeginInit()
        splitContainer2.Panel1.SuspendLayout()
        splitContainer2.Panel2.SuspendLayout()
        splitContainer2.SuspendLayout()
        pnlSyncTop.SuspendLayout()
        tlpSyncTop.SuspendLayout()
        grpDirection.SuspendLayout()
        pnlSyncBottom.SuspendLayout()
        menuStrip1.SuspendLayout()
        statusStrip.SuspendLayout()
        SuspendLayout()
        ' 
        ' splitContainer1
        ' 
        splitContainer1.Dock = DockStyle.Fill
        splitContainer1.Location = New Point(0, 24)
        splitContainer1.Name = "splitContainer1"
        ' 
        ' splitContainer1.Panel1
        ' 
        splitContainer1.Panel1.Controls.Add(leftPanel)
        ' 
        ' splitContainer1.Panel2
        ' 
        splitContainer1.Panel2.Controls.Add(rightPanel)
        splitContainer1.Size = New Size(961, 315)
        splitContainer1.SplitterDistance = 255
        splitContainer1.SplitterWidth = 6
        splitContainer1.TabIndex = 0
        ' 
        ' leftPanel
        ' 
        leftPanel.Controls.Add(grpFtpSettings)
        leftPanel.Dock = DockStyle.Fill
        leftPanel.Location = New Point(0, 0)
        leftPanel.Name = "leftPanel"
        leftPanel.Padding = New Padding(8)
        leftPanel.Size = New Size(255, 315)
        leftPanel.TabIndex = 0
        ' 
        ' grpFtpSettings
        ' 
        grpFtpSettings.Controls.Add(lblServerHint)
        grpFtpSettings.Controls.Add(btnTestConnection)
        grpFtpSettings.Controls.Add(txtFtpPassword)
        grpFtpSettings.Controls.Add(txtFtpUsername)
        grpFtpSettings.Controls.Add(txtFtpServer)
        grpFtpSettings.Controls.Add(lblFtpPassword)
        grpFtpSettings.Controls.Add(lblFtpUsername)
        grpFtpSettings.Controls.Add(lblFtpServer)
        grpFtpSettings.Dock = DockStyle.Fill
        grpFtpSettings.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        grpFtpSettings.Location = New Point(8, 8)
        grpFtpSettings.Name = "grpFtpSettings"
        grpFtpSettings.Padding = New Padding(8)
        grpFtpSettings.Size = New Size(239, 299)
        grpFtpSettings.TabIndex = 0
        grpFtpSettings.TabStop = False
        grpFtpSettings.Text = "FTP Connection Settings"
        ' 
        ' lblServerHint
        ' 
        lblServerHint.AutoSize = True
        lblServerHint.Font = New Font("Segoe UI", 8F, FontStyle.Italic)
        lblServerHint.ForeColor = Color.Gray
        lblServerHint.Location = New Point(15, 200)
        lblServerHint.Name = "lblServerHint"
        lblServerHint.Size = New Size(197, 13)
        lblServerHint.TabIndex = 7
        lblServerHint.Text = "Example: ftp.example.com or 192.168.1.1"
        ' 
        ' btnTestConnection
        ' 
        btnTestConnection.BackColor = Color.FromArgb(CByte(0), CByte(120), CByte(215))
        btnTestConnection.FlatStyle = FlatStyle.Flat
        btnTestConnection.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnTestConnection.ForeColor = Color.White
        btnTestConnection.Location = New Point(15, 240)
        btnTestConnection.Name = "btnTestConnection"
        btnTestConnection.Size = New Size(205, 40)
        btnTestConnection.TabIndex = 6
        btnTestConnection.Text = "🔌 Test Connection"
        btnTestConnection.UseVisualStyleBackColor = False
        ' 
        ' txtFtpPassword
        ' 
        txtFtpPassword.Font = New Font("Segoe UI", 10F)
        txtFtpPassword.Location = New Point(15, 170)
        txtFtpPassword.Name = "txtFtpPassword"
        txtFtpPassword.PasswordChar = "*"c
        txtFtpPassword.Size = New Size(205, 25)
        txtFtpPassword.TabIndex = 5
        ' 
        ' txtFtpUsername
        ' 
        txtFtpUsername.Font = New Font("Segoe UI", 10F)
        txtFtpUsername.Location = New Point(15, 120)
        txtFtpUsername.Name = "txtFtpUsername"
        txtFtpUsername.Size = New Size(205, 25)
        txtFtpUsername.TabIndex = 4
        ' 
        ' txtFtpServer
        ' 
        txtFtpServer.Font = New Font("Segoe UI", 10F)
        txtFtpServer.Location = New Point(15, 70)
        txtFtpServer.Name = "txtFtpServer"
        txtFtpServer.Size = New Size(205, 25)
        txtFtpServer.TabIndex = 3
        ' 
        ' lblFtpPassword
        ' 
        lblFtpPassword.AutoSize = True
        lblFtpPassword.Font = New Font("Segoe UI", 9F)
        lblFtpPassword.Location = New Point(15, 150)
        lblFtpPassword.Name = "lblFtpPassword"
        lblFtpPassword.Size = New Size(60, 15)
        lblFtpPassword.TabIndex = 2
        lblFtpPassword.Text = "Password:"
        ' 
        ' lblFtpUsername
        ' 
        lblFtpUsername.AutoSize = True
        lblFtpUsername.Font = New Font("Segoe UI", 9F)
        lblFtpUsername.Location = New Point(15, 100)
        lblFtpUsername.Name = "lblFtpUsername"
        lblFtpUsername.Size = New Size(63, 15)
        lblFtpUsername.TabIndex = 1
        lblFtpUsername.Text = "Username:"
        ' 
        ' lblFtpServer
        ' 
        lblFtpServer.AutoSize = True
        lblFtpServer.Font = New Font("Segoe UI", 9F)
        lblFtpServer.Location = New Point(15, 50)
        lblFtpServer.Name = "lblFtpServer"
        lblFtpServer.Size = New Size(42, 15)
        lblFtpServer.TabIndex = 0
        lblFtpServer.Text = "Server:"
        ' 
        ' rightPanel
        ' 
        rightPanel.Controls.Add(grpSync)
        rightPanel.Controls.Add(grpSyncSettings)
        rightPanel.Dock = DockStyle.Fill
        rightPanel.Location = New Point(0, 0)
        rightPanel.Name = "rightPanel"
        rightPanel.Padding = New Padding(4, 8, 8, 8)
        rightPanel.Size = New Size(700, 315)
        rightPanel.TabIndex = 0
        ' 
        ' grpSync
        ' 
        grpSync.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        grpSync.Controls.Add(pnlSyncControls)
        grpSync.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        grpSync.Location = New Point(4, 210)
        grpSync.Name = "grpSync"
        grpSync.Padding = New Padding(8)
        grpSync.Size = New Size(688, 97)
        grpSync.TabIndex = 2
        grpSync.TabStop = False
        grpSync.Text = "Synchronization Actions"
        ' 
        ' pnlSyncControls
        ' 
        pnlSyncControls.Controls.Add(progressBar)
        pnlSyncControls.Controls.Add(lblProgress)
        pnlSyncControls.Controls.Add(btnSync)
        pnlSyncControls.Controls.Add(btnViewLogs)
        pnlSyncControls.Controls.Add(btnCleanFolders)
        pnlSyncControls.Dock = DockStyle.Fill
        pnlSyncControls.Font = New Font("Segoe UI", 9F)
        pnlSyncControls.Location = New Point(8, 26)
        pnlSyncControls.Name = "pnlSyncControls"
        pnlSyncControls.Size = New Size(672, 63)
        pnlSyncControls.TabIndex = 0
        ' 
        ' progressBar
        ' 
        progressBar.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        progressBar.Location = New Point(367, 30)
        progressBar.Name = "progressBar"
        progressBar.Size = New Size(292, 20)
        progressBar.Style = ProgressBarStyle.Continuous
        progressBar.TabIndex = 5
        progressBar.Visible = False
        ' 
        ' lblProgress
        ' 
        lblProgress.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        lblProgress.AutoSize = True
        lblProgress.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        lblProgress.ForeColor = Color.FromArgb(CByte(40), CByte(167), CByte(69))
        lblProgress.Location = New Point(363, 9)
        lblProgress.Name = "lblProgress"
        lblProgress.Size = New Size(51, 19)
        lblProgress.TabIndex = 4
        lblProgress.Text = "Ready"
        ' 
        ' btnSync
        ' 
        btnSync.BackColor = Color.FromArgb(CByte(220), CByte(53), CByte(69))
        btnSync.FlatStyle = FlatStyle.Flat
        btnSync.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        btnSync.ForeColor = Color.White
        btnSync.Location = New Point(8, 12)
        btnSync.Name = "btnSync"
        btnSync.Size = New Size(112, 39)
        btnSync.TabIndex = 3
        btnSync.Text = "🔄 Sync"
        btnSync.UseVisualStyleBackColor = False
        ' 
        ' btnViewLogs
        ' 
        btnViewLogs.BackColor = Color.FromArgb(CByte(108), CByte(117), CByte(125))
        btnViewLogs.FlatStyle = FlatStyle.Flat
        btnViewLogs.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnViewLogs.ForeColor = Color.White
        btnViewLogs.Location = New Point(127, 12)
        btnViewLogs.Name = "btnViewLogs"
        btnViewLogs.Size = New Size(110, 39)
        btnViewLogs.TabIndex = 6
        btnViewLogs.Text = "📄 Logs"
        btnViewLogs.UseVisualStyleBackColor = False
        ' 
        ' btnCleanFolders
        ' 
        btnCleanFolders.BackColor = Color.FromArgb(CByte(255), CByte(193), CByte(7))
        btnCleanFolders.FlatStyle = FlatStyle.Flat
        btnCleanFolders.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnCleanFolders.ForeColor = Color.Black
        btnCleanFolders.Location = New Point(245, 12)
        btnCleanFolders.Name = "btnCleanFolders"
        btnCleanFolders.Size = New Size(110, 39)
        btnCleanFolders.TabIndex = 7
        btnCleanFolders.Text = ChrW(55358) & ChrW(56825) & " Clean"
        btnCleanFolders.UseVisualStyleBackColor = False
        ' 
        ' grpSyncSettings
        ' 
        grpSyncSettings.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        grpSyncSettings.Controls.Add(splitContainer2)
        grpSyncSettings.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        grpSyncSettings.Location = New Point(4, 8)
        grpSyncSettings.Name = "grpSyncSettings"
        grpSyncSettings.Padding = New Padding(8)
        grpSyncSettings.Size = New Size(688, 199)
        grpSyncSettings.TabIndex = 1
        grpSyncSettings.TabStop = False
        grpSyncSettings.Text = "Synchronization Settings"
        ' 
        ' splitContainer2
        ' 
        splitContainer2.Dock = DockStyle.Fill
        splitContainer2.Location = New Point(8, 26)
        splitContainer2.Name = "splitContainer2"
        splitContainer2.Orientation = Orientation.Horizontal
        ' 
        ' splitContainer2.Panel1
        ' 
        splitContainer2.Panel1.Controls.Add(pnlSyncTop)
        ' 
        ' splitContainer2.Panel2
        ' 
        splitContainer2.Panel2.Controls.Add(pnlSyncBottom)
        splitContainer2.Size = New Size(672, 165)
        splitContainer2.SplitterDistance = 136
        splitContainer2.TabIndex = 0
        ' 
        ' pnlSyncTop
        ' 
        pnlSyncTop.Controls.Add(tlpSyncTop)
        pnlSyncTop.Dock = DockStyle.Fill
        pnlSyncTop.Location = New Point(0, 0)
        pnlSyncTop.Name = "pnlSyncTop"
        pnlSyncTop.Size = New Size(672, 136)
        pnlSyncTop.TabIndex = 0
        ' 
        ' tlpSyncTop
        ' 
        tlpSyncTop.ColumnCount = 3
        tlpSyncTop.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 120F))
        tlpSyncTop.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100F))
        tlpSyncTop.ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 80F))
        tlpSyncTop.Controls.Add(lblSyncDirection, 0, 0)
        tlpSyncTop.Controls.Add(grpDirection, 1, 0)
        tlpSyncTop.Controls.Add(lblLocalFolder, 0, 1)
        tlpSyncTop.Controls.Add(txtLocalFolder, 1, 1)
        tlpSyncTop.Controls.Add(btnBrowseLocal, 2, 1)
        tlpSyncTop.Controls.Add(lblRemoteFolder, 0, 2)
        tlpSyncTop.Controls.Add(txtRemoteFolder, 1, 2)
        tlpSyncTop.Dock = DockStyle.Fill
        tlpSyncTop.Font = New Font("Segoe UI", 9F)
        tlpSyncTop.Location = New Point(0, 0)
        tlpSyncTop.Name = "tlpSyncTop"
        tlpSyncTop.RowCount = 3
        tlpSyncTop.RowStyles.Add(New RowStyle(SizeType.Absolute, 60F))
        tlpSyncTop.RowStyles.Add(New RowStyle(SizeType.Absolute, 40F))
        tlpSyncTop.RowStyles.Add(New RowStyle(SizeType.Absolute, 40F))
        tlpSyncTop.Size = New Size(672, 136)
        tlpSyncTop.TabIndex = 0
        ' 
        ' lblSyncDirection
        ' 
        lblSyncDirection.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        lblSyncDirection.AutoSize = True
        lblSyncDirection.Location = New Point(3, 22)
        lblSyncDirection.Name = "lblSyncDirection"
        lblSyncDirection.Size = New Size(114, 15)
        lblSyncDirection.TabIndex = 0
        lblSyncDirection.Text = "Sync Direction:"
        ' 
        ' grpDirection
        ' 
        grpDirection.Controls.Add(rbRemoteToLocal)
        grpDirection.Controls.Add(rbLocalToRemote)
        grpDirection.Dock = DockStyle.Fill
        grpDirection.Location = New Point(123, 3)
        grpDirection.Name = "grpDirection"
        grpDirection.Size = New Size(466, 54)
        grpDirection.TabIndex = 1
        grpDirection.TabStop = False
        ' 
        ' rbRemoteToLocal
        ' 
        rbRemoteToLocal.AutoSize = True
        rbRemoteToLocal.Checked = True
        rbRemoteToLocal.Location = New Point(10, 20)
        rbRemoteToLocal.Name = "rbRemoteToLocal"
        rbRemoteToLocal.Size = New Size(111, 19)
        rbRemoteToLocal.TabIndex = 0
        rbRemoteToLocal.TabStop = True
        rbRemoteToLocal.Text = "Remote to Local"
        rbRemoteToLocal.UseVisualStyleBackColor = True
        ' 
        ' rbLocalToRemote
        ' 
        rbLocalToRemote.AutoSize = True
        rbLocalToRemote.Location = New Point(140, 20)
        rbLocalToRemote.Name = "rbLocalToRemote"
        rbLocalToRemote.Size = New Size(111, 19)
        rbLocalToRemote.TabIndex = 1
        rbLocalToRemote.Text = "Local to Remote"
        rbLocalToRemote.UseVisualStyleBackColor = True
        ' 
        ' lblLocalFolder
        ' 
        lblLocalFolder.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        lblLocalFolder.AutoSize = True
        lblLocalFolder.Location = New Point(3, 72)
        lblLocalFolder.Name = "lblLocalFolder"
        lblLocalFolder.Size = New Size(114, 15)
        lblLocalFolder.TabIndex = 2
        lblLocalFolder.Text = "Local Folder:"
        ' 
        ' txtLocalFolder
        ' 
        txtLocalFolder.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        txtLocalFolder.Enabled = False
        txtLocalFolder.Location = New Point(123, 68)
        txtLocalFolder.Name = "txtLocalFolder"
        txtLocalFolder.ReadOnly = True
        txtLocalFolder.Size = New Size(466, 23)
        txtLocalFolder.TabIndex = 3
        ' 
        ' btnBrowseLocal
        ' 
        btnBrowseLocal.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        btnBrowseLocal.BackColor = Color.FromArgb(CByte(108), CByte(117), CByte(125))
        btnBrowseLocal.FlatStyle = FlatStyle.Flat
        btnBrowseLocal.ForeColor = Color.White
        btnBrowseLocal.Location = New Point(595, 63)
        btnBrowseLocal.Name = "btnBrowseLocal"
        btnBrowseLocal.Size = New Size(74, 34)
        btnBrowseLocal.TabIndex = 4
        btnBrowseLocal.Text = "Browse"
        btnBrowseLocal.UseVisualStyleBackColor = False
        ' 
        ' lblRemoteFolder
        ' 
        lblRemoteFolder.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        lblRemoteFolder.AutoSize = True
        lblRemoteFolder.Location = New Point(3, 112)
        lblRemoteFolder.Name = "lblRemoteFolder"
        lblRemoteFolder.Size = New Size(114, 15)
        lblRemoteFolder.TabIndex = 5
        lblRemoteFolder.Text = "Remote Folder:"
        ' 
        ' txtRemoteFolder
        ' 
        txtRemoteFolder.Anchor = AnchorStyles.Left Or AnchorStyles.Right
        txtRemoteFolder.Location = New Point(123, 108)
        txtRemoteFolder.Name = "txtRemoteFolder"
        txtRemoteFolder.Size = New Size(466, 23)
        txtRemoteFolder.TabIndex = 6
        ' 
        ' pnlSyncBottom
        ' 
        pnlSyncBottom.Controls.Add(lblStatus)
        pnlSyncBottom.Dock = DockStyle.Fill
        pnlSyncBottom.Location = New Point(0, 0)
        pnlSyncBottom.Name = "pnlSyncBottom"
        pnlSyncBottom.Size = New Size(672, 25)
        pnlSyncBottom.TabIndex = 0
        ' 
        ' lblStatus
        ' 
        lblStatus.AutoSize = True
        lblStatus.Font = New Font("Segoe UI", 9F)
        lblStatus.Location = New Point(3, 8)
        lblStatus.Name = "lblStatus"
        lblStatus.Size = New Size(42, 15)
        lblStatus.TabIndex = 6
        lblStatus.Text = "Status:"
        ' 
        ' menuStrip1
        ' 
        menuStrip1.Items.AddRange(New ToolStripItem() {fileToolStripMenuItem, helpToolStripMenuItem})
        menuStrip1.Location = New Point(0, 0)
        menuStrip1.Name = "menuStrip1"
        menuStrip1.Size = New Size(961, 24)
        menuStrip1.TabIndex = 1
        ' 
        ' fileToolStripMenuItem
        ' 
        fileToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {settingsToolStripMenuItem, toolStripSeparator1, exitToolStripMenuItem})
        fileToolStripMenuItem.Name = "fileToolStripMenuItem"
        fileToolStripMenuItem.Size = New Size(37, 20)
        fileToolStripMenuItem.Text = "&File"
        ' 
        ' settingsToolStripMenuItem
        ' 
        settingsToolStripMenuItem.Name = "settingsToolStripMenuItem"
        settingsToolStripMenuItem.Size = New Size(135, 22)
        settingsToolStripMenuItem.Text = "&Settings..."
        ' 
        ' toolStripSeparator1
        ' 
        toolStripSeparator1.Name = "toolStripSeparator1"
        toolStripSeparator1.Size = New Size(132, 6)
        ' 
        ' exitToolStripMenuItem
        ' 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem"
        exitToolStripMenuItem.ShortcutKeys = Keys.Alt Or Keys.F4
        exitToolStripMenuItem.Size = New Size(135, 22)
        exitToolStripMenuItem.Text = "E&xit"
        ' 
        ' helpToolStripMenuItem
        ' 
        helpToolStripMenuItem.DropDownItems.AddRange(New ToolStripItem() {aboutToolStripMenuItem})
        helpToolStripMenuItem.Name = "helpToolStripMenuItem"
        helpToolStripMenuItem.Size = New Size(44, 20)
        helpToolStripMenuItem.Text = "&Help"
        ' 
        ' aboutToolStripMenuItem
        ' 
        aboutToolStripMenuItem.Name = "aboutToolStripMenuItem"
        aboutToolStripMenuItem.Size = New Size(107, 22)
        aboutToolStripMenuItem.Text = "&About"
        ' 
        ' statusStrip
        ' 
        statusStrip.Items.AddRange(New ToolStripItem() {toolStripStatusLabel})
        statusStrip.Location = New Point(0, 339)
        statusStrip.Name = "statusStrip"
        statusStrip.Padding = New Padding(1, 0, 16, 0)
        statusStrip.Size = New Size(961, 22)
        statusStrip.TabIndex = 2
        ' 
        ' toolStripStatusLabel
        ' 
        toolStripStatusLabel.Name = "toolStripStatusLabel"
        toolStripStatusLabel.Size = New Size(39, 17)
        toolStripStatusLabel.Text = "Ready"
        ' 
        ' folderBrowserDialog
        ' 
        folderBrowserDialog.Description = "Select project folder"
        ' 
        ' frmMain
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(248), CByte(249), CByte(250))
        ClientSize = New Size(961, 361)
        Controls.Add(splitContainer1)
        Controls.Add(statusStrip)
        Controls.Add(menuStrip1)
        Font = New Font("Segoe UI", 9F)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MainMenuStrip = menuStrip1
        MaximizeBox = False
        MinimumSize = New Size(600, 400)
        Name = "frmMain"
        StartPosition = FormStartPosition.CenterScreen
        Text = "FTP Folder Synchronizer v2.0"
        splitContainer1.Panel1.ResumeLayout(False)
        splitContainer1.Panel2.ResumeLayout(False)
        CType(splitContainer1, ComponentModel.ISupportInitialize).EndInit()
        splitContainer1.ResumeLayout(False)
        leftPanel.ResumeLayout(False)
        grpFtpSettings.ResumeLayout(False)
        grpFtpSettings.PerformLayout()
        rightPanel.ResumeLayout(False)
        grpSync.ResumeLayout(False)
        pnlSyncControls.ResumeLayout(False)
        pnlSyncControls.PerformLayout()
        grpSyncSettings.ResumeLayout(False)
        splitContainer2.Panel1.ResumeLayout(False)
        splitContainer2.Panel2.ResumeLayout(False)
        CType(splitContainer2, ComponentModel.ISupportInitialize).EndInit()
        splitContainer2.ResumeLayout(False)
        pnlSyncTop.ResumeLayout(False)
        tlpSyncTop.ResumeLayout(False)
        tlpSyncTop.PerformLayout()
        grpDirection.ResumeLayout(False)
        grpDirection.PerformLayout()
        pnlSyncBottom.ResumeLayout(False)
        pnlSyncBottom.PerformLayout()
        menuStrip1.ResumeLayout(False)
        menuStrip1.PerformLayout()
        statusStrip.ResumeLayout(False)
        statusStrip.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    ' Control declarations with same names as original for compatibility
    Friend WithEvents splitContainer1 As SplitContainer
    Friend WithEvents leftPanel As Panel
    Friend WithEvents rightPanel As Panel

    ' FTP Settings Group (left panel)
    Friend WithEvents grpFtpSettings As GroupBox
    Friend WithEvents lblServerHint As Label
    Friend WithEvents btnTestConnection As Button
    Friend WithEvents txtFtpPassword As TextBox
    Friend WithEvents txtFtpUsername As TextBox
    Friend WithEvents txtFtpServer As TextBox
    Friend WithEvents lblFtpPassword As Label
    Friend WithEvents lblFtpUsername As Label
    Friend WithEvents lblFtpServer As Label

    ' Sync Settings Group (right panel top)
    Friend WithEvents grpSyncSettings As GroupBox
    Friend WithEvents splitContainer2 As SplitContainer
    Friend WithEvents pnlSyncTop As Panel
    Friend WithEvents tlpSyncTop As TableLayoutPanel
    Friend WithEvents lblSyncDirection As Label
    Friend WithEvents grpDirection As GroupBox
    Friend WithEvents rbRemoteToLocal As RadioButton
    Friend WithEvents rbLocalToRemote As RadioButton
    Friend WithEvents lblLocalFolder As Label
    Friend WithEvents txtLocalFolder As TextBox
    Friend WithEvents btnBrowseLocal As Button
    Friend WithEvents lblRemoteFolder As Label
    Friend WithEvents txtRemoteFolder As TextBox
    Friend WithEvents pnlSyncBottom As Panel
    Friend WithEvents lblStatus As Label

    ' Sync Actions Group (right panel bottom)
    Friend WithEvents grpSync As GroupBox
    Friend WithEvents pnlSyncControls As Panel
    Friend WithEvents btnSync As Button
    Friend WithEvents btnViewLogs As Button
    Friend WithEvents btnCleanFolders As Button
    Friend WithEvents lblProgress As Label
    Friend WithEvents progressBar As ProgressBar

    ' Menu and Status - keeping original names
    Friend WithEvents menuStrip1 As MenuStrip
    Friend WithEvents fileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents settingsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents toolStripSeparator1 As ToolStripSeparator
    Friend WithEvents exitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents helpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents aboutToolStripMenuItem As ToolStripMenuItem

    Friend WithEvents statusStrip As StatusStrip
    Friend WithEvents toolStripStatusLabel As ToolStripStatusLabel

    ' Dialogs - keeping original names
    Friend WithEvents folderBrowserDialog As FolderBrowserDialog
    Friend WithEvents toolTip As ToolTip

End Class