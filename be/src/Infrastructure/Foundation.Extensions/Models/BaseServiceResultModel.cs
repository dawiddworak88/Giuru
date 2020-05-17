using System.Collections.Generic;
using System.Linq;

namespace Foundation.Extensions.Models
{
    public class BaseServiceResultModel
    {
        public BaseServiceResultModel()
        {
            this.Errors = new List<string>();
        }

        public List<string> Errors { get; set; }

        public bool IsValid
        {
            get
            {
                return !this.Errors.Any();
            }
        }
    }
}
