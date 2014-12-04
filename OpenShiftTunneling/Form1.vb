Imports Renci
Public Class Form1

    Dim SSHTunnel As SshNet.SshClient
    Dim portforward As SshNet.ForwardedPortLocal
    Dim dir As String

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim client = New SshNet.SshClient(ipText.Text, 22, usernameText.Text, pass.Text)
        appendLine("Connecting SSH")
        client.Connect()
        appendLine("Connected SSH")
        Dim list = client.RunCommand("ls -la")
        appendLine(list.Result)
    End Sub

    Sub appendLine(line As String)
        If RichTextBox1.Text.Length = 0 Then
            RichTextBox1.Text = line
        Else
            RichTextBox1.Text = RichTextBox1.Text & vbNewLine & line
        End If
    End Sub

    Function connectSSH() As SshNet.SshClient
        Dim client = New SshNet.SshClient("192.168.1.15", 22, "slem", "******")
        client.Connect()
        Return client
    End Function

    Sub createTunnel(app As String)
        Try
            SSHTunnel = New SshNet.SshClient(ipText.Text, 22, usernameText.Text, pass.Text)
            appendLine("Connecting SSH")
            SSHTunnel.Connect()
            appendLine("Connected SSH")
            portforward = New SshNet.ForwardedPortLocal("127.0.0.1", 2121, app, 22)
            SSHTunnel.AddForwardedPort(portforward)
            If SSHTunnel.IsConnected Then
                appendLine("Starting Port forwarding")
                portforward.Start()
                appendLine("Started Port forwarding")
                If portforward.IsStarted Then
                    appendLine("Tunneling started")
                End If
            End If
        Catch ex As Exception
            appendLine(ex.Message)
        End Try
    End Sub

    Function check()
        Dim ok = True

        If usernameText.TextLength = 0 Then
            ok = False
        End If

        If pass.TextLength = 0 Then
            ok = False
        End If
        If ipText.TextLength = 0 Then
            ok = False
        End If
        If OSsc.TextLength = 0 Then
            ok = False
        End If

        If ok = False Then
            MessageBox.Show("Ningun campo puede estar vacio")
        End If


        Return ok
    End Function

    Sub RunCommandCom(command As String, arguments As String, permanent As Boolean)
        Dim p As Process = New Process()
        Dim pi As ProcessStartInfo = New ProcessStartInfo()
        pi.Arguments = " " + If(permanent = True, "/K", "/C") + " " + command + " " + arguments
        pi.FileName = "cmd.exe"
        p.StartInfo = pi
        p.Start()
    End Sub


    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim words = OSsc.Text.Split("/")
        Dim sshData = words(2).Split("@")
        Dim user = sshData(0)
        Dim host = sshData(1)

        If check() Then
            createTunnel(host)
            RunCommandCom("git", "clone ssh://" & user & "@127.0.0.1:2121/~/git/app.git/ " & DirLabel.Text, True)
        End If
        
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim folder = FolderBrowserDialog1.ShowDialog
        If folder.Equals(DialogResult.OK) Then
            DirLabel.Text = FolderBrowserDialog1.SelectedPath.ToString
        End If
    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs)

    End Sub
End Class
