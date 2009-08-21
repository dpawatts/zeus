using System.Web;
using System.Web.Mvc;
using Isis.ExtensionMethods.IO;
using Isis.Web;
using Zeus.AddIns.Forums.ContentTypes;
using Zeus.FileSystem.Images;
using Zeus.Web;

namespace Zeus.AddIns.Forums.Mvc.Controllers
{
	[Controls(typeof(Member), AreaName = ForumsWebPackage.AREA_NAME)]
	public class MemberController : BaseForumController<Member>
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
				ModelState.AddModelError("nickname", "Nickname is required");
				return View("UpdateProfile");
			}

			CurrentMember.Title = nickname;
			CurrentMember.FullName = fullName;
			CurrentMember.Signature = signature;

			if (avatar != null && avatar.ContentLength > 0)
			{
				Image image = new Image
				{
					Name = avatar.FileName,
					Data = avatar.InputStream.ReadAllBytes(),
					ContentType = avatar.ContentType,
					Size = avatar.ContentLength
				};
				image.AddTo(CurrentMember);
				CurrentMember.Avatar = image;
			}

			Context.Persister.Save(CurrentMember);

			return View("Index");
		}
	}
}