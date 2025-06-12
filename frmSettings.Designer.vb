<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSettings
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
        grpCleanSettings = New GroupBox()
        lblCleanFolders = New Label()
        txtCleanFolders = New TextBox()
        lblCleanHint = New Label()
        chkForceDelete = New CheckBox()
        btnAddFolder = New Button()
        btnReset = New Button()
        pnlButtons = New Panel()
        btnCancel = New Button()
        btnSave = New Button()
        grpCleanSettings.SuspendLayout()
        pnlButtons.SuspendLayout()
        SuspendLayout()
        ' 
        ' grpCleanSettings
        ' 
        grpCleanSettings.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        grpCleanSettings.Controls.Add(lblCleanFolders)
        grpCleanSettings.Controls.Add(txtCleanFolders)
        grpCleanSettings.Controls.Add(lblCleanHint)
        grpCleanSettings.Controls.Add(chkForceDelete)
        grpCleanSettings.Controls.Add(btnAddFolder)
        grpCleanSettings.Controls.Add(btnReset)
        grpCleanSettings.Font = New Font("Segoe UI", 9.75F, FontStyle.Bold)
        grpCleanSettings.Location = New Point(12, 12)
        grpCleanSettings.Name = "grpCleanSettings"
        grpCleanSettings.Padding = New Padding(8)
        grpCleanSettings.Size = New Size(560, 200)
        grpCleanSettings.TabIndex = 0
        grpCleanSettings.TabStop = False
        grpCleanSettings.Text = "Clean Folder Settings"
        ' 
        ' lblCleanFolders
        ' 
        lblCleanFolders.AutoSize = True
        lblCleanFolders.Font = New Font("Segoe UI", 9F)
        lblCleanFolders.Location = New Point(15, 30)
        lblCleanFolders.Name = "lblCleanFolders"
        lblCleanFolders.Size = New Size(122, 15)
        lblCleanFolders.TabIndex = 0
        lblCleanFolders.Text = "Folders to clean (CSV):"
        ' 
        ' txtCleanFolders
        ' 
        txtCleanFolders.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        txtCleanFolders.Font = New Font("Segoe UI", 10F)
        txtCleanFolders.Location = New Point(15, 50)
        txtCleanFolders.Multiline = True
        txtCleanFolders.Name = "txtCleanFolders"
        txtCleanFolders.ScrollBars = ScrollBars.Vertical
        txtCleanFolders.Size = New Size(530, 60)
        txtCleanFolders.TabIndex = 1
        ' 
        ' lblCleanHint
        ' 
        lblCleanHint.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblCleanHint.Font = New Font("Segoe UI", 8F, FontStyle.Italic)
        lblCleanHint.ForeColor = Color.Gray
        lblCleanHint.Location = New Point(15, 115)
        lblCleanHint.Name = "lblCleanHint"
        lblCleanHint.Size = New Size(530, 30)
        lblCleanHint.TabIndex = 2
        lblCleanHint.Text = "Enter folder names separated by commas (e.g., bin,obj,packages,.vs,Debug,Release). All files and subfolders inside these folders will be deleted, but the folders themselves will remain."
        ' 
        ' chkForceDelete
        ' 
        chkForceDelete.AutoSize = True
        chkForceDelete.Checked = True
        chkForceDelete.CheckState = CheckState.Checked
        chkForceDelete.Font = New Font("Segoe UI", 9F)
        chkForceDelete.Location = New Point(15, 150)
        chkForceDelete.Name = "chkForceDelete"
        chkForceDelete.Size = New Size(248, 19)
        chkForceDelete.TabIndex = 3
        chkForceDelete.Text = "Force delete (remove read-only attributes)"
        chkForceDelete.UseVisualStyleBackColor = True
        ' 
        ' btnAddFolder
        ' 
        btnAddFolder.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnAddFolder.BackColor = Color.FromArgb(CByte(0), CByte(120), CByte(215))
        btnAddFolder.FlatStyle = FlatStyle.Flat
        btnAddFolder.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        btnAddFolder.ForeColor = Color.White
        btnAddFolder.Location = New Point(380, 145)
        btnAddFolder.Name = "btnAddFolder"
        btnAddFolder.Size = New Size(80, 30)
        btnAddFolder.TabIndex = 4
        btnAddFolder.Text = "Add Folder"
        btnAddFolder.UseVisualStyleBackColor = False
        ' 
        ' btnReset
        ' 
        btnReset.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnReset.BackColor = Color.FromArgb(CByte(108), CByte(117), CByte(125))
        btnReset.FlatStyle = FlatStyle.Flat
        btnReset.Font = New Font("Segoe UI", 9F, FontStyle.Bold)
        btnReset.ForeColor = Color.White
        btnReset.Location = New Point(465, 145)
        btnReset.Name = "btnReset"
        btnReset.Size = New Size(80, 30)
        btnReset.TabIndex = 5
        btnReset.Text = "Reset"
        btnReset.UseVisualStyleBackColor = False
        ' 
        ' pnlButtons
        ' 
        pnlButtons.Controls.Add(btnCancel)
        pnlButtons.Controls.Add(btnSave)
        pnlButtons.Dock = DockStyle.Bottom
        pnlButtons.Location = New Point(0, 225)
        pnlButtons.Name = "pnlButtons"
        pnlButtons.Size = New Size(584, 50)
        pnlButtons.TabIndex = 1
        ' 
        ' btnCancel
        ' 
        btnCancel.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnCancel.BackColor = Color.FromArgb(CByte(220), CByte(53), CByte(69))
        btnCancel.DialogResult = DialogResult.Cancel
        btnCancel.FlatStyle = FlatStyle.Flat
        btnCancel.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnCancel.ForeColor = Color.White
        btnCancel.Location = New Point(490, 10)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(82, 30)
        btnCancel.TabIndex = 1
        btnCancel.Text = "Cancel"
        btnCancel.UseVisualStyleBackColor = False
        ' 
        ' btnSave
        ' 
        btnSave.Anchor = AnchorStyles.Bottom Or AnchorStyles.Right
        btnSave.BackColor = Color.FromArgb(CByte(40), CByte(167), CByte(69))
        btnSave.FlatStyle = FlatStyle.Flat
        btnSave.Font = New Font("Segoe UI", 10F, FontStyle.Bold)
        btnSave.ForeColor = Color.White
        btnSave.Location = New Point(400, 10)
        btnSave.Name = "btnSave"
        btnSave.Size = New Size(82, 30)
        btnSave.TabIndex = 0
        btnSave.Text = "Save"
        btnSave.UseVisualStyleBackColor = False
        ' 
        ' frmSettings
        ' 
        AcceptButton = btnSave
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = Color.FromArgb(CByte(248), CByte(249), CByte(250))
        CancelButton = btnCancel
        ClientSize = New Size(584, 275)
        Controls.Add(pnlButtons)
        Controls.Add(grpCleanSettings)
        Font = New Font("Segoe UI", 9F)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "frmSettings"
        ShowIcon = False
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterParent
        Text = "Settings"
        grpCleanSettings.ResumeLayout(False)
        grpCleanSettings.PerformLayout()
        pnlButtons.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents grpCleanSettings As GroupBox
    Friend WithEvents lblCleanFolders As Label
    Friend WithEvents txtCleanFolders As TextBox
    Friend WithEvents lblCleanHint As Label
    Friend WithEvents chkForceDelete As CheckBox
    Friend WithEvents btnAddFolder As Button
    Friend WithEvents btnReset As Button
    Friend WithEvents pnlButtons As Panel
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnSave As Button

End Class