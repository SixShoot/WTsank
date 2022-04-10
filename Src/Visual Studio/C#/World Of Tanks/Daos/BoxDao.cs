using System.Data.SQLite;

namespace WorldOfTanks {

	class BoxDao {

		public static readonly BoxDao Instance = new BoxDao ();

		readonly SQLiteConnection SQLiteConnection;
		readonly object Lock = new object ();

		const string FilePath = "World Of Tanks.sqlite";

		public BoxDao () {
			SQLiteConnection = new SQLiteConnection ($"data source={FilePath}");
			SQLiteConnection.Open ();
			new SQLiteCommand (@"
				create table if not exists box_battle_detail (
					id text primary key,
					json text
				);
			", SQLiteConnection).ExecuteNonQuery ();
		}

		public void SaveArenaJson (string id, string json) {
			lock (Lock) {
				if (TryGetArenaJson (id, out _)) {
					return;
				}
				new SQLiteCommand ($@"insert into box_battle_detail (id, json) values ('{id}', '{json}')", SQLiteConnection).ExecuteNonQuery ();
			}
		}

		public bool TryGetArenaJson (string id, out string json) {
			lock (Lock) {
				using (SQLiteDataReader reader = new SQLiteCommand ($@"select json from box_battle_detail where id = '{id}'", SQLiteConnection).ExecuteReader ()) {
					if (reader.Read ()) {
						json = reader.GetString (0);
						return true;
					}
					json = null;
					return false;
				}
			}
		}

	}

}