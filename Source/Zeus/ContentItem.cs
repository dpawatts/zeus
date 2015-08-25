using System;
using System.Text;
using Ext.Net;
using Zeus.Admin;
using Zeus.ContentProperties;
using Zeus.ContentTypes;
using Zeus.Globalization;
using Zeus.Integrity;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Linq;
using Zeus.Linq;
using Zeus.Persistence;
using Zeus.Web;
using Zeus.Security;
using System.Security.Principal;
using Zeus.Web.Hosting;
using System.Threading;

namespace Zeus
{
    [RestrictParents(typeof(ContentItem))]
    [System.Serializable]
    public abstract class ContentItem : IUrlParserDependency, INode, IEditableObject
    {
        #region Private Fields

        private IList<AuthorizationRule> _authorizationRules;
        private IList<LanguageSetting> _languageSettings;
        private string _name;
        private DateTime? _expires;
        private IList<ContentItem> _children = new List<ContentItem>();
        private IList<ContentItem> _translations = new List<ContentItem>();
        private IDictionary<string, PropertyData> _details = new Dictionary<string, PropertyData>();
        private IDictionary<string, PropertyCollection> _detailCollections = new Dictionary<string, PropertyCollection>();
        private string _url;
        private bool _visible;

        [Copy]
        private IUrlParser _urlParser;

        #endregion

        #region Public Properties (persisted)

        /// <summary>Gets or sets item ID.</summary>
        public virtual int ID { get; set; }

        /// <summary>Gets or sets this item's parent. This can be null for root items and previous versions but should be another page in other situations.</summary>
        public virtual ContentItem Parent { get; set; }

        /// <summary>Gets or sets the item's title. This is used in edit mode and probably in a custom implementation.</summary>
        [Copy]
        public virtual string Title { get; set; }

        /// <summary>Gets or sets the item's name. This is used to compute the item's url and can be used to uniquely identify the item among other items on the same level.</summary>
        [Copy]
        public virtual string Name
        {
            get
            {
                return _name ?? (ID > 0 ? ID.ToString() : null);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    _name = null;
                else
                    _name = value;
                _url = null;
            }
        }

        /// <summary>Gets or sets when this item was initially created.</summary>
        [Copy]
        public virtual DateTime Created { get; set; }

        /// <summary>Gets or sets the date this item was updated.</summary>
        [Copy]
        public virtual DateTime Updated { get; set; }
        public virtual void ReorderAction() {  }

        /// <summary>Gets or sets the publish date of this item.</summary>
        public virtual DateTime? Published { get; set; }

        /// <summary>Gets or sets the expiration date of this item.</summary>
        public virtual DateTime? Expires
        {
            get { return _expires; }
            set { _expires = value != DateTime.MinValue ? value : null; }
        }

        /// <summary>Gets or sets the sort order of this item.</summary>
        [Copy]
        public virtual int SortOrder { get; set; }

        /// <summary>Gets or sets whether this item is visible. This is normally used to control it's visibility in the site map provider.</summary>
        [Copy]
        public virtual bool Visible
        {
            get
            {
                if (this.TranslationOf == null)
                    return _visible;
                else
                    return this.TranslationOf.Visible;
            }
            set
            {
                _visible = value;
            }
        }

        /// <summary>Gets or sets the published version of this item. If this value is not null then this item is a previous version of the item specified by VersionOf.</summary>
        public virtual ContentItem VersionOf { get; set; }

        /// <summary>
        /// Gets or sets the version number of this item. This starts at 1, and increases for later versions.
        /// </summary>
        [Copy]
        public virtual int Version { get; set; }

        /// <summary>Gets or sets the original language version of this item. If this value is not null then this item is a translated version of the item specified by TranslationOf.</summary>
        public virtual ContentItem TranslationOf { get; set; }

        /// <summary>
        /// Gets or sets the language code of this item.
        /// </summary>
        [Copy]
        public virtual string Language { get; set; }

