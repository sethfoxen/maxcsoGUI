Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class maxcsoGUI
    Private Class ThreadOption
        Private ReadOnly count_ As Integer

        Public Sub New(count As Integer)
            count_ = count
        End Sub

        Public ReadOnly Property Count As Integer
            Get
                Return count_
            End Get
        End Property

        Public Overrides Function ToString() As String
            If Count = 1 Then
                Return "1 Thread"
            End If

            Return Count.ToString() & " Threads"
        End Function
    End Class

    Private Class OperationModeOption
        Private ReadOnly displayName_ As String
        Private ReadOnly decompress_ As Boolean

        Public Sub New(displayName As String, decompress As Boolean)
            displayName_ = displayName
            decompress_ = decompress
        End Sub

        Public ReadOnly Property IsDecompress As Boolean
            Get
                Return decompress_
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return displayName_
        End Function
    End Class

    Private Class FormatOption
        Private ReadOnly displayName_ As String
        Private ReadOnly argumentValue_ As String
        Private ReadOnly outputExtension_ As String
        Private ReadOnly supportsZopfli_ As Boolean
        Private ReadOnly supportsAltBlockSize_ As Boolean
        Private ReadOnly supportsLibdeflate_ As Boolean
        Private ReadOnly supportsLz4Brute_ As Boolean
        Private ReadOnly supportsLz4Cost_ As Boolean

        Public Sub New(displayName As String, argumentValue As String, outputExtension As String, supportsZopfli As Boolean, supportsAltBlockSize As Boolean, supportsLibdeflate As Boolean, supportsLz4Brute As Boolean, supportsLz4Cost As Boolean)
            displayName_ = displayName
            argumentValue_ = argumentValue
            outputExtension_ = outputExtension
            supportsZopfli_ = supportsZopfli
            supportsAltBlockSize_ = supportsAltBlockSize
            supportsLibdeflate_ = supportsLibdeflate
            supportsLz4Brute_ = supportsLz4Brute
            supportsLz4Cost_ = supportsLz4Cost
        End Sub

        Public ReadOnly Property DisplayName As String
            Get
                Return displayName_
            End Get
        End Property

        Public ReadOnly Property ArgumentValue As String
            Get
                Return argumentValue_
            End Get
        End Property

        Public ReadOnly Property OutputExtension As String
            Get
                Return outputExtension_
            End Get
        End Property

        Public ReadOnly Property SupportsZopfli As Boolean
            Get
                Return supportsZopfli_
            End Get
        End Property

        Public ReadOnly Property SupportsAltBlockSize As Boolean
            Get
                Return supportsAltBlockSize_
            End Get
        End Property

        Public ReadOnly Property SupportsLibdeflate As Boolean
            Get
                Return supportsLibdeflate_
            End Get
        End Property

        Public ReadOnly Property SupportsLz4Brute As Boolean
            Get
                Return supportsLz4Brute_
            End Get
        End Property

        Public ReadOnly Property SupportsLz4Cost As Boolean
            Get
                Return supportsLz4Cost_
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return DisplayName
        End Function
    End Class

    Private Class ConversionSettings
        Public Property ThreadCount As Integer
        Public Property Format As FormatOption
        Public Property Fast As Boolean
        Public Property UseZopfli As Boolean
        Public Property UseLibdeflate As Boolean
        Public Property UseLz4Brute As Boolean
        Public Property Decompress As Boolean
        Public Property CrcOnly As Boolean
        Public Property MeasureOnly As Boolean
        Public Property BlockSizeEnabled As Boolean
        Public Property BlockSize As UInteger
        Public Property OrigCostEnabled As Boolean
        Public Property OrigCostPercent As Double
        Public Property Lz4CostEnabled As Boolean
        Public Property Lz4CostPercent As Double
        Public Property DeleteOriginal As Boolean
        Public Property UseCustomOutputDir As Boolean
        Public Property CustomOutputDir As String
    End Class

    Private Class ConversionJob
        Public Property InputPath As String
        Public Property OutputPath As String
    End Class

    Private Class ConversionBatchResult
        Public Property Success As Boolean
        Public Property FailureMessage As String
        Public Property CompletedCount As Integer
        Public ReadOnly Property ResultSummary As New List(Of String)
    End Class

    Private Shared ReadOnly ProgressPercentPattern As New System.Text.RegularExpressions.Regex("(\d+)%", System.Text.RegularExpressions.RegexOptions.Compiled)

    Private _dragFilter As DragCursorMessageFilter
    Private _copyCursor As Cursor = Nothing

    Private Function GetSelectedThreadCount() As Integer
        Dim selectedThread As ThreadOption = TryCast(ThreadSelection.SelectedItem, ThreadOption)
        If selectedThread IsNot Nothing Then
            Return selectedThread.Count
        End If

        Return Math.Max(1, Environment.ProcessorCount)
    End Function

    Private Function IsDecompressModeSelected() As Boolean
        Dim selectedMode As OperationModeOption = TryCast(ModeSelection.SelectedItem, OperationModeOption)
        Return selectedMode IsNot Nothing AndAlso selectedMode.IsDecompress
    End Function

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ModeSelection.Items.Add(New OperationModeOption("Compress", False))
        ModeSelection.Items.Add(New OperationModeOption("Decompress", True))
        ModeSelection.SelectedIndex = 0

        Dim cores As Integer = Environment.ProcessorCount
        For count As Integer = 1 To cores
            ThreadSelection.Items.Add(New ThreadOption(count))
        Next

        If ThreadSelection.Items.Count > 0 Then
            ThreadSelection.SelectedIndex = ThreadSelection.Items.Count - 1
        End If

        FormatSelection.Items.Add(New FormatOption("CSO v1 (.cso)", "cso1", ".cso", True, True, True, False, False))
        FormatSelection.Items.Add(New FormatOption("CSO v2 (.cso)", "cso2", ".cso", True, True, True, True, True))
        FormatSelection.Items.Add(New FormatOption("ZSO (.zso)", "zso", ".zso", False, True, False, True, False))
        FormatSelection.Items.Add(New FormatOption("DAX (.dax)", "dax", ".dax", True, False, True, False, False))
        FormatSelection.SelectedIndex = 0

        UpdateCompressionOptionState()

        Dim cursorPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Cursors", "aero_copy.cur")
        If File.Exists(cursorPath) Then
            Try
                Using stream As New FileStream(cursorPath, FileMode.Open, FileAccess.Read, FileShare.Read)
                    _copyCursor = New Cursor(stream)
                End Using
            Catch
            End Try
        End If
        DragDropListBox.DragCopyCursor = _copyCursor

        _dragFilter = New DragCursorMessageFilter()
        Application.AddMessageFilter(_dragFilter)
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Application.RemoveMessageFilter(_dragFilter)
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = &H20 AndAlso DragCursorMessageFilter.IsDragActive AndAlso _copyCursor IsNot Nothing Then
            Cursor.Current = _copyCursor
            m.Result = New IntPtr(1)
            Return
        End If
        MyBase.WndProc(m)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles About.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ThreadSelection.SelectedIndexChanged
    End Sub

    Private Async Sub Button2_Click(sender As Object, e As EventArgs) Handles Convert.Click
        If FileList.Items.Count = 0 Then
            MessageBox.Show("Please drag and drop at least one file to convert.")
            Return
        End If

        Dim selectedFormat As FormatOption = TryCast(FormatSelection.SelectedItem, FormatOption)
        If Not IsDecompressModeSelected() AndAlso selectedFormat Is Nothing Then
            MessageBox.Show("Please select an output format.")
            Return
        End If

        Dim blockSizeValue As Integer
        If BlockSize.Checked AndAlso Not Integer.TryParse(BlockText.Text.Trim(), blockSizeValue) Then
            MessageBox.Show("Please enter a valid numeric block size.")
            Return
        End If

        Dim origCostValue As Double
        If OrigCost.Checked AndAlso Not Double.TryParse(OrigCostText.Text.Trim(), origCostValue) Then
            MessageBox.Show("Please enter a valid Orig Cost percentage.")
            Return
        End If

        Dim lz4CostValue As Double
        If Lz4Cost.Checked AndAlso Not Double.TryParse(Lz4CostText.Text.Trim(), lz4CostValue) Then
            MessageBox.Show("Please enter a valid LZ4 Cost percentage.")
            Return
        End If

        If CustDir.Checked AndAlso Not CrcOnly.Checked AndAlso Not MeasureOnly.Checked Then
            If String.IsNullOrWhiteSpace(CustOut.Text) Then
                MessageBox.Show("Please choose a custom output directory.")
                Return
            End If

            Try
                Directory.CreateDirectory(CustOut.Text)
            Catch ex As Exception
                MessageBox.Show("Couldn't use that output directory." & Environment.NewLine & Environment.NewLine & ex.Message)
                Return
            End Try
        End If

        Dim settings As New ConversionSettings() With {
            .ThreadCount = GetSelectedThreadCount(),
            .Format = selectedFormat,
            .Fast = Fast.Checked,
            .UseZopfli = Zopfli.Checked,
            .UseLibdeflate = UseLibdeflate.Checked,
            .UseLz4Brute = UseLz4Brute.Checked,
            .Decompress = IsDecompressModeSelected(),
            .CrcOnly = CrcOnly.Checked,
            .MeasureOnly = MeasureOnly.Checked,
            .BlockSizeEnabled = BlockSize.Checked,
            .BlockSize = If(BlockSize.Checked, CUInt(blockSizeValue), 0UI),
            .OrigCostEnabled = OrigCost.Checked,
            .OrigCostPercent = If(OrigCost.Checked, origCostValue, 0.0),
            .Lz4CostEnabled = Lz4Cost.Checked,
            .Lz4CostPercent = If(Lz4Cost.Checked, lz4CostValue, 0.0),
            .DeleteOriginal = DeleteCheck.Checked,
            .UseCustomOutputDir = CustDir.Checked,
            .CustomOutputDir = CustOut.Text
        }

        Dim jobs As New List(Of ConversionJob)
        For Each item As String In FileList.Items
            jobs.Add(New ConversionJob() With {
                .InputPath = item,
                .OutputPath = BuildOutputPath(item, settings)
            })
        Next

        SetBusyState(True)
        UpdateProgressSafe(-1, "Preparing conversion...")

        Dim batchResult As ConversionBatchResult = Nothing

        Try
            batchResult = Await Task.Run(Function() RunConversionBatch(jobs, settings))
        Catch ex As Exception
            SetBusyState(False)
            UpdateProgressSafe(0, "Conversion failed")
            MessageBox.Show("The conversion stopped unexpectedly." & Environment.NewLine & Environment.NewLine & ex.Message, "Conversion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            UpdateProgressSafe(0, "Ready")
            Return
        End Try

        For index As Integer = 1 To batchResult.CompletedCount
            If FileList.Items.Count > 0 Then
                FileList.Items.RemoveAt(0)
            End If
        Next

        SetBusyState(False)

        If Not batchResult.Success Then
            UpdateProgressSafe(0, "Conversion failed")
            MessageBox.Show(batchResult.FailureMessage, "Conversion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        UpdateProgressSafe(100, "Conversion completed")

        If batchResult.ResultSummary.Count > 0 Then
            MessageBox.Show(String.Join(Environment.NewLine & Environment.NewLine, batchResult.ResultSummary), "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Conversion completed!")
        End If

        UpdateProgressSafe(0, "Ready")
    End Sub

    Private Sub ToolTip1_Popup(sender As Object, e As PopupEventArgs)
    End Sub

    Private Sub Form1_DragEnter(sender As Object, e As DragEventArgs) Handles MyBase.DragEnter
        DragCursorMessageFilter.IsDragActive = True
        e.Effect = DragDropEffects.Copy
    End Sub

    Private Sub Form1_DragOver(sender As Object, e As DragEventArgs) Handles MyBase.DragOver
        e.Effect = DragDropEffects.Copy
    End Sub

    Private Sub Form1_DragDrop(sender As Object, e As DragEventArgs) Handles MyBase.DragDrop
        DragCursorMessageFilter.IsDragActive = False
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            For Each path As String In DirectCast(e.Data.GetData(DataFormats.FileDrop), String())
                FileList.Items.Add(path)
            Next
        End If
    End Sub

    Private Sub Form1_DragLeave(sender As Object, e As EventArgs) Handles MyBase.DragLeave
        If Not ClientRectangle.Contains(PointToClient(Cursor.Position)) Then
            DragCursorMessageFilter.IsDragActive = False
        End If
    End Sub

    Private Sub Form1_GiveFeedback(sender As Object, e As GiveFeedbackEventArgs) Handles MyBase.GiveFeedback
        e.UseDefaultCursors = False
        Cursor.Current = If(e.Effect = DragDropEffects.Copy AndAlso _copyCursor IsNot Nothing, _copyCursor, Cursors.Default)
    End Sub

    Private Sub ListBox1_DragEnter(sender As Object, e As DragEventArgs) Handles FileList.DragEnter
        DragCursorMessageFilter.IsDragActive = True
        e.Effect = DragDropEffects.Copy
        FileList.BackColor = Color.FromArgb(210, 230, 255)
    End Sub

    Private Sub FileList_DragOver(sender As Object, e As DragEventArgs) Handles FileList.DragOver
        e.Effect = DragDropEffects.Copy
    End Sub

    Private Sub ListBox1_DragDrop(sender As Object, e As DragEventArgs) Handles FileList.DragDrop
        DragCursorMessageFilter.IsDragActive = False
        FileList.BackColor = SystemColors.Window
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            For Each path As String In DirectCast(e.Data.GetData(DataFormats.FileDrop), String())
                FileList.Items.Add(path)
            Next
        End If
    End Sub

    Private Sub FileList_DragLeave(sender As Object, e As EventArgs) Handles FileList.DragLeave
        FileList.BackColor = SystemColors.Window
        If Not ClientRectangle.Contains(PointToClient(Cursor.Position)) Then
            DragCursorMessageFilter.IsDragActive = False
        End If
    End Sub

    Private Sub FileList_GiveFeedback(sender As Object, e As GiveFeedbackEventArgs) Handles FileList.GiveFeedback
        e.UseDefaultCursors = False
        Cursor.Current = If(e.Effect = DragDropEffects.Copy AndAlso _copyCursor IsNot Nothing, _copyCursor, Cursors.Default)
    End Sub

    Private Sub FileList_DoubleClick(sender As Object, e As EventArgs) Handles FileList.DoubleClick
        Dim i As Integer = FileList.SelectedIndex
        If i >= 0 And i < FileList.Items.Count Then
            FileList.Items.RemoveAt(i)
        End If
    End Sub

    Private Sub FinList_SelectedIndexChanged(sender As Object, e As EventArgs)
    End Sub

    Private Sub DropHelp_Click(sender As Object, e As EventArgs) Handles DropHelp.Click
    End Sub

    Private Sub Fast_CheckedChanged(sender As Object, e As EventArgs) Handles Fast.CheckedChanged
        UpdateCompressionOptionState()
    End Sub

    Private Sub BlockSize_CheckedChanged(sender As Object, e As EventArgs) Handles BlockSize.CheckedChanged
        UpdateCompressionOptionState()
    End Sub

    Private Sub Zopfli_CheckedChanged(sender As Object, e As EventArgs) Handles Zopfli.CheckedChanged
        UpdateCompressionOptionState()
    End Sub

    Private Sub ModeSelection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ModeSelection.SelectedIndexChanged
        UpdateCompressionOptionState()
    End Sub

    Private Sub CrcOnly_CheckedChanged(sender As Object, e As EventArgs) Handles CrcOnly.CheckedChanged
        If CrcOnly.Checked Then
            MeasureOnly.Checked = False
        End If

        UpdateCompressionOptionState()
    End Sub

    Private Sub MeasureOnly_CheckedChanged(sender As Object, e As EventArgs) Handles MeasureOnly.CheckedChanged
        If MeasureOnly.Checked Then
            CrcOnly.Checked = False
        End If

        UpdateCompressionOptionState()
    End Sub

    Private Sub OrigCost_CheckedChanged(sender As Object, e As EventArgs) Handles OrigCost.CheckedChanged
        UpdateCompressionOptionState()
    End Sub

    Private Sub Lz4Cost_CheckedChanged(sender As Object, e As EventArgs) Handles Lz4Cost.CheckedChanged
        UpdateCompressionOptionState()
    End Sub

    Private Sub DeleteCheck_CheckedChanged(sender As Object, e As EventArgs) Handles DeleteCheck.CheckedChanged
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter
    End Sub

    Private Sub CustDir_CheckedChanged(sender As Object, e As EventArgs) Handles CustDir.CheckedChanged
        If CustDir.Checked Then
            Browse.Enabled = True
            CustOut.Enabled = True
        Else
            Browse.Enabled = False
            CustOut.Enabled = False
        End If
    End Sub

    Private Sub Browse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Browse.Click
        FolderBrowserDialog1.ShowDialog()
        CustOut.Text = FolderBrowserDialog1.SelectedPath
    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles CustOut.TextChanged
    End Sub

    Private Sub FormatSelection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FormatSelection.SelectedIndexChanged
        UpdateCompressionOptionState()
    End Sub

    Private Sub UpdateCompressionOptionState()
        Dim selectedFormat As FormatOption = TryCast(FormatSelection.SelectedItem, FormatOption)
        Dim isDecompressMode As Boolean = IsDecompressModeSelected()
        Dim supportsZopfli As Boolean = selectedFormat Is Nothing OrElse selectedFormat.SupportsZopfli
        Dim supportsAltBlockSize As Boolean = selectedFormat Is Nothing OrElse selectedFormat.SupportsAltBlockSize
        Dim supportsLibdeflate As Boolean = selectedFormat IsNot Nothing AndAlso selectedFormat.SupportsLibdeflate
        Dim supportsLz4Brute As Boolean = selectedFormat IsNot Nothing AndAlso selectedFormat.SupportsLz4Brute
        Dim supportsLz4Cost As Boolean = selectedFormat IsNot Nothing AndAlso selectedFormat.SupportsLz4Cost
        Dim isReadOnlyMode As Boolean = CrcOnly.Checked OrElse MeasureOnly.Checked

        If Not supportsZopfli AndAlso Zopfli.Checked Then
            Zopfli.Checked = False
        End If

        If Not supportsAltBlockSize AndAlso BlockSize.Checked Then
            BlockSize.Checked = False
        End If

        If Not supportsLibdeflate AndAlso UseLibdeflate.Checked Then
            UseLibdeflate.Checked = False
        End If

        If Not supportsLz4Brute AndAlso UseLz4Brute.Checked Then
            UseLz4Brute.Checked = False
        End If

        If Not supportsLz4Cost AndAlso Lz4Cost.Checked Then
            Lz4Cost.Checked = False
        End If

        If isReadOnlyMode AndAlso DeleteCheck.Checked Then
            DeleteCheck.Checked = False
        End If

        If isReadOnlyMode AndAlso CustDir.Checked Then
            CustDir.Checked = False
        End If

        If isReadOnlyMode AndAlso isDecompressMode AndAlso ModeSelection.Items.Count > 0 Then
            ModeSelection.SelectedIndex = 0
            isDecompressMode = False
        End If

        FormatSelection.Enabled = Not isDecompressMode AndAlso Not CrcOnly.Checked
        Fast.Enabled = Not isDecompressMode AndAlso Not CrcOnly.Checked AndAlso Not Zopfli.Checked
        Zopfli.Enabled = Not isDecompressMode AndAlso Not CrcOnly.Checked AndAlso Not Fast.Checked AndAlso supportsZopfli
        BlockSize.Enabled = Not isDecompressMode AndAlso Not CrcOnly.Checked AndAlso supportsAltBlockSize
        BlockText.Enabled = BlockSize.Enabled AndAlso BlockSize.Checked
        ModeSelection.Enabled = Not isReadOnlyMode
        DeleteCheck.Enabled = Not isReadOnlyMode
        CustDir.Enabled = Not isReadOnlyMode
        Browse.Enabled = CustDir.Checked AndAlso CustDir.Enabled
        CustOut.Enabled = CustDir.Checked AndAlso CustDir.Enabled

        UseLibdeflate.Enabled = Not isDecompressMode AndAlso Not CrcOnly.Checked AndAlso supportsLibdeflate
        UseLz4Brute.Enabled = Not isDecompressMode AndAlso Not CrcOnly.Checked AndAlso supportsLz4Brute
        OrigCost.Enabled = Not isDecompressMode AndAlso Not CrcOnly.Checked
        OrigCostText.Enabled = OrigCost.Enabled AndAlso OrigCost.Checked
        Lz4Cost.Enabled = Not isDecompressMode AndAlso Not CrcOnly.Checked AndAlso supportsLz4Cost
        Lz4CostText.Enabled = Lz4Cost.Enabled AndAlso Lz4Cost.Checked
    End Sub

    Private Function RunConversionBatch(jobs As List(Of ConversionJob), settings As ConversionSettings) As ConversionBatchResult
        Dim result As New ConversionBatchResult()
        Dim maxcsoExecutable As String = String.Empty
        Dim checkedForExecutable As Boolean = False

        Try
            For jobIndex As Integer = 0 To jobs.Count - 1
                Dim job As ConversionJob = jobs(jobIndex)
                Dim fileLabel As String = "[" & (jobIndex + 1).ToString() & "/" & jobs.Count.ToString() & "] " & Path.GetFileName(job.InputPath)
                Dim failureDetails As String = String.Empty
                Dim successDetails As String = String.Empty

                UpdateProgressSafe(-1, If(ShouldUseIndeterminateStatus(settings), fileLabel & " - In Progress", fileLabel & " - Starting"))

                Dim nativeRequest As NativeBridgeRequest = BuildNativeRequest(job.InputPath, job.OutputPath, settings)
                Dim nativeResult As NativeBridgeRunResult = MaxcsoNative.Run(nativeRequest, successDetails, failureDetails,
                    Sub(percent As Integer, message As String)
                        UpdateProgressSafe(percent, fileLabel & " - In Progress", FormatProgressBytes(message))
                    End Sub)

                Select Case nativeResult
                    Case NativeBridgeRunResult.Success
                        ' Native bridge succeeded.
                    Case NativeBridgeRunResult.Failed
                        CleanupFailedOutput(job.OutputPath, settings)
                        result.FailureMessage = BuildFailureMessage(job.InputPath, failureDetails)
                        Return result
                    Case NativeBridgeRunResult.Unavailable
                        If Not checkedForExecutable Then
                            maxcsoExecutable = ResolveMaxcsoExecutable()
                            checkedForExecutable = True
                        End If

                        If String.IsNullOrWhiteSpace(maxcsoExecutable) Then
                            result.FailureMessage = "I couldn't load the integrated maxcso bridge."
                            If Not String.IsNullOrWhiteSpace(failureDetails) Then
                                result.FailureMessage &= Environment.NewLine & Environment.NewLine & failureDetails.Trim()
                            End If
                            CleanupFailedOutput(job.OutputPath, settings)
                            Return result
                        End If

                        UpdateProgressSafe(-1, fileLabel & " - Running fallback converter")

                        Dim processInfo As ProcessStartInfo = BuildProcessStartInfo(maxcsoExecutable, job.InputPath, job.OutputPath, settings)
                        If Not RunConversion(processInfo, successDetails, failureDetails,
                            Sub(percent As Integer, message As String)
                                Dim progressText As String = fileLabel & " - " & If(String.IsNullOrWhiteSpace(message), "In Progress", message)
                                UpdateProgressSafe(percent, progressText)
                            End Sub) Then
                            CleanupFailedOutput(job.OutputPath, settings)
                            result.FailureMessage = BuildFailureMessage(job.InputPath, failureDetails)
                            Return result
                        End If
                End Select

                If settings.CrcOnly OrElse settings.MeasureOnly Then
                    If Not String.IsNullOrWhiteSpace(successDetails) Then
                        result.ResultSummary.Add(successDetails.Trim())
                    End If
                End If

                If settings.DeleteOriginal AndAlso Not settings.CrcOnly AndAlso Not settings.MeasureOnly Then
                    My.Computer.FileSystem.DeleteFile(job.InputPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                End If

                result.CompletedCount += 1
                UpdateProgressSafe(100, fileLabel & " - Completed")
            Next
        Catch ex As Exception
            result.FailureMessage = "The conversion stopped unexpectedly." & Environment.NewLine & Environment.NewLine & ex.Message
            Return result
        End Try

        result.Success = True
        Return result
    End Function

    Private Function ShouldUseIndeterminateStatus(settings As ConversionSettings) As Boolean
        Return Not settings.Decompress AndAlso
            Not settings.CrcOnly AndAlso
            Not settings.MeasureOnly AndAlso
            settings.Format IsNot Nothing AndAlso
            String.Equals(settings.Format.ArgumentValue, "dax", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Function BuildFailureMessage(inputPath As String, failureDetails As String) As String
        Dim failureMessage As String = "maxcso failed while processing " & Path.GetFileName(inputPath) & "."
        If Not String.IsNullOrWhiteSpace(failureDetails) Then
            failureMessage &= Environment.NewLine & Environment.NewLine & failureDetails.Trim()
        End If

        Return failureMessage
    End Function

    Private Sub CleanupFailedOutput(outputPath As String, settings As ConversionSettings)
        If settings.CrcOnly OrElse settings.MeasureOnly OrElse String.IsNullOrWhiteSpace(outputPath) Then
            Return
        End If

        Try
            If File.Exists(outputPath) Then
                Dim outputInfo As New FileInfo(outputPath)
                If outputInfo.Length = 0 Then
                    outputInfo.Delete()
                End If
            End If
        Catch
        End Try
    End Sub

    Private Sub SetBusyState(isBusy As Boolean)
        If Me.InvokeRequired Then
            Me.BeginInvoke(New Action(Of Boolean)(AddressOf SetBusyState), isBusy)
            Return
        End If

        GroupBox1.Enabled = Not isBusy
        GroupBox2.Enabled = Not isBusy
        FileList.Enabled = Not isBusy
        About.Enabled = Not isBusy
        Convert.Enabled = Not isBusy
        Convert.Text = If(isBusy, "Working...", "Convert")
        UseWaitCursor = isBusy
    End Sub

    Private Sub UpdateProgressSafe(percent As Integer, statusText As String, Optional bytesText As String = "")
        If IsDisposed Then
            Return
        End If

        If InvokeRequired Then
            Try
                BeginInvoke(New Action(Of Integer, String, String)(AddressOf UpdateProgressSafe), percent, statusText, bytesText)
            Catch ex As ObjectDisposedException
            End Try
            Return
        End If

        ProgressText.Text = statusText

        If percent < 0 Then
            ProgressPercent.Text = String.Empty
            ProgressBytes.Text = String.Empty
            If ConversionProgress.Style <> ProgressBarStyle.Marquee Then
                ConversionProgress.Style = ProgressBarStyle.Marquee
                ConversionProgress.MarqueeAnimationSpeed = 30
            End If
            Return
        End If

        If ConversionProgress.Style <> ProgressBarStyle.Continuous Then
            ConversionProgress.Style = ProgressBarStyle.Continuous
            ConversionProgress.MarqueeAnimationSpeed = 0
        End If

        Dim normalizedPercent As Integer = Math.Max(0, Math.Min(100, percent))
        ConversionProgress.Value = normalizedPercent
        ProgressPercent.Text = normalizedPercent.ToString() & "%"
        ProgressBytes.Text = bytesText
    End Sub

    Private Function FormatProgressBytes(message As String) As String
        If String.IsNullOrWhiteSpace(message) Then
            Return String.Empty
        End If

        Dim m As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(
            message, "(\d+)\s*/\s*(\d+)\s*bytes")
        If Not m.Success Then
            Return String.Empty
        End If

        Dim current As Long
        Dim total As Long
        If Not Long.TryParse(m.Groups(1).Value, current) OrElse Not Long.TryParse(m.Groups(2).Value, total) Then
            Return String.Empty
        End If

        Return FormatBytes(current) & " / " & FormatBytes(total)
    End Function

    Private Function FormatBytes(value As Long) As String
        If value >= 1073741824L Then
            Return (value / 1073741824.0).ToString("0.0") & " GB"
        ElseIf value >= 1048576L Then
            Return (value / 1048576.0).ToString("0.0") & " MB"
        ElseIf value >= 1024L Then
            Return (value / 1024.0).ToString("0.0") & " KB"
        End If
        Return value.ToString() & " B"
    End Function

    Private Function ResolveMaxcsoExecutable() As String
        Dim candidates As New List(Of String) From {
            Path.Combine(Application.StartupPath, "maxcso.exe"),
            Path.Combine(Application.StartupPath, "maxcso32.exe")
        }

        Dim currentDirectory As DirectoryInfo = New DirectoryInfo(Application.StartupPath)
        Do While currentDirectory IsNot Nothing
            candidates.Add(Path.Combine(currentDirectory.FullName, "maxcso", "maxcso.exe"))
            candidates.Add(Path.Combine(currentDirectory.FullName, "maxcso", "maxcso32.exe"))
            currentDirectory = currentDirectory.Parent
        Loop

        For Each candidate In candidates.Distinct(StringComparer.OrdinalIgnoreCase)
            If File.Exists(candidate) Then
                Return candidate
            End If
        Next

        Return String.Empty
    End Function

    Private Function BuildOutputPath(inputPath As String, settings As ConversionSettings) As String
        If settings.CrcOnly OrElse settings.MeasureOnly Then
            Return String.Empty
        End If

        Dim outputExtension As String = If(settings.Decompress, ".iso", settings.Format.OutputExtension)
        Dim outputDirectory As String = If(settings.UseCustomOutputDir, settings.CustomOutputDir, Path.GetDirectoryName(inputPath))

        If String.IsNullOrWhiteSpace(outputDirectory) Then
            outputDirectory = Application.StartupPath
        End If

        Return Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(inputPath) & outputExtension)
    End Function

    Private Function BuildNativeRequest(inputPath As String, outputPath As String, settings As ConversionSettings) As NativeBridgeRequest
        Dim request As New NativeBridgeRequest()
        request.InputPath = inputPath
        request.OutputPath = If(String.IsNullOrWhiteSpace(outputPath), Nothing, outputPath)
        request.Threads = settings.ThreadCount
        request.Format = GetNativeFormat(settings.Format)
        request.Fast = settings.Fast
        request.UseZopfli = settings.UseZopfli
        request.UseLibdeflate = settings.UseLibdeflate
        request.UseLz4Brute = settings.UseLz4Brute
        request.Decompress = settings.Decompress
        request.CrcOnly = settings.CrcOnly
        request.MeasureOnly = settings.MeasureOnly
        request.BlockSizeEnabled = settings.BlockSizeEnabled
        request.BlockSize = If(settings.BlockSizeEnabled, settings.BlockSize, 0UI)
        request.OrigCostEnabled = settings.OrigCostEnabled
        request.OrigCostPercent = If(settings.OrigCostEnabled, settings.OrigCostPercent, 0.0)
        request.Lz4CostEnabled = settings.Lz4CostEnabled
        request.Lz4CostPercent = If(settings.Lz4CostEnabled, settings.Lz4CostPercent, 0.0)
        Return request
    End Function

    Private Function GetNativeFormat(selectedFormat As FormatOption) As NativeFormat
        If selectedFormat Is Nothing Then
            Return NativeFormat.Cso1
        End If

        Select Case selectedFormat.ArgumentValue
            Case "cso2"
                Return NativeFormat.Cso2
            Case "zso"
                Return NativeFormat.Zso
            Case "dax"
                Return NativeFormat.Dax
            Case Else
                Return NativeFormat.Cso1
        End Select
    End Function

    Private Function BuildProcessStartInfo(maxcsoExecutable As String, inputPath As String, outputPath As String, settings As ConversionSettings) As ProcessStartInfo
        Dim arguments As New List(Of String) From {
            "--threads=" & settings.ThreadCount.ToString()
        }

        If settings.Decompress Then
            arguments.Add("--decompress")
        Else
            arguments.Add("--format=" & settings.Format.ArgumentValue)
        End If

        If settings.CrcOnly Then
            arguments.Add("--crc")
        End If

        If settings.MeasureOnly Then
            arguments.Add("--measure")
        End If

        If settings.Fast Then
            arguments.Add("--fast")
        End If

        If settings.UseZopfli Then
            arguments.Add("--use-zopfli")
        End If

        If settings.BlockSizeEnabled Then
            arguments.Add("--block=" & settings.BlockSize.ToString())
        End If

        If settings.UseLibdeflate Then
            arguments.Add("--use-libdeflate")
        End If

        If settings.UseLz4Brute Then
            arguments.Add("--use-lz4brute")
        End If

        If settings.OrigCostEnabled Then
            arguments.Add("--orig-cost=" & settings.OrigCostPercent.ToString(Globalization.CultureInfo.InvariantCulture))
        End If

        If settings.Lz4CostEnabled Then
            arguments.Add("--lz4-cost=" & settings.Lz4CostPercent.ToString(Globalization.CultureInfo.InvariantCulture))
        End If

        arguments.Add(QuoteArgument(inputPath))

        If Not settings.CrcOnly AndAlso Not settings.MeasureOnly Then
            arguments.Add("-o")
            arguments.Add(QuoteArgument(outputPath))
        End If

        Return New ProcessStartInfo(maxcsoExecutable) With {
            .Arguments = String.Join(" ", arguments),
            .CreateNoWindow = True,
            .RedirectStandardError = True,
            .RedirectStandardOutput = True,
            .UseShellExecute = False
        }
    End Function

    Private Sub ParseAndReportProgress(line As String, progressCallback As Action(Of Integer, String))
        Dim m As System.Text.RegularExpressions.Match = ProgressPercentPattern.Match(line)
        If m.Success Then
            Dim percent As Integer
            If Integer.TryParse(m.Groups(1).Value, percent) Then
                progressCallback(Math.Max(0, Math.Min(100, percent)), line)
            End If
        End If
    End Sub

    Private Sub ReadStreamWithProgress(reader As IO.StreamReader, output As System.Text.StringBuilder, progressCallback As Action(Of Integer, String))
        Dim lineBuffer As New System.Text.StringBuilder()
        Dim charBuffer(255) As Char

        Do
            Dim charsRead As Integer = reader.Read(charBuffer, 0, charBuffer.Length)
            If charsRead = 0 Then Exit Do

            Dim text As New String(charBuffer, 0, charsRead)
            output.Append(text)

            If progressCallback IsNot Nothing Then
                For Each ch As Char In text
                    If ch = Chr(13) OrElse ch = Chr(10) Then
                        Dim line As String = lineBuffer.ToString().Trim()
                        lineBuffer.Clear()
                        If line.Length > 0 Then
                            ParseAndReportProgress(line, progressCallback)
                        End If
                    Else
                        lineBuffer.Append(ch)
                    End If
                Next
            End If
        Loop

        If progressCallback IsNot Nothing AndAlso lineBuffer.Length > 0 Then
            ParseAndReportProgress(lineBuffer.ToString().Trim(), progressCallback)
        End If
    End Sub

    Private Function RunConversion(processInfo As ProcessStartInfo, ByRef successDetails As String, ByRef failureDetails As String, Optional progressCallback As Action(Of Integer, String) = Nothing) As Boolean
        Using conversionProcess As Process = Process.Start(processInfo)
            If conversionProcess Is Nothing Then
                failureDetails = "The maxcso process could not be started."
                Return False
            End If

            Dim outputBuilder As New System.Text.StringBuilder()
            Dim errorBuilder As New System.Text.StringBuilder()

            Dim stdoutTask As System.Threading.Tasks.Task = System.Threading.Tasks.Task.Run(Sub() ReadStreamWithProgress(conversionProcess.StandardOutput, outputBuilder, progressCallback))
            Dim stderrTask As System.Threading.Tasks.Task = System.Threading.Tasks.Task.Run(Sub() ReadStreamWithProgress(conversionProcess.StandardError, errorBuilder, progressCallback))
            System.Threading.Tasks.Task.WaitAll(stdoutTask, stderrTask)
            conversionProcess.WaitForExit()

            successDetails = (outputBuilder.ToString() & Environment.NewLine & errorBuilder.ToString()).Trim()
            failureDetails = If(String.IsNullOrWhiteSpace(errorBuilder.ToString().Trim()), outputBuilder.ToString(), errorBuilder.ToString())

            Return conversionProcess.ExitCode = 0
        End Using
    End Function

    Private Function QuoteArgument(value As String) As String
        Return """" & value & """"
    End Function
End Class

Friend Class DragDropListBox
    Inherits System.Windows.Forms.ListBox

    Private Const WM_SETCURSOR As Integer = &H20
    Friend Shared DragCopyCursor As Cursor = Nothing

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = WM_SETCURSOR AndAlso DragCursorMessageFilter.IsDragActive Then
            If DragCopyCursor IsNot Nothing Then
                Cursor.Current = DragCopyCursor
            End If
            m.Result = New IntPtr(1)
            Return
        End If
        MyBase.WndProc(m)
    End Sub
End Class

Friend Class DragCursorMessageFilter
    Implements IMessageFilter

    Private Const WM_SETCURSOR As Integer = &H20
    Friend Shared IsDragActive As Boolean = False

    Public Function PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage
        If m.Msg = WM_SETCURSOR AndAlso IsDragActive Then
            Return True
        End If
        Return False
    End Function
End Class
