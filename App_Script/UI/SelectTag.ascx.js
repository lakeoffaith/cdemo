/***************************      外部可以调用的部分 Begin        ********************************/

var SelectTag=new Object();
//设置已经被选中的标签的ID值
SelectTag.SelectedTagIDs="";
//设置标签用户的ID
SelectTag.UserID="";
//当用户点击确定之后，控件关闭后回调的函数
SelectTag.RegFunction=function(){};
SelectTag.FunctionNo="";
//获取被选中的标签名称
SelectTag.GetSelectedNames=function(){return $input_GetValue("hidSelectedTagNames");};
//获取被选中的标签ID
SelectTag.GetSelectedIDs=function(){return $input_GetValue("hidSelectedTagIDs");};
//获取被选中的标签MAC
SelectTag.GetSelectedMACs=function(){return $input_GetValue("hidSelectedTagMACs");};

//设置被选中的标签信息
SelectTag.SetTip =function()// (SelectedIDs,userID,regFuntion)
{
   /********************/  
  $$("selectedList").showLoading("数据正在加载中。。");
  
  
  
  isClickOK=false; 
  //gRegFuntion=regFuntion;
  $("tags").innerHTML="";
  $("span_SelectedTags").innerHTML="";
  $("span_SelectedTagsCount").innerHTML="0";
  $("keyWord").value="关键字";
  $$("loading").showLoading("数据正在加载中。。");
  gTags=null;
  //gUserID=userID;
  
  var pageNum=1; 
  NetRadio.LocatingMonitor.Controls.__SelectTag.getTags(SelectTag.FunctionNo,SelectTag.SelectedTagIDs,pageNum,getKeyWord(),       function(r)
  {
    if(r.error==0)
	{
	    gTags=r.value[0];
	    pageNum=r.value[2];
	    var pageCount=r.value[1];
	    $("tags").innerHTML=  createTags(r.value[0],0);
	    set_span_SelectedTags();
	    set_selectedList_hidSelectedTagNames_hidSelectedTagIDs();
	    $("loading").innerHTML="";
	    
	     $select_ClearOptions("selectPage");	     
	     for(var i=1;i<=pageCount;i++)
	     {
	         var op=  document.createElement("option");
	         op.text="第 "+i+" 页";
	         op.value=i;
	         if(i==pageNum)
	         {
	           op.selected=true;
	         }
	         $select_AddOption("selectPage",op)
	     }
	     if(pageCount==0)
	     {
	       var op=  document.createElement("option");
	         op.text="第 0 页";
	         op.value=0;	        
	         $select_AddOption("selectPage",op)
	     }
    }
    else
    {
       alert(r.errorText);
    }  
  });
};


SelectTag.Show =function()// (SelectedIDs,userID,regFuntion)
{
  __userControl.show("divSelectTag");
  isClickOK=false; 
  //gRegFuntion=regFuntion;
  $("tags").innerHTML="";
  $("span_SelectedTags").innerHTML="";
  $("span_SelectedTagsCount").innerHTML="0";
  $("keyWord").value="关键字";
  $$("loading").showLoading("数据正在加载中。。");
  gTags=null;
  //gUserID=userID;
  
  var pageNum=1; 
  NetRadio.LocatingMonitor.Controls.__SelectTag.getTags(SelectTag.FunctionNo,SelectTag.SelectedTagIDs,pageNum,getKeyWord(),       function(r)
  {
    if(r.error==0)
	{
	    gTags=r.value[0];
	    pageNum=r.value[2];
	    var pageCount=r.value[1];
	    $("tags").innerHTML=  createTags(r.value[0],0);
	    set_span_SelectedTags();
	    set_selectedList_hidSelectedTagNames_hidSelectedTagIDs();
	    $("loading").innerHTML="";
	    
	     $select_ClearOptions("selectPage");	     
	     for(var i=1;i<=pageCount;i++)
	     {
	         var op=  document.createElement("option");
	         op.text="第 "+i+" 页";
	         op.value=i;
	         if(i==pageNum)
	         {
	           op.selected=true;
	         }
	         $select_AddOption("selectPage",op)
	     }
	     if(pageCount==0)
	     {
	       var op=  document.createElement("option");
	         op.text="第 0 页";
	         op.value=0;	        
	         $select_AddOption("selectPage",op)
	     }
    }
    else
    {
       alert(r.errorText);
    }  
  });
  
  
};
SelectTag.Close=function()
{
$("selectorLauncher").style.display="none";
$("divSelectTag").style.display="none";
$("selectedList").style.display="none";
}
/***************************      外部可以调用的部分 End        ********************************/







//供搜索使用
var gTags=null;
//var gUserID=0;
//var gRegFuntion=null;
var isClickOK=false;


function getModel(){
//获取模式
    var _modle =parseInt( $("hidModel").value);
    if(isNaN(_modle))
    {
      _modle=0;
    }
    return _modle;
}

