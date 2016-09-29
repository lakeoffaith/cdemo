using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common.LocatingMonitor;
using NetRadio.Data;
using System.ComponentModel;
using NetRadio.DataExtension;
using NetRadio.Model;
using System.Reflection;
using Summer;
namespace NetRadio.LocatingMonitor.Controls
{
    public enum SelectModel
    {
        Single = 0,
        Multiples = 1
    }

    public partial class __SelectTag : System.Web.UI.UserControl
    {
        /// <summary>
        /// 选中的模式，默认值是单选
        /// </summary>
        public SelectModel Model
        {
            get
            {
                if (ViewState["Model"] != null)
                {
                    return (SelectModel)ViewState["Model"];
                }
                return SelectModel.Single;
            }
            set
            {
                //labHidden.Text = "<input type=\"hidden\" name=\"hidModel\" value=\"" + (int)value + "\" />";
                ViewState["Model"] = value;
            }
        }
        /// <summary>
        /// 获取被选中的标签名称，（之间逗号分隔，当页面post的时候才可以获得）
        /// </summary>
        public string SelectedTagNames
        {
            get
            {
                if (IsPostBack)
                {
                    return Request.Form["hidSelectedTagNames"];
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// 获取被选中的标签ID，（之间逗号分隔，当页面post的时候才可以获得）
        /// </summary>
        public string SelectedTagIDs
        {
            get
            {
                if (IsPostBack)
                {
                    return Request.Form["hidSelectedTagIDs"];
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 获取被选中的标签ID，（之间逗号分隔，当页面post的时候才可以获得）
        /// </summary>
        public int[] SelectedTagIdArray
        {
            get
            {
                if (string.IsNullOrEmpty(SelectedTagIDs))
                {
                    return new int[] { };
                }
                else
                {
                    return Strings.ParseToArray<int>(SelectedTagIDs);
                }
            }
        }
        /// <summary>
        /// 是否显示头部的提示框信息（被选中的标签及个数），默认值true
        /// </summary>
        public bool VisibleTip
        {
            get
            {
                if (ViewState["VisibleTip"] != null)
                {
                    return (bool)ViewState["VisibleTip"];
                }
                return true;
            }
            set
            {
                ViewState["VisibleTip"] = value;
            }
        }
        /// <summary>
        /// 是否显示解除按钮，默认值true
        /// </summary>
        public bool VisibleUnBindButton
        {
            get
            {
                if (ViewState["VisibleUnBindButton"] != null)
                {
                    return (bool)ViewState["VisibleUnBindButton"];
                }
                return true;
            }
            set
            {
                ViewState["VisibleUnBindButton"] = value;
            }
        }

        /// <summary>
        /// 点击确定，是否调用后台函数，默认值true
        /// </summary>
        public bool EnableOkFunction
        {
            get
            {
                if (ViewState["EnableOkFunction"] != null)
                {
                    return (bool)ViewState["EnableOkFunction"];
                }
                return true;
            }
            set
            {
                ViewState["EnableOkFunction"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.AjaxManager.RegisterClass(typeof(__SelectTag), Page);
        }

        /// <summary>
        ///  获取未被绑定的所有标签， selectedIDs参数里指定已绑定的标签除外
        /// </summary>
        /// <param name="selectedIDs">指定已绑定的标签</param>
        /// <returns></returns>
        [Ajax.AjaxMethod]
        public object[] getTags(string FunctionNo, string selectedIDs, int pageNum, string keyWord)
        {
            int _pageCount;
            int _pageSize = 6 * 15;
            TagWithChecked[] twc = null;
            object[] obj = new object[3];
            switch (FunctionNo)
            {
                case "01"://用户列表页面
                    TagWithChecked[] tc = Business.BusTag.GetTags(selectedIDs).Where(_d => _d.TagName.IndexOf(keyWord) != -1).ToArray();
                    _pageCount = (int)Math.Ceiling((double)tc.Length / _pageSize);
                    if (_pageCount < pageNum)
                    {
                        pageNum = 1;
                    }
                    twc = tc.Skip(_pageSize * (pageNum - 1)).Take(_pageSize).ToArray();
                    obj[0] = twc;
                    obj[1] = _pageCount;
                    obj[2] = pageNum;
                    break;
                default:
                    throw new Exception("未能被识别的功能编号");
                    break;
            }
            return obj;

        }

        /// <summary>
        /// 更新数据库，更新用户标签
        /// </summary>
        /// <param name="selectedIDs"></param>
        /// <param name="userID"></param>
        [Ajax.AjaxMethod]
        public void UpdateAction(string FunctionNo, string selectedIDs, int userID)
        {
            switch (FunctionNo)
            {
                case "01"://用户列表页面
                    NetRadio.Model.object_HostTag ht = new object_HostTag();
                    ht.HostId = userID;
                    if (ht.Select() && ht.TagId != 0)
                    {
                        NetRadio.LocatingMonitor.TagUsers.__TagUser.ChangeTag(ht.TagId, selectedIDs == "" ? 0 : Convert.ToInt32(selectedIDs));
                    }
                    else
                    {
                        NetRadio.LocatingMonitor.TagUsers.__TagUser.BindTag(userID, selectedIDs == "" ? 0 : Convert.ToInt32(selectedIDs));
                    }
                    break;
                default:
                    throw new Exception("未能被识别的功能编号");
                    break;
            }
        }


    }
}