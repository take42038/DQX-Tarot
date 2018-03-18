using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//using System;
using System.IO;
//using System.Text;
using System.Runtime.InteropServices;


namespace DQXTarotApp
{
    public partial class frmMain : Form
    {
        [DllImport("kernel32.dll")]
        static extern int GetPrivateProfileSectionNames(
            IntPtr lpszReturnBuffer,
            uint nSize,
            string lpFileName);

        [DllImport("KERNEL32.DLL",EntryPoint = "GetPrivateProfileStringA")]
        static extern int GetPrivateProfileStringByByteArray(
            string lpAppName,
            string lpKeyName, 
            string lpDefault,
            byte[] lpReturnedString, 
            uint nSize,
            string lpFileName);


        [DllImport("KERNEL32.DLL")]
        static extern int GetPrivateProfileString(
            string lpAppName,
            string lpKeyName,
            string lpDefault,
            StringBuilder lpReturnedString,
            uint nSize,
            string lpFileName);

        public string path;
        public string[] strArySection;

        public frmMain()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

            cmbRank.Text = "ランクを選択してください。";

            //iniファイルのパス設定
            path = Application.StartupPath + Path.DirectorySeparatorChar + "Monster.ini";
            
            if (File.Exists(path))
            {
                // iniファイルよりセクション名一覧を取得
                IntPtr ptr = Marshal.StringToHGlobalAnsi(new String('\0', 1024));
                int length = GetPrivateProfileSectionNames(ptr, 1024, path);

                //セクション名が取得できたら
                if (0 < length)
                {
                    //セクション名一覧のポインタからstringへ
                    String resultSection = Marshal.PtrToStringAnsi(ptr, length);

                    //取得したセクション名一覧を配列へ退避
                    strArySection = resultSection.Split( new[] { '\0' },StringSplitOptions.RemoveEmptyEntries);

                    //取得したセクション名一覧をコンボボックスに設定
                    Array.ForEach<String>(resultSection.Split(new[] { '\0' },StringSplitOptions.RemoveEmptyEntries), strRank => cmbRank.Items.Add(strRank));

/* すべてのランクをいったん廃止
                    //コンボボックスの一番上に”すべて”を追加
                    cmbRank.Items.Insert(0, "すべて");                    
*/
                }

                Marshal.FreeHGlobal(ptr);



            }
        }

        private void cmbRank_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cmbMonster.Items.Clear();
            cmbMonster.Text = "";
            txtBoxSozaiA.Text = "";
            txtBoxSozaiB.Text = "";

            // iniファイルより指定したセクションのキー一覧を取得
            //IntPtr ptr = Marshal.StringToHGlobalAnsi(new String('\0', 1024));
            byte[] ar1 = new byte[1024];

            //選択されたランクを設定
            string strSelectRank = cmbRank.SelectedItem.ToString();

            /* すべてのランクをいったん廃止
                        //選択されたランクが”すべて”の場合
                        if (strSelectRank == "すべて")
                        {
                            //退避していたセクション名一覧（全ランク）分のキー名一覧を取得
                            foreach (string s in strArySection)
                            {
                                int lengthAll = GetPrivateProfileStringByByteArray(s, null, "", ar1, 1024, path);
                                if (0 < lengthAll)
                                {
                                    //取得したキー名一覧をコンボボックスに設定
                                    string resultKey = Encoding.Default.GetString(ar1, 0, lengthAll - 1);
                                    Array.ForEach<String>(resultKey.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries), strMonster => cmbMonster.Items.Add(strMonster));
                                }

                            }
                            cmbMonster.Text = "モンスターを選択してください。";

                        }
                        //選択されたランクが”すべて”以外の場合
                        else
            {
            */
            //選択されたランクのセクションのキー名一覧を取得
            int length = GetPrivateProfileStringByByteArray(strSelectRank, null, "", ar1, 1024, path);

                //キーが取得できた場合
                if (0 < length)
                {
                    //取得したキー名一覧をコンボボックスに設定
                    string resultKey = Encoding.Default.GetString(ar1, 0, length - 1);
                    Array.ForEach<String>(resultKey.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries), strMonster => cmbMonster.Items.Add(strMonster));
                    cmbMonster.Text = "モンスターを選択してください。";
                }
                //キーが取得できなかった場合
                else
                {
                    cmbMonster.SelectedIndex = -1;
                    cmbMonster.Items.Clear();
                    cmbMonster.Items.Add("");
                    cmbMonster.Text = "対象モンスターが見つかりません。";
                }
                
//            }

        }

        private void btnSozai_Click(object sender, EventArgs e)
        {
            if ((cmbRank.SelectedIndex == -1) || (cmbMonster.SelectedIndex == -1))
            {
                return;
            }
            //選択されたランクを設定
            string strSelectRank = cmbRank.SelectedItem.ToString();
            //選択されたモンスターを設定
            string strSelectMonster = cmbMonster.SelectedItem.ToString();

            StringBuilder sb = new StringBuilder(1024);
            GetPrivateProfileString(strSelectRank, strSelectMonster,
                    "error", sb, (uint)sb.Capacity, path);
 
            string[] strAryMonster;

            if (sb.ToString() != "error")
            {
                //取得したセクション名一覧を配列へ退避
                strAryMonster = sb.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                txtBoxSozaiA.Text = strAryMonster[0];
                txtBoxSozaiB.Text = strAryMonster[1];

            }
            else
            {
                txtBoxSozaiA.Text = "";
                txtBoxSozaiB.Text = "";
            }




        }
    }
}
