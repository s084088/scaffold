using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Auth.Sts;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using Aliyun.OSS;

namespace BasicLibrary.Comm
{
    public static class OssHelper
    {
        private const string region = "cn-shanghai";
        private const string endpoint = "oss-cn-shanghai.aliyuncs.com";
        private const string accessKeyId = "LTAI5tGttid1gvAuVRAEyRXu";
        private const string accessKeySecret = "leD40cvcrc2mVz6GirG22yvNQ8oTfo";
        private const double time = 60;
        private readonly static OssClient client = new(endpoint, accessKeyId, accessKeySecret);

        /// <summary>
        /// 获取OSS下载路径
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetUrl(string bucket, string file) => client.GeneratePresignedUri(new GeneratePresignedUriRequest(bucket, file, SignHttpMethod.Get) { Expiration = DateTime.Now.AddMinutes(time) }).AbsoluteUri;

        /// <summary>
        /// 创建OSS上传路径
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string PutUrl(string bucket, string file) => client.GeneratePresignedUri(new GeneratePresignedUriRequest(bucket, file, SignHttpMethod.Put) { Expiration = DateTime.Now.AddMinutes(time) }).AbsoluteUri;

        /// <summary>
        /// 获取OSSToken
        /// </summary>
        /// <returns></returns>
        public static object GetOssToken()
        {
            //const string ENDPOINT = "sts.cn-shanghai.aliyuncs.com";
            //构建一个阿里云client，用于发起请求
            //构建阿里云client时需要设置AccessKey ID和AccessKey Secret
            //DefaultProfile.AddEndpoint(REGIONID, REGIONID, "Sts", ENDPOINT);
            IClientProfile profile = DefaultProfile.GetProfile(region, accessKeyId, accessKeySecret);
            DefaultAcsClient client = new(profile);
            //构建AssumeRole请求
            AssumeRoleRequest request = new()
            {
                AcceptFormat = FormatType.JSON,
                //指定角色ARN
                RoleArn = "acs:ram::1242682884536834:role/ramosstest",
                RoleSessionName = GuidHelper.GenerateGuid(),
                //设置Token有效期，可选参数，默认3600秒
                DurationSeconds = 3600
            };
            //设置Token的附加权限策略；在获取Token时，通过额外设置一个权限策略进一步减小Token的权限
            //request.Policy="<policy-content>"

            try
            {
                AssumeRoleResponse response = client.GetAcsResponse(request);
                return new
                {
                    response.Credentials.AccessKeyId,
                    response.Credentials.AccessKeySecret,
                    response.Credentials.SecurityToken,
                    Expiration = DateTime.Parse(response.Credentials.Expiration).ToLocalTime(),
                };
            }
            catch (Exception ex)
            {
                throw new ApiException(ex.Message);
            }
        }
    }
}