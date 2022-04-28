using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using File = Google.Apis.Drive.v3.Data.File;

namespace GoogleDriveUploadDemo
{
    class Program
    {
        private const string PathToServiceAccountKeyFile = @"F:\BackupDB\dev\backupdatabase-346207-d5ef98f19806.json";

        private const string ServiceAccountEmail =
            "backup-database-service-accoun@backupdatabase-346207.iam.gserviceaccount.com";

        private const string UploadFileName = @"F:\BackupDB\Test.txt";
        private const string DirectoryId = "1xRoscJOpwlU24bdX3SUuyib2pB7mTgQN";

        static async Task Main(string[] args)
        {
            // Load the Service account credentials and define the scope of its access.
            var credential = GoogleCredential.FromFile(PathToServiceAccountKeyFile)
                .CreateScoped(DriveService.ScopeConstants.Drive);

            // Create the  Drive service.
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            // Upload file Metadata
            var fileMetadata = new File()
            {
                Name = "Test.txt",
                Parents = new List<string>() {"1xRoscJOpwlU24bdX3SUuyib2pB7mTgQN"}
            };

            string uploadedFileId;
            // Create a new file on Google Drive
            await using (var fsSource = new FileStream(UploadFileName, FileMode.Open, FileAccess.Read))
            {
                // Create a new file, with metadata and stream.
                var request = service.Files.Create(fileMetadata, fsSource, "text/plain");
                request.Fields = "*";
                var results = await request.UploadAsync(CancellationToken.None);

                if (results.Status == UploadStatus.Failed)
                {
                    Console.WriteLine($"Error uploading file: {results.Exception.Message}");
                }

                // the file id of the new file we created
                uploadedFileId = request.ResponseBody?.Id;
            }

            // Find the find was just uploaded.
            var uploadedFile = await service.Files.Get(uploadedFileId).ExecuteAsync();
            Console.Write(
                $"{uploadedFile.Id} {uploadedFile.Name} {uploadedFile.MimeType} {uploadedFile.Parents?.FirstOrDefault()} ");
            
            // // Let's change the files name.
            // // Note: not all fields are writeable watch out, you cant just send uploadedFile back.
            // var updateFileBody = new File()
            // {
            //     Name = "update.txt"
            // };
            //
            // // Let's add some text to our file.
            // await System.IO.File.WriteAllTextAsync(UploadFileName, "Text changed in file.");
            //
            // // Then upload the file again with a new name and new data.
            // await using (var uploadStream = new FileStream(UploadFileName, FileMode.Open, FileAccess.Read))
            // {
            //     // Update the file id, with new metadata and stream.
            //     var updateRequest = service.Files.Update(updateFileBody, uploadedFile.Id, uploadStream, "text/plain");
            //     var result = await updateRequest.UploadAsync(CancellationToken.None);
            //
            //     if (result.Status == UploadStatus.Failed)
            //     {
            //         Console.WriteLine($"Error uploading file: {result.Exception.Message}");
            //     }
            // }
        }
    }
}