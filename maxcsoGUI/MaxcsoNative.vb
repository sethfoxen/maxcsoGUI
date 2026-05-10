Imports System.Runtime.InteropServices
Imports System.Text

Friend Enum NativeBridgeRunResult
    Success
    Failed
    Unavailable
End Enum

Friend Enum NativeFormat
    Cso1 = 0
    Cso2 = 1
    Zso = 2
    Dax = 3
End Enum

<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
Friend Structure NativeBridgeRequest
    <MarshalAs(UnmanagedType.LPWStr)>
    Public InputPath As String
    <MarshalAs(UnmanagedType.LPWStr)>
    Public OutputPath As String
    Public Threads As Integer
    Public Format As NativeFormat
    <MarshalAs(UnmanagedType.Bool)>
    Public Fast As Boolean
    <MarshalAs(UnmanagedType.Bool)>
    Public UseZopfli As Boolean
    <MarshalAs(UnmanagedType.Bool)>
    Public UseLibdeflate As Boolean
    <MarshalAs(UnmanagedType.Bool)>
    Public UseLz4Brute As Boolean
    <MarshalAs(UnmanagedType.Bool)>
    Public Decompress As Boolean
    <MarshalAs(UnmanagedType.Bool)>
    Public CrcOnly As Boolean
    <MarshalAs(UnmanagedType.Bool)>
    Public MeasureOnly As Boolean
    <MarshalAs(UnmanagedType.Bool)>
    Public BlockSizeEnabled As Boolean
    Public BlockSize As UInteger
    <MarshalAs(UnmanagedType.Bool)>
    Public OrigCostEnabled As Boolean
    Public OrigCostPercent As Double
    <MarshalAs(UnmanagedType.Bool)>
    Public Lz4CostEnabled As Boolean
    Public Lz4CostPercent As Double
End Structure

