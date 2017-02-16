using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using Vlast.Util.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SimpleEmail;

namespace Vlast.Util.Aws
{
    /// <summary>
    /// Factory de conexões para API Amazon AWS
    /// </summary>
    public class AWSFactory
    {
        private static volatile AWSCredentials awsCredentials;

        /**
         * Get a Aws credentials
         *
         * @return
         */
        public static AWSCredentials AWSCredentials
        {
            get
            {

                if (awsCredentials == null)
                {
                    String AWS_ACCESS_KEY_ID = ParameterCache.Get("AWS_ACCESS_KEY_ID");
                    String AWS_SECRET_KEY = ParameterCache.Get("AWS_SECRET_KEY");
                    awsCredentials = new BasicAWSCredentials(AWS_ACCESS_KEY_ID, AWS_SECRET_KEY);
                }
                return awsCredentials;
            }
        }

        /**
         * Gets a Amazon S3 client to get buckets and blobs
         *
         * @return
         */
        public static IAmazonS3 S3Client
        {
            get
            {
                IAmazonS3 s3client = AWSClientFactory.CreateAmazonS3Client(AWSCredentials, RegionEndpoint.USEast1 );
                    //new AmazonS3Client(AWSCredentials, RegionEndpoint.USEast1);
                return s3client;
            }
        }

        /**
         * Gets a DynamoDB client to noSQl tables tasks
         *
         * @return
         */
        public static  AmazonDynamoDBClient DynamoDBClient
        {
            get
            {
                AmazonDynamoDBClient dynamoCLient = new AmazonDynamoDBClient(AWSCredentials);
                return dynamoCLient;
            }
        }

        /**
         * Gets a SQS queue tasks
         *
         * @return
         */
        public static  AmazonSQSClient SQSClient
        {
            get
            {
                AmazonSQSClient client = new AmazonSQSClient(AWSCredentials);
                return client;
            }
        }

        public static  AmazonSimpleNotificationServiceClient SNSClient
        {
            get
            {
                AmazonSimpleNotificationServiceClient client = new AmazonSimpleNotificationServiceClient(AWSCredentials, RegionEndpoint.USEast1);
                return client;
            }
        }


        public static  AmazonSimpleEmailServiceClient SESClient
        {
            get
            {
                AmazonSimpleEmailServiceClient client = new AmazonSimpleEmailServiceClient(AWSCredentials, RegionEndpoint.USEast1);
                return client;
            }
        }
    }
}
