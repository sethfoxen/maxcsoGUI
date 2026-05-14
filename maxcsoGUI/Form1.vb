Imports System.IO
Imports System.Runtime.InteropServices
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

    Friend Enum CompressionAlgo
        Zlib
        Zopfli
        SevenZipDeflate
        Lz4
        Lz4Brute
        Libdeflate
    End Enum

    Private Shared ReadOnly DeflateAlgos As CompressionAlgo() = {
        CompressionAlgo.Zlib, CompressionAlgo.Zopfli, CompressionAlgo.SevenZipDeflate, CompressionAlgo.Libdeflate
    }

    Private Shared ReadOnly Lz4Algos As CompressionAlgo() = {
        CompressionAlgo.Lz4, CompressionAlgo.Lz4Brute
    }

    Private Shared ReadOnly CompressDropExtensions As HashSet(Of String) = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
        ".iso"
    }

    Private Shared ReadOnly DecompressDropExtensions As HashSet(Of String) = New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
        ".cso", ".zso", ".dax"
    }

    Private Class FormatOption
        Private ReadOnly displayName_ As String
        Private ReadOnly argumentValue_ As String
        Private ReadOnly outputExtension_ As String
        Private ReadOnly available_ As HashSet(Of CompressionAlgo)
        Private ReadOnly defaults_ As HashSet(Of CompressionAlgo)
        Private ReadOnly supportsAltBlockSize_ As Boolean
        Private ReadOnly requireLz4_ As Boolean
        Private ReadOnly requireDeflate_ As Boolean

        Public Sub New(displayName As String, argumentValue As String, outputExtension As String,
                       available As HashSet(Of CompressionAlgo), defaults As HashSet(Of CompressionAlgo),
                       supportsAltBlockSize As Boolean, requireLz4 As Boolean, requireDeflate As Boolean)
            displayName_ = displayName
            argumentValue_ = argumentValue
            outputExtension_ = outputExtension
            available_ = available
            defaults_ = defaults
            supportsAltBlockSize_ = supportsAltBlockSize
            requireLz4_ = requireLz4
            requireDeflate_ = requireDeflate
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

        Public ReadOnly Property Available As HashSet(Of CompressionAlgo)
            Get
                Return available_
            End Get
        End Property

        Public ReadOnly Property Defaults As HashSet(Of CompressionAlgo)
            Get
                Return defaults_
            End Get
        End Property

        Public ReadOnly Property SupportsAltBlockSize As Boolean
            Get
                Return supportsAltBlockSize_
            End Get
        End Property

        Public ReadOnly Property RequireLz4 As Boolean
            Get
                Return requireLz4_
            End Get
        End Property

        Public ReadOnly Property RequireDeflate As Boolean
            Get
                Return requireDeflate_
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
        Public Property UseZlib As Boolean
        Public Property UseZopfli As Boolean
        Public Property Use7zDeflate As Boolean
        Public Property UseLz4 As Boolean
        Public Property UseLz4Brute As Boolean
        Public Property UseLibdeflate As Boolean
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

    Private Enum TaskbarProgressState
        NoProgress = 0
        Indeterminate = 1
        Normal = 2
        [Error] = 4
    End Enum

    <ComImport(), Guid("EA1AFB91-9E28-4B86-90E9-9E9F8A5EEFAF"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Private Interface ITaskbarList3
        Sub HrInit()
        Sub AddTab(hwnd As IntPtr)
        Sub DeleteTab(hwnd As IntPtr)
        Sub ActivateTab(hwnd As IntPtr)
        Sub SetActiveAlt(hwnd As IntPtr)
        Sub MarkFullscreenWindow(hwnd As IntPtr, <MarshalAs(UnmanagedType.Bool)> fFullscreen As Boolean)
        Sub SetProgressValue(hwnd As IntPtr, ullCompleted As ULong, ullTotal As ULong)
        Sub SetProgressState(hwnd As IntPtr, tbpFlags As TaskbarProgressState)
    End Interface

    <ComImport(), Guid("56FDF344-FD6D-11D0-958A-006097C9A090")>
    Private Class CTaskbarList
    End Class

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Private Shared Function RegisterWindowMessage(lpString As String) As Integer
    End Function

    Private Shared ReadOnly ProgressPercentPattern As New System.Text.RegularExpressions.Regex("(\d+)%", System.Text.RegularExpressions.RegexOptions.Compiled)
    Private Shared ReadOnly TaskbarButtonCreatedMessage As Integer = RegisterWindowMessage("TaskbarButtonCreated")

    Private _dragFilter As DragCursorMessageFilter
    Private _copyCursor As Cursor = Nothing
    Private _suppressPoolEvents As Boolean = False
    Private _fileListToolTipIndex As Integer = -1
    Private _taskbarList As ITaskbarList3 = Nothing
    Private _isTaskbarButtonCreated As Boolean = False
    Private _pendingTaskbarState As TaskbarProgressState = TaskbarProgressState.NoProgress
    Private _pendingTaskbarCompleted As ULong = 0UL
    Private _pendingTaskbarTotal As ULong = 100UL
    Private _lastTaskbarState As Nullable(Of TaskbarProgressState)
    Private _lastTaskbarCompleted As Nullable(Of ULong)
    Private _lastTaskbarTotal As Nullable(Of ULong)

    Private Function BuildFormatOption(displayName As String, argumentValue As String, outputExtension As String) As FormatOption
        Dim available As New HashSet(Of CompressionAlgo)()
        Dim defaults As New HashSet(Of CompressionAlgo)()
        Dim supportsAltBlock As Boolean = True
        Dim requireLz4 As Boolean = False
        Dim requireDeflate As Boolean = False

        Select Case argumentValue
            Case "cso1"
                available.UnionWith(DeflateAlgos)
                defaults.Add(CompressionAlgo.Zlib)
                defaults.Add(CompressionAlgo.SevenZipDeflate)
                requireDeflate = True
            Case "cso2"
                available.UnionWith(DeflateAlgos)
                available.UnionWith(Lz4Algos)
                defaults.Add(CompressionAlgo.Zlib)
                defaults.Add(CompressionAlgo.SevenZipDeflate)
                defaults.Add(CompressionAlgo.Lz4)
                defaults.Add(CompressionAlgo.Libdeflate)
            Case "zso"
                available.UnionWith(Lz4Algos)
                defaults.Add(CompressionAlgo.Lz4)
                requireLz4 = True
            Case "dax"
                available.UnionWith(DeflateAlgos)
                defaults.Add(CompressionAlgo.Zlib)
                defaults.Add(CompressionAlgo.SevenZipDeflate)
                supportsAltBlock = False
                requireDeflate = True
        End Select

        Return New FormatOption(displayName, argumentValue, outputExtension, available, defaults, supportsAltBlock, requireLz4, requireDeflate)
    End Function

    Private Function GetTrialPoolCheckBox(algo As CompressionAlgo) As CheckBox
        Select Case algo
            Case CompressionAlgo.Zlib : Return UseZlib
            Case CompressionAlgo.Zopfli : Return UseZopfli
            Case CompressionAlgo.SevenZipDeflate : Return Use7zDeflate
            Case CompressionAlgo.Lz4 : Return UseLz4
            Case CompressionAlgo.Lz4Brute : Return UseLz4Brute
            Case CompressionAlgo.Libdeflate : Return UseLibdeflate
        End Select
        Return Nothing
    End Function

    Private Iterator Function AllAlgos() As IEnumerable(Of CompressionAlgo)
        Yield CompressionAlgo.Zlib
        Yield CompressionAlgo.Zopfli
        Yield CompressionAlgo.SevenZipDeflate
        Yield CompressionAlgo.Lz4
        Yield CompressionAlgo.Lz4Brute
        Yield CompressionAlgo.Libdeflate
    End Function

    Private Sub ApplyFormatDefaultsToTrialPool()
        Dim selectedFormat As FormatOption = TryCast(FormatSelection.SelectedItem, FormatOption)
        If selectedFormat Is Nothing Then
            Return
        End If

        _suppressPoolEvents = True
        Try
            For Each algo As CompressionAlgo In AllAlgos()
                GetTrialPoolCheckBox(algo).Checked = selectedFormat.Defaults.Contains(algo)
            Next
        Finally
            _suppressPoolEvents = False
        End Try
    End Sub

    Private Function CountCheckedDeflate() As Integer
        Dim count As Integer = 0
        For Each algo As CompressionAlgo In DeflateAlgos
            If GetTrialPoolCheckBox(algo).Checked Then
                count += 1
            End If
        Next
        Return count
    End Function

    Private Function CountCheckedTotal() As Integer
        Dim count As Integer = 0
        For Each algo As CompressionAlgo In AllAlgos()
            If GetTrialPoolCheckBox(algo).Checked Then
                count += 1
            End If
        Next
        Return count
    End Function

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
        Dim applicationIcon As Icon = AppIconHelper.GetApplicationIcon()
        If applicationIcon IsNot Nothing Then
            Me.Icon = applicationIcon
        End If

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

        FormatSelection.Items.Add(BuildFormatOption("CSO v1 (.cso)", "cso1", ".cso"))
        FormatSelection.Items.Add(BuildFormatOption("CSO v2 (.cso)", "cso2", ".cso"))
        FormatSelection.Items.Add(BuildFormatOption("ZSO (.zso)", "zso", ".zso"))
        FormatSelection.Items.Add(BuildFormatOption("DAX (.dax)", "dax", ".dax"))
        FormatSelection.SelectedIndex = 0

        ApplyFormatDefaultsToTrialPool()
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

        Dim titleWidth As Integer = TextRenderer.MeasureText(GroupBox2.Text, GroupBox2.Font).Width
        PoolHelpLabel.Left = GroupBox2.Left + titleWidth + 4
        PoolHelpLabel.Top = GroupBox2.Top + (GroupBox2.Font.Height - PoolHelpLabel.Height) \ 2
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Application.RemoveMessageFilter(_dragFilter)
        If _taskbarList IsNot Nothing Then
            Marshal.FinalReleaseComObject(_taskbarList)
            _taskbarList = Nothing
        End If
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        _isTaskbarButtonCreated = False
        _lastTaskbarState = Nothing
        _lastTaskbarCompleted = Nothing
        _lastTaskbarTotal = Nothing
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = &H20 AndAlso DragCursorMessageFilter.IsDragActive AndAlso _copyCursor IsNot Nothing Then
            Cursor.Current = _copyCursor
            m.Result = New IntPtr(1)
            Return
        End If
        MyBase.WndProc(m)

        If m.Msg = TaskbarButtonCreatedMessage Then
            _isTaskbarButtonCreated = True
            ApplyPendingTaskbarProgress()
        End If
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
            MessageBox.Show("Please enter a valid Uncompressed Size Tolerance percentage.")
            Return
        End If

        Dim lz4CostValue As Double
        If Lz4Cost.Checked AndAlso Not Double.TryParse(Lz4CostText.Text.Trim(), lz4CostValue) Then
            MessageBox.Show("Please enter a valid LZ4 Size Tolerance percentage.")
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
            .UseZlib = UseZlib.Checked,
            .UseZopfli = UseZopfli.Checked,
            .Use7zDeflate = Use7zDeflate.Checked,
            .UseLz4 = UseLz4.Checked,
            .UseLz4Brute = UseLz4Brute.Checked,
            .UseLibdeflate = UseLibdeflate.Checked,
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
        UpdateTaskbarQueueProgressSafe(0, jobs.Count, 0)

        Dim batchResult As ConversionBatchResult = Nothing

        Try
            batchResult = Await Task.Run(Function() RunConversionBatch(jobs, settings))
        Catch ex As Exception
            SetBusyState(False)
            UpdateProgressSafe(0, "Conversion failed")
            UpdateTaskbarQueueProgressSafe(0, jobs.Count, 0, TaskbarProgressState.Error)
            MessageBox.Show("The conversion stopped unexpectedly." & Environment.NewLine & Environment.NewLine & ex.Message, "Conversion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            UpdateProgressSafe(0, "Ready")
            ClearTaskbarProgressSafe()
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
            UpdateTaskbarQueueProgressSafe(batchResult.CompletedCount, jobs.Count, 0, TaskbarProgressState.Error)
            MessageBox.Show(batchResult.FailureMessage, "Conversion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ClearTaskbarProgressSafe()
            Return
        End If

        UpdateProgressSafe(100, "Conversion completed")
        UpdateTaskbarQueueProgressSafe(jobs.Count, jobs.Count, 0)

        If batchResult.ResultSummary.Count > 0 Then
            MessageBox.Show(String.Join(Environment.NewLine & Environment.NewLine, batchResult.ResultSummary), "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Conversion completed!")
        End If

        UpdateProgressSafe(0, "Ready")
        ClearTaskbarProgressSafe()
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
            AddDroppedPaths(DirectCast(e.Data.GetData(DataFormats.FileDrop), String()))
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
            AddDroppedPaths(DirectCast(e.Data.GetData(DataFormats.FileDrop), String()))
        End If
    End Sub

    Private Sub AddDroppedPaths(paths As IEnumerable(Of String))
        If paths Is Nothing Then
            Return
        End If

        Dim allowedExtensions As HashSet(Of String) = If(IsDecompressModeSelected(), DecompressDropExtensions, CompressDropExtensions)
        Dim existingPaths As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Dim acceptedPaths As New List(Of String)()
        Dim filteredCount As Integer = 0

        For Each existingItem As Object In FileList.Items
            Dim existingPath As String = TryCast(existingItem, String)
            If Not String.IsNullOrWhiteSpace(existingPath) Then
                existingPaths.Add(existingPath)
            End If
        Next

        For Each droppedPath As String In paths
            Dim extension As String = System.IO.Path.GetExtension(droppedPath)
            If allowedExtensions.Contains(extension) AndAlso Not existingPaths.Contains(droppedPath) Then
                acceptedPaths.Add(droppedPath)
                existingPaths.Add(droppedPath)
            Else
                If Not allowedExtensions.Contains(extension) Then
                    filteredCount += 1
                End If
            End If
        Next

        For Each acceptedPath As String In acceptedPaths
            FileList.Items.Add(acceptedPath)
        Next

        If filteredCount > 0 Then
            MessageBox.Show(BuildFilteredDropMessage(filteredCount), "Unsupported Files Filtered", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Function BuildFilteredDropMessage(filteredCount As Integer) As String
        If IsDecompressModeSelected() Then
            Return filteredCount.ToString() & " dropped item" & If(filteredCount = 1, " was", "s were") &
                   " filtered out because Decompress mode only accepts .cso, .zso, and .dax files."
        End If

        Return filteredCount.ToString() & " dropped item" & If(filteredCount = 1, " was", "s were") &
               " filtered out because Compress mode only accepts .iso files."
    End Function

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

    Private Sub FileList_MouseMove(sender As Object, e As MouseEventArgs) Handles FileList.MouseMove
        Dim itemIndex As Integer = FileList.IndexFromPoint(e.Location)
        If itemIndex < 0 Then
            ClearFileListToolTip()
            Return
        End If

        Dim itemBounds As Rectangle = FileList.GetItemRectangle(itemIndex)
        If Not itemBounds.Contains(e.Location) Then
            ClearFileListToolTip()
            Return
        End If

        If _fileListToolTipIndex = itemIndex Then
            Return
        End If

        _fileListToolTipIndex = itemIndex
        ToolTip1.SetToolTip(FileList, FileList.Items(itemIndex).ToString())
    End Sub

    Private Sub FileList_MouseLeave(sender As Object, e As EventArgs) Handles FileList.MouseLeave
        ClearFileListToolTip()
    End Sub

    Private Sub ClearFileListToolTip()
        If _fileListToolTipIndex >= 0 Then
            _fileListToolTipIndex = -1
            ToolTip1.Hide(FileList)
            ToolTip1.SetToolTip(FileList, String.Empty)
        End If
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
        ' When the user turns Fast off while still in compress mode, restore the format's default
        ' trial pool (same effect as re-selecting the current format). Skip if Fast was auto-unchecked
        ' because CRC32 Only or decompress mode took over — those modes preserve pool state.
        If Not Fast.Checked AndAlso Not CrcOnly.Checked AndAlso Not IsDecompressModeSelected() Then
            ApplyFormatDefaultsToTrialPool()
        End If
        UpdateCompressionOptionState()
    End Sub

    Private Sub BlockSize_CheckedChanged(sender As Object, e As EventArgs) Handles BlockSize.CheckedChanged
        UpdateCompressionOptionState()
    End Sub

    Private Sub UseZlib_CheckedChanged(sender As Object, e As EventArgs) Handles UseZlib.CheckedChanged
        HandleAlgorithmToggle(CompressionAlgo.Zlib)
    End Sub

    Private Sub UseZopfli_CheckedChanged(sender As Object, e As EventArgs) Handles UseZopfli.CheckedChanged
        HandleAlgorithmToggle(CompressionAlgo.Zopfli)
    End Sub

    Private Sub Use7zDeflate_CheckedChanged(sender As Object, e As EventArgs) Handles Use7zDeflate.CheckedChanged
        HandleAlgorithmToggle(CompressionAlgo.SevenZipDeflate)
    End Sub

    Private Sub UseLz4_CheckedChanged(sender As Object, e As EventArgs) Handles UseLz4.CheckedChanged
        HandleAlgorithmToggle(CompressionAlgo.Lz4)
    End Sub

    Private Sub UseLz4Brute_CheckedChanged(sender As Object, e As EventArgs) Handles UseLz4Brute.CheckedChanged
        HandleAlgorithmToggle(CompressionAlgo.Lz4Brute)
    End Sub

    Private Sub UseLibdeflate_CheckedChanged(sender As Object, e As EventArgs) Handles UseLibdeflate.CheckedChanged
        HandleAlgorithmToggle(CompressionAlgo.Libdeflate)
    End Sub

    Private Sub HandleAlgorithmToggle(algo As CompressionAlgo)
        If _suppressPoolEvents Then
            Return
        End If

        Dim selectedFormat As FormatOption = TryCast(FormatSelection.SelectedItem, FormatOption)
        Dim cb As CheckBox = GetTrialPoolCheckBox(algo)

        ' Reject illegal unchecks (format mandates).
        If Not cb.Checked Then
            Dim mustReCheck As Boolean = False

            If selectedFormat IsNot Nothing AndAlso selectedFormat.RequireLz4 AndAlso algo = CompressionAlgo.Lz4 Then
                mustReCheck = True
            ElseIf selectedFormat IsNot Nothing AndAlso selectedFormat.RequireDeflate AndAlso DeflateAlgos.Contains(algo) AndAlso CountCheckedDeflate() = 0 Then
                mustReCheck = True
            ElseIf CountCheckedTotal() = 0 Then
                mustReCheck = True
            End If

            If mustReCheck Then
                _suppressPoolEvents = True
                Try
                    cb.Checked = True
                Finally
                    _suppressPoolEvents = False
                End Try
                Return
            End If
        End If

        ' LZ4 Brute requires LZ4 to be checked. If LZ4 was just unchecked, also uncheck LZ4 Brute.
        If algo = CompressionAlgo.Lz4 AndAlso Not cb.Checked AndAlso UseLz4Brute.Checked Then
            _suppressPoolEvents = True
            Try
                UseLz4Brute.Checked = False
            Finally
                _suppressPoolEvents = False
            End Try
        End If

        UpdateCompressionOptionState()
    End Sub

    Private Sub ModeSelection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ModeSelection.SelectedIndexChanged
        If FileList.Items.Count > 0 Then
            FileList.Items.Clear()
        End If

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
        ApplyFormatDefaultsToTrialPool()
        UpdateCompressionOptionState()
    End Sub

    Private Sub UpdateCompressionOptionState()
        Dim selectedFormat As FormatOption = TryCast(FormatSelection.SelectedItem, FormatOption)
        Dim isDecompressMode As Boolean = IsDecompressModeSelected()

        If isDecompressMode Then
            If CrcOnly.Checked Then
                CrcOnly.Checked = False
            End If

            If MeasureOnly.Checked Then
                MeasureOnly.Checked = False
            End If
        End If

        Dim supportsAltBlockSize As Boolean = selectedFormat Is Nothing OrElse selectedFormat.SupportsAltBlockSize
        Dim isReadOnlyMode As Boolean = CrcOnly.Checked OrElse MeasureOnly.Checked
        Dim trialPoolDisabled As Boolean = isDecompressMode OrElse CrcOnly.Checked

        ' Auto-uncheck any algorithm that is no longer compatible with the selected format.
        If selectedFormat IsNot Nothing Then
            _suppressPoolEvents = True
            Try
                For Each algo As CompressionAlgo In AllAlgos()
                    Dim cb As CheckBox = GetTrialPoolCheckBox(algo)
                    If Not selectedFormat.Available.Contains(algo) AndAlso cb.Checked Then
                        cb.Checked = False
                    End If
                Next

                ' lz4brute implies lz4 must be on. If lz4 is off but lz4brute is on, fix it.
                If UseLz4Brute.Checked AndAlso Not UseLz4.Checked Then
                    UseLz4Brute.Checked = False
                End If
            Finally
                _suppressPoolEvents = False
            End Try
        End If

        ' Fast Mode (maxcso --fast) forces off the HC/slow variants but leaves basic zlib + basic LZ4
        ' user-toggleable. Uncheck the slow algos; if that empties the pool, fall back to zlib (or LZ4).
        If Fast.Checked Then
            _suppressPoolEvents = True
            Try
                If UseZopfli.Checked Then UseZopfli.Checked = False
                If Use7zDeflate.Checked Then Use7zDeflate.Checked = False
                If UseLz4Brute.Checked Then UseLz4Brute.Checked = False
                If UseLibdeflate.Checked Then UseLibdeflate.Checked = False
                If CountCheckedTotal() = 0 AndAlso selectedFormat IsNot Nothing Then
                    If selectedFormat.Available.Contains(CompressionAlgo.Zlib) Then
                        UseZlib.Checked = True
                    ElseIf selectedFormat.Available.Contains(CompressionAlgo.Lz4) Then
                        UseLz4.Checked = True
                    End If
                End If
            Finally
                _suppressPoolEvents = False
            End Try
        End If

        If Not supportsAltBlockSize AndAlso BlockSize.Checked Then
            BlockSize.Checked = False
        End If

        If isReadOnlyMode AndAlso DeleteCheck.Checked Then
            DeleteCheck.Checked = False
        End If

        If isReadOnlyMode AndAlso CustDir.Checked Then
            CustDir.Checked = False
        End If

        FormatSelection.Enabled = Not isDecompressMode AndAlso Not CrcOnly.Checked
        Fast.Enabled = Not isDecompressMode AndAlso Not CrcOnly.Checked
        BlockSize.Enabled = Not isDecompressMode AndAlso Not CrcOnly.Checked AndAlso supportsAltBlockSize
        BlockText.Enabled = BlockSize.Enabled AndAlso BlockSize.Checked
        ModeSelection.Enabled = Not isReadOnlyMode
        CrcOnly.Enabled = Not isDecompressMode
        MeasureOnly.Enabled = Not isDecompressMode
        DeleteCheck.Enabled = Not isReadOnlyMode
        CustDir.Enabled = Not isReadOnlyMode
        Browse.Enabled = CustDir.Checked AndAlso CustDir.Enabled
        CustOut.Enabled = CustDir.Checked AndAlso CustDir.Enabled

        ' Enable/disable algorithm checkboxes based on format availability and global mode.
        ' Fast Mode disables the HC/slow variants per algorithm but leaves basic zlib + basic LZ4 toggleable.
        For Each algo As CompressionAlgo In AllAlgos()
            Dim cb As CheckBox = GetTrialPoolCheckBox(algo)
            Dim availableForFormat As Boolean = selectedFormat IsNot Nothing AndAlso selectedFormat.Available.Contains(algo)
            Dim disabledByFast As Boolean = Fast.Checked AndAlso (algo = CompressionAlgo.Zopfli OrElse algo = CompressionAlgo.SevenZipDeflate OrElse algo = CompressionAlgo.Lz4Brute OrElse algo = CompressionAlgo.Libdeflate)
            cb.Enabled = availableForFormat AndAlso Not trialPoolDisabled AndAlso Not disabledByFast
        Next

        ' LZ4 Brute is only meaningful if LZ4 is on.
        If UseLz4Brute.Enabled AndAlso Not UseLz4.Checked Then
            UseLz4Brute.Enabled = False
        End If

        ' If the format mandates LZ4 (ZSO), lock the LZ4 checkbox in the checked state.
        If selectedFormat IsNot Nothing AndAlso selectedFormat.RequireLz4 Then
            UseLz4.Enabled = False
        End If

        Dim anyDeflateChecked As Boolean = (CountCheckedDeflate() > 0)
        Dim anyLz4Checked As Boolean = (UseLz4.Checked OrElse UseLz4Brute.Checked)

        OrigCost.Enabled = Not trialPoolDisabled AndAlso (anyDeflateChecked OrElse anyLz4Checked)
        OrigCostText.Enabled = OrigCost.Enabled AndAlso OrigCost.Checked
        Lz4Cost.Enabled = Not trialPoolDisabled AndAlso anyLz4Checked AndAlso anyDeflateChecked
        Lz4CostText.Enabled = Lz4Cost.Enabled AndAlso Lz4Cost.Checked

        ' Auto-uncheck disabled options so the UI never shows a checked-but-inactive state.
        If Not Fast.Enabled AndAlso Fast.Checked Then Fast.Checked = False
        If Not BlockSize.Enabled AndAlso BlockSize.Checked Then BlockSize.Checked = False
        If Not OrigCost.Enabled AndAlso OrigCost.Checked Then OrigCost.Checked = False
        If Not Lz4Cost.Enabled AndAlso Lz4Cost.Checked Then Lz4Cost.Checked = False
    End Sub

    Private Function RunConversionBatch(jobs As List(Of ConversionJob), settings As ConversionSettings) As ConversionBatchResult
        Dim result As New ConversionBatchResult()
        Dim maxcsoExecutable As String = String.Empty
        Dim checkedForExecutable As Boolean = False

        Try
            For jobIndex As Integer = 0 To jobs.Count - 1
                Dim job As ConversionJob = jobs(jobIndex)
                Dim currentJobIndex As Integer = jobIndex
                Dim totalJobs As Integer = jobs.Count
                Dim fileLabel As String = "[" & (currentJobIndex + 1).ToString() & "/" & totalJobs.ToString() & "] " & Path.GetFileName(job.InputPath)
                Dim failureDetails As String = String.Empty
                Dim successDetails As String = String.Empty

                UpdateProgressSafe(-1, If(ShouldUseIndeterminateStatus(settings), fileLabel & " - In Progress", fileLabel & " - Starting"))
                UpdateTaskbarQueueProgressSafe(currentJobIndex, totalJobs, 0)

                Dim nativeRequest As NativeBridgeRequest = BuildNativeRequest(job.InputPath, job.OutputPath, settings)
                Dim nativeResult As NativeBridgeRunResult = MaxcsoNative.Run(nativeRequest, successDetails, failureDetails,
                    Sub(percent As Integer, message As String)
                        UpdateProgressSafe(percent, fileLabel & " - In Progress", FormatProgressBytes(message))
                        UpdateTaskbarQueueProgressSafe(currentJobIndex, totalJobs, percent)
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
                                UpdateTaskbarQueueProgressSafe(currentJobIndex, totalJobs, percent)
                            End Sub) Then
                            CleanupFailedOutput(job.OutputPath, settings)
                            result.FailureMessage = BuildFailureMessage(job.InputPath, failureDetails)
                            Return result
                        End If
                End Select

                If settings.CrcOnly OrElse settings.MeasureOnly Then
                    If Not String.IsNullOrWhiteSpace(successDetails) Then
                        Dim summaryLine As String = If(settings.MeasureOnly, FormatMeasureResult(successDetails, job.InputPath), successDetails.Trim())
                        result.ResultSummary.Add(summaryLine)
                    End If
                End If

                If settings.DeleteOriginal AndAlso Not settings.CrcOnly AndAlso Not settings.MeasureOnly Then
                    My.Computer.FileSystem.DeleteFile(job.InputPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                End If

                result.CompletedCount += 1
                UpdateProgressSafe(100, fileLabel & " - Completed")
                UpdateTaskbarQueueProgressSafe(result.CompletedCount, totalJobs, 0)
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

    Private Sub UpdateTaskbarQueueProgressSafe(completedJobs As Integer, totalJobs As Integer, currentJobPercent As Integer, Optional state As TaskbarProgressState = TaskbarProgressState.Normal)
        If IsDisposed Then
            Return
        End If

        If InvokeRequired Then
            Try
                BeginInvoke(New Action(Of Integer, Integer, Integer, TaskbarProgressState)(AddressOf UpdateTaskbarQueueProgressSafe), completedJobs, totalJobs, currentJobPercent, state)
            Catch ex As ObjectDisposedException
            End Try
            Return
        End If

        If totalJobs <= 0 Then
            SetTaskbarProgressState(TaskbarProgressState.NoProgress)
            Return
        End If

        Dim normalizedCompletedJobs As Integer = Math.Max(0, Math.Min(totalJobs, completedJobs))
        Dim normalizedCurrentPercent As Integer = Math.Max(0, Math.Min(100, currentJobPercent))
        Dim completedUnits As ULong = CULng((CLng(normalizedCompletedJobs) * 100L) + normalizedCurrentPercent)
        Dim totalUnits As ULong = CULng(CLng(totalJobs) * 100L)

        If completedUnits > totalUnits Then
            completedUnits = totalUnits
        End If

        SetTaskbarProgressValue(state, completedUnits, totalUnits)
    End Sub

    Private Sub ClearTaskbarProgressSafe()
        If IsDisposed Then
            Return
        End If

        If InvokeRequired Then
            Try
                BeginInvoke(New Action(AddressOf ClearTaskbarProgressSafe))
            Catch ex As ObjectDisposedException
            End Try
            Return
        End If

        SetTaskbarProgressState(TaskbarProgressState.NoProgress)
    End Sub

    Private Sub SetTaskbarProgressValue(state As TaskbarProgressState, completed As ULong, total As ULong)
        _pendingTaskbarState = state
        _pendingTaskbarCompleted = Math.Min(completed, If(total = 0UL, 1UL, total))
        _pendingTaskbarTotal = If(total = 0UL, 1UL, total)
        ApplyPendingTaskbarProgress()
    End Sub

    Private Sub SetTaskbarProgressState(state As TaskbarProgressState)
        _pendingTaskbarState = state
        ApplyPendingTaskbarProgress()
    End Sub

    Private Sub ApplyPendingTaskbarProgress()
        If Not _isTaskbarButtonCreated OrElse Not IsHandleCreated Then
            Return
        End If

        If Not EnsureTaskbarListInitialized() Then
            Return
        End If

        Try
            If Not _lastTaskbarState.HasValue OrElse _lastTaskbarState.Value <> _pendingTaskbarState Then
                _taskbarList.SetProgressState(Handle, _pendingTaskbarState)
                _lastTaskbarState = _pendingTaskbarState
            End If

            If _pendingTaskbarState = TaskbarProgressState.Normal OrElse _pendingTaskbarState = TaskbarProgressState.Error Then
                If Not _lastTaskbarCompleted.HasValue OrElse
                    Not _lastTaskbarTotal.HasValue OrElse
                    _lastTaskbarCompleted.Value <> _pendingTaskbarCompleted OrElse
                    _lastTaskbarTotal.Value <> _pendingTaskbarTotal Then
                    _taskbarList.SetProgressValue(Handle, _pendingTaskbarCompleted, _pendingTaskbarTotal)
                    _lastTaskbarCompleted = _pendingTaskbarCompleted
                    _lastTaskbarTotal = _pendingTaskbarTotal
                End If
            Else
                _lastTaskbarCompleted = Nothing
                _lastTaskbarTotal = Nothing
            End If
        Catch ex As COMException
        End Try
    End Sub

    Private Function EnsureTaskbarListInitialized() As Boolean
        If _taskbarList IsNot Nothing Then
            Return True
        End If

        Try
            _taskbarList = CType(New CTaskbarList(), ITaskbarList3)
            _taskbarList.HrInit()
            Return True
        Catch ex As COMException
            _taskbarList = Nothing
            Return False
        End Try
    End Function

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

    Private Function FormatMeasureResult(rawMessage As String, inputPath As String) As String
        Dim trimmed As String = If(rawMessage, String.Empty).Trim()
        Dim m As System.Text.RegularExpressions.Match = System.Text.RegularExpressions.Regex.Match(
            trimmed, "(\d+)\s*->\s*(\d+)\s*bytes\s*\(([\d.]+)%\)")
        If Not m.Success Then
            Return Path.GetFileName(inputPath) & ": " & trimmed
        End If

        Dim original As Long
        Dim compressed As Long
        If Not Long.TryParse(m.Groups(1).Value, original) OrElse Not Long.TryParse(m.Groups(2).Value, compressed) Then
            Return Path.GetFileName(inputPath) & ": " & trimmed
        End If

        Dim ratio As String = m.Groups(3).Value
        Return Path.GetFileName(inputPath) & Environment.NewLine &
               "  Original:   " & FormatBytes(original) & Environment.NewLine &
               "  Compressed: " & FormatBytes(compressed) & " (" & ratio & "% of original)"
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
        request.UseZlib = settings.UseZlib
        request.UseZopfli = settings.UseZopfli
        request.Use7zDeflate = settings.Use7zDeflate
        request.UseLz4 = settings.UseLz4
        request.UseLz4Brute = settings.UseLz4Brute
        request.UseLibdeflate = settings.UseLibdeflate
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

        If settings.BlockSizeEnabled Then
            arguments.Add("--block=" & settings.BlockSize.ToString())
        End If

        ' Emit explicit enable/disable for each of the six trial-pool methods so the CLI run
        ' mirrors the GUI's trial-pool selection regardless of CLI defaults.
        arguments.Add(If(settings.UseZlib, "--use-zlib", "--no-zlib"))
        arguments.Add(If(settings.UseZopfli, "--use-zopfli", "--no-zopfli"))
        arguments.Add(If(settings.Use7zDeflate, "--use-7zdeflate", "--no-7zdeflate"))
        arguments.Add(If(settings.UseLz4, "--use-lz4", "--no-lz4"))
        arguments.Add(If(settings.UseLz4Brute, "--use-lz4brute", "--no-lz4brute"))
        arguments.Add(If(settings.UseLibdeflate, "--use-libdeflate", "--no-libdeflate"))

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
