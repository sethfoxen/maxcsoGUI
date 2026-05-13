Imports System.Drawing
Imports System.Windows.Forms

Friend Module AppIconHelper
    Private _cachedApplicationIcon As Icon = Nothing

    Friend Function GetApplicationIcon() As Icon
        If _cachedApplicationIcon Is Nothing Then
            Try
                Using extractedIcon As Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath)
                    If extractedIcon IsNot Nothing Then
                        _cachedApplicationIcon = CType(extractedIcon.Clone(), Icon)
                    End If
                End Using
            Catch
            End Try
        End If

        If _cachedApplicationIcon Is Nothing Then
            Return Nothing
        End If

        Return CType(_cachedApplicationIcon.Clone(), Icon)
    End Function
End Module
