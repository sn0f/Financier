using System;

namespace Financier.Import.Sber
{
    public class SberBrokerReportProcessorFactory
    {
        public static IBrokerReportProcessor CreateFromFileExtension(string fileExtension)
        {
            if (fileExtension == ".txt")
                return new SberTextBrokerReportProcessor();
            if (fileExtension == ".html")
                return new SberHtmlBrokerReportProcessor();

            throw new NotSupportedException($"Unprocessable file type {fileExtension}");
        }
    }
}
