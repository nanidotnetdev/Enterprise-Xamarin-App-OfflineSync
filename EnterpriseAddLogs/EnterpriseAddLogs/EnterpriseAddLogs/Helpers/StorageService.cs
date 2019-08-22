using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EnterpriseAddLogs.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.File;
using Xamarin.Forms;

namespace EnterpriseAddLogs.Helpers
{
    public static class StorageService
    {
        private static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };

        private static readonly CloudFileClient fileClient = cloudStorageAccount.CreateCloudFileClient();

        private static readonly CloudFileShare share = fileClient.GetShareReference("offlinesyncapp");
        private static readonly CloudBlobClient _blobClient = cloudStorageAccount.CreateCloudBlobClient();
        private static CloudBlobContainer blobContainer = _blobClient.GetContainerReference("offlinesyncapp");

        public static FileSource ReturnFileSource<T>(T blob) where T: ICloudBlob
        {
            if (ImageExtensions
                .Contains(Path.GetExtension(blob.Uri.ToString()).ToUpperInvariant()))
            {
                var file = new FileSource
                {
                    Text = blob.Name,
                    Image = ImageSource.FromUri(blob.Uri)
                };

                return file;
            }
            else
            {
                var file = new FileSource
                {
                    Text = blob.Name,
                    Image = ImageSource.FromResource("EnterpriseAddLogs.Images.VideoIcon.png")
                };

                return file;
            }
        }

        public static async Task<ObservableRangeCollection<FileSource>> GetBlobs<T>(string entityItemId, string prefix = "", 
            int? maxresultsPerQuery = null, BlobListingDetails blobListingDetails = BlobListingDetails.None) 
            where T : ICloudBlob
        {
            var blobList = new ObservableRangeCollection<FileSource>();
            BlobContinuationToken continuationToken = null;
            
            try
            {
                do
                {
                    var response = await blobContainer.ListBlobsSegmentedAsync(entityItemId, true, blobListingDetails, maxresultsPerQuery, continuationToken, null, null);

                    continuationToken = response?.ContinuationToken;

                    foreach (var blob in response?.Results?.OfType<T>())
                    {
                        blobList.Add(ReturnFileSource(blob));
                    }

                } while (continuationToken != null);
            }
            catch (Exception e)
            {
                //Handle Exception
            }

            return blobList;
        }

        public static async Task UploadFiles(IEnumerable<FileSource> files, string entityItemId)
        {
            foreach (var fileSource in files)
            {
                await UploadFile(fileSource, entityItemId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="entityItemId"></param>
        /// <returns></returns>
        public static async Task<FileSource> UploadFile(FileSource item, string entityItemId)
        {
            //to blob
            //await SaveBlockBlob(entityItemId, "offlinesyncapp", MemoryStream(item.FilePath), $"{item.Text}{fileExtension}");
            return await SaveBlockBlob(entityItemId, item);

            //to file storage
            //CloudFileDirectory rootDir = share.GetRootDirectoryReference();

            //var dir = rootDir.GetDirectoryReference(entityItemId.ToString());
            //await dir.CreateIfNotExistsAsync();
            //CloudFile file = dir.GetFileReference($"{item.Text}{fileExtension}");
            //await file.UploadFromFileAsync(item.FilePath);
        }

        public static async Task<FileSource> SaveBlockBlob(string entityItemId, FileSource file)
        {
            if (!string.IsNullOrWhiteSpace(file.FilePath ?? ""))
            {
                await blobContainer.CreateIfNotExistsAsync();

                string fileExtension = Path.GetExtension(file.FilePath);
                var blobName = $"{entityItemId.ToString()}/{file.Text}{fileExtension}";

                var blockBlob = blobContainer.GetBlockBlobReference(blobName);

                await blockBlob.UploadFromFileAsync(file.FilePath);
                //await blockBlob.UploadFromByteArrayAsync(MemoryStream(file.FilePath), 0, blob.Length);

                File.Delete(file.FilePath);

                return ReturnFileSource(blockBlob);
            }

            return null;
        }

        public static byte[] MemoryStream(string path)
        {
            using (var streamReader = new StreamReader(path))
            {
                var bytes = default(byte[]);
                using (var memstream = new MemoryStream())
                {
                    streamReader.BaseStream.CopyTo(memstream);
                    bytes = memstream.ToArray();
                }

                return bytes;
            }
        }

        public static async Task<List<byte[]>> GetFiles(Guid entityItemId)
        {
            CloudFileDirectory rootDir = share.GetRootDirectoryReference();
            var dir = rootDir.GetDirectoryReference(entityItemId.ToString());

            var fileList = new List<byte[]>();

            FileContinuationToken continuationToken = null;

            try
            {
                if (await dir.ExistsAsync())
                {
                    do
                    {
                        var response = await dir.ListFilesAndDirectoriesSegmentedAsync(continuationToken);

                        continuationToken = response?.ContinuationToken;

                        foreach (var file in response?.Results.OfType<IListFileItem>())
                        {
                            //fileList.Add(file);
                        }

                    } while (continuationToken != null);
                }
                
            }
            catch (Exception e)
            {
                //Handle Exception
            }

            return fileList;
        }

        public static async Task<bool> DeleteFile(string fileName)
        {
            var blob = blobContainer.GetBlobReference(fileName);
            return await blob.DeleteIfExistsAsync();

            //CloudFileDirectory rootDir = share.GetRootDirectoryReference();

            //var dir = rootDir.GetDirectoryReference(fileName);
            //await dir.CreateIfNotExistsAsync();

            //CloudFile file = dir.GetFileReference(fileName);
            //return await file.DeleteIfExistsAsync();
        }
    }
}
