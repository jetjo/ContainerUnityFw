

using System;
using System.Configuration;
using System.IO;
using Microsoft.Practices.ObjectBuilder2;
using System.Linq;

namespace Microsoft.Practices.Unity.GuardSupport.Configuration
{
    public class ConfigFileLoader<TResourceLocator>
    {
        private System.Configuration.Configuration configuration;
        private string ConfigFileDir_Abs_;

        public ConfigFileLoader(string configFileName)
        {
            string configFileDir = AppDomain.CurrentDomain.BaseDirectory;
            this.ConfigFileDir_Abs_= Path.Combine(configFileDir, configFileName);// + ".config");
            DumpResourceFileToDisk(configFileName);
            LoadConfigFromFile(configFileName);
        }

        public TSection GetSection<TSection>(string sectionName)
            where TSection : ConfigurationSection
        {
            try
            {
                if (!File.Exists(ConfigFileDir_Abs_)) throw new FileNotFoundException("",this.ConfigFileDir_Abs_);
                return (TSection)configuration.GetSection(sectionName);
            }
            finally
            {
                if (File.Exists(ConfigFileDir_Abs_)) File.Delete(ConfigFileDir_Abs_);
            }

        }

        private void LoadConfigFromFile(string configFileName)
        {
            /*if (!configFileName.EndsWith(".config"))
            {
                configFileName += ".config";
            }
            var fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = configFileName
        };*/

            //this.ConfigFileDir_Abs_ = GetConfigFileDir_Abs_(configFileName);
            if (!File.Exists(ConfigFileDir_Abs_)) throw new FileNotFoundException("", this.ConfigFileDir_Abs_);
            configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
            {
                ExeConfigFilename = this.ConfigFileDir_Abs_
            }, ConfigurationUserLevel.None);

        }

        private void DumpResourceFileToDisk(string configFileName)
        {
            using (Stream resourceStream = GetResourceStream(configFileName))
            using (Stream outputStream = GetOutputStream(configFileName))
            {
                CopyStream(resourceStream, outputStream);
            }
        }

        private static Stream GetResourceStream(string configFileName)
        {
            //var name = configFileName + ".config";
            var currentAssembly = typeof(TResourceLocator).Assembly;
            string resourceName = currentAssembly.GetManifestResourceNames().First(it => it.EndsWith(configFileName));

            return currentAssembly.GetManifestResourceStream(resourceName);
        }

        private static string GetResourceNamespace()
        {
            return typeof(TResourceLocator).Namespace;
        }

        private Stream GetOutputStream(string configFileName)
        {
            return new FileStream(ConfigFileDir_Abs_, FileMode.Create, FileAccess.Write);
        }

        private static void CopyStream(Stream inputStream, Stream outputStream)
        {
            var buffer = new byte[4096];
            int numRead = inputStream.Read(buffer, 0, buffer.Length);
            while (numRead > 0)
            {
                outputStream.Write(buffer, 0, numRead);
                numRead = inputStream.Read(buffer, 0, buffer.Length);
            }
        }

        ~ConfigFileLoader() { if (File.Exists(ConfigFileDir_Abs_)) File.Delete(ConfigFileDir_Abs_); }
    }
}
