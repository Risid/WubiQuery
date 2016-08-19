using System;
using System.Linq;
using HtmlAgilityPack;


namespace WubiQuery
{
    public class HtmlDecoder
    {
        private readonly WubiStruct _wubi = new WubiStruct();
        private readonly HtmlDocument _html;

        /// <summary>
        /// HtmlDecoder构造函数
        /// </summary>
        /// <param name="input">传入html内容</param>
        public HtmlDecoder(string input)
        {
            // 由输入的html字符串建立HtmlDocument用于解析
            var html = new HtmlDocument();
            html.LoadHtml(input);
            _html = html;

        }
        /// <summary>
        /// 解析html
        /// 并返回结构体
        /// </summary>
        /// <returns>五笔内容结构体</returns>
        public WubiStruct Decoded()
        {
            
            // 找到tr的全部节点
            var trs = _html.DocumentNode.SelectNodes("//tr").ToList();

            // 将tr的第一个td节点存入List中，放弃table的th（因为已经知道内容）
            var tds = trs.Select(tr => tr.SelectNodes("td")[0]).ToList();

            // 汉字在第二个td中，第一个是广告url。
            // 由于td中有一个空格，所以使用Trim函数去除空格。Sample： <td> 我</td>
            _wubi.Character = tds[1].InnerText.Trim(' ');

            _wubi.Pinyin = tds[2].InnerText;
            _wubi.WubiCoding = tds[3].InnerText;

            // 使用TakeOutTextBetween提取gif图片地址。Sample：<td><img src="/wbbmcx/tp/嗯.gif" alt="嗯的五笔拆分图" /></td>
            _wubi.StrokesPicturePath = TakeOutTextBetween("img src=\"", "\" alt", tds[4].InnerHtml);
            return _wubi;
        }





        /// <summary>
        /// 取出文本中间
        /// 若没有错误则返回文本，有错误返回错误信息的字符串
        /// </summary>
        /// <param name="leftstr">欲取出文本左边</param>
        /// <param name="rightstr">欲取出文本右边</param>
        /// <param name="str">原文本</param>
        /// <returns>返回取出的文本内容或错误内容</returns>
        public static string TakeOutTextBetween(string leftstr, string rightstr, string str )
        {
            try
            {
                var i = str.IndexOf(leftstr, StringComparison.Ordinal) + leftstr.Length;
                return str.Substring(i, str.IndexOf(rightstr, i, StringComparison.Ordinal) - i);
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            
            
        }


    }
}
