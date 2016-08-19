using System;
using System.Net;
using System.Windows;
using static System.String;

namespace WubiQuery
{

    

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        private readonly string _tempDirectory = Environment.GetEnvironmentVariable("TEMP") + "\\WubiQueryTemp";

        public MainWindow()
        {

            InitializeComponent();
            // 尚未查询时，loading控件不可见
            Loading.IsActive = false;

            // loading bar的速度
            Loading.SpeedRatio = 2.0;

            // 文本框获取焦点
            TextBox.Focus();

        }

        private async void Query(object sender, RoutedEventArgs e)
        {
            //var btn = new Button
            //{
            //    Height = 30,
            //    Style = TryFindResource("SimpleButton") as Style,
            //    Margin = new Thickness(20, 2, 20, 0)
            //};
            //btn.Name = "newButton";//这里设置的Name是找不到的  

            // 复用按钮样式


            // 设置边距
            try
            {
                // 加载loading动画
                Loading.IsActive = true;

                // 隐藏结果表格
                Result.Visibility = Visibility.Hidden;

                // 建立异步网络请求
                var request = new AsyncNetRequest();

                var content = await request.GetHtmlContent(TextBox.Text);


                // 解析html
                var htmlDecoder = new HtmlDecoder(content);

                // 传入结构体
                var wubi = htmlDecoder.Decoded();

                // 图片获取并缓存到目录
                

                var imageName = wubi.StrokesPicturePath.Substring(11);

                

                System.IO.Directory.CreateDirectory(_tempDirectory);


                var imagePath = _tempDirectory + "\\" + imageName;

                if (System.IO.File.Exists(imagePath))
                {
                    Picture.Source = new Uri(imagePath);
                }
                else
                {
                    var picDown = new WebClient();
                    picDown.DownloadFileAsync(new Uri("http://www.52wubi.com" + wubi.StrokesPicturePath), imagePath);
                    picDown.DownloadFileCompleted += delegate
                    {
                        Picture.Source = new Uri(imagePath);
                    };
                }

                

                Loading.IsActive = false;
                WubiCoding.Text = wubi.WubiCoding;
                Pinyin.Text = wubi.Pinyin;
                Character.Text = wubi.Character;

                Result.Visibility = Visibility.Visible;
                Console.Write(content);
                Loading.IsActive = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                Loading.IsActive = false;
                //Console.Write(Properties.Resources.ErrorInput);
            }
            finally
            {
                TextBox.Text = Empty;
                TextBox.Focus();
            }

        }

        /// <summary>
        /// 退出程序后删除缓存目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClose(object sender, EventArgs e)
        {
            try
            {
                System.IO.Directory.Delete(_tempDirectory, true);
            }
            catch (System.IO.IOException)
            {
                MessageBox.Show("缓存的文件被占用，请手动删除！");
            }
            
        }
    }

}