Friend Module MaxcsoNative
    Private Const BridgeLibraryName As String = "maxcsoBridge.dll"
    Private libraryHandle As IntPtr = IntPtr.Zero
    Private bridgeDelegate As MaxcsoBridgeProcessDelegate = Nothing
    Private loadedLibraryPath As String = String.Empty

    <DllImport("kernel32.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Function LoadLibrary(lpFileName As String) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Function FreeLibrary(hLibModule As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("kernel32.dll", CharSet:=CharSet.Ansi, SetLastError:=True)>
    Private Function GetProcAddress(hModule As IntPtr, procName As String) As IntPtr
    End Function

    <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
    Private Delegate Function MaxcsoBridgeProcessDelegate(ByRef request As NativeBridgeRequest, progressCallback As MaxcsoBridgeProgressDelegate, userData As IntPtr, messageBuffer As StringBuilder, messageBufferChars As Integer) As Integer

    <UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet:=CharSet.Unicode)>
    Private Delegate Sub MaxcsoBridgeProgressDelegate(percent As Integer, <MarshalAs(UnmanagedType.LPWStr)> message As String, userData As IntPtr)

    Friend Function ResolveBridgeLibraryPath() As String
        Dim architectureFolder As String = If(IntPtr.Size = 8, "x64", "Win32")
        Dim candidates As New List(Of String) From {
            IO.Path.Combine(Application.StartupPath, BridgeLibraryName),
            IO.Path.Combine(Application.StartupPath, "native", architectureFolder, BridgeLibraryName)
        }

        Dim currentDirectory As IO.DirectoryInfo = New IO.DirectoryInfo(Application.StartupPath)
        Do While currentDirectory IsNot Nothing
            candidates.Add(IO.Path.Combine(currentDirectory.FullName, "native", architectureFolder, BridgeLibraryName))

            Dim binDirectory As String = IO.Path.Combine(currentDirectory.FullName, "bin")
            If IO.Directory.Exists(binDirectory) Then
                Dim buildDirectories As IEnumerable(Of String) =
                    IO.Directory.GetDirectories(binDirectory).
                    OrderBy(Function(path) GetBridgeDirectoryPriority(IO.Path.GetFileName(path))).
                    ThenBy(Function(path) path, StringComparer.OrdinalIgnoreCase)

                For Each buildDirectory As String In buildDirectories
                    candidates.Add(IO.Path.Combine(buildDirectory, "native", architectureFolder, BridgeLibraryName))
                Next
            End If

            currentDirectory = currentDirectory.Parent
        Loop

        For Each candidate In candidates.Distinct(StringComparer.OrdinalIgnoreCase)
            If IO.File.Exists(candidate) Then
                Return candidate
            End If
        Next

        Return String.Empty
    End Function

    Private Function GetBridgeDirectoryPriority(directoryName As String) As Integer
        Select Case directoryName.ToLowerInvariant()
            Case "release_progressfix"
                Return 0
            Case "release"
                Return 1
            Case "debug"
                Return 2
            Case Else
                Return 10
        End Select
    End Function

    Friend Function Run(ByRef request As NativeBridgeRequest, ByRef successDetails As String, ByRef failureDetails As String, Optional progressCallback As Action(Of Integer, String) = Nothing) As NativeBridgeRunResult
        successDetails = String.Empty
        failureDetails = String.Empty

        Dim libraryPath As String = ResolveBridgeLibraryPath()
        If String.IsNullOrWhiteSpace(libraryPath) Then
            Return NativeBridgeRunResult.Unavailable
        End If

        If Not EnsureBridgeLoaded(libraryPath, failureDetails) Then
            Return NativeBridgeRunResult.Unavailable
        End If

        Dim messageBuffer As New StringBuilder(2048)
        Dim nativeProgressCallback As MaxcsoBridgeProgressDelegate = Nothing

        If progressCallback IsNot Nothing Then
            nativeProgressCallback = Sub(percent As Integer, message As String, userData As IntPtr)
                                         progressCallback(percent, If(message, String.Empty))
                                     End Sub
        End If

        Try
            Dim result As Integer = bridgeDelegate.Invoke(request, nativeProgressCallback, IntPtr.Zero, messageBuffer, messageBuffer.Capacity)
            Dim message As String = messageBuffer.ToString().Trim()

            If result = 0 Then
                successDetails = message
                Return NativeBridgeRunResult.Success
            End If

            failureDetails = If(String.IsNullOrWhiteSpace(message), "The native maxcso bridge reported an error.", message)
            Return NativeBridgeRunResult.Failed
        Catch ex As Exception
            failureDetails = ex.Message
            Return NativeBridgeRunResult.Unavailable
        Finally
            GC.KeepAlive(nativeProgressCallback)
        End Try
    End Function

    Private Function EnsureBridgeLoaded(libraryPath As String, ByRef failureDetails As String) As Boolean
        If bridgeDelegate IsNot Nothing AndAlso String.Equals(loadedLibraryPath, libraryPath, StringComparison.OrdinalIgnoreCase) Then
            Return True
        End If

        If libraryHandle <> IntPtr.Zero Then
            FreeLibrary(libraryHandle)
            libraryHandle = IntPtr.Zero
            bridgeDelegate = Nothing
            loadedLibraryPath = String.Empty
        End If

        libraryHandle = LoadLibrary(libraryPath)
        If libraryHandle = IntPtr.Zero Then
            failureDetails = "Could not load " & libraryPath
            Return False
        End If

        Dim processPtr As IntPtr = ResolveBridgeEntryPoint(libraryHandle)
        If processPtr = IntPtr.Zero Then
            failureDetails = "Could not find MaxcsoBridgeProcess in " & libraryPath
            FreeLibrary(libraryHandle)
            libraryHandle = IntPtr.Zero
            Return False
        End If

        bridgeDelegate = CType(Marshal.GetDelegateForFunctionPointer(processPtr, GetType(MaxcsoBridgeProcessDelegate)), MaxcsoBridgeProcessDelegate)
        loadedLibraryPath = libraryPath
        Return True
    End Function

    Private Function ResolveBridgeEntryPoint(moduleHandle As IntPtr) As IntPtr
        Dim entryPoints As New List(Of String) From {
            "MaxcsoBridgeProcess"
        }

        ' x86 stdcall exports are commonly decorated as _Name@bytes.
        If IntPtr.Size = 4 Then
            entryPoints.Add("_MaxcsoBridgeProcess@20")
        End If

        For Each entryPoint In entryPoints
            Dim processPtr As IntPtr = GetProcAddress(moduleHandle, entryPoint)
            If processPtr <> IntPtr.Zero Then
                Return processPtr
            End If
        Next

        Return IntPtr.Zero
    End Function
End Module
