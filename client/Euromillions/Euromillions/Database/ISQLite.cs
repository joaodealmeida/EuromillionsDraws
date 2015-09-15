using System;
using SQLite.Net;


namespace Euromillions
{
	public interface ISQLite
	{
		SQLiteConnection GetConnection();

		byte[] GetDatabase();
		void OverrideDatabase(string DBb64);
	}
}

