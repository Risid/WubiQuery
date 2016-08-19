using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WubiQuery
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        public volatile bool ShoudlStop;
        public MainWindow()
        {
           
            InitializeComponent();
            // 尚未查询时，loading控件不可见
            Loading.IsActive = false;

            // loading bar的速度
            Loading.SpeedRatio = 2.0;

        }

        private async void Query(object sender, RoutedEventArgs e)
        {
            var btn = new Button
            {
                Height = 30,
                Style = TryFindResource("SimpleButton") as Style,
                Margin = new Thickness(20, 2, 20, 0)
            };
            //btn.Name = "newButton";//这里设置的Name是找不到的  

            // 复用按钮样式


            // 设置边距
            Loading.IsActive = true;
            Result.Visibility = Visibility.Hidden;
            try
            {
                var request = new AsyncNetRequest();

                var content = await request.GetHtmlContent(TextBox.Text);
                
                var htmlDecoder = new HtmlDecoder(content);

                var wubi = htmlDecoder.Decoded();

                var picDown = new WebClient();

                picDown.DownloadFileAsync(new Uri("http://www.52wubi.com" + wubi.StrokesPicturePath), Environment.GetEnvironmentVariable("TEMP") + "\\tmp.gif");

                picDown.DownloadFileCompleted += delegate
                {
                    Picture.Source = new Uri(Environment.GetEnvironmentVariable("TEMP") + "\\tmp.gif");
                };

                


                //Picture.Source = new Uri("http://www.52wubi.com" + wubi.StrokesPicturePath);            
                Loading.IsActive = false;
                WubiCoding.Text = wubi.WubiCoding;
                Pinyin.Text = wubi.Pinyin;
                Character.Text = wubi.Character;



                



                Result.Visibility = Visibility.Visible;
                Console.Write(content);
                Loading.IsActive = false;
                
            }catch(NullReferenceException)
            {
                Loading.IsActive = false;
                Console.Write(Properties.Resources.ErrorInput);
            }





        }
        

    }
}
