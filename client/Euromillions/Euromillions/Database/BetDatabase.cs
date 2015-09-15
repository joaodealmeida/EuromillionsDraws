using System;
using SQLite.Net;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Euromillions
{
	public class BetDatabase
	{

		static object locker = new object();
		SQLiteConnection db;

		public BetDatabase ()
		{
			db = DependencyService.Get<ISQLite> ().GetConnection ();

			db.CreateTable<Bet> ();
		}

		public IEnumerable<Bet> GetBets()
		{
			lock (locker) {
				List<Bet> ret =  (from i in db.Table<Bet> () select i).ToList ();
				return ret.OrderByDescending(o=>o.ID).ToList();
			}
		}

		public Bet GetBet (int id)
		{
			lock (locker) {
				return db.Table<Bet>().FirstOrDefault(x => x.ID == id);
			}	
		}

		public int SaveBet (Bet bet)
		{
			lock (locker) {
				WebService conn = new WebService ();
				if (bet.ID != 0) {
					db.Update (bet);
					byte[] teste = DependencyService.Get<ISQLite>().GetDatabase();
					conn.PushData (App.Current.Properties ["key"] as string, teste);
					return bet.ID;
				} else {
					int ret = db.Insert (bet);
					byte[] teste = DependencyService.Get<ISQLite>().GetDatabase();
					conn.PushData(App.Current.Properties["key"] as string, teste);
					return ret;
				}
			}
		}

		public int DeleteBet(int id)
		{
			lock (locker) {
				int ret = db.Delete<Bet> (id);
				byte[] teste = DependencyService.Get<ISQLite>().GetDatabase();
				WebService conn = new WebService ();
				conn.PushData(App.Current.Properties["key"] as string, teste);
				return ret;
			}
		}
	}
}

