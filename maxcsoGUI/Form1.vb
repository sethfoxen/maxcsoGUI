Public Class maxcsoGUI
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles About.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ThreadSelection.SelectedIndexChanged
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Convert.Click
        'Check the maxcso binary has been placed correctly
        If My.Computer.FileSystem.FileExists("maxcso.exe") Then
            'Make sure the thread selection has be chosen
            If IsNumeric(ThreadSelection.SelectedItem) = True Then
                'Loop through the file list
                Do
                    'Store the first item in the list
                    Dim NextSelec As String = FileList.Items(0).ToString
                    'Store the selected options as a string
                    Dim arg As String = ""
                    If Fast.Checked = True Then arg = arg & " --fast"
                    If Zopfli.Checked = True Then arg = arg & " --use-zopfli"
                    If BlockSize.Checked = True Then arg = arg & " --block=" & BlockText.Text
                    'Store the process command with arguments
                    Dim maxcso As New ProcessStartInfo("maxcso.exe", " --threads=" & ThreadSelection.SelectedItem & " " & arg & " " & Chr(34) & NextSelec & Chr(34) & " -o " & Chr(34) & NextSelec.Substring(0, NextSelec.Length - 3) & "cso" & Chr(34))
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
                Loop Until FileList.Items.Count = 0
                MessageBox.Show("Conversion Completed!")
            Else
                MessageBox.Show("Please select how many threads to run.")
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
End Class
