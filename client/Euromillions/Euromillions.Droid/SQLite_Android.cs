using System;
using Euromillions;
using Euromillions.Droid;
using Xamarin.Forms;
using System.IO;

[assembly: Dependency (typeof (SQLite_Android))]

namespace Euromillions.Droid
{
	public class SQLite_Android : ISQLite
	{
		public SQLite_Android ()
		{
		}

		#region ISQLite implementation
		public SQLite.Net.SQLiteConnection GetConnection()
		{
			var sqliteFilename = "BetDatabase.db3";
			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			var path = Path.Combine (documentsPath, sqliteFilename);

			/*if(!File.Exists(path))
			{
				//Resource Name
				var resource = Forms.Context.Resources.OpenRawResource (Resource.zzz);

				//Create Write Stream
				FileStream writeStream = FileStream(path,FileMode.OpenOrCreate,FileAccess.Write);
				//Write to Stream
				ReadWriteStream(resource,writeStream);
				*/
			
			var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
			var conn = new SQLite.Net.SQLiteConnection (plat, path);


			return conn;
		}

		public byte[] GetDatabase()
		{
			var sqliteFilename = "BetDatabase.db3";
			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine (documentsPath, sqliteFilename);

			byte[] dbByte = File.ReadAllBytes (dbPath);
			return dbByte;
		}

		public void OverrideDatabase(string DBb64)
		{
			var sqliteFilename = "BetDatabase.db3";
			string documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			var dbPath = Path.Combine (documentsPath, sqliteFilename);

			//Workaround - decoder for b64
			string r1 = DBb64.Replace('-','+');
			string r2 = r1.Replace('_','/');
			string finalB64 = r2.Replace(',','=');

			byte[] dbByte = System.Convert.FromBase64String(finalB64);

			File.WriteAllText (dbPath, string.Empty);
			File.WriteAllBytes (dbPath, dbByte);
		}


		/// <summary>
		/// helper method to get the database out of /raw/ and into the user filesystem
		/// </summary>
		void ReadWriteStream(Stream readStream, Stream writeStream)
		{
			int Length = 256;
			Byte[] buffer = new Byte[Length];
			int bytesRead = readStream.Read(buffer, 0, Length);
			// write the required bytes
			while (bytesRead > 0)
			{
				writeStream.Write(buffer, 0, bytesRead);
				bytesRead = readStream.Read(buffer, 0, Length);
			}
			readStream.Close();
			writeStream.Close();
		}

		#endregion
	}
}

