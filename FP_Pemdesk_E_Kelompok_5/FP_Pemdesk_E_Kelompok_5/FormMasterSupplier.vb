Imports System.Data.SqlClient
Public Class FormMasterSupplier
    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox6.Text = ""
        ComboBox1.Items.Clear()
        ComboBox1.Text = ""
        TextBox1.Enabled = False
        TextBox2.Enabled = False
        TextBox3.Enabled = False
        TextBox4.Enabled = False
        TextBox5.Enabled = False
        TextBox6.Enabled = False
        ComboBox1.Enabled = False

        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
        Button1.Text = "Input"
        Button2.Text = "Edit"
        Button3.Text = "Hapus"
        Button4.Text = "Tutup"
        Call Koneksi()
        Da = New SqlDataAdapter("Select KodeSupplier, NamaSupplier, AlamatSupplier, NoTelpSupplier, JenisSupplier, HargaSupplier, StatusSupplier From tbl_supplier", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "tbl_supplier")
        DataGridView1.DataSource = Ds.Tables("tbl_supplier")
        DataGridView1.ReadOnly = True
    End Sub
    Sub SiapIsi()
        TextBox1.Enabled = True
        TextBox2.Enabled = True
        TextBox3.Enabled = True
        TextBox4.Enabled = True
        TextBox5.Enabled = True
        TextBox6.Enabled = True
        ComboBox1.Enabled = True
        ComboBox1.Items.Add("Aktif")
        ComboBox1.Items.Add("Tidak Aktif")
    End Sub

    Private Sub FormMasterSupplier_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub
    Sub NomorOtomatis()
        Call Koneksi()
        Cmd = New SqlCommand("Select * from tbl_supplier where kodesupplier in (select max(kodesupplier) from tbl_supplier)", Conn)
        Dim UrutanKode As String
        Dim Hitung As Long
        Rd = Cmd.ExecuteReader
        Rd.Read()
        If Not Rd.HasRows Then
            UrutanKode = "SP" + "001"
        Else
            Hitung = Microsoft.VisualBasic.Right(Rd.GetString(0), 3) + 1
            UrutanKode = "SP" + Microsoft.VisualBasic.Right("000" & Hitung, 3)
        End If
        TextBox1.Text = UrutanKode
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
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Or TextBox5.Text = "" Or TextBox6.Text = "" Or ComboBox1.Text = "" Then
                MsgBox("Silahkan Isi Semua Field")
            Else
                Call Koneksi()
                Dim InputData As String = "insert into tbl_supplier values('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & TextBox4.Text & "','" & TextBox5.Text & "','" & TextBox6.Text & "','" & ComboBox1.Text & "')"
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
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Or TextBox5.Text = "" Or TextBox6.Text = "" Or ComboBox1.Text = "" Then
                MsgBox("Silahkan Isi Semua Field")
            Else
                Call Koneksi()
                Dim UpdateData As String = "Update tbl_supplier set namasupplier='" & TextBox2.Text & "',alamatsupplier='" & TextBox3.Text & "',notelpsupplier='" & TextBox4.Text & "',jenissupplier='" & TextBox5.Text & "',hargasupplier='" & TextBox5.Text & "',statussupplier='" & ComboBox1.Text & "' where kodemenu='" & TextBox1.Text & "'"
                Cmd = New SqlCommand(UpdateData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Update Data Berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            Call Koneksi()
            Cmd = New SqlCommand("Select * from tbl_supplier where kodesupplier ='" & TextBox1.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            Rd.Read()
            If Not Rd.HasRows Then
                MsgBox("Kode Supplier Tidak Ada")
            Else
                TextBox1.Text = Rd.Item("KodeSupplier")
                TextBox2.Text = Rd.Item("NamaSupplier")
                TextBox3.Text = Rd.Item("AlamatSupplier")
                TextBox4.Text = Rd.Item("NoTelpSupplier")
                TextBox5.Text = Rd.Item("JenisSupplier")
                TextBox6.Text = Rd.Item("HargaSupplier")
                ComboBox1.Text = Rd.Item("StatusSupplier")
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
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Or TextBox5.Text = "" Or TextBox6.Text = "" Or ComboBox1.Text = "" Then
                MsgBox("Silahkan Isi Semua Field")
            Else
                Call Koneksi()
                Dim HapusData As String = "Delete from tbl_supplier where kodesupplier='" & TextBox1.Text & "'"
                Cmd = New SqlCommand(HapusData, Conn)
                Cmd.ExecuteNonQuery()
                MsgBox("Hapus Data Berhasil")
                Call KondisiAwal()
            End If
        End If
    End Sub
    Private Sub TextBox2_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox3.Focus()
        End If
    End Sub

    Private Sub TextBox3_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox3.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox4.Focus()
        End If
    End Sub

    Private Sub TextBox4_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox4.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox5.Focus()
        End If
    End Sub

    Private Sub TextBox5_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox5.KeyPress
        If e.KeyChar = Chr(13) Then
            TextBox6.Focus()
        End If
    End Sub

    Private Sub TextBox6_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox6.KeyPress
        If e.KeyChar = Chr(13) Then
            ComboBox1.Focus()
        End If
    End Sub
End Class