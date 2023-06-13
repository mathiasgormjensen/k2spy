using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy.ExtensionMethods
{
    public static class CategoryManagerExtensionMethods
    {
        public static SourceCode.Categories.Client.Category GetCategoryById(this SourceCode.Categories.Client.CategoryManager that, int categoryId)
        {
            foreach (SourceCode.Categories.Client.Category category in that.Categories)
            {
                if (category.Id == categoryId)
                    return category;
                SourceCode.Categories.Client.Category match = category.GetCategoryById(categoryId);
                if (match != null)
                    return match;
            }
            return null;
        }
    }
}
