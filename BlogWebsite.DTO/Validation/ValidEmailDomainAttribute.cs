using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.DTO.Validation
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string allowDomain;
        public ValidEmailDomainAttribute(string allowDomain)
        {
            this.allowDomain = allowDomain;
        }
        public override bool IsValid(object? value)
        {
            string[] strings = value.ToString().Split('@');
            return strings[1].ToUpper() == allowDomain.ToUpper();

            //return base.IsValid(value);
        }
    }
}
