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
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CefSharp;
using Cookie = CefSharp.Cookie;

namespace CefNetObjectExposingBug
{
    public class SampleResourceHandler : IResourceHandler
    {
        private Stream _responseStream;
        private string _mimeType;

        public bool ProcessRequest(IRequest request, ICallback callback)
        {
            var uri = new Uri(request.Url);
            var filePath = Path.Combine(Environment.CurrentDirectory, uri.Host + uri.AbsolutePath.Replace("/", "\\")).TrimEnd('\\');
            if (File.Exists(filePath))
            {
                Task.Run(() =>
                {
                    var fileExtension = Path.GetExtension(filePath);
                    _mimeType = ResourceHandler.GetMimeType(fileExtension);
                    _responseStream = File.OpenRead(filePath);
                    callback.Continue();
                });
                return true;
            }

            callback.Dispose();
            return false;
        }

        public void GetResponseHeaders(IResponse response, out long responseLength, out string redirectUrl)
        {
            responseLength = _responseStream == null ? 0 : _responseStream.Length;
            redirectUrl = null;

            response.StatusCode = (int)HttpStatusCode.OK;
            response.StatusText = "OK";
            response.MimeType = _mimeType;
        }

        public bool ReadResponse(Stream dataOut, out int bytesRead, ICallback callback)
        {
            callback.Dispose();
            var buffer = new byte[dataOut.Length];

            bytesRead = _responseStream.Read(buffer, 0, buffer.Length);
            dataOut.Write(buffer, 0, buffer.Length);

            return bytesRead > 0;
        }

        public bool CanGetCookie(Cookie cookie)
        {
            return true;
        }

        public bool CanSetCookie(Cookie cookie)
        {
            return true;
        }

        public void Cancel()
        {

        }
    }
}