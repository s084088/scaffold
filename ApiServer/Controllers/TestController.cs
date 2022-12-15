using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Cache;
using Newtonsoft.Json.Linq;
using Util.Model;

namespace ApiServer.Controllers
{
    [Route("Api/Test/Test")]
    [ApiExplorerSettings(GroupName = "Test")]
    [CustomAuthorize(EnumCustomAuthorize.None)]
    public class TestController : BaseController
    {
        /// <summary>
        /// 测试接口-Get
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTest")]
        public WebApiPackage<TestModel> GetTest() => Data(new TestModel());

        /// <summary>
        /// 测试接口-Head
        /// </summary>
        /// <returns></returns>
        [HttpHead]
        [Route("HeadTest")]
        public WebApiPackage<TestModel> HeadTest() => Data(new TestModel());

        /// <summary>
        /// 测试接口-Options
        /// </summary>
        /// <returns></returns>
        [HttpOptions]
        [Route("OptionsTest")]
        public WebApiPackage<TestModel> OptionsTest() => Data(new TestModel());

        /// <summary>
        /// 测试接口-Patch
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        [Route("PatchTest")]
        public WebApiPackage<TestModel> PatchTest() => Data(new TestModel());

        /// <summary>
        /// 测试接口-Post
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("PostTest")]
        public WebApiPackage<JObject> PostTest([FromBody] JObject json) => Data(json);

        /// <summary>
        /// 测试接口-Put
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("PutTest")]
        public WebApiPackage<TestModel> PutTest() => Data(new TestModel());

        /// <summary>
        /// 测试接口-Delete
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteTest")]
        public WebApiPackage<TestModel> DeleteTest() => Data(new TestModel());

        /// <summary>
        /// 测试接口-错误
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("ErrorTest")]
        public WebApiPackage<TestModel> ErrorTest() => throw new ApiException("错误测试");

        /// <summary>
        /// 测试接口-数据库连接
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("DB")]
        public WebApiPackage<string> DB()
        {
            try
            {
                MyDB dB = new MyDB();
                return Data("数据库连接OK!");
            }
            catch
            {
                return Data("数据库连接失败");
            }
        }

        /// <summary>
        /// 测试接口-清理内存垃圾
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Clear")]
        public WebApiPackage<JObject> Clear()
        {
            JObject json = new JObject();
            long before = Process.GetCurrentProcess().WorkingSet64;
            json.Add("清理前", before.ToTrafficString());
            GC.Collect();
            long after = Process.GetCurrentProcess().WorkingSet64;
            json.Add("清理后", after.ToTrafficString());
            json.Add("清理量", (before - after).ToTrafficString());
            return Data(json);
        }

        /// <summary>
        /// 测试接口-环境测试
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Environments")]
        public WebApiPackage Environments()
        {
            var ret = new
            {
                系统名称 = RuntimeInformation.OSDescription,
                机器名称 = Environment.MachineName,
                版本号 = Environment.OSVersion,
                进程架构 = RuntimeInformation.ProcessArchitecture,
                程序环境 = Res.IsTest ? "测试环境" : "正式环境",
                CPU核心数 = Environment.ProcessorCount,
                进程信息 = Process.GetProcesses().Select(l => new
                {
                    进程名称 = l.ProcessName,
                    占用内存 = l.WorkingSet64,
                    优先级 = l.BasePriority,
                }),
            };
            return OK(ret);
        }

        /// <summary>
        /// 测试接口-权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Auth1")]
        public WebApiPackage<JObject> Auth1()
        {
            JObject ob = new JObject
            {
                { "连接状态", "连接OK" },
                { "空值", null },
                { "字符串", "测试接口" },
                { "数值", -123.5 },
                { "日期", DateTime.Now },
                { "布尔值", true }
            };
            return Data(ob);
        }

        /// <summary>
        /// 获取API请求次数
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("GetApiCount")]
        public IActionResult GetApiCount()
        {
            var ret = ApiStatistics.AllApi.Select(l => new
            {
                l.Api,
                l.SuccessCount,
                l.TotalTime,
                l.ErrorCount,
                l.FatalErrorCount,
                l.TokenFailCount,
                l.InterceptionCount
            }).OrderByDescending(l => l.SuccessCount).ToList();
            List<ApiUrlModel> apiurls = new ApiUrlHelper().GetApiUrls();
            var req = ret.Select(l => new
            {
                l.Api,
                apiurls.FirstOrDefault(k => k.Url.ToLower() == l.Api)?.Name,
                AverageTime = l.SuccessCount == 0 ? TimeSpan.Zero : l.TotalTime / l.SuccessCount,
                l.SuccessCount,
                l.InterceptionCount,
                l.TokenFailCount,
                l.ErrorCount,
                l.FatalErrorCount,
            });
            string fileName = $"{Guid.NewGuid()}.xlsx";
            System.Data.DataTable dt = req.ToDataTable();
            MemoryStream ms = AsposeOfficeHelper.DataTableToExcel(dt);

            ms.Position = 0;
            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}