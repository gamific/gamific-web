using Amazon.S3;
using Amazon.S3.Model;
using Vlast.Util.Instrumentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vlast.Util.Aws
{
    /// <summary>
    /// Helper para gerenciamento de arquivos no S3
    /// </summary>
    public class S3Helper
    {

        ///<sumary>
        /// Gets a Amazon S3 client to CRUD operations on buckets and blobs
        ///
        /// @return
        ///</sumary>
        public static IAmazonS3 S3Client
        {
            get
            {
                return AWSFactory.S3Client;
            }
        }

        ///<sumary>
        /// Returns a StreamingOutput Array representation of S3 object
        ///
        /// @param bucketName
        /// @param objectKey
        /// @return
        ///</sumary>
        public static Stream GetS3ObjectStream(string bucketName, string objectKey)
        {
            try
            {
                IAmazonS3 s3Client = S3Helper.S3Client;
                GetObjectResponse blobObject = s3Client.GetObject(bucketName, objectKey);

                Stream objectContent = blobObject.ResponseStream;
                return objectContent;
            }
            catch
            {
                return null;
            }
        }

        ///<sumary>
        /// Returns a byte Array representation of S3 object
        ///
        /// @param bucketName
        /// @param objectKey
        /// @return
        ///</sumary>
        public static byte[] GetS3ObjectBytes(string bucketName, string objectKey)
        {
            Stream data = GetS3ObjectStream(bucketName, objectKey);
            if (data != null)
                return ToByteArray(data);
            else return null;
        }


        ///<sumary>
        /// Converts the inputStream on a byte[] representation
        ///
        /// @param objectContent
        /// @return
        ///</sumary>
        public static byte[] ToByteArray(Stream objectContent)
        {
            try
            {
                using (var streamReader = new MemoryStream())
                {
                    objectContent.CopyTo(streamReader);
                    return streamReader.ToArray();
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }


        ///<sumary>
        /// Upload file in S3
        ///
        /// @param bucketName
        /// @param objectKey
        /// @param file
        ///</sumary>
        public static bool PutObjectToS3(string bucketName, string objectKey, FileStream file)
        {
            bool result = true;
            try
            {
                IAmazonS3 s3Client = S3Helper.S3Client;
                PutObjectRequest req = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    InputStream = file
                };

                s3Client.PutObject(req);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                result = false;
            }

            return result;
        }

        ///<sumary>
        /// Upload a inputStream. Not saves the file in memory, sends directly to S3,
        /// but only if the size is empty.
        ///
        /// @param bucketName
        /// @param objectKey
        /// @param size
        /// @param contentType
        /// @param inputStream
        /// @return true if successful saved object; false otherwise
        ///</sumary>
        public static bool PutObjectToS3(string bucketName, string objectKey, string contentType, MemoryStream inputStream)
        {
            bool result = true;
            try
            {
                IAmazonS3 s3Client = S3Helper.S3Client;

                PutObjectRequest req = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    InputStream = inputStream,
                    ContentType = contentType
                };

                s3Client.PutObject(req);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                result = false;
            }
            return result;
        }


        ///<sumary>
        /// Upload a inputStream. Not saves the file in memory, sends directly to S3,
        /// but only if the size is empty.
        ///
        /// @param bucketName
        /// @param objectKey
        /// @param size
        /// @param contentType
        /// @param inputStream
        /// @return true if successful saved object; false otherwise
        ///</sumary>
        public static bool PutObjectToS3(string bucketName, string objectKey, string contentType, byte[] data)
        {
            using (MemoryStream st = new MemoryStream(data))
            {
                return PutObjectToS3(bucketName, objectKey, contentType, st);
            }
        }

        ///<sumary>
        /// Deletes current file and uplods a new
        ///
        /// @param bucketName
        /// @param objectKey
        /// @param contentType
        /// @param fileToReplace
        /// @return
        ///</sumary>
        public static bool ReplaceS3Object(string bucketName, string objectKey, string contentType, byte[] fileToReplace)
        {
            bool result = true;
            if (fileToReplace != null)
            {
                IAmazonS3 s3Client = S3Helper.S3Client;

                var current = GetS3ObjectStream(bucketName, objectKey);
                if (current != null)
                {
                    DeleteObjectRequest delReq = new DeleteObjectRequest()
                    {
                        Key = objectKey,
                        BucketName = bucketName
                    };

                    s3Client.DeleteObject(bucketName, objectKey);
                }

                using (MemoryStream ms = new MemoryStream(fileToReplace))
                {
                    PutObjectToS3(bucketName, objectKey, contentType, ms);
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

        /* método usado para testes, comentado para deixar como referência
        public static void ListS3Objects(string bucketName)
        {
            try
            {
                ListObjectsRequest request = new ListObjectsRequest
                {
                    BucketName = bucketName,
                    Prefix = "news/media/images",
                    MaxKeys = 2
                };
                do
                {
                    ListObjectsResponse response = S3Client.ListObjects(request);

                    // Process response.
                    foreach (S3Object entry in response.S3Objects)
                    {
                        Console.WriteLine("key = {0} size = {1}",
                            entry.Key, entry.Size);
                    }

                    // If response is truncated, set the marker to get the next 
                    // set of keys.
                    if (response.IsTruncated)
                    {
                        request.Marker = response.NextMarker;
                    }
                    else
                    {
                        request = null;
                    }
                } while (request != null);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId")
                    ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Check the provided AWS Credentials.");
                    Console.WriteLine(
                    "To sign up for service, go to http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine(
                     "Error occurred. Message:'{0}' when listing objects",
                     amazonS3Exception.Message);
                }
            }
        }
         */
    }
}
