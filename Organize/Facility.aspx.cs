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
namespace NetRadio.LocatingMonitor.Organize
{
    public partial class __Facility : BasePage
    {
        protected override void RegisterScriptFileInHead(ClientFileCollection scriptFiles)
        {
            scriptFiles.Clear();
            scriptFiles.Add("0", "App_Script/Global.js");
            scriptFiles.Add("1", "App_Script/func.js");
            scriptFiles.Add("2", "App_Script/master.js");
            scriptFiles.Add("3", "App_Script/Common.js");
            scriptFiles.Add("4", "App_Script/Project.js");
            scriptFiles.Add("5", "App_Script/UI/Facility.aspx.js");
            
            //base.RegisterScriptFileInHead(scriptFiles);
        }
        EditMode _editMode;
        int _parentId;

        public __Facility()
        {
            _editMode = Fetch.QueryUrl("action") == "addnew" ? EditMode.AddNew : EditMode.Modify;
            _parentId = Fetch.QueryUrlAsIntegerOrDefault("parentId", 0);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Sitemap.Text2 = "系统设置";
            //Sitemap.Text3 = this.Wrap.Title;

            if (!IsPostBack)
            {
                LoadCurrentFacilityForm();
                LoadFacilityTree();
            }
        }


        // Form

        #region LoadCurrentFacilityForm

        private void LoadCurrentFacilityForm()
        {
            // AddNew
            if (_editMode == EditMode.AddNew)
            {
                facilityId.Text = "(新增)";
                facilityName.Text = "";
                facilityId.CssClass += " t2";
                //save.Text = "新增";

                parentList.SelectedFacilityId = _parentId;

                parentList.Disabled = true;
                //linkOfAppendChild.Visible = false;
            }

            // Else: Modify
            if (_editMode == EditMode.Modify)
            {
                // Load facility properties
                // -----------------------------------------
                Facility facility = null;

                int id = Fetch.QueryUrlAsIntegerOrDefault("id", -1);
                if (id == -1)
                {
                    facility = Facility.All.Where(f => f.ParentFacilityId == 0).OrderByDescending(f => f.Id).FirstOrDefault();
                    if (facility == null)
                    {
                        Terminator.Redirect("Facility.aspx?action=addnew");
                    }
                    else
                    {
                        Terminator.Redirect("Facility.aspx?id=" + facility.Id);
                    }
                }
                else
                {
                    facility = Facility.All.SingleOrDefault(f => f.Id == id);
                    if (facility == null)
                    {
                        ShowMessagePage("场所节点不存在。");
                    }
                }

                facilityId.Text = facility.Id.ToString();
                facilityName.Text = facility.FacilityName;
                parentList.SelectedFacilityId = facility.ParentFacilityId;
                parentList.HideFacilityId = facility.Id;
                //save.Text = "修改";

                //linkOfAppendChild.Href = "Facility.aspx?action=addnew&parentId=" + facility.Id.ToString();
                //linkOfAppendChild.Visible = true;
            }
        }

        #endregion


        // Tree

        #region SelectFacilitiesByParentId

        private IList<Facility> SelectFacilitiesByParentId(int parentFacilityId)
        {
            IList<Facility> facilities = Facility.All.Where(f => f.ParentFacilityId == parentFacilityId).ToList();

            if (_editMode == EditMode.AddNew && parentFacilityId == _parentId)
            {
                Facility appending = new Facility();
                appending.Id = int.MinValue;
                appending.FacilityName = "新增";
                facilities.Add(appending);
            }
            return facilities;
        }

        #endregion

        #region LoadFacilityTree

        private void LoadFacilityTree()
        {
            companyIcon.Src = base.ThemePath + "/Images/Icon/bullet_company.gif";

            facilityTreeRepeater.DataSource = this.SelectFacilitiesByParentId(0);
            facilityTreeRepeater.ItemCreated += new RepeaterItemEventHandler(facilityTreeRepeater_ItemCreated);
            facilityTreeRepeater.DataBind();            
        }

        #endregion

        #region facilityTreeRepeater_ItemCreated

