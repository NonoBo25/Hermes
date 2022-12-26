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
    public class CommunicationBinder: Binder
    {
        public CommunicationBinder(CommunicationService service)
        {
            this.Service = service;
        }
        public CommunicationService Service { get; private set; }
    }
}