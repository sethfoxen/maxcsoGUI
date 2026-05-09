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

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim cores As Integer = Environment.ProcessorCount
        For count As Integer = 1 To cores
            ThreadSelection.Items.Add(New ThreadOption(count))
        Next

        FormatSelection.Items.Add(New FormatOption("CSO v1 (.cso)", "cso1", ".cso", True, True, True, False, False))
        FormatSelection.Items.Add(New FormatOption("CSO v2 (.cso)", "cso2", ".cso", True, True, True, True, True))
        FormatSelection.Items.Add(New FormatOption("ZSO (.zso)", "zso", ".zso", False, True, False, True, False))
        FormatSelection.Items.Add(New FormatOption("DAX (.dax)", "dax", ".dax", True, False, True, False, False))
        FormatSelection.SelectedIndex = 0

        UpdateCompressionOptionState()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles About.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ThreadSelection.SelectedIndexChanged
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Convert.Click
        Dim maxcsoExecutable As String = ResolveMaxcsoExecutable()
        If String.IsNullOrWhiteSpace(maxcsoExecutable) Then
            MessageBox.Show("I can't find maxcso.exe or maxcso32.exe. I looked next to this GUI and in any sibling maxcso repo folders above it.")
            Return
        End If

        Dim selectedThread As ThreadOption = TryCast(ThreadSelection.SelectedItem, ThreadOption)
        If selectedThread Is Nothing Then
            MessageBox.Show("Please select how many threads to run.")
            Return
        End If

        If FileList.Items.Count = 0 Then
            MessageBox.Show("Please drag and drop at least one file to convert.")
            Return
        End If

        Dim selectedFormat As FormatOption = TryCast(FormatSelection.SelectedItem, FormatOption)
        If Not Decompress.Checked AndAlso selectedFormat Is Nothing Then
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

        Dim resultSummary As New List(Of String)
        Do Until FileList.Items.Count = 0
            Dim inputPath As String = FileList.Items(0).ToString()
            Dim outputPath As String = BuildOutputPath(inputPath, selectedFormat)
            Dim processInfo As ProcessStartInfo = BuildProcessStartInfo(maxcsoExecutable, inputPath, outputPath, selectedThread, selectedFormat)
            Dim failureDetails As String = String.Empty
            Dim successDetails As String = String.Empty

            If Not RunConversion(processInfo, successDetails, failureDetails) Then
                Dim failureMessage As String = "maxcso failed while processing " & Path.GetFileName(inputPath) & "."
                If Not String.IsNullOrWhiteSpace(failureDetails) Then
                    failureMessage &= Environment.NewLine & Environment.NewLine & failureDetails.Trim()
                End If

                MessageBox.Show(failureMessage, "Conversion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            If CrcOnly.Checked OrElse MeasureOnly.Checked Then
                If Not String.IsNullOrWhiteSpace(successDetails) Then
                    resultSummary.Add(successDetails.Trim())
                End If
            End If

            If DeleteCheck.Checked AndAlso Not CrcOnly.Checked AndAlso Not MeasureOnly.Checked Then
                My.Computer.FileSystem.DeleteFile(inputPath, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
            End If

            FileList.Items.RemoveAt(0)
        Loop

        If resultSummary.Count > 0 Then
            MessageBox.Show(String.Join(Environment.NewLine & Environment.NewLine, resultSummary), "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            MessageBox.Show("Conversion completed!")
        End If
    End Sub

    Private Sub ToolTip1_Popup(sender As Object, e As PopupEventArgs)
    End Sub

    Private Sub ListBox1_DragEnter(sender As Object, e As DragEventArgs) Handles FileList.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        End If
    End Sub

    Private Sub ListBox1_DragDrop(sender As Object, e As DragEventArgs) Handles FileList.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
        For Each path In files
            FileList.Items.Add(path)
        Next
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

    Private Sub Decompress_CheckedChanged(sender As Object, e As EventArgs) Handles Decompress.CheckedChanged
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

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles Decompress.CheckedChanged
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

        If isReadOnlyMode AndAlso Decompress.Checked Then
            Decompress.Checked = False
        End If

        FormatSelection.Enabled = Not Decompress.Checked AndAlso Not CrcOnly.Checked
        Fast.Enabled = Not Decompress.Checked AndAlso Not CrcOnly.Checked AndAlso Not Zopfli.Checked
        Zopfli.Enabled = Not Decompress.Checked AndAlso Not CrcOnly.Checked AndAlso Not Fast.Checked AndAlso supportsZopfli
        BlockSize.Enabled = Not Decompress.Checked AndAlso Not CrcOnly.Checked AndAlso supportsAltBlockSize
        BlockText.Enabled = BlockSize.Enabled AndAlso BlockSize.Checked
        Decompress.Enabled = Not isReadOnlyMode
        DeleteCheck.Enabled = Not isReadOnlyMode
        CustDir.Enabled = Not isReadOnlyMode
        Browse.Enabled = CustDir.Checked AndAlso CustDir.Enabled
        CustOut.Enabled = CustDir.Checked AndAlso CustDir.Enabled

        UseLibdeflate.Enabled = Not Decompress.Checked AndAlso Not CrcOnly.Checked AndAlso supportsLibdeflate
        UseLz4Brute.Enabled = Not Decompress.Checked AndAlso Not CrcOnly.Checked AndAlso supportsLz4Brute
        OrigCost.Enabled = Not Decompress.Checked AndAlso Not CrcOnly.Checked
        OrigCostText.Enabled = OrigCost.Enabled AndAlso OrigCost.Checked
        Lz4Cost.Enabled = Not Decompress.Checked AndAlso Not CrcOnly.Checked AndAlso supportsLz4Cost
        Lz4CostText.Enabled = Lz4Cost.Enabled AndAlso Lz4Cost.Checked
    End Sub

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

    Private Function BuildOutputPath(inputPath As String, selectedFormat As FormatOption) As String
        If CrcOnly.Checked OrElse MeasureOnly.Checked Then
            Return String.Empty
        End If

        Dim outputExtension As String = If(Decompress.Checked, ".iso", selectedFormat.OutputExtension)
        Dim outputDirectory As String = If(CustDir.Checked, CustOut.Text, Path.GetDirectoryName(inputPath))

        If String.IsNullOrWhiteSpace(outputDirectory) Then
            outputDirectory = Application.StartupPath
        End If

        Return Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(inputPath) & outputExtension)
    End Function

    Private Function BuildProcessStartInfo(maxcsoExecutable As String, inputPath As String, outputPath As String, selectedThread As ThreadOption, selectedFormat As FormatOption) As ProcessStartInfo
        Dim arguments As New List(Of String) From {
            "--threads=" & selectedThread.Count.ToString()
        }

        If Decompress.Checked Then
            arguments.Add("--decompress")
        Else
            arguments.Add("--format=" & selectedFormat.ArgumentValue)
        End If

        If CrcOnly.Checked Then
            arguments.Add("--crc")
        End If

        If MeasureOnly.Checked Then
            arguments.Add("--measure")
        End If

        If Fast.Checked Then
            arguments.Add("--fast")
        End If

        If Zopfli.Checked Then
            arguments.Add("--use-zopfli")
        End If

        If BlockSize.Checked Then
            arguments.Add("--block=" & BlockText.Text.Trim())
        End If

        If UseLibdeflate.Checked Then
            arguments.Add("--use-libdeflate")
        End If

        If UseLz4Brute.Checked Then
            arguments.Add("--use-lz4brute")
        End If

        If OrigCost.Checked Then
            arguments.Add("--orig-cost=" & OrigCostText.Text.Trim())
        End If

        If Lz4Cost.Checked Then
            arguments.Add("--lz4-cost=" & Lz4CostText.Text.Trim())
        End If

        arguments.Add(QuoteArgument(inputPath))

        If Not CrcOnly.Checked AndAlso Not MeasureOnly.Checked Then
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

    Private Function RunConversion(processInfo As ProcessStartInfo, ByRef successDetails As String, ByRef failureDetails As String) As Boolean
        Using conversionProcess As Process = Process.Start(processInfo)
            If conversionProcess Is Nothing Then
                failureDetails = "The maxcso process could not be started."
                Return False
            End If

            Dim standardOutput As String = conversionProcess.StandardOutput.ReadToEnd()
            Dim standardError As String = conversionProcess.StandardError.ReadToEnd()
            conversionProcess.WaitForExit()

            successDetails = (standardOutput & Environment.NewLine & standardError).Trim()
            failureDetails = If(String.IsNullOrWhiteSpace(standardError), standardOutput, standardError)

            Return conversionProcess.ExitCode = 0
        End Using
    End Function

    Private Function QuoteArgument(value As String) As String
        Return """" & value & """"
    End Function
End Class