        protected void facilityTreeRepeater_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            Facility facility = e.Item.DataItem as Facility;
            if (facility != null)
            {
                bool isNadir = Facility.All.Count(f => f.ParentFacilityId == facility.Id) == 0;

                // Node icon
                Img nodeIcon = new Img();
                nodeIcon.Src = base.ThemePath + "/Images/Icon/treeview_" + (isNadir ? "nadir" : "expanded") + ".gif";
                nodeIcon.CssClass = "nodeIcon";
                nodeIcon.Attributes.Add("align", "absmiddle");
                if (!isNadir)
                {
                    nodeIcon.Style.Add("cursor", "pointer");
                    nodeIcon.Attributes.Add("onclick", "javascript:treeNodeClicked(this, 'n_" + facility.Id + "');");
                }
                e.Item.Controls.Add(nodeIcon);

                if (facility.Id == int.MinValue)
                {
                    e.Item.Controls.Add(new LiteralControl("<span class=\"t2 bold underline\">(新增...)</span>"));
                }
                else
                {
                    Anchor nodeAnchor = new Anchor();
                    e.Item.Controls.Add(nodeAnchor);

                    nodeAnchor.Text = facility.FacilityName;
                    nodeAnchor.Href = "Facility.aspx?id=" + facility.Id;

                    if (Fetch.QueryUrlAsIntegerOrDefault("id", -1) == facility.Id)
                        nodeAnchor.CssClass += " t2 bold underline";
                }

                e.Item.Controls.Add(new LiteralControl("<br />\r\n"));

                // Recursion
                if (!isNadir || facility.Id == _parentId)	// Is nadir || appending new child in current node.
                {
                    Div holder = new Div();
                    holder.Attributes.Add("id", "n_" + facility.Id);
                    holder.CssClass = "treeBlock";
                    e.Item.Controls.Add(holder);

                    Repeater childRepeater = new Repeater();
                    childRepeater.DataSource = this.SelectFacilitiesByParentId(facility.Id);
                    childRepeater.ItemCreated += new RepeaterItemEventHandler(facilityTreeRepeater_ItemCreated);
                    childRepeater.DataBind();
                    holder.Controls.Add(childRepeater);
                }
            }
        }

        #endregion

        protected void removeCache_Click(object sender, EventArgs e)
        {
            Cache.Remove(AppKeys.Cache_AllFacilities);
            Cache.Remove(AppKeys.Cache_AllSurveyGroups);
            Cache.Remove(AppKeys.Cache_AllMapAreas);
            LoadFacilityTree();
        }


        // PostBack

        #region Check Form

        //private void CheckFacilityForm() {
        //    if (facilityName.Text.Trim().Length == 0) {
        //        feedbacks.Items.AddError("请为该节点输入一个简短的名称。");
        //    }
        //}

        #endregion

        #region save_Click

        //protected void save_Click(object sender, EventArgs e)
        //{
        //    //Do checks
        //    this.CheckFacilityForm();
        //    if (feedbacks.HasItems)
        //    {
        //        return;
        //    }

        //    Facility facility = new Facility();
        //    facility.ParentFacilityId = parentList.SelectedFacilityId;
        //    facility.FacilityName = facilityName.Text.Trim();
        //    if (_editMode == EditMode.Modify)
        //    {
        //        Facility.UpdateById(Fetch.QueryUrlAsInteger("id"), facility.FacilityName, facility.ParentFacilityId);
        //    }
        //    else
        //    {
        //        facility.WriteTime = DateTime.Now;
        //        Facility.Insert(facility);
        //    }

        //    // Update Cache.
        //    NetRadio.Assistant.Web.Util.Caching.Remove(AppKeys.Cache_AllFacilities);

        //    // Response
        //    if (_editMode == EditMode.AddNew)
        //    {
        //        Terminator.End("添加完毕。", 2, new Link("查看新添加的记录", "Facility.aspx?id=" + facility.Id));
        //    }

        //    this.LoadFacilityTree();
        //    feedbacks.Items.AddPrompt("保存完毕。");
        //}

        #endregion
    }
}
