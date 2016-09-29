<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="MapAreaGroup.aspx.cs"
    Inherits="NetRadio.LocatingMonitor.Organize.__MapAreaGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <div id="content">
    </div>

    <script type="text/javascript" language="javascript">   
    var isClickOK=false;
	function __show(groupID)
	{
	    isClickOK=false;
	    $("hidCurrentGroupID").value=groupID;
	    $("DivAreaGroup").style.display="block";
	    $("areas").innerHTML= "";
	    $("butOK").disabled=true;
	    $("butCancel").disabled=true;
	    $$("loading").showLoading("数据正在加载中。。");
	    NetRadio.LocatingMonitor.Organize.__MapAreaGroup.get_MapAreas(groupID,function(r){
            if(r.error==0)
            {
               var vs=r.value;
               var html="";
               var widthCount=4;
               var heightCount=Math.ceil(vs.length/widthCount);
               var lastCount=widthCount - vs.length%widthCount;
               html+='<table width="100%" border="0" cellspacing="0" cellpadding="0">';
               for(var  h=0,c=0; h<heightCount; h++)
               {
                   html+='<tr>';
                   for(var i=0 ;c<vs.length && i<widthCount;i++,c++)
                   {
                      html+='<td><input name="areasINPUT" type="checkbox" value="' + vs[c].areaid + '" '+
                            (vs[c].selected.toString()=="1" ? 'checked="checked"': '')+ ' />'+vs[c].areaname+"</td>";
                   }
                   if(h==heightCount-1)
                   {
                     for(var i=0; i<lastCount; i++)
                     {
                       html+='<td>&nbsp;</td>';
                     }
                   }
                   html+='</tr>';
               }
               html+='</table>';
                $("areas").innerHTML=html;
                $("butOK").disabled=false;
	            $("butCancel").disabled=false;
	            $("loading").innerHTML="";
            }
            else
            {
               $("butCancel").disabled=false;
               $("loading").innerHTML="<font color=red>× "+r.errorText+"</font>";
            }            
           }); 
	}
	function butOK_click()
	{
	    $("butOK").disabled=true;
	    $("butCancel").disabled=true;
	    $$("loading").showLoading("数据正在提交中。。");
	    var areas= document.getElementsByName("areasINPUT");
	    var groupID=$("hidCurrentGroupID").value;
	    var newAreaIDs="";
	    for(var i=0;i< areas.length;i++)
	    { 
	      if(areas.item(i).checked==true)
	      {
	        if(newAreaIDs=="")
	        {
	            newAreaIDs+=areas.item(i).value;
	        }
	        else
	        {
	            newAreaIDs+=","+areas.item(i).value;
	        }
	      }
	    }	    
	    NetRadio.LocatingMonitor.Organize.__MapAreaGroup.set_AreaGroup(groupID,newAreaIDs,function(r){
            if(r.error==0)
            {                           
                $("butOK").disabled=false;
	            $("butCancel").disabled=false;
	            $("loading").innerHTML="<font color=red>√ 数据已经成功提交</font>"; 
	            window.setTimeout(function(){ $("loading").innerHTML="";},1000*1); 
	            isClickOK=true;             
            }
            else
            {
                $("butOK").disabled=false;
                $("butCancel").disabled=false;
                $("loading").innerHTML="<font color=red>× "+r.errorText+"</font>";
            }           
           }); 
	}
	
	function butCancel_click()
	{
	   $("DivAreaGroup").style.display="none";
	   if(isClickOK)
	   {
	      ____load();
	   }
	}
	
	function ____load()
	{
	   NetRadio.LocatingMonitor.Organize.__MapAreaGroup.get_collapseView(function(r){
        if(r.error==0)
        {
            $("content").innerHTML=    r.value;
        }
        else
        {
            $("content").innerHTML=  r.errorText;
        }
       });  
	}	
    
     //页面初始化
    function page_Load()
    {
        $("content").innerHTML= "";
       $$("content").showLoading("数据正在加载中。。");
       ____load();
    }
    </script>

</asp:Content>
