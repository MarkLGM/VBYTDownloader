Imports System.Diagnostics
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Text
Imports System.Net.Http
Imports System.Text.Json
Imports System.Threading
Imports System.Threading.Tasks

Public Class Form1
    Private videoTitle As String = ""
    Private uploaderName As String = ""
    Private videoDuration As String = ""
    Private videoSize As String = ""
    Private currentFileName As String = ""
    Private currentQuality As String = ""
    Private ytDlpPath As String = Path.Combine(Application.StartupPath, "yt-dlp.exe")
    Private ffmpegPath As String =
    Path.Combine(Application.StartupPath, "ffmpeg.exe")
    Private downloadCompleted As Boolean = False
    Private _metaCts As CancellationTokenSource
    Private ReadOnly _http As New HttpClient()
    Private formatSizeCache As New Dictionary(Of String, String)
    Private lastLoadedUrl As String = ""
    Private formatCache As New Dictionary(Of String, FormatInfo)
    Private audioSizeMB As Double = 0



    Private Class FormatInfo

        Public Property SizeMB As Double
        Public Property FPS As Integer
        Public Property FormatID As String
        Public Property VideoCodec As String

    End Class


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = "VBYTDownloader v1.0"
        cmbFormat.SelectedIndex = 0 ' Default to Video

        ' Quality
        cmbQuality.Items.Add("360p")
        cmbQuality.Items.Add("480p")
        cmbQuality.Items.Add("720p")
        cmbQuality.Items.Add("1080p")
        cmbQuality.SelectedIndex = 2

        RefreshVersions()
        _http.DefaultRequestHeaders.UserAgent.ParseAdd(
            "VBYTDownloader")
        txtFolder.ReadOnly = True

        txtFolder.Text =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.UserProfile),
                "Downloads")
        If File.Exists("history.txt") Then

            lstHistory.Items.AddRange(
                File.ReadAllLines("history.txt"))

        End If
        lblCurrentTask.Text = "Task: Ready"
        lblCurrentFile.Text = "File: None"
        lblCurrentQuality.Text = "Quality: None"
        lblProgressPercent.Text = "Progress: 0%"
        lblTitle.Text = "Title: Waiting..."
        lblUploader.Text = "Uploader: Waiting..."
        lblDuration.Text = "Duration: Waiting..."
        lblFileSize.Text = "Size: Waiting..."
        lblCodec.Text = "Codec: Waiting..."
    End Sub


    Private Sub AddToHistory(fileName As String)

        File.AppendAllText(
        "history.txt",
        fileName & Environment.NewLine)

        lstHistory.Items.Add(fileName)

    End Sub

    Private Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click
        Dim url As String = txtUrl.Text.Trim()
        Dim selectedType As String =
             cmbFormat.SelectedItem.ToString()
        Dim selectedQuality As String =
            cmbQuality.SelectedItem.ToString()

        lblCurrentTask.Text = "Task: Downloading"

        lblCurrentQuality.Text =
            "Quality: " & selectedQuality

        lblCurrentFile.Text = "File: Fetching..."


        lblProgressPercent.Text = "Progress: 0%"
        lblSpeed.Text = "Speed: —"
        lblETA.Text = "ETA: —"



        currentQuality = selectedQuality
        currentFileName = "Fetching..."
        ' Validate URL
        If String.IsNullOrWhiteSpace(url) Then
            MessageBox.Show("Please enter a valid video URL.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Check yt-dlp existence
        If Not File.Exists(ytDlpPath) Then
            MessageBox.Show("yt-dlp.exe not found in application folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Build arguments based on selection
        Dim downloadsFolder As String =
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) &
            "\Downloads"

        Dim outputTemplate As String =
            Path.Combine(downloadsFolder, "%(title)s.%(ext)s")

        Dim arguments As String

        Dim maxHeight As String = "720"

        Select Case selectedQuality


            Case "360p"
                maxHeight = "360"

            Case "480p"
                maxHeight = "480"

            Case "720p"
                maxHeight = "720"

            Case "1080p"
                maxHeight = "1080"


        End Select

        If cmbFormat.SelectedItem.ToString() = "Audio (MP3)" Then
            arguments = String.Format(
                "-x --audio-format mp3 -o ""{0}"" ""{1}""",
                outputTemplate,
                url)
        Else
            arguments = String.Format(
                "-f ""bestvideo[height<={0}]+bestaudio/best[height<={0}]"" " &
                "--merge-output-format mp4 " &
                "-o ""{1}"" ""{2}""",
                maxHeight,
                outputTemplate,
                url)
        End If


        ' Run yt-dlp in background
        Dim psi As New ProcessStartInfo()
        psi.FileName = ytDlpPath
        psi.Arguments = arguments
        psi.UseShellExecute = False
        psi.RedirectStandardOutput = True
        psi.RedirectStandardError = True
        psi.CreateNoWindow = True

        Dim proc As New Process()
        proc.StartInfo = psi
        AddHandler proc.OutputDataReceived, AddressOf OutputHandler
        AddHandler proc.ErrorDataReceived, AddressOf OutputHandler
        proc.EnableRaisingEvents = True

        AddHandler proc.Exited,
            Sub()

                Me.Invoke(Sub()

                              If ProgressBar1.Value < 100 Then
                                  ProgressBar1.Value = 100
                              End If

                              lblCurrentTask.Text = "Task: Completed"

                              If currentFileName <> "Fetching..." Then
                                  lblCurrentFile.Text = "File: " & currentFileName
                              End If

                              lblCurrentQuality.Text =
                                "Quality: " & currentQuality

                              lblProgressPercent.Text =
                                "Progress: 100%"
                              lblSpeed.Text = "Speed: Completed"
                              lblETA.Text = "ETA: 00:00"

                              AddToHistory(
                                  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") &
                                  " | " &
                                  currentQuality &
                                  " | " &
                                  currentFileName)

                              MessageBox.Show(
                                  "Download Complete!",
                                  "VBYTDownloader",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information)

                          End Sub)

            End Sub
        downloadCompleted = False
        Try
            proc.Start()
            txtOutput.AppendText("Command: " & arguments & Environment.NewLine)
            proc.BeginOutputReadLine()
            proc.BeginErrorReadLine()
        Catch ex As Exception
            MessageBox.Show("Error starting yt-dlp: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub OutputHandler(sender As Object, e As DataReceivedEventArgs)

        If e.Data Is Nothing Then Return

        Me.Invoke(Sub()

                      ' Debug ETA detection
                      If e.Data.Contains("ETA") Then

                          txtOutput.AppendText(
                          vbCrLf &
                          "FOUND ETA LINE: " &
                          e.Data &
                          vbCrLf)

                      End If

                      ' Raw output
                      txtOutput.AppendText(
                      "[RAW] " &
                      e.Data &
                      Environment.NewLine)

                      txtOutput.SelectionStart =
                      txtOutput.Text.Length

                      txtOutput.ScrollToCaret()

                      ' Detect destination file
                      If e.Data.Contains("Destination:") Then

                          Dim fileName As String =
                          Path.GetFileName(
                              e.Data.Substring(
                                  e.Data.IndexOf("Destination:") + 12).Trim())

                          currentFileName = fileName

                          lblCurrentFile.Text =
                          "File: " & currentFileName

                      End If

                      ' Detect already-downloaded file
                      If e.Data.Contains("has already been downloaded") Then

                          Dim filePath As String =
                          e.Data.Replace("[download]", "") _
                                .Replace("has already been downloaded", "") _
                                .Trim()

                          currentFileName =
                          Path.GetFileName(filePath)

                          lblCurrentFile.Text =
                          "File: " & currentFileName

                      End If

                      ' Progress parsing
                      If e.Data.Contains("%") Then

                          Try

                              Dim match As System.Text.RegularExpressions.Match =
                              System.Text.RegularExpressions.Regex.Match(
                                  e.Data,
                                  "(\d+(\.\d+)?)%"
                              )

                              If match.Success Then

                                  Dim percent As Integer =
                                  CInt(Math.Round(
                                      Double.Parse(
                                          match.Groups(1).Value)))

                                  If percent >= 0 AndAlso percent <= 100 Then

                                      ProgressBar1.Value = percent

                                      lblProgressPercent.Text =
                                      "Progress: " & percent & "%"

                                  End If

                              End If

                          Catch
                          End Try

                      End If

                      ' Speed parsing
                      Try

                          Dim speedMatch =
                          System.Text.RegularExpressions.Regex.Match(
                              e.Data,
                              "at\s+([^\s]+)"
                          )

                          If speedMatch.Success Then

                              lblSpeed.Text =
                              "Speed: " &
                              speedMatch.Groups(1).Value

                          End If

                      Catch
                      End Try

                      ' ETA parsing
                      Try

                          Dim etaMatch =
                          System.Text.RegularExpressions.Regex.Match(
                              e.Data,
                              "ETA\s+([0-9:]+)"
                          )

                          If etaMatch.Success Then

                              lblETA.Text =
                              "ETA: " &
                              etaMatch.Groups(1).Value

                          End If

                      Catch
                      End Try

                  End Sub)

    End Sub



    Private Sub cmbFormat_SelectedIndexChanged(
    sender As Object,
    e As EventArgs) Handles cmbFormat.SelectedIndexChanged

        If cmbFormat.SelectedItem Is Nothing Then Return

        If cmbFormat.SelectedItem.ToString() = "Audio (MP3)" Then

            cmbQuality.Enabled = False

            If Not String.IsNullOrWhiteSpace(lblDuration.Text) Then

                Dim durationText As String =
                lblDuration.Text.Replace(
                    "Duration: ",
                    "")

                Try

                    Dim parts() As String =
                    durationText.Split(":"c)

                    Dim totalSeconds As Double = 0

                    If parts.Length = 2 Then

                        totalSeconds =
                        (CDbl(parts(0)) * 60) +
                        CDbl(parts(1))

                    ElseIf parts.Length = 3 Then

                        totalSeconds =
                        (CDbl(parts(0)) * 3600) +
                        (CDbl(parts(1)) * 60) +
                        CDbl(parts(2))

                    End If

                    lblFileSize.Text =
                    "Estimated Size: " &
                    EstimateMP3Size(totalSeconds)

                Catch
                End Try

            End If

        Else

            cmbQuality.Enabled = True

            RefreshFormatDisplay()

        End If

    End Sub

    Private Sub txtOutput_TextChanged(sender As Object, e As EventArgs) Handles txtOutput.TextChanged

    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        Using fbd As New FolderBrowserDialog()

            If fbd.ShowDialog() = DialogResult.OK Then

                txtFolder.Text = fbd.SelectedPath

            End If

        End Using
    End Sub

    Private Sub btnOpenFolder_Click(sender As Object, e As EventArgs) Handles btnOpenFolder.Click
        If Directory.Exists(txtFolder.Text) Then

            Process.Start("explorer.exe", txtFolder.Text)

        End If
    End Sub

    Private Sub ProgressBar1_Click(sender As Object, e As EventArgs) Handles ProgressBar1.Click

    End Sub

    Private Sub chkDarkMode_CheckedChanged(
    sender As Object,
    e As EventArgs) Handles chkDarkMode.CheckedChanged

        If chkDarkMode.Checked Then

            Me.BackColor = Color.FromArgb(32, 32, 32)

            ApplyTheme(Me, True)

            picThumbnail.BackColor =
            Color.FromArgb(60, 60, 60)

            ProgressBar1.BackColor =
            Color.FromArgb(45, 45, 45)

            txtOutput.BackColor =
            Color.FromArgb(45, 45, 45)

            txtOutput.ForeColor =
            Color.White

        Else

            Me.BackColor = SystemColors.Control

            ApplyTheme(Me, False)

            picThumbnail.BackColor =
            Color.LightGray

            ProgressBar1.BackColor =
            SystemColors.Control

            txtOutput.BackColor =
            Color.White

            txtOutput.ForeColor =
            Color.Black

        End If

    End Sub


    Private Sub txtUrl_TextChanged(sender As Object, e As EventArgs) Handles txtUrl.TextChanged
        If Not String.IsNullOrWhiteSpace(txtUrl.Text) Then

            LoadVideoInfo(txtUrl.Text.Trim())
            LoadFormatSizes(txtUrl.Text.Trim())
        End If

    End Sub

    Private Async Sub LoadVideoInfo(url As String)

        _metaCts?.Cancel()
        _metaCts?.Dispose()

        _metaCts = New CancellationTokenSource()

        Dim ct As CancellationToken =
        _metaCts.Token

        SetMetadataLoading()

        Dim playlistTitle As String = ""
        Dim playlistCount As Integer = 0

        Try

            Dim json As String =
            Await Task.Run(
                Function() FetchYtDlpJson(url),
                ct)
            Try

                If url.Contains("playlist") Then

                    Dim playlistJson As String =
        Await Task.Run(
            Function()
                Return FetchPlaylistInfo(url)
            End Function,
            ct)

                    If Not String.IsNullOrWhiteSpace(
            playlistJson) Then

                        Using playlistDoc As JsonDocument =
            JsonDocument.Parse(playlistJson)

                            Dim playlistRoot As JsonElement =
                playlistDoc.RootElement

                            playlistTitle =
                GetStr(
                    playlistRoot,
                    "title",
                    "Unknown Playlist")

                            Dim entries As JsonElement

                            If playlistRoot.TryGetProperty(
                                "entries",
                                entries) Then

                                playlistCount =
                                    entries.GetArrayLength()

                                Dim firstVideoId As String = ""

                                For Each entry As JsonElement In entries.EnumerateArray()

                                    firstVideoId =
                                        GetStr(entry,
                                            "id",
                                            "")

                                    If Not String.IsNullOrWhiteSpace(
                                        firstVideoId) Then

                                        Exit For

                                    End If

                                Next

                                If Not String.IsNullOrWhiteSpace(
                                    firstVideoId) Then

                                    Dim firstVideoUrl As String =
                                        "https://www.youtube.com/watch?v=" &
                                        firstVideoId

                                    json =
                                        Await Task.Run(
                                            Function()
                                                Return FetchYtDlpJson(
                                                    firstVideoUrl)
                                            End Function,
                                            ct)

                                End If

                            End If

                        End Using

                    End If

                End If

            Catch
            End Try

            If ct.IsCancellationRequested OrElse
           String.IsNullOrWhiteSpace(json) Then Return

            Using doc As JsonDocument =
            JsonDocument.Parse(json)

                Dim root As JsonElement =
                doc.RootElement

                Dim title As String =
                GetStr(root, "title", "Unknown Title")

                Dim uploader As String =
                GetStr(root, "uploader", "Unknown Channel")

                Dim duration As Double =
                GetDbl(root, "duration")

                Dim thumb As String =
                GetStr(root, "thumbnail", "")

                ' Load cache once per URL
                If lastLoadedUrl <> url Then

                    formatSizeCache.Clear()
                    formatCache.Clear()

                    LoadFormatSizes(url)
                    lastLoadedUrl = url

                End If

                lblTitle.Text =
                $"Title: {title}"

                If playlistCount > 0 Then

                    lblPreviewVideo.Text =
                        "Preview Video: " &
                        title

                Else

                    lblPreviewVideo.Text =
                        "Preview Video: —"

                End If

                lblUploader.Text =
                $"Uploader: {uploader}"

                lblDuration.Text =
                $"Duration: {FormatDuration(duration)}"

                If playlistCount > 0 Then

                    lblPlaylistTitle.Text =
                    "Playlist: " &
                    playlistTitle

                    lblPlaylistCount.Text =
                    "Videos: " &
                    playlistCount

                    lblPlaylistSize.Text =
                    "Playlist Size: Calculating..."

                    Dim playlistSize As Double =
                        Await Task.Run(
                        Function()
                            Return CalculatePlaylistSize(url)
                        End Function)

                    If playlistSize > 1024 Then

                        lblPlaylistSize.Text =
                            "Playlist Size: ~" &
                            (playlistSize / 1024.0).ToString("F2") &
                            " GB"

                    Else

                        lblPlaylistSize.Text =
                            "Playlist Size: ~" &
                            playlistSize.ToString("F1") &
                            " MB"

                    End If

                Else

                    lblPlaylistTitle.Text =
                    "Playlist: No"

                    lblPlaylistCount.Text =
                    "Videos: —"

                    lblPlaylistSize.Text =
                    "Playlist Size: —"

                End If

                If cmbFormat.SelectedItem IsNot Nothing AndAlso
               cmbFormat.SelectedItem.ToString() =
               "Audio (MP3)" Then

                    lblFileSize.Text =
                    "Estimated Size: " &
                    EstimateMP3Size(duration)

                    lblFPS.Text =
                    "FPS: N/A"

                    If lblCodec IsNot Nothing Then
                        lblCodec.Text =
                        "Codec: MP3"
                    End If

                Else

                    If cmbQuality.SelectedItem IsNot Nothing Then

                        Dim selectedHeight As String =
    cmbQuality.SelectedItem.ToString()

                        Dim targetHeight As Integer = 0

                        Integer.TryParse(
                            selectedHeight.Replace("p", ""),
                            targetHeight)

                        Dim bestKey As String = Nothing
                        Dim bestHeight As Integer = 0

                        For Each key As String In formatCache.Keys

                            Dim currentHeight As Integer = 0

                            If Integer.TryParse(
                                key.Replace("p", ""),
                                currentHeight) Then

                                If currentHeight <= targetHeight AndAlso
                                   currentHeight > bestHeight Then

                                    bestHeight = currentHeight
                                    bestKey = key

                                End If

                            End If

                        Next

                        If bestKey IsNot Nothing Then

                            Dim info As FormatInfo =
                                formatCache(bestKey)

                            Dim finalSize As Double =
                                info.SizeMB + audioSizeMB

                            lblFileSize.Text =
                                "Estimated Size: " &
                                finalSize.ToString("F1") &
                                " MB"

                            lblFPS.Text =
                                "FPS: " &
                                If(info.FPS > 0,
                                   info.FPS.ToString(),
                                   "Unknown")

                            If lblCodec IsNot Nothing Then

                                lblCodec.Text =
                                    "Codec: " &
                                    If(String.IsNullOrWhiteSpace(
                                        info.VideoCodec),
                                       "Unknown",
                                       info.VideoCodec)

                            End If

                        Else

                            lblFileSize.Text =
                                "Estimated Size: Unknown"

                            lblFPS.Text =
                                "FPS: Unknown"

                            If lblCodec IsNot Nothing Then

                                lblCodec.Text =
                                    "Codec: Unknown"

                            End If

                        End If

                    End If

                End If

                If Not String.IsNullOrEmpty(thumb) AndAlso
               Not ct.IsCancellationRequested Then

                    Await LoadThumbnailAsync(
                    thumb,
                    ct)

                End If

            End Using

        Catch ex As OperationCanceledException

            ' User pasted a new URL

        Catch ex As Exception

            lblTitle.Text = "Title: —"
            lblUploader.Text = $"Uploader: Error — {ex.Message}"
            lblDuration.Text = "Duration: —"
            lblFileSize.Text = "Estimated Size: —"
            lblFPS.Text = "FPS: —"

            If lblCodec IsNot Nothing Then
                lblCodec.Text = "Codec: —"
            End If

        End Try

    End Sub
    Private Sub SetMetadataLoading()

        lblTitle.Text = "Title: Loading..."
        lblUploader.Text = "Uploader: Loading..."
        lblDuration.Text = "Duration: Loading..."
        lblFileSize.Text = "Size: Calculating..."

        If picThumbnail.Image IsNot Nothing Then
            picThumbnail.Image.Dispose()
            picThumbnail.Image = Nothing
        End If

    End Sub

    Private Function FetchYtDlpJson(url As String) As String

        Dim psi As New ProcessStartInfo()

        psi.FileName = ytDlpPath
        psi.Arguments = "--dump-single-json --playlist-items 1 """ & url & """"

        psi.UseShellExecute = False
        psi.RedirectStandardOutput = True
        psi.RedirectStandardError = True
        psi.CreateNoWindow = True

        Using proc As Process = Process.Start(psi)

            Dim output As String =
            proc.StandardOutput.ReadToEnd()

            proc.WaitForExit()

            If proc.ExitCode <> 0 Then
                Return ""
            End If

            Return output

        End Using

    End Function


    Private Function FetchPlaylistInfo(
url As String) As String

        Dim psi As New ProcessStartInfo()

        psi.FileName = ytDlpPath

        psi.Arguments =
    "--flat-playlist --dump-single-json """ &
    url &
    """"

        psi.UseShellExecute = False
        psi.RedirectStandardOutput = True
        psi.RedirectStandardError = True
        psi.CreateNoWindow = True

        Using proc As Process = Process.Start(psi)

            Dim output As String =
        proc.StandardOutput.ReadToEnd()

            proc.WaitForExit()

            Return output

        End Using

    End Function

    Private Async Function LoadThumbnailAsync(
    url As String,
    ct As CancellationToken) As Task

        Try

            Using resp As HttpResponseMessage =
            Await _http.GetAsync(url, ct)

                resp.EnsureSuccessStatusCode()

                Dim bytes() As Byte =
                Await resp.Content.ReadAsByteArrayAsync()

                Using ms As New MemoryStream(bytes)

                    Dim bmp As New Bitmap(ms)

                    If picThumbnail.Image IsNot Nothing Then
                        picThumbnail.Image.Dispose()
                    End If

                    picThumbnail.Image = bmp

                End Using

            End Using

        Catch
        End Try

    End Function

    Private Shared Function GetStr(
    root As JsonElement,
    key As String,
    fallback As String) As String

        Dim prop As JsonElement

        If root.TryGetProperty(key, prop) Then

            If prop.ValueKind <> JsonValueKind.Null Then

                Dim value As String = prop.GetString()

                If Not String.IsNullOrEmpty(value) Then
                    Return value
                End If

            End If

        End If

        Return fallback

    End Function

    Private Shared Function GetDbl(
    root As JsonElement,
    key As String) As Double

        Dim prop As JsonElement

        If root.TryGetProperty(key, prop) Then

            Try
                Return prop.GetDouble()
            Catch
            End Try

        End If

        Return 0

    End Function

    Private Shared Function FormatDuration(
    seconds As Double) As String

        Dim ts As TimeSpan =
        TimeSpan.FromSeconds(seconds)

        If ts.Hours > 0 Then
            Return ts.ToString("hh\:mm\:ss")
        Else
            Return ts.ToString("mm\:ss")
        End If

    End Function

    Private Shared Function EstimateMP3Size(
    seconds As Double) As String

        Dim totalBytes As Long =
        CLng(seconds * 24L * 1024L)

        If totalBytes >= 1024L * 1024L Then

            Return "~" &
               (totalBytes / (1024.0 * 1024.0)).ToString("F1") &
               " MB"

        Else

            Return "~" &
               (totalBytes \ 1024L).ToString() &
               " KB"

        End If

    End Function

    Protected Overrides Sub OnFormClosed(
    e As FormClosedEventArgs)

        _metaCts?.Cancel()
        _metaCts?.Dispose()
        _http.Dispose()

        MyBase.OnFormClosed(e)

    End Sub

    Private Function GetProgramVersion(
    exePath As String,
    arguments As String) As String

        Try

            If Not File.Exists(exePath) Then
                Return "Not Found"
            End If

            Dim psi As New ProcessStartInfo()

            psi.FileName = exePath
            psi.Arguments = arguments
            psi.UseShellExecute = False
            psi.RedirectStandardOutput = True
            psi.CreateNoWindow = True

            Using proc As Process = Process.Start(psi)

                Dim output As String =
                    proc.StandardOutput.ReadLine()

                proc.WaitForExit()

                Return output

            End Using

        Catch ex As Exception

            Return "Error"

        End Try

    End Function

    Private Async Sub btnCheckUpdates_Click(
    sender As Object,
    e As EventArgs) Handles btnCheckUpdates.Click

        RefreshVersions()

        Await CheckForUpdates()

    End Sub
    Private Sub RefreshVersions()

        Dim ytVersion As String =
            GetProgramVersion(
                ytDlpPath,
                "--version")

        lblYtDlpVersion.Text =
            "yt-dlp: " & ytVersion

        Dim ffVersion As String =
        GetProgramVersion(
        ffmpegPath,
        "-version")

        If ffVersion <> "Not Found" AndAlso
        ffVersion <> "Error" Then

            Dim parts() As String =
        ffVersion.Split(" "c)

            If parts.Length >= 3 Then

                Dim versionText As String =
            parts(2)

                If versionText.Length >= 10 Then
                    versionText =
                versionText.Substring(0, 10)
                End If

                lblFfmpegVersion.Text =
            "FFmpeg: " & versionText

            Else

                lblFfmpegVersion.Text =
            "FFmpeg: Unknown"

            End If

        Else

            lblFfmpegVersion.Text =
        "FFmpeg: " & ffVersion

        End If

    End Sub

    Private Async Function CheckForUpdates() As Task

        Try

            Dim latestYtDlp As String =
            Await _http.GetStringAsync(
            "https://api.github.com/repos/yt-dlp/yt-dlp/releases/latest")

            Dim latestFfmpeg As String =
            Await _http.GetStringAsync(
            "https://api.github.com/repos/BtbN/FFmpeg-Builds/releases/latest")

            Dim ytDoc As JsonDocument =
                JsonDocument.Parse(latestYtDlp)

            Dim latestYtVersion As String =
                ytDoc.RootElement.GetProperty(
                    "tag_name").GetString()

            Dim ffDoc As JsonDocument =
                JsonDocument.Parse(latestFfmpeg)

            Dim latestFfVersion As String =
                ffDoc.RootElement.GetProperty(
                    "tag_name").GetString()
            Dim installedYt As String =
                lblYtDlpVersion.Text.Replace(
                    "yt-dlp: ",
                    "")

            Dim installedFf As String =
                lblFfmpegVersion.Text.Replace(
                    "FFmpeg: ",
                    "")
            Dim msg As String = ""

            If installedYt = latestYtVersion Then

                msg &= "✓ yt-dlp is up to date" &
                       Environment.NewLine &
                       Environment.NewLine

            Else

                msg &= "⚠ yt-dlp update available" &
                       Environment.NewLine &
                       "Installed: " &
                       installedYt &
                       Environment.NewLine &
                       "Latest: " &
                       latestYtVersion &
                       Environment.NewLine &
                       Environment.NewLine

            End If

            msg &= "FFmpeg Installed: " &
                   installedFf &
                   Environment.NewLine &
                   "Latest GitHub Release: " &
                   latestFfVersion

            MessageBox.Show(
                msg,
                "Update Check",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

        Catch ex As Exception

            MessageBox.Show(
            "Unable to check online versions." &
            Environment.NewLine &
            ex.Message,
            "Update Check",
            MessageBoxButtons.OK,
            MessageBoxIcon.Warning)

        End Try

    End Function

    Private Sub ApplyTheme(parent As Control, darkMode As Boolean)

        For Each ctrl As Control In parent.Controls

            If darkMode Then

                ctrl.ForeColor = Color.White

                If TypeOf ctrl Is TextBox Then

                    ctrl.BackColor = Color.FromArgb(45, 45, 45)

                ElseIf TypeOf ctrl Is ListBox Then

                    ctrl.BackColor = Color.FromArgb(45, 45, 45)

                ElseIf TypeOf ctrl Is ComboBox Then

                    ctrl.BackColor = Color.FromArgb(45, 45, 45)

                ElseIf TypeOf ctrl Is Button Then

                    ctrl.BackColor = Color.FromArgb(60, 60, 60)

                ElseIf TypeOf ctrl Is GroupBox Then

                    ctrl.BackColor = Color.FromArgb(32, 32, 32)

                End If

            Else

                ctrl.ForeColor = Color.Black

                If TypeOf ctrl Is TextBox Then

                    ctrl.BackColor = Color.White

                ElseIf TypeOf ctrl Is ListBox Then

                    ctrl.BackColor = Color.White

                ElseIf TypeOf ctrl Is ComboBox Then

                    ctrl.BackColor = Color.White

                ElseIf TypeOf ctrl Is Button Then

                    ctrl.BackColor = SystemColors.Control

                ElseIf TypeOf ctrl Is GroupBox Then

                    ctrl.BackColor = SystemColors.Control

                End If

            End If

            If ctrl.HasChildren Then
                ApplyTheme(ctrl, darkMode)
            End If

        Next

    End Sub

    Private Sub cmbQuality_SelectedIndexChanged(
    sender As Object,
    e As EventArgs) Handles cmbQuality.SelectedIndexChanged

        RefreshFormatDisplay()

    End Sub

    Private Sub LoadFormatSizes(url As String)

        formatSizeCache.Clear()
        formatCache.Clear()

        Try

            Dim psi As New ProcessStartInfo()

            psi.FileName = ytDlpPath

            psi.Arguments =
            "--dump-single-json --playlist-items 1 """ &
            url &
            """"

            psi.UseShellExecute = False
            psi.RedirectStandardOutput = True
            psi.CreateNoWindow = True

            Using proc As Process = Process.Start(psi)

                Dim json As String =
                proc.StandardOutput.ReadToEnd()

                proc.WaitForExit()

                If String.IsNullOrWhiteSpace(json) Then Exit Sub

                Using doc As JsonDocument =
                JsonDocument.Parse(json)

                    Dim root As JsonElement =
                    doc.RootElement

                    Dim formats As JsonElement

                    If Not root.TryGetProperty(
                    "formats",
                    formats) Then Exit Sub

                    For Each fmt As JsonElement In formats.EnumerateArray()

                        Try

                            Dim heightProp As JsonElement

                            If Not fmt.TryGetProperty(
                            "height",
                            heightProp) Then Continue For

                            Dim height As Integer =
                            heightProp.GetInt32()

                            If height <= 0 Then Continue For

                            Dim quality As String =
                            height.ToString() & "p"

                            Dim sizeBytes As Long = 0

                            Dim sizeProp As JsonElement

                            If fmt.TryGetProperty(
                            "filesize",
                            sizeProp) Then

                                sizeBytes =
                                sizeProp.GetInt64()

                            ElseIf fmt.TryGetProperty(
                            "filesize_approx",
                            sizeProp) Then

                                sizeBytes =
                                sizeProp.GetInt64()

                            End If

                            If sizeBytes <= 0 Then Continue For

                            Dim sizeMB As Double =
                            sizeBytes / 1024.0 / 1024.0

                            Dim vcodec As String = ""

                            Dim vcodecProp As JsonElement

                            If fmt.TryGetProperty(
                                 "vcodec",
                                 vcodecProp) Then

                                vcodec =
                                    vcodecProp.ToString()

                            End If

                            Dim acodec As String = ""

                            Dim acodecProp As JsonElement

                            If fmt.TryGetProperty(
                                "acodec",
                                acodecProp) Then

                                acodec =
                                 acodecProp.ToString()

                            End If

                            If vcodec = "none" AndAlso
                            acodec <> "none" Then
                                If sizeMB > audioSizeMB Then
                                    audioSizeMB = sizeMB
                                End If
                                Continue For
                            End If
                            ' FPS
                            Dim fps As Integer = 0

                            Dim fpsProp As JsonElement

                            If fmt.TryGetProperty(
                            "fps",
                            fpsProp) Then

                                Integer.TryParse(
                                fpsProp.ToString(),
                                fps)

                            End If

                            ' Codec
                            Dim codec As String = ""

                            Dim codecProp As JsonElement

                            If fmt.TryGetProperty(
                            "vcodec",
                            codecProp) Then

                                codec = codecProp.ToString()

                            End If

                            ' Format ID
                            Dim formatId As String = ""

                            Dim idProp As JsonElement

                            If fmt.TryGetProperty(
                            "format_id",
                            idProp) Then

                                formatId =
                                idProp.ToString()

                            End If

                            If formatCache.ContainsKey(
                            quality) Then

                                If sizeMB >
                               formatCache(quality).SizeMB Then

                                    formatCache(quality) =
                                    New FormatInfo With {
                                        .SizeMB = sizeMB,
                                        .FPS = fps,
                                        .FormatID = formatId,
                                        .VideoCodec = codec
                                    }

                                    formatSizeCache(quality) =
                                    sizeMB.ToString("F1") &
                                    " MB"

                                End If

                            Else

                                formatCache.Add(
                                quality,
                                New FormatInfo With {
                                    .SizeMB = sizeMB,
                                    .FPS = fps,
                                    .FormatID = formatId,
                                    .VideoCodec = codec
                                })

                                formatSizeCache.Add(
                                quality,
                                sizeMB.ToString("F1") &
                                " MB")

                            End If

                        Catch
                        End Try

                    Next

                End Using

            End Using

        Catch ex As Exception

            MessageBox.Show(
            "Format loading failed:" &
            Environment.NewLine &
            ex.Message)

        End Try

#If DEBUG Then

        For Each kvp In formatCache

            Debug.WriteLine(
            kvp.Key &
            " | Size=" &
            kvp.Value.SizeMB.ToString("F1") &
            " MB | FPS=" &
            kvp.Value.FPS &
            " | Codec=" &
            kvp.Value.VideoCodec)

        Next

#End If

    End Sub
    Private Sub RefreshFormatDisplay()

        If cmbFormat.SelectedItem Is Nothing Then Return

        If cmbFormat.SelectedItem.ToString() = "Audio (MP3)" Then

            lblFPS.Text = "FPS: N/A"

            If lblCodec IsNot Nothing Then
                lblCodec.Text = "Codec: MP3"
            End If

            Return

        End If

        If cmbQuality.SelectedItem Is Nothing Then Return

        Dim selectedHeight As String =
            cmbQuality.SelectedItem.ToString()

        Dim targetHeight As Integer = 0

        Integer.TryParse(
            selectedHeight.Replace("p", ""),
            targetHeight)

        Dim bestKey As String = Nothing
        Dim bestHeight As Integer = 0

        For Each key As String In formatCache.Keys

            Dim currentHeight As Integer = 0

            If Integer.TryParse(
                key.Replace("p", ""),
                currentHeight) Then

                If currentHeight <= targetHeight AndAlso
                   currentHeight > bestHeight Then

                    bestHeight = currentHeight
                    bestKey = key

                End If

            End If

        Next

        If bestKey IsNot Nothing Then

            Dim info As FormatInfo =
                formatCache(bestKey)

            Dim finalSize As Double =
                info.SizeMB + audioSizeMB

            lblFileSize.Text =
                "Estimated Size: " &
                finalSize.ToString("F1") &
                " MB"

            lblFPS.Text =
                "FPS: " &
                info.FPS.ToString()

            If lblCodec IsNot Nothing Then

                lblCodec.Text =
                    "Codec: " &
                    info.VideoCodec

            End If

        End If

    End Sub

    Private Function CalculatePlaylistSize(
    playlistUrl As String) As Double

        Dim totalSizeMB As Double = 0

        Try

            Dim playlistJson As String =
                FetchPlaylistInfo(playlistUrl)

            If String.IsNullOrWhiteSpace(
                playlistJson) Then

                Return 0

            End If

            Using doc As JsonDocument =
                JsonDocument.Parse(playlistJson)

                Dim root As JsonElement =
                    doc.RootElement

                Dim entries As JsonElement

                If root.TryGetProperty(
                    "entries",
                    entries) Then

                    For Each entry As JsonElement In entries.EnumerateArray()

                        Try

                            Dim videoId As String =
                                GetStr(
                                    entry,
                                    "id",
                                    "")

                            If String.IsNullOrWhiteSpace(
                                videoId) Then

                                Continue For

                            End If

                            Dim videoUrl As String =
                                "https://www.youtube.com/watch?v=" &
                                videoId

                            Dim videoJson As String =
                                FetchYtDlpJson(
                                    videoUrl)

                            If String.IsNullOrWhiteSpace(
                                videoJson) Then

                                Continue For

                            End If

                            Using videoDoc As JsonDocument =
                                JsonDocument.Parse(
                                    videoJson)

                                Dim videoRoot As JsonElement =
                                    videoDoc.RootElement

                                Dim sizeBytes As Long = 0

                                Dim prop As JsonElement

                                If videoRoot.TryGetProperty(
                                    "filesize_approx",
                                    prop) Then

                                    sizeBytes =
                                        prop.GetInt64()

                                ElseIf videoRoot.TryGetProperty(
                                    "filesize",
                                    prop) Then

                                    sizeBytes =
                                        prop.GetInt64()

                                End If

                                totalSizeMB +=
                                    sizeBytes /
                                    1024.0 /
                                    1024.0

                            End Using

                        Catch
                        End Try

                    Next

                End If

            End Using

        Catch
        End Try

        Return totalSizeMB

    End Function

End Class

