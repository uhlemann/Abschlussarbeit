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
    Dim button As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        readDirAndWrite()

        lesen()


    End Sub

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

        'ermittelt die Reihe in der ein Burron betaetigt wurde
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = grdResults.Rows(index)
        Dim heute As Date = Date.Now
        Dim bisdate As Date = row.Cells(5).Text

        'die Reihe wird vorher durch "row" ermittelt
        '0 = erste Spallte (ID) 
        Dim id As Integer = row.Cells(0).Text
        Dim doc As String = row.Cells(1).Text
        Dim erw As String = Right(doc, 3)

        Dim tb_dispname As TextBox = row.Cells(2).Controls(0)
        Dim dispname As String = tb_dispname.Text

        Dim tb_auth As TextBox = row.Cells(3).Controls(0)
        Dim auth As String = tb_auth.Text

        Dim tb_datum As TextBox = row.Cells(5).Controls(0)
        Dim datum As String = tb_datum.Text

        'uebergibt den Wert der in die Spalte "anzeigen" eingetragen werden soll
        Dim anz As String = "anzeigen"
        Dim ver As String = "verbergen"
        Dim pfad As String = "X:\Stellen\"
        Dim pfad2 As String = "X:\Stellen\pdf\"

        'ermittelt welcher Button betaetigt wurde
        Select Case e.CommandName

            Case "anzeigen"
                If bisdate > heute Then
                    strSQL = "UPDATE Stellen SET oeffentlich='" & anz & "', mVerbergen='" & anz & "' WHERE ID= " & id & " "
                    UpdateSQL()
                End If

            Case "verbergen"
                strSQL = "UPDATE Stellen SET oeffentlich='" & ver & "', mVerbergen='" & ver & "' WHERE ID= " & id & " "
                UpdateSQL()

            Case "loeschen"
                strSQL = "DELETE FROM Stellen WHERE ID = " & id & " "
                UpdateSQL()

            Case "speichern"
                'e.Item.FindControl("PriceLabel")

            Case "datei"
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
            Dim TB_displayname = New TextBox
            TB_displayname.Style.Add("width", "120px")
            TB_displayname.ID = "TBname-" & id
            Try
                TB_displayname.Text = drv("displayname")
            Catch ex As Exception
                TB_displayname.Text = ""
            End Try
            e.Row.Cells(2).Controls.Add(TB_displayname)

            Dim TB_author = New TextBox
            TB_author.Style.Add("width", "120px")
            TB_author.ID = "TBauthor-" & id
            Try
                TB_author.Text = drv("author")
            Catch ex As Exception
                TB_author.Text = ""
            End Try
            e.Row.Cells(3).Controls.Add(TB_author)

            Dim TB_date = New TextBox
            TB_date.Style.Add("width", "120px")
            TB_date.ID = "TBdate-" & id
            Try
                TB_date.Text = drv("gueltigbis")
            Catch ex As Exception
                TB_date.Text = ""
            End Try
            e.Row.Cells(5).Controls.Add(TB_date)

            e.Row.Cells(4).Visible = False

            'anzeigen
            If InStr(e.Row.Cells(4).Text, "anzeigen") >= 1 Then
                e.Row.Cells(6).Visible = False
                e.Row.Cells(7).Visible = True

                'verbergen
            Else
                e.Row.Cells(6).Visible = True
                e.Row.Cells(7).Visible = False
                'wenn verbergen dann Zeile grau faerben
                e.Row.Style.Add("color", "gray")
                e.Row.Font.Strikeout = True
            End If

            'wenn datum kleiner als aktuelles datum dannn trage "verbergen" in die spalte "oeffentlich" ein
            'Debug.Print(drv(5))
            Dim bisdate As Date = Date.Parse(drv(5))
            Dim heute As Date = Date.Now

            If bisdate < heute Then
                strSQL = "update stellen set oeffentlich ='" & ver & "' where id= '" & id & "' "
            End If
            UpdateSQL()

        End If

    End Sub

End Class
