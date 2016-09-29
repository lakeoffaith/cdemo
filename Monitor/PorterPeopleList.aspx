<%@ Page Language="C#" MasterPageFile="~/Master/WebItem.Master" AutoEventWireup="true"
    CodeBehind="PorterPeopleList.aspx.cs" Inherits="NetRadio.LocatingMonitor.Monitor.__PorterPeopleList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <table style="width: 100%;" cellpadding="0" cellspacing="0" border="0" class="grid alternate fixed">
        <tr>
            <td height="200" width="20" valign="middle" align="center" style="">
                犯人列表
            </td>
            <td id="idCulprit" valign="top" align="left" style="border-left: solid 1px #e9e9e9;">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td height="200" width="20" valign="middle" align="center" style="">
                警察列表
            </td>
            <td id="idPolice" valign="top" align="left" style="border-left: solid 1px #e9e9e9;">
                &nbsp;
            </td>
        </tr>
    </table>

    <script>

        function getPoliceList() {
            if (!isCompletePolice)
                return;
            isCompletePolice = false;

            NetRadio.LocatingMonitor.Monitor.__PorterPeopleList.GetPoliceList(function(r) {
                if (r.error == 0) {
                    $("idPolice").innerHTML = r.value;
                }
                else {
                    $("idPolice").innerHTML = r.errorText;
                }
                isCompletePolice = true;
            });


        }

        function getCulpritList() {
            if (!isCompleteCulprit)
                return;
            isCompleteCulprit = false;

            NetRadio.LocatingMonitor.Monitor.__PorterPeopleList.GetCulpritList(function(r) {
                if (r.error == 0) {
                    $("idCulprit").innerHTML = r.value;
                }
                else {
                    $("idCulprit").innerHTML = r.errorText;
                }
                isCompleteCulprit = true;
            });


        }

        var isCompletePolice = true;
        var isCompleteCulprit = true;
        getPoliceList();
        getCulpritList();
        setInterval(function() { getPoliceList(); }, 1000);
        setInterval(function() { getCulpritList(); }, 1000);
      
    </script>

</asp:Content>
