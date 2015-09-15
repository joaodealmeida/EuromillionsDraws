using System;
using Euromillions;
using Xamarin.Forms;
using Euromillions.iOS;
using System.IO;
using SQLite;

[assembly: Dependency (typeof (SQLite_iOS))]

namespace Euromillions.iOS
{
	public class SQLite_iOS : ISQLite
	{

		public SQLite_iOS()
		{
		}

		#region ISQLite implementation

		public SQLite.Net.SQLiteConnection GetConnection()
		{
			var sqliteFilename = "BetDatabase.db3";
			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string libraryPath = Path.Combine (documentsPath, "..", "Library");
			var path = Path.Combine (libraryPath, sqliteFilename);

			/*	if (!File.Exists (path)) {
				File.Copy (sqliteFilename, path);
			}	*/

			var plat = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
			var conn = new SQLite.Net.SQLiteConnection (plat, path);

			return conn;
		}

		public byte[] GetDatabase()
		{
			var sqliteFilename = "BetDatabase.db3";
			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string libraryPath = Path.Combine (documentsPath, "..", "Library");
			var dbPath = Path.Combine (libraryPath, sqliteFilename);

			byte[] dbByte = File.ReadAllBytes (dbPath);
			return dbByte;
		}

		public void OverrideDatabase(string DBb64)
		{
			var sqliteFilename = "BetDatabase.db3";
			string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string libraryPath = Path.Combine (documentsPath, "..", "Library");
			var dbPath = Path.Combine (libraryPath, sqliteFilename);

			//Workaround - decoder for b64
			string r1 = DBb64.Replace('-','+');
			string r2 = r1.Replace('_','/');
			string finalB64 = r2.Replace(',','=');

			byte[] dbByte = System.Convert.FromBase64String(finalB64);

			File.WriteAllText (dbPath, string.Empty);
			File.WriteAllBytes (dbPath, dbByte);
		}
		#endregion
	}
}