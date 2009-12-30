<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileManagerUserControl.ascx.cs" Inherits="Zeus.Admin.Plugins.FileManager.FileManagerUserControl" %>
<%@ Register TagPrefix="ext" Namespace="Coolite.Ext.Web" Assembly="Coolite.Ext.Web" %>

<ext:Store runat="server" ID="filesStore" AutoLoad="true" OnRefreshData="filesStore_RefreshData">
	<AjaxEventConfig Url="/admin/default.aspx" />
  <Reader>
    <ext:JsonReader>
      <Fields>
				<ext:RecordField Name="name" />
				<ext:RecordField Name="url" />
				<ext:RecordField Name="imageUrl" />
      </Fields>
    </ext:JsonReader>
  </Reader>
</ext:Store>

<ext:Window runat="server" ID="imageChooser" ShowOnLoad="false" Title="Choose A File" Width="515" MinWidth="515" Height="400" MinHeight="400" Modal="true">
	<Body>
		<ext:BorderLayout runat="server">
			<West MinWidth="150" MaxWidth="250" Split="true">
				<ext:TreePanel runat="server" ID="treePanel" Width="150" Animate="true" ContainerScroll="true" RootVisible="true" Border="false">
					<Listeners>
						<Click Handler="currentNode = node.attributes.id; #{filesStore}.reload({ params: { node: node.attributes.id, type: fileManager.type } });" />
					</Listeners>
				</ext:TreePanel>
			</West>
			<Center>
				<ext:Panel runat="server" AutoScroll="true" Cls="img-chooser-view">
					<Body>
						<ext:ContainerLayout runat="server">
							<ext:DataView runat="server" ID="imageChooserView" SingleSelect="true" OverClass="x-view-over" ItemSelector="div.thumb-wrap"
								EmptyText="<div style='padding:10px;'>There are no files in this folder.</div>"
								StoreID="filesStore">
								<Template>
									<tpl for=".">
										<div class="thumb-wrap" id="{name}">
											<div class="thumb"><img src="{imageUrl}" title="{name}"></div>
											<span>{shortName}</span>
										</div>
									</tpl>
								</Template>
								<PrepareData Fn="function(data) { return fileManager.formatData(data); }" />
								<Listeners>
									<BeforeSelect Handler="return this.store.getRange().length > 0;" />
									<DblClick Handler="fileManager.doCallback();" />
								</Listeners>
							</ext:DataView>
						</ext:ContainerLayout>
					</Body>
				</ext:Panel>
			</Center>
		</ext:BorderLayout>
	</Body>
	<Buttons>
		<ext:Button runat="server" Text="OK">
			<Listeners>
				<Click Handler="fileManager.doCallback();" />
			</Listeners>
		</ext:Button>
		<ext:Button runat="server" Text="Cancel">
			<Listeners>
				<Click Handler="fileManager.hide();" />
			</Listeners>
		</ext:Button>
	</Buttons>
</ext:Window>

<script type="text/javascript">
	var currentNode, fileManager, imageChooserView;
	Ext.onReady(function()
	{
		var imageChooser = Ext.getCmp("<%= imageChooser.ClientID %>");
		imageChooserView = Ext.getCmp("<%= imageChooserView.ClientID %>");
		var treePanel = Ext.getCmp("<%= treePanel.ClientID %>");

		fileManager = new Ext.ux.zeus.FileManager(imageChooser, imageChooserView, treePanel);
	});
</script>