Imports System.Data.SqlClient
Public Class FormLogin
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Hide()
    End Sub
    Sub Terbuka()
        FormMenuUtama.LoginToolStripMenuItem.Enabled = False
        FormMenuUtama.LogoutToolStripMenuItem.Enabled = True
        FormMenuUtama.MasterToolStripMenuItem.Enabled = True
        FormMenuUtama.TransaksiToolStripMenuItem.Enabled = True
        FormMenuUtama.LaporanToolStripMenuItem.Enabled = True
    End Sub
    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox1.Focus()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Or TextBox2.Text = "" Then
            MsgBox("KodeAdmin dan Password Tidak Boleh Kosong!")
        Else
            Call Koneksi()
            Cmd = New SqlCommand("Select * From tbl_admin where kodeadmin='" & TextBox1.Text & "' and passadmin = '" & TextBox2.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            Rd.Read()
            If Rd.HasRows Then
                Me.Close()
                Call Terbuka()
                FormMenuUtama.STLabel2.Text = Rd!KodeAdmin
                FormMenuUtama.STLabel4.Text = Rd!NamaAdmin
                FormMenuUtama.STLabel6.Text = Rd!LevelAdmin
                If FormMenuUtama.STLabel6.Text = "KASIR" Then
                    FormMenuUtama.MasterToolStripMenuItem.Enabled = False
                    FormMenuUtama.LaporanToolStripMenuItem.Enabled = False
                End If
            Else
                MsgBox("KodeAdmin atau PassAdmin Salah!")
            End If
        End If
    End Sub

    Private Sub FormLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox2.Focus()
        End If
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = Chr(13) Then
            Button1.Focus()
        End If
    End Sub
End Class