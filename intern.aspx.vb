Imports System.IO
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Web.Services.Description


Public Class intern
    Inherits System.Web.UI.Page

    Const strConnectionString = "Provider=XXXXX;Server=XXXXX;Database=XXXXX;User Id=XXXXX;Password=XXXXX;"

    Dim strSQL As String
    Dim dtList As DataTable = New DataTable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        readDirAndWrite()
        lesen()

    End Sub

    Sub refresh()
        Response.Redirect(Request.RawUrl)
    End Sub
    
    Sub selectSQL()
        dtList.clear()
        Using cn As New OleDbConnection With
        {.ConnectionString = strConnectionString}
            Using cmd As New OleDbCommand With
                {.Connection = cn, .CommandText = strSQL}
                    cn.Open()
                    dtList.Load(cmd.ExecuteReader)
            End Using
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
        strSQL = "SELECT id, docname, displayname, author, oeffentlich, CONVERT(varchar(10),[gueltigbis], 104) AS gueltigbis FROM Stellen ORDER BY id ASC"
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
    End Sub

    Sub readDirAndWrite()

        Dim tmpDoc As String
        Dim tmpDisp As String

        For Each item As String In IO.Directory.GetFiles("x:\Stellen")
            If IO.Path.GetExtension(item) = ".doc" Then
                tmpDoc = IO.Path.GetFileName(item)
                tmpDisp = IO.Path.GetFileName(item)

                strSQL = "IF NOT EXISTS (SELECT * FROM Stellen WHERE docname = '" & tmpDoc & "')" & _
                    "INSERT INTO Stellen (docname, displayname, author, oeffentlich)" & _
                    "VALUES ('" & tmpDoc & "', '" & tmpDisp & "', 'author', 'verbergen')"

                updateSQL()
            End If
        Next
    End Sub

    Public Sub grdResults_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        'ermittelt die Reihe in der ein Burron betaetigt wurde
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = grdResults.Rows(index)

        'die Reihe wird vorher durch "row" ermittelt
        '0 = erste Spallte (ID) 
        Dim id As Integer = row.Cells(0).Text
        Dim doc As String = row.Cells(1).Text
        Dim erw As String = Right(doc, 3)

        'uebergibt den Wert der in die Spalte "anzeigen" eingetragen werden soll
        Dim anz As String = "anzeigen"
        Dim ver As String = "verbergen"
        Dim pfad As String = "X:\Stellen\"
        Dim pfad2 As String = "X:\Stellen\pdf\"

        'ermittelt welcher Button betaetigt wurde
        Select Case e.CommandName

            Case "anzeigen"
                strSQL = "UPDATE Stellen SET oeffentlich='" & anz & "' WHERE ID= " & id & " "
                updateSQL()

            Case "verbergen"
                strSQL = "UPDATE Stellen SET oeffentlich='" & ver & "' WHERE ID= " & id & " "
                updateSQL()

            Case "loeschen"
                strSQL = "DELETE FROM Stellen WHERE ID = " & id & " "
                updateSQL()

            Case "datei"
                If erw = "doc" Then
                    Process.Start(pfad + doc)
                ElseIf erw = "pdf" Then
                    Process.Start(pfad2 + doc)
                Else
                    MsgBox("Datei nicht gefunden!")
                End If

        End Select
        updateSQL()
        refresh()


    End Sub

    Sub grdResults_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        REM anzeigen/verbergen Button
        Dim drv As DataRowView = e.Row.DataItem
        Dim ver As String = "verbergen"
        Dim anz As String = "anzeigen"
        Dim id As String = e.Row.Cells(0).Text

        If e.Row.RowType = DataControlRowType.DataRow Then

            e.Row.Cells(4).Visible = False

            'anzeigen
            If InStr(e.Row.Cells(4).Text, "anzeigen") >= 1 Then
                e.Row.Cells(6).Visible = False
                e.Row.Cells(7).Visible = True

                'verbergen
            Else
                e.Row.Cells(6).Visible = True
                e.Row.Cells(7).Visible = False
                'wenn verbergen dann Zeile rot faerben
                e.Row.Style.Add("color", "#DF0101")
            End If

            'wenn datum kleiner als aktuelles datum dannn trage "verbergen" in die spalte "oeffentlich" ein
            'Debug.Print(drv(5))
            Dim bisdate As Date = Date.Parse(drv(5))
            Dim heute As Date = Date.Now
            Dim mVerbergen as String
            
            mVerbergen = strsql = "SELECT mVerbergen FROM stellen where id = '" & id & "' "
            slectSQL()
            
            If mVerbergen = "anzeigen" Then
                If bisdate < heute Then
                    strSQL = "update stellen set oeffentlich ='" & ver & "' where id= '" & id & "' "
                Else
                    strSQL = "update stellen set oeffentlich ='" & anz & "' where id= '" & id & "' "
                End If
                updateSQL()
            End If
        End If

    End Sub
    Sub uebernehmen_Click(ByVal sender As Object, e As System.EventArgs)

        Dim id As String = TB_ID.Text
        Dim author As String = TB_Author.Text
        Dim anzeige As String = TB_Anzeige.Text
        Dim gueltig As String = TB_gueltig.Text

        If TB_Author.Text.Length < 1 Then
        Else
            strSQL = "UPDATE Stellen SET author='" & author & "' WHERE ID= '" & id & "' "
        End If

        If TB_Anzeige.Text.Length < 1 Then
        Else
            strSQL = "UPDATE Stellen SET displayname='" & anzeige & "' WHERE ID= '" & id & "' "
        End If

        If TB_gueltig.Text.Length < 1 Then
        Else
            strSQL = "UPDATE Stellen SET gueltigbis='" & gueltig & "' WHERE ID= '" & id & "' "
        End If

        updateSQL()
        refresh()
    End Sub

End Class