        /// <summary>Gets or sets the name of the identity who saved this item.</summary>
        [Copy]
        public virtual string SavedBy { get; set; }

        /// <summary>Gets or sets the details collection. These are usually accessed using the e.g. item["Detailname"]. This is a place to store content data.</summary>
        public IDictionary<string, PropertyData> Details
        {
            get { return _details; }
            set { _details = value; }
        }

        /// <summary>Gets or sets the details collection collection. These are details grouped into a collection.</summary>
        public IDictionary<string, PropertyCollection> DetailCollections
        {
            get { return _detailCollections; }
            set { _detailCollections = value; }
        }

        /// <summary>Gets or sets all a collection of child items of this item ignoring permissions. If you want the children the current user has permission to use <see cref="GetChildren()"/> instead.</summary>
        public virtual IList<ContentItem> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        /// <summary>Gets or sets all a collection of child items of this item ignoring permissions. If you want the children the current user has permission to use <see cref="GetChildren()"/> instead.</summary>
        public virtual IList<ContentItem> Translations
        {
            get { return _translations; }
            set { _translations = value; }
        }

        public virtual int? OverrideCacheID { get; set; }
        public int CacheID { get { return OverrideCacheID.HasValue ? OverrideCacheID.Value : ID; } }

        #endregion

        #region Public Properties (generated)

        /// <summary>The default file extension for this content item, e.g. ".aspx".</summary>
        public virtual string Extension
        {
            get { return BaseLibrary.Web.Url.DefaultExtension; }
        }

        /// <summary>Gets whether this item is a page. This is used for site map purposes.</summary>
        public virtual bool IsPage
        {
            get { return true; }
        }

        /// <summary>Needs to be overridden and set to true for the code needed to match a Custom Url to kick in</summary>
        public virtual bool HasCustomUrl
        {
            get { return false; }
        }

        /// <summary>Needs to be overridden and set to true for the code needed to match a Custom Url to kick in</summary>
        public virtual bool CheckItselfForCaching
        {
            get { return true; }
        }

        /// <summary>If set to false it will stop the update going up the tree - useful when you don't want caches kicked for simple updates on large sites - cache dependencies are still easy enough to set</summary>
        public virtual bool PropogateUpdate
        {
            get { return true; }
        }

        /// <summary>Only to be used on Parents with children set to be invisible in tree</summary>
        public virtual bool IgnoreOrderOnSave
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the public url to this item. This is computed by walking the 
        /// parent path and prepending their names to the url.
        /// </summary>
        public virtual string Url
        {
            get
            {
                if (_url == null)
                {
                    if (_urlParser != null)
                        _url = GetUrl(LanguageSelector.Fallback(ContentLanguage.PreferredCulture.Name, false));
                    else
                        _url = FindPath(PathData.DefaultAction).RewrittenUrl;
                }
                return _url;
            }
        }

        /// <summary>
        /// Allows the default language code to be overridden for the purposes of generating a URL.
        /// This is used by NameEditorAttribute to get the URL for a parent item using the new child's
        /// language.
        /// </summary>
        /// <param name="languageCode"></param>
        /// <returns></returns>
        public virtual string GetUrl(string languageCode)
        {
            if (_urlParser != null)
                return _urlParser.BuildUrl(this, languageCode);
            return FindPath(PathData.DefaultAction).RewrittenUrl;
        }

        public virtual string GetUrl(ILanguageSelector languageSelector)
        {
            LanguageSelectorContext args = new LanguageSelectorContext(this);
            languageSelector.LoadLanguage(args);

            if (_urlParser != null)
                return _urlParser.BuildUrl(this, args.SelectedLanguage);
            return FindPath(PathData.DefaultAction).RewrittenUrl;
        }

        public string HierarchicalTitle
        {
            get
            {
                string result = this.Title;
                if (Parent != null)
                    result = Parent.HierarchicalTitle + " - " + result;
                return result;
            }
        }

