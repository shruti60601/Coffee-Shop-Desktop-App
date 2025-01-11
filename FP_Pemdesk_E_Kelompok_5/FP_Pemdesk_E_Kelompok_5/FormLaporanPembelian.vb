Public Class FormLaporanPembelian
    Private Sub FormLaporanPembelian_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("01 - JANUARI")
        ComboBox1.Items.Add("02 - FEBRUARI")
        ComboBox1.Items.Add("03 - MARET")
        ComboBox1.Items.Add("04 - APRIL")
        ComboBox1.Items.Add("05 - MEI")
        ComboBox1.Items.Add("06 - JUNI")
        ComboBox1.Items.Add("07 - JULI")
        ComboBox1.Items.Add("08 - AGUSTUS")
        ComboBox1.Items.Add("09 - SEPTEMBER")
        ComboBox1.Items.Add("10 - OKTOBER")
        ComboBox1.Items.Add("11 - NOVEMBER")
        ComboBox1.Items.Add("12 - DESEMBER")

        ComboBox2.Text = Date.Now.Year
        For i As Integer = 0 To 5
            ComboBox2.Items.Add(Date.Now.Year - i)
        Next
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AxCrystalReport1.SelectionFormula = "month({tbl_pembelian.TglBeli})=" & Val(ComboBox1.Text) & " and year({tbl_pembelian.TglBeli})=" & Val(ComboBox2.Text)
        AxCrystalReport1.ReportFileName = "LaporanPembelianBulanan.rpt"
        AxCrystalReport1.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport1.RetrieveDataFiles()
        AxCrystalReport1.Action = 1
    End Sub
End Class