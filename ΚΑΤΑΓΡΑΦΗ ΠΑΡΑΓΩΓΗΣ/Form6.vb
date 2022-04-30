Imports MySql.Data.MySqlClient

Public Class Form6

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form5.DataGridView1.DataSource = Nothing
        Form5.clearcontrols()
        Form5.TextBox5.Text = DataGridView1.CurrentRow.Cells(1).Value.ToString
        Form5.eidos = Integer.Parse(DataGridView1.CurrentRow.Cells(0).Value.ToString)
        Form5.TextBox1.Text = DataGridView1.CurrentRow.Cells(2).Value.ToString
        dtvaluetoset("SELECT DISTINCT perigrafi,id FROM phases JOIN constructionmaterials ON phashparagoghs=id JOIN construction ON construction.constructionid=constructionmaterials.constructionid WHERE construction.eidos='" & Form5.eidos & "' ORDER BY phase ASC")
        Form5.initrowcount = dt.Rows.Count
        If dt.Rows.Count <> 0 Then
            Form5.TextBox4.Text = dt.Rows(0).Item(0).ToString
            Form5.phase_id = Integer.Parse(dt.Rows(0).Item(1).ToString)
            dtvaluetoset("SELECT path FROM prodextra WHERE eidos_id='" & Form5.eidos & "' AND phase='" & Form5.phase_id & "'")
            If dt.Rows.Count > 0 Then

                Form5.photo = dt.Rows(0).Item(0).ToString
                Form5.PictureBox1.ImageLocation = Form5.addpath(Form5.photo)
                Form5.PictureBox1.Load()
                Form5.TextBox2.Text = Form5.photo
            End If

            dtvaluetoset("SELECT path FROM prodextrapdf WHERE eidos='" & Form5.eidos & "'")
            If dt.Rows.Count > 0 Then
                Form5.TextBox3.Text = dt.Rows(0).Item(0).ToString
            End If
            loadgrid("SELECT DISTINCT eidi.code,eidi.perigrafi,monades_metr.perigrafi,constructionmaterials.posothta FROM constructionmaterials JOIN eidi ON constructionmaterials.eidos=eidi.eidos JOIN monades_metr ON monades_metr.monada=eidi.mm1 WHERE constructionid IN (SELECT constructionid FROM construction WHERE eidos='" & Form5.eidos & "') AND phashparagoghs='" & Form5.phase_id & "'", Form5.DataGridView1)
            Form5.gridlayout()
        Else
            MessageBox.Show("Το επιλεγμένο είδος δεν έχει τεχνικές προδιαγραφές")
            Form5.clearcontrols()
            Form5.DataGridView1.DataSource = Nothing
        End If
        dtvaluetoset("SELECT remarks,path FROM prodextra WHERE eidos_id='" & Form5.eidos & "'")
        If dt.Rows.Count > 0 Then
            Form5.TextBox7.Text = dt.Rows(0).Item(0)
            Form5.TextBox2.Text = dt.Rows(0).Item(1)
        End If
        Form5.Show()
        Me.Close()
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Button1.PerformClick()
    End Sub
    Private Sub frmCustomerDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button1.PerformClick()
        End If

    End Sub
End Class