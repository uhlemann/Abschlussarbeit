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

    REM uebergibt Werte an eine Datenbank und verarbeitet diese
    Sub UpdateSQL()
        Using cn As New OleDbConnection With
        {.ConnectionString = strconnectionString}
            Using cmd As New OleDbCommand With
                {.Connection = cn, .CommandText = strSQL}
                cn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    REM liest eine Datenbank Tabelle aus
    Sub lesen()
        strSQL = "SELECT id, docname, displayname, author, oeffentlich, CONVERT(varchar(10),[gueltigbis], 104) as gueltigbis FROM Stellen ORDER BY id ASC"
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

    REM liest einen Ordner aus und schreibt die Eintraege in die Datenbank
    Sub readDirAndWrite()

        Dim tmpDoc As String
        Dim tmpDisp As String

        For Each item As String In IO.Directory.GetFiles("x:\Stellen")
            If IO.Path.GetExtension(item) = ".doc" Then
                tmpDoc = IO.Path.GetFileName(item)
                tmpDisp = IO.Path.GetFileName(item)

                strSQL = "IF NOT EXISTS (SELECT * FROM Stellen WHERE docname = '" & tmpDoc & "')" & _
                    "INSERT INTO Stellen (docname, displayname, author, oeffentlich, mVerbergen)" & _
                    "VALUES ('" & tmpDoc & "', '" & tmpDisp & "', 'author', 'verbergen', 'anzeigen')"

                UpdateSQL()

            End If
        Next
    End Sub

    Public Sub grdResults_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)

        REM ermittelt die Reihe in der ein Button betaetigt wurde
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = grdResults.Rows(index)
        Dim heute As Date = Date.Now
        Dim bisdate As Date = row.Cells(5).Text

        REM die Reihe wird vorher durch "row" ermittelt
        REM 0 = erste Spallte (ID) 
        Dim id As Integer = row.Cells(0).Text
        Dim doc As String = row.Cells(1).Text
        Dim erw As String = Right(doc, 3)

        REM Textbox fuer das Feld "displayname"
        Dim tb_dispname As TextBox = row.Cells(2).Controls(0)
        Dim dispname As String = tb_dispname.Text

        REM Textbox fuer das Feld "author"
        Dim tb_auth As TextBox = row.Cells(3).Controls(0)
        Dim auth As String = tb_auth.Text

        REM Textbox fuer das Feld "gueltigbis"
        Dim tb_datum As TextBox = row.Cells(5).Controls(0)
        Dim datum As String = tb_datum.Text

        Dim anz As String = "anzeigen"
        Dim ver As String = "verbergen"
        Dim pfad As String = "X:\Stellen\"
        Dim pfad2 As String = "X:\Stellen\pdf\"

        Select Case e.CommandName
            REM ermittelt welcher Button betaetigt wurde

            Case "anzeigen"
                REM das Dokument kann nur dann fuer den User angezeigt werden wenn,
                REM das heutige Datum kleiner ist als "bisdate"
                If bisdate > heute Then
                    strSQL = "UPDATE Stellen SET oeffentlich='" & anz & "', mVerbergen='" & anz & "' WHERE ID= " & id & " "
                    UpdateSQL()
                End If

            Case "verbergen"
                REM macht das Dokument fuer den User "unsichtbar"
                strSQL = "UPDATE Stellen SET oeffentlich='" & ver & "', mVerbergen='" & ver & "' WHERE ID= " & id & " "
                UpdateSQL()

            Case "loeschen"
                REM loescht den eintrag aus der Datenbank
                strSQL = "DELETE FROM Stellen WHERE ID = " & id & " "
                UpdateSQL()

            Case "speichern"
                REM uebernimmt die geanderten eintraege in die Datenbank
                strSQL = "UPDATE Stellen SET displayname = '" & dispname & "', author = '" & auth & "', gueltigbis = '" & datum & "' WHERE ID = " & id & " "
                UpdateSQL()

            Case "datei"
                REM gibt den Pfad zum Dokument an
                If erw = "doc" Then
                    Process.Start(pfad + doc)
                ElseIf erw = "pdf" Then
                    Process.Start(pfad2 + doc)
                Else
                    MsgBox("Datei nicht gefunden!")
                End If

        End Select

        lesen()

    End Sub

    Sub grdResults_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        REM anzeigen/verbergen Button
        Dim drv As DataRowView = e.Row.DataItem
        Dim ver As String = "verbergen"
        Dim anz As String = "anzeigen"
        Dim id As String = e.Row.Cells(0).Text
        Dim calendar = New Calendar

        If e.Row.RowType = DataControlRowType.DataRow Then

            REM erstellt Textboxen fuer die Spalte "displayname"
            Dim TB_displayname = New TextBox
            TB_displayname.Style.Add("width", "120px")
            TB_displayname.ID = "TBname-" & id
            Try
                TB_displayname.Text = drv("displayname")
            Catch ex As Exception
                TB_displayname.Text = ""
            End Try
            e.Row.Cells(2).Controls.Add(TB_displayname)

            REM erstellt Textboxen fuer die Spalte "author"
            Dim TB_author = New TextBox
            TB_author.Style.Add("width", "120px")
            TB_author.ID = "TBauthor-" & id
            Try
                TB_author.Text = drv("author")
            Catch ex As Exception
                TB_author.Text = ""
            End Try
            e.Row.Cells(3).Controls.Add(TB_author)

            REM erstellt Textboxen fuer die Spalte "gueltigbis"
            Dim TB_date = New TextBox
            TB_date.Style.Add("width", "120px")
            TB_date.ID = "TBdate-" & id
            Try
                TB_date.Text = drv("gueltigbis")
            Catch ex As Exception
                TB_date.Text = ""
            End Try
            e.Row.Cells(5).Controls.Add(TB_date)

            REM verbirgt die Spalte "oeffentlich"
            e.Row.Cells(4).Visible = False

            REM wenn "anzeigen" als Eintrag vorhanden ist wird der Button "verbergen" angezeigt
            If InStr(e.Row.Cells(4).Text, "anzeigen") >= 1 Then
                e.Row.Cells(6).Visible = False
                e.Row.Cells(7).Visible = True

                REM wenn "verbergen" als Eintrag vorhanden ist wird der Button "anzeigen" angezeigt
            Else
                e.Row.Cells(6).Visible = True
                e.Row.Cells(7).Visible = False
                REM wenn "verbergen" als Eintrag vorhanden ist, dann Zeile grau faerben und durchstreichen
                e.Row.Style.Add("color", "gray")
                e.Row.Font.Strikeout = True
            End If

            REM wenn datum kleiner als aktuelles datum dannn trage "verbergen" in die spalte "oeffentlich" ein
            Dim bisdate As Date = Date.Parse(drv(5))
            Dim heute As Date = Date.Now

            REM wenn das huetige Datum groesser als "bisdate" ist, wird "verbergen" in die Spalte oeffentlich eingetragen
            If bisdate < heute Then
                strSQL = "update stellen set oeffentlich ='" & ver & "' where id= '" & id & "' "
            End If
            UpdateSQL()

        End If

    End Sub

End Class
