using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Amazon.S3;
using Amazon.Runtime;
namespace AwsBucket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BucketController : ControllerBase
    {

        private readonly IAmazonS3 _s3Client;
        public BucketController()
        {
            var credentials = new BasicAWSCredentials("AKIASKPLPZFJBLIYM3PY", "SbSK/rSWEm5uyE2k8g9dANbpP1UbDhSwSFW4VY7b");
            _s3Client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.APSouth1);
        }


       

        [HttpPost("create")]

        public async Task<IActionResult> CreateBucketAsync(string bucketName)
        {
            var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
            if (bucketExists) return BadRequest($"Bucket {bucketName} already exists.");
            await _s3Client.PutBucketAsync(bucketName);
            return Ok($"Bucket {bucketName} created.");
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBucketAsync()
        {
            var data = await _s3Client.ListBucketsAsync();
            var buckets = data.Buckets.Select(b => { return b.BucketName; });
            return Ok(buckets);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBucketAsync(string bucketName)
        {
            await _s3Client.DeleteBucketAsync(bucketName);
            return NoContent();
        }
    
    }



}

