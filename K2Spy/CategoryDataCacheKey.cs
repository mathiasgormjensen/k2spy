using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Spy
{
    public class CategoryDataCacheKey
    {
        public CategoryDataCacheKey(int categoryId, string data)
        {
            this.CategoryId = categoryId;
            this.Data = data;
        }

        public int CategoryId { get; private set; }

        public string Data { get; private set; }

        public override int GetHashCode()
        {
            return $"CategoryDataCacheKey, {this.CategoryId}, {this.Data}".GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is CategoryDataCacheKey that)
            {
#if true
                if (this.GetHashCode() == that.GetHashCode())
                {
                    return true;
                }
#else
                if (this.CategoryId == that.CategoryId)
                {
                    if (this.Data == that.Data)
                    {
                        return true;
                    }
                }
#endif
            }
            return base.Equals(obj);
        }
    }
}
