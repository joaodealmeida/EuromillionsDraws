using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace Euromillions
{
	public class APIResponse
	{
		string _result;
		string _key;
		string _error;
		string _salt;
		string _request;
		string _id;
		string _data;

		public string result
		{
			get { return _result; }
			set { _result = value; }
		}
		public string error
		{
			get { return _error; }
			set { _error = value; }
		}
		public string key
		{
			get { return _key; }
			set { _key = value; }
		}

		public string salt
		{
			get { return _salt; }
			set { _salt = value; }
		}

		public string request
		{
			get { return _request; }
			set { _request = value; }
		}

		public string id
		{
			get { return _id; }
			set { _id = value; }
		}

		public string data
		{
			get { return _data; }
			set { _data = value; }
		}

	}

	public class HelperAPI
	{
		string[] _numbers;
		string[] _stars;
		string _date;

		public string[] numbers
		{
			get { return _numbers; }
			set { _numbers = value; }
		}

		public string[] stars
		{
			get { return _stars; }
			set { _stars = value; }
		}

		public string date
		{
			get { return _date; }
			set { _date = value; }
		}

	}

	public class WebService
	{
		public WebService ()
		{
		}

		public async Task <List<APIResponse>> GetSalt(string username)
		{
			try
			{

				var list = new List<APIResponse>();

				var client = new System.Net.Http.HttpClient ();
				client.BaseAddress = new Uri ("https://edr.io/em/api/v1/");
				var response = await client.GetAsync("salt/username/"+username);

				response.EnsureSuccessStatusCode();
				var responseJSON = await response.Content.ReadAsStringAsync();

				var msg  = JsonConvert.DeserializeObject<APIResponse>(responseJSON);
				list.Add(msg);

				return list;
			}

			catch (Exception exc)
			{
				APIResponse resp = null;
				resp.result = "Error";
				resp.error = exc.Message;
				resp.key = "Exception";

				var list = new List<APIResponse> ();
				list.Add (resp);

				return list;
			}

		}

		public async Task <List<APIResponse>> PerformLogin(Login login)
		{
			try
			{

				var list = new List<APIResponse>();

				var client = new System.Net.Http.HttpClient ();
				client.BaseAddress = new Uri ("https://edr.io/em/api/v1/");
				string loginJSON = "username="+login.Username+"&password="+login.Password;
				var response = await client.GetAsync("login?"+loginJSON);

				response.EnsureSuccessStatusCode();
				var responseJSON = await response.Content.ReadAsStringAsync();

				var msg  = JsonConvert.DeserializeObject<APIResponse>(responseJSON);
				list.Add(msg);

				return list;
			}

			catch (Exception exc)
			{
				APIResponse resp = null;
				resp.result = "Error";
				resp.error = exc.Message;
				resp.key = "Exception";

				var list = new List<APIResponse> ();
				list.Add (resp);

				return list;
			}

		}

		public async Task <List<APIResponse>> PerformRegister(Login login, byte[] salt)
		{
			try
			{

				var list = new List<APIResponse>();

				var client = new System.Net.Http.HttpClient ();
				client.BaseAddress = new Uri ("https://edr.io/em/api/v1/");

				string pwB64 = System.Convert.ToBase64String(salt);

				//Workaround to dont loose data through POST (decoder is on server side )
				string r1 = pwB64.Replace('+','-');
				string r2 = r1.Replace('/','_');
				string finalB64 = r2.Replace('=',',');

				string loginJSON = "username="+login.Username+"&password="+login.Password+"&salt="+finalB64+"&email="+login.Email;
				HttpContent data = new StringContent(loginJSON,System.Text.Encoding.UTF8,"application/x-www-form-urlencoded");
				var response = await client.PostAsync("register",data);

				response.EnsureSuccessStatusCode();
				var responseJSON = await response.Content.ReadAsStringAsync();

				var msg  = JsonConvert.DeserializeObject<APIResponse>(responseJSON);
				list.Add(msg);

				return list;
			}

			catch (Exception exc)
			{
				APIResponse resp = null;
				resp.result = "Error";
				resp.error = exc.Message;
				resp.key = "Exception";

				var list = new List<APIResponse> ();
				list.Add (resp);

				return list;
			}

		}

		public async Task<List <Dictionary<string, dynamic>>> GetTicket(string key, string id)
		{
			try
			{

				var list = new List<Dictionary<string, dynamic>>();

				var client = new System.Net.Http.HttpClient ();
				client.BaseAddress = new Uri ("https://edr.io/em/api/v1/");
				string loginJSON = "key="+key;
				var response = await client.GetAsync("OCR/"+id+"?"+loginJSON);

				response.EnsureSuccessStatusCode();
				var responseJSON = await response.Content.ReadAsStringAsync();
				//string final = responseJSON.Replace("\\","");
				var msg  = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(responseJSON);
				list.Add(msg);

				return list;
			}

			catch (Exception exc)
			{
				APIResponse resp = null;
				resp.result = "Error";
				resp.error = exc.Message;
				resp.key = "Exception";

				Dictionary<string, dynamic> respD = new Dictionary<string, dynamic>();
				respD.Add ("result", resp.result);
				respD.Add ("error", resp.error);


				var list = new List<Dictionary<string, dynamic>>();
				list.Add (respD);

				return list;
			}

		}



		public async Task <List<APIResponse>> RequestOCR(byte[] img, string key)
		{
			try
			{

				var list = new List<APIResponse>();

				var client = new System.Net.Http.HttpClient ();
				client.BaseAddress = new Uri ("https://edr.io/em/api/v1/");

				string imgb64 = System.Convert.ToBase64String(img);

				//Workaround to dont loose data through POST (decoder is on server side )
				string r1 = imgb64.Replace('+','-');
				string r2 = r1.Replace('/','_');
				string final = r2.Replace('=',',');

				string dataStr = "key="+key+"&blob="+final;
				HttpContent data = new StringContent(dataStr,System.Text.Encoding.UTF8,"application/x-www-form-urlencoded");


				var response = await client.PostAsync("OCR",data);
				
				response.EnsureSuccessStatusCode();
				var responseJSON = await response.Content.ReadAsStringAsync();

				var msg  = JsonConvert.DeserializeObject<APIResponse>(responseJSON);
				list.Add(msg);

				return list;
			}

			catch (Exception exc)
			{
				APIResponse resp = null;
				resp.result = "Error";
				resp.error = exc.Message;
				resp.key = "Exception";

				var list = new List<APIResponse> ();
				list.Add (resp);

				return list;
			}

		}

		public async Task<List <APIResponse>> DeleteTicket(string key, string id)
		{
			try
			{

				var list = new List<APIResponse>();

				var client = new System.Net.Http.HttpClient ();
				client.BaseAddress = new Uri ("https://edr.io/em/api/v1/");
				string loginJSON = "key="+key;
				var response = await client.DeleteAsync("OCR/"+id+"?"+loginJSON);

				response.EnsureSuccessStatusCode();
				var responseJSON = await response.Content.ReadAsStringAsync();
				var msg  = JsonConvert.DeserializeObject<APIResponse>(responseJSON);
				list.Add(msg);

				return list;
			}

			catch (Exception exc)
			{
				APIResponse resp = null;
				resp.result = "Error";
				resp.error = exc.Message;
				resp.key = "Exception";

				var list = new List<APIResponse> ();
				list.Add (resp);

				return list;
			}

		}

		public async Task <List<APIResponse>> PushData(string token, byte[] db)
		{
			try
			{

				var list = new List<APIResponse>();

				var client = new System.Net.Http.HttpClient ();
				client.BaseAddress = new Uri ("https://edr.io/em/api/v1/");

				string db64 = System.Convert.ToBase64String(db);

				//Workaround to dont loose data through POST (decoder is on server side )
				string r1 = db64.Replace('+','-');
				string r2 = r1.Replace('/','_');
				string final = r2.Replace('=',',');

				string dataStr = "key="+token;
				HttpContent data = new StringContent(final,System.Text.Encoding.UTF8,"application/x-www-form-urlencoded");
				var response = await client.PutAsync("data/"+App.Current.Properties["id"]+"?"+dataStr,data);

				response.EnsureSuccessStatusCode();
				var responseJSON = await response.Content.ReadAsStringAsync();

				var msg  = JsonConvert.DeserializeObject<APIResponse>(responseJSON);
				list.Add(msg);

				return list;
			}

			catch (Exception exc)
			{
				APIResponse resp = null;
				resp.result = "Error";
				resp.error = exc.Message;
				resp.key = "Exception";

				var list = new List<APIResponse> ();
				list.Add (resp);

				return list;
			}

		}

		public async Task <List<APIResponse>> GetData(string token)
		{
			try
			{

				var list = new List<APIResponse>();

				var client = new System.Net.Http.HttpClient ();
				client.BaseAddress = new Uri ("https://edr.io/em/api/v1/");

				string dataStr = "key="+token;
				var response = await client.GetAsync("data/"+App.Current.Properties["id"]+"?"+dataStr);

				response.EnsureSuccessStatusCode();
				var responseJSON = await response.Content.ReadAsStringAsync();

				var msg  = JsonConvert.DeserializeObject<APIResponse>(responseJSON);
				list.Add(msg);

				return list;
			}

			catch (Exception exc)
			{
				APIResponse resp = null;
				resp.result = "Error";
				resp.error = exc.Message;
				resp.key = "Exception";

				var list = new List<APIResponse> ();
				list.Add (resp);

				return list;
			}

		}

		public async Task <List<APIResponse>> GetID(string token,string username)
		{
			try
			{

				var list = new List<APIResponse>();

				var client = new System.Net.Http.HttpClient ();
				client.BaseAddress = new Uri ("https://edr.io/em/api/v1/");

				string dataStr = "key="+token;
				var response = await client.GetAsync("ID/username/"+username+"?"+dataStr);

				response.EnsureSuccessStatusCode();
				var responseJSON = await response.Content.ReadAsStringAsync();

				var msg  = JsonConvert.DeserializeObject<APIResponse>(responseJSON);
				list.Add(msg);

				return list;
			}

			catch (Exception exc)
			{
				APIResponse resp = null;
				resp.result = "Error";
				resp.error = exc.Message;
				resp.key = "Exception";

				var list = new List<APIResponse> ();
				list.Add (resp);

				return list;
			}

		}

		public async Task <List<APIResponse>> SendToken(string key, string token)
		{
			try
			{

				var list = new List<APIResponse>();

				var client = new System.Net.Http.HttpClient ();
				client.BaseAddress = new Uri ("https://edr.io/em/api/v1/");
				string final = token;
				string dataStr = "key="+key;
				HttpContent data = new StringContent(final,System.Text.Encoding.UTF8,"application/x-www-form-urlencoded");
				var response = await client.PutAsync("push/"+App.Current.Properties["id"]+"?"+dataStr,data);

				response.EnsureSuccessStatusCode();
				var responseJSON = await response.Content.ReadAsStringAsync();

				var msg  = JsonConvert.DeserializeObject<APIResponse>(responseJSON);
				list.Add(msg);

				return list;
			}

			catch (Exception exc)
			{
				APIResponse resp = null;
				resp.result = "Error";
				resp.error = exc.Message;
				resp.key = "Exception";

				var list = new List<APIResponse> ();
				list.Add (resp);

				return list;
			}

		}

		public async Task <List <Dictionary<string, dynamic>> > GetResults(string token)
		{
			try
			{

				var list = new List<Dictionary<string, dynamic>>();

				var client = new System.Net.Http.HttpClient ();
				client.BaseAddress = new Uri ("https://edr.io/em/api/v1/");

				string dataStr = "key="+token;
				var response = await client.GetAsync("gameResults"+"?"+dataStr);

				response.EnsureSuccessStatusCode();
				var responseJSON = await response.Content.ReadAsStringAsync();

				var msg  = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(responseJSON);

				list.Add(msg);

				return list;
			}

			catch (Exception exc)
			{
				Dictionary<string, dynamic> respD = new Dictionary<string, dynamic>();


				var list = new List<Dictionary<string, dynamic>>();
				list.Add (respD);

				return list;
			}

		}


	}
}

