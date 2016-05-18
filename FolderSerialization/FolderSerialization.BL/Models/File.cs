using System;

namespace FolderSerialization.BL.Models
{
    [Serializable]
    public class File
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string FullName { get; set; }
    }
}
