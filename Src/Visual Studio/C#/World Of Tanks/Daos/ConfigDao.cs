using Eruru.Json;

namespace WorldOfTanks {

	class ConfigDao {

		public static ConfigDao Instance {

			get {
				lock (Lock) {
					return _Instance;
				}
			}

		}

		readonly static object Lock = new object ();
		static readonly ConfigDao _Instance = new ConfigDao ();

		readonly string Path = "Config.json";

		public void Save (ConfigService config) {
			lock (Lock) {
				JsonConvert.Serialize (config, Path, false);
			}
		}

		public ConfigService Load () {
			lock (Lock) {
				try {
					return JsonConvert.DeserializeFile<ConfigService> (Path);
				} catch {
					return new ConfigService ();
				}
			}
		}

	}

}