using StrangerDetection.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StrangerDetection.Validators
{
    public class UserValidator
    {
        //validation
        public static bool ValidationRequestObjForCreateAccount(CreateAccountRequest model)
        {
            if (model.Username.Trim().Length == 0 || model.Password.Trim().Length == 0 ||
                model.FrontIdentityImage.Trim().Length == 0 ||
                model.BackIdentityImage.Trim().Length == 0 ||
                model.Image.Trim().Length == 0)
            {
                return false;
            }
            return true;
        }
    }
}
