﻿Imports System.IO
Imports System.Threading
Imports System.Net
Imports System.Management
Imports System.Diagnostics

Public Class Form3
    Dim clockon As Integer
    Dim online As Integer
    Dim check As Integer

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim address As String = "https://pastebin.com/raw/2x4r8VTS" ' Make this your changes.txt
        Dim client As WebClient = New WebClient()
        Dim reader As StreamReader = New StreamReader(client.OpenRead(address))
        RichTextBox1.Text = reader.ReadToEnd
        RichTextBox1.Multiline = True
        RichTextBox1.ReadOnly = True

        'Generate HWID
        Dim hw As New clsComputerInfo

        Dim cpu As String
        Dim mb As String
        Dim mac As String
        Dim hwid As String

        cpu = hw.GetProcessorId()
        mb = hw.GetMotherboardID()
        mac = hw.GetMACAddress()
        hwid = cpu + mb + mac

        Dim hwidEncrypted As String = Strings.UCase(hw.getMD5Hash(cpu & mb & mac))

        txtHWID.Text = hwidEncrypted
        'HWID Generated

        Label1.Text = "User Status: Fetching"
        lblNetwork.Text = "Fetching status"
        Timer2.Interval = 1500
        online = 2

        Label2.Text = "Loader Version: 1.0.0.0"
        clockon = 1
        Timer2.Start()
        Timer3.Start()
        If (clockon = 1) Then
            Timer1.Start()
        Else
            Timer1.Stop()
        End If
    End Sub

    Private Class clsComputerInfo
        'Get processor ID
        Friend Function GetProcessorId() As String
            Dim strProcessorID As String = String.Empty
            Dim query As New SelectQuery("Win32_processor")
            Dim search As New ManagementObjectSearcher(query)
            Dim info As ManagementObject
            For Each info In search.Get()
                strProcessorID = info("processorID").ToString()
            Next
            Return strProcessorID
        End Function
        ' Get MAC Address
        Friend Function GetMACAddress() As String
            Dim mc As ManagementClass = New ManagementClass("Win32_NetworkAdapterConfiguration")
            Dim moc As ManagementObjectCollection = mc.GetInstances()
            Dim MacAddress As String = String.Empty
            For Each mo As ManagementObject In moc
                If (MacAddress.Equals(String.Empty)) Then
                    If CBool(mo("IPEnabled")) Then MacAddress = mo("MacAddress").ToString()
                    mo.Dispose()
                End If
                MacAddress = MacAddress.Replace(":", String.Empty)
            Next
            Return MacAddress
        End Function
        ' Get Motherboard ID
        Friend Function GetMotherboardID() As String
            Dim strMotherboardID As String = String.Empty
            Dim query As New SelectQuery("Win32_BaseBoard")
            Dim search As New ManagementObjectSearcher(query)
            Dim info As ManagementObject
            For Each info In search.Get()
                strMotherboardID = info("product").ToString()
            Next
            Return strMotherboardID
        End Function
        ' Encrypt HWID
        Friend Function getMD5Hash(ByVal strToHash As String) As String
            Dim md5Obj As New System.Security.Cryptography.MD5CryptoServiceProvider
            Dim bytesToHash() As Byte = System.Text.Encoding.ASCII.GetBytes(strToHash)
            bytesToHash = md5Obj.ComputeHash(bytesToHash)
            Dim strResult As String = ""
            For Each b As Byte In bytesToHash
                strResult += b.ToString("x2")
            Next
            Return strResult
        End Function
    End Class

    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Application.Exit()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs)
        clockon = 0
    End Sub

    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Application.Restart()
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Application.Exit()
    End Sub

    Private Sub Label4_Click(sender As Object, e As EventArgs)
        Dim webAddress As String = "http://artificialaim.net/forums"
        Process.Start(webAddress)
    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs)
        Dim webAddress As String = "http://artificialaim.net/forums/usercp.php"
        Process.Start(webAddress)
    End Sub

    Private Sub Label6_Click(sender As Object, e As EventArgs)
        Dim webAddress As String = "http://artificialaim.net/forums/private.php"
        Process.Start(webAddress)
    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs)

        If (online = 1) Then
            MsgBox("Work in progress", vbCritical)
        Else
            MsgBox("Cheat is offline", vbCritical)
        End If
    End Sub

    Private Sub Label10_Click(sender As Object, e As EventArgs)
        If (online = 1) Then
        Else
            MsgBox("Cheat is offline", vbCritical)
        End If
    End Sub

    Private Sub Label11_Click(sender As Object, e As EventArgs)
        If (online = 1) Then
            Form4.Show()
            Me.Close()
        Else
            MsgBox("Cheat is offline", vbCritical)
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Button5.PerformClick()

        Timer2.Interval = 10000
        Try
            My.Computer.Network.Ping("www.google.com")

            Dim WC As New System.Net.WebClient
            Try
                Dim http3 As String = WC.DownloadString("http://artificialaim.net/loader/status.txt")
                If http3.Contains("1") Then
                    online = 1
                ElseIf http3.Contains("0") Then
                    online = 0
                ElseIf http3.Contains("2") Then
                    online = 3
                End If
            Catch
                online = 0
            End Try

        Catch
            online = 0
        End Try

        If (online = 1) Then
            lblNetwork.ForeColor = Color.Green
            lblNetwork.Text = "Cheat is online!"
        ElseIf (online = 0) Then
            lblNetwork.ForeColor = Color.Red
            lblNetwork.Text = "Cheat is offline!"
        ElseIf (online = 3) Then
            lblNetwork.ForeColor = Color.Orange
            lblNetwork.Text = "Down for matinence"
            lblNetwork.Location = New Point(50, 74)
        End If
    End Sub

    Private Sub lblNetwork_Click(sender As Object, e As EventArgs) Handles lblNetwork.Click
        If (online = 2) Then
            MsgBox("The cheat is currently checking it's connection to a few resources! Please be patient as this can take some time!")
        ElseIf (online = 1) Then
            MsgBox("The cheat is online and should be currently working!")
        ElseIf (online = 3) Then
            MsgBox("The cheat is down for matinence! Please be patient in this current time")
        ElseIf (online = 0) Then
            MsgBox("The cheat is offline!" + Environment.NewLine + "Possible causes: No internet connection, website is down, developer marked the cheat to be offline for matenience")
        End If
    End Sub

    Private Sub WebBrowser1_DocumentCompleted(sender As Object, e As WebBrowserDocumentCompletedEventArgs) Handles WebBrowser1.DocumentCompleted ' cheatz rank user status
        Label1.Text = "User Status: Fetching"



        If WebBrowser1.DocumentText.Contains("4,") Then
            Label1.Text = "User Status: Administrator"

        ElseIf WebBrowser1.DocumentText.Contains("5,") Then
            Label1.Text = "User Status: How are you here??"

        ElseIf WebBrowser1.DocumentText.Contains("8,") Then
            Label1.Text = "User Status: VIP"

        ElseIf WebBrowser1.DocumentText.Contains("9,") Then
            Label1.Text = "User Status: Developer"

        ElseIf WebBrowser1.DocumentText.Contains("11,") Then
            Label1.Text = "User Status: Premium Garry's Mod"

        ElseIf WebBrowser1.DocumentText.Contains("2,") Then
            Label1.Text = "User Status: Registered"
            ListBox1.Items.Remove("Custom Injector")

        ElseIf WebBrowser1.DocumentText.Contains("7,") Then
            Timer2.Stop()
            Label1.Text = "User Status: Banned"
            Dim pProcess() As Process = System.Diagnostics.Process.GetProcessesByName("RadicalHeights", "csgo")
            For Each p As Process In pProcess
                p.Kill()
            Next
            Directory.Delete("C:\temp\Loader\dll", True)
            MsgBox("You have been banned!", vbCritical)
            Application.Exit()
        End If
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click ' cheatz selection
        If (online = 1) Then
            If Me.ListBox1.SelectedItem = "Radical Heights" Then
                'something is selected
                Form5.Show()
                Me.Hide()
            ElseIf Me.ListBox1.SelectedItem = "Custom Injector" Then
                'something is selected
                Form4.Show()
                Me.Hide()
            Else
                'Nothing is
                MsgBox("Error: Please select a cheat")
            End If
        ElseIf (online = 0) Then
            MsgBox("Cheat is offline", vbCritical)
        ElseIf (online = 3) Then
            MsgBox("Cheat is offline", vbCritical)
        End If
    End Sub

    Private Sub Button4_Click_1(sender As Object, e As EventArgs)
        Clipboard.SetText(txtHWID.Text)
        MsgBox("HWID Copied")
    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs) Handles Button5.Click
        WebBrowser1.Navigate("http://artificialaim.net/loader/usercheck_get.php?username=" + My.Settings.username + "&submit=Submit")
    End Sub

    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        ' Removes the .dll if CS:GO is not open. If it is open and it tries to remove it, it will give an error.
        Dim pName As String = "RadicalHeights"
        Dim psList() As Process
        Try
            psList = Process.GetProcesses()
            For Each p As Process In psList
                If (pName = p.ProcessName) Then
                Else
                    Directory.Delete("C:\temp\Loader\dll", True)
                End If
            Next p
        Catch ex As Exception
        End Try
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick ' cheatz info
        If Me.ListBox1.SelectedItem = "Radical Heights" Then
            Dim address As String = "https://pastebin.com/raw/eLc3gBi8"
            Dim client As WebClient = New WebClient()
            Dim reader As StreamReader = New StreamReader(client.OpenRead(address))
            RichTextBox1.Text = reader.ReadToEnd
            RichTextBox1.Multiline = True
            RichTextBox1.ReadOnly = True
        ElseIf Me.ListBox1.SelectedItem = "Custom Injector" Then
            Dim address As String = "https://pastebin.com/raw/FDiBedVt"
            Dim client As WebClient = New WebClient()
            Dim reader As StreamReader = New StreamReader(client.OpenRead(address))
            RichTextBox1.Text = reader.ReadToEnd
            RichTextBox1.Multiline = True
            RichTextBox1.ReadOnly = True
        End If
    End Sub
End Class
'-----------------------------------------------------
' Coded by /id/Thaisen! Free loader source
' https://github.com/ThaisenPM/Cheat-Loader-CSGO
' Note to the person using this, removing this
' text is in violation of the license you agreed
' to by downloading. Only you can see this so what
' does it matter anyways.
' Copyright © ThaisenPM 2017
' Licensed under a MIT license
' Read the terms of the license here
' https://github.com/ThaisenPM/Cheat-Loader-CSGO/blob/master/LICENSE
'-----------------------------------------------------
