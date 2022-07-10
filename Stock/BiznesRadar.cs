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

        private string financialIndicators;
        public string GetFinancialIndicatorsHtml(string company)
        {
            if (string.IsNullOrEmpty(financialIndicators))
            {
                financialIndicators = GetHtml(GetFinancialIndicatorsUrl(company));
            }
            return financialIndicators;
        }

        private string GetProfitIndicatorsUrl(string company)
        {
            return @"http://www.biznesradar.pl/wskazniki-rentownosci/" + company;
        }
        private string profitIndicators;
        public string GetProfitIndicatorsHtml(string company)
        {
            if (string.IsNullOrEmpty(profitIndicators))
            {
                profitIndicators = GetHtml(GetProfitIndicatorsUrl(company));
            }
            return profitIndicators;
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
        string financialIncomeReport;
        public string GetFinancialIncomeReportHtml(string company)
        {
            if (string.IsNullOrEmpty(financialIncomeReport))
            {
                financialIncomeReport = GetHtml(GetFinancialIncomeReportUrl(company));
            }
            return financialIncomeReport;
        }
        private string GetFinancialIncomeReportUrl(string company)
        {
            return $"http://www.biznesradar.pl/raporty-finansowe-rachunek-zyskow-i-strat/{company},Q";
        }
        string financialBalanceReport;
        public string GetFinancialBalanceReportHtml(string company)
        {
            if (string.IsNullOrEmpty(financialBalanceReport))
            {
                financialBalanceReport = GetHtml(GetFinancialBalanceReportUrl(company));
            }
            return financialBalanceReport;
        }
        private string GetFinancialBalanceReportUrl(string company)
        {
            return $"http://www.biznesradar.pl/raporty-finansowe-bilans/{company},Q,0";
        }
        string financialCashFlowReport;
        public string GetFinancialCashflowReportHtml(string company)
        {
            if (string.IsNullOrEmpty(financialCashFlowReport))
            {
                financialCashFlowReport = GetHtml(GetFinancialCashflowReportUrl(company));
            }
            return financialCashFlowReport;
        }
        private string GetFinancialCashflowReportUrl(string company)
        {
            return $"http://www.biznesradar.pl/raporty-finansowe-przeplywy-pieniezne/{company},Q";
        }

    }
}
