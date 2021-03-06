// <copyright file="HtmlDecoderTest.cs">Copyright ©  2016</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WubiQuery;

namespace WubiQuery.Tests
{
    /// <summary>此类包含 HtmlDecoder 的参数化单元测试</summary>
    [PexClass(typeof(HtmlDecoder))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class HtmlDecoderTest
    {
        /// <summary>测试 .ctor(String) 的存根</summary>
        [PexMethod]
        internal HtmlDecoder ConstructorTest(string input)
        {
            HtmlDecoder target = new HtmlDecoder(input);
            return target;
            // TODO: 将断言添加到 方法 HtmlDecoderTest.ConstructorTest(String)
        }
    }
}
