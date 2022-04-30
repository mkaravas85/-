Imports MySql.Data.MySqlClient
Public Class Form2
   
    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dtvaluetoset("SELECT description,id from prodline")
        For i = 0 To dt.Rows.Count - 1
            Dim btn As New CustomControl1
            btn.Text = dt.Rows(i).Item(0).ToString
            btn.prodline = dt.Rows(i).Item(1)
            FlowLayoutPanel1.Controls.Add(btn)
            btn.Width = 150
            btn.Height = 70
            'btn.Anchor = AnchorStyles.Bottom
            'btn.Anchor = AnchorStyles.Left
            'btn.Anchor = AnchorStyles.None 'AnchorStyles.Right Or AnchorStyles.Bottom Or AnchorStyles.Left 'Or AnchorStyles.Top
            'btn.Anchor = AnchorStyles.Top
            AddHandler btn.Click, AddressOf clickme
        Next
    End Sub
    Private Sub clickme(sender As Object, ByVal e As EventArgs)
        Dim btn As CustomControl1
        btn = CType(sender, CustomControl1)
        grammi = btn.prodline
        Form3.Label1.Text = btn.Text
        Form3.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Button2.BackColor = DefaultBackColor Then
            Button2.BackColor = Color.CornflowerBlue
            Exit Sub
        End If
        If Button2.BackColor = Color.CornflowerBlue Then
            Button2.BackColor = DefaultBackColor
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form5.Show()
    End Sub
End Class