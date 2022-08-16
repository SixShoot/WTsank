using System.Data.SQLite;
using System.IO;

namespace WorldOfTanks {

	class BoxDao {

		public static readonly BoxDao Instance = new BoxDao ();

		readonly object Lock = new object ();
		readonly SQLiteConnection SQLiteConnection;

		const string FilePath = "World Of Tanks.sqlite";

		public BoxDao () {
			lock (Lock) {
				SQLiteConnection = new SQLiteConnection ($"data source={FilePath}");
				SQLiteConnection.Open ();
				CreateTable ();
			}
		}

		public void SaveArenaJson (string id, string json) {
			lock (Lock) {
				if (TryGetArenaJson (id, out _)) {
					return;
				}
				new SQLiteCommand ($@"insert into box_battle_detail (id, json) values ('{id}', '{json}');", SQLiteConnection).ExecuteNonQuery ();
			}
		}

		public bool TryGetArenaJson (string id, out string json) {
			lock (Lock) {
				using (SQLiteDataReader reader = new SQLiteCommand ($@"select json from box_battle_detail where id = '{id}';", SQLiteConnection).ExecuteReader ()) {
					if (reader.Read ()) {
						json = reader.GetString (0);
						return true;
					}
					json = null;
					return false;
				}
			}
		}

		public long GetCacheSize () {
			lock (Lock) {
				return new FileInfo (FilePath).Length;
			}
		}

		public void ClearCache () {
			lock (Lock) {
				new SQLiteCommand ("delete from box_battle_detail; VACUUM;", SQLiteConnection).ExecuteNonQuery ();
				CreateTable ();
			}
		}

		void CreateTable () {
			lock (Lock) {
				new SQLiteCommand (@"
					create table if not exists box_battle_detail (
						id text primary key,
						json text
					);
				", SQLiteConnection).ExecuteNonQuery ();
			}
		}

	}

}