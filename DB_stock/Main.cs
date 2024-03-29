﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;



namespace DB_stock
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /**
            삼성전자
            https://finance.naver.com/item/sise.nhn?code=005930
            SK 텔레콤
            https://finance.naver.com/item/main.nhn?code=017670
            LG
            https://finance.naver.com/item/main.nhn?code=003550
            기아차
            https://finance.naver.com/item/main.nhn?code=000270
            카카오
            https://finance.naver.com/item/main.nhn?code=035720
            **/
            string url = URL_BOX.Text;
            string company_name = "";
            var web = new HtmlWeb();
            web.OverrideEncoding = Encoding.Default;
            try
            {
                var htmlDoc = web.Load(url);
                company_name = htmlDoc.DocumentNode
                            .SelectNodes("//*[@id=\"middle\"]/div[1]/div[1]/h2/a")
                            .First().InnerText;
                string[] urls = url.Split(new string[] { "code=" }, StringSplitOptions.None);
                url = "https://finance.naver.com/item/sise_day.nhn?code=" + urls[1];
                URL_BOX.Text = url;
            }
            catch (Exception)
            {
                MessageBox.Show("잘못된 링크입니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            StockParser parser = new StockParser(url, company_name);
            Write_Excel we = new Write_Excel(parser.Parse(), parser.GetName);
            we.PrintExcel();
        }

        private void KOSPI_Click(object sender, EventArgs e)
        {
            string KOSPI = "https://finance.naver.com/sise/sise_index_day.nhn?code=KOSPI";
            KosParser k = new KosParser(KOSPI);
            Write_Excel we = new Write_Excel(k.Parse(), "KOSPI");
            we.PrintExcel();
        }

        private void KOSDAQ_Click(object sender, EventArgs e)
        {
            string KOSDAQ = "https://finance.naver.com/sise/sise_index_day.nhn?code=KOSDAQ";
            KosParser k = new KosParser(KOSDAQ);
            Write_Excel we = new Write_Excel(k.Parse(), "KOSDAQ");
            we.PrintExcel();
        }
    }
}
