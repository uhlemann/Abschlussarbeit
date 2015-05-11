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

    Sub updateSQL()
        Using cn As New OleDbConnection With
        {.ConnectionString = strConnectionString}
            Using cmd As New OleDbCommand With
              {.Connection = cn, .CommandText = strSQL}
                cn.Open()
                dtList.Load(cmd.ExecuteReader)
            End Using
        End Using
    End Sub

    Sub lesen()
        Dim show As String = "anzeigen"
        strSQL = "SELECT * FROM Stellen WHERE oeffentlich='" & show & "' ORDER BY id ASC"
        updateSQL()
    End Sub

    Public Sub grdResults_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        'ermittelt die Reihe in der ein Burron betaetigt wurde
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = grdResults.Rows(index)

        'die Reihe wird vorher durch "row" ermittelt
        '0 = erste Spallte (ID) 
        Dim doc As String = row.Cells(1).Text

        'uebergibt den Wert der in die Spalte "anzeigen" eingetragen werden soll
        Dim pfad As String = "X:\Stellen\"

        'ermittelt welcher Button betaetigt wurde
        Select Case e.CommandName

            Case "datei"
                Process.Start(pfad + doc)


        End Select
        updateSQL()
    End Sub

End Class
