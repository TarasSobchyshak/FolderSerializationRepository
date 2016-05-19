using System.Windows.Input;

namespace FolderSerialization.Client.Models
{
    public class MenuItem
    {
        public string Text { get; set; }
        public ICommand Command { get; set; }
    }
}
