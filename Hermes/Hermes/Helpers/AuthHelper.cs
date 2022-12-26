using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hermes
{

    public static class AuthHelper
    {
        public enum UserVerificationRes { InvalidEmail, InvalidPassword, Ok };
        public class UserVerificationResult
        {
            public UserVerificationRes Result { get; set; }
            public static bool operator true(UserVerificationResult x) => x.Result==UserVerificationRes.Ok;
            public static bool operator false(UserVerificationResult x) => x.Result != UserVerificationRes.Ok;
            public override string ToString()
            {
                switch (Result)
                {
                    case UserVerificationRes.InvalidEmail:
                        return "Invalid Email!";                   
                    case UserVerificationRes.InvalidPassword:
                        return "Invalid Password!";                     
                    case UserVerificationRes.Ok:
                        return "Valid User Data";                    
                    default:
                        return "Invalid User Data";
                }
            }
        }
        
        public static UserVerificationResult VerifyUserData(UserData u)
        {
            if (!(TextHelper.IsValidString(u.Email) && TextHelper.IsValidEmail(u.Email))) {
                return new UserVerificationResult{ Result= UserVerificationRes.InvalidEmail};
            }
            if (!TextHelper.IsValidString(u.Password))
            {
                return new UserVerificationResult { Result = UserVerificationRes.InvalidPassword };
            }
            return new UserVerificationResult { Result = UserVerificationRes.Ok };
        }
    }
}