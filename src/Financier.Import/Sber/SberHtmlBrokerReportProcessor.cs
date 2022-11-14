using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Financier.Import.Sber
{
    public class SberHtmlBrokerReportProcessor : IBrokerReportProcessor
    {
        private readonly string _dateFormat;
        private readonly CultureInfo _cultureInfo;
        private readonly List<SberHtmlTrade> _trades;
        private readonly List<SberHtmlMove> _moves;

        public IEnumerable<IImportedTrade> Trades => _trades;
        public IEnumerable<IImportedMove> Moves => _moves;


        public SberHtmlBrokerReportProcessor()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _dateFormat = "dd.MM.yyyy";
            _cultureInfo = new CultureInfo("ru-RU");
            _cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            _cultureInfo.NumberFormat.NumberGroupSeparator = " ";

            _trades = new List<SberHtmlTrade>();
            _moves = new List<SberHtmlMove>();
        }


        public async Task LoadAndParseAsync(string fileName)
        {
            var text = await File.ReadAllTextAsync(fileName);
            Parse(text);
        }

        public void Parse(string text)
        {
            var html = new HtmlDocument();
            html.LoadHtml(text);

            ParseTrades(html);
            ParseMoves(html);
        }

        private void ParseTrades(HtmlDocument html)
        {
            var tradesNode = html.DocumentNode.SelectSingleNode("//*[text()[contains(., 'Сделки купли/продажи ценных бумаг')]]");

            if (tradesNode is null) return;

            //var table = tradesNode.ParentNode.NextSibling.NextSibling;
            var table = tradesNode.NextSibling.NextSibling;

            foreach (var row in table.SelectNodes("tr").Skip(1).Where(x => x.SelectNodes("th|td").Count > 1))
            {
                var cells = row.SelectNodes("th|td");

                if (cells.Count < 16) return;

                var trade = new SberHtmlTrade
                {
                    Date = DateTime.ParseExact(cells[0].InnerText.Trim(), _dateFormat, _cultureInfo),
                    ContractDate = DateTime.ParseExact(cells[1].InnerText.Trim(), _dateFormat, _cultureInfo),
                    Time = TimeSpan.Parse(cells[2].InnerText.Trim()),
                    SecurityName = cells[3].InnerText.Trim(),
                    SecurityCode = cells[4].InnerText.Trim(),
                    Currency = cells[5].InnerText.Trim(),
                    Direction = cells[6].InnerText.Trim(),
                    Amount = decimal.Parse(cells[7].InnerText, _cultureInfo),
                    Price = decimal.Parse(cells[8].InnerText, _cultureInfo),
                    Volume = decimal.Parse(cells[9].InnerText, _cultureInfo),
                    AccruedInterest = decimal.Parse(cells[10].InnerText, _cultureInfo),
                    BrokerCommission = decimal.Parse(cells[11].InnerText, _cultureInfo),
                    ExchangeCommission = decimal.Parse(cells[12].InnerText, _cultureInfo),
                    ExchangeTradeNo = long.Parse(cells[13].InnerText),
                    Comment = cells[14].InnerText.Trim(),
                    TradeStatus = cells[15].InnerText.Trim(),
                };

                _trades.Add(trade);
            }
        }

        private void ParseMoves(HtmlDocument html)
        {
            var movesNode = html.DocumentNode.SelectSingleNode("//*[text()[contains(., 'Движение ЦБ, не связанное с исполнением сделок')]]");

            if (movesNode is null) return;

            //var table = movesNode.ParentNode.NextSibling.NextSibling;
            var table = movesNode.NextSibling.NextSibling;

            foreach (var row in table.SelectNodes("tr").Skip(2).Where(x => x.SelectNodes("th|td").Count > 1))
            {
                var cells = row.SelectNodes("th|td");

                if (cells.Count < 11) return;

                var move = new SberHtmlMove
                {
                    Date = DateTime.ParseExact(cells[0].InnerText.Trim(), _dateFormat, _cultureInfo),
                    SecurityCode = cells[1].InnerText.Trim(),
                    SecurityName = cells[2].InnerText.Trim(),
                    Type = cells[3].InnerText.Trim(),
                    Reason = cells[4].InnerText.Trim(),
                    Amount = decimal.Parse(cells[5].InnerText, _cultureInfo),
                    BuyDate = cells[6].InnerText.Trim(),
                    Price = cells[7].InnerText.Trim(),
                    BrokerCommission = cells[8].InnerText.Trim(),
                    ExchangeCommission = cells[9].InnerText.Trim(),
                    OtherCosts = cells[10].InnerText.Trim(),
                };

                _moves.Add(move);
            }
        }
    }
}
