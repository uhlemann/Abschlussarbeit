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
<script type="text/javascript" src="scripts/jquery-1.7.1.js"> </script> 
<script type="text/javascript" src="scripts/jquery.tablesorter.js"> </script> 
<script type="text/javascript" src="scripts/jquery.tablesorter.pager.js"> </script> 
<script type="text/javascript" src="scripts/jquery.tablesorter.widgets.js"></script>
<script type="text/javascript">
    $(function () {

        // **********************************
        //  Description of ALL pager options
        // **********************************
        var pagerOptions = {

            container: $(".pager"),
            dateFormat: "ddmmyyyy", // set the default date format
            ajaxUrl: null,
            customAjaxUrl: function (table, url) { return url; },
            ajaxProcessing: function (ajax) {
                if (ajax && ajax.hasOwnProperty('data')) {
                    // return [ "data", "total_rows" ];
                    return [ajax.total_rows, ajax.data];
                }
            },
            output: '{startRow} to {endRow} ({totalRows})',
            updateArrows: true,
            page: 0,
            size: 10,
            fixedHeight: true,
            removeRows: false,
            cssNext: '.next', // next page arrow
            cssPrev: '.prev', // previous page arrow
            cssFirst: '.first', // go to first page arrow
            cssLast: '.last', // go to last page arrow
            cssGoto: '.gotoPage', // select dropdown to allow choosing a page
            cssPageDisplay: '.pagedisplay', // location of where the "output" is displayed
            cssPageSize: '.pagesize', // page size selector - select dropdown that sets the "size" option
            cssDisabled: 'disabled' // Note there is no period "." in front of this class name

        };
        $.tablesorter.addParser({
            // set a unique id 
            id: 'labelzahl',
            is: function (s) {
                // return false so this parser is not auto detected 
                return false;
            },
            format: function (s) {
                return s.split(" ")[0];
            },
            // set type, either numeric or text
            type: 'numeric'
        });

        $("table")

          // Initialize tablesorter
          // ***********************
            .tablesorter({
                headers: {
                    0: { sorter: "shortDate", dateFormat: "ddmmyyyy" }
                },
                sortList: [[0, 0], [1, 0]],
                widthFixed: true,
                widgets: ['zebra']
            })


          // bind to pager events
          // *********************
          .bind('pagerChange pagerComplete pagerInitialized pageMoved', function (e, c) {
              var msg = '"</span> event triggered, ' + (e.type === 'pagerChange' ? 'going to' : 'now on') +
                ' page <span class="typ">' + (c.page + 1) + '/' + c.totalPages + '</span>';
              $('#display')
                .append('<li><span class="str">"' + e.type + msg + '</li>')
                .find('li:first').remove();
          })

          // initialize the pager plugin
          // ****************************
          .tablesorterPager(pagerOptions);


        // Destroy pager / Restore pager
        // **************
        $('button:contains(Destroy)').click(function () {
            var $t = $(this);
            if (/Destroy/.test($t.text())) {
                $('table').trigger('destroy.pager');
                $t.text('Restore Pager');
            } else {
                $('table').tablesorterPager(pagerOptions);
                $t.text('Destroy Pager');
            }
        });

        // Disable / Enable
        // **************
        $('.toggle').click(function () {
            var mode = /Disable/.test($(this).text());
            $('table').trigger((mode ? 'disable' : 'enable') + '.pager');
            $(this).text((mode ? 'Enable' : 'Disable') + 'Pager');
        });
        $('table').bind('pagerChange', function () {
            // pager automatically enables when table is sorted.
            $('.toggle').text('Disable Pager');
        });

    });
  </script>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Interne Stellenausschreibung-Admin</title>
<style type="text/css">
* { font-family:"Lucida Grande","Lucida Sans Unicode","Lucida Sans",sans-serif;font-size:9pt}
/* pager wrapper, div */
.tablesorter-pager { padding: 5px;}
/* pager wrapper, in thead/tfoot */
td.tablesorter-pager {
  background-color: #e6eeee;
  margin: 0; /* needed for bootstrap .pager gets a 18px bottom margin */
}
/* pager navigation arrows */
.tablesorter-pager img {
  vertical-align: middle;
  margin-right: 2px;
  cursor: pointer;
}

/* pager output text */
.tablesorter-pager .pagedisplay {
  padding: 0 5px 0 5px;
  width: 50px;
  text-align: center;
  font-weight: bold;
}
div.trblumen
   {
   opacity:0.6;
   filter:alpha(opacity=60); /* For IE8 and earlier */
   }

/* pager element reset (needed for bootstrap) */
.tablesorter-pager select {
  margin: 0;
  padding: 0;
}

/*** css used when "updateArrows" option is true ***/
/* the pager itself gets a disabled class when the number of rows is less than the size */
.tablesorter-pager.disabled {
  display: none;
}
/* hide or fade out pager arrows when the first or last row is visible */
.tablesorter-pager .disabled {
  /* visibility: hidden */
  opacity: 0.5;
  filter: alpha(opacity=50);
  cursor: default;
}

#eingabe {
    margin-left: 2em;
}
</style>

</head>
    
<body>
    
    <form id="form1" runat="server">
        
    <asp:Label ID="ergebnis" runat="server" />
        
    <div>
        <h2>Interne Stellenausschreibung Admin-Bereich</h2> <!-- Ueberschrifft -->
                       
    </div>
    <div id="eingabe">
        Nummer:<asp:TextBox ID="TB_ID" runat="server" />
        Anzeige:<asp:TextBox ID="TB_Anzeige" runat="server" />
        Autor:<asp:TextBox ID="TB_Author" runat="server" />
        gültig bis:<asp:TextBox ID="TB_gueltig" runat="server" />
        <asp:Button ID="btn_ueber" runat="server" Text="Übernehmen" OnClick="uebernehmen_Click"/>
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
            <asp:boundfield datafield="oeffentlich" htmlencode="false" headertext=""  HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="Black"/>
            <asp:boundfield datafield="gueltigbis" htmlencode="false" headertext="gültig bis" HeaderStyle-HorizontalAlign="Left" HeaderStyle-ForeColor="Black"/>

            <asp:ButtonField ButtonType="Button" CommandName="anzeigen" HeaderText="" Text="anzeigen" ItemStyle-HorizontalAlign="center" />
            <asp:ButtonField ButtonType="Button" CommandName="verbergen" HeaderText="" Text="verbergen" ItemStyle-HorizontalAlign="center" />
            <asp:ButtonFIeld ButtonType="Button" CommandName="loeschen" HeaderText="" Text="löschen" ItemStyle-HorizontalAlign="Center" />
                                   
          </columns>          
      </asp:GridView>
     
        </div>
        
    </form>
</body>
</html>
