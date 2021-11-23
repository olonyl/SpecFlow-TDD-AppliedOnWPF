using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace TrueOrFalse.Models
{
    public interface IPersistence {        
        int Count { get; }
        List<Statement> List { get; }
        string FileName { get; set; }
        void Add(Statement statement);
        void Remove(int index);
        Statement this[int index] { get; }
        void Save();
        void Load();
        void Change(int index, Statement statement);
        bool Exists(int index);
    }
   
    public class Persistence : IPersistence {
        public string FileName { get; set; }

        private List<Statement> _list = new List<Statement>();

        public Persistence(string fileName)
        {
            FileName = fileName;
        }
        
        public void Add(Statement statement)
        {
            _list.Add(statement);
        }

        public void Remove(int index)
        {
            _list.RemoveAt(index);
        }

        public Statement this[int index] => _list[index];

        public void Save()
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Statement>));
            Stream fStream = new FileStream(FileName, FileMode.Create, FileAccess.Write);
            xmlFormat.Serialize(fStream, _list);
            fStream.Close();
        }

        public void Load() {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(List<Statement>));
            Stream fStream = new FileStream(FileName, FileMode.Open, FileAccess.Read);
            _list = (List<Statement>)xmlFormat.Deserialize(fStream);
            fStream.Close();
        }        

        public int Count => _list.Count;

        public void Change(int index, Statement statement)
        {
            _list[index] = statement;
        }

        public bool Exists(int index)
        {
            return Count > index;
        }        

        public List<Statement> List => _list;
    }
}
