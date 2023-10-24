Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class maxcsoGUI
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim cores As Integer = (Environment.ProcessorCount)
        Dim count As Integer
        Do
            count += 1
            ThreadSelection.Items.Add(count)
        Loop Until count = cores
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles About.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ThreadSelection.SelectedIndexChanged
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Convert.Click
        'Check the maxcso binary has been placed correctly
        If My.Computer.FileSystem.FileExists("maxcso.exe") Then
            'Make sure the thread selection has been chosen
            If IsNumeric(ThreadSelection.SelectedItem) = True Then
                'Loop through the file list
                Do
                    If Decompress.Checked = False Then
                        'Store the first item in the list
                        Dim NextSelec As String = FileList.Items(0).ToString
                        'Find the file name
                        Dim FileName As String = NextSelec.Substring(NextSelec.LastIndexOf("\"))
                        'Store the selected options as a string
                        Dim arg As String = ""
                        Dim out As String = ""
                        If Decompress.Checked = True Then arg = arg & " --decompress"
                        If Fast.Checked = True Then arg = arg & " --fast"
                        If Zopfli.Checked = True Then arg = arg & " --use-zopfli"
                        If BlockSize.Checked = True Then arg = arg & " --block=" & BlockText.Text
                        'Custom directory was a pain in my ass, god I hope I composed this string properly
                        If CustDir.Checked = True Then out = Chr(34) & CustOut.Text & FileName.Substring(0, FileName.Length - 3) & "cso" & Chr(34) Else out = Chr(34) & NextSelec.Substring(0, NextSelec.Length - 3) & "cso" & Chr(34)
                        'Store the process command with arguments
                        Dim maxcso As New ProcessStartInfo("maxcso.exe", " --threads=" & ThreadSelection.SelectedItem & " " & arg & " " & Chr(34) & NextSelec & Chr(34) & " -o " & out)
                        'Start to the maxcso process
                        Dim Thread As Process = Process.Start(maxcso)
                        'Wait until the file finishes processing before moving on
                        Thread.WaitForExit()
                        'Delete original files, if applicable
                        If DeleteCheck.Checked = True Then
                            'Delete the first item in the list off the PC
                            My.Computer.FileSystem.DeleteFile(NextSelec, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                        End If
                        'Remove the first item from the list
                        FileList.Items.RemoveAt(0)
                    Else
                        'Store the first item in the list
                        Dim NextSelec As String = FileList.Items(0).ToString
                        'Find the file name
                        Dim FileName As String = NextSelec.Substring(NextSelec.LastIndexOf("\"))
                        'Store the selected options as a string
                        Dim arg As String = ""
                        Dim out As String = ""
                        If Decompress.Checked = True Then arg = arg & " --decompress"
                        If CustDir.Checked = True Then out = Chr(34) & CustOut.Text & FileName.Substring(0, FileName.Length - 3) & "iso" & Chr(34) Else out = Chr(34) & NextSelec.Substring(0, NextSelec.Length - 3) & "iso" & Chr(34)
                        'Store the process command with arguments
                        Dim maxcso As New ProcessStartInfo("maxcso.exe", " --threads=" & ThreadSelection.SelectedItem & " " & arg & " " & Chr(34) & NextSelec & Chr(34) & " -o " & out)
                        'Start to the maxcso process
                        Dim Thread As Process = Process.Start(maxcso)
                        'Wait until the file finishes processing before moving on
                        Thread.WaitForExit()
                        'Delete original files, if applicable
                        If DeleteCheck.Checked = True Then
                            'Delete the first item in the list off the PC
                            My.Computer.FileSystem.DeleteFile(NextSelec, FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                        End If
                        'Remove the first item from the list
                        FileList.Items.RemoveAt(0)
                    End If

                Loop Until FileList.Items.Count = 0
                MessageBox.Show("Conversion Completed!")
            Else
                MessageBox.Show("Please Select how many threads To run.")
            End If
        Else
            MessageBox.Show("I can't find the maxcso binary, was it placed side by side with this executable?")
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
        'Enable/Disable the Zopfli check box if the Fast Mode box is unchecked/checked
        If Fast.Checked = True Then
            Zopfli.Enabled = False
        End If
        If Fast.Checked = False Then
            Zopfli.Enabled = True
        End If
    End Sub

    Private Sub BlockSize_CheckedChanged(sender As Object, e As EventArgs) Handles BlockSize.CheckedChanged
        'Enable the Block Size text box when needed
        If BlockSize.Checked = True Then
            BlockText.Enabled = True
        ElseIf BlockSize.Checked = False Then
            BlockText.Enabled = False
        End If
    End Sub

    Private Sub Zopfli_CheckedChanged(sender As Object, e As EventArgs) Handles Zopfli.CheckedChanged
        'Enable/Disable the Fast Mode check box if the Zopfli box is unchecked/checked
        If Zopfli.Checked = True Then
            Fast.Enabled = False
        End If
        If Zopfli.Checked = False Then
            Fast.Enabled = True
        End If

    End Sub
    Private Sub Decompress_CheckedChanged(sender As Object, e As EventArgs) Handles Decompress.CheckedChanged
        If Decompress.Checked = True Then
            Fast.Enabled = False
            Zopfli.Enabled = False
            BlockText.Enabled = False
            BlockSize.Enabled = False
        Else
            Fast.Enabled = True
            Zopfli.Enabled = True
            BlockText.Enabled = True
            BlockSize.Enabled = True

        End If
    End Sub
    Private Sub DeleteCheck_CheckedChanged(sender As Object, e As EventArgs) Handles DeleteCheck.CheckedChanged

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles Decompress.CheckedChanged

    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub CustDir_CheckedChanged(sender As Object, e As EventArgs) Handles CustDir.CheckedChanged
        If CustDir.Checked = True Then
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
End Class
