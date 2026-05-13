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
        Me.ModeSelection = New System.Windows.Forms.ComboBox()
        Me.BlockText = New System.Windows.Forms.TextBox()
        Me.BlockSize = New System.Windows.Forms.CheckBox()
        Me.DeleteCheck = New System.Windows.Forms.CheckBox()
        Me.Fast = New System.Windows.Forms.CheckBox()
        Me.FormatSelection = New System.Windows.Forms.ComboBox()
        Me.ThreadSelection = New System.Windows.Forms.ComboBox()
        Me.CrcOnly = New System.Windows.Forms.CheckBox()
        Me.MeasureOnly = New System.Windows.Forms.CheckBox()
        Me.PoolHelpLabel = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.UseZlib = New System.Windows.Forms.CheckBox()
        Me.UseZopfli = New System.Windows.Forms.CheckBox()
        Me.Use7zDeflate = New System.Windows.Forms.CheckBox()
        Me.UseLz4 = New System.Windows.Forms.CheckBox()
        Me.UseLz4Brute = New System.Windows.Forms.CheckBox()
        Me.UseLibdeflate = New System.Windows.Forms.CheckBox()
        Me.PoolDivider = New System.Windows.Forms.Label()
        Me.Lz4CostText = New System.Windows.Forms.TextBox()
        Me.Lz4Cost = New System.Windows.Forms.CheckBox()
        Me.OrigCostText = New System.Windows.Forms.TextBox()
        Me.OrigCost = New System.Windows.Forms.CheckBox()
        Me.FileList = New DragDropListBox()
        Me.DropHelp = New System.Windows.Forms.Label()
        Me.ProgressText = New System.Windows.Forms.Label()
        Me.ConversionProgress = New System.Windows.Forms.ProgressBar()
        Me.Convert = New System.Windows.Forms.Button()
        Me.ProgressBytes = New System.Windows.Forms.Label()
        Me.ProgressPercent = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.FolderBrowserDialog1 = New System.Windows.Forms.FolderBrowserDialog()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'About
        '
        Me.About.Location = New System.Drawing.Point(12, 249)
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
        Me.GroupBox1.Controls.Add(Me.ModeSelection)
        Me.GroupBox1.Controls.Add(Me.BlockText)
        Me.GroupBox1.Controls.Add(Me.BlockSize)
        Me.GroupBox1.Controls.Add(Me.CrcOnly)
        Me.GroupBox1.Controls.Add(Me.MeasureOnly)
        Me.GroupBox1.Controls.Add(Me.DeleteCheck)
        Me.GroupBox1.Controls.Add(Me.Fast)
        Me.GroupBox1.Controls.Add(Me.FormatSelection)
        Me.GroupBox1.Controls.Add(Me.ThreadSelection)
        Me.GroupBox1.Location = New System.Drawing.Point(379, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(454, 98)
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
        'ModeSelection
        '
        Me.ModeSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ModeSelection.FormattingEnabled = True
        Me.ModeSelection.Location = New System.Drawing.Point(10, 19)
        Me.ModeSelection.Name = "ModeSelection"
        Me.ModeSelection.Size = New System.Drawing.Size(100, 21)
        Me.ModeSelection.TabIndex = 4
        Me.ToolTip1.SetToolTip(Me.ModeSelection, "Choose whether to compress to a container format or decompress back to ISO.")
        '
        'BlockText
        '
        Me.BlockText.Enabled = False
        Me.BlockText.Location = New System.Drawing.Point(104, 69)
        Me.BlockText.Name = "BlockText"
        Me.BlockText.Size = New System.Drawing.Size(40, 20)
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
        Me.DeleteCheck.Location = New System.Drawing.Point(334, 71)
        Me.DeleteCheck.Name = "DeleteCheck"
        Me.DeleteCheck.Size = New System.Drawing.Size(119, 17)
        Me.DeleteCheck.TabIndex = 8
        Me.DeleteCheck.Text = "Delete Original Files"
        Me.ToolTip1.SetToolTip(Me.DeleteCheck, "Check to delete the original ISO files after they've been converted." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(BE CAREFUL" &
        "!! EVEN IF A FILE FAILS TO CONVERT, THE ORIGINAL WILL STILL BE DELETED!!)")
        Me.DeleteCheck.UseVisualStyleBackColor = True
        '
        'Fast
        '
        Me.Fast.AutoSize = True
        Me.Fast.Location = New System.Drawing.Point(116, 48)
        Me.Fast.Name = "Fast"
        Me.Fast.Size = New System.Drawing.Size(76, 17)
        Me.Fast.TabIndex = 7
        Me.Fast.Text = "Fast Mode"
        Me.ToolTip1.SetToolTip(Me.Fast, "Use only basic zlib or lz4 for fastest result." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Will result in bigger files.)")
        Me.Fast.UseVisualStyleBackColor = True
        '
        'FormatSelection
        '
        Me.FormatSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.FormatSelection.FormattingEnabled = True
        Me.FormatSelection.Location = New System.Drawing.Point(116, 19)
        Me.FormatSelection.Name = "FormatSelection"
        Me.FormatSelection.Size = New System.Drawing.Size(133, 21)
        Me.FormatSelection.TabIndex = 5
        Me.ToolTip1.SetToolTip(Me.FormatSelection, "Select the output format to create when compressing.")
        '
        'ThreadSelection
        '
        Me.ThreadSelection.FormattingEnabled = True
        Me.ThreadSelection.Location = New System.Drawing.Point(10, 44)
        Me.ThreadSelection.Name = "ThreadSelection"
        Me.ThreadSelection.Size = New System.Drawing.Size(100, 21)
        Me.ThreadSelection.TabIndex = 6
        Me.ThreadSelection.Text = "Threads"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.UseZlib)
        Me.GroupBox2.Controls.Add(Me.UseZopfli)
        Me.GroupBox2.Controls.Add(Me.Use7zDeflate)
        Me.GroupBox2.Controls.Add(Me.UseLz4)
        Me.GroupBox2.Controls.Add(Me.UseLz4Brute)
        Me.GroupBox2.Controls.Add(Me.UseLibdeflate)
        Me.GroupBox2.Controls.Add(Me.PoolDivider)
        Me.GroupBox2.Controls.Add(Me.OrigCost)
        Me.GroupBox2.Controls.Add(Me.OrigCostText)
        Me.GroupBox2.Controls.Add(Me.Lz4Cost)
        Me.GroupBox2.Controls.Add(Me.Lz4CostText)
        Me.GroupBox2.Location = New System.Drawing.Point(379, 101)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(454, 96)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Compression Algorithms Trial Pool"
        '
        'UseZlib
        '
        Me.UseZlib.AutoSize = True
        Me.UseZlib.Location = New System.Drawing.Point(10, 20)
        Me.UseZlib.Name = "UseZlib"
        Me.UseZlib.Size = New System.Drawing.Size(78, 17)
        Me.UseZlib.TabIndex = 0
        Me.UseZlib.Text = "zlib"
        Me.ToolTip1.SetToolTip(Me.UseZlib, "Enable trials with zlib for deflate compression." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Default method - broadly compatible with a balanced speed/size trade-off.)")
        Me.UseZlib.UseVisualStyleBackColor = True
        '
        'UseZopfli
        '
        Me.UseZopfli.AutoSize = True
        Me.UseZopfli.Location = New System.Drawing.Point(115, 20)
        Me.UseZopfli.Name = "UseZopfli"
        Me.UseZopfli.Size = New System.Drawing.Size(88, 17)
        Me.UseZopfli.TabIndex = 1
        Me.UseZopfli.Text = "Zopfli"
        Me.ToolTip1.SetToolTip(Me.UseZopfli, "Enable trials with Zopfli for deflate compression." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Significantly slower, uses more memory, marginally smaller files.)")
        Me.UseZopfli.UseVisualStyleBackColor = True
        '
        'Use7zDeflate
        '
        Me.Use7zDeflate.AutoSize = True
        Me.Use7zDeflate.Location = New System.Drawing.Point(225, 20)
        Me.Use7zDeflate.Name = "Use7zDeflate"
        Me.Use7zDeflate.Size = New System.Drawing.Size(108, 17)
        Me.Use7zDeflate.TabIndex = 2
        Me.Use7zDeflate.Text = "7zdeflate"
        Me.ToolTip1.SetToolTip(Me.Use7zDeflate, "Enable trials with 7-zip's deflate implementation." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Slow, but often produces smaller deflate output than zlib.)")
        Me.Use7zDeflate.UseVisualStyleBackColor = True
        '
        'UseLz4
        '
        Me.UseLz4.AutoSize = True
        Me.UseLz4.Location = New System.Drawing.Point(10, 43)
        Me.UseLz4.Name = "UseLz4"
        Me.UseLz4.Size = New System.Drawing.Size(81, 17)
        Me.UseLz4.TabIndex = 3
        Me.UseLz4.Text = "LZ4"
        Me.ToolTip1.SetToolTip(Me.UseLz4, "Trial both default/fast LZ4 and LZ4 High Compression level 16, then keep whichever result is smallest." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(CSO v2 and ZSO only.)")
        Me.UseLz4.UseVisualStyleBackColor = True
        '
        'UseLz4Brute
        '
        Me.UseLz4Brute.AutoSize = True
        Me.UseLz4Brute.Location = New System.Drawing.Point(115, 43)
        Me.UseLz4Brute.Name = "UseLz4Brute"
        Me.UseLz4Brute.Size = New System.Drawing.Size(104, 17)
        Me.UseLz4Brute.TabIndex = 4
        Me.UseLz4Brute.Text = "LZ4 Expanded"
        Me.ToolTip1.SetToolTip(Me.UseLz4Brute, "AKA ""LZ4 Brute"". Add LZ4 High Compression levels 4, 7, 10, and 13 to the trial pool, in case any beat" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "the default/fast trial and High Compression level 16 trial from plain LZ4, then keep the smallest result." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Much slower than plain LZ4; requires LZ4 to be enabled.)")
        Me.UseLz4Brute.UseVisualStyleBackColor = True
        '
        'UseLibdeflate
        '
        Me.UseLibdeflate.AutoSize = True
        Me.UseLibdeflate.Location = New System.Drawing.Point(225, 43)
        Me.UseLibdeflate.Name = "UseLibdeflate"
        Me.UseLibdeflate.Size = New System.Drawing.Size(105, 17)
        Me.UseLibdeflate.TabIndex = 5
        Me.UseLibdeflate.Text = "libdeflate"
        Me.ToolTip1.SetToolTip(Me.UseLibdeflate, "Enable trials with libdeflate compression." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Modern, faster deflate alternative; can produce smaller results in some cases.)")
        Me.UseLibdeflate.UseVisualStyleBackColor = True
        '
        'PoolDivider
        '
        Me.PoolDivider.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PoolDivider.Location = New System.Drawing.Point(10, 66)
        Me.PoolDivider.Name = "PoolDivider"
        Me.PoolDivider.Size = New System.Drawing.Size(434, 2)
        Me.PoolDivider.TabIndex = 6
        '
        'OrigCost
        '
        Me.OrigCost.AutoSize = False
        Me.OrigCost.Location = New System.Drawing.Point(10, 73)
        Me.OrigCost.Name = "OrigCost"
        Me.OrigCost.Size = New System.Drawing.Size(200, 17)
        Me.OrigCost.TabIndex = 7
        Me.OrigCost.Text = "Uncompressed Size Tolerance %"
        Me.ToolTip1.SetToolTip(Me.OrigCost, "Allow a block to stay uncompressed if it is only up to this percent larger than the smallest compressed result." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Trades a little file size for faster reads/decompression.)")
        Me.OrigCost.UseVisualStyleBackColor = True
        '
        'OrigCostText
        '
        Me.OrigCostText.Enabled = False
        Me.OrigCostText.Location = New System.Drawing.Point(212, 71)
        Me.OrigCostText.Name = "OrigCostText"
        Me.OrigCostText.Size = New System.Drawing.Size(30, 20)
        Me.OrigCostText.TabIndex = 8
        Me.OrigCostText.Text = "0"
        '
        'Lz4Cost
        '
        Me.Lz4Cost.AutoSize = False
        Me.Lz4Cost.Location = New System.Drawing.Point(282, 73)
        Me.Lz4Cost.Name = "Lz4Cost"
        Me.Lz4Cost.Size = New System.Drawing.Size(130, 17)
        Me.Lz4Cost.TabIndex = 9
        Me.Lz4Cost.Text = "LZ4 Size Tolerance %"
        Me.ToolTip1.SetToolTip(Me.Lz4Cost, "Allow an LZ4-compressed block to be up to this percent larger than the smallest result" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "from the other checked algorithms, and still pick LZ4 — since LZ4 decompresses much faster." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "(Trades a little file size for faster reads/decompression. CSO v2 only.)")
        Me.Lz4Cost.UseVisualStyleBackColor = True
        '
        'Lz4CostText
        '
        Me.Lz4CostText.Enabled = False
        Me.Lz4CostText.Location = New System.Drawing.Point(414, 71)
        Me.Lz4CostText.Name = "Lz4CostText"
        Me.Lz4CostText.Size = New System.Drawing.Size(30, 20)
        Me.Lz4CostText.TabIndex = 10
        Me.Lz4CostText.Text = "0"
        '
        'PoolHelpLabel
        '
        Me.PoolHelpLabel.AutoSize = True
        Me.PoolHelpLabel.Text = "(?)"
        Me.PoolHelpLabel.ForeColor = System.Drawing.Color.Blue
        Me.PoolHelpLabel.Location = New System.Drawing.Point(447, 101)
        Me.PoolHelpLabel.Name = "PoolHelpLabel"
        Me.PoolHelpLabel.TabIndex = 11
        Me.ToolTip1.SetToolTip(Me.PoolHelpLabel, "For each block of the ISO, maxcso compresses it with every enabled algorithm below and keeps whichever result is smallest." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) &
            "Blocks are evaluated independently — each ends up stored with whichever algorithm worked best on that specific data.")
        '
        'MeasureOnly
        '
        Me.MeasureOnly.AutoSize = True
        Me.MeasureOnly.Location = New System.Drawing.Point(240, 71)
        Me.MeasureOnly.Name = "MeasureOnly"
        Me.MeasureOnly.Size = New System.Drawing.Size(90, 17)
        Me.MeasureOnly.TabIndex = 16
        Me.MeasureOnly.Text = "Measure Only"
        Me.ToolTip1.SetToolTip(Me.MeasureOnly, "Measure compressed size without saving output.")
        Me.MeasureOnly.UseVisualStyleBackColor = True
        '
        'CrcOnly
        '
        Me.CrcOnly.AutoSize = True
        Me.CrcOnly.Location = New System.Drawing.Point(152, 71)
        Me.CrcOnly.Name = "CrcOnly"
        Me.CrcOnly.Size = New System.Drawing.Size(82, 17)
        Me.CrcOnly.TabIndex = 15
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
        Me.FileList.Location = New System.Drawing.Point(12, 6)
        Me.FileList.Name = "FileList"
        Me.FileList.Size = New System.Drawing.Size(360, 239)
        Me.FileList.TabIndex = 4
        Me.FileList.Tag = ""
        '
        'DropHelp
        '
        Me.DropHelp.Location = New System.Drawing.Point(379, 202)
        Me.DropHelp.Name = "DropHelp"
        Me.DropHelp.Size = New System.Drawing.Size(454, 28)
        Me.DropHelp.TabIndex = 5
        Me.DropHelp.Text = "Drag and drop ISO files to this box. The output files will be produced in the same " &
    "directory as the ISO unless the Custom Output Dir box is checked."
        Me.DropHelp.UseCompatibleTextRendering = True
        '
        'ProgressText
        '
        Me.ProgressText.Location = New System.Drawing.Point(379, 232)
        Me.ProgressText.Name = "ProgressText"
        Me.ProgressText.Size = New System.Drawing.Size(454, 13)
        Me.ProgressText.TabIndex = 7
        Me.ProgressText.Text = "Ready"
        '
        'ConversionProgress
        '
        Me.ConversionProgress.Location = New System.Drawing.Point(379, 249)
        Me.ConversionProgress.Name = "ConversionProgress"
        Me.ConversionProgress.Size = New System.Drawing.Size(219, 15)
        Me.ConversionProgress.TabIndex = 8
        '
        'Convert
        '
        Me.Convert.Location = New System.Drawing.Point(602, 245)
        Me.Convert.Name = "Convert"
        Me.Convert.Size = New System.Drawing.Size(75, 23)
        Me.Convert.TabIndex = 3
        Me.Convert.Text = "Convert"
        Me.Convert.UseVisualStyleBackColor = True
        '
        'ProgressBytes
        '
        Me.ProgressBytes.Location = New System.Drawing.Point(681, 248)
        Me.ProgressBytes.Name = "ProgressBytes"
        Me.ProgressBytes.Size = New System.Drawing.Size(110, 15)
        Me.ProgressBytes.TabIndex = 10
        Me.ProgressBytes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProgressPercent
        '
        Me.ProgressPercent.Location = New System.Drawing.Point(795, 248)
        Me.ProgressPercent.Name = "ProgressPercent"
        Me.ProgressPercent.Size = New System.Drawing.Size(40, 15)
        Me.ProgressPercent.TabIndex = 9
        Me.ProgressPercent.TextAlign = System.Drawing.ContentAlignment.MiddleRight
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
        Me.ClientSize = New System.Drawing.Size(845, 277)
        Me.Controls.Add(Me.FileList)
        Me.Controls.Add(Me.ProgressBytes)
        Me.Controls.Add(Me.ProgressPercent)
        Me.Controls.Add(Me.ConversionProgress)
        Me.Controls.Add(Me.ProgressText)
        Me.Controls.Add(Me.PoolHelpLabel)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.DropHelp)
        Me.Controls.Add(Me.Convert)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.About)
        Me.AllowDrop = True
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
    Friend WithEvents ModeSelection As ComboBox
    Friend WithEvents FileList As DragDropListBox
    Friend WithEvents DropHelp As Label
    Friend WithEvents ProgressText As Label
    Friend WithEvents ConversionProgress As ProgressBar
    Friend WithEvents Convert As Button
    Friend WithEvents ProgressPercent As Label
    Friend WithEvents ProgressBytes As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents UseZlib As CheckBox
    Friend WithEvents UseZopfli As CheckBox
    Friend WithEvents Use7zDeflate As CheckBox
    Friend WithEvents UseLz4 As CheckBox
    Friend WithEvents UseLz4Brute As CheckBox
    Friend WithEvents UseLibdeflate As CheckBox
    Friend WithEvents PoolDivider As Label
    Friend WithEvents Lz4CostText As TextBox
    Friend WithEvents Lz4Cost As CheckBox
    Friend WithEvents OrigCostText As TextBox
    Friend WithEvents OrigCost As CheckBox
    Friend WithEvents MeasureOnly As CheckBox
    Friend WithEvents CrcOnly As CheckBox
    Friend WithEvents Fast As CheckBox
    Friend WithEvents FormatSelection As ComboBox
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents DeleteCheck As CheckBox
    Friend WithEvents BlockText As TextBox
    Friend WithEvents BlockSize As CheckBox
    Friend WithEvents CustDir As CheckBox
    Friend WithEvents Browse As Button
    Friend WithEvents FolderBrowserDialog1 As FolderBrowserDialog
    Friend WithEvents CustOut As TextBox
    Friend WithEvents PoolHelpLabel As Label
End Class
