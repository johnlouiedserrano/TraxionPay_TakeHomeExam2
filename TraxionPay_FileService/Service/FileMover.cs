using System;
using System.IO;
using TraxionPay_FileService.Interface;

namespace TraxionPay_FileService.Service
{
    public class FileMover
    {
        private readonly FileSystemWatcher _watcher;
        private readonly ILogger _logger;
        private string _targetFolder;

        public FileMover(Logger logger)
        {
            _logger = logger;
            _watcher = new FileSystemWatcher();
            _watcher.Created += OnFileCreated; // Hook up the file creation event
            _watcher.Error += OnError; // Catch any watcher issues
        }

        public void StartMonitoring(string sourceFolder, string targetFolder)
        {
            if (!Directory.Exists(sourceFolder) || !Directory.Exists(targetFolder))
            {
                _logger.LogError("One of the folders is missing!");
                throw new DirectoryNotFoundException("One of the folders is missing!");
            }

            _watcher.Path = sourceFolder;
            _watcher.Filter = "*.*"; // Watch all file types
            _watcher.EnableRaisingEvents = true;
            _targetFolder = targetFolder;
        }

        public void StopMonitoring()
        {
            _watcher.EnableRaisingEvents = false;
        }

        private void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            try
            {
                string targetPath = Path.Combine(_targetFolder, e.Name);
                File.Move(e.FullPath, targetPath);
                _logger.LogInformation($"Moved {e.FullPath} to {targetPath}");
            }
            catch (IOException ex)
            {
                _logger.LogError($"Error: moving {e.FullPath}: {ex.Message}. ");
                try
                {
                    File.Move(e.FullPath, Path.Combine(_targetFolder, e.Name));
                }
                catch (Exception retryEx)
                {
                    _logger.LogError($"Retry  {e.FullPath}: {retryEx.Message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {e.FullPath}: {ex.Message}");
            }
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            _logger.LogError($"Error: {e.GetException().Message}");
        }
    }
}
