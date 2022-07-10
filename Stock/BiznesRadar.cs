using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace Stock
{
    public class BiznesRadar
    {
        public string GetHtml(string url)
        {
            using (WebClient client = new WebClient())
            {
                while (true)
                {
                    try
                    {
                        return client.DownloadString(url);
                    }
                    catch
                    {
                        Thread.Sleep(3*60*1000);
                    }
                }
                
            }
        }
        public string GetListeningHtml(string biznesRadarName, int page = 1)
        {
            return GetHtml(GetArchiveListeningUrl(biznesRadarName, page));
        }
        public string GetMainPageHtml(string biznesRadarName)
        {
            return GetHtml(GetMainPageUrl(biznesRadarName));
        }
        public string GetAllGPWStocksHtml()
        {
            return GetHtml(GetAllGPWStocksUrl());
        }
        private string GetAllGPWStocksUrl()
        {
            return "http://www.biznesradar.pl/gielda/akcje_gpw";
        }

        public string GetAllNewConnectStocksHtml()
        {
            return GetHtml(GetAllNewConnectStocksUrl());
        }
        private string GetAllNewConnectStocksUrl()
        {
            return "http://www.biznesradar.pl/gielda/newconnect";
        }
        
        private string GetFinancialIndicatorsUrl(string company)
        {
            return @"http://www.biznesradar.pl/wskazniki-wartosci-rynkowej/" + company;
        }
        public string GetFinancialIndicatorsHtml(string company)
        {
            return GetHtml(GetFinancialIndicatorsUrl(company));
        }

        private string GetProfitIndicatorsUrl(string company)
        {
            return @"http://www.biznesradar.pl/wskazniki-rentownosci/" + company;
        }
        public string GetProfitIndicatorsHtml(string company)
        {
            return GetHtml(GetProfitIndicatorsUrl(company));
        }

        private string GetMainPageUrl(string biznesRadarName)
        {
            return "http://www.biznesradar.pl/notowania/" + biznesRadarName;
        }
        private string GetArchiveListeningUrl(string company, int page = 1)
        {
            string url = "http://www.biznesradar.pl/notowania-historyczne/" + company;
            if (page > 1)
            {
                url += "," + page;
            }
            return url;
        }

        public string GetFinancialIncomeReportHtml(string company)
        {
            return GetHtml(GetFinancialIncomeReportUrl(company));
        }
        private string GetFinancialIncomeReportUrl(string company)
        {
            return $"http://www.biznesradar.pl/raporty-finansowe-rachunek-zyskow-i-strat/{company},Q";
        }
        public string GetFinancialBalanceReportHtml(string company)
        {
            return GetHtml(GetFinancialBalanceReportUrl(company));
        }
        private string GetFinancialBalanceReportUrl(string company)
        {
            return $"http://www.biznesradar.pl/raporty-finansowe-bilans/{company},Q,0";
        }
        public string GetFinancialCashflowReportHtml(string company)
        {
            return GetHtml(GetFinancialCashflowReportUrl(company));
        }
        private string GetFinancialCashflowReportUrl(string company)
        {
            return $"http://www.biznesradar.pl/raporty-finansowe-przeplywy-pieniezne/{company},Q";
        }

    }
}
