Public Class FormMenuUtama
    Sub Terkunci()
        LoginToolStripMenuItem.Enabled = True
        LogoutToolStripMenuItem.Enabled = False
        MasterToolStripMenuItem.Enabled = False
        TransaksiToolStripMenuItem.Enabled = False
        LaporanToolStripMenuItem.Enabled = False
        STLabel2.Text = ""
        STLabel4.Text = ""
        STLabel6.Text = ""
    End Sub
    Private Sub FormMenuUtama_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Terkunci()
        STLabel10.Text = Today
    End Sub

    Private Sub LoginToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LoginToolStripMenuItem.Click
        FormLogin.ShowDialog()
    End Sub

    Private Sub LogoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogoutToolStripMenuItem.Click
        Call Terkunci()
    End Sub
    Private Sub KeluarToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles KeluarToolStripMenuItem.Click
        End
    End Sub
    Private Sub AdminToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdminToolStripMenuItem.Click
        FormMasterAdmin.ShowDialog()
    End Sub

    Private Sub SupplierToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupplierToolStripMenuItem.Click
        FormMasterSupplier.ShowDialog()
    End Sub
    Private Sub MenuProdukToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MenuProdukToolStripMenuItem.Click
        FormMasterMenu.ShowDialog()
    End Sub

    Private Sub PenjualanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PenjualanToolStripMenuItem.Click
        FormTransaksiPenjualan.ShowDialog()
    End Sub

    Private Sub PembelianToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PembelianToolStripMenuItem.Click
        FormTransaksiPembelian.ShowDialog()
    End Sub

    Private Sub LaporanPenjualanToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LaporanPenjualanToolStripMenuItem.Click
        FormLaporanPenjualan.ShowDialog()
    End Sub

    Private Sub LaporanPembelianToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LaporanPembelianToolStripMenuItem.Click
        FormLaporanPembelian.ShowDialog()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        STLabel8.Text = TimeOfDay
    End Sub

End Class