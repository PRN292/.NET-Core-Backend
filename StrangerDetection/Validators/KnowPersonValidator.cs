using StrangerDetection.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StrangerDetection.Validators
{
    public class KnowPersonValidator
    {
        public static bool ValidateCreateKnowPersonRequestObj(string name, string email)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success && name.Length != 0)
            {
                return true;
            }
            return false;
        }

    }
}
