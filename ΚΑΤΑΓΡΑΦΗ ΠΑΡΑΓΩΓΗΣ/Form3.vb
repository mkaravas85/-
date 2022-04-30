Imports System.Runtime.InteropServices
Public Class Form3
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form1.Label1.Text = Label1.Text
        If RadioButton1.Checked Then
            Form1.Case1 = 1
            TODATE()
        End If
        If RadioButton2.Checked Then
            Form1.Case1 = 2
            Form1.TextBox2.Text = DateTimePicker2.Text
            Form1.TextBox3.Text = DateTimePicker3.Text
        End If
        If RadioButton3.Checked Then
            Form1.Case1 = 3
        End If
        If RadioButton4.Checked Then
            Form1.Case1 = 4
        End If
        Form1.Show()
    End Sub
    Public Sub TODATE()
        Dim date1 As String = DateTimePicker1.Value.ToString

        Dim date3 As String() = date1.Split(" ")

        Dim date4 As String() = date3(0).Split("/")
        date5 = date4(2) + "-" + date4(1) + "-" + date4(0)
    End Sub
    <DllImport("Uxtheme.dll", SetLastError:=True)>
    Public Shared Function SetWindowTheme(ByVal hWnd As IntPtr, ByVal pszSubAppName As String, ByVal pszSubIdList As String) As Integer
    End Function
    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        DateTimePicker1.Format = DateTimePickerFormat.Custom
        DateTimePicker1.CustomFormat = "dd/MM/yyyy"
        SetWindowTheme(DateTimePicker1.Handle, "", "")
        DateTimePicker2.Format = DateTimePickerFormat.Custom
        DateTimePicker2.CustomFormat = "dd/MM/yyyy"
        DateTimePicker3.Format = DateTimePickerFormat.Custom
        DateTimePicker3.CustomFormat = "dd/MM/yyyy"
        DateTimePicker4.Format = DateTimePickerFormat.Custom
        DateTimePicker4.CustomFormat = "dd/MM/yyyy"
        DateTimePicker5.Format = DateTimePickerFormat.Custom
        DateTimePicker5.CustomFormat = "dd/MM/yyyy"
        DateTimePicker6.Format = DateTimePickerFormat.Custom
        DateTimePicker6.CustomFormat = "dd/MM/yyyy"
        DateTimePicker7.Format = DateTimePickerFormat.Custom
        DateTimePicker7.CustomFormat = "dd/MM/yyyy"
    End Sub

    Private Sub DateTimePicker2_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker2.ValueChanged
        If DateTimePicker3.Value < DateTimePicker2.Value Then
            DateTimePicker3.Value = DateTimePicker2.Value.AddDays(1)
        End If
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked = False Then

            DateTimePicker1.Enabled = False
        Else
            DateTimePicker1.Enabled = True
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked = False Then
            DateTimePicker3.Enabled = False
            DateTimePicker2.Enabled = False
            Label2.Enabled = False
            Label3.Enabled = False
        Else
            DateTimePicker3.Enabled = True
            DateTimePicker2.Enabled = True
            Label2.Enabled = True
            Label3.Enabled = True
        End If
    End Sub

    Private Sub RadioButton3_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton3.CheckedChanged
        If RadioButton3.Checked = False Then
            DateTimePicker4.Enabled = False
            DateTimePicker5.Enabled = False
            Label4.Enabled = False
            Label5.Enabled = False
        Else
            Label4.Enabled = True

            Label5.Enabled = True
            DateTimePicker4.Enabled = True
            DateTimePicker5.Enabled = True
        End If
    End Sub

    Private Sub DateTimePicker5_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker5.ValueChanged
        If DateTimePicker4.Value < DateTimePicker5.Value Then
            DateTimePicker4.Value = DateTimePicker5.Value.AddDays(1)
        End If
    End Sub

    Private Sub DateTimePicker6_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker6.ValueChanged
        If DateTimePicker7.Value < DateTimePicker6.Value Then
            DateTimePicker7.Value = DateTimePicker6.Value.AddDays(1)
        End If
    End Sub

    Private Sub RadioButton4_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton4.CheckedChanged
        If RadioButton4.Checked = False Then
            DateTimePicker6.Enabled = False
            DateTimePicker7.Enabled = False
            Label6.Enabled = False
            Label7.Enabled = False
        Else
            Label6.Enabled = True
            Label7.Enabled = True
            DateTimePicker6.Enabled = True
            DateTimePicker7.Enabled = True
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Close()
    End Sub
End Class