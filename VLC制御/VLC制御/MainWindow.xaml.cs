using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using Microsoft.Win32;  //ファイル選択ダイアログ使用のため追加
using System.Text.RegularExpressions;  //ファイルから文字列を検索するためのメソッドを呼び出すため追加
using System.Runtime.InteropServices;  //マウスクリックイベント処理のため追加

namespace VLC制御アプリ
{
    class DllImportSample
    {
        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void SetCursorPos(int X, int Y);

        [DllImport("USER32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

    }

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[] Multi_vlc = new string[6]; //VLCプレイヤーを複数していして起動する際に使用


        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndparent, IntPtr hwndChildafter, string lpszClass, string lpszWindow);

        // 指定したプロセスをアクティブにするためのメソッド使用のために宣言追加
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.ProcessStartInfo app = new System.Diagnostics.ProcessStartInfo();
            app.FileName = @"C:\Program Files\VideoLAN\VLC\vlc.exe";
            //app.Arguments = @"""C:\Users\nttbp\Desktop\20190610\103802\20190610_103802_300FPS_CAM01.MP4""";

            //System.Diagnostics.Process.Start(app);




            var dialog1 = new Microsoft.Win32.OpenFileDialog();

            dialog1.Multiselect = true;

            dialog1.InitialDirectory = @"C:\TagAdding\DoujiVLC"; //フォルダ指定

            dialog1.Title = "同時再生する動画ファイルを6つまで選んでください"; //ダイアログタイトル指定

            dialog1.Filter = "MP4ファイル(*.MP4)|*.MP4|全てのファイル(*.*)|*.*";

            if (dialog1.ShowDialog() == true)
            {
                if (dialog1.FileNames.Length < 7)
                {
                    System.Diagnostics.Trace.WriteLine(dialog1.FileName);
                    int cnt1 = dialog1.FileNames.Length;

                    for (int cnt2 = 0; cnt2 < dialog1.FileNames.Length; cnt2++)
                    {
                        Multi_vlc[cnt2] = dialog1.FileNames[cnt2];
                        app.Arguments = Multi_vlc[cnt2] + " --align=1";

                        System.Diagnostics.Process.Start(app);

                        //Multi_vlc[cnt2] = dialog1.FileNames[cnt2];

                        System.Diagnostics.Trace.WriteLine("app.Argument = " + app.Arguments);
                        System.Diagnostics.Trace.WriteLine("FileName = " + dialog1.FileNames[cnt2]);
                        System.Diagnostics.Trace.WriteLine("cnt2 = " + cnt2);
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("ファイル数が7ファイル以上選ばれています。6以下としてください。");
                }

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int a = 0
                ;
            IntPtr hWnd = FindWindow(null, "vlc");

            //起動中のプロセス全てをサーチする
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                //プロセス中でタイトルに"VLCメディアプレイヤー"と表記があるものについて処理を行う
                if (0 <= p.MainWindowTitle.IndexOf("VLCメディアプレイヤー"))
                {
                    //VLCメディアプレイヤーの起動数カウント
                    a++;

                    //コマンド入力が行えるようにするためにVLCプレイヤーの画面を前面にする
                    SetForegroundWindow(p.MainWindowHandle);
                    System.Diagnostics.Trace.WriteLine("a[" + a + "]:" + p.MainWindowHandle);

                    //VLCプレイヤーの仕様で"Space"ボタン押下で、再生、一時停止を繰り返す使用となっている。
                    //そのため、Spaceボタン押下コマンドを送信する。
                    System.Windows.Forms.SendKeys.SendWait(" ");
                }
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int b = 0;

            //起動中のプロセス全てをサーチする
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                //プロセス中でタイトルに"VLCメディアプレイヤー"と表記があるものについて処理を行う
                if (0 <= p.MainWindowTitle.IndexOf("VLCメディアプレイヤー"))
                {
                    //VLCメディアプレイヤーの起動数カウント
                    b++;

                    //コマンド入力が行えるようにするためにVLCプレイヤーの画面を前面にする
                    SetForegroundWindow(p.MainWindowHandle);
                    System.Diagnostics.Trace.WriteLine("b[" + b + "]:" + p.MainWindowHandle);

                    //VLCプレイヤーの仕様で"s"ボタン押下で、終了
                    System.Windows.Forms.SendKeys.SendWait("s");
                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            int c = 0;

            //起動中のプロセス全てをサーチする
            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                //プロセス中でタイトルに"VLCメディアプレイヤー"と表記があるものについて処理を行う
                if (0 <= p.MainWindowTitle.IndexOf("VLCメディアプレイヤー"))
                {
                    //VLCメディアプレイヤーの起動数カウント
                    c++;

                    //コマンド入力が行えるようにするためにVLCプレイヤーの画面を前面にする

                    SetForegroundWindow(p.MainWindowHandle);
                    System.Diagnostics.Trace.WriteLine("c[" + c + "]:" + p.MainWindowHandle);

                    //VLCプレイヤーの仕様で"s"ボタン押下で、終了
                    System.Windows.Forms.SendKeys.SendWait("^q");
                }
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process p = System.Diagnostics.Process.Start(@"C:\TagAdding\SideNaraebe.vbs");
        }
    }
}
