using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess;

namespace BL
{
    public class TransactionBL
    {
        /// <summary>
        /// This will checkout all the products in the cart
        /// </summary>
        /// <param name="username">The username to checkout</param>
        /// <param name="ItemsAdded">The Items added to the order</param>
        /// <param name="OrderID">The OrderID</param>
        /// <returns>The checkout status</returns>
        public GlobalDefines.CheckOut Checkout(string username, ref  List<TransactionDetail> ItemsAdded, ref int OrderID)
        {
            TransactionRepository trans = new TransactionRepository();
            trans.entity.Connection.Open();
            trans.DbTransaction = trans.entity.Connection.BeginTransaction();
            GlobalDefines.CheckOut chk;
            try
            {

                List<TransactionDetail> Items = new List<TransactionDetail>();
                Transaction transaction = new Transaction();
                List<ShoppingCart> shoppingcart = new ShoppinCartRepository().GetShoppingCartItems(username).ToList();
                DateTime date = DateTime.Now;
                transaction.Username = username;
                transaction.Date = date;
                transaction.Payed = false;
                for (int i = 0; i < shoppingcart.Count(); i++)
                {
                    TransactionDetail detail = new TransactionDetail
                    {
                        ProductID = shoppingcart[i].ProductID,
                        Price = shoppingcart[i].Product.Price,
                        Qty = shoppingcart[i].Qty
                    };

                    Items.Add(detail);

                }
                bool Itemadded = false;
                chk = trans.Checkout(Items, transaction, ref Itemadded,ref ItemsAdded);

                if (chk == GlobalDefines.CheckOut.ERROR)
                {
                    throw new Exception();
                }

                if (!Itemadded)
                {
                    trans.DbTransaction.Rollback();
                    chk = GlobalDefines.CheckOut.NOITEMWASADDED;
                }
                else
                {

                    trans.DbTransaction.Commit();
                }
            }
            catch (Exception ex)
            {
                trans.DbTransaction.Rollback();
                chk = GlobalDefines.CheckOut.ERROR;
            }
            finally
            {
                trans.entity.Connection.Close();

            }

            return chk;
        }


        /// <summary>
        /// Gets the transactions details by ID
        /// </summary>
        /// <param name="TransactionId">The transaction ID</param>
        /// <returns>list of transaction details</returns>
        public IEnumerable<TransactionDetail> GetTransactionDetails(int TransactionId)
        {
            return new TransactionRepository().GetTransactionDetails(TransactionId);
        }


        /// <summary>
        /// Gets the transactions details by Username
        /// </summary>
        /// <param name="Username">The username</param>
        /// <returns>list of transaction details</returns>
        public IEnumerable<TransactionDetail> GetTransactionDetailsByUsername(string Username)
        {
            return new TransactionRepository().GetTransactionDetailsByUsername(Username);
        }

        /// <summary>
        /// Gets the transactions by Username
        /// </summary>
        /// <param name="Username">The username</param>
        /// <returns>list of transactions</returns>
        public IEnumerable<Transaction> GetTransactionForUser(string Username)
        {
            return new TransactionRepository().GetTransactionForUser(Username);
        }


        /// <summary>
        /// Gets the transactions between two dates
        /// </summary>
        /// <param name="Username">the username</param>
        /// <param name="To">The to date</param>
        /// <param name="From">The from date</param>
        /// <returns>list of transactions</returns>
        public IEnumerable<Transaction> GetTransactionBetweenDates(string Username, DateTime To, DateTime From)
        {
            return new TransactionRepository().GetTransactionBetweenDates(Username, To, From);
        }


        public IEnumerable<Transaction> GetPendingTransactions()
        {
            return new TransactionRepository().GetPendingTransactions();
        }

        public bool ApprovePament(int TransactionId)
        {
            return new TransactionRepository().ApprovePament(TransactionId);
        }

        /// <summary>
        /// Checks if user bought a product
        /// </summary>
        /// <param name="Username">the username</param>
        /// <param name="ProductID"></param>
        /// <returns>true if he bought product</returns>
        public bool didUserBuyProduct(string Username, int ProductID)
        {
            return new TransactionRepository().didUserBuyProduct(Username, ProductID);
        }
    }
}
