/*
  Copyright © 2018 ASCON-Design Systems LLC. All rights reserved.
  This sample is licensed under the MIT License.
*/
using System.IO;
using System.Reflection;

namespace RabbitMQ.Consumer.PilotICE
{
    class DirectoryHelper
    {
        private const string VENDOR_FOLDER = "ASCON";

        public static string GetTempPath()
        {
            var tempPath = Path.Combine(GetVendorTempDirectory(), "ObjectModifierSample");
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);
            return tempPath;
        }

        public static string GetVendorTempDirectory()
        {
            var tempPath = Path.Combine(Path.GetTempPath(), VENDOR_FOLDER);
            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);
            return tempPath;
        }
        public static string GetLocalFile()
        {
            var executingAssemblyFilename = Assembly.GetExecutingAssembly().Location;
            var directory = Directory.GetParent(executingAssemblyFilename);
            var localFile = Path.Combine(directory.FullName, "file.xps");
            return localFile;
        }
    }
}