        /// <summary>Gets the icon of this item. This can be used to distinguish item types in edit mode.</summary>
        public virtual string IconUrl
        {
            get { return Utility.GetCooliteIconUrl(Icon); }
        }

        protected virtual Icon Icon
        {
            get { return (IsPage) ? Icon.Page : Icon.PageWhite; }
        }

        /// <summary>The logical path to the node from the root node.</summary>
        public string Path
        {
            get
            {
                string path = "/";
                ContentItem startingParent = (TranslationOf != null) ? TranslationOf.Parent : Parent;
                if (startingParent != null)
                    path += Name;
                for (ContentItem item = startingParent; item != null && item.Parent != null; item = item.Parent)
                    path = "/" + item.Name + path;
                return path;
            }
        }

        #endregion

        /// <summary>Gets an array of roles allowed to read this item. Null or empty list is interpreted as this item has no access restrictions (anyone may read).</summary>
        public virtual IList<AuthorizationRule> AuthorizationRules
        {
            get
            {
                if (_authorizationRules == null)
                    _authorizationRules = new List<AuthorizationRule>();
                return _authorizationRules;
            }
            set { _authorizationRules = value; }
        }

        /// <summary>Gets an array of language settings for this item. Null or empty list is interpreted as this item inheriting its settings from its parent.</summary>
        public virtual IList<LanguageSetting> LanguageSettings
        {
            get
            {
                if (_languageSettings == null)
                    _languageSettings = new List<LanguageSetting>();
                return _languageSettings;
            }
            set { _languageSettings = value; }
        }

        #region this[]

        /// <summary>Gets or sets the detail or property with the supplied name. If a property with the supplied name exists this is always returned in favour of any detail that might have the same name.</summary>
        /// <param name="detailName">The name of the propery or detail.</param>
        /// <returns>The value of the property or detail. If now property exists null is returned.</returns>
        public virtual object this[string detailName]
        {
            get
            {
                if (detailName == null)
                    throw new ArgumentNullException("detailName");

                switch (detailName)
                {
                    case "ID":
                        return ID;
                    case "Title":
                        return Title;
                    case "Name":
                        return Name;
                    case "Url":
                        return Url;
                    default:
                        return Utility.Evaluate(this, detailName)
                            ?? GetDetail(detailName)
                            ?? GetDetailCollection(detailName, false);
                }
            }
            set
            {
                if (string.IsNullOrEmpty(detailName))
                    throw new ArgumentNullException("detailName", "Parameter 'detailName' cannot be null or empty.");

                PropertyInfo info = GetType().GetProperty(detailName);
                if (info != null && info.CanWrite)
                {
                    if (value != null && info.PropertyType != value.GetType())
                        value = Utility.Convert(value, info.PropertyType);
                    info.SetValue(this, value, null);
                }
                else if (value is PropertyCollection)
                {
                    DetailCollections[detailName] = (PropertyCollection)value;
                    //throw new ZeusException("Cannot set a detail collection this way, add it to the DetailCollections collection instead.");
                }
                else
                {
                    SetDetail(detailName, value);
                }
            }
        }

        #endregion

        protected ContentItem()
        {
            Created = DateTime.Now;
            Updated = DateTime.Now;
            Published = DateTime.Now;
            Visible = true;
            Version = 1;
        }

        #region GetDetail & SetDetail<T> Methods

        /// <summary>Gets a detail from the details bag.</summary>
        /// <param name="detailName">The name of the value to get.</param>
        /// <returns>The value stored in the details bag or null if no item was found.</returns>
        public virtual object GetDetail(string detailName)
        {
            return Details.ContainsKey(detailName)
                ? Details[detailName].Value
                : null;
        }

