Public Class maxcsoGUI
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles About.Click
        AboutBox1.ShowDialog()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ThreadSelection.SelectedIndexChanged
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Convert.Click
        If IsNumeric(ThreadSelection.SelectedItem) = True Then
            Do
                Dim NextSelec As String = FileList.Items(0).ToString
                Dim maxcso As New ProcessStartInfo("maxcso.exe", " --threads=" & ThreadSelection.SelectedItem & " " & Chr(34) & NextSelec & Chr(34) & " -o " & Chr(34) & NextSelec.Substring(0, NextSelec.Length - 3) & "cso" & Chr(34))
                Dim Thread As Process = Process.Start(maxcso)
                Thread.WaitForExit()
                FileList.Items.RemoveAt(0)
            Loop Until FileList.Items.Count = 0
            MessageBox.Show("Conversion Completed!")
        Else
            MessageBox.Show("Please select how many threads to run.")
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
End Class
