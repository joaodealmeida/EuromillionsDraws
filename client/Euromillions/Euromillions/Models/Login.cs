using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Euromillions
{
	public class Login : INotifyPropertyChanged
	{
		string _username;
		string _password;
		string _email;

		public Login ()
		{
		}

		public string Username
		{
			get { return _username; }
			set { _username = value; OnPropertyChanged(); }
		}

		public string Password
		{
			get { return _password; }
			set { _password = value; OnPropertyChanged(); }
		}

		public string Email
		{
			get { return _email; }
			set { _email = value; OnPropertyChanged(); }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}


	}
}

