Imports System.Data.SqlClient
'This code and database were based on a sample program provided by Prof. Patricia NcDermott-Wells
'in the COP 4005 course, from Jan. 10, 2023 to May 3, 2023
'Changes: new functions and stored procedures added in the frmHenry form that display tables for branch, inventory, publisher and wrote tables
'as well as new functions that allow users to clear their screens and fill the whole portion of the displaygridview objec
Module ModDB
    Public objSQLConn As SqlConnection
    Public objSQLCommand As SqlCommand
    Public gstrConn As String = "Data Source=(LocalDB)\
MSSQLLocalDB;AttachDbFilename=C:\Users\Roberto Casadiego\source\repos\HBooksSample\HBooks_2019.mdf;Integrated Security=True"
End Module
'C:\Users\Roberto Casadiego\source\repos\HBooksSample'