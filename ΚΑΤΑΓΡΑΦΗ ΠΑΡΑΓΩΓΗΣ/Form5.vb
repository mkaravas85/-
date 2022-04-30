Imports MySql.Data.MySqlClient
Imports System.IO
Public Class Form5
    Public eidos, initrowcount, i, phase_id As Integer
    Dim S As String
    Public eidos_code, pdf, photo As String
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        i = 0
        loadgrid("SELECT eidos,code,perigrafi FROM eidi WHERE code LIKE '" & TextBox5.Text & "%'", Form6.DataGridView1)
        Form6.DataGridView1.Columns(0).Visible = False
        Form6.DataGridView1.Columns(1).Width = 100
        Form6.DataGridView1.Columns(2).Width = 293
        Form6.DataGridView1.Columns(1).HeaderText = "EIΔΟΣ"
        Form6.DataGridView1.Columns(2).HeaderText = "ΠΕΡΙΓΡΑΦΗ"
        Form6.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If i > 0 Then
            i = i - 1
        End If

        dtvaluetoset("SELECT DISTINCT perigrafi,id FROM phases JOIN constructionmaterials ON phashparagoghs=id JOIN construction ON construction.constructionid=constructionmaterials.constructionid WHERE construction.eidos='" & eidos & "' ORDER BY phase ASC")
        phase_id = Integer.Parse(dt.Rows(i).Item(1).ToString)
        TextBox4.Text = dt.Rows(i).Item(0).ToString
        dtvaluetoset("SELECT path FROM prodextra WHERE eidos_id='" & eidos & "' AND phase='" & phase_id & "'")
        If dt.Rows.Count > 0 Then
            photo = dt.Rows(0).Item(0).ToString
            PictureBox1.ImageLocation = photo
            TextBox2.Text = photo
        End If

        dtvaluetoset("SELECT path FROM prodextrapdf WHERE eidos='" & eidos & "'")
        If dt.Rows.Count > 0 Then
            TextBox3.Text = dt.Rows(0).Item(0).ToString
        End If
        loadgrid("SELECT DISTINCT eidi.code,eidi.perigrafi,monades_metr.perigrafi,constructionmaterials.posothta FROM constructionmaterials JOIN eidi ON constructionmaterials.eidos=eidi.eidos JOIN monades_metr ON monades_metr.monada=eidi.mm1 WHERE constructionid IN (SELECT constructionid FROM construction WHERE eidos='" & eidos & "') AND phashparagoghs='" & phase_id & "'", Me.DataGridView1)

        gridlayout()
    End Sub
    Public Sub gridlayout()
        DataGridView1.Columns(0).HeaderText = "EIΔΟΣ"
        DataGridView1.Columns(1).HeaderText = "ΠΕΡΙΓΡΑΦΗ"
        DataGridView1.Columns(2).HeaderText = "ΜΟΝΑΔΑ ΜΕΤΡΗΣΗΣ"
        DataGridView1.Columns(3).HeaderText = "ΠΟΣΟΤΗΤΑ"
        DataGridView1.Columns(0).Width = 120
        DataGridView1.Columns(1).Width = 425
    End Sub
    Private Sub frmCustomerDetails_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button3.PerformClick()
        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        OpenFileDialog1.Title = "Επιλογή Αρχείου PDF"
        OpenFileDialog1.Filter = "PDF Files|*.pdf"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.InitialDirectory = Directory.GetCurrentDirectory
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            TextBox3.Text = System.IO.Path.GetFileName(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        pdf = addpath(TextBox3.Text)
        photo = addpath(TextBox2.Text)
        dt.Reset()
        dtvaluetoset("SELECT id FROM prodextra WHERE eidos_id='" & eidos & "' AND phase='" & phase_id & "'")

        If dt.Rows.Count > 0 Then
            runquery("UPDATE prodextra set remarks='" & TextBox7.Text & "',path='" & TextBox2.Text & "' WHERE eidos_id='" & eidos & "'AND phase='" & phase_id & "'")
            'runquery("UPDATE prodextrapdf set path='" & pdf & "' WHERE eidos='" & eidos & "'")
        Else 'If String.IsNullOrWhiteSpace(TextBox3.Text) = False Or String.IsNullOrWhiteSpace(TextBox2.Text) Then
            runquery("INSERT INTO prodextra (eidos_id,phase,remarks,path) VALUES ('" & eidos & "','" & phase_id & "','" & TextBox7.Text & "','" & TextBox2.Text & "')")
            'runquery("INSERT INTO prodextrapdf (eidos,path) VALUES ('" & eidos & "','" & pdf & "')")
        End If
        dtvaluetoset("SELECT id1 FROM prodextrapdf WHERE eidos='" & eidos & "'")
        If dt.Rows.Count > 0 Then
            runquery("UPDATE prodextrapdf set path='" & TextBox3.Text & "' WHERE eidos='" & eidos & "'")
        Else
            runquery("INSERT INTO prodextrapdf (eidos,path) VALUES ('" & eidos & "','" & TextBox3.Text & "')")
        End If
        If queryerror = False Then
            MessageBox.Show("Επιτυχής Καταχώρηση")
        Else
            queryerror = False
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        addpath(TextBox3.Text)

        If System.IO.File.Exists(S) Then

            Try
                Process.Start("explorer.exe", S)
            Catch ex As Exception
                MessageBox.Show(ex.ToString)
            End Try
        Else
            MessageBox.Show("Η διαδρομή αρχείου δεν υπάρχει")
        End If

    End Sub

    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dtvaluetoset("SELECT eidos FROM eidi WHERE code='" & eidos_code & "'")
        If dt.Rows.Count > 0 Then
            eidos = dt.Rows(0).Item(0)

            dtvaluetoset("SELECT DISTINCT perigrafi,id FROM phases JOIN constructionmaterials ON phashparagoghs=id JOIN construction ON construction.constructionid=constructionmaterials.constructionid WHERE construction.eidos='" & eidos & "' ORDER BY phase ASC")
            initrowcount = dt.Rows.Count
            phase_id = Integer.Parse(dt.Rows(i).Item(1).ToString)
            TextBox4.Text = dt.Rows(0).Item(0).ToString
            dtvaluetoset("SELECT path FROM prodextra WHERE eidos_id='" & eidos & "' AND phase='" & phase_id & "'")
            If dt.Rows.Count > 0 Then
                photo = dt.Rows(0).Item(0).ToString
                PictureBox1.ImageLocation = photo
                TextBox2.Text = photo
            End If

            dtvaluetoset("SELECT path FROM prodextrapdf WHERE eidos='" & eidos & "'")
            If dt.Rows.Count > 0 Then
                TextBox3.Text = dt.Rows(0).Item(0).ToString
            End If
            loadgrid("SELECT DISTINCT eidi.code,eidi.perigrafi,monades_metr.perigrafi,constructionmaterials.posothta FROM constructionmaterials JOIN eidi ON constructionmaterials.eidos=eidi.eidos JOIN monades_metr ON monades_metr.monada=eidi.mm1 WHERE constructionid IN (SELECT constructionid FROM construction WHERE eidos='" & eidos & "') AND phashparagoghs='" & phase_id & "'", DataGridView1)
                gridlayout()
            End If
    End Sub
    Public Sub clearcontrols()
        Dim ctl As Control
        For Each ctl In Me.Controls
            If TypeOf ctl Is TextBox Then
                ctl.Text = ""
            End If
        Next
    End Sub

    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        OpenFileDialog1.Title = "Επιλογή Αρχείου Φωτογραφίας"
        OpenFileDialog1.Filter = "Αρχείο Φωτογραφίας|*.jpg;*.png;*.bmp"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.InitialDirectory = Directory.GetCurrentDirectory
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            TextBox2.Text = System.IO.Path.GetFileName(OpenFileDialog1.FileName)
        End If
    End Sub

    Public Function addpath(filename As String) As String

        S = Directory.GetCurrentDirectory & "\" & filename
        Return S
    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If i < initrowcount - 1 Then
            i = i + 1
        End If

        dtvaluetoset("SELECT DISTINCT perigrafi,id FROM phases JOIN constructionmaterials ON phashparagoghs=id JOIN construction ON construction.constructionid=constructionmaterials.constructionid WHERE construction.eidos='" & eidos & "' ORDER BY phase ASC")
        phase_id = Integer.Parse(dt.Rows(i).Item(1).ToString)
        TextBox4.Text = dt.Rows(i).Item(0).ToString
        dtvaluetoset("SELECT DISTINCT perigrafi,id FROM phases JOIN constructionmaterials ON phashparagoghs=id JOIN construction ON construction.constructionid=constructionmaterials.constructionid WHERE construction.eidos='" & eidos & "' ORDER BY phase ASC")
        initrowcount = dt.Rows.Count

        dtvaluetoset("SELECT path FROM prodextra WHERE eidos_id='" & eidos & "' AND phase='" & phase_id & "'")
        If dt.Rows.Count > 0 Then
            photo = dt.Rows(0).Item(0).ToString
            PictureBox1.ImageLocation = photo
            TextBox2.Text = photo
        End If

        dtvaluetoset("SELECT path FROM prodextrapdf WHERE eidos='" & eidos & "'")
        If dt.Rows.Count > 0 Then
            TextBox3.Text = dt.Rows(0).Item(0).ToString
        End If
        loadgrid("SELECT DISTINCT eidi.code,eidi.perigrafi,monades_metr.perigrafi,constructionmaterials.posothta FROM constructionmaterials JOIN eidi ON constructionmaterials.eidos=eidi.eidos JOIN monades_metr ON monades_metr.monada=eidi.mm1 WHERE constructionid IN (SELECT constructionid FROM construction WHERE eidos='" & eidos & "') AND phashparagoghs='" & phase_id & "'", DataGridView1)

        gridlayout()
    End Sub
End Class