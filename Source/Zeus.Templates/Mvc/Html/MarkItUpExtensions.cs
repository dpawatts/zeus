using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Zeus.Web.Mvc.Html;

namespace Zeus.Templates.Mvc.Html
{
	public static class MarkItUpExtensions
	{
		public static string MarkItUpResources(this HtmlHelper helper, bool ensureJQuery)
		{
			StringBuilder result = new StringBuilder();
			if (ensureJQuery)
				result.Append(helper.RegisterJQuery());
			result.Append(helper.Javascript("/assets/templates/typewatch/jquery.typewatch.js"));
			result.Append(helper.Javascript("/assets/templates/markitup/jquery.markitup.js"));
			result.Append(helper.Javascript("/assets/templates/markitup/sets/bbcode/set.js"));
			result.Append(helper.Css("/assets/templates/markitup/skins/simple/style.css"));
			result.Append(helper.Css("/assets/templates/markitup/sets/bbcode/style.css"));
			return result.ToString();
		}

		public static string MarkItUpTextArea(this HtmlHelper htmlHelper, string name, string id)
		{
			var result = new StringBuilder();
			result.Append("<div class=\"markItUpTextArea\">" + htmlHelper.TextArea(name) + "</div>");
			result.Append("<div class=\"markItUpPreviewContainer\"><div id=\"" + id + "Preview\"></div></div>");
			result.AppendFormat(@"<script type=""text/javascript"">/* <![CDATA[ */
					$(document).ready(function() {{
						$('#{0}').markItUp(mySettings);
		        
						var previewSeq = 0;
						function reloadPreview() {{
							var bbCode = $('#{0}').val();
							var seq = ++previewSeq;
							$.post(
								'/services/templates/bbcode',
								{{ bbCode: bbCode }},
								function(data, textStatus) {{
									if (seq >= previewSeq && data != '') {{
										$('#{0}Preview').html(data);
										//prettyPrint();
									}}
								}},
								'html');
						}};
		               
						$('#{0}').typeWatch(
						{{
								callback: reloadPreview,
								wait: 250,
								highlight: false,
								captureLength: -1
						}});
		        
						reloadPreview();
					}});
				/* ]]> */</script>", id);
			return result.ToString();
		}
	}
}