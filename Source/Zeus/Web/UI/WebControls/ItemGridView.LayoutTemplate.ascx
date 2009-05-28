<div class="gridToolbar">
	<div class="deleteWrap">
		<a href="#" class="delete">Delete Checked</a>
		<asp:LinkButton runat="server" Text="Confirm" CommandName="Delete" CssClass="confirm" />
		<a href="#" class="cancel">cancel</a>
	</div>
	<div class="pageNo">
		<asp:DataPager runat="server" ID="dpgEntities1" PageSize="25">
			<Fields>
				<isis:GooglePagerField NextPageImageUrl="/webResources/zeus/admin/Assets/Images/View/button_arrow_right.gif"
					PreviousPageImageUrl="/webResources/zeus/admin/Assets/Images/View/button_arrow_left.gif" />
			</Fields>
		</asp:DataPager>
	</div>
</div><div class="divide">
	<!-- -->
</div>
<table runat="server" id="dataTable" class="tb">
	<tr class="titles">
		<th class="check"><img src="/webResources/zeus/admin/assets/images/view/littleTick.gif" border="0" alt="Tick" /></th>
		<th class="edit"><!-- --></th>
	</tr>
	<tr runat="server" ID="itemPlaceholder" />
</table>
<div class="divide">
	<!-- -->
</div><div class="gridToolbar">
	<div class="deleteWrap">
		<a href="#" class="delete">Delete Checked</a>
		<asp:LinkButton runat="server" Text="Confirm" CommandName="Delete" CssClass="confirm" />
		<a href="#" class="cancel">cancel</a>
	</div>
	<div class="pageNo">
		<asp:DataPager runat="server" ID="dpgEntities2" PageSize="25">
			<Fields>
				<isis:GooglePagerField NextPageImageUrl="/webResources/zeus/admin/Assets/Images/View/button_arrow_right.gif"
					PreviousPageImageUrl="/webResources/zeus/admin/Assets/Images/View/button_arrow_left.gif" />
			</Fields>
		</asp:DataPager>
	</div>
</div>