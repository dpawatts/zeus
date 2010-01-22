using System;
using System.Text;
using Ext.Net;
using Newtonsoft.Json;

namespace Coolite.Ext.UX
{
	public class IconComboListItemCollectionJsonConverter : JsonConverter
	{
		private readonly string _iconClsField;

		public IconComboListItemCollectionJsonConverter(string iconClsField)
		{
			_iconClsField = iconClsField;
		}

		public override void WriteJson(JsonWriter writer, object value)
		{
			ListItemCollection<ListItem> items = value as ListItemCollection<ListItem>;

			StringBuilder sb = new StringBuilder("new Ext.data.SimpleStore({fields:[\"text\",\"value\",\"" + _iconClsField + "\"],data :[");
			if (items != null && items.Count > 0)
			{
				foreach (IconComboListItem item in items)
				{
					sb.Append("[");
					sb.Append(JSON.Serialize(item.Text));
					sb.Append(",");
					sb.Append(JSON.Serialize(item.Value));
					sb.Append(",");
					sb.Append(JSON.Serialize(item.IconCls));
					sb.Append("],");
				}
				sb.Remove(sb.Length - 1, 1);
			}

			sb.Append("]})");

			writer.WriteRawValue(sb.ToString());
		}

		public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType)
		{
			throw new System.NotImplementedException();
		}

		public override bool CanConvert(Type objectType)
		{
			return typeof(ListItemCollection<ListItem>).IsAssignableFrom(objectType);
		}
	}
}