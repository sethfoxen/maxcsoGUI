<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class maxcsoGUI
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Me.About = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Note = New System.Windows.Forms.Label()
        Me.ThreadSelection = New System.Windows.Forms.ComboBox()
        Me.Convert = New System.Windows.Forms.Button()
        Me.FileList = New System.Windows.Forms.ListBox()
        Me.DropHelp = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'About
        '
        Me.About.Location = New System.Drawing.Point(380, 172)
        Me.About.Name = "About"
        Me.About.Size = New System.Drawing.Size(75, 23)
        Me.About.TabIndex = 0
        Me.About.Text = "About"
        Me.About.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Note)
        Me.GroupBox1.Controls.Add(Me.ThreadSelection)
        Me.GroupBox1.Location = New System.Drawing.Point(299, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(156, 67)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Options"
        '
        'Note
        '
        Me.Note.AutoSize = True
        Me.Note.Location = New System.Drawing.Point(6, 51)
        Me.Note.Name = "Note"
        Me.Note.Size = New System.Drawing.Size(109, 13)
        Me.Note.TabIndex = 4
        Me.Note.Text = "Surely more to come?"
        '
        'ThreadSelection
        '
        Me.ThreadSelection.FormattingEnabled = True
        Me.ThreadSelection.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6", "7", "8"})
        Me.ThreadSelection.Location = New System.Drawing.Point(10, 19)
        Me.ThreadSelection.Name = "ThreadSelection"
        Me.ThreadSelection.Size = New System.Drawing.Size(121, 21)
        Me.ThreadSelection.TabIndex = 3
        Me.ThreadSelection.Text = "Threads"
        '
        'Convert
        '
        Me.Convert.Location = New System.Drawing.Point(299, 172)
        Me.Convert.Name = "Convert"
        Me.Convert.Size = New System.Drawing.Size(75, 23)
        Me.Convert.TabIndex = 3
        Me.Convert.Text = "Convert"
        Me.Convert.UseVisualStyleBackColor = True
        '
        'FileList
        '
        Me.FileList.AllowDrop = True
        Me.FileList.FormattingEnabled = True
        Me.FileList.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.FileList.Location = New System.Drawing.Point(12, 12)
        Me.FileList.Name = "FileList"
        Me.FileList.Size = New System.Drawing.Size(281, 186)
        Me.FileList.TabIndex = 4
        Me.FileList.Tag = ""
        '
        'DropHelp
        '
        Me.DropHelp.Location = New System.Drawing.Point(299, 82)
        Me.DropHelp.Name = "DropHelp"
        Me.DropHelp.Size = New System.Drawing.Size(156, 87)
        Me.DropHelp.TabIndex = 5
        Me.DropHelp.Text = "Drag and drop ISO files to this box, the CSO files will be produced in the same d" &
    "irectory as the ISO."
        Me.DropHelp.UseCompatibleTextRendering = True
        '
        'maxcsoGUI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(460, 210)
        Me.Controls.Add(Me.DropHelp)
        Me.Controls.Add(Me.FileList)
        Me.Controls.Add(Me.Convert)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.About)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "maxcsoGUI"
        Me.Text = "maxcsoGUI"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents About As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents ThreadSelection As ComboBox
    Friend WithEvents Convert As Button
    Friend WithEvents FileList As ListBox
    Friend WithEvents Note As Label
    Friend WithEvents DropHelp As Label
End Class