        /// <summary>Gets a detail from the details bag.</summary>
        /// <param name="detailName">The name of the value to get.</param>
        /// <param name="defaultValue">The default value to return when no detail is found.</param>
        /// <returns>The value stored in the details bag or null if no item was found.</returns>
        public virtual T GetDetail<T>(string detailName, T defaultValue)
        {
            IDictionary<string, PropertyData> details = GetCurrentOrMasterLanguageDetails(detailName);

            //try inserted to stop "illegal access" error
            bool? bContains;
            bContains = tryAndWaitIfNecessary(details, detailName);

            //if failed try again
            if (bContains == null)
            {
                bContains = tryAndWaitIfNecessary(details, detailName);
                //if still failed, then another second has already passed so try a final time with no catch
                if (bContains == null)
                    bContains = details.ContainsKey(detailName);
            }

            if (bContains.Value)
                return Utility.Convert<T>(details[detailName].Value);
            return defaultValue;
        }

        private bool? tryAndWaitIfNecessary(IDictionary<string, PropertyData> details, string detailName)
        {
            try
            {
                return details.ContainsKey(detailName);
            }
            catch (System.Exception ex)
            {
                if (ex.Message.ToLower().IndexOf("illegal access to loading collection") > -1)
                {
                    //wait for a second and try again
                    Thread.Sleep(1000);
                }
                else if (ex.Message.ToLower().IndexOf("could not initialize a collection batch") > -1)
                {
                    //wait for a second and try again
                    Thread.Sleep(1000);
                }
                else if (ex.Message.ToLower().IndexOf("was deadlocked on lock resources with another process and has been chosen as the deadlock victim") > -1)
                {
                    //wait for a second and try again
                    Thread.Sleep(1000);
                }
                else
                {
                    throw (ex);
                }
                return null;
            }
        }

        private IDictionary<string, PropertyData> GetCurrentOrMasterLanguageDetails(string detailName)
        {
            // Look up content property matching this name.
            IContentProperty property = Context.ContentTypes.GetContentType(GetType()).GetProperty(detailName);
            if (property == null || property.Shared)
            {
                ContentItem currentItem = VersionOf ?? this;
                if (currentItem.TranslationOf != null)
                    return currentItem.TranslationOf.Details;
            }
            return Details;
        }

        public virtual void SetDetail(string detailName, object value)
        {
            // TODO: Throw exception if this is a shared property and this is not the master language version.

            if (string.IsNullOrEmpty(detailName))
                throw new ArgumentNullException("detailName");

            PropertyData detail = Details.ContainsKey(detailName) ? Details[detailName] : null;

            if (detail != null && value != null && value.GetType().IsAssignableFrom(detail.ValueType))
            {
                // update an existing detail
                detail.Value = value;
            }
            else
            {
                if (detail != null)
                    // delete detail or remove detail of wrong type
                    Details.Remove(detailName);
                if (value != null)
                {
                    // add new detail
                    PropertyData propertyData = Context.ContentTypes.GetContentType(GetType()).GetProperty(detailName, value).CreatePropertyData(this, value);
                    Details.Add(detailName, propertyData);
                }
            }
        }

        /// <summary>Set a value into the <see cref="Details"/> bag. If a value with the same name already exists it is overwritten. If the value equals the default value it will be removed from the details bag.</summary>
        /// <param name="detailName">The name of the item to set.</param>
        /// <param name="value">The value to set. If this parameter is null or equal to defaultValue the detail is removed.</param>
        /// <param name="defaultValue">The default value. If the value is equal to this value the detail will be removed.</param>
        protected virtual void SetDetail<T>(string detailName, T value, T defaultValue)
        {
            if (value == null || !value.Equals(defaultValue))
                SetDetail(detailName, value);
            else if (Details.ContainsKey(detailName))
                Details.Remove(detailName);
        }

        /// <summary>Set a value into the <see cref="Details"/> bag. If a value with the same name already exists it is overwritten.</summary>
        /// <param name="detailName">The name of the item to set.</param>
        /// <param name="value">The value to set. If this parameter is null the detail is removed.</param>
        protected virtual void SetDetail<T>(string detailName, T value)
        {
            SetDetail(detailName, (object)value);
        }

        #endregion

