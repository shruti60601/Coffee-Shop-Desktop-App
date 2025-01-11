Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar

Public Class FormMasterMenu
    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        ComboBox1.Items.Clear()
        ComboBox1.Text = ""
        TextBox1.Enabled = False
        TextBox2.Enabled = False
        TextBox3.Enabled = False
        TextBox4.Enabled = False
        ComboBox1.Enabled = False

        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
        Button1.Text = "Input"
        Button2.Text = "Edit"
        Button3.Text = "Hapus"
        Button4.Text = "Tutup"
        Call Koneksi()
        Da = New SqlDataAdapter("Select KodeMenu, NamaMenu, HargaMenu, JumlahMenu, StatusMenu From tbl_menuproduk", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "tbl_menuproduk")
        DataGridView1.DataSource = Ds.Tables("tbl_menuproduk")
        DataGridView1.ReadOnly = True
    End Sub
    Sub SiapIsi()
        TextBox1.Enabled = True
        TextBox2.Enabled = True
        TextBox3.Enabled = True
        TextBox4.Enabled = True
        ComboBox1.Enabled = True
        ComboBox1.Items.Add("Ada")
        ComboBox1.Items.Add("Habis")

    End Sub
    Private Sub FormMasterMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "Input" Then
            Button1.Text = "Simpan"
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Text = "Batal"
            Call SiapIsi()
            Call NomorOtomatis()
            TextBox1.Enabled = False
            TextBox2.Focus()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Or ComboBox1.Text = "" Then
                MsgBox("Silahkan Isi Semua Field")
            Else
                Call Koneksi()
                Dim InputData As String = "insert into tbl_menuproduk values('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & TextBox4.Text & "','" & ComboBox1.Text & "')"
                Cmd = New SqlCommand(InputData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Input Data Berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Button2.Text = "Edit" Then
            Button2.Text = "Simpan"
            Button1.Enabled = False
            Button3.Enabled = False
            Button4.Text = "Batal"
            Call SiapIsi()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Or ComboBox1.Text = "" Then
                MsgBox("Silahkan Isi Semua Field")
            Else
                Call Koneksi()
                Dim UpdateData As String = "Update tbl_menuproduk set namamenu='" & TextBox2.Text & "',hargamenu='" & TextBox3.Text & "',jumlahmenu='" & TextBox4.Text & "',statusmenu='" & ComboBox1.Text & "' where kodemenu='" & TextBox1.Text & "'"
                Cmd = New SqlCommand(UpdateData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Update Data Berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub

    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled = True
        If e.KeyChar = Chr(13) Then
            TextBox4.Focus()
        End If
    End Sub

    Private Sub TextBox4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox4.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then e.Handled = True
        If e.KeyChar = Chr(13) Then
            ComboBox1.Focus()
        End If
    End Sub
    Sub NomorOtomatis()
        Call Koneksi()
        Cmd = New SqlCommand("Select * from tbl_menuproduk where kodemenu in (select max(kodemenu) from tbl_menuproduk)", Conn)
        Dim UrutanKode As String
        Dim Hitung As Long
        Rd = Cmd.ExecuteReader
        Rd.Read()
        If Not Rd.HasRows Then
            UrutanKode = "MN" + "001"
        Else
            Hitung = Microsoft.VisualBasic.Right(Rd.GetString(0), 3) + 1
            UrutanKode = "MN" + Microsoft.VisualBasic.Right("000" & Hitung, 3)
        End If
        TextBox1.Text = UrutanKode
    End Sub

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            Call Koneksi()
            Cmd = New SqlCommand("Select * from tbl_menuproduk where kodemenu='" & TextBox1.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            Rd.Read()
            If Not Rd.HasRows Then
                MsgBox("Kode Menu Tidak Ada")
            Else
                TextBox1.Text = Rd.Item("KodeMenu")
                TextBox2.Text = Rd.Item("NamaMenu")
                TextBox3.Text = Rd.Item("HargaMenu")
                TextBox4.Text = Rd.Item("JumlahMenu")
                ComboBox1.Text = Rd.Item("StatusMenu")
            End If
        End If
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Button4.Text = "Tutup" Then
            Me.Close()
        Else
            Call KondisiAwal()
        End If
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If Button3.Text = "Hapus" Then
            Button3.Text = "Delete"
            Button1.Enabled = False
            Button2.Enabled = False
            Button4.Text = "Batal"
            Call SiapIsi()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Or ComboBox1.Text = "" Then
                MsgBox("Silahkan Isi Semua Field")
            Else
                Call Koneksi()
                Dim HapusData As String = "Delete from tbl_menuproduk where kodemenu='" & TextBox1.Text & "'"
                Cmd = New SqlCommand(HapusData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Hapus Data Berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged

    End Sub
    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox3.Focus()
        End If
    End Sub
End Class