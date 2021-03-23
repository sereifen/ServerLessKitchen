using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainWs
{
    public class RecipeCount
    {
        public int id { set; get; }
        public int count { set; get; }

        /// <summary>
        /// constructor with ID
        /// </summary>
        /// <param name="id">Id from recipe</param>
        public RecipeCount(int id)
        {
            this.count = 0;
            this.id = id;
        }

    }
}
