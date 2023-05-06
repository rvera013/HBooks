Imports System.Data.SqlClient
'This code and database were based on a sample program provided by Prof. Patricia NcDermott-Wells
'in the COP 4005 course, from Jan. 10, 2023 to May 3, 2023
'Changes: new functions and stored procedures added in the frmHenry form that display tables for branch, inventory, publisher and wrote tables
'as well as new functions that allow users to clear their screens and fill the whole portion of the displaygridview objec
Public Class CDB
    Public Function OpenDB() As Boolean
        objSQLCommand = New SqlCommand
        'Dim blnError
        Dim blnResult As Boolean
        If objSQLConn Is Nothing Then
            Try
                objSQLConn = New SqlConnection(gstrConn)
                objSQLConn.Open()
                blnResult = True
            Catch exOpenConnError As Exception
                MessageBox.Show("Cannot open database" & exOpenConnError.ToString(), "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                blnResult = False

            End Try
        Else
            If objSQLConn.State = ConnectionState.Open Then
                Return True
            Else
                Return False
            End If
        End If
        Return blnResult
    End Function
    Public Sub close_DB()
        Try
            objSQLConn.Close()
        Catch ex As Exception
            MessageBox.Show("error attempting to close database" & ex.ToString, "database error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Function functionGetDataReaderBySP(ByVal strSP As String, params As ArrayList) As SqlDataReader
        If Not OpenDB() Then
            'should eror log this problem
            Return Nothing


        End If
        objSQLCommand = New SqlCommand(strSP, objSQLConn)
        objSQLCommand.CommandType = CommandType.StoredProcedure
        If Not params Is Nothing Then
            For Each p As SqlParameter In params
                objSQLCommand.Parameters.Add(p)
            Next
        End If
        Try
            Return objSQLCommand.ExecuteReader
        Catch ex As Exception
            MessageBox.Show("failed to get reader" & ex.ToString, "database error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return Nothing
    End Function

    Public Function getDataAdapter(ByVal strSP As String, ByVal params As ArrayList) As SqlDataAdapter
        objSQLCommand = New SqlCommand(strSP, objSQLConn)
        Dim sqlDA As SqlDataAdapter
        objSQLCommand.CommandType = CommandType.StoredProcedure
        If Not params Is Nothing Then
            For Each p As SqlParameter In params
                objSQLCommand.Parameters.Add(p)
            Next
        End If
        Try
            sqlDA = New SqlDataAdapter(objSQLCommand)
            Return sqlDA
        Catch ex As Exception
            MessageBox.Show("failed to get adapter" & ex.ToString, "database error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        Return Nothing
    End Function
    Public Function execSP(ByVal strSP As String, ByRef params As ArrayList) As Integer
        If Not OpenDB() Then
            Return -1
        End If
        objSQLCommand = New SqlCommand(strSP, objSQLConn)
        Try
            If Not params Is Nothing Then
                For Each p As SqlParameter In params
                    objSQLCommand.Parameters.Add(p)
                Next
            End If
            Return objSQLCommand.ExecuteNonQuery
        Catch ex As Exception
            MessageBox.Show("failed to execute SP" & ex.ToString, "database error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return -1
        End Try
    End Function
    Public Function getSingleValueFromSP(ByVal strSP As String, ByRef params As ArrayList) As String
        Dim sqlDR As SqlDataReader = functionGetDataReaderBySP(strSP, params)
        Dim strResult As String
        If Not sqlDR Is Nothing Then
            If sqlDR.Read Then
                strResult = sqlDR.GetValue(0).ToString
                sqlDR.Close()
                Return strResult
            Else
                Return "-1"
                'got nothing
            End If
        End If
        Return "-2" 'failed to connect or even get a data reader'
    End Function

End Class
