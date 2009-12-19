<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileManagerUserControl.ascx.cs" Inherits="Zeus.Admin.Plugins.FileManager.FileManagerUserControl" %>
<%@ Register TagPrefix="ext" Namespace="Coolite.Ext.Web" Assembly="Coolite.Ext.Web" %>

<script type="text/javascript">
	var currentDirectory;
	var lookup = {};
	
	var prepareData = function(data)
	{
		data.shortName = Ext.util.Format.ellipsis(data.name, 15);
		this.lookup[data.name] = data;
		return data;
	};
</script>

<ext:Store runat="server" ID="filesStore" AutoLoad="true">
  <Reader>
    <ext:JsonReader>
      <Fields>
				<ext:RecordField Name="name" />
				<ext:RecordField Name="size" Type="Int" />
				<ext:RecordField Name="type" />      
				<ext:RecordField Name="relativePath" />
				<ext:RecordField Name="fullPath" />      
				<ext:RecordField Name="webPath" />      
      </Fields>
    </ext:JsonReader>
  </Reader>
</ext:Store>

<ext:Window runat="server" ID="imageChooser" ShowOnLoad="false" Title="Choose An Image" Width="515" MinWidth="515" Height="400" MinHeight="400">
	<Body>
		<ext:BorderLayout runat="server">
			<West MinWidth="150" MaxWidth="250">
				<ext:TreePanel runat="server" ID="treePanel" Width="150" Animate="true" ContainerScroll="true" RootVisible="true">
					<Listeners>
						<Click Handler="currentDirectory = node.attributes.url; this.store.load({ params: { directory: node.attributes.url } });" Scope="this" />
					</Listeners>
				</ext:TreePanel>
			</West>
			<Center>
				<ext:DataView runat="server" SingleSelect="true" OverClass="x-view-over" ItemSelector="div.thumb-wrap"
					EmptyText="<div style='padding:10px;'>There are no images in this folder.</div>"
					StoreID="filesStore">
					<Template>
						<tpl for=".">
							<div class="thumb-wrap" id="{name}">
								<div class="thumb"><img src="{web_path}" title="{name}"></div>
								<span>{short_name}</span>
							</div>
						</tpl>
					</Template>
					<PrepareData Fn="prepareData" />
					<Listeners>
						<BeforeSelect Scope="this" Handler="return view.store.getRange().length > 0;" />
						<DblClick Scope="this" Handler="this.doCallback" />
					</Listeners>
				</ext:DataView>
			</Center>
		</ext:BorderLayout>
	</Body>
	<Buttons>
		<ext:Button runat="server" Text="OK" Handler="this.doCallback" />
		<ext:Button runat="server" Text="Cancel" Handler="function() { #{imageChooser}.hide(); }" />
	</Buttons>
</ext:Window>

<script type="text/javascript">
	var imageChooser;
	Ext.onReady(function()
	{
		imageChooser = Ext.getCmp("<%= imageChooser.ClientID %>");
	});
</script>