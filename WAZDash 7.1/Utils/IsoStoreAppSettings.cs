using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;


namespace WindowsAzureStatus
{
    public class IsoStoreAppSettings
    {
        /// <summary>
        /// The Silverlight Isolated Storage Settings object, which will be lazily
        /// initlialized once the property is accessed
        /// </summary>
        private IsolatedStorageSettings _isolatedstore;

        /// <summary>
        /// The Silverlight Isolated Storage Settings object, which will be lazily
        /// initlialized once the property is accessed
        /// </summary>
        private IsolatedStorageSettings IsolatedSettings
        {
            get
            {
                if (_isolatedstore == null)
                {
                    _isolatedstore = IsolatedStorageSettings.ApplicationSettings;
                }
                return _isolatedstore;
            }
        }

        /// <summary>
        /// Sets a value in the App Settings, overwriting an existing setting if
        /// applicable
        /// </summary>
        /// <param name="key">Keyname of the setting</param>
        /// <param name="value">Value of the setting</param>
        /// <returns>True if the value was changed</returns>
        public bool AddOrUpdateValue(string key, Object value)
        {
            bool valueChanged = false;

            if (IsolatedSettings.Contains(key))
            {
                if (IsolatedSettings[key] != value)
                {
                    // set the value if it is different than what is in settings
                    IsolatedSettings[key] = value;
                    valueChanged = true;
                    return valueChanged;
                }
                else
                {
                    return valueChanged;
                }
            }
            else
            {
                IsolatedSettings.Add(key, value);
                valueChanged = true;
                return valueChanged;
            }

        }

        /// <summary>
        /// Retrieves a value from the App Settings. If the value does not
        /// exist in the settings, the defaultValue is returned.
        /// </summary>
        /// <typeparam name="T">Type desired</typeparam>
        /// <param name="Key">Key</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>The value, or the default value if not found</returns>
        public T GetValueOrDefault<T>(string key, T defaultValue)
        {

            if (IsolatedSettings.Contains(key))
            {
                return (T)IsolatedSettings[key];
            }
            else
            {
                return defaultValue;
            }

            
        }

        /// <summary>
        /// Explicit Save.  Not needed as Save will automatically be called at appInstance exit
        /// </summary>
        public void Save()
        {
            IsolatedSettings.Save();
        }
    }
}