        #region GetDetailCollection

        /// <summary>Gets a named detail collection.</summary>
        /// <param name="collectionName">The name of the detail collection to get.</param>
        /// <param name="createWhenEmpty">Wether a new collection should be created if none exists. Setting this to false means null will be returned if no collection exists.</param>
        /// <returns>A new or existing detail collection or null if the createWhenEmpty parameter is false and no collection with the given name exists..</returns>
        public virtual PropertyCollection GetDetailCollection(string collectionName, bool createWhenEmpty)
        {
            if (DetailCollections.ContainsKey(collectionName))
                return DetailCollections[collectionName];
            else if (createWhenEmpty)
            {
                PropertyCollection collection = new PropertyCollection(this, collectionName);
                DetailCollections.Add(collectionName, collection);
                return collection;
            }
            else
                return null;
        }

        #endregion

        #region Methods

        private const int SORT_ORDER_THRESHOLD = 9999;

        /// <summary>Adds an item to the children of this item updating it's parent refernce.</summary>
        /// <param name="newParent">The new parent of the item. If this parameter is null the item is detached from the hierarchical structure.</param>
        public virtual void AddTo(ContentItem newParent)
        {
            if (Parent != null && Parent != newParent && Parent.Children.Contains(this))
                Parent.Children.Remove(this);

            Parent = newParent;

            //see if we care about ordering...
            if (newParent != null && !newParent.Children.Contains(this))
            {
                IList<ContentItem> siblings = newParent.Children;
                if (siblings.Count > 0 && !newParent.IgnoreOrderOnSave)
                {
                    int lastOrder = siblings[siblings.Count - 1].SortOrder;

                    for (int i = siblings.Count - 2; i >= 0; i--)
                    {
                        if (siblings[i].SortOrder < lastOrder - SORT_ORDER_THRESHOLD)
                        {
                            siblings.Insert(i + 1, this);
                            return;
                        }
                        lastOrder = siblings[i].SortOrder;
                    }

                    if (lastOrder > SORT_ORDER_THRESHOLD)
                    {
                        siblings.Insert(0, this);
                        return;
                    }
                }
                
                siblings.Add(this);
            }
        }

        public virtual ContentItem Clone(bool includeChildren)
        {
            return Clone(includeChildren, false);
        }

        /// <summary>Creats a copy of this item including details, authorization rules, and language settings, while resetting ID.</summary>
        /// <param name="includeChildren">Specifies whether this item's child items also should be cloned.</param>
        /// <param name="includeTranslations"></param>
        /// <returns>The cloned item with or without cloned child items.</returns>
        public virtual ContentItem Clone(bool includeChildren, bool includeTranslations)
        {
            ContentItem cloned = (ContentItem)MemberwiseClone();
            cloned.ID = 0;
            cloned._url = null;

            CloneDetails(cloned);
            CloneChildren(includeChildren, cloned);
            CloneTranslations(includeTranslations, cloned);
            CloneAuthorizationRules(cloned);
            CloneLanguageSettings(cloned);

            return cloned;
        }

        #region Clone Helper Methods

        private void CloneAuthorizationRules(ContentItem cloned)
        {
            if (AuthorizationRules != null)
            {
                cloned.AuthorizationRules = new List<AuthorizationRule>();
                foreach (AuthorizationRule rule in AuthorizationRules)
                {
                    AuthorizationRule clonedRule = rule.Clone();
                    clonedRule.EnclosingItem = cloned;
                    cloned.AuthorizationRules.Add(clonedRule);
                }
            }
        }

        private void CloneLanguageSettings(ContentItem cloned)
        {
            if (LanguageSettings != null)
            {
                cloned.LanguageSettings = new List<LanguageSetting>();
                foreach (LanguageSetting languageSetting in LanguageSettings)
                {
                    LanguageSetting clonedLanguageSetting = languageSetting.Clone();
                    clonedLanguageSetting.EnclosingItem = cloned;
                    cloned.LanguageSettings.Add(clonedLanguageSetting);
                }
            }
        }

