using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock
{
    public class TaxCalculator
    {
        List<ITransaction> TransactionsInYear;
        List<ITransaction> AllTransactions;
        List<PositionTransaction> AllPositionTransactions;
        List<TransactionsPair> TransactionsPair = new List<TransactionsPair>();
        public TaxCalculator(int year, List<ITransaction> Transactions)
        {
            this.AllTransactions = new List<ITransaction>(Transactions);
            this.TransactionsInYear = new List<ITransaction>(Transactions.Where(t => t.GetDateTime().Year == year));
            this.AllPositionTransactions = new List<PositionTransaction>(AllTransactions.Where(t => t is PositionTransaction).ToList().ConvertAll(x => (PositionTransaction)x));
            FillTransactionsPairs();
        }

        public decimal GetTaxValue()
        {
            decimal commisions = TransactionsInYear.Where(t => t is CommisionTransaction).Sum(t => t.GetValue());
            return Math.Max(TransactionsPair.Sum(tp => tp.Tax) - commisions, 0);
        }

        private void FillTransactionsPairs()
        {
            List<PositionTransaction> allPositionTransactionsCopy = new List<PositionTransaction>(AllPositionTransactions);

            foreach (SellTransaction sellTrans in TransactionsInYear.Where(t => t is SellTransaction).OrderBy(t => t.GetDateTime()))
            {
                
                TransactionsPair pair = new TransactionsPair();
                pair.SellTransaction = sellTrans;
                allPositionTransactionsCopy.Remove(sellTrans);

                foreach (BuyTransaction buyTransaction in allPositionTransactionsCopy
                                                          .Where(t => t is BuyTransaction 
                                                                   && t.GetDateTime() < sellTrans.GetDateTime() 
                                                                   && t.Position.GetSymbol() == sellTrans.Position.GetSymbol())
                                                          .ToList().ConvertAll(x => (BuyTransaction)x))
                {
                    decimal amountRemain = sellTrans.Amount - pair.BuyTransactions.Sum(b => b.Amount);
                    if (amountRemain == 0)
                    {
                        break;
                    }
                    if (buyTransaction.Amount == amountRemain)
                    {
                        pair.BuyTransactions.Add(buyTransaction);
                        allPositionTransactionsCopy.Remove(buyTransaction);
                    }
                    else if (buyTransaction.Amount >= amountRemain)
                    {
                        allPositionTransactionsCopy.Remove(buyTransaction);

                        List<BuyTransaction> divided = buyTransaction.Divide(amountRemain).ConvertAll(x => (BuyTransaction)x);
                        BuyTransaction transToPair = divided.First(t => t.Amount == amountRemain);
                        divided.Remove(transToPair);

                        pair.BuyTransactions.Add(transToPair);
                        allPositionTransactionsCopy.Add(divided.First());
                    }
                    else if (buyTransaction.Amount <= amountRemain)
                    {
                        pair.BuyTransactions.Add(buyTransaction);
                        allPositionTransactionsCopy.Remove(buyTransaction);
                    }
                }

                TransactionsPair.Add(pair);
            }

        }
    }
}
