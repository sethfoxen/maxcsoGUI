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
        Me.components = New System.ComponentModel.Container()
        Me.About = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.BlockText = New System.Windows.Forms.TextBox()
        Me.BlockSize = New System.Windows.Forms.CheckBox()
        Me.DeleteCheck = New System.Windows.Forms.CheckBox()
        Me.Zopfli = New System.Windows.Forms.CheckBox()
        Me.Fast = New System.Windows.Forms.CheckBox()
        Me.ThreadSelection = New System.Windows.Forms.ComboBox()
        Me.FileList = New System.Windows.Forms.ListBox()
        Me.DropHelp = New System.Windows.Forms.Label()
        Me.Convert = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Decompress = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'About
        '
        Me.About.Location = New System.Drawing.Point(489, 143)
        Me.About.Name = "About"
        Me.About.Size = New System.Drawing.Size(75, 23)
        Me.About.TabIndex = 0
        Me.About.Text = "About"
        Me.About.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Decompress)
        Me.GroupBox1.Controls.Add(Me.BlockText)
        Me.GroupBox1.Controls.Add(Me.BlockSize)
        Me.GroupBox1.Controls.Add(Me.DeleteCheck)
        Me.GroupBox1.Controls.Add(Me.Zopfli)
        Me.GroupBox1.Controls.Add(Me.Fast)
        Me.GroupBox1.Controls.Add(Me.ThreadSelection)
        Me.GroupBox1.Location = New System.Drawing.Point(299, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(265, 102)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Options"
        '
        'BlockText
        '
        Me.BlockText.Enabled = False
        Me.BlockText.Location = New System.Drawing.Point(7, 70)
        Me.BlockText.Name = "BlockText"
        Me.BlockText.Size = New System.Drawing.Size(100, 20)
        Me.BlockText.TabIndex = 9
        Me.BlockText.Text = "2048"
        '
        'BlockSize
        '
        Me.BlockSize.AutoSize = True
        Me.BlockSize.Location = New System.Drawing.Point(10, 46)
        Me.BlockSize.Name = "BlockSize"
        Me.BlockSize.Size = New System.Drawing.Size(91, 17)
        Me.BlockSize.TabIndex = 8
        Me.BlockSize.Text = "Alt Block Size"
        Me.ToolTip1.SetToolTip(Me.BlockSize, "Specify a block size (default depends on iso size)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Many readers only support the" &
        " 2048 size" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(DON'T CHECK UNLESS YOU KNOW WHAT YOU'RE DOING)")
        Me.BlockSize.UseVisualStyleBackColor = True
        '
        'DeleteCheck
        '
        Me.DeleteCheck.AutoSize = True
        Me.DeleteCheck.Location = New System.Drawing.Point(137, 58)
        Me.DeleteCheck.Name = "DeleteCheck"
        Me.DeleteCheck.Size = New System.Drawing.Size(119, 17)
        Me.DeleteCheck.TabIndex = 7
        Me.DeleteCheck.Text = "Delete Original Files"
        Me.ToolTip1.SetToolTip(Me.DeleteCheck, "Check to delete the original ISO files after they've been converted." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(BE CAREFUL" &
        "!! EVEN IF A FILE FAILS TO CONVERT, THE ORIGINAL WILL STILL BE DELETED!!)")
        Me.DeleteCheck.UseVisualStyleBackColor = True
        '
        'Zopfli
        '
        Me.Zopfli.AutoSize = True
        Me.Zopfli.Location = New System.Drawing.Point(137, 35)
        Me.Zopfli.Name = "Zopfli"
        Me.Zopfli.Size = New System.Drawing.Size(88, 17)
        Me.Zopfli.TabIndex = 6
        Me.Zopfli.Text = "Enable Zopfli"
        Me.ToolTip1.SetToolTip(Me.Zopfli, "Enable trials with Zopfli for deflate compression." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Significantly slower, uses m" &
        "ore memory, marginally smaller files.)")
        Me.Zopfli.UseVisualStyleBackColor = True
        '
        'Fast
        '
        Me.Fast.AutoSize = True
        Me.Fast.Location = New System.Drawing.Point(137, 12)
        Me.Fast.Name = "Fast"
        Me.Fast.Size = New System.Drawing.Size(76, 17)
        Me.Fast.TabIndex = 5
        Me.Fast.Text = "Fast Mode"
        Me.ToolTip1.SetToolTip(Me.Fast, "Use only basic zlib or lz4 for fastest result." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Will result in bigger files.)")
        Me.Fast.UseVisualStyleBackColor = True
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
        'FileList
        '
        Me.FileList.AllowDrop = True
        Me.FileList.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.FileList.FormattingEnabled = True
        Me.FileList.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.FileList.Location = New System.Drawing.Point(12, 9)
        Me.FileList.Name = "FileList"
        Me.FileList.Size = New System.Drawing.Size(280, 147)
        Me.FileList.TabIndex = 4
        Me.FileList.Tag = ""
        '
        'DropHelp
        '
        Me.DropHelp.Location = New System.Drawing.Point(298, 116)
        Me.DropHelp.Name = "DropHelp"
        Me.DropHelp.Size = New System.Drawing.Size(256, 24)
        Me.DropHelp.TabIndex = 5
        Me.DropHelp.Text = "Drag and drop ISO files to this box, the CSO files will be produced in the same d" &
    "irectory as the ISO."
        Me.DropHelp.UseCompatibleTextRendering = True
        '
        'Convert
        '
        Me.Convert.Location = New System.Drawing.Point(408, 143)
        Me.Convert.Name = "Convert"
        Me.Convert.Size = New System.Drawing.Size(75, 23)
        Me.Convert.TabIndex = 3
        Me.Convert.Text = "Convert"
        Me.Convert.UseVisualStyleBackColor = True
        '
        'ToolTip1
        '
        Me.ToolTip1.AutomaticDelay = 0
        Me.ToolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info
        '
        'Decompress
        '
        Me.Decompress.AutoSize = True
        Me.Decompress.Location = New System.Drawing.Point(137, 81)
        Me.Decompress.Name = "Decompress"
        Me.Decompress.Size = New System.Drawing.Size(85, 17)
        Me.Decompress.TabIndex = 10
        Me.Decompress.Text = "Decompress"
        Me.ToolTip1.SetToolTip(Me.Decompress, "Write out to raw ISO, decompressing as needed")
        Me.Decompress.UseVisualStyleBackColor = True
        '
        'maxcsoGUI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(568, 170)
        Me.Controls.Add(Me.FileList)
        Me.Controls.Add(Me.DropHelp)
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
    Friend WithEvents FileList As ListBox
    Friend WithEvents DropHelp As Label
    Friend WithEvents Convert As Button
    Friend WithEvents Zopfli As CheckBox
    Friend WithEvents Fast As CheckBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents DeleteCheck As CheckBox
    Friend WithEvents BlockText As TextBox
    Friend WithEvents BlockSize As CheckBox
    Friend WithEvents Decompress As CheckBox
End Class
