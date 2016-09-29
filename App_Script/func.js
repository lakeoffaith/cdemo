function Search(objectArray,columnName,columnValue)
{
///<summary>在对象数组中搜索</summary>
///<param name="objectArray">对象数组</param>
///<param name="columnName">属性名称</param>
///<param name="columnValue">属性值</param>
///<returns>符合条件的对象数组</returns>
    var sign=false;
    var r=new Array();     
    for(var i=0,j=0;i<objectArray.length;i++)
    {      
        sign=(objectArray[i][columnName]==columnValue);
        if(sign)
        {
         r[j++]=objectArray[i];            
        }
        sign=false;        
    }
    return r;
}

function Query(objectArray,columnName,columnValueArray)
{
///<summary>在对象数组中搜索</summary>
///<param name="objectArray">对象数组</param>
///<param name="columnName">属性名称</param>
///<param name="columnValue">属性值数组</param>
///<returns>符合条件的对象数组</returns>
    var sign=false;
    var r=new Array();     
    for(var i=0,j=0;i<objectArray.length;i++)
    { 
        for(var k=0;k<columnValueArray.length;k++)
        {
            sign=(objectArray[i][columnName]==columnValueArray[k]);
            if(sign)
            {
             r[j++]=objectArray[i];            
            }
            sign=false; 
        }               
    }
    return r;
}
  
function IsExist(objectArray,columnName,columnValue)
{
///<summary>在对象数组中搜索对象是否存在</summary>
///<param name="objectArray">对象数组</param>
///<param name="columnName">属性名称</param>
///<param name="columnValue">属性值</param>
///<returns>对象是否存在</returns>        
    for(var i=0,j=0;i<objectArray.length;i++)
    {      
        if(objectArray[i][columnName]==columnValue)
        return true;        
    }
    return false;
}  
function Sort(objectArray,columnName,ascDesc,lowerUpper)
{   
///<summary>对象数组排序</summary>
///<param name="objectArray">对象数组</param>
///<param name="columnName">属性名称</param>
///<param name="ascDesc">大于0为升序，否则降序</param>
///<param name="lowerUpper">大于0为区分大小写，否则不区分大小写</param>
///<returns>排序后的对象数组</returns>    
    var temp;
    for(var i=0;i<objectArray.length-1;i++)
    {
      for(var j=0;j<objectArray.length-i-1;j++)
      {                  
              if(ascDesc>0)
              {   
                    if(lowerUpper<=0)
                     {
                          if(getLowerCase(objectArray[j][columnName])>getLowerCase(objectArray[j+1][columnName]))
                           {
                               temp=objectArray[j];
                               objectArray[j]= objectArray[j+1];
                               objectArray[j+1]=temp;
                           }
                           continue;
                     }
                   else if(objectArray[j][columnName]>objectArray[j+1][columnName])
                   {
                       temp=objectArray[j];
                       objectArray[j]= objectArray[j+1];
                       objectArray[j+1]=temp;
                   }
              }
              else
              {
                     if(lowerUpper<=0)
                     {                     
                           if(getLowerCase(objectArray[j][columnName])<getLowerCase(objectArray[j+1][columnName]))
                           {
                               temp=objectArray[j];
                               objectArray[j]= objectArray[j+1];
                               objectArray[j+1]=temp;
                           }
                           continue;
                     }
                   else  if(objectArray[j][columnName]<objectArray[j+1][columnName])
                   {
                       temp=objectArray[j];
                       objectArray[j]= objectArray[j+1];
                       objectArray[j+1]=temp;
                   }
              }
      }
    }
    return objectArray;
}
    
function getLowerCase(value)
{
    if(value!=null && typeof(value)=="string" )
    {
        return value.toLowerCase();
    }
    return value;
}
function tree()
{
 this.treeNodes=null;
 this.rootNode=null;
}
function treeNode()
{
    this.key=null;
    this.parentKey=null;
    this.value=null;
}
tree.prototype.getNode=function(key)
{
    return Search(this.treeNodes,"key",key)[0];
}

tree.prototype.getChildNodes=function(key)
{
    return Search(this.treeNodes,"parentKey",key);
}

