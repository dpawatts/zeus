using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Isis.Web;
using Isis.Web.UI;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.AddIns.Forums.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(MessageBoard), typeof(Forum))]
	[Template("~/UI/Views/Forums/Forum.aspx")]
	public class Forum : BasePage
	{
		public override string IconUrl
		{
			get { return WebResourceUtility.GetUrl(typeof(Forum), "Zeus.AddIns.Forums.Web.Resources.comment.png"); }
		}

		[TextBoxEditor("Description", 20, TextMode = TextBoxMode.MultiLine)]
		public virtual string Description
		{
			get { return GetDetail("Description", string.Empty); }
			set { SetDetail("Description", value); }
		}

		[LinkedItemDropDownListEditor("Moderator", 30, TypeFilter = typeof(Member))]
		public virtual Member Moderator
		{
			get { return GetDetail<Member>("Moderator", null); }
			set { SetDetail("Moderator", value); }
		}

		public virtual int TopicCount
		{
			get { return Topics.Count(); }
		}

		public virtual int PostCount
		{
			get { return Topics.Sum(t => t.PostCount); }
		}

		/// <summary>
		/// Here as an optimisation so we don't need to query the DB
		/// every time we display the forum list page
		/// </summary>
		public virtual Post LastPost
		{
			get { return Topics.Select(t => t.LastPost).OrderBy(p => p.Created).LastOrDefault(); }
		}

		public MessageBoard MessageBoard
		{
			get
			{
				if (Parent is MessageBoard)
					return (MessageBoard) Parent;
				return ((Forum) Parent).MessageBoard;
			}
		}

		public string NewTopicUrl
		{
			get
			{
				return new Url(MessageBoard.NewPostPage.Url)
					.AppendQuery("f", ID)
					.AppendQuery("mode", "newTopic")
					.ToString();
			}
		}

		public IEnumerable<Topic> Topics
		{
			get { return GetChildren<Topic>(); }
		}
	}
}