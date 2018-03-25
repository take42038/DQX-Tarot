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

        public static string iniPath;
        public static string[] strArySection;
        public static string jpgPath;
        public static string strName;


        public frmMain()
        {
            InitializeComponent();
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255))))) ;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            //詳細表示ボタンを無効にする
            btnDetail.Enabled = false;

            //iniファイルのパス設定
            iniPath = Application.StartupPath + Path.DirectorySeparatorChar + "Monster.ini";
            //jpgファイルのパス設定
            jpgPath = Application.StartupPath + Path.DirectorySeparatorChar + @"image\";

            if (File.Exists(iniPath))
            {
                // iniファイルよりセクション名一覧を取得
                IntPtr ptr = Marshal.StringToHGlobalAnsi(new String('\0', 1024));
                int length = GetPrivateProfileSectionNames(ptr, 1024, iniPath);

                //セクション名が取得できたら
                if (0 < length)
                {
                    //セクション名一覧のポインタからstringへ
                    String resultSection = Marshal.PtrToStringAnsi(ptr, length);

                    //取得したセクション名一覧を配列へ退避
                    strArySection = resultSection.Split( new[] { '\0' },StringSplitOptions.RemoveEmptyEntries);

                    //取得したセクション名一覧をコンボボックスに設定
                    Array.ForEach<String>(resultSection.Split(new[] { '\0' },StringSplitOptions.RemoveEmptyEntries), strRank => cmbRank.Items.Add(strRank));

                    //コンボボックスの一番上に”すべて”を追加
                    cmbRank.Items.Insert(0, "すべて");                    
                }

                Marshal.FreeHGlobal(ptr);

            }

        }

        private void cmbRank_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cmbMonster.Items.Clear();
            cmbMonster.Items.Add("");
            cmbMonster.SelectedIndex = 0;

