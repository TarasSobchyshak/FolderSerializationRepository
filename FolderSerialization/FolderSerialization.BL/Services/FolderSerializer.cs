using FolderSerialization.BL.Models;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace FolderSerialization.BL.Services
{
    public class FolderSerializer
    {
        private BinaryFormatter _binaryFormatter;
        public FolderSerializer()
        {
            _binaryFormatter = new BinaryFormatter();
        }
        public static string FileFormat => "xax";
        public async Task SerializeAsync(Folder folder, string path)
        {
            await Task.Run(() => Serialize(folder, path));
        }
        public void Serialize(Folder folder, string path)
        {
            using (var stream = new FileStream(Path.Combine(path, folder.Name + "." + FileFormat), FileMode.OpenOrCreate))
            {
                _binaryFormatter.Serialize(stream, folder);
            }
        }
        public async Task DeserializeAsync(string filePath, string deserializedFolderPath)
        {
            await Task.Run(() => Deserialize(filePath, deserializedFolderPath));
        }
        public void Deserialize(string filePath, string deserializedFolderPath)
        {
            if (!System.IO.File.Exists(filePath))
                throw new FileNotFoundException($"File {filePath} doesn't exist.");

            Folder folder;

            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                folder = (Folder)_binaryFormatter.Deserialize(stream);
            }

            DeserializeFolderTo(folder, deserializedFolderPath);
        }
        private void DeserializeFolderTo(Folder folder, string deserializedFolderPath)
        {
            Directory.CreateDirectory(deserializedFolderPath);
            foreach (var file in folder.Files)
            {
                System.IO.File.WriteAllBytes(Path.Combine(deserializedFolderPath, file.Name), file.Content);
            }
            foreach (var subFolder in folder.SubFolders)
            {
                DeserializeFolderTo(subFolder, Path.Combine(deserializedFolderPath, subFolder.Name));
            }
        }
    }
}
