using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite.Net.Attributes;

namespace Euromillions
{
	public class Bet : INotifyPropertyChanged
	{

		private int _id;
		private string _numbers;
		private string _stars;
		private string _date;
		private string _status; // WAIT / WIN / LOSS

		public Bet(){
			this._status = "WAIT";
		}
			
		public Bet(string numbers, string stars, string date, string status)
		{
			_numbers = numbers;
			_stars = stars;
			_date = date;
			_status = status;
		}
		[PrimaryKey,AutoIncrement]
		public int ID{
			get { return _id; }
			set { _id = value; }

		}

		public string Numbers
		{
			get { return _numbers; }
			set { _numbers = value; OnPropertyChanged(); }
		}

		public string Date
		{
			get { return _date; }
			set { _date = value; OnPropertyChanged(); }
		}

		public string Stars
		{
			get { return _stars; }
			set { _stars = value; OnPropertyChanged (); }
		}

		public string Status
		{
			get { return _status; }
			set { _status = value; OnPropertyChanged (); }
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

