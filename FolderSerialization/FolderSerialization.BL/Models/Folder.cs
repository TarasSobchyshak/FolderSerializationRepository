using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FolderSerialization.BL.Models
{
    [Serializable]
    public class Folder
    {
        public string Name { get; private set; }
        public string FullPath { get; private set; }
        public List<File> Files { get; private set; }
        public List<Folder> SubFolders { get; private set; }

        private DirectoryInfo directoryInfo;

        public void LoadFilesAndDirectories(string folderPath)
        {
            if (!Directory.Exists(folderPath))
                throw new DirectoryNotFoundException($"Directory {folderPath} doesn't exist.");

            directoryInfo = new DirectoryInfo(folderPath);
            FullPath = folderPath;
            Name = directoryInfo.Name;

            Files = directoryInfo.GetFiles().Select(
                fi => new File
                {
                    Name = fi.Name,
                    FullName = fi.FullName,
                    Content = System.IO.File.ReadAllBytes(fi.FullName)
                }).ToList();

            SubFolders = directoryInfo.GetDirectories()
                                        .Select(di =>
                                        {
                                            Folder f = new Folder();
                                            f.LoadFilesAndDirectories(di.FullName);
                                            return f;
                                        }).ToList();
        }
    }
}
