using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using Amazon.S3.Model;
using FilesProj.Core.Entities;
using FilesProj.Core.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Image = Amazon.Rekognition.Model.Image;
using S3Object = Amazon.Rekognition.Model.S3Object;

namespace FilesProj.Service.Services
{
    public class AwsService : IAwsService
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonS3 _s3Client;
        private readonly AmazonRekognitionClient _rekognitionClient;

        private readonly string _bucketName;

        public AwsService(IConfiguration configuration)
        {
            _configuration = configuration;
            _bucketName = _configuration["AWS:BucketName"];
            
            var accessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            var secretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");

            var region = RegionEndpoint.USEast1;
            var rekAccessKey = Environment.GetEnvironmentVariable("ACCESS_KEY_ID");
            var rekSecretKey = Environment.GetEnvironmentVariable("SECRET_ACCESS_KEY");
            _s3Client = new AmazonS3Client(accessKey, secretKey, region);
            _rekognitionClient = new AmazonRekognitionClient(rekAccessKey, rekSecretKey, region);

        }

        public async Task<string> GetPreSignedUrlAsync(int userId, string path, string fileName, string contentType)
        {
            string url = "/";
            if (!string.IsNullOrEmpty(path))
                url = path + "/";

            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = userId + url + fileName,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(15),
                ContentType = contentType
            };

            return await Task.FromResult(_s3Client.GetPreSignedURL(request));
        }

        //cop
        

        // Download file from S3
        public async Task<string> GetDownloadUrlAsync(int userId, string path)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = userId + path,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(15),
            };

            return await Task.FromResult(_s3Client.GetPreSignedURL(request));
        }


        // Delete file from S3
        //public async Task DeleteFileAsync(string fileName, string userName)
        //{
        //    var deleteRequest = new DeleteObjectRequest
        //    {
        //        BucketName = "branding-files-proj",
        //        Key = _key + userName + "/" + fileName
        //    };

        //    var response = await _s3Client.DeleteObjectAsync(deleteRequest);
        //    if (response.HttpStatusCode != System.Net.HttpStatusCode.NoContent)
        //    {
        //        throw new Exception("Failed to delete file from S3.");
        //    }
        //}

        //List files in S3
        //public async Task<IEnumerable<string>> ListFilesAsync(string userName)
        //{
        //    var request = new ListObjectsV2Request
        //    {
        //        BucketName = "branding-files-proj",
        //        Prefix = _key + userName + "/"
        //    };

        //    var response = await _s3Client.ListObjectsV2Async(request);
        //    if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
        //    {
        //        throw new Exception("Failed to list files in S3.");
        //    }

        //    return response.S3Objects.Select(o => o.Key);
        //}


        //Frames------------------------
        public async Task<string> GetPreSignedUrlAsync(string fileName, string contentType)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = "frames/" + fileName,
                Verb = HttpVerb.PUT,
                Expires = DateTime.UtcNow.AddMinutes(15),
                ContentType = contentType
            };

            return await Task.FromResult(_s3Client.GetPreSignedURL(request));
        }

        public async Task<string> GetDownloadUrlAsync(string fileName)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = "frames/" + fileName,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(15),
            };

            return await Task.FromResult(_s3Client.GetPreSignedURL(request));
        }

        public async Task<List<string>> DetectLabelsAsync(string path, int maxLabels = 10, float minConfidence = 75)
        {
            var request = new DetectLabelsRequest
            {
                Image = new Image
                {
                    S3Object = new S3Object
                    {
                        Bucket = _bucketName,
                        Name = path
                    }
                },
                MaxLabels = maxLabels,
                MinConfidence = minConfidence
            };

            try
            {
                var response = await _rekognitionClient.DetectLabelsAsync(request);

                // Extract label names
                var labels = response.Labels.Select(label => label.Name).ToList();
                return labels;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., logging)
                throw new Exception($"Error detecting labels: {ex.Message}", ex);
            }
        }



    }



}