tree.prototype.getLeafNodes=function(key)
{
    var nodes=this.getChildNodes(key);
    if(nodes.length==0)
    {
       return this.getNode(key);
    }
    var _result=new Array();
    for(var i=0;i<nodes.length;i++)
    {   
      var temp= this.getChildNodes(nodes[i].key);
      if(temp.length==0)
      {
        _result[_result.length]=nodes[i];
      }
      else
      {
        for(var j=0;j<temp.length;j++)
        {
           var temp2= this.getLeafNodes(temp[j].key);
           for(var k=0;k<temp2.length;k++)
           {
               _result[_result.length]=temp2[k];
           }
        }
      }
    }
    return _result;
}

tree.prototype.getAllNodes=function(key)
{
  var nodes=this.getChildNodes(key);
  var _result=nodes;
  if(nodes.length>0)
  {
    for(var i=0;i<nodes.length;i++)
    {
      var temp= this.getAllNodes(nodes[i].key);
      for(var j=0;j<temp.length;j++)
      {
        _result[_result.length]=temp[j];
      }
    }   
  }
  return _result;
}

//function setCookie(name,value)//两个参数，一个是cookie的名子，一个是值
//{
/////<summary>设置Cookie</summary>
/////<param name="name">cookie的名子</param>
/////<param name="value">值</param>
/////<returns></returns> 
//    var Hours = 24; //此 cookie 将被保存 24 小时
//    var exp  = new Date();    //new Date("December 31, 9998");
//    exp.setTime(exp.getTime() + Hours*60*60*1000);
//    //debugger;
//    document.cookie = name + "="+ escape (value) + ";expires=" + exp.toGMTString()+";path=/;";
//}
//function getCookie(name)       
//{
/////<summary>获取Cookie</summary>
/////<param name="name">cookie的名子</param>
/////<returns>返回Cookie值</returns> 
//    var arr = document.cookie.match(new RegExp("(^| )"+name+"=([^;]*)(;|$)"));
//    //debugger;
//     if(arr != null) return unescape(arr[2]); return null;

//}
//function delCookie(name)
//{
/////<summary>删除Cookie</summary>
/////<param name="name">cookie的名子</param>
/////<returns></returns> 
//    var exp = new Date();
//    exp.setTime(exp.getTime() - 1);
//    var cval=getCookie(name);
//    if(cval!=null) document.cookie= name + "="+cval+";expires="+exp.toGMTString()+";path=/;";
//}

//userdata操作
var isIE = !!document.all;

if(isIE)
{
    document.documentElement.addBehavior("#default#userdata");
}

function  setUserData(key, value){
    var ex; 
    if(isIE){
        with(document.documentElement)try {
            load(key);
            setAttribute("value", value);
            save(key);
            return  getAttribute("value");
        }catch (ex){
            alert(ex.message)
        }
    }else if(window.sessionStorage){//for firefox 2.0+
        try{
            sessionStorage.setItem(key,value)
        }catch (ex){
            alert(ex);
        }
    }else{
        alert("当前浏览器不支持userdata或者sessionStorage特性")
    }
}
 

function getUserData(key){
    var ex; 
    if(isIE){
        with(document.documentElement)try{
            load(key);
            return getAttribute("value");
        }catch (ex){
            alert(ex.message);return null;
        }
    }else if(window.sessionStorage){//for firefox 2.0+
        try{
            return sessionStorage.getItem(key)
        }catch (ex){
            alert(ex)
        }
    }else{
        alert("当前浏览器不支持userdata或者sessionStorage特性")
    }
}

function  delUserData(key){
    var ex; 
    if(isIE){
        with(document.documentElement)try{
            load(key);
            expires = new Date(315532799000).toUTCString();
            save(key);
        }
        catch (ex){
            alert(ex.message);
        }
    }else if(window.sessionStorage){//for firefox 2.0+
        try{
            sessionStorage.removeItem(key)
        }catch (ex){
            alert(ex)
        }
    }else{
        alert("当前浏览器不支持userdata或者sessionStorage特性")
    }
} 

