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
using System.IO;
using NetRadio.LocatingMonitor.Controls;
namespace NetRadio.LocatingMonitor.Organize
{
    /// <summary>
    /// 区域分组设置，内容模块。lyz
    /// </summary>
    public partial class __MapAreaGroup0 : NetRadio.Web.BaseUserControl
    {
        private DataTable dt_areaGroup;

        public __MapAreaGroup0()
        {

        }

       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            dt_areaGroup = Business.BusAreaGroup.GetAreaGroupIncludeAreas();
            areaGroupList.DataSource = Business.BusAreaGroup.GetAreaGroupInfo();
            areaGroupList.DataBind();
        }

        protected void areaGroupList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lab = e.Item.FindControl("labAreaNames") as Label;
                lab.Text = "";
                if (lab != null)
                {
                    Model.map_MapAreaGroupInfo ag = e.Item.DataItem as Model.map_MapAreaGroupInfo;
                    object[] drs = dt_areaGroup.AsEnumerable().Where(_d => _d["AreaGroupId"].ToString().Trim() == ag.GroupID.ToString().Trim()).Select(u => u["areaname"]).Distinct().ToArray();
                    foreach (object dr in drs)
                    {
                        if (lab.Text == "")
                        {
                            lab.Text += dr.ToString();
                        }
                        else
                        {
                            lab.Text += "，" + dr.ToString();
                        }
                    }
                    if (lab.Text != "")
                    {
                        lab.Text = "<br/><font color=gray>{&nbsp;" + lab.Text + "&nbsp;}</font>";
                    }
                }
            }
        }


        //public virtual string RenderAsString()
        //{
        //    return this.iform1.RenderChildrenAsString();
        //}
    }
}