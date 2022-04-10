using Eruru.Json;

namespace WorldOfTanks {

	class ConfigDao {

		public static ConfigDao Instance = new ConfigDao ();

		readonly string Path = "Config.json";

		public void Save (Config config) {
			JsonConvert.Serialize (config, Path, false);
		}

		public Config Load () {
			try {
				return JsonConvert.DeserializeFile<Config> (Path);
			} catch {
				return new Config ();
			}
		}

	}

}