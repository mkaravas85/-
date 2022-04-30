Imports MySql.Data.MySqlClient
Module Module1
    Public mysqlcon As MySqlConnection
    Public command As MySqlCommand
    Public reader As MySqlDataReader
    Public sda As New MySqlDataAdapter
    Public dt As New DataTable
    Public bs As New BindingSource
    Public queryerror As Boolean = False
    'Public constr = "server=192.168.0.241;userid=signet;password=enapass;database=dbtcan;port=3306"
    'Public constr = "server=192.168.1.10;userid=signet;password=enapass;database=dbtechnocan;port=33953;charset=utf8;"
    Public constr = "server=195.167.110.35;userid=remote;password=enapassremote!@#;database=dbtcan" ';port=33953;charset=utf8;"

    Public date5, date6 As String
    Public grammi As Integer = 0
    Public Sub runquery(ByVal query As String)
        mysqlcon = New MySqlConnection
        'mysqlcon.ConnectionString = "server=localhost;userid=root;password=12345;database=sigmix"
        mysqlcon.ConnectionString = constr

        Try
            mysqlcon.Open()
            command = New MySqlCommand(query, mysqlcon)
            reader = command.ExecuteReader
            mysqlcon.Close()
        Catch ex As Exception
            queryerror = True
            MessageBox.Show(ex.Message)
            Exit Sub
        End Try
        If mysqlcon.State = ConnectionState.Open Then
            mysqlcon.Close()
        End If
    End Sub
    Public Sub dtvaluetoset(ByVal query As String)
        mysqlcon = New MySqlConnection
        'mysqlcon.ConnectionString = "server=localhost;userid=root;password=12345;database=sigmix"
        mysqlcon.ConnectionString = constr
        dt.Reset()

        Try

            mysqlcon.Open()
                command = New MySqlCommand(query, mysqlcon)
                sda.SelectCommand = command
                sda.Fill(dt)
                bs.DataSource = dt

                sda.Update(dt)
                mysqlcon.Close()

        Catch ex As Exception
            queryerror = True
            MessageBox.Show(ex.Message)

        End Try

        If mysqlcon.State = ConnectionState.Open Then
            mysqlcon.Close()
        End If
    End Sub
    Public Sub loadgrid(ByVal query As String, dgv As DataGridView)
        mysqlcon = New MySqlConnection
        'mysqlcon.ConnectionString = "server=localhost;userid=root;password=12345;database=sigmix"
        mysqlcon.ConnectionString = constr
        Dim sda As New MySqlDataAdapter
        Dim dt As New DataTable
        Dim bs As New BindingSource
        Try
            mysqlcon.Open()
            command = New MySqlCommand(query, mysqlcon)
            sda.SelectCommand = command
            sda.Fill(dt)
            bs.DataSource = dt
            dgv.DataSource = bs
            sda.Update(dt)
            mysqlcon.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        If mysqlcon.State = ConnectionState.Open Then
            mysqlcon.Close()
        End If
    End Sub
End Module
