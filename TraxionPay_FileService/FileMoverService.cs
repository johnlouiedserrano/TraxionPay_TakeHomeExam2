using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using TraxionPay_FileService.Service;

namespace TraxionPay_FileService
{
    public partial class FileMoverService : ServiceBase
    {
        private readonly FileMover _fileMover;
        private readonly Logger _logger;

        public FileMoverService()
        {
            InitializeComponent();
            _logger = new Logger("TraxionPay_FileService"); 
            _fileMover = new FileMover(_logger); 
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                _logger.LogInformation("Starting off the service...");
                _fileMover.StartMonitoring(@"C:\Folder1", @"C:\Folder2");
                _logger.LogInformation("Checking Folder1 for new files.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                Stop(); // Shut down if we hit a snag
            }
        }

        protected override void OnStop()
        {
            try
            {
                _logger.LogInformation("Shutting down the service...");
                _fileMover.StopMonitoring();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while stopping the service: {ex.Message}");
            }
        }
    }
}
