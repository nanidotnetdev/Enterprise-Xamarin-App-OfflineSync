using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        static readonly CloudStorageAccount cloudStorageAccount =
            CloudStorageAccount.Parse("Connection strong");

        private static readonly CloudFileClient fileClient = cloudStorageAccount.CreateCloudFileClient();

        private static readonly CloudFileShare share = fileClient.GetShareReference("offlinesyncapp");
        readonly static CloudBlobClient _blobClient = cloudStorageAccount.CreateCloudBlobClient();

        public static async Task<ObservableRangeCollection<FileSource>> GetBlobs<T>(Guid entityItemId, string containerName, string prefix = "", 
            int? maxresultsPerQuery = null, BlobListingDetails blobListingDetails = BlobListingDetails.None) where T : ICloudBlob
        {
            var blobContainer = _blobClient.GetContainerReference(containerName);

            var blobList = new ObservableRangeCollection<FileSource>();
            BlobContinuationToken continuationToken = null;
            
            try
            {
                do
                {
                    var response = await blobContainer.ListBlobsSegmentedAsync(entityItemId.ToString(), true, blobListingDetails, maxresultsPerQuery, continuationToken, null, null);

                    continuationToken = response?.ContinuationToken;

                    foreach (var blob in response?.Results?.OfType<T>())
                    {
                        var file = new FileSource
                        {
                            Text = blob.Name,
                            Image = ImageSource.FromUri(blob.Uri)
                        };
                        
                        blobList.Add(file);
                    }

                } while (continuationToken != null);
            }
            catch (Exception e)
            {
                //Handle Exception
            }

            return blobList;
        }

        public static async Task UploadFiles(IEnumerable<FileSource> files, Guid entityItemId)
        {
            foreach (var fileSource in files)
            {
                await UploadFile(fileSource, entityItemId);
            }
        }

        public static async Task UploadFile(FileSource item, Guid entityItemId)
        {
            //to blob
            //await SaveBlockBlob(entityItemId, "offlinesyncapp", MemoryStream(item.FilePath), $"{item.Text}{fileExtension}");
            await SaveBlockBlob(entityItemId, "offlinesyncapp", item);

            //to file storage
            //CloudFileDirectory rootDir = share.GetRootDirectoryReference();

            //var dir = rootDir.GetDirectoryReference(entityItemId.ToString());
            //await dir.CreateIfNotExistsAsync();
            //CloudFile file = dir.GetFileReference($"{item.Text}{fileExtension}");
            //await file.UploadFromFileAsync(item.FilePath);

            File.Delete(item.FilePath);
        }

        public static async Task<CloudBlockBlob> SaveBlockBlob(Guid entityItemId, string containerName, FileSource file)
        {
            if (!string.IsNullOrWhiteSpace(file.FilePath ?? ""))
            {
                var blobContainer = _blobClient.GetContainerReference(containerName);
                await blobContainer.CreateIfNotExistsAsync();

                string fileExtension = Path.GetExtension(file.FilePath);
                var blobName = $"{entityItemId.ToString()}/{file.Text}{fileExtension}";

                var blockBlob = blobContainer.GetBlockBlobReference(blobName);

                await blockBlob.UploadFromFileAsync(file.FilePath);
                //await blockBlob.UploadFromByteArrayAsync(MemoryStream(file.FilePath), 0, blob.Length);

                File.Delete(file.FilePath);

                return blockBlob;
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

        public static async Task<bool> DeleteFile(string fileName, Guid entityItemId)
        {

            CloudFileDirectory rootDir = share.GetRootDirectoryReference();


            var dir = rootDir.GetDirectoryReference(fileName);
            await dir.CreateIfNotExistsAsync();

            CloudFile file = dir.GetFileReference(fileName);
            return await file.DeleteIfExistsAsync();
        }
    }
}
