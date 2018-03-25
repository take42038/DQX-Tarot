using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DQXTarotApp
{
    public partial class Form2 : Form
    {

        public int intPnlPosColBK;
        public int intPnlPosRowBK;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //テーブルレイアウトパネル初期化関数呼び出し
            InitTableLayoutPanel();

            int intPnlPosCol = 0;
            int intPnlPosRow = 0;

            //ラベル動的追加
            //親のモンスター名をテーブルレイアウトパネルに表示
            AutoAddLabel(frmMain.strName, intPnlPosCol, intPnlPosRow);

            intPnlPosColBK = 0;
            intPnlPosRowBK = 0;

        }

        private System.Windows.Forms.Label lblAuto;

        private void AutoAddLabel(string strName, int intNowCol, int intNowRow)
        {
            //ラベル作成
            lblAuto = new System.Windows.Forms.Label();
            lblAuto.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            lblAuto.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            lblAuto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            lblAuto.Location = new System.Drawing.Point(10, 10);
            lblAuto.Size = new System.Drawing.Size(100, 20);
            lblAuto.TabIndex = 8;
            lblAuto.Text = strName;
            lblAuto.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            //テーブルレイアウトパネルラベル追加
            tblLayOutPnl.Controls.Add(lblAuto, intNowCol, intNowRow);

            //strNameをキーとして素材判定関数呼び出し
            //素材判定関数 CheckSozaiHantei(）呼び出し
            int intRtn = frmMain.CheckSozaiHantei(frmMain.iniPath, strName, out string strSozaiMonster1, out string strSozaiMonster2);

            //関数の戻り値が正常の場合
            if (intRtn == 0)
            {
                //子のモンスター名をテーブルレイアウトパネルに表示
                AddMonsterNameInPanel(strSozaiMonster1, strSozaiMonster2, intNowCol, intNowRow);

            }

        }
        private void InsertRow(int intInsertRow)
        {
            //行挿入関数
            //引数１：挿入位置（行）

            foreach (Control c in tblLayOutPnl.Controls)
            {
                TableLayoutPanelCellPosition pos = tblLayOutPnl.GetPositionFromControl(c);
                tblLayOutPnl.SetCellPosition(c, pos);
                if (tblLayOutPnl.RowCount <= pos.Row)
                {
                    tblLayOutPnl.RowCount = pos.Row + 1;
                }

                if (tblLayOutPnl.ColumnCount <= pos.Column)
                {
                    tblLayOutPnl.ColumnCount = pos.Column + 1;
                }
            }

            //行を増やす
            tblLayOutPnl.RowCount++;

            //コントロールを移動
            for (int y = tblLayOutPnl.RowCount - 1; y >= intInsertRow; y--)
            {
                for (int x = 0; x < tblLayOutPnl.ColumnCount; x++)
                {
                    Control c = tblLayOutPnl.GetControlFromPosition(x, y);
                    if (c != null)
                    {
                        tblLayOutPnl.SetCellPosition(
                            c, new TableLayoutPanelCellPosition(x, y + 1));
                    }
                }
            }

            //スタイルを挿入
            if (tblLayOutPnl.RowStyles.Count > intInsertRow)
            {
                tblLayOutPnl.RowStyles.Insert(
                    intInsertRow, new RowStyle(SizeType.AutoSize));
            }
        }

        private void InsertColumn(int intInsertCol)
        {
            //列挿入関数
            //引数１：挿入位置（列）

            foreach (Control c in tblLayOutPnl.Controls)
            {
                TableLayoutPanelCellPosition pos = tblLayOutPnl.GetPositionFromControl(c);
                tblLayOutPnl.SetCellPosition(c, pos);
                if (tblLayOutPnl.RowCount <= pos.Row)
                {
                    tblLayOutPnl.RowCount = pos.Row + 1;
                }

                if (tblLayOutPnl.ColumnCount <= pos.Column)
                {
                    tblLayOutPnl.ColumnCount = pos.Column + 1;
                }
            }

            //列を増やす
            tblLayOutPnl.ColumnCount++;

            //コントロールを移動
            for (int x = tblLayOutPnl.ColumnCount - 1; x >= intInsertCol; x--)
            {
                for (int y = 0; y < tblLayOutPnl.RowCount; y++)
                {
                    Control c = tblLayOutPnl.GetControlFromPosition(x, y);
                    if (c != null)
                    {
                        tblLayOutPnl.SetCellPosition(
                            c, new TableLayoutPanelCellPosition(x + 1, y));
                    }
                }
            }

            //スタイルを挿入
            if (tblLayOutPnl.ColumnStyles.Count > intInsertCol)
            {
                tblLayOutPnl.ColumnStyles.Insert(
                    intInsertCol, new ColumnStyle(SizeType.AutoSize));
            }

        }

        private void AddMonsterNameInPanel(string strKo1, string strKo2, int intPnlPosCol, int intPnlPosRow)
        {
            //テーブルレイアウトパネルにモンスター名追加関数
            //引数１：子１モンスター名
            //引数２：子２モンスター名
            //引数３：テーブルレイアウトパネルの現在位置（列）
            //引数４：テーブルレイアウトパネルの現在位置（行）


            //if (intPnlPosRow > tblLayOutPnl.RowCount)
            //{
            //    //テーブルレイアウトパネルの現在行の後ろ挿入する。
            //    InsertRow(intPnlPosRow);
            //}

            if (intPnlPosCol+1 >= tblLayOutPnl.ColumnCount)
            {
                //テーブルレイアウトパネルの現在列の後ろ挿入する。
                InsertColumn(intPnlPosCol + 1);
//                intPnlPosColBK = intPnlPosCol + 1;
            }


            ////テーブルレイアウトパネルの現在列のまえに挿入する。
            //InsertColumn(intPnlPosCol);

            //テーブルレイアウトパネルの現在行のまえに挿入する。
            InsertRow(intPnlPosRow);

            ////子１ラベル動的追加
            //intPnlPosRowBK = intPnlPosRow + 1;
            //AutoAddLabel(strKo1, intPnlPosCol, intPnlPosRow + 1);

            //子１ラベル動的追加
            intPnlPosColBK = intPnlPosCol + 1;
            AutoAddLabel(strKo1, intPnlPosCol+1, intPnlPosRow);

            ////テーブルレイアウトパネルの列を追加する。
            //intPnlPosCol = intPnlPosColBK;

            //テーブルレイアウトパネルの列を追加する。
            intPnlPosRow = intPnlPosRowBK;

            //InsertColumn(intPnlPosCol + 2);

            InsertRow(intPnlPosRow + 2);

            ////子２ラベル動的追加
            //intPnlPosColBK += 2;
            //AutoAddLabel(strKo2, intPnlPosCol + 2, intPnlPosRow + 1);

            //子２ラベル動的追加
            intPnlPosRowBK += 2;
            AutoAddLabel(strKo2, intPnlPosCol + 1, intPnlPosRow + 2);
        }

        private void InitTableLayoutPanel()
        { 
        //テーブルレイアウトパネルクリア
            foreach (Control item in tblLayOutPnl.Controls)
            {
                item.Dispose();
            }
            tblLayOutPnl.Controls.Clear();
            tblLayOutPnl.ColumnCount = 0;
            tblLayOutPnl.RowCount = 0;
        }

    }
}
