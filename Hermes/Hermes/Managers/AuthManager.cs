﻿using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hermes
{
    public class AuthManager
    {
        private FirebaseAuth mAuth;
        public AuthManager()
        {
            mAuth = FirebaseAuth.Instance;
        }
        

        public bool SignIn(UserData u)
        {
            Task signin = mAuth.SignInWithEmailAndPassword(u.Email, u.Password);
            try
            {
                Thread thr = new Thread(new ThreadStart(delegate { Android.Gms.Tasks.TasksClass.Await(signin); }));
                thr.Start();
                thr.Join();
                return signin.IsSuccessful;
            }
            catch
            {
                return false;
            }

        }
        public bool SignUp(UserData u)
        {
            Task signup = mAuth.CreateUserWithEmailAndPassword(u.Email, u.Password);
            try
            {
                Thread thr = new Thread(new ThreadStart(delegate { Android.Gms.Tasks.TasksClass.Await(signup); return; }));
                thr.Start();
                thr.Join();

                if (signup.IsSuccessful)
                {
                    if (SignIn(u))
                    {
                        App.UserManager.RegisterUsername(CurrentUserUid, u.Username);

                    }
                }
                return signup.IsSuccessful;
            }
            catch
            {
                return false;
            }
        }
        
        public string CurrentUserUid { get => mAuth.CurrentUser.Uid; }
    }

}