String.prototype.trim=function ()
{
return this.replace(/\s/g,"");
}
String.prototype.compress=function ()
{
 var s=this.toLowerCase();
s= s.replace(/(<table)/g,"T1")           //<table            T1
s= s.replace(/(<\/table>)/g,"T2")         //</table>         T2
s= s.replace(/(<tbody>)/g,"")            //<tbody>          ""
s= s.replace(/(<\/tbody>)/g,"")          //</tbody>         ""
s= s.replace(/(<tr)/g,"R1")               //<tr              R1
s= s.replace(/(<\/tr>)/g,"R2")           //</tr>             R2
s= s.replace(/(<td)/g,"D1")               //<td              D1
s= s.replace(/(<\/td>)/g,"D2")           //</td>             D2
s= s.replace(/(<div)/g,"I1")              //<div             I1
s= s.replace(/(<\/div>)/g,"I2")          //</div>            I2
s= s.replace(/(<span)/g,"S1")             //<span            S1
s= s.replace(/(<\/span>)/g,"S2")         //</span>           S2
s= s.replace(/(class=)/g,"C1")           //class=            C1
s= s.replace(/(padding-left:)/g,"P1")    // padding-left:    P1
s= s.replace(/(padding-right:)/g,"P2")   // padding-right:   P2
s= s.replace(/(padding-top:)/g,"P3")     // padding-top:     P3
s= s.replace(/(padding-bottom:)/g,"P4")  // padding-left:    P4
s= s.replace(/(margin-left:)/g,"M1")     // margin-left:     M1
s= s.replace(/(margin-right:)/g,"M2")    // margin-right:    M2
s= s.replace(/(margin-top:)/g,"M3")      // margin-top:      M3
s= s.replace(/(margin-bottom:)/g,"M4")   // margin-left:     M4
s= s.replace(/(border:)/g,"B1")           // border :        B1
s= s.replace(/(border-left:)/g,"B2")     // border-left:     B2
s= s.replace(/(border-right:)/g,"B3")    // border-right:    B3
s= s.replace(/(border-top:)/g,"B4")      // border-top:      B4
s= s.replace(/(border-bottom:)/g,"B5")   // border-bottom:   B5
s= s.replace(/(style:)/g,"S3")           // style:           S3
s= s.replace(/(solid)/g,"S4")            // solid            S4
s= s.replace(/(width)/g,"W1")             // width           W1
s= s.replace(/(height)/g,"H1")            // height          H1
s= s.replace(/(onclick)/g,"O1")          // onclick          O1
s= s.replace(/(onmousemove)/g,"O2")      // onmousemove      O2
s= s.replace(/(onmouseout)/g,"O3")       // onmouseout       O3
 ;
 return s;
}
String.prototype.decompress=function ()
{
 var s=this;
s= s.replace(/(T1)/g,"<table")           //<table            T1
s= s.replace(/(T2)/g,"</table>")         //</table>          T2
//s= s.replace(/(<tbody>)/g,"")            //<tbody>          ""
//s= s.replace(/(<\/tbody>)/g,"")          //</tbody>         ""
s= s.replace(/(R1)/g,"<tr")               //<tr              R1
s= s.replace(/(R2)/g,"</tr>")           //</tr>              R2
s= s.replace(/(D1)/g,"<td")               //<td              D1
s= s.replace(/(D2)/g,"</td>")           //</td>              D2
s= s.replace(/(I1)/g,"<div")              //<div             I1
s= s.replace(/(I2)/g,"</div>")          //</div>             I2
s= s.replace(/(S1)/g,"<span")             //<span            S1
s= s.replace(/(S2)/g,"</span>")         //</span>            S2
s= s.replace(/(C1)/g,"class=")           //class=            C1
s= s.replace(/(P1)/g,"padding-left:")    // padding-left:    P1
s= s.replace(/(P2)/g,"padding-right:")   // padding-right:   P2
s= s.replace(/(P3)/g,"padding-top:")     // padding-top:     P3
s= s.replace(/(P4)/g,"padding-bottom:")  // padding-left:    P4
s= s.replace(/(M1)/g,"margin-left:")     // margin-left:     M1
s= s.replace(/(M2)/g,"margin-right:")    // margin-right:    M2
s= s.replace(/(M3)/g,"margin-top:")      // margin-top:      M3
s= s.replace(/(M4)/g,"margin-bottom:")   // margin-left:     M4
s= s.replace(/(B1)/g,"border:")           // border :        B1
s= s.replace(/(B2)/g,"border-left:")     // border-left:     B2
s= s.replace(/(B3)/g,"border-right:")    // border-right:    B3
s= s.replace(/(B4)/g,"border-top:")      // border-top:      B4
s= s.replace(/(B5)/g,"border-bottom:")   // border-bottom:   B5
s= s.replace(/(S3)/g,"style:")           // style:           S3
s= s.replace(/(S4)/g,"solid")            // solid            S4
s= s.replace(/(W1)/g,"width")             // width           W1
s= s.replace(/(H1)/g,"height")            // height          H1
s= s.replace(/(O1)/g,"onclick")          // onclick          O1
s= s.replace(/(O2)/g,"onmousemove")      // onmousemove      O2
s= s.replace(/(O3)/g,"onmouseout")       // onmouseout       O3
 ;
 return s;
}

