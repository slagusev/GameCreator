using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace GameCreator
{
    public class GC_Object : PropertyChangedBase
    {
        #region "Properties"
        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { SetProperty(value, ref _Name); }
        }
        private GC_Class _Class;
        public GC_Class Class
        {
            get { return _Class; }
            set { SetProperty(value, ref _Class); }
        }
        private GC_Image _Image;
        public GC_Image Image
        {
            get { return _Image; }
            set { SetProperty(value, ref _Image); }
        }

        #endregion
        public GC_Object(string Name)
        {
            this.Name = Name;
        }

    }
}