//            cmbMonster.Text = "";
            txtBoxSozai_2_1.Text = "";
            txtBoxSozai_2_2.Text = "";

            // iniファイルより指定したセクションのキー一覧を取得
            byte[] ar1 = new byte[1024];

            //選択されたランクを設定
            string strSelectRank = cmbRank.SelectedItem.ToString();

            //選択されたランクが”すべて”の場合
            if (strSelectRank == "すべて")
            {
                //退避していたセクション名一覧（全ランク）分のキー名一覧を取得
                foreach (string s in strArySection)
                {
                    int lengthAll = GetPrivateProfileStringByByteArray(s, null, "", ar1, 1024, iniPath);
                    if (0 < lengthAll)
                    {
                        //取得したキー名一覧をコンボボックスに設定
                        string resultKey = Encoding.Default.GetString(ar1, 0, lengthAll - 1);
                        Array.ForEach<String>(resultKey.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries), strMonster => cmbMonster.Items.Add(strMonster));
                    }

                }
                cmbMonster.SelectedIndex = -1;
            }
            //選択されたランクが”すべて”以外の場合
            else
            {
            //選択されたランクのセクションのキー名一覧を取得
            int length = GetPrivateProfileStringByByteArray(strSelectRank, null, "", ar1, 1024, iniPath);

                //キーが取得できた場合
                if (0 < length)
                {
                    //取得したキー名一覧をコンボボックスに設定
                    string resultKey = Encoding.Default.GetString(ar1, 0, length - 1);
                    Array.ForEach<String>(resultKey.Split(new[] { '\0' }, StringSplitOptions.RemoveEmptyEntries), strMonster => cmbMonster.Items.Add(strMonster));
                    cmbMonster.SelectedIndex = 0;
                }
                //キーが取得できなかった場合
                else
                {
                    cmbMonster.SelectedIndex = -1;
                    cmbMonster.Items.Clear();
                    cmbMonster.Items.Add("対象モンスターが見つかりません。");
                    cmbMonster.SelectedIndex = 0;
                }
                cmbMonster.SelectedIndex = -1;

            }

        }

        private void cmbMonster_SelectedIndexChanged(object sender, EventArgs e)
        {

            if ((cmbRank.SelectedIndex == -1) || (cmbMonster.SelectedIndex == -1))
            {
                return;
            }


            //選択されたモンスター名（ランク）を設定
            string orgStr = cmbMonster.SelectedItem.ToString();

            //モンスター名の画像を表示する
            pictBox_1_1.ImageLocation = jpgPath + orgStr + @".jpg";

            //素材判定関数 CheckSozaiHantei(）呼び出し
            int intRtn = CheckSozaiHantei(iniPath, orgStr, out string strSozaiMonster1, out string strSozaiMonster2);

            //関数の戻り値が正常の場合
            if (intRtn == 0)
            {
                //詳細表示ボタンを有効にする
                btnDetail.Enabled = true;

                //テキストボックスに素材モンスター名を設定
                txtBoxSozai_2_1.Text = strSozaiMonster1;
                txtBoxSozai_2_2.Text = strSozaiMonster2;
                line1_1.Visible = true;
                line1_2.Visible = true;

            }
            else
            //関数の戻り値が異常の場合
            {
                //テキストボックスをクリア
                txtBoxSozai_2_1.Text = "";
                txtBoxSozai_2_2.Text = "";
                line1_1.Visible = false;
                line1_2.Visible = false;
            }
        }


        public static int CheckSozaiHantei(string IniFilePath,string InParm,out string OutParm1,out string OutParm2)
        {
            //機能　：合成後のモンスター名を入力し、iniファイルから素材となるモンスター名を検索する。
            //引数１：入力　iniファイルのフルパス　
            //引数２：入力　合成モンスター　
            //引数３：出力　素材モンスター１
            //引数４：出力　素材モンスター２
            //戻り値：正常終了（0）、異常終了（-1）

            try
            {
                //引数２の”モンスター名（ランク）”からランクだけを抽出
                //検索文字列設定
                string str1 = "(";
                string str2 = ")";
                //引数２の合成モンスター名の文字列の長さ
                int orgLen = InParm.Length;
                //str1の長さ
                int str1Len = str1.Length;
                //str1が原文のどの位置にあるか
                int str1Num = InParm.IndexOf(str1); 
                //原文の初めからstr1のある位置まで削除
                string strSection = InParm.Remove(0, str1Num + str1Len); 
                //str2がどの位置にあるか
                int str2Num = strSection.IndexOf(str2);
                //str2のある位置から最後まで削除して、セクション名にランクを設定
                strSection = strSection.Remove(str2Num); 

                //キー名にモンスター名を設定
                string strKey = InParm;

                StringBuilder sb = new StringBuilder(1024);

                //iniファイルから指定したセクションのキーの値を読み込む
                GetPrivateProfileString(
                    strSection,             //セクション名 
                    strKey,                 //キー名
                    "error",                //読み込めなかった時の設定値
                    sb,                     //読み込んだキーの値
                    (uint)sb.Capacity,      //読み込んだキーの値のサイズ
                    IniFilePath             //iniファイルのフルパス
                    );

                string[] strAryValue;

                if (sb.ToString() != "error")
                {
                    //取得したキーの値を配列へ退避
                    strAryValue = sb.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    //引数３の出力１へ設定
                    OutParm1 = strAryValue[0];
                    //引数４の出力２へ設定
                    OutParm2 = strAryValue[1];

                }
                else
                {
                    //キーの値が取得できなかった場合
                    OutParm1 = "";
                    OutParm2 = "";
                }

                return 0;

            }
            catch
            {
                OutParm1 = "";
                OutParm2 = "";
                return -1;
            }

        }

        private void txtBoxSozai_2_1_TextChanged(object sender, EventArgs e)
        {
            //選択されたモンスター名（ランク）を設定
            string orgStr = txtBoxSozai_2_1.Text; 

            //モンスター名の画像を表示する
            pictBox_2_1.ImageLocation = jpgPath + orgStr + @".jpg";


            //素材判定関数 CheckSozaiHantei(）呼び出し
            int intRtn = CheckSozaiHantei(iniPath, orgStr, out string strSozaiMonster1, out string strSozaiMonster2);

            //関数の戻り値が正常の場合
            if (intRtn == 0)
            {
                //テキストボックスに素材モンスター名を設定
                txtBoxSozai_3_1.Text = strSozaiMonster1;
                txtBoxSozai_3_2.Text = strSozaiMonster2;
                line2_1.Visible = true;
                line2_2.Visible = true;
            }
            else
            //関数の戻り値が異常の場合
            {
                //テキストボックスをクリア
                txtBoxSozai_3_1.Text = "";
                txtBoxSozai_3_2.Text = "";
                line2_1.Visible = false;
                line2_2.Visible = false;
            }


        }

        private void txtBoxSozai_2_2_TextChanged(object sender, EventArgs e)
        {
            //選択されたモンスター名（ランク）を設定
            string orgStr = txtBoxSozai_2_2.Text;

            //モンスター名の画像を表示する
            pictBox_2_2.ImageLocation = jpgPath + orgStr + @".jpg";

            //素材判定関数 CheckSozaiHantei(）呼び出し
            int intRtn = CheckSozaiHantei(iniPath, orgStr, out string strSozaiMonster1, out string strSozaiMonster2);

            //関数の戻り値が正常の場合
            if (intRtn == 0)
            {
                //テキストボックスに素材モンスター名を設定
                txtBoxSozai_3_3.Text = strSozaiMonster1;
                txtBoxSozai_3_4.Text = strSozaiMonster2;
                line2_3.Visible = true;
                line2_4.Visible = true;
            }
            else
            //関数の戻り値が異常の場合
            {
                //テキストボックスをクリア
                txtBoxSozai_3_3.Text = "";
                txtBoxSozai_3_4.Text = "";
                line2_3.Visible = false;
                line2_4.Visible = false;
            }



        }

        private void txtBoxSozai_3_1_TextChanged(object sender, EventArgs e)
        {
            //選択されたモンスター名（ランク）を設定
            string orgStr = txtBoxSozai_3_1.Text;

            //モンスター名の画像を表示する
            pictBox_3_1.ImageLocation = jpgPath + orgStr + @".jpg";

            //素材判定関数 CheckSozaiHantei(）呼び出し
            int intRtn = CheckSozaiHantei(iniPath, orgStr, out string strSozaiMonster1, out string strSozaiMonster2);

            //関数の戻り値が正常の場合
            if (intRtn == 0)
            {
                //テキストボックスに素材モンスター名を設定
                txtBoxSozai_4_1.Text = strSozaiMonster1;
                txtBoxSozai_4_2.Text = strSozaiMonster2;
                line3_1.Visible = true;
                line3_2.Visible = true;
            }
            else
            //関数の戻り値が異常の場合
            {
                //テキストボックスをクリア
                txtBoxSozai_4_1.Text = "";
                txtBoxSozai_4_2.Text = "";
                line3_1.Visible = false;
                line3_2.Visible = false;
            }

        }

        private void txtBoxSozai_3_2_TextChanged(object sender, EventArgs e)
        {
            //選択されたモンスター名（ランク）を設定
            string orgStr = txtBoxSozai_3_2.Text;

            //モンスター名の画像を表示する
            pictBox_3_2.ImageLocation = jpgPath + orgStr + @".jpg";

            //素材判定関数 CheckSozaiHantei(）呼び出し
            int intRtn = CheckSozaiHantei(iniPath, orgStr, out string strSozaiMonster1, out string strSozaiMonster2);

            //関数の戻り値が正常の場合
            if (intRtn == 0)
            {
                //テキストボックスに素材モンスター名を設定
                txtBoxSozai_4_3.Text = strSozaiMonster1;
                txtBoxSozai_4_4.Text = strSozaiMonster2;
                line3_3.Visible = true;
                line3_4.Visible = true;
            }
            else
            //関数の戻り値が異常の場合
            {
                //テキストボックスをクリア
                txtBoxSozai_4_3.Text = "";
                txtBoxSozai_4_4.Text = "";
                line3_3.Visible = false;
                line3_4.Visible = false;
            }

        }

        private void txtBoxSozai_3_3_TextChanged(object sender, EventArgs e)
        {
            //選択されたモンスター名（ランク）を設定
            string orgStr = txtBoxSozai_3_3.Text;

            //モンスター名の画像を表示する
            pictBox_3_3.ImageLocation = jpgPath + orgStr + @".jpg";

            //素材判定関数 CheckSozaiHantei(）呼び出し
            int intRtn = CheckSozaiHantei(iniPath, orgStr, out string strSozaiMonster1, out string strSozaiMonster2);

            //関数の戻り値が正常の場合
            if (intRtn == 0)
            {
                //テキストボックスに素材モンスター名を設定
                txtBoxSozai_4_5.Text = strSozaiMonster1;
                txtBoxSozai_4_6.Text = strSozaiMonster2;
                line3_5.Visible = true;
                line3_6.Visible = true;
            }
            else
            //関数の戻り値が異常の場合
            {
                //テキストボックスをクリア
                txtBoxSozai_4_5.Text = "";
                txtBoxSozai_4_6.Text = "";
                line3_5.Visible = false;
                line3_6.Visible = false;
            }

        }

        private void txtBoxSozai_3_4_TextChanged(object sender, EventArgs e)
        {
            //選択されたモンスター名（ランク）を設定
            string orgStr = txtBoxSozai_3_4.Text;

            //モンスター名の画像を表示する
            pictBox_3_4.ImageLocation = jpgPath + orgStr + @".jpg";

            //素材判定関数 CheckSozaiHantei(）呼び出し
            int intRtn = CheckSozaiHantei(iniPath, orgStr, out string strSozaiMonster1, out string strSozaiMonster2);

            //関数の戻り値が正常の場合
            if (intRtn == 0)
            {
                //テキストボックスに素材モンスター名を設定
                txtBoxSozai_4_7.Text = strSozaiMonster1;
                txtBoxSozai_4_8.Text = strSozaiMonster2;
                line3_7.Visible = true;
                line3_8.Visible = true;
            }
            else
            //関数の戻り値が異常の場合
            {
                //テキストボックスをクリア
                txtBoxSozai_4_7.Text = "";
                txtBoxSozai_4_8.Text = "";
                line3_7.Visible = false;
                line3_8.Visible = false;
            }

        }

        private void txtBoxSozai_4_1_TextChanged(object sender, EventArgs e)
        {
            //選択されたモンスター名（ランク）を設定
            string orgStr = txtBoxSozai_4_1.Text;

            //モンスター名の画像を表示する
            pictBox_4_1.ImageLocation = jpgPath + orgStr + @".jpg";


        }

        private void txtBoxSozai_4_2_TextChanged(object sender, EventArgs e)
        {
            //選択されたモンスター名（ランク）を設定
            string orgStr = txtBoxSozai_4_2.Text;

            //モンスター名の画像を表示する
            pictBox_4_2.ImageLocation = jpgPath + orgStr + @".jpg";

        }

        private void txtBoxSozai_4_3_TextChanged(object sender, EventArgs e)
        {
            //選択されたモンスター名（ランク）を設定
            string orgStr = txtBoxSozai_4_3.Text;

            //モンスター名の画像を表示する
            pictBox_4_3.ImageLocation = jpgPath + orgStr + @".jpg";

        }

        private void txtBoxSozai_4_4_TextChanged(object sender, EventArgs e)
        {
            //選択されたモンスター名（ランク）を設定
            string orgStr = txtBoxSozai_4_4.Text;

            //モンスター名の画像を表示する
            pictBox_4_4.ImageLocation = jpgPath + orgStr + @".jpg";

        }

        private void txtBoxSozai_4_5_TextChanged(object sender, EventArgs e)
        {
            //選択されたモンスター名（ランク）を設定
            string orgStr = txtBoxSozai_4_5.Text;

            //モンスター名の画像を表示する
            pictBox_4_5.ImageLocation = jpgPath + orgStr + @".jpg";

        }

        private void txtBoxSozai_4_6_TextChanged(object sender, EventArgs e)
        {
            //選択されたモンスター名（ランク）を設定
            string orgStr = txtBoxSozai_4_6.Text;

            //モンスター名の画像を表示する
            pictBox_4_6.ImageLocation = jpgPath + orgStr + @".jpg";

        }

        private void txtBoxSozai_4_7_TextChanged(object sender, EventArgs e)
        {
            //選択されたモンスター名（ランク）を設定
            string orgStr = txtBoxSozai_4_7.Text;

            //モンスター名の画像を表示する
            pictBox_4_7.ImageLocation = jpgPath + orgStr + @".jpg";

        }

        private void txtBoxSozai_4_8_TextChanged(object sender, EventArgs e)
        {
            //選択されたモンスター名（ランク）を設定
            string orgStr = txtBoxSozai_4_8.Text;

            //モンスター名の画像を表示する
            pictBox_4_8.ImageLocation = jpgPath + orgStr + @".jpg";

        }


        private void btnDetail_Click(object sender, EventArgs e)
        {
            strName = cmbMonster.SelectedItem.ToString();
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}
