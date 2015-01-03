using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Repository;

namespace DataAccess
{
    public class TransactionRepository : Connection
    {
        public TransactionRepository()
            : base()
        {

        }

        public TransactionRepository(DBTradersMarketEntities entity)
            : base(entity)
        {

        }

        public GlobalDefines.CheckOut Checkout(List<TransactionDetail> Items, Transaction transaction, ref bool ItemAdded, ref List<TransactionDetail> listItemsAdded)
        {
            
            
            GlobalDefines.CheckOut chk = GlobalDefines.CheckOut.ALLCHECKOUT;
            try
            {

                for (int i = 0; i < Items.Count(); i++)
                {
                    ShoppingCart shoppingcart = new ShoppinCartRepository(entity).GetItem(transaction.Username, Items[i].ProductID);

                    if (new ProductsRepository().ReduceQuantity(Items[i].ProductID, Items[i].Qty))
                    {
                        transaction.TransactionDetails.Add(Items[i]);
                        entity.ShoppingCarts.DeleteObject(shoppingcart);
                        ItemAdded = true;

                        listItemsAdded.Add(Items[i]);
                    }
                    else
                    {
                        chk = GlobalDefines.CheckOut.NOTENOUGHSTOCK;
                    }
                }
                
                entity.Transactions.AddObject(transaction);
                entity.SaveChanges();
                return chk;
            }
            catch (Exception ex)
            {
                return GlobalDefines.CheckOut.ERROR;
            }
        }


        public void AddTransaction(Transaction transaction)
        {
            entity.Transactions.AddObject(transaction);
            entity.SaveChanges();
        }

        public IEnumerable<TransactionDetail> GetTransactionDetails(int TransactionId)
        {
            return entity.TransactionDetails.Where(x => x.TransactionID == TransactionId).AsEnumerable();
        }


        public IEnumerable<TransactionDetail> GetTransactionDetailsByUsername(string Username)
        {
            return entity.TransactionDetails.Where(x => x.Transaction.Username == Username).AsEnumerable();
        }

        public IEnumerable<Transaction> GetTransactionForUser(string Username)
        {
            return entity.Transactions.Where(x => x.Username == Username).AsEnumerable();
        }

        public bool didUserBuyProduct(string Username, int ProductID)
        {
            return entity.TransactionDetails.Count(x => x.Transaction.Username == Username && x.ProductID == ProductID) != 0;
        }

        public IEnumerable<Transaction> GetTransactionBetweenDates(string Username, DateTime To, DateTime From)
        {
            return entity.Transactions.Where(x => x.Username == Username && (x.Date >= From && x.Date <= To)).AsEnumerable();
        }


        public IEnumerable<Transaction> GetPendingTransactions()
        {
            return entity.Transactions.Where(x => !x.Payed);
        }

        public bool ApprovePament(int TransactionId)
        {
           Transaction trans = entity.Transactions.SingleOrDefault(x => x.TransactionID == TransactionId);

            if (trans != null) 
                trans.Payed = true;

            int ret = entity.SaveChanges();

            return ret >= 1;
        }
    }
    
}
