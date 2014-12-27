using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess;

namespace Repository
{
    public class CategoriesRepository: Connection
    {
        public CategoriesRepository()
            : base()
        {

        }

        public CategoriesRepository(DBTradersMarketEntities entity)
            : base(entity)
        {

        }

        public void GetAllCategories(ref List<Category> MainCat, ref List<Category> SubCat)
        {
            MainCat.Clear();
            SubCat.Clear();

            MainCat = entity.Categories.Where(x => x.ParentID == null).ToList();
            SubCat = entity.Categories.Where(x => x.ParentID != null).ToList();
        }

        public string GetCategoryName(int CategoryID)
        {
            var singleOrDefault = entity.Categories.SingleOrDefault(x => x.CategoryID == CategoryID);

            return singleOrDefault != null ? singleOrDefault.Name : "";
        }
    }
}
