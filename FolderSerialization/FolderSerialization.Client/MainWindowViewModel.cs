using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF = System.Windows.Forms;
using FolderSerialization.BL.Models;
using FolderSerialization.BL.Services;
using System.Windows.Input;
using FolderSerialization.Client.Models;
using System.Collections.ObjectModel;

namespace FolderSerialization.Client
{
    internal class MainWindowViewModel : BaseViewModel
    {
        private Folder _folder;
        private FolderSerializer _folderSerializer;
        private WF.FolderBrowserDialog _folderBrowserDialog;
        private WF.OpenFileDialog _openFileDialog;
        private string _folderPath;
        private string _status;

        public MainWindowViewModel()
        {
            _folderPath = "";
            _status = "Waiting for path...";
            _folder = new Folder();
            _folderSerializer = new FolderSerializer();
            _folderBrowserDialog = new WF.FolderBrowserDialog();
            _openFileDialog = new WF.OpenFileDialog();
        }
        public string FolderPath
        {
            get { return _folderPath; }
            set { SetProperty(ref _folderPath, value); }
        }
        public string Status
        {
            get { return _status; }
            set { SetProperty(ref _status, value); }
        }
        public ObservableCollection<MenuItem> MenuItems =>
             new ObservableCollection<MenuItem>{
                new MenuItem { Text = "Select path", Command = SelectFolderPath },
                new MenuItem { Text = "Load",        Command = LoadFilesAndDirectories },
                new MenuItem { Text = "Serialize",   Command = SerializeFolder },
                new MenuItem { Text = "Deserialize", Command = DeserializeFolder }
            };

        private ICommand SelectFolderPath => new AutoCanExecuteCommandWrapper(
                                                 new DelegateCommand(obj => ShowFolderBrowserDialog()));

        private ICommand LoadFilesAndDirectories => new AutoCanExecuteCommandWrapper(
                                                        new DelegateCommand(async obj => await LoadFolder(),
                                                                                  obj => !string.IsNullOrEmpty(FolderPath)));

        private ICommand SerializeFolder => new AutoCanExecuteCommandWrapper(
                                                new DelegateCommand(async obj => await Serialize(),
                                                                          obj => !string.IsNullOrEmpty(_folder.Name)));

        private ICommand DeserializeFolder => new AutoCanExecuteCommandWrapper(
                                                  new DelegateCommand(async obj => await Deserialize()));

        private void ShowFolderBrowserDialog()
        {
            try
            {
                if (_folderBrowserDialog.ShowDialog() == WF.DialogResult.OK)
                {
                    FolderPath = _folderBrowserDialog.SelectedPath;
                    _folder = new Folder();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private async Task LoadFolder()
        {
            try
            {
                await _folder.LoadFilesAndDirectoriesAsync(FolderPath);
                FolderPath = "";
                Status = $"{_folder.Name} loaded";
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private async Task Serialize()
        {
            try
            {
                if (_folderBrowserDialog.ShowDialog() == WF.DialogResult.OK)
                {
                    await _folderSerializer.SerializeAsync(_folder, _folderBrowserDialog.SelectedPath);
                    Status = $"{_folder.Name} serialized";
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private async Task Deserialize()
        {
            try
            {
                _openFileDialog.Title = "Select binary file for deserialization";
                _openFileDialog.Filter = $"Binary files (*.{FolderSerializer.FileFormat})|*.{FolderSerializer.FileFormat}";
                string fileName = "";

                if (_openFileDialog.ShowDialog() == WF.DialogResult.OK)
                {
                    fileName = _openFileDialog.FileName;
                }
                if (!string.IsNullOrEmpty(fileName) && _folderBrowserDialog.ShowDialog() == WF.DialogResult.OK)
                {
                    await _folderSerializer.DeserializeAsync(fileName, _folderBrowserDialog.SelectedPath);
                    Status = $"{fileName}\ndeserialized to\n{_folderBrowserDialog.SelectedPath}";
                    _folder = new Folder();
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
