using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WubiQuery
{
   

    public class AsyncNetRequest
    {
        /// <summary>
        /// 该方法通过异步执行可等待的网络请求获取html内容
        /// </summary>
        /// <param name="queryContent">需要查询的单个汉字</param>
        /// <returns>返回值为html</returns>
        /// <returns>返回值为html</returns>
        public async Task<string> GetHtmlContent(string queryContent)
        {
            // 实例化一个http委托
            var client = new HttpClient();

            // 内容需要用gb2312编码，才能发送正确的内容
            var url = "http://www.52wubi.com/wbsoft/index.php?hzname=" + HttpUtility.UrlEncode(queryContent, Encoding.GetEncoding("gb2312"));

            // 使用GetByteArrayAsync可以返回一串byte
            // 若使用GetStringAsync会引发由gb2312编码导致的乱码
            var getByteTask = client.GetByteArrayAsync(url);

            // 获取到返回字节数组后，再转为字符串
            var urlContents = Encoding.Default.GetString(await getByteTask);

            // 最终返回请求到的内容
            return urlContents;
        }
       
    }
}
