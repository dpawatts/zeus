using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Zeus.Configuration;

namespace Zeus.DynamicContent
{
	public class DynamicContentManager : IDynamicContentManager
	{
		private readonly DynamicContentSection _configSection;

		public DynamicContentManager(DynamicContentSection configSection)
			: this()
		{
			_configSection = configSection;

			foreach (DynamicContentControl control in configSection.Controls)
				NameToTypeMap.Add(control.Name, Type.GetType(control.Type));

			foreach (DynamicContentControl control in configSection.Controls)
				TypeToNameMap.Add(Type.GetType(control.Type), control.Name);
		}

		public DynamicContentManager()
		{
			NameToTypeMap = new Dictionary<string, Type>();
			TypeToNameMap = new Dictionary<Type, string>();			
		}

		public IDictionary<string, Type> NameToTypeMap { get; private set; }
		public IDictionary<Type, string> TypeToNameMap { get; private set; }

		public IDynamicContent CreateDynamicContent(string name, string state)
		{
			IDynamicContent dynamicContent = InstantiateDynamicContent(name);
			dynamicContent.State = state;
			return dynamicContent;
		}

		public string GetMarkup(IDynamicContent dynamicContent)
		{
			return string.Format(@"<span class=""nonEditable""><dynamiccontent state=""{0}"">{{DynamicContent:{1}}}</dynamiccontent></span>",
				dynamicContent.State, TypeToNameMap[dynamicContent.GetType()]);
		}

		public string RenderDynamicContent(string value)
		{
			/*
			 * Looks for the following HTML in the passed-in string, and replaces
			 * it with the output of the relevant dynamic content control.
			 * 
			 * <span class="nonEditable" state="3,MyPropName">{DynamicContent:DynamicPageProperty}</span>
			 */

			const string pattern = @"<span class=""nonEditable""><dynamiccontent state=""([\s\S]+?)"">{DynamicContent:([a-z ]+?)}</dynamiccontent></span>";
			Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
			return regex.Replace(value, OnPatternMatched);
		}

		private string OnPatternMatched(Match match)
		{
			string dynamicContentControlName = match.Groups[2].Value;
			string state = match.Groups[1].Value;

			// Instantiate dynamic content control for this match.
			IDynamicContent dynamicContent = CreateDynamicContent(dynamicContentControlName, state);
			return dynamicContent.Render();
		}

		public IEnumerable<DynamicContentControl> GetAvailableControls()
		{
			return _configSection.Controls.Cast<DynamicContentControl>();
		}

		public IDynamicContent InstantiateDynamicContent(string name)
		{
			Type type = NameToTypeMap[name];
			return (IDynamicContent) Activator.CreateInstance(type);
		}
	}
}