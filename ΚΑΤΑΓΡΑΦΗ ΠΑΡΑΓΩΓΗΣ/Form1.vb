Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Imports System.Runtime.InteropServices
Imports System.Globalization

Public Class Form1
    Dim col2 As New DataGridViewColumn
    Public Case1, hours, mins, wres1, lepta1 As Integer
    Public d2, d3 As DateTime
    Dim time As String
    Dim inputerror As Boolean = False
    Dim firstlog As Boolean = False
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ToolTip1.ShowAlways = True
        'ToolTip1.SetToolTip(DataGridView1, "hlk ")

        If Case1 = 1 Then
            Me.Text = "Καταχώρηση Στοιχείων"

            TextBox2.Text = Form3.DateTimePicker1.Text
            TextBox3.Text = Form3.DateTimePicker1.Text
            TextBox4.Text = Form3.DateTimePicker1.Text
            Button3.Enabled = False
            loadgrid("SELECT eidi.code,eidi.perigrafi,pelproms.eponymia,qty,actiondescr,hrs,mins,prodreal2.final_qty,prodreal2.final_hrs,prodreal2.final_mins,prodscheduleline.id,prodreal2.idprodline FROM prodscheduleline LEFT JOIN eidi ON prodscheduleline.eidos=eidi.eidos LEFT JOIN pelproms ON eidi.pelprom=pelproms.pelprom LEFT JOIN prodreal2 ON prodscheduleline.id=prodreal2.idprodline WHERE prodscheduleline.fdate='" & date5 & "' AND prodline='" & grammi & "'", Me.DataGridView1)
            Button4.Enabled = False
            gridlayout()
        End If

        If Case1 = 2 Then
            Me.Text = "Προγράμματα παραγωγής"
            TextBox2.Text = Form3.DateTimePicker2.Value.Date
            TextBox3.Text = Form3.DateTimePicker3.Value.Date
            d2 = Form3.DateTimePicker2.Value.Date
            d3 = Form3.DateTimePicker3.Value.Date
            checksoukou()

            TODATETIME(d2)

            loadgrid("SELECT eidi.code,eidi.perigrafi,pelproms.eponymia,qty,actiondescr,hrs,mins,prodreal2.final_qty,prodreal2.final_hrs,prodreal2.final_mins,prodscheduleline.id,prodreal2.idprodline FROM prodscheduleline LEFT JOIN eidi ON prodscheduleline.eidos=eidi.eidos LEFT JOIN pelproms ON eidi.pelprom=pelproms.pelprom LEFT JOIN prodreal2 ON prodscheduleline.id=prodreal2.idprodline WHERE prodscheduleline.fdate='" & date6 & "' AND prodline='" & grammi & "'", Me.DataGridView1)
            gridlayout()
        End If
        If Case1 = 3 Then
            Me.Text = "Εκκρεμείς παραγωγές"
            TextBox2.Text = Form3.DateTimePicker5.Value.Date
            TextBox3.Text = Form3.DateTimePicker4.Value.Date
            d2 = Form3.DateTimePicker5.Value.Date
            d3 = Form3.DateTimePicker4.Value.Date
            checksoukou()
            TODATETIME(d2)
            loadgrid("SELECT eidi.code,eidi.perigrafi,pelproms.eponymia,qty,actiondescr,hrs,mins,prodreal2.final_qty,prodreal2.final_hrs,prodreal2.final_mins,prodscheduleline.id,prodreal2.idprodline FROM prodscheduleline LEFT JOIN eidi ON prodscheduleline.eidos=eidi.eidos LEFT JOIN pelproms ON eidi.pelprom=pelproms.pelprom LEFT JOIN prodreal2 ON prodscheduleline.id=prodreal2.idprodline WHERE prodscheduleline.fdate='" & date6 & "' AND prodline='" & grammi & "' AND prodscheduleline.id NOT IN (SELECT idprodline from prodreal2)", Me.DataGridView1)
            gridlayout()

        End If
        If Case1 = 4 Then
            Me.Text = "Καθυστερημένες παραγωγές"
            TextBox2.Text = Form3.DateTimePicker6.Value.Date
            TextBox3.Text = Form3.DateTimePicker7.Value.Date
            d2 = Form3.DateTimePicker6.Value.Date
            d3 = Form3.DateTimePicker7.Value.Date
            checksoukou()
            TODATETIME(d2)
            loadgrid("SELECT eidi.code,eidi.perigrafi,pelproms.eponymia,qty,actiondescr,hrs,mins,prodreal2.final_qty,prodreal2.final_hrs,prodreal2.final_mins,prodscheduleline.id,prodreal2.idprodline FROM prodscheduleline LEFT JOIN eidi ON prodscheduleline.eidos=eidi.eidos LEFT JOIN pelproms ON eidi.pelprom=pelproms.pelprom LEFT JOIN prodreal2 ON prodscheduleline.id=prodreal2.idprodline WHERE prodscheduleline.fdate='" & date6 & "' AND prodline='" & grammi & "' AND date_ins>prodscheduleline.fdate", Me.DataGridView1)
            gridlayout()
            'TextBox4.Text = d2.ToShortDateString
        End If
        If DataGridView1.RowCount = 0 Then
            MessageBox.Show("Δε βρέθηκαν εγγραφές για τη συγκεκριμένη ημερομηνία")
            Me.Close()
        End If

    End Sub
    Private Sub checksoukou()
        If Case1 = 2 Or Case1 = 3 Or Case1 = 4 Then
            If d2.DayOfWeek = DayOfWeek.Saturday Then
                d2 = d2.AddDays(2)
            End If
            If d2.DayOfWeek = DayOfWeek.Sunday Then
                d2 = d2.AddDays(1)
            End If
            Button3.Enabled = True
            Button4.Enabled = True
            Button1.Enabled = False
            Button2.Enabled = False
            TextBox4.Text = d2.ToShortDateString
        End If
    End Sub
    Private Sub TODATETIME(DT As DateTime)
        Dim date1 As String = DT.Date.ToString

        Dim date3 As String() = date1.Split(" ")

        Dim date4 As String() = date3(0).Split("/")
        date6 = date4(2) + "-" + date4(1) + "-" + date4(0)
    End Sub
    Private Sub diastasi()
        Try
            For i = 0 To DataGridView1.RowCount - 1

                Dim a As String = DataGridView1.Rows(i).Cells(0).Value.ToString
                If a <> "" Then
                    DataGridView1.Rows(i).Cells(2).Value = a.Substring(1, 6)
                End If
            Next
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Private Sub todecimalvalue(cl As DataGridViewCell, ByRef ex As String)
        If cl.ToString.Contains(",") Then
            ex = cl.Value.ToString.Replace(",", ".")

        End If

    End Sub
    Private Sub splittime()
        Dim dat As Date = Today
        TODATETIME(dat)

        Form3.TODATE()

        dtvaluetoset("SELECT max(time) FROM prodreal2 WHERE date_ins='" & date6 & "' AND idprodline IN (SELECT id FROM prodscheduleline WHERE prodline='" & grammi & "' AND fdate='" & date5 & "')")
        Dim wra As String

        wra = dt.Rows(0).Item(0).ToString
        If wra = "" Then
            firstlog = True
            Exit Sub
        End If
        wra = wra.Remove(wra.Length - 3, 3)
        wra = wra.Replace(":", "")
        Dim wres, lepta As String
        wres = wra.Substring(0, 2)
        lepta = wra.Substring(2, 2)
        wres1 = Integer.Parse(wres)
        lepta1 = Integer.Parse(lepta)

    End Sub
    Private Sub subtracthours(hh As Integer, m As Integer)
        hours = Math.Abs(hh - DateTime.Now.TimeOfDay.Hours)
        mins = m - DateTime.Now.TimeOfDay.Minutes
        If mins > 0 Then
            mins = 60 - mins
            hours = hours - 1
        Else
            mins = Math.Abs(mins)
        End If

        time = DateTime.Now.TimeOfDay.Hours.ToString & ":" & DateTime.Now.TimeOfDay.Minutes.ToString & ":00"

    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'ΘΕΛΟΥΜΕ ΟΙ ΩΡΕΣ ΚΑΙ ΤΑ ΛΕΠΤΑ ΝΑ ΥΠΟΛΟΓΙΖΟΝΤΑΙ ΑΥΤΟΜΑΤΑ ΚΑΙ ΝΑ ΜΗΝ ΚΑΤΑΧΩΡΟΥΝΤΑΙ ΑΠΟ ΤΟ ΧΡΗΣΤΗ. ΘΑ ΥΠΟΛΟΓΙΖΟΝΤΑΙ
        'ΒΑΣΕΙ ΤΗΣ ΩΡΑΣ ΚΑΤΑΧΩΡΗΣΗΣ ΤΗΣ ΠΟΣΟΤΗΤΑΣ.ΟΠΟΤΕ ΕΧΩ ΗΔΗ ΕΝΑ ΠΕΔΙΟ ΠΟΥ ΚΑΤΑΧΩΡΩ ΤΗΝ ΗΜΕΡΟΜΗΝΙΑ ΑΣ ΦΤΙΑΞΩ ΚΑΙ ΕΝΑ
        ' ΝΑ ΚΑΤΑΧΩΡΩ ΤΗΝ ΩΡΑ ΚΑΙ ΝΑ ΠΑΙΡΝΩ ΤΟ ΤΕΛΕΥΤΑΙΟ ΠΟΥ ΕΧΕΙ ΚΑΤΑΧΩΡΗΘΕΙ ΒΑΣΕΙ ΩΡΑΣ ΚΑΙ ΑΠΟ ΑΥΤΟ ΝΑ ΥΠΟΛΟΓΙΖΩ
        'ΤΗ ΔΙΑΡΚΕΙΑ. ΤΟ splittime ΑΚΡΙΒΩΣ ΑΠΟ ΠΑΝΩ ΑΥΤΟ ΕΧΕΙ ΡΟΛΟ ΝΑ ΚΑΝΕΙ. ΝΑ ΠΑΙΡΝΕΙ ΣΑ STRING ΤΗΝ ΩΡΑ ΠΟΥ ΤΟΥ ΖΗΤΑΩ ΚΑΙ 
        'ΝΑ ΤΗ ΣΠΑΕΙ. Ο ΑΠΟ ΚΑΤΩ ΚΩΔΙΚΑΣ ΚΑΝΕΙ ΤΟΝ ΠΡΩΤΟ ΥΠΟΛΟΓΙΣΜΟ ΤΗΣ ΚΑΘΕ ΜΕΡΑΣ Η ΟΠΟΙΑ ΞΕΚΙΝΑΕΙ ΑΠΟ ΤΙΣ 7:30 ΤΟ ΠΡΩΙ
        'ΑΠΟ ΔΩ ΚΑΙ ΚΑΤΩ ΟΤΙ ΕΙΝΑΙ ΣΧΟΛΙΑΣΜΕΝΟ (ΓΙΑ ΑΥΤΟ ΤΟ ΚΟΥΜΠΙ) ΕΙΝΑΙ ΠΡΩΗΝ ΛΕΙΤΟΥΡΓΙΚΟΣ ΚΩΔΙΚΑΣ
        splittime()

        If firstlog = True Then
            subtracthours(7, 30)
        Else

            subtracthours(wres1, lepta1)
        End If
        firstlog=False
        Dim dat As Date = Today
        TODATETIME(dat)
        If Button2.BackColor = DefaultBackColor Then
            If DataGridView1.CurrentRow.Cells(8).Value.ToString <> "" Then 'And DataGridView1.CurrentRow.Cells(9).Value.ToString <> "" And DataGridView1.CurrentRow.Cells(10).Value.ToString <> "" Then
                If DataGridView1.CurrentRow.Cells(12).Value.ToString = "" Then
                    'If Integer.Parse(DataGridView1.CurrentRow.Cells(9).Value.ToString) > 24 Or Integer.Parse(DataGridView1.CurrentRow.Cells(10).Value.ToString) > 59 Then
                    '    If Integer.Parse(DataGridView1.CurrentRow.Cells(9).Value.ToString) > 24 Then
                    '        DataGridView1.CurrentRow.Cells(9).Style.ForeColor = Color.Red
                    '    End If
                    '    If Integer.Parse(DataGridView1.CurrentRow.Cells(10).Value.ToString) > 59 Then
                    '        DataGridView1.CurrentRow.Cells(10).Style.ForeColor = Color.Red
                    '    End If

                    '    DataGridView1.CurrentCell.Selected = False
                    '    MessageBox.Show("Λάθος είσοδος στο χρόνο ολοκλήρωσης")
                    '    Exit Sub
                    'End If
                    runquery("INSERT INTO prodreal2 (idprodline,final_qty,final_hrs,final_mins,date_ins,time) VALUES ('" & DataGridView1.CurrentRow.Cells(11).Value.ToString & "','" & DataGridView1.CurrentRow.Cells(8).Value.ToString & "','" & hours & "','" & mins & "','" & date6 & "','" & time & "')")
                    If queryerror = False Then

                        Form4.Label1.Text = "Επιτυχής Καταχώρηση"
                        Timer1.Enabled = True
                        Form4.Show()

                    End If
                Else
                    MessageBox.Show("Πρέπει να επιλέξετε μεταβολή για να τροποποιήσετε τη συγκεκριμένη γραμμή")
                    Exit Sub
                End If
            Else
                MessageBox.Show("Υπάρχουν ασυμπλήρωτα πεδία στη γραμμή που προσπαθείτε να καταχωρήσετε")
                Exit Sub

            End If

        End If
        If Button2.BackColor = Color.CornflowerBlue Then
            Dim cl1 As DataGridViewCell = DataGridView1.CurrentRow.Cells(8)
            Dim ex1 As String
            todecimalvalue(cl1, ex1)
            If Integer.Parse(DataGridView1.CurrentRow.Cells(9).Value.ToString) > 24 Or Integer.Parse(DataGridView1.CurrentRow.Cells(10).Value.ToString) > 59 Then
                If Integer.Parse(DataGridView1.CurrentRow.Cells(9).Value.ToString) > 24 Then
                    DataGridView1.CurrentRow.Cells(9).Style.ForeColor = Color.Red
                End If
                If Integer.Parse(DataGridView1.CurrentRow.Cells(10).Value.ToString) > 59 Then
                    DataGridView1.CurrentRow.Cells(10).Style.ForeColor = Color.Red
                End If

                DataGridView1.CurrentCell.Selected = False
                MessageBox.Show("Λάθος είσοδος στο χρόνο ολοκλήρωσης")

                Exit Sub
            End If
            'runquery("UPDATE prodreal2 Set final_qty='" & ex1 & "',final_hrs='" & DataGridView1.CurrentRow.Cells(9).Value & "',final_mins='" & DataGridView1.CurrentRow.Cells(10).Value & "',date_ins='" & date6 & "' WHERE idprodline='" & DataGridView1.CurrentRow.Cells(11).Value & "'")
            runquery("UPDATE prodreal2 Set final_qty='" & ex1 & "' WHERE idprodline='" & DataGridView1.CurrentRow.Cells(11).Value & "'")
            If queryerror = False Then
                Form4.Label1.Text = "Η μεταβολή έγινε"
                Timer1.Enabled = True
                Form4.Show()
            End If
        End If
        DataGridView1.Columns.Clear()
        Button2.BackColor = DefaultBackColor
        If queryerror = True Then
            queryerror = False
            Exit Sub
        End If
        loadgrid("SELECT eidi.code,eidi.perigrafi,pelproms.eponymia,qty,actiondescr,hrs,mins,prodreal2.final_qty,prodreal2.final_hrs,prodreal2.final_mins,prodscheduleline.id,prodreal2.idprodline FROM prodscheduleline LEFT JOIN eidi ON prodscheduleline.eidos=eidi.eidos LEFT JOIN pelproms ON eidi.pelprom=pelproms.pelprom LEFT JOIN prodreal2 ON prodscheduleline.id=prodreal2.idprodline WHERE prodscheduleline.fdate='" & date5 & "' AND prodline='" & grammi & "'", Me.DataGridView1)
        Button2.Enabled = False
        gridlayout()
        readonlycells(False)
        Button1.Enabled = False
        inputerror = False

    End Sub
    Private Sub readonlycells(condition As Boolean)
        For k = 0 To DataGridView1.RowCount - 1
            If DataGridView1.Rows(k).Cells(12).Value.ToString = "" Then
                DataGridView1.Rows(k).Cells(8).ReadOnly = condition
                ' DataGridView1.Rows(k).Cells(9).ReadOnly = condition
                ' DataGridView1.Rows(k).Cells(10).ReadOnly = condition
            End If
        Next
    End Sub

    Private Sub gridlayout()
        DataGridView1.Columns(0).Visible = False
        DataGridView1.Columns(1).HeaderText = "Είδος"
        col2.CellTemplate = New DataGridViewTextBoxCell
        DataGridView1.Columns.Insert(2, col2)
        DataGridView1.Columns(2).HeaderText = "Διάσταση"
        DataGridView1.Columns(3).HeaderText = "Πελάτης"

        diastasi()

        DataGridView1.Columns(4).HeaderText = "Ποσότητα παραγωγής"
        DataGridView1.Columns(5).HeaderText = "Τύπος πράξης"
        DataGridView1.Columns(6).HeaderText = "Εκτιμούμενος χρόνος ολοκλήρωσης (Ωρες)"
        DataGridView1.Columns(7).HeaderText = "Εκτιμούμενος χρόνος ολοκλήρωσης (Λεπτά)"
        DataGridView1.Columns(8).HeaderText = "Παραχθείσα ποσότητα"
        DataGridView1.Columns(9).HeaderText = "Πραγματικός χρόνος ολοκλήρωσης (Ωρες)"
        DataGridView1.Columns(10).HeaderText = "Πραγματικός χρόνος ολοκλήρωσης (Λεπτά)"
        DataGridView1.Columns(1).Width = 550
        DataGridView1.Columns(3).Width = 469
        DataGridView1.Columns(5).Width = 230
        DataGridView1.Columns(6).Width = 80
        DataGridView1.Columns(7).Width = 80
        DataGridView1.Columns(9).Width = 80
        DataGridView1.Columns(10).Width = 80
        DataGridView1.Columns(11).Visible = False
        DataGridView1.Columns(12).Visible = False
        For i = 0 To 10
            DataGridView1.Columns(i).ReadOnly = True
        Next
        DataGridView1.Columns(8).ReadOnly = False
    End Sub
    Private Sub killer(name As String)
        Dim p() As Process
        Dim PS As Process
        p = Process.GetProcessesByName(name)

        If p.Count > 0 Then
            For Each PS In p
                PS.Kill()
            Next
        End If
    End Sub
    Private Sub osk()
        Dim proc As System.Diagnostics.Process

        'proc = System.Diagnostics.Process.Start("C:\Program Files\Common Files\microsoft shared\ink\TabTip.exe")
        proc = System.Diagnostics.Process.Start("C:\signetdir\osk.exe")
        'Shell("C:\signetdir\sigtools1.exe")
    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If Case1 = 1 Then
            DataGridView1.BeginEdit(True)
            If DataGridView1.CurrentRow.Cells(12).Value.ToString <> "" Then
                Button2.Enabled = True

            End If
            If DataGridView1.CurrentCell.Value.ToString = "" Then
                Button1.Enabled = True
                Button2.Enabled = False
            End If
        End If
        If DataGridView1.CurrentCell.ReadOnly = False And Form2.Button2.BackColor = Color.CornflowerBlue Then
            If DataGridView1.RowCount > 10 Then
                Me.WindowState = FormWindowState.Normal
                Me.Size = New Size(1936, 740)

                Me.Location = New Point(-7, 0)
            End If
            Dim P As Process()
            P = Process.GetProcessesByName("osk.exe")

            If P.Count = 0 Then
                osk()

            End If
        End If

    End Sub

    Public Declare Auto Function GetSystemMetrics Lib "user32.dll" (ByVal smIndex As Integer) As Integer

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Button2.BackColor = Color.CornflowerBlue
        'For n = 0 To DataGridView1.RowCount - 1
        '    If DataGridView1.Rows(n).Cells(12).Value.ToString = "" Then
        '        DataGridView1.Rows(n).Cells(8).ReadOnly = True
        '        DataGridView1.Rows(n).Cells(9).ReadOnly = True
        '        DataGridView1.Rows(n).Cells(10).ReadOnly = True
        '    End If
        'Next
        Button1.Enabled = True
        'For q = 0 To DataGridView1.RowCount - 1
        '    DataGridView1.Rows(q).Cells(8).ReadOnly = True
        '    DataGridView1.Rows(q).Cells(9).ReadOnly = True
        '    DataGridView1.Rows(q).Cells(10).ReadOnly = True
        'Next
        'For z = 8 To 10
        '    DataGridView1.CurrentRow.Cells(z).ReadOnly = False
        'Next
        readonlycells(True)
    End Sub

    'Const SM_MAXIMUMTOUCHES As Integer = 95
    'Public Function HasTouchscreen() As Boolean
    '    Dim numberOfTouches = GetSystemMetrics(SM_MAXIMUMTOUCHES)
    '    Return numberOfTouches > 0
    'End Function

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        'killer("WindowsInternal.ComposableShell.Experiences.TextInput.InputApp")
        killer("osk")
    End Sub


    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        d2 = d2.AddDays(1)
        If d2.DayOfWeek = DayOfWeek.Saturday Then
            d2 = d2.AddDays(2)
        End If
        If d2.DayOfWeek = DayOfWeek.Sunday Then
            d2 = d2.AddDays(1)
        End If
        If Case1 = 2 Then

            If d2 <= Form3.DateTimePicker3.Value.Date Then


                TextBox4.Text = d2.ToShortDateString

                TODATETIME(d2)

                DataGridView1.Columns.Clear()
                loadgrid("SELECT eidi.code,eidi.perigrafi,pelproms.eponymia,qty,actiondescr,hrs,mins,prodreal2.final_qty,prodreal2.final_hrs,prodreal2.final_mins,prodscheduleline.id,prodreal2.idprodline FROM prodscheduleline LEFT JOIN eidi ON prodscheduleline.eidos=eidi.eidos LEFT JOIN pelproms ON eidi.pelprom=pelproms.pelprom LEFT JOIN prodreal2 ON prodscheduleline.id=prodreal2.idprodline WHERE prodscheduleline.fdate='" & date6 & "' AND prodline='" & grammi & "'", Me.DataGridView1)

                gridlayout()
            Else
                d2 = d2.AddDays(-1)

            End If
        End If
        If Case1 = 3 Then

            If d2 <= Form3.DateTimePicker4.Value.Date Then

                TextBox4.Text = d2.ToShortDateString

                TODATETIME(d2)

                DataGridView1.Columns.Clear()
                loadgrid("SELECT eidi.code,eidi.perigrafi,pelproms.eponymia,qty,actiondescr,hrs,mins,prodreal2.final_qty,prodreal2.final_hrs,prodreal2.final_mins,prodscheduleline.id,prodreal2.idprodline FROM prodscheduleline LEFT JOIN eidi ON prodscheduleline.eidos=eidi.eidos LEFT JOIN pelproms ON eidi.pelprom=pelproms.pelprom LEFT JOIN prodreal2 ON prodscheduleline.id=prodreal2.idprodline WHERE prodscheduleline.fdate='" & date6 & "' AND prodline='" & grammi & "' AND prodscheduleline.id NOT IN (SELECT idprodline FROM prodreal2)", Me.DataGridView1)
                gridlayout()
            Else
                d2 = d2.AddDays(-1)
            End If

        End If
        If Case1 = 4 Then

            If d2 <= Form3.DateTimePicker7.Value.Date Then

                TextBox4.Text = d2.ToShortDateString

                TODATETIME(d2)

                DataGridView1.Columns.Clear()
                loadgrid("SELECT eidi.code,eidi.perigrafi,pelproms.eponymia,qty,actiondescr,hrs,mins,prodreal2.final_qty,prodreal2.final_hrs,prodreal2.final_mins,prodscheduleline.id,prodreal2.idprodline FROM prodscheduleline LEFT JOIN eidi ON prodscheduleline.eidos=eidi.eidos LEFT JOIN pelproms ON eidi.pelprom=pelproms.pelprom LEFT JOIN prodreal2 ON prodscheduleline.id=prodreal2.idprodline WHERE prodscheduleline.fdate='" & date6 & "' AND prodline='" & grammi & "' AND date_ins>prodscheduleline.fdate", Me.DataGridView1)
                gridlayout()
            Else
                d2 = d2.AddDays(-1)
            End If

        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        d2 = d2.AddDays(-1)
        If d2.DayOfWeek = DayOfWeek.Saturday Then
            d2 = d2.AddDays(-1)
        End If
        If d2.DayOfWeek = DayOfWeek.Sunday Then
            d2 = d2.AddDays(-2)
        End If
        If Case1 = 2 Then

            If d2 >= Form3.DateTimePicker2.Value.Date Then
                TextBox4.Text = d2.ToShortDateString
                TODATETIME(d2)

                DataGridView1.Columns.Clear()
                loadgrid("SELECT eidi.code,eidi.perigrafi,pelproms.eponymia,qty,actiondescr,hrs,mins,prodreal2.final_qty,prodreal2.final_hrs,prodreal2.final_mins,prodscheduleline.id,prodreal2.idprodline FROM prodscheduleline LEFT JOIN eidi ON prodscheduleline.eidos=eidi.eidos LEFT JOIN pelproms ON eidi.pelprom=pelproms.pelprom LEFT JOIN prodreal2 ON prodscheduleline.id=prodreal2.idprodline WHERE prodscheduleline.fdate='" & date6 & "' AND prodline='" & grammi & "'", Me.DataGridView1)
                gridlayout()
            Else
                d2 = d2.AddDays(1)
            End If
        End If

        If Case1 = 3 Then

            If d2 >= Form3.DateTimePicker5.Value.Date Then

                TextBox4.Text = d2.ToShortDateString
                TODATETIME(d2)

                DataGridView1.Columns.Clear()
                loadgrid("SELECT eidi.code,eidi.perigrafi,pelproms.eponymia,qty,actiondescr,hrs,mins,prodreal2.final_qty,prodreal2.final_hrs,prodreal2.final_mins,prodscheduleline.id,prodreal2.idprodline FROM prodscheduleline LEFT JOIN eidi ON prodscheduleline.eidos=eidi.eidos LEFT JOIN pelproms ON eidi.pelprom=pelproms.pelprom LEFT JOIN prodreal2 ON prodscheduleline.id=prodreal2.idprodline WHERE prodscheduleline.fdate='" & date6 & "' AND prodline='" & grammi & "' AND prodscheduleline.id NOT IN (SELECT idprodline from prodreal2)", Me.DataGridView1)
                gridlayout()
            Else
                d2 = d2.AddDays(1)
            End If

        End If
        If Case1 = 4 Then

            If d2 >= Form3.DateTimePicker6.Value.Date Then
                TextBox4.Text = d2.ToShortDateString
                TODATETIME(d2)

                DataGridView1.Columns.Clear()
                loadgrid("SELECT eidi.code,eidi.perigrafi,pelproms.eponymia,qty,actiondescr,hrs,mins,prodreal2.final_qty,prodreal2.final_hrs,prodreal2.final_mins,prodscheduleline.id,prodreal2.idprodline FROM prodscheduleline LEFT JOIN eidi ON prodscheduleline.eidos=eidi.eidos LEFT JOIN pelproms ON eidi.pelprom=pelproms.pelprom LEFT JOIN prodreal2 ON prodscheduleline.id=prodreal2.idprodline WHERE prodscheduleline.fdate='" & date6 & "' AND prodline='" & grammi & "' AND date_ins>prodscheduleline.fdate", Me.DataGridView1)
                gridlayout()
            Else
                d2 = d2.AddDays(1)
            End If
        End If
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click

        Me.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Form4.Close()
    End Sub

    Private Sub Form1_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
        If Button2.BackColor = Color.CornflowerBlue Then
            Button2.BackColor = DefaultBackColor
            readonlycells(False)
        End If
    End Sub

    Private Sub DataGridView1_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DataGridView1.DataError

        MessageBox.Show("Ελέγξτε τις τιμές που προσπαθείτε να καταταχωρήσετε")

        inputerror = True
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellDoubleClick
        Form5.TextBox1.Text = DataGridView1.CurrentRow.Cells(1).Value.ToString
        Form5.eidos_code = DataGridView1.CurrentRow.Cells(0).Value
        Form5.TextBox5.Text = DataGridView1.CurrentRow.Cells(0).Value.ToString
        Form5.Show()
        Me.Close()
    End Sub
End Class
