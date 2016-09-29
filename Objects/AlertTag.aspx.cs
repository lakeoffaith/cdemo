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
    public partial class __AlertTag : BasePage
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
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.Title = this.WebSiteLabel = "受虐标签设置";
            if (!IsPostBack)
            {
                LoadJailRoomList();
                LoadIllTreatedTag();
            }
        }

        private void LoadJailRoomList()
        {
            newJailRoom.Items.Clear();

            newJailRoom.DataSource = MapArea.All;
            newJailRoom.DataTextField = "AreaName";
            newJailRoom.DataValueField = "Id";
            newJailRoom.DataBind();

            newJailRoom.Items.Insert(0, new ListItem("请选择 ..", "-1"));
        }

        private void LoadIllTreatedTag()
        {
            using (AppDataContext db = new AppDataContext())
            {
                list.DataSource = db.HostTags.Where(h => h.HostType == (byte)HostTypeType.Other).ToList();
                list.DataBind();
            }
        }

        protected void list_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HostTag ev = e.Item.DataItem as HostTag;

            if (ev != null)
            {
                SmartLabel isChecked = (SmartLabel)e.Item.FindControl("isChecked");
                SmartLabel jailRoom = e.Item.FindControl("jailRoom") as SmartLabel;
                SmartLabel tagHostName = e.Item.FindControl("tagHostName") as SmartLabel;
                SmartLabel tagMac = e.Item.FindControl("tagMac") as SmartLabel;
                DateTimeLabel writeTime = e.Item.FindControl("writeTime") as DateTimeLabel;

                isChecked.Text = "<input type='checkbox' name='selection' value='" + ev.HostId + "' />";
                jailRoom.Text = ev.Description;
                tagHostName.Text = ev.HostName;
                tagMac.Text = Tag.Select(ev.TagId).TagMac;
                writeTime.DisplayValue = ev.WriteTime;
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            try
            {
                int hostId= HostTag.AddOrUpdateHostTag(0, tagSelector.SelectedTagIdArray[0], "", tagName.Text.Trim(), (int)HostTypeType.Other, newJailRoom.SelectedItem.Text + "监房", "");
                HostTag.SetHostGroup(hostId, (int)TagUserType.Position);
            }
            catch
            {
            }
            Response.Redirect("/Objects/AlertTag.aspx");
        }

        protected void setDelete_Click(object sender, EventArgs e)
        {
            IEnumerable<int> idList = Strings.ParseToArray<int>(Request.Form["selection"]);
            if (idList.Count() > 0)
            {
                foreach (int hostId in idList)
                {
                    HostTag.DeleteHostTag(hostId);
                    HostTag.RemoveHostGroupByHostId(hostId);
                }
            }
            Response.Redirect(Fetch.CurrentUrl);
        }
    }
}