        private void CloneChildren(bool includeChildren, ContentItem cloned)
        {
            cloned.Children = new List<ContentItem>();
            if (includeChildren)
                foreach (ContentItem child in Children)
                {
                    ContentItem clonedChild = child.Clone(true);
                    clonedChild.AddTo(cloned);
                }
        }

        private void CloneTranslations(bool includeTranslations, ContentItem cloned)
        {
            cloned.Translations = new List<ContentItem>();
            if (includeTranslations)
                foreach (ContentItem translation in Translations)
                {
                    ContentItem clonedTranslation = translation.Clone(true);
                    clonedTranslation.AddTo(cloned);
                }
        }

        private void CloneDetails(ContentItem cloned)
        {
            cloned.Details = new Dictionary<string, PropertyData>();
            foreach (var detail in Details.Values)
                cloned[detail.Name] = detail.Value;

            cloned.DetailCollections = new Dictionary<string, PropertyCollection>();
            foreach (PropertyCollection collection in DetailCollections.Values)
            {
                PropertyCollection clonedCollection = collection.Clone();
                clonedCollection.EnclosingItem = cloned;
                cloned.DetailCollections[collection.Name] = clonedCollection;
            }
        }

        #endregion

        public TAncestor FindFirstAncestor<TAncestor>()
            where TAncestor : ContentItem
        {
            return FindFirstAncestorRecursive<TAncestor>(this);
        }

        private static TAncestor FindFirstAncestorRecursive<TAncestor>(ContentItem contentItem)
            where TAncestor : ContentItem
        {
            if (contentItem == null)
                return null;

            if (contentItem is TAncestor)
                return (TAncestor)contentItem;

            return FindFirstAncestorRecursive<TAncestor>(contentItem.Parent);
        }

        /// <summary>
        /// Gets child items that the user is allowed to access.
        /// It doesn't have to return the same collection as
        /// the Children property.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<ContentItem> GetChildren()
        {
            return GetChildrenInternal().Authorized(HttpContext.Current.User, Context.SecurityManager, Operations.Read);
        }

        /// <summary>
        /// Gets child items that the user is allowed to access.
        /// It doesn't have to return the same collection as
        /// the Children property.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<ContentItem> GetGlobalizedChildren()
        {
            return GetChildrenInternalWithLanguageSelection().Authorized(HttpContext.Current.User, Context.SecurityManager, Operations.Read);
        }

        public virtual IEnumerable<T> GetChildren<T>()
        {
            return GetChildrenInternal().OfType<T>();
        }

        public virtual IEnumerable<T> GetGlobalizedChildren<T>()
        {
            return GetChildrenInternalWithLanguageSelection().OfType<T>();
        }

        private IEnumerable<ContentItem> GetChildrenInternalWithLanguageSelection()
        {
            return GetChildrenInternalWithLanguageSelection(LanguageSelector.AutoDetect());
        }

        private IEnumerable<ContentItem> GetChildrenInternalWithLanguageSelection(ILanguageSelector languageSelector)
        {
            IEnumerable<ContentItem> children = GetChildrenInternal();

            LanguageSelectorContext args = new LanguageSelectorContext(this);
            languageSelector.LoadLanguage(args);
            return FilterLanguage(children, languageSelector);
        }

        private IEnumerable<ContentItem> GetChildrenInternal()
        {
            // Get the actual item this item represents.
            ContentItem realItem = this;
            if (VersionOf != null)
                realItem = realItem.VersionOf;
            if (TranslationOf != null)
                realItem = realItem.TranslationOf;

            return realItem.Children;
        }

        private static IEnumerable<ContentItem> FilterLanguage(IEnumerable<ContentItem> pages, ILanguageSelector langSelector)
        {
            List<ContentItem> datas = new List<ContentItem>();
            foreach (ContentItem page in pages)
            {
                ContentItem translation = SelectLanguageBranch(page, langSelector);
                if (translation != null)
                    datas.Add(translation);
            }
            return datas;
        }

