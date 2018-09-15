using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ConfigurationManager
{
    /// <summary>
    /// This class allows to manage your application settings.
    /// Resulting configuration file contains settings in JSON format.
    /// </summary>
    public class ApplicationSettings
    {
        private bool _settingsLoaded;

        private ApplicationSettingsContainer LoadedSettings { get; set; }
        private string ConfigPath { get; }

        /// <summary>
        /// Initializes class and setting configuration file path.
        /// </summary>
        /// <param name="path">Absolute desired path to your configuration file.</param>
        public ApplicationSettings(string path)
        {
            ConfigPath = path;
        }

        #region Creating/loading/saving configuration file
        /// <summary>
        /// Creates empty configuration file.
        /// </summary>
        /// <param name="defaultValues">Default config values.</param>
        /// <returns>Returns true if creating is succeeded.</returns>
        public bool CreateConfig(Dictionary<string, object> defaultValues = null)
        {
            try
            {
                File.Create(ConfigPath).Dispose();

                var emptyConfig = new ApplicationSettingsContainer();

                if (defaultValues != null)
                {
                    emptyConfig.VariablesDictionary = defaultValues;
                }

                var json = JsonConvert.SerializeObject(emptyConfig);

                File.WriteAllText(ConfigPath, json);

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Loads configuration from file.
        /// Creates empty config first if needed.
        /// </summary>
        /// <returns>Returns true if creating/loading operations are succeeded.</returns>
        public bool LoadConfigCreateIfNeeded(Dictionary<string, object> defaultValues = null)
        {
            if (!File.Exists(ConfigPath))
            {
                if (!CreateConfig(defaultValues))
                {
                    return false;
                }
            }

            try
            {
                LoadConfigUnsafe();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Loads configuration from file.
        /// </summary>
        /// <returns>Returns true if loading succeeded.</returns>
        public bool LoadConfig()
        {
            if (!File.Exists(ConfigPath))
            {
                return false;
            }

            try
            {
                LoadConfigUnsafe();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Internal function for loading configuration from file.
        /// It has no sanity checks.
        /// Shouldn't be called from outside of this class!
        /// </summary>
        private void LoadConfigUnsafe()
        {
            var json = File.ReadAllText(ConfigPath);
            var loadedContainer = JsonConvert.DeserializeObject<ApplicationSettingsContainer>(json);

            LoadedSettings = loadedContainer;
            _settingsLoaded = true;
        }

        /// <summary>
        /// Saves configuration to file.
        /// </summary>
        /// <returns>Returns true if succeeded.</returns>
        public bool SaveConfig()
        {
            if (!_settingsLoaded)
            {
                return false;
            }

            try
            {
                var json = JsonConvert.SerializeObject(LoadedSettings);
                File.WriteAllText(ConfigPath, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if configuration file exists.
        /// </summary>
        /// <returns>Returns true if configuration file exists.</returns>
        public bool ConfigExists() => File.Exists(ConfigPath);
        
        /// <returns>Returns true if Settings are loaded.</returns>
        public bool AreSettingsLoaded() => _settingsLoaded;
        #endregion

        #region Reading/writing settings
        /// <summary>
        /// Checks if given config entry exists.
        /// </summary>
        /// <param name="key">Config entry name.</param>
        /// <returns>Returns true if config entry exists.</returns>
        public bool IsConfigEntryExists(string key) => LoadedSettings.VariablesDictionary.ContainsKey(key);

        /// <summary>
        /// Tries to return config entry value by name.
        /// </summary>
        /// <typeparam name="T">Config entry value type.</typeparam>
        /// <param name="key">Config entry name.</param>
        /// <exception cref="Exception">Throws when config entry wasn't found by name.</exception>
        /// <returns>Returns config entry as T.</returns>
        public T GetConfigEntry<T>(string key)
        {
            if (!LoadedSettings.VariablesDictionary.ContainsKey(key))
            {
                throw new Exception("Config entry wasn't found.");
            }

            return (T) LoadedSettings.VariablesDictionary[key];
        }

        /// <summary>
        /// Sets config entry value.
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <param name="key">Config entry name.</param>
        /// <param name="value">New config entry value.</param>
        public void SetConfigEntry<T>(string key, T value)
        {
            if (!LoadedSettings.VariablesDictionary.ContainsKey(key))
            {
                LoadedSettings.VariablesDictionary.Add(key, value);
                return;
            }
            
            LoadedSettings.VariablesDictionary[key] = value;
        }
        #endregion
    }
}