using System.Collections.Generic;
using Zeus.AddIns.Blogs.ContentTypes;
using Zeus.Web.Mvc.ViewModels;
using Zeus.Linq;
using System.Linq;

namespace Zeus.AddIns.Blogs.Mvc.ViewModels
{
	public class PostViewModel : ViewModel<Post>
	{
        public PostViewModel(Post currentItem, IEnumerable<FeedbackItem> comments, IEnumerable<FeedbackItem> allComments)
			: base(currentItem)
		{
			Comments = comments;
            AllComments = allComments;
		}

        private IEnumerable<Post> _allPosts { get; set; }
        private IEnumerable<Post> AllPosts { get { 
            if (_allPosts == null)
                _allPosts = Find.EnumerateAccessibleChildren(CurrentItem.CurrentBlog)
                 .NavigablePages()
                 .OfType<Post>().OrderBy(post => post.Date);

            return _allPosts;
        } }

        private bool PreviousPostTested { get; set; }
        public bool HasPreviousPost { get {

            Post prevPost = null;
            foreach (Post post in AllPosts)
            {
                if (post.ID == CurrentItem.ID)
                { 
                    //we have a match so set prev post (if exists) and return
                    if (prevPost != null)
                    {
                        _previousPost = prevPost;
                        return true;
                    }
                    else
                        return false;
                }
                prevPost = post;
            }
            return false;
        } }

        private Post _previousPost { get; set; }
        public Post PreviousPost {
            get { 
                bool throwAway;
                if (!PreviousPostTested)
                    throwAway = HasPreviousPost;

                return _previousPost;
            }
        }



        private bool NextPostTested { get; set; }
        public bool HasNextPost
        {
            get
            {
                bool found = false;
                
                foreach (Post post in AllPosts)
                {
                    if (found)
                    {
                        _NextPost = post;
                        return true;
                    }
                    if (post.ID == CurrentItem.ID)
                    {
                        //we have a match so set flag
                        found = true;
                    }
                }
                return false;
            }
        }

        private Post _NextPost { get; set; }
        public Post NextPost
        {
            get
            {
                bool throwAway;
                if (!NextPostTested)
                    throwAway = HasNextPost;

                return _NextPost;
            }
        }

        public IEnumerable<FeedbackItem> Comments { get; set; }
        public IEnumerable<FeedbackItem> AllComments { get; set; }
		public string CaptchaError { get; set; }
	}
}