<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="intern.aspx.vb" Inherits="WebApplication1.intern" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <link rel="stylesheet" href="//intranet/skin/medovgu/css/standard_screen.css?v=2" type="text/css" title="OvGU" media="screen" />
 <link rel="stylesheet" href="//intranet/skin/medovgu/css/med/style.css?v=3" type="text/css" title="OvGU" media="screen" />
 <link rel="stylesheet" href="//intranet/skin/medovgu/css/lightbox.css" type="text/css" title="OvGU" media="screen" />
 <link rel="stylesheet" href="//intranet/skin/medovgu/css/standard_print.css" type="text/css" media="print" />
 <link rel="alternate stylesheet" href="//intranet/skin/medovgu/css/barrierefrei.css" title="Barrierefrei" type="text/css" media="screen" />
 <link rel="stylesheet" href="//intra4/css/defaultTablesorter.css" type="text/css"/>

<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Interne Stellenausschreibung-Admin</title>
<style type="text/css">
* { font-family:"Lucida Grande","Lucida Sans Unicode","Lucida Sans",sans-serif;font-size:9pt}

</style>

</head>
    
<body>
    
    <form id="form1" runat="server">
        
    <asp:Label ID="ergebnis" runat="server" />
        
    <div>
        <h2>Interne Stellenausschreibung Admin-Bereich</h2> <!-- Ueberschrifft -->
                       
    </div>
    
    <div>  
        <!-- erstellt die Tablle (GridView) 
                der Bereich <columns> gibt die Spalten an aus der die abgefragten Eintraege angezeigt werden sollen
                die Button "anzeigen" und "verbergen" funktionieren auch ohne aktivierter checkbox-->
        
        <asp:GridView 
          ID="grdResults" 
          runat="server" 
          PagerStyle-BorderStyle="None" 
          RowStyle-BorderStyle="None" 
          SelectedRowStyle-BorderStyle="None" 
          HeaderStyle-BorderStyle="None" 
          GridLines="Horizontal" 
          FooterStyle-BorderStyle="None" 
          EmptyDataRowStyle-BorderStyle="None" 
          BorderStyle="none"
          OnRowCommand="grdResults_RowCommand"
          Onrowdatabound="grdResults_RowDataBound"
          AutoGenerateEditButton="false"
          >
          
          <columns>
              
            <asp:boundfield datafield="id" htmlencode="false" headertext="Nummer" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="Black"/>
            <asp:BoundField DataField="docname" HtmlEncode="false" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" visible="true" HeaderStyle-ForeColor="Black"/>
            
            <asp:boundfield datafield="displayname" htmlencode="false" headertext="Anzeige" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="Black"/>
            <asp:boundfield datafield="author" htmlencode="false" headertext="Autor" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="Black"/>
            <asp:boundfield datafield="oeffentlich" htmlencode="false" headertext="gültig bis" ItemStyle-Font-Size="0" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="Black"/>
            <asp:boundfield datafield="gueltigbis" htmlencode="false" headertext="" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="Black" />

            <asp:ButtonField ButtonType="Button" CommandName="anzeigen" HeaderText="" Text="anzeigen" ItemStyle-HorizontalAlign="center" />
            <asp:ButtonField ButtonType="Button" CommandName="verbergen" HeaderText="" Text="verbergen" ItemStyle-HorizontalAlign="center" />
            <asp:ButtonFIeld ButtonType="image" CommandName="loeschen" HeaderText="" imageurl="http://intra4/image/delete.png" ItemStyle-HorizontalAlign="Center" />
            <asp:ButtonFIeld ButtonType="image" CommandName="speichern" HeaderText="" ImageUrl="http://intra4/image/ok.jpg" ItemStyle-HorizontalAlign="Center" />
                                   
          </columns>          
      </asp:GridView>
     
        </div>
        
    </form>
</body>
</html>
