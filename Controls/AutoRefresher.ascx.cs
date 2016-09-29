using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using NetRadio.Common.LocatingMonitor;
using NetRadio.Assistant.Web.Util;

namespace NetRadio.LocatingMonitor.Controls
{
	public partial class __AutoRefresher : UserControl
	{
		protected void Page_Load(object sender, EventArgs e) {
			autoRefreshDurationText.Text = autoRefreshDuration.Value + "秒";
			this.RegisterAutoPostBackScript();			
            this.Page.ClientScript.RegisterClientScriptInclude("xx_" + DateTime.Now.ToString("yyyyMMddhhmmss") + new Random().Next(0, 10000), NetRadio.Web.WebPath.GetFullPath("App_Script/UI/AutoRefresher.ascx.js"));
		}

        protected override void OnInit(EventArgs e)
        {
            Page.PreLoad += new EventHandler(Page_PreLoad);
            base.OnInit(e);
        }

        void Page_PreLoad(object sender, EventArgs e)
        {
            if (Request.Form["autoRefreshFlag"] == "1")
            {
                OnRefresh(null);
            }
        }

        

		#region Event: OnRefresh

		static readonly object EventRefresh = new object();

		public event EventHandler<EventArgs> Refresh {
			add {
				base.Events.AddHandler(EventRefresh, value);
			}
			remove {
				base.Events.RemoveHandler(EventRefresh, value);
			}
		}

		bool OnRefresh(EventArgs e) {
			EventHandler<EventArgs> handler = (EventHandler<EventArgs>)base.Events[EventRefresh];
			if (handler == null) {
				return false;
			}
			handler(this, e);
			return true;
		}

		#endregion

		public int DefaultDuration {
			get {
				return int.Parse(autoRefreshDuration.Value);
			}
			set {
				autoRefreshDuration.Value = value.ToString();
			}
		}

		private void RegisterAutoPostBackScript() {
			var scriptCode = @"
				setTimeout(
					doRefresh,
					parseInt($('" + autoRefreshDuration.ClientID + @"').value) * 1000
				);
			";
			ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Auto_Refresh_Timer", scriptCode, true);
		}
	}
}