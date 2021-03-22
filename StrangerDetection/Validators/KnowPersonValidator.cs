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
        public static bool ValidateCreateKnowPersonRequestObj(CreateKnownPersonRequest req)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(req.email);
            if (match.Success && req.name.Length != 0)
            {
                return true;
            }
            return false;
        }

        public static bool ValidateUpdateKnowPersonRequestObj(UpdateKnownPersonRequest req)
        {
            if (req.name.Length != 0)
            {
                return true;
            }
            return false;
        }

    }
}