function request(paras){ 
var url = location.href;   
var paraString = url.substring(url.indexOf("?")+1,url.length).split("&");   
var paraObj = {}   
for (i=0; j=paraString[i]; i++){   
paraObj[j.substring(0,j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf 
("=")+1,j.length);   
}   
var returnValue = paraObj[paras.toLowerCase()];   
if(typeof(returnValue)=="undefined"){   
return "";   
}else{   
return returnValue;   
}   
} 

function $n(tag,name) {    var returns = document.getElementsByName(name);
    if (returns.length > 0) return returns;
    returns = new Array();
    var e = document.getElementsByTagName(tag);

      for (i = 0; i < e.length; i++) {
        if (e[i].getAttribute("name") == name)
            returns[returns.length] = e[i];
    }
    return returns;
}


function closeWindow()
{
    window.opener=null;window.open('','_self');window.close();
}


   
	
	
	/*html 控件操作*/
	function $input_GetAttribute(id,attribute)
	{
	  return $(id)[attribute];
	}
	function $input_GetValue(id)
	{
	  return $(id)["value"];
	}
	
	function $input_SetAttribute(id,attribute,value)
	{
	  return $(id)[attribute]=value;
	}
	
	function $radio_GetCheckedRadio(name)
	{
	      for(var i=document.getElementsByName(name).length-1;i>=0;i--)
          {
            if(document.getElementsByName(name).item(i).checked)
            {
                return document.getElementsByName(name).item(i);    
            }
          }
	}
	
	
	function $radio_setUnCheckedRadio(name)
	{
	      for(var i=document.getElementsByName(name).length-1;i>=0;i--)
          {
            document.getElementsByName(name).item(i).checked=false;
          }
	}
	

function $checkbox_GetCheckedBoxs(name)
{
      var obj=new Array();
      var k=0;
      for(var i=document.getElementsByName(name).length-1;i>=0;i--)
          {
            if(document.getElementsByName(name).item(i).checked)
            {
              obj[k++]=document.getElementsByName(name).item(i);   
            }
          }
      return obj;
}	
	
function $select_GetSelectedOption(id)
{
      for(var i=$(id).options.length-1;i>=0;i--)
      {
        if($(id).options[i].selected)
        {
            return $(id).options[i];    
        }
      }
  }
  function $select_GetSelectedOptions(id) {
      var options = [];
      var j = 0;
      for (var i = $(id).options.length - 1; i >= 0; i--) {
          if ($(id).options[i].selected) {
              options[j++] = $(id).options[i];
          }
      }
      return options;
  }
		
function $select_ClearOptions(id)
{
  for(var i=$(id).options.length-1;i>=0;i--)
  {
    $(id).options.remove(i);
  }
}
function $select_AddOption(id,option)
{
    $(id).options.add(option);
}
function $select_AddOneOption(id, text, value) {
    var _option = document.createElement("option");
    _option.text = text;
    _option.value = value;
    $select_AddOption(id, _option);
}

function $select_DeleteOption(id,index)
{
   $(id).options.remove(index);
}
	
	
function execInnerScript(innerhtml)
{  
  var temp=innerhtml.replace(/\n|\r/g,"");
  var regex=/<script.+?<\/script>/gi;
  var arr=temp.match(regex);  
  if(arr)
  {    
    for(var i=0;i<arr.length;i++)
    {      
      var temp1=arr[i];      
      var reg=new RegExp("^<script(.+?)>(.+)<\/script>$","gi");
      reg.test(temp1);                  
      eval(RegExp.$2);            
    }
  }  
}
