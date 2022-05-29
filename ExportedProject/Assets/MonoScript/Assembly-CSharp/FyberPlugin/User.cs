using FyberPlugin.LitJson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace FyberPlugin
{
	public class User
	{
		protected const string AGE = "age";

		protected const string BIRTHDATE = "birthdate";

		protected const string GENDER = "gender";

		protected const string SEXUAL_ORIENTATION = "sexual_orientation";

		protected const string ETHNICITY = "ethnicity";

		protected const string MARITAL_STATUS = "marital_status";

		protected const string NUMBER_OF_CHILDRENS = "children";

		protected const string ANNUAL_HOUSEHOLD_INCOME = "annual_household_income";

		protected const string EDUCATION = "education";

		protected const string ZIPCODE = "zipcode";

		protected const string INTERESTS = "interests";

		protected const string IAP = "iap";

		protected const string IAP_AMOUNT = "iap_amount";

		protected const string NUMBER_OF_SESSIONS = "number_of_sessions";

		protected const string PS_TIME = "ps_time";

		protected const string LAST_SESSION = "last_session";

		protected const string CONNECTION = "connection";

		protected const string DEVICE = "device";

		protected const string APP_VERSION = "app_version";

		protected const string LOCATION = "fyberlocation";

		static User()
		{
		}

		public User()
		{
		}

		protected static string GenerateGetJsonString(string key)
		{
			Dictionary<string, string> strs = new Dictionary<string, string>()
			{
				{ "action", "get" },
				{ "key", key }
			};
			return JsonMapper.ToJson(strs);
		}

		private static string GeneratePutJsonString(string key, object value)
		{
			Dictionary<string, object> strs = new Dictionary<string, object>()
			{
				{ "action", "put" },
				{ "key", key },
				{ "type", value.GetType().ToString() }
			};
			if (!(value is DateTime))
			{
				strs.Add("value", value);
			}
			else
			{
				strs.Add("value", ((DateTime)value).ToString("yyyy/MM/dd"));
			}
			return JsonMapper.ToJson(strs);
		}

		protected static T Get<T>(string key)
		{
			User.JsonResponse<T> obj = JsonMapper.ToObject<User.JsonResponse<T>>(User.GetJsonMessage(key));
			if (obj.success)
			{
				return obj.@value;
			}
			Debug.Log(obj.error);
			return default(T);
		}

		public static int? GetAge()
		{
			return User.Get<int?>("age");
		}

		public static int? GetAnnualHouseholdIncome()
		{
			return User.Get<int?>("annual_household_income");
		}

		public static string GetAppVersion()
		{
			return User.Get<string>("app_version");
		}

		public static DateTime? GetBirthdate()
		{
			DateTime dateTime;
			if (DateTime.TryParseExact(User.Get<string>("birthdate"), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
			{
				return new DateTime?(dateTime);
			}
			return null;
		}

		public static UserConnection? GetConnection()
		{
			return User.Get<UserConnection?>("connection");
		}

		public static string GetCustomValue(string key)
		{
			return User.Get<string>(key);
		}

		public static string GetDevice()
		{
			return User.Get<string>("device");
		}

		public static UserEducation? GetEducation()
		{
			return User.Get<UserEducation?>("education");
		}

		public static UserEthnicity? GetEthnicity()
		{
			return User.Get<UserEthnicity?>("ethnicity");
		}

		public static UserGender? GetGender()
		{
			return User.Get<UserGender?>("gender");
		}

		public static bool? GetIap()
		{
			return User.Get<bool?>("iap");
		}

		public static float? GetIapAmount()
		{
			float? nullable;
			double? nullable1 = User.Get<double?>("iap_amount");
			if (!nullable1.HasValue)
			{
				nullable = null;
			}
			else
			{
				nullable = new float?((float)nullable1.Value);
			}
			return nullable;
		}

		public static string[] GetInterests()
		{
			return User.Get<string[]>("interests");
		}

		protected static string GetJsonMessage(string key)
		{
			string str;
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.user.UserWrapper", new object[0]))
			{
				str = androidJavaObject.CallStatic<string>("get", new object[] { key });
			}
			return str;
		}

		public static long? GetLastSession()
		{
			return User.Get<long?>("last_session");
		}

		public static Location GetLocation()
		{
			return User.Get<Location>("fyberlocation");
		}

		public static UserMaritalStatus? GetMaritalStatus()
		{
			return User.Get<UserMaritalStatus?>("marital_status");
		}

		public static int? GetNumberOfChildrens()
		{
			return User.Get<int?>("children");
		}

		public static int? GetNumberOfSessions()
		{
			return User.Get<int?>("number_of_sessions");
		}

		public static long? GetPsTime()
		{
			return User.Get<long?>("ps_time");
		}

		public static UserSexualOrientation? GetSexualOrientation()
		{
			return User.Get<UserSexualOrientation?>("sexual_orientation");
		}

		public static string GetZipcode()
		{
			return User.Get<string>("zipcode");
		}

		protected static void NativePut(string json)
		{
			using (AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.fyber.unity.user.UserWrapper", new object[0]))
			{
				androidJavaObject.CallStatic("put", new object[] { json });
			}
		}

		private static void Put(string key, object value)
		{
			User.NativePut(User.GeneratePutJsonString(key, value));
		}

		public static void PutCustomValue(string key, string value)
		{
			User.Put(key, value);
		}

		public static void SetAge(int age)
		{
			User.Put("age", age);
		}

		public static void SetAnnualHouseholdIncome(int annualHouseholdIncome)
		{
			User.Put("annual_household_income", annualHouseholdIncome);
		}

		public static void SetAppVersion(string appVersion)
		{
			User.Put("app_version", appVersion);
		}

		public static void SetBirthdate(DateTime birthdate)
		{
			User.Put("birthdate", birthdate);
		}

		public static void SetConnection(UserConnection connection)
		{
			User.Put("connection", connection);
		}

		public static void SetDevice(string device)
		{
			User.Put("device", device);
		}

		public static void SetEducation(UserEducation education)
		{
			User.Put("education", education);
		}

		public static void SetEthnicity(UserEthnicity ethnicity)
		{
			User.Put("ethnicity", ethnicity);
		}

		public static void SetGender(UserGender gender)
		{
			User.Put("gender", gender);
		}

		public static void SetIap(bool iap)
		{
			User.Put("iap", iap);
		}

		public static void SetIapAmount(float iap_amount)
		{
			User.Put("iap_amount", (double)iap_amount);
		}

		public static void SetInterests(string[] interests)
		{
			User.Put("interests", interests);
		}

		public static void SetLastSession(long lastSession)
		{
			User.Put("last_session", lastSession);
		}

		public static void SetLocation(Location location)
		{
			User.Put("fyberlocation", location);
		}

		public static void SetMaritalStatus(UserMaritalStatus maritalStatus)
		{
			User.Put("marital_status", maritalStatus);
		}

		public static void SetNumberOfChildrens(int numberOfChildrens)
		{
			User.Put("children", numberOfChildrens);
		}

		public static void SetNumberOfSessions(int numberOfSessions)
		{
			User.Put("number_of_sessions", numberOfSessions);
		}

		public static void SetPsTime(long ps_time)
		{
			User.Put("ps_time", ps_time);
		}

		public static void SetSexualOrientation(UserSexualOrientation sexualOrientation)
		{
			User.Put("sexual_orientation", sexualOrientation);
		}

		public static void SetZipcode(string zipcode)
		{
			User.Put("zipcode", zipcode);
		}

		[Obfuscation(Exclude=true)]
		private class JsonResponse<T>
		{
			public string error
			{
				get;
				set;
			}

			public string key
			{
				get;
				set;
			}

			public bool success
			{
				get;
				set;
			}

			public T @value
			{
				get;
				set;
			}

			public JsonResponse()
			{
			}
		}
	}
}