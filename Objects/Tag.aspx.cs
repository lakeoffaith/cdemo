using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using NetRadio.Web;
using NetRadio.Assistant.Web.Ajax;
using NetRadio.Assistant.Web.Controls;
using NetRadio.Assistant.Web.Util;
using NetRadio.Common;
using NetRadio.Common.LocatingMonitor;
using NetRadio.DataExtension;
using NetRadio.Data;
using NetRadio.LocatingService.RemotingEntry;
namespace NetRadio.LocatingMonitor.Objects
{
    [AjaxRegister]
    public partial class __Tag : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        public __Tag()
        {
            _tagId = Fetch.QueryUrlAsInteger("id");
        }

        int _tagId;

        protected void Page_Load(object sender, EventArgs e)
        {

            Tag tag = Tag.Select(_tagId);
            if (tag == null)
            {
                ShowMessagePage("标签不存在。");
            }

            try
            {
                //using (AppDataContext db = new AppDataContext())
                //{
                //    var hostTagView = db.HostTagGroupStatus.Where(h => h.ParentGroupId == 0).Single(h => h.TagId == _tagId);
                HostTagGroupStatus hostTagView = HostTagGroupStatus.SelectByTagId(_tagId);
                if (hostTagView != null)
                {
                    Response.Redirect("../TagUsers/TagUser.aspx?type=" + hostTagView.HostGroupId + "&id=" + hostTagView.HostId, true);
                }
                //}
            }
            catch (Exception err)
            {
                Diary.Debug("Tag.aspx: " + err.ToString());
            }

            tagName.Text = tag.TagName;
            macAddress.Text = tag.TagMac;
            productType.Text = ((TagProductType)tag.ProductType).ToString();
        }
    }
}
