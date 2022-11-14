using System;
using System.Collections.Generic;
using System.Text;

namespace Financier.Import.Moex
{
    public class DataJson
    {
        public string BOARDID { get; set; }
        public string TRADEDATE { get; set; }
        public string SHORTNAME { get; set; }
        public string SECID { get; set; }
        public int NUMTRADES { get; set; }
        public double VALUE { get; set; }
        public double? OPEN { get; set; }
        public double? LOW { get; set; }
        public double? HIGH { get; set; }
        public double? LEGALCLOSEPRICE { get; set; }
        public double? WAPRICE { get; set; }
        public double? CLOSE { get; set; }
        public long VOLUME { get; set; }
        public double? MARKETPRICE2 { get; set; }
        public double? MARKETPRICE3 { get; set; }
        public double? ADMITTEDQUOTE { get; set; }
        public double MP2VALTRD { get; set; }
        public double MARKETPRICE3TRADESVALUE { get; set; }
        public double ADMITTEDVALUE { get; set; }
        public object WAVAL { get; set; }
    }
}
