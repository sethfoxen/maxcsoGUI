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
        Me.CustOut = New System.Windows.Forms.TextBox()
        Me.Browse = New System.Windows.Forms.Button()
        Me.CustDir = New System.Windows.Forms.CheckBox()
        Me.Decompress = New System.Windows.Forms.CheckBox()
        Me.BlockText = New System.Windows.Forms.TextBox()
        Me.BlockSize = New System.Windows.Forms.CheckBox()
        Me.DeleteCheck = New System.Windows.Forms.CheckBox()
        Me.Zopfli = New System.Windows.Forms.CheckBox()
        Me.Fast = New System.Windows.Forms.CheckBox()
        Me.FormatSelection = New System.Windows.Forms.ComboBox()
        Me.ThreadSelection = New System.Windows.Forms.ComboBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Lz4CostText = New System.Windows.Forms.TextBox()
        Me.Lz4Cost = New System.Windows.Forms.CheckBox()
        Me.OrigCostText = New System.Windows.Forms.TextBox()
        Me.OrigCost = New System.Windows.Forms.CheckBox()
        Me.UseLz4Brute = New System.Windows.Forms.CheckBox()
        Me.UseLibdeflate = New System.Windows.Forms.CheckBox()
        Me.MeasureOnly = New System.Windows.Forms.CheckBox()
        Me.CrcOnly = New System.Windows.Forms.CheckBox()
        Me.FileList = New System.Windows.Forms.ListBox()
        Me.DropHelp = New System.Windows.Forms.Label()
        Me.Convert = New System.Windows.Forms.Button()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'About
        '
        Me.About.Location = New System.Drawing.Point(299, 265)
        Me.About.Name = "About"
        Me.About.Size = New System.Drawing.Size(75, 23)
        Me.About.TabIndex = 0
        Me.About.Text = "About"
        Me.About.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.CustOut)
        Me.GroupBox1.Controls.Add(Me.Browse)
        Me.GroupBox1.Controls.Add(Me.CustDir)
        Me.GroupBox1.Controls.Add(Me.Decompress)
        Me.GroupBox1.Controls.Add(Me.BlockText)
        Me.GroupBox1.Controls.Add(Me.BlockSize)
        Me.GroupBox1.Controls.Add(Me.DeleteCheck)
        Me.GroupBox1.Controls.Add(Me.Zopfli)
        Me.GroupBox1.Controls.Add(Me.Fast)
        Me.GroupBox1.Controls.Add(Me.FormatSelection)
        Me.GroupBox1.Controls.Add(Me.ThreadSelection)
        Me.GroupBox1.Location = New System.Drawing.Point(299, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(454, 126)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Options"
        '
        'CustOut
        '
        Me.CustOut.Enabled = False
        Me.CustOut.Location = New System.Drawing.Point(255, 41)
        Me.CustOut.Name = "CustOut"
        Me.CustOut.Size = New System.Drawing.Size(193, 20)
        Me.CustOut.TabIndex = 14
        '
        'Browse
        '
        Me.Browse.Enabled = False
        Me.Browse.Location = New System.Drawing.Point(373, 12)
        Me.Browse.Name = "Browse"
        Me.Browse.Size = New System.Drawing.Size(75, 23)
        Me.Browse.TabIndex = 13
        Me.Browse.Text = "Browse"
        Me.Browse.UseVisualStyleBackColor = True
        '
        'CustDir
        '
        Me.CustDir.AutoSize = True
        Me.CustDir.Location = New System.Drawing.Point(255, 18)
        Me.CustDir.Name = "CustDir"
        Me.CustDir.Size = New System.Drawing.Size(112, 17)
        Me.CustDir.TabIndex = 12
        Me.CustDir.Text = "Custom Output Dir"
        Me.CustDir.UseVisualStyleBackColor = True
        '
        'Decompress
        '
        Me.Decompress.AutoSize = True
        Me.Decompress.Location = New System.Drawing.Point(137, 71)
        Me.Decompress.Name = "Decompress"
        Me.Decompress.Size = New System.Drawing.Size(85, 17)
        Me.Decompress.TabIndex = 11
        Me.Decompress.Text = "Decompress"
        Me.ToolTip1.SetToolTip(Me.Decompress, "Write out to raw ISO, decompressing as needed")
        Me.Decompress.UseVisualStyleBackColor = True
        '
        'BlockText
        '
        Me.BlockText.Enabled = False
        Me.BlockText.Location = New System.Drawing.Point(10, 94)
        Me.BlockText.Name = "BlockText"
        Me.BlockText.Size = New System.Drawing.Size(100, 20)
        Me.BlockText.TabIndex = 10
        Me.BlockText.Text = "2048"
        '
        'BlockSize
        '
        Me.BlockSize.AutoSize = True
        Me.BlockSize.Location = New System.Drawing.Point(10, 71)
        Me.BlockSize.Name = "BlockSize"
        Me.BlockSize.Size = New System.Drawing.Size(91, 17)
        Me.BlockSize.TabIndex = 9
        Me.BlockSize.Text = "Alt Block Size"
        Me.ToolTip1.SetToolTip(Me.BlockSize, "Specify a block size (default depends on iso size)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Many readers only support the" &
        " 2048 size" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(DON'T CHECK UNLESS YOU KNOW WHAT YOU'RE DOING)")
        Me.BlockSize.UseVisualStyleBackColor = True
        '
        'DeleteCheck
        '
        Me.DeleteCheck.AutoSize = True
        Me.DeleteCheck.Location = New System.Drawing.Point(255, 79)
        Me.DeleteCheck.Name = "DeleteCheck"
        Me.DeleteCheck.Size = New System.Drawing.Size(119, 17)
        Me.DeleteCheck.TabIndex = 8
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
        Me.Zopfli.TabIndex = 7
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
        Me.Fast.TabIndex = 6
        Me.Fast.Text = "Fast Mode"
        Me.ToolTip1.SetToolTip(Me.Fast, "Use only basic zlib or lz4 for fastest result." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Will result in bigger files.)")
        Me.Fast.UseVisualStyleBackColor = True
        '
        'FormatSelection
        '
        Me.FormatSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.FormatSelection.FormattingEnabled = True
        Me.FormatSelection.Location = New System.Drawing.Point(10, 44)
        Me.FormatSelection.Name = "FormatSelection"
        Me.FormatSelection.Size = New System.Drawing.Size(121, 21)
        Me.FormatSelection.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.FormatSelection, "Select the output format to create when compressing.")
        '
        'ThreadSelection
        '
        Me.ThreadSelection.FormattingEnabled = True
        Me.ThreadSelection.Location = New System.Drawing.Point(10, 19)
        Me.ThreadSelection.Name = "ThreadSelection"
        Me.ThreadSelection.Size = New System.Drawing.Size(121, 21)
        Me.ThreadSelection.TabIndex = 4
        Me.ThreadSelection.Text = "Threads"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Lz4CostText)
        Me.GroupBox2.Controls.Add(Me.Lz4Cost)
        Me.GroupBox2.Controls.Add(Me.OrigCostText)
        Me.GroupBox2.Controls.Add(Me.OrigCost)
        Me.GroupBox2.Controls.Add(Me.UseLz4Brute)
        Me.GroupBox2.Controls.Add(Me.UseLibdeflate)
        Me.GroupBox2.Controls.Add(Me.MeasureOnly)
        Me.GroupBox2.Controls.Add(Me.CrcOnly)
        Me.GroupBox2.Location = New System.Drawing.Point(299, 129)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(454, 100)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Advanced"
        '
        'Lz4CostText
        '
        Me.Lz4CostText.Enabled = False
        Me.Lz4CostText.Location = New System.Drawing.Point(360, 61)
        Me.Lz4CostText.Name = "Lz4CostText"
        Me.Lz4CostText.Size = New System.Drawing.Size(70, 20)
        Me.Lz4CostText.TabIndex = 7
        Me.Lz4CostText.Text = "0"
        '
        'Lz4Cost
        '
        Me.Lz4Cost.AutoSize = True
        Me.Lz4Cost.Location = New System.Drawing.Point(262, 63)
        Me.Lz4Cost.Name = "Lz4Cost"
        Me.Lz4Cost.Size = New System.Drawing.Size(74, 17)
        Me.Lz4Cost.TabIndex = 6
        Me.Lz4Cost.Text = "LZ4 Cost %"
        Me.ToolTip1.SetToolTip(Me.Lz4Cost, "Allow lz4 to increase block size by this percent at most." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(CSO v2 only.)")
        Me.Lz4Cost.UseVisualStyleBackColor = True
        '
        'OrigCostText
        '
        Me.OrigCostText.Enabled = False
        Me.OrigCostText.Location = New System.Drawing.Point(360, 25)
        Me.OrigCostText.Name = "OrigCostText"
        Me.OrigCostText.Size = New System.Drawing.Size(70, 20)
        Me.OrigCostText.TabIndex = 5
        Me.OrigCostText.Text = "0"
        '
        'OrigCost
        '
        Me.OrigCost.AutoSize = True
        Me.OrigCost.Location = New System.Drawing.Point(262, 27)
        Me.OrigCost.Name = "OrigCost"
        Me.OrigCost.Size = New System.Drawing.Size(76, 17)
        Me.OrigCost.TabIndex = 4
        Me.OrigCost.Text = "Orig Cost %"
        Me.ToolTip1.SetToolTip(Me.OrigCost, "Allow uncompressed data to increase block size by this percent at most.")
        Me.OrigCost.UseVisualStyleBackColor = True
        '
        'UseLz4Brute
        '
        Me.UseLz4Brute.AutoSize = True
        Me.UseLz4Brute.Location = New System.Drawing.Point(133, 63)
        Me.UseLz4Brute.Name = "UseLz4Brute"
        Me.UseLz4Brute.Size = New System.Drawing.Size(104, 17)
        Me.UseLz4Brute.TabIndex = 3
        Me.UseLz4Brute.Text = "Enable LZ4 Brute"
        Me.ToolTip1.SetToolTip(Me.UseLz4Brute, "Enable bruteforce trials with lz4hc for lz4 compression.")
        Me.UseLz4Brute.UseVisualStyleBackColor = True
        '
        'UseLibdeflate
        '
        Me.UseLibdeflate.AutoSize = True
        Me.UseLibdeflate.Location = New System.Drawing.Point(133, 27)
        Me.UseLibdeflate.Name = "UseLibdeflate"
        Me.UseLibdeflate.Size = New System.Drawing.Size(105, 17)
        Me.UseLibdeflate.TabIndex = 2
        Me.UseLibdeflate.Text = "Enable libdeflate"
        Me.ToolTip1.SetToolTip(Me.UseLibdeflate, "Enable trials with libdeflate compression.")
        Me.UseLibdeflate.UseVisualStyleBackColor = True
        '
        'MeasureOnly
        '
        Me.MeasureOnly.AutoSize = True
        Me.MeasureOnly.Location = New System.Drawing.Point(10, 63)
        Me.MeasureOnly.Name = "MeasureOnly"
        Me.MeasureOnly.Size = New System.Drawing.Size(90, 17)
        Me.MeasureOnly.TabIndex = 1
        Me.MeasureOnly.Text = "Measure Only"
        Me.ToolTip1.SetToolTip(Me.MeasureOnly, "Measure compressed size without saving output.")
        Me.MeasureOnly.UseVisualStyleBackColor = True
        '
        'CrcOnly
        '
        Me.CrcOnly.AutoSize = True
        Me.CrcOnly.Location = New System.Drawing.Point(10, 27)
        Me.CrcOnly.Name = "CrcOnly"
        Me.CrcOnly.Size = New System.Drawing.Size(82, 17)
        Me.CrcOnly.TabIndex = 0
        Me.CrcOnly.Text = "CRC32 Only"
        Me.ToolTip1.SetToolTip(Me.CrcOnly, "Log CRC32 checksums and skip output files.")
        Me.CrcOnly.UseVisualStyleBackColor = True
        '
        'FileList
        '
        Me.FileList.AllowDrop = True
        Me.FileList.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.FileList.FormattingEnabled = True
        Me.FileList.ImeMode = System.Windows.Forms.ImeMode.[On]
        Me.FileList.Location = New System.Drawing.Point(12, 9)
        Me.FileList.Name = "FileList"
        Me.FileList.Size = New System.Drawing.Size(280, 264)
        Me.FileList.TabIndex = 4
        Me.FileList.Tag = ""
        '
        'DropHelp
        '
        Me.DropHelp.Location = New System.Drawing.Point(299, 235)
        Me.DropHelp.Name = "DropHelp"
        Me.DropHelp.Size = New System.Drawing.Size(448, 24)
        Me.DropHelp.TabIndex = 5
        Me.DropHelp.Text = "Drag and drop ISO files to this box. The output files will be produced in the same " &
    "directory as the ISO unless the Custom Output Dir box is checked."
        Me.DropHelp.UseCompatibleTextRendering = True
        '
        'Convert
        '
        Me.Convert.Location = New System.Drawing.Point(678, 265)
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
        'maxcsoGUI
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(765, 297)
        Me.Controls.Add(Me.FileList)
        Me.Controls.Add(Me.GroupBox2)
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
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents About As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents ThreadSelection As ComboBox
    Friend WithEvents FileList As ListBox
    Friend WithEvents DropHelp As Label
    Friend WithEvents Convert As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Lz4CostText As TextBox
    Friend WithEvents Lz4Cost As CheckBox
    Friend WithEvents OrigCostText As TextBox
    Friend WithEvents OrigCost As CheckBox
    Friend WithEvents UseLz4Brute As CheckBox
    Friend WithEvents UseLibdeflate As CheckBox
    Friend WithEvents MeasureOnly As CheckBox
    Friend WithEvents CrcOnly As CheckBox
    Friend WithEvents Zopfli As CheckBox
    Friend WithEvents Fast As CheckBox
    Friend WithEvents FormatSelection As ComboBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents DeleteCheck As CheckBox
    Friend WithEvents BlockText As TextBox
    Friend WithEvents BlockSize As CheckBox
    Friend WithEvents Decompress As CheckBox
    Friend WithEvents CustDir As CheckBox
    Friend WithEvents Browse As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents CustOut As TextBox
End Class
