using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace vGraphic
{
    class Helper
    {
        static ResourceManager rm;
        static ResourceSet rs;
        static Dictionary<string, object> GraphicFromResxFile = new Dictionary<string, object>();
        //  static string location = System.Reflection.Assembly.GetEntryAssembly().Location;
        //  static string directoryPath = Path.GetDirectoryName(location).ToString();
      //  public static string ResxFile =  @"G:\MVVm\New folder\Hten.SymFact\bin\Debug\GraphicLib.Lib"; // File thêm vào
        public static string ResxFile = AppDomain.CurrentDomain.BaseDirectory+ "\\GraphicLib.Lib"; // File thêm vào

       

        // public static string ResxFile = @"bin\Debug\GraphicLib.Lib";

        public static List<Dir> listDir = new List<Dir>()
        {
            new Dir(){dirname="Graphics",dicsym=new Dictionary<string, string>()}
        };
        public static void GetListResource()
        {

            rm = SymLib.ResourceManager;

            rs = rm.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);

            var objs =
                    (from obj in rs.Cast<DictionaryEntry>()
                     select obj).ToList();
            foreach (var v in objs)
            {
                Dir _dir;
                string dir = v.Key.ToString().Split(new string[] { "___" }, StringSplitOptions.None)[0];
                string sym = v.Key.ToString().Split(new string[] { "___" }, StringSplitOptions.None)[1];
                if (!listDir.Any(p => p.dirname == dir))
                {
                    _dir = new Dir();
                    _dir.dirname = dir;
                    listDir.Add(_dir);
                }
                else
                {
                    _dir = listDir.Where(p => p.dirname == dir).FirstOrDefault();
                }
                if (!_dir.dicsym.ContainsKey(sym))
                {

                    _dir.dicsym.Add(sym, v.Value.ToString());
                }
            }
            try
            {
                GraphicFromResxFile = ReadAllResxEntries(ResxFile);
                foreach (var v in GraphicFromResxFile)
                {
                    Dir _dir;
                    string dir = v.Key.ToString().Split(new string[] { "___" }, StringSplitOptions.None)[0];
                    string sym = v.Key.ToString().Split(new string[] { "___" }, StringSplitOptions.None)[1];
                    if (!listDir.Any(p => p.dirname == dir))
                    {
                        _dir = new Dir();
                        _dir.dirname = dir;
                        listDir.Add(_dir);
                    }
                    else
                    {
                        _dir = listDir.Where(p => p.dirname == dir).FirstOrDefault();
                    }
                    if (!_dir.dicsym.ContainsKey(sym))
                    {

                        _dir.dicsym.Add(sym, v.Value.ToString());
                    }
                }
            }
            catch { }
        }

        public static string getdata(string name)
        {
        //    MessageBox.Show(ResxFile);
            if (name.StartsWith("Graphics___"))
            {
                if (GraphicFromResxFile.ContainsKey(name))
                {
                    return GraphicFromResxFile[name].ToString();
                }
                else
                {
                    GraphicFromResxFile = ReadAllResxEntries(ResxFile);
                    if (GraphicFromResxFile.ContainsKey(name))
                    {
                        return GraphicFromResxFile[name].ToString();
                    }
                }
                return vGraphic.Properties.Resources.XXX;
            }
            else
            {
                return SymLib.ResourceManager.GetString(name, SymLib.Culture);
            }
        }

        public static int AddOrUpdateResource(string resxfile, string key, string value, int forceupdate = 0)
        {
            int result = 0;

            var resx = new List<DictionaryEntry>();
            string resourceFilepath = resxfile;
            using (var reader = new ResXResourceReader(resourceFilepath))
            {
                resx = reader.Cast<DictionaryEntry>().ToList();
                var existingResource = resx.Where(r => r.Key.ToString() == key).FirstOrDefault();
                if (existingResource.Key == null && existingResource.Value == null) // NEW!
                {
                    resx.Add(new DictionaryEntry() { Key = key, Value = value });
                    result = 1;
                }

                if (forceupdate == 0)
                {
                    if (existingResource.Key != null && (existingResource.Value == null))// || existingResource.Value == "")) // NEW!
                    {
                        var modifiedResx = new DictionaryEntry() { Key = existingResource.Key, Value = value };
                        resx.Remove(existingResource);  // REMOVING RESOURCE!
                        resx.Add(modifiedResx);  // AND THEN ADDING RESOURCE!
                        result = 1;
                    }
                }
                if (forceupdate == 1)
                {
                    if (existingResource.Key != null) // NEW!
                    {
                        var modifiedResx = new DictionaryEntry() { Key = existingResource.Key, Value = value };
                        resx.Remove(existingResource);  // REMOVING RESOURCE!
                        resx.Add(modifiedResx);  // AND THEN ADDING RESOURCE!
                        result = 1;
                    }

                }
            }
            if (result > 0)
            {
                using (var writer = new ResXResourceWriter(resourceFilepath))
                {
                    resx.ForEach(r =>
                    {
                        // Again Adding all resource to generate with final items
                        writer.AddResource(r.Key.ToString(), r.Value.ToString());
                    });
                    writer.Generate();
                }
            }
            return result;
        }

        public static int RemoveResource(string resxfile, string key)
        {
            int result = 0;

            var resx = new List<DictionaryEntry>();
            string resourceFilepath = resxfile;
            using (var reader = new ResXResourceReader(resourceFilepath))
            {
                resx = reader.Cast<DictionaryEntry>().ToList();
                var existingResource = resx.Where(r => r.Key.ToString() == key).FirstOrDefault();
                if (existingResource.Key != null)
                {
                    resx.Remove(existingResource);  // REMOVING RESOURCE!
                    result = 1;

                }
                if (result > 0)
                {
                    using (var writer = new ResXResourceWriter(resourceFilepath))
                    {
                        resx.ForEach(r =>
                        {
                            // Again Adding all resource to generate with final items
                            writer.AddResource(r.Key.ToString(), r.Value.ToString());
                        });
                        writer.Generate();
                    }
                }
            }
            return result;
        }

        private static Dictionary<string, object> ReadAllResxEntries(string resxfile)
        {
            using (var reader = new ResXResourceReader(resxfile))
            {
                return reader
                    .Cast<DictionaryEntry>()
                    .ToDictionary(entry => (string)entry.Key, entry => entry.Value);
            }
        }

    }
}
