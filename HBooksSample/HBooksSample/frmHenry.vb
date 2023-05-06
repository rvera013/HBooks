Imports System.Data.SqlClient
'This code and database were based on a sample program provided by Prof. Patricia NcDermott-Wells
'in the COP 4005 course, from Jan. 10, 2023 to May 3, 2023
'Changes: new functions and stored procedures added that display tables for branch, inventory, publisher and wrote tables
'as well as new functions that allow users to clear their screens and fill the whole portion of the displaygridview object
Public Class frmMain
    Private blnErrors As Boolean
    Private sqlDA As SqlDataAdapter
    Private dt As DataTable
    Private myDB As CDB
    Private sqlDR As SqlDataReader
    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles Me.Load
        myDB = New CDB
        If Not myDB.OpenDB Then
            Application.Exit()
        End If
        loadTableNames(cboTable)
        LoadSearchChoices(cboSearch)
    End Sub
    Private Sub loadTableNames(cbo As ComboBox)
        cbo.Items.Clear()
        sqlDR = myDB.functionGetDataReaderBySP("sp_getTableList", Nothing)
        While sqlDR.Read
            cbo.Items.Add(sqlDR.Item("name"))
        End While
        sqlDR.Close()
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        errP.Clear()
        blnErrors = False
        Dim strSP As String = ""
        objSQLCommand.Parameters.Clear()
        objSQLCommand.CommandType = CommandType.StoredProcedure
        If cboTable.SelectedIndex = -1 Then
            errP.SetError(cboTable, "You must make a selection here")
            blnErrors = True
        End If
        If blnErrors Then
            Exit Sub
        End If
        Select Case cboTable.SelectedItem.ToString
            Case "AUTHOR"
                strSP = "dbo.sp_getTableAuthor"
            Case "BOOK"
                strSP = "dbo.sp_getTableBook"
            Case "BRANCH"
                strSP = "dbo.sp_getTableBranches"
            Case "INVENTORY"
                strSP = "dbo.sp_getTableInventory"
            Case "PUBLISHER"
                strSP = "dbo.sp_getPublisherNames"
            Case "WROTE"
                strSP = "dbo.sp_getBooksWritten"
            Case Else
                MessageBox.Show("invalid selection. Please limit selections for showing data to either author or book", "program error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

        End Select
        sqlDA = myDB.getDataAdapter(strSP, Nothing)
        dt = New DataTable
        dgrInventory.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.Fill
        sqlDA.Fill(dt)
        dgrInventory.DataSource = dt
        dgrInventory.AutoGenerateColumns = True

    End Sub
    Private Sub LoadSearchChoices(cbo As ComboBox)
        cbo.Items.Add("Book Title")
        cbo.Items.Add("Author's last name")
        cbo.Items.Add("Branch Name")
        cbo.Items.Add("Publisher Name")
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim params As New ArrayList
        errP.Clear()
        If txtVal.Text.Length = 0 Then 'missing search value' 
            errP.SetError(txtVal, "You must enter a search value here")
            blnErrors = True
        End If
        If cboSearch.SelectedIndex = -1 Then
            errP.SetError(cboSearch, "You must make a selection here")
            blnErrors = True
        End If
        If blnErrors Then
            Exit Sub
        End If
        Select Case cboSearch.SelectedItem
            Case "Book Title"
                params.Add(New SqlParameter("@title", txtVal.Text))
                sqlDA = myDB.getDataAdapter("dbo.sp_getTitleList", params)
            Case "Author's last name"
                params.Add(New SqlParameter("@lastname", txtVal.Text))
                sqlDA = myDB.getDataAdapter("dbo.sp_getAuthorList", params)
            Case "Branch Name"
                params.Add(New SqlParameter("@branch", txtVal.Text))
                sqlDA = myDB.getDataAdapter("dbo.sp_getBranchList", params)
            Case "Publisher Name"
                params.Add(New SqlParameter("@Publisher", txtVal.Text))
                sqlDA = myDB.getDataAdapter("dbo.sp_GetInventoryInfo", params)
            Case Else
                MessageBox.Show("invalid table name in btnSearch click event", "program error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Select
        dt = New DataTable
        sqlDA.Fill(dt)
        dgrInventory.DataSource = dt
        dgrInventory.AutoGenerateColumns = True

    End Sub

    Private Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        If objSQLConn.State = ConnectionState.Open Then
            myDB.close_DB()
        End If
        Application.Exit()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        dgrInventory.DataSource = Nothing
        cboTable.SelectedIndex = -1
        cboSearch.SelectedIndex = -1
        errP.Clear()
        blnErrors = False
        Dim strSP As String = ""
        objSQLCommand.Parameters.Clear()
        objSQLCommand.CommandType = CommandType.StoredProcedure
    End Sub
End Class
