 
var SelectStrongestRssiTag=new Object();
SelectStrongestRssiTag.UserID=0;
//当用户点击确定之后回调的函数
SelectStrongestRssiTag.RegFunction=function(){};
 
function enableOK() {return document.getElementById("spanOK")!=undefined; }
 
var hidTagID="hidTagID";
var hidTagMAC="hidTagMAC";

var timer1;
var __SelectedID="";
var __SelectedMAC="";
SelectStrongestRssiTag.Show=function()
{
   $("divSelectTag").style.display="inline";
}
SelectStrongestRssiTag.Close=function()
{
  if(timer1!=null)
  {
    clearInterval(timer1);
  }
   $("divSelectTag").style.display="none";
}
SelectStrongestRssiTag.GetSelectedID=function()
{
return __SelectedID;
}
SelectStrongestRssiTag.GetSelectedMAC=function()
{
return __SelectedMAC;
}
SelectStrongestRssiTag.SetTitle=function(mes)
{
 $("labTitle").innerHTML=mes;
}
SelectStrongestRssiTag.StopTimer=function()
{
 if(timer1!=null)
 {
   clearInterval(timer1);
 }
}

function ChangeTag()
{
    $$("labLoading").showLoading("数据提交中。。。");
    window.clearInterval(timer1);
    NetRadio.LocatingMonitor.Controls.__SelectTag.UpdateAction("01", parseInt( $(hidTagID).value), SelectStrongestRssiTag.UserID,function(r){
           $("labLoading").innerHTML="";
           if(r.error==0)
           {
                $("labTag").innerHTML="√标签更新成功";
                 __SelectedID=$(hidTagID).value;
                 __SelectedMAC=$(hidTagMAC).value;
                 
                 SelectStrongestRssiTag.RegFunction();
           }
           else
           {
                $("labTag").innerHTML="×标签更新失败，错误信息："+r.errorText;
           }
           window.setTimeout(function(){
                timer1=setInterval(function(){getStrongestRssiTag();},1000);
                                            },1000);
      }); 
}

function UnbindTag()
{
   $$("labLoading").showLoading("数据提交中。。。");
    window.clearInterval(timer1);
    NetRadio.LocatingMonitor.Controls.__SelectTag.UpdateAction("01","0", SelectStrongestRssiTag.UserID,function(r){
          $("labLoading").innerHTML="";
           if(r.error==0)
           {
                $("labTag").innerHTML="√标签绑定解除成功";
                 __SelectedID="";
                 __SelectedMAC="";
                 
                 SelectStrongestRssiTag.RegFunction();
           }
           else
           {
                $("labTag").innerHTML="×标签绑定解除失败，错误信息："+r.errorText;
           }
          
           window.setTimeout(function(){
                timer1=setInterval(function(){getStrongestRssiTag();},1000);
                                            },1000);
      });
}
function clearInfo()
{
    $("labTag").innerHTML="";
    $(hidTagID).value="";
    $("spanClear").style.display="none";
    if(enableOK())
    {
      $("spanOK").style.display="none";  
    } 
}


function GetStrongestRssiTagID()
{
    var tid=parseInt( $(hidTagID).value);
    if(tid==NaN)tid=0;
    return tid;
}

function getStrongestRssiTag()
{
if(!isComplete)
return;
isComplete=false;       
NetRadio.LocatingMonitor.Controls.__SelectStrongestRssiTag.GetStrongestRssiTag(function(r){
 if ($("labTag")!=null)//防止定时器在执行阶段，节点没有了。
 {
           if(r.error==0)
           {
                 var _v=r.value;
                
                 if(_v.Error==0)
                 {  
                    $("labTag").innerHTML=_v.Mes;                                  
                    $(hidTagID).value=_v.TagID;                    
                    $(hidTagMAC).value=_v.TagMAC;
                    
                    if(enableOK())
                    {
                      $("spanOK").style.display="inline";
                    }
                 }
                 else if(_v.Error==1 || _v.Error==3)
                 { 
                    $("labTag").innerHTML=_v.Mes;                 
                    $(hidTagID).value="";
                    $(hidTagMAC).value="";
                    if(enableOK())
                    {
                      $("spanOK").style.display="none";
                    }
                 } 
                 else if(_v.Error==2)
                 {
                     //LocatingService不可用
                 }
           }
           else
           {
                $("labTag").innerHTML=r.errorText;
                $(hidTagID).value="";
                $(hidTagMAC).value="";
                if(enableOK())
                {
                  $("spanOK").style.display="none";  
                }              
           }  
           isComplete=true;  
           $("spanClear").style.display="inline";  
}
});
}

 var isComplete=true;
 
 SelectStrongestRssiTag.Init=function(){
 if(timer1!=null)
 {
   clearInterval(timer1);
 }
  __SelectedID="";
 __SelectedMAC="";
 isComplete=true;
 getStrongestRssiTag();
 timer1=setInterval(function(){getStrongestRssiTag();},1000); 
 }
 