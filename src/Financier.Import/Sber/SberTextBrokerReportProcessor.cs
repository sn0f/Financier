using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Financier.Import.Sber
{
    public class SberTextBrokerReportProcessor : IBrokerReportProcessor
    {
        private string _dateFormat;
        private Encoding _encoding;
        private CultureInfo _cultureInfo;
        private readonly List<SberTextTrade> _trades;
        private readonly List<SberTextMove> _moves;

        public IEnumerable<IImportedTrade> Trades => _trades;
        public IEnumerable<IImportedMove> Moves => _moves;

        #region Regex Patterns
        private string patternCleanRunningTitle = @"\f\s+Учет операций с ЦБ  DiasoftCustody[^\r\n]+\s+-+\s";
        private string patternSecurityMoveTable = @"\s+Движение ЦБ, не связанное с исполнением сделок.\s.+(?:=\s\s)";

        /// <summary>
        /// |15/07/2015 ...
        /// </summary>
        private string patternSecurityMoveTableParse = @"^\|([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}.*";
        private string patternTradesTableParse = @"^\|([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}.*";
        private string patternTradesTables = @"\s+Биржевые сделки купли-продажи ценных бумаг \(ТС ФБ ММВБ\).+?(?=(?:\r*\n){2})";
        #endregion


        public SberTextBrokerReportProcessor()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _dateFormat = "dd/MM/yyyy";
            _encoding = Encoding.GetEncoding("windows-1251");
            _cultureInfo = CultureInfo.InvariantCulture;

            _trades = new List<SberTextTrade>();
            _moves = new List<SberTextMove>();
        }


        public async Task LoadAndParseAsync(string fileName)
        {
            var text = await File.ReadAllTextAsync(fileName, _encoding);
            Parse(text);
        }

        public void Parse(string text)
        {
            text = Regex.Replace(text, patternCleanRunningTitle, "");
            // чистим дубли символа конца строк
            text = Regex.Replace(text, @"\n\n", "\n");

            var tradesMatches = Regex.Matches(text, patternTradesTables,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var trades = new List<string>();

            foreach (var match in tradesMatches)
            {
                var matchItems = Regex.Matches(match.ToString(), patternTradesTableParse,
                    RegexOptions.IgnoreCase | RegexOptions.Multiline);

                foreach (var match2 in matchItems)
                    trades.Add(match2.ToString());
            }

            ParseTrades(trades);

            var movesMatches = Regex.Matches(text, patternSecurityMoveTable,
                RegexOptions.IgnoreCase | RegexOptions.Singleline);

            var moves = new List<string>();

            foreach (var match in movesMatches)
            {
                var matchItems = Regex.Matches(match.ToString(), patternSecurityMoveTableParse,
                    RegexOptions.IgnoreCase | RegexOptions.Multiline);

                foreach (var match2 in matchItems)
                    moves.Add(match2.ToString());
            }

            ParseMoves(moves);
        }

        private void ParseTrades(ICollection<string> trades)
        {
            _trades.AddRange(trades.Select(x => x.Split("|")).Select(x => new SberTextTrade
            {
                Date = DateTime.ParseExact(x[1].Trim(), _dateFormat, _cultureInfo),
                Time = TimeSpan.Parse(x[2]),
                SecurityName = x[3].Trim(),
                SecurityCode = x[4].Trim(),
                Direction = x[5].Trim(),
                BankTradeNo = x[6].Trim(),
                ExchangeTradeNo = long.Parse(x[7]),
                Amount = decimal.Parse(x[8], _cultureInfo),
                Price = decimal.Parse(x[9], _cultureInfo),
                Volume = decimal.Parse(x[10], _cultureInfo),
                AccruedInterest = decimal.Parse(x[11], _cultureInfo),
                BrokerCommission = decimal.Parse(x[12], _cultureInfo),
                ExchangeCommission = decimal.Parse(x[13], _cultureInfo),
                Comment = x[14].Trim(),
                PaymentDate = DateTime.ParseExact(x[15].Trim(), _dateFormat, _cultureInfo),
                SupplyDate = DateTime.ParseExact(x[16].Trim(), _dateFormat, _cultureInfo),
                TradeStatus = x[17].Trim(),
            }).OrderBy(x => x.Date).ThenBy(x => x.Time).ToList());
        }

        private void ParseMoves(ICollection<string> moves)
        {
            _moves.AddRange(moves.Select(x => x.Split("|")).Select(x => new SberTextMove
            {
                Date = DateTime.ParseExact(x[1].Trim(), _dateFormat, _cultureInfo),
                SecurityName = x[2].Trim(),
                SecurityCode = x[3].Trim(),
                OperationCode = x[4].Trim(),
                Type = x[5].Trim(),
                BankOperationNo = x[6].Trim(),
                Reason = x[7].Trim(),
                Amount = decimal.Parse(x[8], _cultureInfo),
                BuyDate = x[9].Trim(),
                Price = decimal.Parse(x[10], _cultureInfo),
                BrokerCommission = decimal.Parse(x[11], _cultureInfo),
                ExchangeCommission = decimal.Parse(x[12], _cultureInfo),
                OtherCosts = x[13].Trim(),
            }).OrderBy(x => x.Date).ToList());
        }
    }
}