function getSelectedIDs(){
//获取被选中的id字符串
    var rs=null;
    var values="";
    switch(getModel())
    {
        case 0:
           rs= $radio_GetCheckedRadio("tagsINPUT");
           if(typeof( rs)!="undefined")
            {
               values= rs.value;
            }
        break;
        case 1:
         rs= $checkbox_GetCheckedBoxs("tagsINPUT");
         for(var i=0 ;i<rs.length;i++)
         {
            values+=(i==0?"":",")+rs[i].value;
         }
        break;
        default:return ;
    }   
    return values;
}

function getSelectedMACs(){
//获取被选中的id字符串
    var rs=null;
    var values="";
    switch(getModel())
    {
        case 0:
           rs= $radio_GetCheckedRadio("tagsINPUT");
           if(typeof( rs)!="undefined")
            {
               values= rs.mac;
            }
        break;
        case 1:
         rs= $checkbox_GetCheckedBoxs("tagsINPUT");
         for(var i=0 ;i<rs.length;i++)
         {
            values+=(i==0?"":",")+rs[i].mac;
         }
        break;
        default:return ;
    }   
    return values;
}

function getSelectedNames(){
//获取被选中的name字符串

   var rs=null;
    var values="";
    switch(getModel())
    {
        case 0:
           rs= $radio_GetCheckedRadio("tagsINPUT");
           if(typeof( rs)!="undefined")
            {
               values= rs.text;
            }
        break;
        case 1:
         rs= $checkbox_GetCheckedBoxs("tagsINPUT");
         for(var i=0 ;i<rs.length;i++)
         {
            values+=(i==0?"":",")+rs[i].text;
         }
        break;
        default:return ;
    }   
    return values;    
}

function searchTags()
{
//   var tagName=$("keyWord").value.trim();
//   if(tagName.trim()=="" ||tagName.trim()=="关键字")
//   {
//      $("tags").innerHTML=createTags(gTags);
//      set_span_SelectedTags();
//      return;
//   }
//  var rs=new Array();
//  if(gTags!=null)
//  {  
//      var j=0;
//      for(var i=0;i<gTags.length;i++)
//      {
//        if( gTags[i].TagName.indexOf(tagName)!=-1 )
//        {
//            rs[j++]=gTags[i];
//        }
//      }
//  }
//    $("tags").innerHTML=createTags(rs);
//    set_span_SelectedTags();
//    return;


  $("tags").innerHTML="";
  $("span_SelectedTags").innerHTML="";
  $("span_SelectedTagsCount").innerHTML="0"; 
  $$("loading").showLoading("数据正在加载中。。");
  var pageNum=1; 
   
   
  NetRadio.LocatingMonitor.Controls.__SelectTag.getTags(SelectTag.FunctionNo,SelectTag.SelectedTagIDs,pageNum,getKeyWord(),    function(r)
  {
    if(r.error==0)
	{
	    gTags=r.value[0];
	    pageNum=r.value[2];
	    var pageCount=r.value[1];
	    $("tags").innerHTML=  createTags(r.value[0],0);
	    set_span_SelectedTags();
	    set_selectedList_hidSelectedTagNames_hidSelectedTagIDs();
	    $("loading").innerHTML="";
	    
	     $select_ClearOptions("selectPage");	     
	     for(var i=1;i<=pageCount;i++)
	     {
	         var op=  document.createElement("option");
	         op.text="第 "+i+" 页";
	         op.value=i;
	         if(i==pageNum)
	         {
	           op.selected=true;
	         }
	         $select_AddOption("selectPage",op)
	     }
	     if(pageCount==0)
	     {
	       var op=  document.createElement("option");
	         op.text="第 0 页";
	         op.value=0;	        
	         $select_AddOption("selectPage",op)
	     }
    }
    else
    {
       alert(r.errorText);
    }  
  });
  
}
function getKeyWord()
{
  var keyWord="";
  var tagName=$("keyWord").value.trim();
  if( ! (tagName.trim()=="" ||tagName.trim()=="关键字") )
  {
     keyWord=tagName;
  }
  return keyWord;
}
function selectPage_onchange()
{

  $("tags").innerHTML="";
  $("span_SelectedTags").innerHTML="";
  $("span_SelectedTagsCount").innerHTML="0"; 
  $$("loading").showLoading("数据正在加载中。。");
  var pageNum=parseInt($select_GetSelectedOption("selectPage").value);
  NetRadio.LocatingMonitor.Controls.__SelectTag.getTags(SelectTag.FunctionNo,SelectTag.SelectedTagIDs,pageNum,getKeyWord(),function(r)
  {
    if(r.error==0)
	{
	    gTags=r.value[0];
	    pageNum=r.value[2];
	    var pageCount=r.value[1];
	    $("tags").innerHTML=  createTags(r.value[0],0);
	    set_span_SelectedTags();
	    set_selectedList_hidSelectedTagNames_hidSelectedTagIDs();
	    $("loading").innerHTML="";
	    
	     $select_ClearOptions("selectPage");	     
	     for(var i=1;i<=pageCount;i++)
	     {
	         var op=  document.createElement("option");
	         op.text="第 "+i+" 页";
	         op.value=i;
	         if(i==pageNum)
	         {
	           op.selected=true;
	         }
	         $select_AddOption("selectPage",op)
	     }
	     if(pageCount==0)
	     {
	       var op=  document.createElement("option");
	         op.text="第 0 页";
	         op.value=0;	        
	         $select_AddOption("selectPage",op)
	     }
    }
    else
    {
       alert(r.errorText);
    }  
  });

}
function createTags(tags)
{
              var radioORcheck= getModel();
              var _type="";
              if(radioORcheck==0)
              {
                _type="radio";
              }
              else if(radioORcheck==1)
              { 
                _type="checkbox";  
              }
               var vs=tags;
               var html=new Array();
               var widthCount=6;
               var heightCount=Math.ceil(vs.length/widthCount);
               var lastCount=widthCount - vs.length%widthCount;
               html.push('<table border="0" width="630" cellspacing="0" cellpadding="0">');
               for(var  h=0,c=0; h<heightCount; h++)
               {
                   html.push('<tr>');
                   for(var i=0 ;c<vs.length && i<widthCount;i++,c++)
                   {
                      html.push('<td><input name="tagsINPUT" type="');
                      html.push(_type);
                      html.push( '" onclick="return tagsINPUT_click()" value="');
                      html.push( vs[c].TagID);
                      html.push(  '" ');
                      html.push(' text="' );
                      html.push( vs[c].TagName );
                      html.push( '"  ');
                      html.push(' mac="' );
                      html.push( vs[c].TagMac );
                      html.push( '"  ');
                      html.push((vs[c].Checked.toString()=="1" ? 'checked="checked"': ''));
                      html.push(' />');
                      html.push(vs[c].TagName);
                      html.push("</td>");
                              
                   }
                   if(h==heightCount-1)
                   {
                     for(var i=0; i<lastCount; i++)
                     {
                       html.push('<td>&nbsp;</td>');
                     }
                   }
                   html.push('</tr>');
               }
               html.push('</table>');
               return html.join("");
}
 function set_selectedList_hidSelectedTagNames_hidSelectedTagIDs()
 { 
     var count=0;
     
     switch(getModel())
    {
        case 0:
             count= typeof($radio_GetCheckedRadio("tagsINPUT"))=="undefined"?"0":1;
        break;
        case 1:
             count= $checkbox_GetCheckedBoxs("tagsINPUT").length;
        break;
        default:return ;
    }  
    var text= "已选择标签 <font color=red>"+count+"</font> 个"; 
    if(count!=0)
    {
     text+="：";
    }
    $("hidSelectedTagNames").value=getSelectedNames();
    $("hidSelectedTagIDs").value=getSelectedIDs();
    $("hidSelectedTagMACs").value=getSelectedMACs();    
    text+=$("hidSelectedTagNames").value;
	$("selectedList").innerHTML=text;	
 }
 
 function set_span_SelectedTags()
 {
     var text=getSelectedNames(); 
     switch(getModel())
    {
        case 0:
             $("span_SelectedTagsCount").innerHTML= typeof($radio_GetCheckedRadio("tagsINPUT"))=="undefined"?"0":1;
        break;
        case 1:
             $("span_SelectedTagsCount").innerHTML= $checkbox_GetCheckedBoxs("tagsINPUT").length;
        break;
        default:return ;
    }     
     $("span_SelectedTags").innerHTML=text;
 }
