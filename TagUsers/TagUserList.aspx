<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/Default.Master"
    AutoEventWireup="true" CodeBehind="TagUserList.aspx.cs" Inherits="NetRadio.LocatingMonitor.TagUsers.__TagUserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server">
    <div id="content0">
    </div>
    <div id="content1">
    </div>

    <script type="text/javascript" language="javascript">	
	   var _action=1;
	   var userType=1;
	   var _keyword ="";
	   var _extandId= "";
	   var _jailRoomSelectedIndex=0;
	   var _jailRoomSelectedValue="";
	   var tagBindingSelectedValue="0";
	   var tagOnlineSelectedValue="0";
	   var currentPageIndex=1;	  	 
   
    
    //页面初始化
    function page_Load()
    {
           <% 
           if (NetRadio.Business.BusSystemConfig.IsLoadHostInfo())
           {
           %>
             importUsers_onclick();
           <% }else { %>
             load();
           <%}%>
       
    }
     
     function load()
     {
      _action=1;
	   userType=parseInt( request("type"));
	   if( isNaN(userType)) userType=1;
	  
	   $$("content0").showLoading("数据正在加载中。。");
       NetRadio.LocatingMonitor.TagUsers.__TagUserList.get_TagUserList0(_action,userType,function(r){
            if(r.error==0)
            {
                $("content0").innerHTML=    r.value; 
                searchButton_click(1);
            }
            else
            {
                $("content0").innerHTML=  r.errorText;
            }
           });
     }
        
    //搜索
    function searchButton_click(pageIndex)
    {    
        currentPageIndex=pageIndex;
	    _action=2;
	    userType=parseInt( request("type"));
	   if( isNaN(userType)) userType=1;
	   _keyword = $input_GetAttribute("ctl00_keyword","value");
	   _extandId= $input_GetAttribute("ctl00_extandId","value");	  
	   if( isNaN(_jailRoomSelectedIndex)) userType=0;
	   if ($("ctl00_jailRoom") != null)
	   {
	    _jailRoomSelectedIndex=parseInt( $select_GetSelectedOption("ctl00_jailRoom").index);
	   _jailRoomSelectedValue=$select_GetSelectedOption("ctl00_jailRoom").value;	
	   }   
	   tagBindingSelectedValue=$radio_GetCheckedRadio("ctl00$tagBinding").value;
	   tagOnlineSelectedValue=$radio_GetCheckedRadio("ctl00$tagOnline").value;	   
	   
	     $("content1").innerHTML="";
	     $$("content1").showLoading("数据正在加载中。。");
        NetRadio.LocatingMonitor.TagUsers.__TagUserList.get_TagUserList1(_action,userType,_keyword ,_extandId, _jailRoomSelectedIndex, _jailRoomSelectedValue, tagBindingSelectedValue,tagOnlineSelectedValue,pageIndex,function(r){
            if(r.error==0)
            {            
                $("content1").innerHTML=    r.value;
            }
            else
            {
                $("content1").innerHTML=  r.errorText;
            }
           });       
    }   
       //分页按钮
	   function pagerScriptLinkButton_click(id,pageIndex)
		{
		    searchButton_click(pageIndex);		
		}	
		//标签绑定
		function bindTag_click(tagId,userID,userNO)
		{
		      <% 
		        if (NetRadio.Business.BusSystemConfig.IsAutoSelectStrongestRssiTag() == false)
		        {
              %>
             SelectTag.FunctionNo="01";
		     SelectTag.SelectedTagIDs=tagId;
             SelectTag.UserID=userID;
             SelectTag.RegFunction=function(){searchButton_click(currentPageIndex);	};
		     SelectTag.Show();
             <% 
              } 
             else
              {
             %> 
		     SelectStrongestRssiTag.Init();
		     SelectStrongestRssiTag.UserID=userID;
		     SelectStrongestRssiTag.SetTitle("正在修改编号："+ userNO.toString() +"<br/><font color=gray>..................................................</font><br/>");
		     SelectStrongestRssiTag.Show();
		     SelectStrongestRssiTag.RegFunction=function(){searchButton_click(currentPageIndex);	};
		     <% 
              }             
             %>
		 }	
		 
		 function importUsers(sucCallBack,failCallBack)
		 {
		  NetRadio.LocatingMonitor.TagUsers.__TagUserList.ImportUsers(function(r){
            if(r.error==0)
            {            
                sucCallBack();
            }
            else
            {
               failCallBack();
               $("content0").innerHTML=""; 
               $$("content0").showLoading(r.errorText);
            }
           });
		 
		 }	
		 
		  function importUsers_onclick()
		  {
		    SelectStrongestRssiTag.StopTimer();
		    $("content0").innerHTML=""; 
		    $$("content0").showLoading("数据导入中，请稍侯。。");
            $("content1").innerHTML=""; 
            window.setTimeout(function(){
             importUsers(function(){$("content0").innerHTML="";load();});
            },2);
		  }

    </script>

</asp:Content>
