﻿using FilesSizeApp.Models;
using FilesSizeApp.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FilesSizeApp.Tests
{
    public class SizeServiceTests
    {
        private string _tempFolderPath = Path.GetTempPath();
        private string _tempFilePath = Path.GetTempFileName();
        private string _printedData;
        private void MakeTempFile()
        {
            FileInfo file = new FileInfo(_tempFilePath);

            using (StreamWriter sw = file.CreateText())
            {
                sw.WriteLine("Temp test file");
            }
        }

        [Fact]
        public void CreationTest()
        {
            SizeService service = new SizeService(new FolderDetails { Path = _tempFolderPath });
            Assert.NotNull(service);
        }
        [Fact]
        public async Task FileSizeCreationTest()
        {
            MakeTempFile();
            SizeService service = new SizeService(new FolderDetails { Path = _tempFolderPath });
            var size = await service.FileSize(_tempFilePath);
            Assert.True(size != 0);
        }
        [Fact]
        public async Task FileSizeAccuracyTest()
        {
            MakeTempFile();
            SizeService service = new SizeService(new FolderDetails { Path = _tempFolderPath });
            var size = await service.FileSize(_tempFilePath);
            FileInfo file = new FileInfo(_tempFilePath);

            Assert.Equal(file.Length, size);
        }
        private async Task printToString(Task<long> number, string path)
        {
            _printedData += await number + " ";
        }
        [Fact]
        public async Task FileSizePrintAccuracyTest()
        {
            _printedData = string.Empty;
            MakeTempFile();
            SizeService service = new SizeService(new FolderDetails { Path = _tempFolderPath });
            await service.FileSizePrint(printToString, _tempFilePath);
            FileInfo file = new FileInfo(_tempFilePath);
            Assert.Equal(file.Length.ToString(), _printedData.Trim());
        }
    }
}