function tagsINPUT_click()
{       
   set_span_SelectedTags();    
}

function unbind_onclick()
{
   $radio_setUnCheckedRadio("tagsINPUT");
   set_span_SelectedTags(); 
}

function ok_onclick()
{
   set_selectedList_hidSelectedTagNames_hidSelectedTagIDs();
   if($("hidEnableOkFunction").value.toLowerCase()=="true")
   {
        $("butOK").disabled=true;
        $("butCancel").disabled=true;
        $$("loading").showLoading("数据正在提交中。。");
        NetRadio.LocatingMonitor.Controls.__SelectTag.UpdateAction(SelectTag.FunctionNo,getSelectedIDs(),SelectTag.UserID,function(r)
          {
            if(r.error==0)
	        {
	            $("butOK").disabled=false;
                $("butCancel").disabled=false;
                $("loading").innerHTML="<font color=red>√ 数据已经成功提交</font>"; 
                isClickOK=true; 
    	        window.setTimeout(function(){ if($("loading")!=null){ $("loading").innerHTML="";}},1000*1);

            }
            else
            {
                $("butOK").disabled=false;
                $("butCancel").disabled=false;
                $("loading").innerHTML="<font color=red>× "+r.errorText+"</font>";
            }
          
          });
    }
    else
    {
      isClickOK=true;
      cancel_onclick();
    }
}

function cancel_onclick()
{
//var gTags=null;
//var gUserID=0;
__userControl.close("divSelectTag");
if( isClickOK==true)
{
 if(typeof(SelectTag.RegFunction)!="undefined")
 {
   SelectTag.RegFunction();
 }
}
}