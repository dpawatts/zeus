using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Zeus.BaseLibrary.ComponentModel.DataAnnotations
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
	public class CompareAttribute : ValidationAttribute
	{
		public Type CompareType { get; private set; }
		public string SourcePropertyName { get; private set; }
		public string TargetPropertyName { get; private set; }

		/// <summary>
		/// Gets a unique identifier for this attribute.
		/// </summary>
		public override object TypeId
		{
			get { return this; }
		}

		public CompareAttribute(Type compareType, string sourcePropertyName, string targetPropertyName)
		{
			CompareType = compareType;
			SourcePropertyName = sourcePropertyName;
			TargetPropertyName = targetPropertyName;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			PropertyInfo sourcePropertyInfo = validationContext.ObjectType.GetProperty(SourcePropertyName, BindingFlags.Public | BindingFlags.Instance);
			PropertyInfo targetPropertyInfo = validationContext.ObjectType.GetProperty(TargetPropertyName, BindingFlags.Public | BindingFlags.Instance);

			if (CompareType == typeof(string))
			{
				if (Convert.ToString(sourcePropertyInfo.GetValue(validationContext.ObjectInstance, null)) != Convert.ToString(targetPropertyInfo.GetValue(validationContext.ObjectInstance, null)))
					return new ValidationResult(ErrorMessage);
				return null;
			}
			
			if (CompareType == typeof(int))
				throw new NotImplementedException();

			if (CompareType == typeof(double))
				throw new NotImplementedException();

			if (CompareType == typeof(DateTime))
				throw new NotImplementedException();

			throw new NotImplementedException();
		}
	}
}