<%@ Page Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="LocatingManager.aspx.cs" Inherits="NetRadio.LocatingMonitor.Settings.__LocatingManager"%><%@ Register TagPrefix="NetRadio" TagName="ObjectNavigator" Src="~/Controls/ObjectNavigator.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CphRight" runat="server"> <NetRadio:ObjectNavigator id="objectNavigator" runat="server" />
			<div class="tabitem">
				<NetRadio:Remind id="feedbacks" autohideduration="5" runat="server" />
				<NetRadio:CollapseView id="filterView" runat="server">
					<Content>
						<div class="bg3" style="padding:5px 10px 6px 10px">
							<div>
								<asp:RadioButtonList id="stateFilter" repeatLayout="flow" repeatDirection="Horizontal" autoPostBack="true" onSelectedIndexChanged="stateFilter_SelectedIndexChanged" runat="server">
									<asp:ListItem style="padding-right:16px" selected="true">所有</asp:ListItem>
									<asp:ListItem style="padding-right:16px">正在定位</asp:ListItem>
									<asp:ListItem style="padding-right:16px">未开始定位</asp:ListItem>
									<asp:ListItem style="padding-right:16px">未设置过定位参数</asp:ListItem>
								</asp:RadioButtonList>
							</div>
						</div>
						<div class="bg3" style="padding:5px 10px; margin-top:4px">
							<div>
								名称: <asp:TextBox id="tagNameKeyword" width="130" text="关键字" style="color:gray" onfocus="javascript:Bmw.keywordFocus(this);" onblur="javascript:Bmw.keywordBlur(this);" runat="server" />
								<!-- <NetRadio:TagGroupSelector id="groupSelector" width="120" runat="server" /> -->
								<asp:Button id="filterButton" text="查询" onclick="filterButton_Click" runat="server" />
							</div>
						</div>
					</Content>
					<Footer />
				</NetRadio:CollapseView>
				
				<NetRadio:CollapseView id="listView" runat="server">
					<Header>
						<Caption>选择使用者进行开启或停止定位</Caption>
						<Subcaption><asp:LinkButton id="refresh" text="刷新" onclick="refresh_Click" runat="server" /></Subcaption>
					</Header>
					<Content>
						<table id="tagList" class="grid alternate fixed">
							<thead>
								<th width="25"><input type="checkbox" onclick="javascript:CheckboxUtil.checkAll(this); Locating.selectTag(this);" /></th>
								<th width="16"></th>
								<th width="120">使用者</th>
								<th width="120">使用者类型</th>
								<th>使用者备注</th>
								<th width="160">状态/开始时间</th>
								<th width="60">快捷操作</th>
							</thead>
							<asp:Repeater id="settingList" onitemcreated="settingList_ItemCreated" runat="server">
								<ItemTemplate>
									<tr>
										<td><NetRadio:SmartLabel id="idSelection" runat="server" /></td>
										<td><NetRadio:Img id="icon" runat="server" /></td>
										<td><NetRadio:Anchor id="tagName" target="_blank" runat="server" /></td>
										<td><NetRadio:SmartLabel id="userType" runat="server" /></td>
										<td><NetRadio:SmartLabel id="memo" runat="server" /></td>
										<td><NetRadio:SmartLabel id="commandState" runat="server" />
										<NetRadio:DateTimeLabel id="startTime" showastimespan="true" format="M-d H:mm:ss" runat="server" /></td>
										<td><NetRadio:Anchor id="operate" runat="server" /></td>
									</tr>
								</ItemTemplate>
							</asp:Repeater>
							<tr>
								<td colspan="7"><NetRadio:Pagination id="p" mode="Postback" pagesize="30" class="p_left" onpageindexchanged="p_PageIndexChanged" runat="server" /></td>
							</tr>
						</table>
					</Content>
				</NetRadio:CollapseView>
				
				<div class="vspace"></div>
				<div class="vspace"></div>
				<NetRadio:Div id="locatingServiceState" class="t2" runat="server" />
				
				
				<asp:HiddenField id="selectedTagIds" runat="server" />
				
				<!-- 漂浮层 -->
				
				<div id="floatEntry" class="floatLayer" style="display:none">
					<table cellpadding="0" cellspacing="0">
						<tr>
							<td id="selectionTitle1"></td>
							<td align="right"><a href="javascript:Locating.closeEntry();" class="underline">取消</a></td>
						</tr>
						<tr>
							<td colspan="2" style="padding-top:30px">
								<span class="t3">将选中的使用者: &nbsp;</span>
								<a href="javascript:Locating.startLocatingClicked();">启动定位</a>															   
								<span class="separator"> | </span>
								<asp:LinkButton id="stopLocating" text="停止定位" onclick="stopLocating_Click" runat="server" />
							</td>
						</tr>
					</table>
				</div>
				<div id="floatPanel" class="floatLayer" style="display:none">
					<table cellpadding="0" cellspacing="0">
						<tr>
							<td id="selectionTitle2"></td>
							<td align="right"><a href="javascript:Locating.closePanel();" class="underline">取消</a></td>
						</tr>
						<tr>
							<td colspan="2" style="padding-top:10px">
								<table cellpadding="5" cellspacing="0">
								   <tr>
								   <td >定位间隔时间</td>
								   <td ><asp:TextBox ID="locatingInterval" runat="server">300</asp:TextBox>秒</td>
								   </tr>
								   <tr><td>最大扫描次数</td>
								   <td><asp:TextBox ID="scanCount" runat="server">10</asp:TextBox>次</td></tr>
								   	<tr>
								   <td>好的信号强度</td>
								   <td><asp:TextBox ID="goodSignal" runat="server">-35</asp:TextBox></td>
								   </tr>
								   <tr>
								   <td>振动检测间隔</td>
								   <td><asp:TextBox ID="detectInterval" runat="server">5</asp:TextBox>秒</td>
								   </tr>
								   <tr>
								   <td>扫描时间</td>
								   <td><asp:TextBox ID="scanTime" runat="server">3200</asp:TextBox>秒</td>
								   </tr>								   
								   <tr>
								   <td>扫描开始时间间隔</td>
								   <td><asp:TextBox ID="wscanInterval" runat="server">10</asp:TextBox>秒</td>
								   </tr>
									<tr visible="false" runat="server">
										<td width="100">定位方式:</td>
										<td width="245">
											<asp:RadioButtonList id="locatingMode" repeatLayout="Flow" repeatDirection="Horizontal" runat="server">
												<asp:ListItem onclick="javascript:setSurveyGroupDisabled(false);" value="1" style="margin-right:8px" selected="true">采样定位</asp:ListItem>
												<asp:ListItem onclick="javascript:setSurveyGroupDisabled(true);" value="2">三角定位</asp:ListItem>
											</asp:RadioButtonList>
										</td>
									</tr>
									<tbody>
										<tr visible="false" runat="server">
											<td>选择采样组:</td>
											<td><asp:DropDownList id="surveyGroup" width="180" runat="server" /></td>
										</tr>
									</tbody>
									<tr visible="false" runat="server">
										<td>扫描方式:</td>
										<td>
											<asp:RadioButtonList id="scanMode" repeatLayout="Flow" repeatDirection="Horizontal" runat="server">
												
												<asp:ListItem value="2" onclick="javascript:setScanTargetSsidDisabled(false)" selected="true">AP扫描标签</asp:ListItem>
											</asp:RadioButtonList>
										</td>
									</tr>
									<tbody>
										<tr visible="false" runat="server">
											<td>扫描对象</td>
											<td>
												<asp:DropDownList id="scanTarget" width="180" runat="server">
													<asp:ListItem value="0">所有 AP</asp:ListItem>
													<asp:ListItem value="1">指定 SSID 的 AP</asp:ListItem>
													<asp:ListItem value="2">标识标签</asp:ListItem>
													<asp:ListItem value="3">所有 AP 和标识标签</asp:ListItem>
													<asp:ListItem value="4">指定 SSID 的 AP 和标识标签</asp:ListItem>
													<asp:ListItem value="5">定位标签</asp:ListItem>
												</asp:DropDownList>
											</td>
										</tr>
										<tr visible="false" runat="server">
											<td>指定SSID</td>
											<td><asp:TextBox id="scanSsid" width="100" runat="server" /></td>
										</tr>
									</tbody>
									<tbody>
										<tr visible="false" runat="server">
											<td>扫描信道</td>
											<td><asp:CheckBoxList id="scanChannels" RepeatLayout="Table" RepeatDirection="Horizontal" RepeatColumns="5" CellPadding="1"  runat="server">
													<asp:ListItem value="1">#1</asp:ListItem>
													<asp:ListItem value="2">#2</asp:ListItem>
													<asp:ListItem value="3">#3</asp:ListItem>
													<asp:ListItem value="4">#4</asp:ListItem>
													<asp:ListItem value="5">#5</asp:ListItem>
													<asp:ListItem value="6">#6</asp:ListItem>
													<asp:ListItem value="7">#7</asp:ListItem>
													<asp:ListItem value="8">#8</asp:ListItem>
													<asp:ListItem value="9">#9</asp:ListItem>
													<asp:ListItem value="10">#10</asp:ListItem>
													<asp:ListItem value="11">#11</asp:ListItem>
													<asp:ListItem value="12">#12</asp:ListItem>
													<asp:ListItem value="13">#13</asp:ListItem>
													<asp:ListItem value="14">#14</asp:ListItem>
												</asp:CheckBoxList></td>
										</tr>
									</tbody>
									<tr visible="false" runat="server">
										<td>其他设置</td>
										<td><asp:DropDownList id="rssiBackCount" runat="server">
												<asp:ListItem value="1">返回1个RSSI</asp:ListItem>
												<asp:ListItem value="2">返回2个RSSI</asp:ListItem>
												<asp:ListItem value="3">返回3个RSSI</asp:ListItem>
												<asp:ListItem value="4">返回4个RSSI</asp:ListItem>
												<asp:ListItem value="5">返回5个RSSI</asp:ListItem>
												<asp:ListItem value="6">返回6个RSSI</asp:ListItem>
												<asp:ListItem value="7">返回7个RSSI</asp:ListItem>
												<asp:ListItem value="8">返回8个RSSI</asp:ListItem>
												<asp:ListItem value="9">返回9个RSSI</asp:ListItem>
												<asp:ListItem value="10">返回10个RSSI</asp:ListItem>
											</asp:DropDownList>
											<asp:DropDownList id="scanInterval" runat="server">
												<asp:ListItem value="1">间隔2秒扫描</asp:ListItem>
												<asp:ListItem value="2">间隔2秒扫描</asp:ListItem>
												<asp:ListItem value="3">间隔3秒扫描</asp:ListItem>
												<asp:ListItem value="5">间隔5秒扫描</asp:ListItem>
												<asp:ListItem value="8">间隔8秒扫描</asp:ListItem>
												<asp:ListItem value="10">间隔10秒扫描</asp:ListItem>
												<asp:ListItem value="15">间隔15秒扫描</asp:ListItem>
												<asp:ListItem value="20">间隔20秒扫描</asp:ListItem>
												<asp:ListItem value="30">间隔30秒扫描</asp:ListItem>
											</asp:DropDownList></td>
									</tr>
									<tr>
										<td></td>
										<td style="padding-top:10px">
											<asp:Button id="startLocating" text="开始定位" width="80" onclientclick="javascript:return Locating.start();" onclick="startLocating_Click" runat="server" />
											&nbsp;
											<span class="t3">选中使用者按此设置定位</span></td>
									</tr>
									
								</table>
							</td>
						</tr>
					</table>
				</div>
			</div>
</asp:Content>
