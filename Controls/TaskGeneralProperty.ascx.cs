using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using NetRadio.Common.LocatingMonitor;
using NetRadio.Assistant.Web.Util;

namespace NetRadio.LocatingMonitor.Controls
{
    public partial class __TaskGeneralProperty : UserControl
	{
		public DateTime ExecuteTime {
			get {
				string merge = executeDate.Text + " " + executeHour.SelectedValue + ":" + executeMinute.Text.PadLeft(2, '0') + ":00";

				DateTime dt;
				if (executeDate.Text.Length > 0 && DateTime.TryParse(merge, out dt)) {
					return dt;
				}
				return DateTime.Now.AddMinutes(10);
			}
			set {
				executeDate.Text = value.ToString("yyyy-M-d");
				executeHour.SelectedIndex = value.Hour;
				executeMinute.Text = value.ToString("mm");
			}
		}

		public TaskRepeat Period {
			get {
				return (TaskRepeat)int.Parse(period.SelectedValue);
			}
			set {
				period.SelectedIndex = (int)value;
			}
		}

		public string Memo {
			get {
				return memo.Text;
			}
			set {
				memo.Text = value.Trim();
			}
		}

		protected void Page_Load(object sender, EventArgs e) {
			if (!Page.IsPostBack && executeDate.Text.Length == 0) {
				this.ExecuteTime = DateTime.Now.AddMinutes(10);
			}
		}

		public string CheckSettings() {
			if (Period == TaskRepeat.NoRepeat && ExecuteTime <= DateTime.Now) {
				return "任务设置为不重复执行，但设置的执行时间已过期";
			}
			if (Memo.Length > 200) {
				return "备注说明的内容长度不能超过200个字符，目前为" + Memo.Length + "个字符";
			}
			return string.Empty;
		}
	}
}