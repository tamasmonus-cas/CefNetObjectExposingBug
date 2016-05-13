/*
 * Created by Tamas Monus
 *
 * This software is confidential and proprietary information of CAS
 * Software AG. You shall not disclose such Confidential Information
 * and shall use it only in accordance with the terms of the license
 * agreement you entered into with CAS Software AG.
 *
 */

using CefSharp;

namespace CefNetObjectExposingBug
{
    public class SchemeHandlerFactory : ISchemeHandlerFactory
    {
        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            return new SampleResourceHandler();
        }
    }
}