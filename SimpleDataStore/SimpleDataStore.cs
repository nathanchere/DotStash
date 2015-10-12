using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NServiceKit.Text;

namespace DotStash
{
    public class SimpleDataStore : IDataStore
    {
        // TODO: maybe config provider? Hopefully overkill.
        public class ConfigurationModel
        {
            internal ConfigurationModel()
            {
                TypeKeyProperties = new TypeDictionary<string>();
                TypeFolderNames = new TypeDictionary<string>();
            }

            private static readonly string DefaultDataPath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "DotStash");

            public string DataStoreRootPath { get; set; } = DefaultDataPath;

            /// <summary>
            /// Roughly equivalent of "Database Name" in RDBMS land
            /// </summary>
            public string DataStoreName { get; set; }

            public bool UseFullNameAsFolder { get; set; } = false;

            public string RecordFileExtension { get; set; } = ".json";

            public string DefaultKeyProperty { get; set; } = "Id";

            /// <remarks>
            /// Warning: this will delete any persisted data when the data store instance is disposed
            /// </remarks>
            public bool CleanupOnExit { get; set; } = false;

            internal readonly TypeDictionary<string> TypeKeyProperties;
            internal readonly TypeDictionary<string> TypeFolderNames;
        }

        public readonly ConfigurationModel Config = new ConfigurationModel();

        public SimpleDataStore(string dataStoreName)
        {
            Config.DataStoreName = dataStoreName;
        }
       
        public virtual string DataPath<T>()
        {
            return Path.Combine(
                Path.Combine(Config.DataStoreRootPath,Config.DataStoreName),
                Config.TypeFolderNames.SafeGet<T>() ?? typeof(T).Name
            );
        }

        public virtual string GetFileName<T>(T item)
        {
            var id = GetKeyProperty(item);
            return Path.Combine(DataPath<T>(), string.Format("{0}{1}", id, Config.RecordFileExtension));
        }

        private void VerifyPathExists(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        private string GetKeyProperty<T>(T item)
        {
            var key = Config.TypeKeyProperties.SafeGet<T>() ?? Config.DefaultKeyProperty;
            return item.GetType().GetProperty(key).GetValue(item, null).ToString();
        }

        public IEnumerable<T> GetAll<T>()
        {
            var path = DataPath<T>();
            VerifyPathExists(path);

            var files = Directory.GetFiles(path).Where(x => x.EndsWith(Config.RecordFileExtension));
            return files
                .Select(File.ReadAllText)
                .Select(text => text.FromJson<T>());
        }

        public T Get<T>(object id)
        {
            var path = DataPath<T>();
            VerifyPathExists(path);

            var file = Directory.GetFiles(path)
                .SingleOrDefault(x => Path.GetFileName(x) == string.Format("{0}{1}", id, Config.RecordFileExtension));

            if (file == null) return default(T);
            return File.ReadAllText(file).FromJson<T>();
        }

        public void Save<T>(T item)
        {
            var path = DataPath<T>();
            VerifyPathExists(path);

            var serialisedItem = item.ToJson();
            var fileName = GetFileName(item);

            File.WriteAllText(fileName, serialisedItem);
        }

        public void Delete<T>(object key)
        {
            var path = DataPath<T>();
            VerifyPathExists(path);

            File.Delete(path);
        }

        public void Configure<T>(string folderName = null, string keyName = null)
        {
            if (folderName != null)
                Config.TypeFolderNames[typeof(T)] = folderName;

            if (keyName != null)
                Config.TypeKeyProperties[typeof(T)] = keyName;
        }

        private void DeleteResource(string path, bool throwOnError = false)
        {
            if (!Directory.Exists(path))
            {
                Debug.WriteLine("Nothing to delete");
                return;
            }

            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException ex)
            {
                Debug.WriteLine(string.Format("Delete failed{0}Reason: {1}", Environment.NewLine, ex.Message));
                if(throwOnError) throw;
            }
        }

        public void DeleteFolder<T>()
        {
            DeleteResource(DataPath<T>(), true);
        }

        public void DeleteDataStore()
        {
            DeleteResource(Config.DataStoreRootPath, true);
        }

        public void Dispose()
        {
            if (Config.CleanupOnExit) DeleteDataStore();
        }
    }
}