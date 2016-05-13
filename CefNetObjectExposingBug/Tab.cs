/*
 * Created by Tamas Monus
 *
 * This software is confidential and proprietary information of CAS
 * Software AG. You shall not disclose such Confidential Information
 * and shall use it only in accordance with the terms of the license
 * agreement you entered into with CAS Software AG.
 *
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using SmartDesign.TabControl.Interface;

namespace CefNetObjectExposingBug
{
    public class Tab : ObservableObject, IResumable
    {
        #region Name

        private string _name;

        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        #endregion

        #region BrowserUrls

        private List<string> _browserUrls;

        public List<string> BrowserUrls
        {
            get { return _browserUrls; }
            set { Set(ref _browserUrls, value); }
        }

        #endregion

        public Tab()
        {
            _browserUrls = new List<string>
            {
                "sample://sample.html",
                "sample://sample.html",
                "sample://sample.html",
                "sample://sample.html"
            };
        }

        public async Task Suspend()
        {
            
        }

        public void Resume()
        {
            
        }

        public bool Suspended { get; set; }
    }
}