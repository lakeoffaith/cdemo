function setCookie(name,value)
{
///<summary>设置Cookie</summary>
///<param name="name">cookie的名子</param>
///<param name="value">值</param>
///<returns></returns> 
    var Hours = 24; //此 cookie 将被保存 24 小时
    var exp  = new Date();    //new Date("December 31, 9998");
    exp.setTime(exp.getTime() + Hours*60*60*1000);
    //debugger;
    document.cookie = name + "="+ escape (value) + ";expires=" + exp.toGMTString()+";path=/;";
}
function getCookie(name)       
{
///<summary>获取Cookie</summary>
///<param name="name">cookie的名子</param>
///<returns>返回Cookie值</returns> 
    var arr = document.cookie.match(new RegExp("(^| )"+name+"=([^;]*)(;|$)"));
    //debugger;
     if(arr != null) return unescape(arr[2]); return null;

}
function delCookie(name)
{
///<summary>删除Cookie</summary>
///<param name="name">cookie的名子</param>
///<returns></returns> 
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval=getCookie(name);
    if(cval!=null) document.cookie= name + "="+cval+";expires="+exp.toGMTString()+";path=/;";
}

