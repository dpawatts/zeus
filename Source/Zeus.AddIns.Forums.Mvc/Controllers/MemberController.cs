using System.Web;
using System.Web.Mvc;
using Isis.ExtensionMethods.IO;
using Isis.Web;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.FileSystem.Images;
using Zeus.Web;

namespace Zeus.AddIns.Forums.Mvc.Controllers
{
	[Controls(typeof(Member))]
	public class MemberController : BaseForumController<Member, IMemberViewData>
	{
		protected override MessageBoard CurrentMessageBoard
		{
			get { return CurrentItem.Parent.Parent as MessageBoard; }
		}

		protected override string SearchUrl
		{
			get { return new Url(CurrentMessageBoard.Url).AppendSegment("search"); }
		}

		public override ActionResult Index()
		{
			return View("Index");
		}

		[AcceptVerbs(HttpVerbs.Get)]
		public ActionResult UpdateProfile()
		{
			if (CurrentMember == null)
				return View("Index");

			ViewData["nickname"] = CurrentMember.Title;
			ViewData["fullName"] = CurrentMember.FullName;
			ViewData["signature"] = CurrentMember.Signature;
			return View("UpdateProfile");
		}

		[AcceptVerbs(HttpVerbs.Post)]
		public ActionResult UpdateProfile(string nickname, string fullName, string signature, HttpPostedFileBase avatar)
		{
			if (string.IsNullOrEmpty(nickname))
			{
				ModelState["nickname"].Errors.Add("Nickname is required");
				return View("UpdateProfile");
			}

			CurrentMember.Title = nickname;
			CurrentMember.FullName = fullName;
			CurrentMember.Signature = signature;

			if (avatar != null && avatar.ContentLength > 0)
			{
				ImageData image = new ImageData
				{
					Data = avatar.InputStream.ReadAllBytes(),
					ContentType = avatar.ContentType,
					Size = avatar.ContentLength
				};
				CurrentMember.Avatar = image;
			}

			Context.Persister.Save(CurrentMember);

			return View("Index");
		}
	}

	public interface IMemberViewData : IBaseForumViewData<Member>
	{
		
	}
}