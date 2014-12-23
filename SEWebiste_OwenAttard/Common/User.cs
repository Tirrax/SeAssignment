//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Common
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.ShoppingCarts = new HashSet<ShoppingCart>();
            this.Transactions = new HashSet<Transaction>();
            this.Roles = new HashSet<Role>();
        }
    
        public string Username { get; set; }
        public string Password { get; set; }
        public string Pin { get; set; }
        public string Code { get; set; }
        public System.DateTime LastCodeDate { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Town { get; set; }
        public string Country { get; set; }
        public string MobileNumber { get; set; }
        public bool SellerApproved { get; set; }
    
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
    }
}
