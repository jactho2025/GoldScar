using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Utilities
{
    public static class Serialiser
    {
        public static void ToFile<T>(T instance, string path)
        {
            try
            {
                var fs = new FileStream(path, FileMode.Create);
                var serialiser = new DataContractSerializer(typeof(T));
                serialiser.WriteObject(fs, instance);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                //TODO: log error
            }
        }

        internal static T FromFile<T>(string path)
        {
            try
            {
                var fs = new FileStream(path, FileMode.Open);
                var serialiser = new DataContractSerializer(typeof(T));
                T instance = (T)serialiser.ReadObject(fs);
                return instance;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                //TODO: log error
                return default(T);
            }
        }
    }
}