        private static ContentItem SelectLanguageBranch(ContentItem page, ILanguageSelector selector)
        {
            LanguageSelectorContext args = new LanguageSelectorContext(page);
            selector.SelectPageLanguage(args);
            if (string.IsNullOrEmpty(args.SelectedLanguage))
                return null;
            return Context.Current.LanguageManager.GetTranslationDirect(page, args.SelectedLanguage);
        }

        /// <summary>Finds children based on the given url segments. The method supports convering the last segments into action and parameter.</summary>
        /// <param name="remainingUrl">The remaining url segments.</param>
        /// <returns>A path data object which can be empty (check using data.IsEmpty()).</returns>
        public virtual PathData FindPath(string remainingUrl)
        {
            return FindPath(remainingUrl, Context.Current.LanguageManager.GetDefaultLanguage());
        }

        public virtual PathData FindPath(string remainingUrl, string languageCode)
        {
            // Get correct translation.
            ContentItem translation = Context.Current.LanguageManager.GetTranslation(this, languageCode) ?? this;

            if (remainingUrl == null)
                return translation.GetTemplate(string.Empty);

            remainingUrl = remainingUrl.TrimStart('/');

            if (remainingUrl.Length == 0)
                return translation.GetTemplate(string.Empty);

            int slashIndex = remainingUrl.IndexOf('/');
            string nameSegment = slashIndex < 0 ? remainingUrl : remainingUrl.Substring(0, slashIndex);
            foreach (ContentItem child in translation.GetChildrenInternal())
            {
                // Get correct translation.
                ContentItem childTranslation = Context.Current.LanguageManager.GetTranslation(child, languageCode);
                if (childTranslation != null && childTranslation.Equals(nameSegment))
                {
                    remainingUrl = slashIndex < 0 ? null : remainingUrl.Substring(slashIndex + 1);
                    return childTranslation.FindPath(remainingUrl, languageCode);
                }
            }

            return GetTemplate(remainingUrl);
        }

        private PathData GetTemplate(string remainingUrl)
        {
            IPathFinder[] finders = PathDictionary.GetFinders(GetType());

            foreach (IPathFinder finder in finders)
            {
                PathData data = finder.GetPath(this, remainingUrl);
                if (data != null)
                    return data;
            }

            return PathData.Empty;
        }

        /// <summary>
        /// Checks whether this content item contains any properties or property collections.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsEmpty()
        {
            return string.IsNullOrEmpty(Title) && !Details.Any() && !DetailCollections.Any();
        }

        /// <summary>
        /// Translations don't have their Parent object set, so this is an abstraction to allow
        /// translations to act as normal content items.
        /// </summary>
        /// <returns></returns>
        public virtual ContentItem GetParent()
        {
            return GetParent(ContentLanguage.PreferredCulture.Name);
        }

        /// <summary>
        /// Translations don't have their Parent object set, so this is an abstraction to allow
        /// translations to act as normal content items.
        /// </summary>
        /// <returns></returns>
        public virtual ContentItem GetParent(string languageName)
        {
            ContentItem realItem = TranslationOf ?? this;
            ContentItem parent = realItem.Parent;

            if (parent == null)
                return null;

            return (Context.Current.LanguageManager.Enabled)
                ? (Context.Current.LanguageManager.GetTranslation(parent, languageName) ?? parent)
                : parent;
        }

