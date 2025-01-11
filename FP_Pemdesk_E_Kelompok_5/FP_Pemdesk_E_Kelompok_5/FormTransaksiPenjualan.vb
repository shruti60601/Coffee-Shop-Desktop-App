Imports System.Data.SqlClient
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class FormTransaksiPenjualan
    Dim TglSQL As String
    Dim JamSQL As String
    Sub KondisiAwal()
        Label6.Text = ""
        Label7.Text = ""
        Label10.Text = Today
        Label13.Text = FormMenuUtama.STLabel4.Text
        Label18.Text = ""
        Label19.Text = ""
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox2.Enabled = False
        Call NomorOtomatis()
        Call BuatKolom()
        Label15.Text = ""
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Label11.Text = TimeOfDay
    End Sub

    Private Sub FormTransaksiPenjualan_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
        TextBox1.Focus()

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
        Cmd = New SqlCommand("Select * from tbl_penjualan where nojual in (select max(nojual) from tbl_penjualan)", Conn)
        Dim UrutanKode As String
        Dim Hitung As Long
        Rd = Cmd.ExecuteReader
        Rd.Read()
        If Not Rd.HasRows Then
            UrutanKode = "J" + Format(Now, "yyMMdd") + "001"
        Else
            Hitung = Microsoft.VisualBasic.Right(Rd.GetString(0), 9) + 1
            UrutanKode = "J" + Format(Now, "yyMMdd") + Microsoft.VisualBasic.Right("000" & Hitung, 3)
        End If
        Label4.Text = UrutanKode
    End Sub
    Sub BuatKolom()
        DataGridView1.Columns.Clear()
        DataGridView1.Columns.Add("Kode", "Kode")
        DataGridView1.Columns.Add("Nama", "Nama Menu")
        DataGridView1.Columns.Add("Harga", "Harga")
        DataGridView1.Columns.Add("Jumlah", "Jumlah")
        DataGridView1.Columns.Add("Subtotal", "Subtotal")
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
                Label6.Text = Rd.Item("NamaMenu")
                Label7.Text = Rd.Item("HargaMenu")
                'TextBox4.Text = Rd.Item("JumlahMenu")
                'ComboBox1.Text = Rd.Item("StatusMenu")
                TextBox2.Enabled = True
                TextBox2.Focus()
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Label6.Text = "" Or TextBox2.Text = "" Then
            MsgBox("Silahkan masukkan Kode Menu dan Tekan ENTER!")
        Else
            DataGridView1.Rows.Add(New String() {TextBox1.Text, Label6.Text, Label7.Text, TextBox2.Text, Val(Label7.Text) * Val(TextBox2.Text)})
            Call RumusSubTotal()
            TextBox1.Text = ""
            Label6.Text = ""
            Label7.Text = ""
            TextBox2.Text = ""
            TextBox2.Enabled = False
            Call RumusCariItem()
            TextBox3.Focus()
        End If
    End Sub
    Sub RumusSubTotal()
        Dim Hitung As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            Hitung = Hitung + DataGridView1.Rows(i).Cells(4).Value
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
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Label18.Text = "" Or Label19.Text = "" Then
            MsgBox("Transaksi tidak ada, silahkan lakukan transaksi terlebih dahulu")
        Else
            Call Koneksi()
            TglSQL = Format(Today, "yyyy-MM-dd")
            JamSQL = Format(TimeOfDay)
            Dim SimpanJual As String = "insert into tbl_penjualan values ('" & Label4.Text & "','" & TglSQL & "','" & JamSQL & "','" & Label19.Text & "','" & Label15.Text & "','" & TextBox3.Text & "','" & Label18.Text & "','" & FormMenuUtama.STLabel2.Text & "')"
            Cmd = New SqlCommand(SimpanJual, Conn)
            Cmd.ExecuteNonQuery()

            For Baris As Integer = 0 To DataGridView1.Rows.Count - 2
                Dim SimpanDetail As String = "insert into tbl_detailpenjualan values ('" & Label4.Text & "','" & DataGridView1.Rows(Baris).Cells(0).Value & "','" & DataGridView1.Rows(Baris).Cells(1).Value & "','" & DataGridView1.Rows(Baris).Cells(2).Value & "','" & DataGridView1.Rows(Baris).Cells(3).Value & "','" & DataGridView1.Rows(Baris).Cells(4).Value & "')"
                Cmd = New SqlCommand(SimpanDetail, Conn)
                Cmd.ExecuteNonQuery()

                Cmd = New SqlCommand("select * from tbl_menuproduk where kodemenu='" & DataGridView1.Rows(Baris).Cells(0).Value & "'", Conn)
                Rd = Cmd.ExecuteReader
                Rd.Read()
                Dim KurangiStok As String = "Update tbl_menuproduk set jumlahmenu ='" & Rd.Item("JumlahMenu") - DataGridView1.Rows(Baris).Cells(3).Value & "' where kodemenu='" & DataGridView1.Rows(Baris).Cells(0).Value & "'"
                Cmd = New SqlCommand(KurangiStok, Conn)
                Rd.Close()
                Cmd.ExecuteNonQuery()
            Next
            If MessageBox.Show("Tampilkan Nota?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                AxCrystalReport1.SelectionFormula = "totext({tbl_penjualan.NoJual})='" & Label4.Text & "'"
                AxCrystalReport1.ReportFileName = "Faktur.rpt"
                AxCrystalReport1.WindowState = Crystal.WindowStateConstants.crptMaximized
                AxCrystalReport1.RetrieveDataFiles()
                AxCrystalReport1.Action = 1
                Call KondisiAwal()
            Else
                Call KondisiAwal()
                MsgBox("Simpan Data Berhasil")
            End If
        End If
    End Sub
    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Call KondisiAwal()
    End Sub
    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = Chr(13) Then
            Button4.Focus()
        End If
    End Sub

    Private Sub AxCrystalActiveXReportViewer1_Enter(sender As Object, e As EventArgs)

    End Sub
End Class