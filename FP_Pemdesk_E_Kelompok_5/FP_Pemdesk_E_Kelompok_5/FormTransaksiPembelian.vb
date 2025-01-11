Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class FormTransaksiPembelian
    Dim TglSQL As String
    Dim JamSQL As String
    Sub KondisiAwal()
        Label6.Text = ""
        Label7.Text = ""
        Label10.Text = Today
        Label13.Text = FormMenuUtama.STLabel4.Text
        Label15.Text = ""
        Label18.Text = ""
        Label19.Text = ""
        Label21.Text = ""
        Label23.Text = ""
        Label25.Text = ""
        Label30.Text = ""
        TextBox3.Text = ""
        TextBox2.Text = ""
        ComboBox1.Text = ""
        TextBox2.Enabled = False
        Call NomorOtomatis()
        Call BuatKolom()
        Call MunculKodeSupplier()
        Label15.Text = ""
        ComboBox1.Focus()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label11.Text = TimeOfDay
    End Sub

    Private Sub FormTransaksiPembelian_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub
    Sub MunculKodeSupplier()
        Call Koneksi()
        Cmd = New SqlCommand("Select * from tbl_supplier", Conn)
        Rd = Cmd.ExecuteReader
        Do While Rd.Read
            ComboBox1.Items.Add(Rd.Item(0))
        Loop
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Call Koneksi()
        Cmd = New SqlCommand("Select * from tbl_supplier where kodesupplier ='" & ComboBox1.Text & "'", Conn)
        Rd = Cmd.ExecuteReader
        Rd.Read()
        If Rd.HasRows Then
            Label6.Text = Rd!NamaSupplier
            Label7.Text = Rd!AlamatSupplier
            Label21.Text = Rd!NoTelpSupplier
            Label23.Text = Rd!JenisSupplier
            Label25.Text = Rd!HargaSupplier
            TextBox2.Enabled = True
        End If
    End Sub
    Private Sub Button3_Click(sender As Object, e As EventArgs)
        If Button3.Text = "Tutup" Then
            Me.Close()
        Else
            Call KondisiAwal()
        End If
    End Sub
    Sub NomorOtomatis()
        Call Koneksi()
        Cmd = New SqlCommand("Select * from tbl_pembelian where nobeli in (select max(nobeli) from tbl_pembelian)", Conn)
        Dim UrutanKode As String
        Dim Hitung As Long
        Rd = Cmd.ExecuteReader
        Rd.Read()
        If Not Rd.HasRows Then
            UrutanKode = "B" + Format(Now, "yyMMdd") + "001"
        Else
            Hitung = Microsoft.VisualBasic.Right(Rd.GetString(0), 9) + 1
            UrutanKode = "B" + Format(Now, "yyMMdd") + Microsoft.VisualBasic.Right("000" & Hitung, 3)
        End If
        Label4.Text = UrutanKode
    End Sub
    Sub BuatKolom()
        DataGridView1.Columns.Clear()
        DataGridView1.Columns.Add("Nama", "Nama Bahan")
        DataGridView1.Columns.Add("Harga", "Harga")
        DataGridView1.Columns.Add("Jumlah", "Jumlah")
        DataGridView1.Columns.Add("Subtotal", "Subtotal")
        DataGridView1.Columns.Add("Kode", "Kode Menu")
    End Sub
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ComboBox1.Text = "" Or Label6.Text = "" Or TextBox2.Text = "" Then
            MsgBox("Silahkan masukkan Kode Supplier dan Tekan ENTER!")
        Else
            DataGridView1.Rows.Add(New String() {Label23.Text, Label25.Text, TextBox2.Text, Val(Label25.Text) * Val(TextBox2.Text), TextBox4.Text})
            Call RumusSubTotal()
            'ComboBox1.Text = ""
            Label6.Text = ""
            Label7.Text = ""
            Label21.Text = ""
            Label23.Text = ""
            Label25.Text = ""
            Label30.Text = ""
            TextBox2.Text = ""
            TextBox4.Text = ""
            TextBox2.Enabled = False
            Call RumusCariItem()
        End If
    End Sub
    Sub RumusSubTotal()
        Dim Hitung As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            Hitung = Hitung + DataGridView1.Rows(i).Cells(3).Value
            Label15.Text = Hitung
        Next
    End Sub
    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If e.KeyChar = Chr(13) Then
            If Val(TextBox3.Text) < Val(Label15.Text) Then
                MsgBox("Pembayaran Kurang!")
            ElseIf Val(TextBox3.Text) = Val(Label15.Text) Then
                Label18.Text = 0
            ElseIf Val(TextBox3.Text) > Val(Label15.Text) Then
                Label18.Text = Val(TextBox3.Text) - Val(Label15.Text)
                Button1.Focus()
            End If
        End If
    End Sub
    Sub RumusCariItem()
        Dim HitungItem As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            HitungItem = HitungItem + DataGridView1.Rows(i).Cells(3).Value
            Label19.Text = HitungItem
        Next
    End Sub
    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Label18.Text = "" Or Label19.Text = "" Then
            MsgBox("Transaksi tidak ada, silahkan lakukan transaksi terlebih dahulu")
        Else
            Call Koneksi()
            TglSQL = Format(Today, "yyyy-MM-dd")
            JamSQL = Format(TimeOfDay)
            Dim SimpanBeli As String = "insert into tbl_pembelian values ('" & Label4.Text & "','" & TglSQL & "','" & JamSQL & "','" & Label19.Text & "','" & Label15.Text & "','" & TextBox3.Text & "','" & Label18.Text & "','" & ComboBox1.Text & "','" & FormMenuUtama.STLabel2.Text & "')"
            Cmd = New SqlCommand(SimpanBeli, Conn)
            Cmd.ExecuteNonQuery()

            For Baris As Integer = 0 To DataGridView1.Rows.Count - 2
                Dim SimpanDetail As String = "insert into tbl_detailpembelian values ('" & Label4.Text & "','" & DataGridView1.Rows(Baris).Cells(0).Value & "','" & DataGridView1.Rows(Baris).Cells(1).Value & "','" & DataGridView1.Rows(Baris).Cells(2).Value & "','" & DataGridView1.Rows(Baris).Cells(3).Value & "','" & DataGridView1.Rows(Baris).Cells(4).Value & "')"
                Cmd = New SqlCommand(SimpanDetail, Conn)
                Cmd.ExecuteNonQuery()

                Cmd = New SqlCommand("select * from tbl_menuproduk where kodemenu='" & DataGridView1.Rows(Baris).Cells(4).Value & "'", Conn)
                Rd = Cmd.ExecuteReader
                Rd.Read()
                Dim TambahiStok As String = "Update tbl_menuproduk set jumlahmenu ='" & Rd.Item("JumlahMenu") + CInt(DataGridView1.Rows(Baris).Cells(2).Value * 100) & "' where kodemenu='" & DataGridView1.Rows(Baris).Cells(4).Value & "'"
                Cmd = New SqlCommand(TambahiStok, Conn)
                Rd.Close()
                Cmd.ExecuteNonQuery()
            Next
            Call KondisiAwal()
            MsgBox("Simpan Data Berhasil")
        End If
    End Sub

    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = Chr(13) Then
            Label30.Text = TextBox2.Text * 40
            TextBox4.Focus()
        End If
    End Sub
    Private Sub TextBox4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox4.KeyPress
        If e.KeyChar = Chr(13) Then
            Button4.Focus()
            Call Koneksi()
            Cmd = New SqlCommand("Select * from tbl_menuproduk where kodemenu='" & TextBox4.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            Rd.Read()
            If Not Rd.HasRows Then
                MsgBox("Kode Menu Tidak Ada")
            Else
                TextBox4.Text = Rd.Item("KodeMenu")
            End If
        End If
    End Sub
End Class