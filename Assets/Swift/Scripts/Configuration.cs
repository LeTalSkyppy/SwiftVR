using UnityEngine;
using System.Xml.Serialization;
using System.Xml;
using System;
using System.IO;
/**
 * Helper class with methods to serialize and deserialize json and xml files
 */
public static class Configuration
{
    static public Config DeserializeFromFile (string configPath)
    {
        if (File.Exists(configPath))
        {
            if (Path.GetExtension(configPath) == ".json")
            {
                string json = File.ReadAllText(configPath);
                return Deserialize(json, "json");
            }
            else if (Path.GetExtension(configPath) == ".xml")
            {
                string xml = File.ReadAllText(configPath);
                return Deserialize(xml, "xml");
            }
            else
            {
                Debug.LogError("the configuration file format is not supported");
                throw new FormatException();
            }
        }
        else
        {
            Debug.LogError("configuration file not found");
            throw new FileNotFoundException();
        }
    }

    static public Config Deserialize (string content, string format)
    {
        switch (format)
        {
            case "json":
                try
                {
                    return JsonUtility.FromJson<Config>(content);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                    throw new FileLoadException();
                }
            case "xml":
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Config));

                    using (TextReader reader = new StringReader(content))
                    {
                        return (Config)serializer.Deserialize(reader);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                    throw new FileLoadException();
                }
            default:
                throw new ArgumentException("the format requested is not supported");
        }
    }

    static public string Serialize (Config config, string format)
    {
        switch (format)
        {
            case "json":
                return JsonUtility.ToJson(config, true);
            case "xml":
                var xmlserializer = new XmlSerializer(typeof(Config));
                var stringWriter = new StringWriter();

                // pretty printing
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;

                using (var writer = XmlWriter.Create(stringWriter, settings))
                {
                    xmlserializer.Serialize(writer, config);
                    return stringWriter.ToString();
                }
            default:
                throw new ArgumentException("the format requested is not supported");
        }
    }

    static public void SerializeToFile (Config config, string configPath)
    {
        if (Path.GetExtension(configPath) == ".json")
        {
            File.WriteAllText(configPath, Serialize(config, "json"));
        }
        else if (Path.GetExtension(configPath) == ".xml")
        {
            File.WriteAllText(configPath, Serialize(config, "xml"));
        }
        else
        {
            Debug.LogError("the configuration file format is not supported");
            throw new FormatException();
        }
    }

    static public void Export ()
    {
        Exportable[] exportables = GameObject.FindObjectsOfType<Exportable>();
        DateTime localDate = DateTime.Now;

        Debug.Log(exportables.Length);

        Config config = new Config();
        config.elements = new Config.Element[exportables.Length];

        int i = 0;
        foreach (Exportable exportable in exportables)
        {
            config.elements[i++] = new Config.Element(exportable.name, exportable.transform.position, exportable.transform.rotation);
        }

        SerializeToFile(config, "Assets/StreamingAssets/SavedLayout/Swift" + localDate.ToString("dd_MM_yyyy-HH_mm_ss") +".json");
    }

    static public void Import ()
    {
        string lastFilePath = "";
        DateTime dateTimeFile = new DateTime();

        foreach(string file in System.IO.Directory.GetFiles("Assets/StreamingAssets/SavedLayout/"))
        {
            if(Path.GetExtension(file) == ".json")
            {
                var fileInfo = new FileInfo(file);
                if(DateTime.Compare(fileInfo.CreationTime,dateTimeFile) > 0)
                {
                    lastFilePath = file;
                    dateTimeFile = fileInfo.CreationTime;
                }

            }
        }
        try
        {
            Config config = DeserializeFromFile(lastFilePath);

            foreach (Config.Element element in config.elements)
            {
                GameObject obj = GameObject.Find(element.name);
                obj.transform.position = element.position;
                obj.transform.rotation = element.rotation;
            } 
        }
        catch
        {

        }
        
    }
}
