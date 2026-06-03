<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        txtUrl = New TextBox()
        cmbFormat = New ComboBox()
        ProgressBar1 = New ProgressBar()
        txtOutput = New TextBox()
        btnDownload = New Button()
        cmbQuality = New ComboBox()
        Label1 = New Label()
        btnBrowse = New Button()
        btnOpenFolder = New Button()
        chkDarkMode = New CheckBox()
        txtFolder = New TextBox()
        lblCurrentTask = New Label()
        lblCurrentFile = New Label()
        lblCurrentQuality = New Label()
        lblProgressPercent = New Label()
        lstHistory = New ListBox()
        grpStatus = New GroupBox()
        lblETA = New Label()
        lblSpeed = New Label()
        picThumbnail = New PictureBox()
        GroupBox1 = New GroupBox()
        lblFPS = New Label()
        lblPlaylistCount = New Label()
        lblPlaylistTitle = New Label()
        lblPlaylistSize = New Label()
        lblPreviewVideo = New Label()
        lblDuration = New Label()
        lblCodec = New Label()
        lblFileSize = New Label()
        lblUploader = New Label()
        lblTitle = New Label()
        grpUpdates = New GroupBox()
        lblFfmpegVersion = New Label()
        lblYtDlpVersion = New Label()
        btnCheckUpdates = New Button()
        grpStatus.SuspendLayout()
        CType(picThumbnail, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox1.SuspendLayout()
        grpUpdates.SuspendLayout()
        SuspendLayout()
        ' 
        ' txtUrl
        ' 
        txtUrl.Location = New Point(46, 340)
        txtUrl.Multiline = True
        txtUrl.Name = "txtUrl"
        txtUrl.Size = New Size(605, 35)
        txtUrl.TabIndex = 0
        ' 
        ' cmbFormat
        ' 
        cmbFormat.DropDownStyle = ComboBoxStyle.DropDownList
        cmbFormat.FormattingEnabled = True
        cmbFormat.Items.AddRange(New Object() {"Video (MP4)", "Audio (MP3)"})
        cmbFormat.Location = New Point(46, 391)
        cmbFormat.Name = "cmbFormat"
        cmbFormat.Size = New Size(107, 23)
        cmbFormat.TabIndex = 1
        ' 
        ' ProgressBar1
        ' 
        ProgressBar1.Location = New Point(46, 497)
        ProgressBar1.Name = "ProgressBar1"
        ProgressBar1.Size = New Size(605, 61)
        ProgressBar1.TabIndex = 2
        ' 
        ' txtOutput
        ' 
        txtOutput.Location = New Point(39, 808)
        txtOutput.Multiline = True
        txtOutput.Name = "txtOutput"
        txtOutput.Size = New Size(612, 76)
        txtOutput.TabIndex = 3
        ' 
        ' btnDownload
        ' 
        btnDownload.Location = New Point(323, 390)
        btnDownload.Name = "btnDownload"
        btnDownload.Size = New Size(169, 23)
        btnDownload.TabIndex = 4
        btnDownload.Text = "Download"
        btnDownload.UseVisualStyleBackColor = True
        ' 
        ' cmbQuality
        ' 
        cmbQuality.DropDownStyle = ComboBoxStyle.DropDownList
        cmbQuality.FormattingEnabled = True
        cmbQuality.Location = New Point(159, 391)
        cmbQuality.Name = "cmbQuality"
        cmbQuality.Size = New Size(100, 23)
        cmbQuality.TabIndex = 5
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Trebuchet MS", 36F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(148, 9)
        Label1.Name = "Label1"
        Label1.Size = New Size(412, 61)
        Label1.TabIndex = 6
        Label1.Text = "VBYTDownloader"
        ' 
        ' btnBrowse
        ' 
        btnBrowse.Location = New Point(522, 408)
        btnBrowse.Name = "btnBrowse"
        btnBrowse.Size = New Size(129, 23)
        btnBrowse.TabIndex = 7
        btnBrowse.Text = "Browse Folder"
        btnBrowse.UseVisualStyleBackColor = True
        ' 
        ' btnOpenFolder
        ' 
        btnOpenFolder.Location = New Point(522, 437)
        btnOpenFolder.Name = "btnOpenFolder"
        btnOpenFolder.Size = New Size(129, 23)
        btnOpenFolder.TabIndex = 8
        btnOpenFolder.Text = "Open Folder"
        btnOpenFolder.UseVisualStyleBackColor = True
        ' 
        ' chkDarkMode
        ' 
        chkDarkMode.AutoSize = True
        chkDarkMode.Location = New Point(49, 472)
        chkDarkMode.Name = "chkDarkMode"
        chkDarkMode.Size = New Size(84, 19)
        chkDarkMode.TabIndex = 9
        chkDarkMode.Text = "Dark Mode"
        chkDarkMode.UseVisualStyleBackColor = True
        ' 
        ' txtFolder
        ' 
        txtFolder.Location = New Point(45, 420)
        txtFolder.Multiline = True
        txtFolder.Name = "txtFolder"
        txtFolder.Size = New Size(450, 47)
        txtFolder.TabIndex = 10
        ' 
        ' lblCurrentTask
        ' 
        lblCurrentTask.AutoSize = True
        lblCurrentTask.Location = New Point(6, 19)
        lblCurrentTask.Name = "lblCurrentTask"
        lblCurrentTask.Size = New Size(39, 15)
        lblCurrentTask.TabIndex = 11
        lblCurrentTask.Text = "Ready"
        ' 
        ' lblCurrentFile
        ' 
        lblCurrentFile.AutoSize = True
        lblCurrentFile.Location = New Point(6, 43)
        lblCurrentFile.Name = "lblCurrentFile"
        lblCurrentFile.Size = New Size(39, 15)
        lblCurrentFile.TabIndex = 12
        lblCurrentFile.Text = "Ready"
        ' 
        ' lblCurrentQuality
        ' 
        lblCurrentQuality.AutoSize = True
        lblCurrentQuality.Location = New Point(6, 67)
        lblCurrentQuality.Name = "lblCurrentQuality"
        lblCurrentQuality.Size = New Size(39, 15)
        lblCurrentQuality.TabIndex = 13
        lblCurrentQuality.Text = "Ready"
        ' 
        ' lblProgressPercent
        ' 
        lblProgressPercent.AutoSize = True
        lblProgressPercent.Location = New Point(6, 88)
        lblProgressPercent.Name = "lblProgressPercent"
        lblProgressPercent.Size = New Size(39, 15)
        lblProgressPercent.TabIndex = 14
        lblProgressPercent.Text = "Ready"
        ' 
        ' lstHistory
        ' 
        lstHistory.FormattingEnabled = True
        lstHistory.Location = New Point(42, 738)
        lstHistory.Name = "lstHistory"
        lstHistory.Size = New Size(609, 64)
        lstHistory.TabIndex = 15
        ' 
        ' grpStatus
        ' 
        grpStatus.Controls.Add(lblETA)
        grpStatus.Controls.Add(lblSpeed)
        grpStatus.Controls.Add(lblCurrentTask)
        grpStatus.Controls.Add(lblCurrentFile)
        grpStatus.Controls.Add(lblProgressPercent)
        grpStatus.Controls.Add(lblCurrentQuality)
        grpStatus.Location = New Point(49, 575)
        grpStatus.Name = "grpStatus"
        grpStatus.Size = New Size(602, 157)
        grpStatus.TabIndex = 16
        grpStatus.TabStop = False
        grpStatus.Text = "Status"
        ' 
        ' lblETA
        ' 
        lblETA.AutoSize = True
        lblETA.Location = New Point(6, 139)
        lblETA.Name = "lblETA"
        lblETA.Size = New Size(45, 15)
        lblETA.TabIndex = 16
        lblETA.Text = "ETA: —"
        ' 
        ' lblSpeed
        ' 
        lblSpeed.AutoSize = True
        lblSpeed.Location = New Point(6, 114)
        lblSpeed.Name = "lblSpeed"
        lblSpeed.Size = New Size(57, 15)
        lblSpeed.TabIndex = 15
        lblSpeed.Text = "Speed: —"
        ' 
        ' picThumbnail
        ' 
        picThumbnail.BackColor = SystemColors.ControlDarkDark
        picThumbnail.BorderStyle = BorderStyle.FixedSingle
        picThumbnail.Location = New Point(49, 88)
        picThumbnail.Name = "picThumbnail"
        picThumbnail.Size = New Size(309, 246)
        picThumbnail.SizeMode = PictureBoxSizeMode.Zoom
        picThumbnail.TabIndex = 17
        picThumbnail.TabStop = False
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Controls.Add(lblFPS)
        GroupBox1.Controls.Add(lblPlaylistCount)
        GroupBox1.Controls.Add(lblPlaylistTitle)
        GroupBox1.Controls.Add(lblPlaylistSize)
        GroupBox1.Controls.Add(lblPreviewVideo)
        GroupBox1.Controls.Add(lblDuration)
        GroupBox1.Controls.Add(lblCodec)
        GroupBox1.Controls.Add(lblFileSize)
        GroupBox1.Controls.Add(lblUploader)
        GroupBox1.Controls.Add(lblTitle)
        GroupBox1.Location = New Point(364, 88)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Size = New Size(287, 246)
        GroupBox1.TabIndex = 17
        GroupBox1.TabStop = False
        GroupBox1.Text = "Video Information"
        ' 
        ' lblFPS
        ' 
        lblFPS.Location = New Point(6, 169)
        lblFPS.Name = "lblFPS"
        lblFPS.Size = New Size(275, 23)
        lblFPS.TabIndex = 15
        lblFPS.Text = "FPS: Waiting..."
        ' 
        ' lblPlaylistCount
        ' 
        lblPlaylistCount.Location = New Point(6, 148)
        lblPlaylistCount.Name = "lblPlaylistCount"
        lblPlaylistCount.Size = New Size(275, 33)
        lblPlaylistCount.TabIndex = 18
        lblPlaylistCount.Text = "Videos: —"
        ' 
        ' lblPlaylistTitle
        ' 
        lblPlaylistTitle.Location = New Point(6, 126)
        lblPlaylistTitle.Name = "lblPlaylistTitle"
        lblPlaylistTitle.Size = New Size(275, 33)
        lblPlaylistTitle.TabIndex = 17
        lblPlaylistTitle.Text = "Playlist: —"
        ' 
        ' lblPlaylistSize
        ' 
        lblPlaylistSize.Location = New Point(6, 102)
        lblPlaylistSize.Name = "lblPlaylistSize"
        lblPlaylistSize.Size = New Size(275, 33)
        lblPlaylistSize.TabIndex = 19
        lblPlaylistSize.Text = "Estimated Playlist Size: —"
        ' 
        ' lblPreviewVideo
        ' 
        lblPreviewVideo.Location = New Point(6, 80)
        lblPreviewVideo.Name = "lblPreviewVideo"
        lblPreviewVideo.Size = New Size(275, 33)
        lblPreviewVideo.TabIndex = 20
        lblPreviewVideo.Text = "Preview Video: —"
        ' 
        ' lblDuration
        ' 
        lblDuration.Location = New Point(6, 60)
        lblDuration.Name = "lblDuration"
        lblDuration.Size = New Size(275, 33)
        lblDuration.TabIndex = 13
        lblDuration.Text = "Duration: Waiting..."
        ' 
        ' lblCodec
        ' 
        lblCodec.Location = New Point(6, 215)
        lblCodec.Name = "lblCodec"
        lblCodec.Size = New Size(275, 19)
        lblCodec.TabIndex = 16
        lblCodec.Text = "Codec: Waiting..."
        ' 
        ' lblFileSize
        ' 
        lblFileSize.Location = New Point(6, 192)
        lblFileSize.Name = "lblFileSize"
        lblFileSize.Size = New Size(275, 23)
        lblFileSize.TabIndex = 14
        lblFileSize.Text = "Estimated Size: Waiting..."
        ' 
        ' lblUploader
        ' 
        lblUploader.Location = New Point(6, 42)
        lblUploader.Name = "lblUploader"
        lblUploader.Size = New Size(275, 27)
        lblUploader.TabIndex = 12
        lblUploader.Text = "Uploader: Waiting..."
        ' 
        ' lblTitle
        ' 
        lblTitle.Location = New Point(6, 20)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New Size(275, 33)
        lblTitle.TabIndex = 11
        lblTitle.Text = "Title: Waiting..."
        ' 
        ' grpUpdates
        ' 
        grpUpdates.Controls.Add(lblFfmpegVersion)
        grpUpdates.Controls.Add(lblYtDlpVersion)
        grpUpdates.Controls.Add(btnCheckUpdates)
        grpUpdates.Location = New Point(36, 890)
        grpUpdates.Name = "grpUpdates"
        grpUpdates.Size = New Size(615, 52)
        grpUpdates.TabIndex = 19
        grpUpdates.TabStop = False
        grpUpdates.Text = "Updates:"
        ' 
        ' lblFfmpegVersion
        ' 
        lblFfmpegVersion.AutoSize = True
        lblFfmpegVersion.Location = New Point(6, 30)
        lblFfmpegVersion.Name = "lblFfmpegVersion"
        lblFfmpegVersion.Size = New Size(107, 15)
        lblFfmpegVersion.TabIndex = 22
        lblFfmpegVersion.Text = "FFmpeg: Unknown"
        ' 
        ' lblYtDlpVersion
        ' 
        lblYtDlpVersion.AutoSize = True
        lblYtDlpVersion.Location = New Point(6, 14)
        lblYtDlpVersion.Name = "lblYtDlpVersion"
        lblYtDlpVersion.Size = New Size(96, 15)
        lblYtDlpVersion.TabIndex = 21
        lblYtDlpVersion.Text = "yt-dlp: Unknown"
        ' 
        ' btnCheckUpdates
        ' 
        btnCheckUpdates.Location = New Point(480, 14)
        btnCheckUpdates.Name = "btnCheckUpdates"
        btnCheckUpdates.Size = New Size(129, 31)
        btnCheckUpdates.TabIndex = 20
        btnCheckUpdates.Text = "Check Updates"
        btnCheckUpdates.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(702, 942)
        Controls.Add(grpUpdates)
        Controls.Add(GroupBox1)
        Controls.Add(picThumbnail)
        Controls.Add(grpStatus)
        Controls.Add(lstHistory)
        Controls.Add(txtFolder)
        Controls.Add(chkDarkMode)
        Controls.Add(btnOpenFolder)
        Controls.Add(btnBrowse)
        Controls.Add(Label1)
        Controls.Add(cmbQuality)
        Controls.Add(btnDownload)
        Controls.Add(txtOutput)
        Controls.Add(ProgressBar1)
        Controls.Add(cmbFormat)
        Controls.Add(txtUrl)
        Name = "Form1"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Form1"
        grpStatus.ResumeLayout(False)
        grpStatus.PerformLayout()
        CType(picThumbnail, ComponentModel.ISupportInitialize).EndInit()
        GroupBox1.ResumeLayout(False)
        grpUpdates.ResumeLayout(False)
        grpUpdates.PerformLayout()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents txtUrl As TextBox
    Friend WithEvents cmbFormat As ComboBox
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents txtOutput As TextBox
    Friend WithEvents btnDownload As Button
    Friend WithEvents cmbQuality As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents btnBrowse As Button
    Friend WithEvents btnOpenFolder As Button
    Friend WithEvents chkDarkMode As CheckBox
    Friend WithEvents txtFolder As TextBox
    Friend WithEvents lblCurrentTask As Label
    Friend WithEvents lblCurrentFile As Label
    Friend WithEvents lblCurrentQuality As Label
    Friend WithEvents lblProgressPercent As Label
    Friend WithEvents lstHistory As ListBox
    Friend WithEvents grpStatus As GroupBox
    Friend WithEvents picThumbnail As PictureBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblTitle As Label
    Friend WithEvents lblUploader As Label
    Friend WithEvents lblFileSize As Label
    Friend WithEvents lblDuration As Label
    Friend WithEvents grpUpdates As GroupBox
    Friend WithEvents lblYtDlpVersion As Label
    Friend WithEvents btnCheckUpdates As Button
    Friend WithEvents lblFfmpegVersion As Label
    Friend WithEvents lblETA As Label
    Friend WithEvents lblSpeed As Label
    Friend WithEvents lblFPS As Label
    Friend WithEvents lblCodec As Label
    Friend WithEvents lblPlaylistTitle As Label
    Friend WithEvents lblPlaylistCount As Label
    Friend WithEvents lblPlaylistSize As Label
    Friend WithEvents lblPreviewVideo As Label

End Class
