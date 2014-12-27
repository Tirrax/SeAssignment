using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Repository;

namespace BL
{
    public class CategoriesBL
    {
        public void GetAllCategories(ref List<Category> MainCat, ref List<Category> SubCat)
        {
            new CategoriesRepository().GetAllCategories(ref MainCat, ref SubCat);
        }

        public string GetCategoryName(int CategoryID)
        {
            return new CategoriesRepository().GetCategoryName(CategoryID);
        }

    }
}