        /// <summary>
        /// Tries to get a child item with a given name. This method ignores
        /// user permissions and any trailing '.aspx' that might be part of
        /// the name.
        /// </summary>
        /// <param name="childName">The name of the child item to get.</param>
        /// <returns>The child item if it is found otherwise null.</returns>
        /// <remarks>If the method is passed an empty or null string it will return itself.</remarks>
        public virtual ContentItem GetChild(string childName)
        {
            if (string.IsNullOrEmpty(childName))
                return null;

            int slashIndex = childName.IndexOf('/');
            if (slashIndex == 0) // starts with slash
            {
                if (childName.Length == 1)
                    return this;
                return GetChild(childName.Substring(1));
            }

            if (slashIndex > 0) // contains a slash further down
            {
                string nameSegment = childName.Substring(0, slashIndex);
                foreach (ContentItem child in GetChildrenInternal())
                    foreach (ContentItem translation in Context.Current.LanguageManager.GetTranslationsOf(child, true))
                        if (translation.Equals(nameSegment))
                            return translation.GetChild(childName.Substring(slashIndex));
                return null;
            }

            // no slash, only a name
            foreach (ContentItem child in GetChildrenInternal())
                foreach (ContentItem translation in Context.Current.LanguageManager.GetTranslationsOf(child, true))
                    if (translation.Equals(childName))
                        return translation;
            return null;
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if ((obj == null) || (obj.GetType() != GetType())) return false;
            ContentItem item = obj as ContentItem;
            if (ID != 0 && item.ID != 0)
                return ID == item.ID;
            else
                return ReferenceEquals(this, item);
        }

        /// <summary>Gets a hash code based on the item's id.</summary>
        /// <returns>A hash code.</returns>
        public override int GetHashCode()
        {
            return this.ID.GetHashCode();
        }

        protected virtual bool Equals(string name)
        {
            if (Name == null)
                return false;
            return Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
                || HttpUtility.UrlDecode(Name).Equals(name, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>Gets wether a certain user is authorized to view this item.</summary>
        /// <param name="user">The user to check.</param>
        /// <param name="operation"></param>
        /// <returns>True if the item is open for all or the user has the required permissions.</returns>
        public virtual bool IsAuthorized(IPrincipal user, string operation)
        {
            if (AuthorizationRules == null || AuthorizationRules.Count == 0)
                return true;

            // Iterate rules to find a rule that matches
            foreach (AuthorizationRule auth in AuthorizationRules)
                if (auth.IsAuthorized(user, operation))
                    return true;

            return false;
        }

        public virtual bool IsPublished()
        {
            return (Published != null && Published.Value <= DateTime.Now)
                && !(Expires != null && Expires.Value < DateTime.Now);
        }

        public virtual bool HasMinRequirementsForSaving()
        {
            return true;
        }

        /// <summary>
        /// Return something other than null to group items in the admin site tree.
        /// </summary>
        /// <returns></returns>
        public virtual string FolderPlacementGroup
        {
            get { return null; }
        }

        #endregion

        void IUrlParserDependency.SetUrlParser(IUrlParser parser)
        {
            _urlParser = parser;
        }

        #region INode Members

        string INode.PreviewUrl
        {
            get
            {
                if (IsPage)
                    return Url;
                return Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(Context.Current.Resolve<IAdminAssemblyManager>().Assembly, "Zeus.Admin.View.aspx") + "?selected=" + Path;
            }
        }

        string INode.ClassNames
        {
            get
            {
                StringBuilder className = new StringBuilder();

                if (!Published.HasValue || Published > DateTime.Now)
                    className.Append("unpublished ");
                else if (Published > DateTime.Now.AddDays(-1))
                    className.Append("day ");
                else if (Published > DateTime.Now.AddDays(-7))
                    className.Append("week ");
                else if (Published > DateTime.Now.AddMonths(-1))
                    className.Append("month ");

                if (Expires.HasValue && Expires <= DateTime.Now)
                    className.Append("expired ");

                if (!Visible)
                    className.Append("invisible ");

                if (AuthorizationRules != null && AuthorizationRules.Count > 0)
                    className.Append("locked ");

                return className.ToString();
            }
        }

        #endregion

        #region ILink Members

        string ILink.Contents
        {
            get { return Title; }
        }

        string ILink.ToolTip
        {
            get { return string.Empty; }
        }

        string ILink.Target
        {
            get { return string.Empty; }
        }

        #endregion
    }
}
