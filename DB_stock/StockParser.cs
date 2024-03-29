﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace DB_stock
{
    class StockParser
    {
        private string url;
        private string company_name;
        private List<string> Dates;    //날짜
        private List<string> ClosingsPrices;   //종가
        private List<string> AgoPrices;    //전일비
        private List<string> MarketValues;  //시가
        private List<string> HighValues;    //고가
        private List<string> LowValues;     //저가
        private List<string> Volumes;       //거래량
        
        public string GetName
        {
            get { return company_name; }
        }

        public StockParser(string url, string company_name)
        {
            this.url = url;
            this.company_name = company_name;
        }

        public void InitList()
        {
            Dates = new List<string>();
            ClosingsPrices = new List<string>();
            AgoPrices = new List<string>();
            MarketValues = new List<string>();
            HighValues = new List<string>();
            LowValues = new List<string>();
            Volumes = new List<string>();
        }
        public List<List<string>> Parse()
        {
            InitList();
            List<List<string>> res = new List<List<string>>();

            const string page = "&page=";
            for(int cnt = 1; cnt <= 12; cnt++)
            {
                string cur_page = page + cnt.ToString();
                string cur_url = url + cur_page;
                var web = new HtmlWeb();
                web.OverrideEncoding = Encoding.Default;
                web.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36";
                var htmlDoc = web.Load(cur_url);
                for (int i = 3; i <= 15; i++)
                {
                    if (i == 8 || i == 9 || i == 10)
                        continue;
                    string RootNode = "/html/body/table[1]/tr[" + i.ToString() + "]";
                    List<string> datas = ParserFunction.ReturnDatas(htmlDoc, RootNode, len: 7);
                    Dates.Add(datas[0]);
                    ClosingsPrices.Add(datas[1]);
                    AgoPrices.Add(datas[2] + datas[3]);
                    MarketValues.Add(datas[4]);
                    HighValues.Add(datas[5]);
                    LowValues.Add(datas[6]);
                    Volumes.Add(datas[7]);
                }
            }
            res.Add(Dates);
            res.Add(ClosingsPrices);
            res.Add(AgoPrices);
            res.Add(MarketValues);
            res.Add(HighValues);
            res.Add(LowValues);
            res.Add(Volumes);
            return res;
        }
    }
}
