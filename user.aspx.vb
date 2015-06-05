Imports System.IO
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Web.Services.Description


Public Class user
    Inherits System.Web.UI.Page

    Const strConnectionString = "Provider=XXXXX;Server=XXXXX;Database=XXXXX;User Id=XXXXX;Password=XXXXX;"

    Dim strSQL As String
    Dim dtList As DataTable = New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lesen()

    End Sub

    REM holt werte aus einer Datenbank und uebertraeg diese in "dtList"
    Sub selectSQL()
        dtList.Clear()
        Using cn As New OleDbConnection With
        {.ConnectionString = strConnectionString}
            Using cmd As New OleDbCommand With
              {.Connection = cn, .CommandText = strSQL}
                cn.Open()
                dtList.Load(cmd.ExecuteReader)
            End Using
        End Using
    End Sub

    REM liest eine Datenbank Tabelle aus
    Sub lesen()
        Dim show As String = "anzeigen"
        strSQL = "SELECT id, docname, displayname, author, CONVERT(varchar(10),[gueltigbis], 104) as gueltigbis, oeffentlich FROM Stellen WHERE oeffentlich='" & show & "' ORDER BY id ASC"
        selectSQL()

        If dtList.Rows.Count > 0 Then
            grdResults.Visible = True
            grdResults.AutoGenerateColumns = False
            grdResults.DataSource = dtList
            grdResults.DataBind()
            grdResults.UseAccessibleHeader = True

            Try
                grdResults.HeaderRow.TableSection = TableRowSection.TableHeader
            Catch ex As Exception
            End Try
        End If

    End Sub

    Public Sub grdResults_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        REM ermittelt die Reihe in der ein Burron betaetigt wurde
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = grdResults.Rows(index)

        REM Reihe wird vorher durch "row" ermittelt
        REM 0 = erste Spallte (ID) 
        Dim doc As String = row.Cells(1).Text

        REM uebergibt den Wert der in die Spalte "anzeigen" eingetragen werden soll
        Dim pfad As String = "X:\Stellen\"

        REM ermittelt welcher Button betaetigt wurde
        Select Case e.CommandName

            Case "datei"
                Process.Start(pfad + doc)

        End Select
    End Sub

End Class
