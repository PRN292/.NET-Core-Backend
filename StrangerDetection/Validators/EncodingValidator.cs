using StrangerDetection.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace StrangerDetection.Validators
{
    public class EncodingValidator
    {
        public static bool ValidateCreateEncodingRequest(string email, string image)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (match.Success && image.Length != 0)
            {
                return true;
            }
            return false;
        }

        
    }
}
