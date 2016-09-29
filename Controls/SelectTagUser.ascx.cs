using System;
using System.Collections;
using System.Collections.Generic;
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
namespace NetRadio.LocatingMonitor.Controls
{

    /// <summary>
    /// 标签用户选择控件，lyz 2010-3-5
    /// </summary>
    public partial class __SelectTagUser : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Ajax.AjaxManager.RegisterClass(typeof(__SelectTagUser));
            Ajax.AjaxManager.RegisterClass(typeof(Model.TagUser2));
        }
        private Assembly ass = Assembly.Load("NetRadio.Business");
        /// <summary>
        /// 设置左边标题名称
        /// </summary>
        public string TitleLeft
        {
            set
            {
                labLeft.Text = value;
            }
        }
        /// <summary>
        /// 设置右边标题名称
        /// </summary>
        public string TitleRight
        {
            set
            {
                labRight.Text = value;
            }
        }
        /// <summary>
        /// 设置右边列表最多存放的数目
        /// </summary>
        public int MaxSelectCount
        {
            set
            {
                maxLength.Value = value.ToString();
            }
        }
        /// <summary>
        /// 获取被选择的用户的id
        /// </summary>
        public int[] SelectedUserIds
        {
            get
            {
                return (from _d in SelectedTagUsers.AsEnumerable()
                        select _d.UserID).ToArray();
            }
        }
        /// <summary>
        /// 获取被选择的用户
        /// </summary>
        public Model.TagUser[] SelectedTagUsers
        {
            get
            {
                if (selectedUserIds.Value == "null")
                {
                    return GetTagUserSource_Right_Distinct(methodGetUserRight.Value);
                }
                else
                {
                    string[] UserIds = selectedUserIds.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    return GetTagUserSource_Left_Distinct(methodGetUserLeft.Value).Where(_d => UserIds.Contains(_d.UserID.ToString())).ToArray();
                }
            }
        }
        class TagUseComparer : IEqualityComparer<Model.TagUser>
        {
            #region IEqualityComparer<TagUser> Members

            public bool Equals(NetRadio.Model.TagUser x, NetRadio.Model.TagUser y)
            {
                return x.UserID == y.UserID;
            }

            public int GetHashCode(NetRadio.Model.TagUser obj)
            {
                return obj.UserID.GetHashCode();
            }

            #endregion
        }

        public bool Visible_HrefSelectUser
        {
            set
            {
                selectorLauncher.Visible = value;
            }
        }

        public bool Visible_selectedList
        {
            set
            {
                selectedList.Visible = value;
            }
        }
        public bool Visible_divGroup
        {
            set
            {
                divGroup.Style["display"] = value ? "inline" : "none";
            }
        }



        public bool AutoLoadData = true;

        [Ajax.AjaxMethod(Ajax.SessionState.ReadWrite)]
        public Model.TagUser[] GetTagUserSource_Left(string methodMes)
        {
            //if (string.IsNullOrEmpty(methodMes))
            //{
            //    return new NetRadio.Model.TagUser[0];
            //}
            //string[] names = methodMes.Split(new string[] { "!#" }, StringSplitOptions.RemoveEmptyEntries);
            //string _classTypeName = names[0];
            //string _methodName = names[1];

            //if (names.Length == 3)//含有参数
            //{
            //    MethodInfo mi = ass.GetType(_classTypeName).GetMethod(_methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, new Type[] { typeof(String) }, null);
            //    Model.TagUser[] tu = mi.Invoke(Activator.CreateInstance(Type.GetType(_classTypeName)), new object[] { names[2] }) as Model.TagUser[];
            //    return tu;
            //}
            //else
            //{
            //    MethodInfo mi = ass.GetType(_classTypeName).GetMethod(_methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            //    Model.TagUser[] tu = mi.Invoke(Activator.CreateInstance(Type.GetType(_classTypeName)), new object[] { }) as Model.TagUser[];
            //    return tu;
            //}
            if (string.IsNullOrEmpty(methodMes))
            {
                return new NetRadio.Model.TagUser[0];
            }
            string[] names = methodMes.Split(new string[] { "!#" }, StringSplitOptions.RemoveEmptyEntries);
            string _classTypeName = names[0];
            string _methodName = names[1];

            if (names.Length == 3)//含有参数
            {
                MethodInfo mi = Type.GetType(_classTypeName).GetMethod(_methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, new Type[] { typeof(String) }, null);
                Model.TagUser[] tu = mi.Invoke(Activator.CreateInstance(Type.GetType(_classTypeName)), new object[] { names[2] }) as Model.TagUser[];
                return tu;
            }
            else
            {
                MethodInfo mi = Type.GetType(_classTypeName).GetMethod(_methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                Model.TagUser[] tu = mi.Invoke(Activator.CreateInstance(Type.GetType(_classTypeName)), new object[] { }) as Model.TagUser[];
                return tu;
            }
        }

        [Ajax.AjaxMethod(Ajax.SessionState.ReadWrite)]
        public Model.TagUser[] GetTagUserSource_Right(string methodMes)
        {
            if (string.IsNullOrEmpty(methodMes))
            {
                return new NetRadio.Model.TagUser[0];
            }
            string[] names = methodMes.Split(new string[] { "!#" }, StringSplitOptions.RemoveEmptyEntries);
            string _classTypeName = names[0];
            string _methodName = names[1];
            if (names.Length == 3)//含有参数
            {
                MethodInfo mi = Type.GetType(_classTypeName).GetMethod(_methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, new Type[] { typeof(String) }, null);
                Model.TagUser[] tu = mi.Invoke(Activator.CreateInstance(Type.GetType(_classTypeName)), new object[] { names[2] }) as Model.TagUser[];
                return tu;
            }
            else
            {
                MethodInfo mi = Type.GetType(_classTypeName).GetMethod(_methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                Model.TagUser[] tu = mi.Invoke(Activator.CreateInstance(Type.GetType(_classTypeName)), null) as Model.TagUser[];
                return tu;
            }
        }

        [Ajax.AjaxMethod(Ajax.SessionState.ReadWrite)]
        public Model.TagUser[] GetTagUserSource_Left_Distinct(string methodMes)
        {
            return GetTagUserSource_Left(methodMes).Distinct(new TagUseComparer()).ToArray();
        }

        [Ajax.AjaxMethod(Ajax.SessionState.ReadWrite)]
        public Model.TagUser[] GetTagUserSource_Right_Distinct(string methodMes)
        {
            return GetTagUserSource_Right(methodMes).Distinct(new TagUseComparer()).ToArray();
        }

        [Ajax.AjaxMethod(Ajax.SessionState.ReadWrite)]
        public Model.TagUser[] GetTagUserSource_Right_Distinct_ForLoad(string lMethodMes, string rMethodMes, string userIds)
        {
            if (userIds == "null")
            {
                return GetTagUserSource_Right_Distinct(rMethodMes);
            }
            else
            {
                string[] UserIds = userIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                return GetTagUserSource_Left_Distinct(lMethodMes).Where(_d => UserIds.Contains(_d.UserID.ToString())).ToArray();
            }
        }

        [Ajax.AjaxMethod(Ajax.SessionState.ReadWrite)]
        public Model.TagUser[] SearchTagUserSource_Left(string methodMes, string groupIDs, string userName)
        {
            Model.TagUser[] tu = GetTagUserSource_Left(methodMes);
            if (!(groupIDs == null || groupIDs == "" || groupIDs == "未指定组" || groupIDs.Length <= 5))//"已选组: "
            {
                int[] gIDs = Strings.ParseToArray<int>(groupIDs.Substring(5));
                tu = tu.Where(_d => gIDs.Contains(_d.GroupID)).ToArray();
            }
            if (!string.IsNullOrEmpty(userName))
            {
                tu = tu.Where(_d => _d.UserName.IndexOf(userName) >= 0).ToArray();
            }
            return tu;
        }

        [Ajax.AjaxMethod(Ajax.SessionState.ReadWrite)]
        public Model.TagUser[] SearchTagUserSource_Right(string methodMes, string userName)
        {
            Model.TagUser[] tu = GetTagUserSource_Right(methodMes);
            if (!string.IsNullOrEmpty(userName))
            {
                tu = tu.Where(_d => _d.UserName.IndexOf(userName) >= 0).ToArray();
            }
            return tu;
        }

        [Ajax.AjaxMethod(Ajax.SessionState.ReadWrite)]
        public Model.TagUser[] SearchTagUserSource_Left_Distinct(string methodMes, string groupIDs, string userName)
        {
            return SearchTagUserSource_Left(methodMes, groupIDs, userName).Distinct(new TagUseComparer()).ToArray();
        }

        [Ajax.AjaxMethod(Ajax.SessionState.ReadWrite)]
        public Model.TagUser[] SearchTagUserSource_Right_Distinct(string methodMes, string userName)
        {
            return SearchTagUserSource_Right(methodMes, userName).Distinct(new TagUseComparer()).ToArray();
        }

        public delegate Model.TagUser[] SearchHandler();
        public delegate Model.TagUser[] SearchHandlerParams(string para);
        public void SetDataSourceLeft(SearchHandler searchHandler)
        {
            Delegate[] methods = searchHandler.GetInvocationList();
            MethodInfo mi = methods[methods.Length - 1].Method;
            Type classType = mi.ReflectedType;
            string classTypeName = classType.FullName;
            string methodName = mi.Name;
            methodGetUserLeft.Value = classTypeName + "!#" + methodName;
        }
        public void SetDataSourceRight(SearchHandler searchHandler)
        {
            Delegate[] methods = searchHandler.GetInvocationList();
            MethodInfo mi = methods[methods.Length - 1].Method;
            Type classType = mi.ReflectedType;
            string classTypeName = classType.FullName;
            string methodName = mi.Name;
            methodGetUserRight.Value = classTypeName + "!#" + methodName;
        }
        public void SetDataSourceLeft(SearchHandlerParams searchHandler, string para)
        {
            Delegate[] methods = searchHandler.GetInvocationList();
            MethodInfo mi = methods[methods.Length - 1].Method;
            Type classType = mi.ReflectedType;
            string classTypeName = classType.FullName;
            string methodName = mi.Name;
            methodGetUserLeft.Value = classTypeName + "!#" + methodName + "!#" + para;
        }
        public void SetDataSourceRight(SearchHandlerParams searchHandler, string para)
        {
            Delegate[] methods = searchHandler.GetInvocationList();
            MethodInfo mi = methods[methods.Length - 1].Method;
            Type classType = mi.ReflectedType;
            string classTypeName = classType.FullName;
            string methodName = mi.Name;
            methodGetUserRight.Value = classTypeName + "!#" + methodName + "!#" + para;
        }






        /* 静态方法这里用于向页面提供数据源，最好放在业务层，lyz */

        private static string totalTagUserSQL = " select distinct g.HostGroupId,h.HostId,h.HostName,h.TagId from object_HostGroup g,object_HostTag h where h.hostid=g.hostid ";

        /// <summary>
        /// 获取所有组的用户
        /// </summary>
        /// <returns></returns>
        public static Model.TagUser2[] GetAllTagUsers()
        {
            DataTable dt = Summer.Query.RunQuerySQLString(totalTagUserSQL, "LocatingMonitor");
            return (
                from _d in dt.AsEnumerable()
                select new Model.TagUser2
                {
                    GroupID = Convert.ToInt32(_d["hostgroupid"]),
                    UserID = Convert.ToInt32(_d["HostId"]),
                    UserName = _d["HostName"].ToString(),
                    TagID = Convert.ToInt32(_d["TagId"])
                }
                ).ToArray();
        }
        /// <summary>
        /// tag用户分组页面使用
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public static Model.TagUser2[] SelectTagUsers0(string groupId)
        {
            DataTable dt = Summer.Query.RunQuerySQLString(
                           string.Format(totalTagUserSQL + " and g.hostgroupid={0}",
                           groupId), "LocatingMonitor");
            return (
                from _d in dt.AsEnumerable()
                select new Model.TagUser2
                {
                    GroupID = (_d["hostgroupid"] == null || _d["hostgroupid"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["hostgroupid"]),
                    UserID = (_d["HostId"] == null || _d["HostId"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["HostId"]),
                    UserName = (_d["HostName"] == null || _d["HostName"] == DBNull.Value) ? "" : _d["HostName"].ToString(),
                    TagID = (_d["TagId"] == null || _d["TagId"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["TagId"])
                }
                ).ToArray();
        }
        /// <summary>
        /// 轨迹回放页面使用
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Model.TagUser2[] SelectTagUsers1()
        {
            //var query = (HostTagGroupStatus.All
            //       .Where(u => u.TagId > 0 && u.ParentGroupId == 0 && !u.HostName.Contains("受虐"))
            //       .Select(u => new
            //       {
            //           groupID=u.HostGroupId,
            //           Id = u.HostId,
            //           TagName = u.HostName
            //       })).Distinct();

            ////if (!string.IsNullOrEmpty(userName))
            ////{
            ////    query = query.Where(t => t.TagName.IndexOf(userName.Trim()) >= 0);
            ////}
            //return (from _d in query
            //        select new Model.TagUser
            //        {
            //            GroupID= Convert.ToInt32(_d.groupID),
            //            UserID = Convert.ToInt32(_d.Id),
            //            UserName = _d.TagName
            //        }).ToArray();

            DataTable dt = Summer.Query.RunQuerySQLString(totalTagUserSQL + "and h.tagid>0", "LocatingMonitor");
            return (
                from _d in dt.AsEnumerable()
                select new Model.TagUser2
                {
                    GroupID = (_d["hostgroupid"] == null || _d["hostgroupid"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["hostgroupid"]),
                    UserID = (_d["HostId"] == null || _d["HostId"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["HostId"]),
                    UserName = (_d["HostName"] == null || _d["HostName"] == DBNull.Value) ? "" : _d["HostName"].ToString(),
                    TagID = (_d["TagId"] == null || _d["TagId"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["TagId"])
                }
                ).Where(_d=>_d.GroupID==1||_d.GroupID==2).ToArray();//2010-11-27bydyp
        }
        /// <summary>
        /// 区域规则页面使用
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Model.TagUser2[] SelectTagUsers2()
        {
            DataTable dt = Summer.Query.RunQuerySQLString(totalTagUserSQL + "and h.tagid>0", "LocatingMonitor");
            return (
                from _d in dt.AsEnumerable()
                select new Model.TagUser2
                {
                    GroupID = (_d["hostgroupid"] == null || _d["hostgroupid"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["hostgroupid"]),
                    UserID = (_d["HostId"] == null || _d["HostId"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["HostId"]),
                    UserName = (_d["HostName"] == null || _d["HostName"] == DBNull.Value) ? "" : _d["HostName"].ToString(),
                    TagID = (_d["TagId"] == null || _d["TagId"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["TagId"])
                }
                ).ToArray();
        }
        /// <summary>
        /// 报表进出统计页面使用
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Model.TagUser2[] SelectTagUsers3()
        {
            DataTable dt = Summer.Query.RunQuerySQLString(totalTagUserSQL + "and h.tagid>0", "LocatingMonitor");
            return (
                from _d in dt.AsEnumerable()
                select new Model.TagUser2
                {
                    GroupID = (_d["hostgroupid"] == null || _d["hostgroupid"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["hostgroupid"]),
                    UserID = (_d["HostId"] == null || _d["HostId"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["HostId"]),
                    UserName = (_d["HostName"] == null || _d["HostName"] == DBNull.Value) ? "" : _d["HostName"].ToString(),
                    TagID = (_d["TagId"] == null || _d["TagId"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["TagId"])
                }
                ).Where(_d => _d.GroupID == 1 || _d.GroupID == 2).ToArray();//2010-11-28bydyp
        }
        /// <summary>
        ///巡逻统计页面使用
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Model.TagUser2[] SelectTagUsers4()
        {
            //var query = (HostTagGroupStatus.All
            //       .Where(u => u.TagId > 0 && u.ParentGroupId == 0 && !u.HostName.Contains("受虐"))
            //       .Select(u => new
            //       {
            //           groupID=u.HostGroupId,
            //           Id = u.HostId,
            //           TagName = u.HostName
            //       })).Distinct();

            ////if (!string.IsNullOrEmpty(userName))
            ////{
            ////    query = query.Where(t => t.TagName.IndexOf(userName.Trim()) >= 0);
            ////}
            //return (from _d in query
            //        select new Model.TagUser
            //        {
            //            GroupID= Convert.ToInt32(_d.groupID),
            //            UserID = Convert.ToInt32(_d.Id),
            //            UserName = _d.TagName
            //        }).ToArray();

            DataTable dt = Summer.Query.RunQuerySQLString(totalTagUserSQL + "and h.tagid>0", "LocatingMonitor");
            return (
                from _d in dt.AsEnumerable()
                select new Model.TagUser2
                {
                    GroupID = (_d["hostgroupid"] == null || _d["hostgroupid"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["hostgroupid"]),
                    UserID = (_d["HostId"] == null || _d["HostId"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["HostId"]),
                    UserName = (_d["HostName"] == null || _d["HostName"] == DBNull.Value) ? "" : _d["HostName"].ToString(),
                    TagID = (_d["TagId"] == null || _d["TagId"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["TagId"])
                }
                ).Where(_d => _d.GroupID == 1).ToArray();
        }
        /// <summary>
        ///区域报警规则页面使用
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Model.TagUser2[] SelectTagUsers5()
        {
            //var query = (HostTagGroupStatus.All
            //       .Where(u => u.TagId > 0 && u.ParentGroupId == 0 && !u.HostName.Contains("受虐"))
            //       .Select(u => new
            //       {
            //           groupID=u.HostGroupId,
            //           Id = u.HostId,
            //           TagName = u.HostName
            //       })).Distinct();

            ////if (!string.IsNullOrEmpty(userName))
            ////{
            ////    query = query.Where(t => t.TagName.IndexOf(userName.Trim()) >= 0);
            ////}
            //return (from _d in query
            //        select new Model.TagUser
            //        {
            //            GroupID= Convert.ToInt32(_d.groupID),
            //            UserID = Convert.ToInt32(_d.Id),
            //            UserName = _d.TagName
            //        }).ToArray();

            DataTable dt = Summer.Query.RunQuerySQLString(totalTagUserSQL + "and h.tagid>0", "LocatingMonitor");
            return (
                from _d in dt.AsEnumerable()
                select new Model.TagUser2
                {
                    GroupID = (_d["hostgroupid"] == null || _d["hostgroupid"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["hostgroupid"]),
                    UserID = (_d["HostId"] == null || _d["HostId"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["HostId"]),
                    UserName = (_d["HostName"] == null || _d["HostName"] == DBNull.Value) ? "" : _d["HostName"].ToString(),
                    TagID = (_d["TagId"] == null || _d["TagId"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["TagId"])

                }
                ).ToArray();
        }

        /// <summary>
        ///上海巡逻统计页面使用
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static Model.TagUser2[] SelectTagUsers6()
        {
            //var query = (HostTagGroupStatus.All
            //       .Where(u => u.TagId > 0 && u.ParentGroupId == 0 && !u.HostName.Contains("受虐"))
            //       .Select(u => new
            //       {
            //           groupID=u.HostGroupId,
            //           Id = u.HostId,
            //           TagName = u.HostName
            //       })).Distinct();

            ////if (!string.IsNullOrEmpty(userName))
            ////{
            ////    query = query.Where(t => t.TagName.IndexOf(userName.Trim()) >= 0);
            ////}
            //return (from _d in query
            //        select new Model.TagUser
            //        {
            //            GroupID= Convert.ToInt32(_d.groupID),
            //            UserID = Convert.ToInt32(_d.Id),
            //            UserName = _d.TagName
            //        }).ToArray();

            DataTable dt = Summer.Query.RunQuerySQLString(totalTagUserSQL + "and h.tagid>0", "LocatingMonitor");
            return (
                from _d in dt.AsEnumerable()
                select new Model.TagUser2
                {
                    GroupID = (_d["hostgroupid"] == null || _d["hostgroupid"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["hostgroupid"]),
                    UserID = (_d["HostId"] == null || _d["HostId"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["HostId"]),
                    UserName = (_d["HostName"] == null || _d["HostName"] == DBNull.Value) ? "" : _d["HostName"].ToString(),
                    TagID = (_d["TagId"] == null || _d["TagId"] == DBNull.Value) ? 0 : Convert.ToInt32(_d["TagId"])
                }
                ).Where(_d => _d.GroupID == 1).ToArray();
        }


    }
}