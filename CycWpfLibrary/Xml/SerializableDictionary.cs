using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CycWpfLibrary.Xml
{
  [Serializable]
  public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
  {
    public void WriteXml(XmlWriter write)       // Serializer
    {
      var KeySerializer = new XmlSerializer(typeof(TKey));
      var ValueSerializer = new XmlSerializer(typeof(TValue));

      foreach (var kv in this)
      {
        write.WriteStartElement("SerializableDictionary");
        write.WriteStartElement("key");
        KeySerializer.Serialize(write, kv.Key);
        write.WriteEndElement();
        write.WriteStartElement("value");
        ValueSerializer.Serialize(write, kv.Value);
        write.WriteEndElement();
        write.WriteEndElement();
      }
    }
    public void ReadXml(XmlReader reader)       // Deserializer
    {
      reader.Read();
      var KeySerializer = new XmlSerializer(typeof(TKey));
      var ValueSerializer = new XmlSerializer(typeof(TValue));

      while (reader.NodeType != XmlNodeType.EndElement)
      {
        reader.ReadStartElement("SerializableDictionary");
        reader.ReadStartElement("key");
        var tk = (TKey)KeySerializer.Deserialize(reader);
        reader.ReadEndElement();
        reader.ReadStartElement("value");
        var vl = (TValue)ValueSerializer.Deserialize(reader);
        reader.ReadEndElement();
        reader.ReadEndElement();
        Add(tk, vl);
        reader.MoveToContent();
      }
      reader.ReadEndElement();

    }
    public XmlSchema GetSchema()
    {
      return null;
    }
  }
}
