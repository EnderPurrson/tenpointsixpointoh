using System;
using System.Collections.Generic;

namespace FyberPlugin.LitJson
{
	internal struct ObjectMetadata
	{
		private global::System.Type element_type;

		private bool is_dictionary;

		private IDictionary<string, PropertyMetadata> properties;

		public global::System.Type ElementType
		{
			get
			{
				if (element_type == null)
				{
					return typeof(JsonData);
				}
				return element_type;
			}
			set
			{
				element_type = value;
			}
		}

		public bool IsDictionary
		{
			get
			{
				return is_dictionary;
			}
			set
			{
				is_dictionary = value;
			}
		}

		public IDictionary<string, PropertyMetadata> Properties
		{
			get
			{
				return properties;
			}
			set
			{
				properties = value;
			}
		}
	}
}
