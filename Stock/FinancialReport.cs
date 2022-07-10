using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class FinancialReport
    {
        [DatabaseField]
        public string ISIN;

        [DatabaseField]
        public string Period;

        [DatabaseField]
        public DateTime PrimaryReport;

        /* RACHUNEK ZYSKÓW I STRAT */

        /* Przychody ze sprzedaży */
        [DatabaseField]
        public Int64 IncomeRevenues;

        /* Techniczny koszt wytworzenia produkcji sprzedanej */
        [DatabaseField]
        public Int64 IncomeCostOfSales;

        /* Koszt sprzedaży */
        [DatabaseField]
        public Int64 IncomeDistributionExpenses;

        /* Koszty ogólnego zarządu */
        [DatabaseField]
        public Int64 IncomeAdministrativExpenses;

        /* Zysk ze sprzedaży */
        [DatabaseField]
        public Int64 IncomeGrossProfit;

        /* Pozostałe przychody operacyjne */
        [DatabaseField]
        public Int64 IncomeOtherOperatingIncome;

        /* Pozostałe koszty operacyjne */
        [DatabaseField]
        public Int64 IncomeOtherOperatingCosts;

        /* Zysk operacyjny EBIT */
        [DatabaseField]
        public Int64 IncomeEBIT;

        /* Przychody finansowe */
        [DatabaseField]
        public Int64 IncomeFinanceIncome;

        /* Koszty finansowe */
        [DatabaseField]
        public Int64 IncomeFinanceCosts;

        /* Pozostałe przychody(koszty) */
        [DatabaseField]
        public Int64 IncomeOtherIncome;

        /* Zysk z działalnośc gospodarczej */
        [DatabaseField]
        public Int64 IncomeNetGrossProfit;

        /* Wynik zdarzeń nadzwyczajnych */
        [DatabaseField]
        public Int64 IncomeExtraordinarProfit;

        /* Zysk przed opodatkowaniem */
        [DatabaseField]
        public Int64 IncomeBeforeTaxProfit;

        /* Zysk(strata) netto z działalności zaniechanej */
        [DatabaseField]
        public Int64 IncomeDiscontinuedProfit;

        /* Zysk netto */
        [DatabaseField]
        public Int64 IncomeNetProfit;

        /* Zysk netto akcjonariuszy jednostki dominującej */
        [DatabaseField]
        public Int64 IncomeShareholderNetProfit;



        /* BILANS */

        /* Aktywa trwałe */
        [DatabaseField]
        public Int64 BalanceNoncurrentAssets;

        /* Wartości niematerialne i prawne */
        [DatabaseField]
        public Int64 BalanceIntangibleAssets;

        /* Rzeczowe składniki majątku trwałego */
        [DatabaseField]
        public Int64 BalanceProperty;

        /* Należności długoterminowe */
        [DatabaseField]
        public Int64 BalanceNoncurrentReceivables;

        /* Inwestycje długoterminowe */
        [DatabaseField]
        public Int64 BalanceNoncurrInvestments;

        /* Pozostałe aktywa trwałe */
        [DatabaseField]
        public Int64 BalanceOtherNoncurrentAssets;

        /* Aktywa obrotowe */
        [DatabaseField]
        public Int64 BalanceCurrentAssets;

        /* Zapasy */
        [DatabaseField]
        public Int64 BalanceInventory;

        /* Należności krótkoterminowe */
        [DatabaseField]
        public Int64 BalanceCurrentReceivables;

        /* Inwestycje krótkoterminowe */
        [DatabaseField]
        public Int64 BalanceCurrentInvestments;

        /* Środki pieniężne i inne aktywa pieniężne */
        [DatabaseField]
        public Int64 BalanceCash;

        /* Pozostałe aktywa obrotowe */
        [DatabaseField]
        public Int64 BalanceOtherCurrentAssets;

        /* Aktywa trwałe przeznaczone do sprzedaży */
        [DatabaseField]
        public Int64 BalanceAssetsForSale;

        /* Aktywa razem */
        [DatabaseField]
        public Int64 BalanceTotalAssets;

        /* Kapitał własny akcjonariuszy jednostki dominującej */
        [DatabaseField]
        public Int64 BalanceCapital;

        /* Kapitał(fundusz) podstawowy */
        [DatabaseField]
        public Int64 BalanceShareCapital;

        /* Udziały(akcje) własne */
        [DatabaseField]
        public Int64 BalanceOwnShares;

        /* Kapitał(fundusz) zapasowy */
        [DatabaseField]
        public Int64 BalanceReserve;

        /* Udziały niekontrolujące */
        [DatabaseField]
        public Int64 BalanceNonshareCapital;

        /* Zobowiązania długoterminowe */
        [DatabaseField]
        public Int64 BalanceNoncurrentLiabilities;

        /* Z tytułu dostaw i usług */
        [DatabaseField]
        public Int64 BalanceNoncurrentTradeLiabilities;

        /* Kredyty i pożyczki */
        [DatabaseField]
        public Int64 BalanceNoncurrentBorrowings;

        /* Z tytułu emisji dłużnych papierów wartościowych */
        [DatabaseField]
        public Int64 BalanceNoncurrentObligations;

        /* Zobowiązania z tytułu leasingu finansowego */
        [DatabaseField]
        public Int64 BalanceNoncurrentLeasing;

        /* Inne zobowiązania długoterminowe */
        [DatabaseField]
        public Int64 BalanceNoncurrentOtherLiabilities;

        /* Zobowiązania krótkoterminowe */
        [DatabaseField]
        public Int64 BalanceCurrentLiabilities;

        /* Z tytułu dostaw i usług */
        [DatabaseField]
        public Int64 BalanceCurrentTradePayablesv;

        /* Kredyty i pożyczki */
        [DatabaseField]
        public Int64 BalanceCurrentBorrowings;

        /* Z tytułu emisji dłużnych papierów wartościowych */
        [DatabaseField]
        public Int64 BalanceCurrentObligations;

        /* Zobowiązania z tytułu leasingu finansowego */
        [DatabaseField]
        public Int64 BalanceCurrentLeasing;

        /* Inne zobowiązania krótkoterminowe */
        [DatabaseField]
        public Int64 BalanceCurrentOtherLiabilities;

        /* Rozliczenia międzyokresowe */
        [DatabaseField]
        public Int64 BalanceReckoning;

        /* Pasywa razem */
        [DatabaseField]
        public Int64 BalanceTotalEquityAndLiabilities;



        /* PRZEPŁYWY PIENIĘŻNE */

        /* Przepływy pieniężne z działalności operacyjnej */
        [DatabaseField]
        public Int64 CashflowOperatingCashflow;

        /* Amortyzacja */
        [DatabaseField]
        public Int64 CashflowAmortization;

        /* Przepłyzy pieniężne z działalności inwestycyjnej */
        [DatabaseField]
        public Int64 CashflowInvestingCashflow;

        /* CAPEX */
        [DatabaseField]
        public Int64 CashflowCapex;

        /* Przepływy z działalności finansowej */
        [DatabaseField]
        public Int64 CashflowFinancingCashflow;

        /* Emisja akcji */
        [DatabaseField]
        public Int64 CashflowShareCapitalCash;

        /* Dywidenda */
        [DatabaseField]
        public Int64 CashflowDividend;

        /* Przepływy pieniężne razem */
        [DatabaseField]
        public Int64 CashflowNetCashflow;

        [DatabaseField]
        public Int64 ShareAmount;

        [DatabaseField]
        public decimal ROE;

        [DatabaseField]
        public decimal ROA;

        /* Marża zysku operacyjnego */
        [DatabaseField]
        public decimal OPM;

        /* Marża zysku netto */
        [DatabaseField]
        public decimal ROS;

        /* Marża zysku ze sprzedaży */
        [DatabaseField]
        public decimal RS;

        /* Marża zysku brutto */
        [DatabaseField]
        public decimal GPM;

        /* Marża zysku brutto ze sprzedaży */
        [DatabaseField]
        public decimal RSB;

        /* Rentowność operacjna aktywów */
        [DatabaseField]
        public decimal ROPA;

        public string GetSQLInsert()
        {
            return SqlBuilder.GetInsertTableQuery(this);
        }
    }
}
