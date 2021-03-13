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
        public static bool ValidateCreateEncodingRequest(CreateEncodingRequest reqObj)
        {
            //check isEmpty
            if (reqObj.image.Length == 0 || reqObj.knownPersonEmail.Length == 0)
            {
                return false;
            }
            //check Regex for email
            string pattern = "@^[A-Za-z0-9_.]{2,}@[A-Za-z0-9]{2,}(.[A-Za-z0-9]{2,}){1,2}$";

            return true;
        }
    }
}
