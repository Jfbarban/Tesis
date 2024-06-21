using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NexCloudClient.Scraping;
using Tesis.Models.NextCloudClient.Scraping;



namespace Tesis.Models.NextCloudClient
{
    public class NexxCloudClient
    {


        #region fileds

        private CookieContainer _cookieContainer;
        public WebProxy pproxy;
        private HttpClientHandler _httpHandler;
        private HttpClient _httpClient;
        HttpClient cone;
        private object cookies;
        string urlretorno = "";
        string status = "";
        string shareid = "";
        private string sms = "";
        private string Tokenlogin = "";
        private bool smsactive;
        bool proxy;
        #endregion

        #region propierties
        public string GetTokenLogin
        {

            get { return Tokenlogin; }
            set
            {
                Tokenlogin = value;

            }
        }
        public bool SmsResultActive
        {

            get { return smsactive; }
            set
            {
                smsactive = value;

            }
        }
        public string SmsResult
        {
            get { return sms; }
            set
            {
                sms = value;

            }
        }
        public string Getshareid
        {

            get { return shareid; }
            set
            {
                shareid = value;

            }
        }
        public string Host { get; set; } = "https://nube.uclv.cu/";

        #endregion




        public async Task<HttpResponseMessage> SyncDbRootAsync(string path)
        {
            path = "FileExpress.s3db";

            string request = "https://xaviapacscloud.xutil.cu//remote.php/webdav/" + path;
            var bd = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "XAVIAPACSStation", "DB") + "\\0-FileExpress.s3db";


            HttpResponseMessage task = await _httpClient.GetAsync(request);


            return task;

        }

        


        public NexxCloudClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            _cookieContainer = new CookieContainer();

            _httpHandler = new HttpClientHandler()
            {
                UseCookies = true,
                CookieContainer = _cookieContainer

            };
            _httpClient = new HttpClient(_httpHandler);
        }




        #region proxi

        WebProxy MakeProxyAutentication(string proxyHost, string proxyPort, string proxyUserName, string proxyPassword, bool needServerAuthentication, string serverUserName, string serverPassword)
        {

            //  create a proxy object
            var proxy = new WebProxy
            {

                Address = new Uri($"http://{proxyHost}:{proxyPort}"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,

                // *** These creds are given to the proxy server, not the web server ***
                Credentials = new NetworkCredential(
                    userName: proxyUserName,
                    password: proxyPassword)

            };


            // create a client handler which uses that proxy
            var httpClientHandler = new HttpClientHandler
            {

                UseCookies = true,
                CookieContainer = _cookieContainer,

                Proxy = proxy,
            };

            // Omit this part if you don't need to authenticate with the web server:
            if (needServerAuthentication)
            {
                httpClientHandler.PreAuthenticate = true;
                httpClientHandler.UseDefaultCredentials = false;

                // *** These creds are given to the web server, not the proxy server ***
                httpClientHandler.Credentials = new NetworkCredential(
                    userName: serverUserName,
                    password: serverPassword);
            }


            //  create the HTTP client object
            _httpClient = new HttpClient(handler: httpClientHandler, disposeHandler: true);


            return proxy;

        }


        



        #endregion




        //delete file from webdav with filename (need previous login)
        public async Task del_FilesAsync(string filename)
        {

            if (!_httpClient.DefaultRequestHeaders.Contains("requesttoken"))
            {
                _httpClient.DefaultRequestHeaders.Add("requesttoken", Tokenlogin);
            };


            var urldel = Host + "remote.php/webdav/" + filename;


            var response = await _httpClient.DeleteAsync(urldel);


            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                status = "b";
                //borrado

                //  var result = response.Content.ReadAsStringAsync();
            }



            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {

                //error de credenciales

                //  var result = response.Content.ReadAsStringAsync();
            }



            if (response.StatusCode == HttpStatusCode.NotFound)
            {

                //server of

                //  var result = response.Content.ReadAsStringAsync();
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {

                //Error en la ruta del fichero

                //  var result = response.Content.ReadAsStringAsync();
            }
        }

        //delete link
        public async Task<bool> DeleteShareLinkAsync(string fileid, string token)
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("requesttoken"))
            {
                _httpClient.DefaultRequestHeaders.Add("requesttoken", token);
            };

            var resp = await _httpClient.DeleteAsync(Host + "ocs/v2.php/apps/files_sharing/api/v1/shares/" + fileid);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }

            else
            {
                return false;
            }

        }

        //for remove password = ""
        public async Task PutPasswordProtectionAsync(string fileid, string token, string password)
        {

            //Request URL: https://xaviapacscloud.xutil.cu/ocs/v2.php/apps/password_policy/api/v1/generate
            // Request Method: GET


            string pass = password;

            Dictionary<string, string> formdata = new Dictionary<string, string>() {

                  {"password",password}


                    };


            FormUrlEncodedContent formContent = new FormUrlEncodedContent(formdata);
            if (!_httpClient.DefaultRequestHeaders.Contains("requesttoken"))
            {
                _httpClient.DefaultRequestHeaders.Add("requesttoken", token);
            };


            var resp = await _httpClient.PutAsync(Host + "ocs/v2.php/apps/files_sharing/api/v1/shares/" + fileid, formContent);

        }

        //for remove expiredate = ""
        public async Task PutExpirationDateAsync(string fileid, string token, string expiredate)
        {


            string date = expiredate;

            Dictionary<string, string> formdata = new Dictionary<string, string>() {

                  {"expireDate",date}


                    };


            FormUrlEncodedContent formContent = new FormUrlEncodedContent(formdata);
            if (!_httpClient.DefaultRequestHeaders.Contains("requesttoken"))
            {
                _httpClient.DefaultRequestHeaders.Add("requesttoken", token);
            };

            try
            {
                var resp = await _httpClient.PutAsync(Host + "ocs/v2.php/apps/files_sharing/api/v1/shares/" + fileid, formContent);
                if (resp.IsSuccessStatusCode)
                {

                }
                else
                {

                }
            }
            catch (Exception)
            {


            }



        }

        //for remove note = ""
        public async Task MakeNoteforDestinatary(string fileid, string token, string note)
        {





            Dictionary<string, string> formdata = new Dictionary<string, string>() {

                  {"note",note}


                    };


            FormUrlEncodedContent formContent = new FormUrlEncodedContent(formdata);
            if (!_httpClient.DefaultRequestHeaders.Contains("requesttoken"))
            {
                _httpClient.DefaultRequestHeaders.Add("requesttoken", token);
            };


            var resp = await _httpClient.PutAsync(Host + "ocs/v2.php/apps/files_sharing/api/v1/shares/" + fileid, formContent);



        }

        //get direct dowload link
        public async Task<string> get_share_linkAsync(string filename, string token)
        {

            string shareType = "3";
            string path = "/" + filename;
            //  token = Tokenlogin;
            Dictionary<string, string> formdata = new Dictionary<string, string>() {

                  {"path",path},
                  {"shareType",shareType}

                    };


            FormUrlEncodedContent formContent = new FormUrlEncodedContent(formdata);
            if (!_httpClient.DefaultRequestHeaders.Contains("requesttoken"))
            {
                _httpClient.DefaultRequestHeaders.Add("requesttoken", token);
            };


            var resp = await _httpClient.PostAsync(Host + "ocs/v2.php/apps/files_sharing/api/v1/shares", formContent);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                //  status = "l";
                string html2 = await resp.Content.ReadAsStringAsync();

                //get share link
                Getshareid = Utils.stringBetween(html2, "<id>", "</id>");
                return urlretorno = Utils.stringBetween(html2, "<url>", "</url>");

            }

            return Host;

        }

        // check if the server is online
        public async Task<bool> conectadoAsync(string host)
        {

            bool result = false;

            bool RedActiva = System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();

            if (RedActiva)
                result = true;
            else
                return result = false;
            System.Uri Url = new System.Uri(host);


            return result;
        }


        //Autenticarse Metodo ****************************************

        public async Task<bool> LoginAsync(string user, string password, 
            string host, bool netproxy, string proxyip, string proxyport, string userproxy, string passproxy)
        {

            Host = host;
            _cookieContainer = new CookieContainer();


            //configure proxy


            if (netproxy)
            {
                pproxy = MakeProxyAutentication(proxyip, proxyport, userproxy, passproxy, false, "", "");
                proxy = true;
            }

            else

            {

                _httpHandler = new HttpClientHandler()
                {
                    UseCookies = true,
                    CookieContainer = _cookieContainer,


                };

                _httpClient = new HttpClient(_httpHandler);
                proxy = false;
            }

            string loginurl = host + "index.php/login";
            var requestTimeout = TimeSpan.FromSeconds(30);
            var httpTimeout = TimeSpan.FromSeconds(30);
            //  _httpClient.Timeout = httpTimeout;
            //        var stopwatch = Stopwatch.StartNew();


            if (conectadoAsync(host + "index.php/").Result)
            {
                try
                {

                    using (var tokenSource = new CancellationTokenSource(requestTimeout))
                    {
                        //var resp = await _httpClient.GetAsync(loginurl, tokenSource.Token);
                        var resp = await _httpClient.GetAsync(loginurl);

                        if (resp.StatusCode == HttpStatusCode.OK)
                        {
                            string html = await resp.Content.ReadAsStringAsync();

                            WebScraping scr = new WebScraping(html);

                            HtmlEtiq head = scr.Fin("head");
                            string requesttoken = head.GetAttrib("data-requesttoken");

                            string timezone = "America/Mexico_City";
                            string timezone_offset = "-5";

                            Dictionary<string, string> formdata = new Dictionary<string, string>()
                            {
                                {"user",user},
                                {"password",password},
                                {"timezone",timezone},
                                {"timezone_offset",timezone_offset},
                                {"requesttoken",requesttoken}

                            };

                            FormUrlEncodedContent formContent = new FormUrlEncodedContent(formdata);

                            resp = await _httpClient.PostAsync(loginurl, formContent);
                            if (resp.StatusCode == HttpStatusCode.OK)
                            {
                                html = await resp.Content.ReadAsStringAsync();
                                scr = new WebScraping(html);
                                HtmlEtiq title = scr.Fin("title");

                                bool a = (title.Content == "Dashboard - Nextcloud");
                                Host = host;

                                return a
                                    ;
                            }
                        }

                    }

                }
                catch (TaskCanceledException)
                {
                    //  Console.WriteLine($"Timed out after {stopwatch.Elapsed}");

                }
                return false;
            }

            else
            {
                return false;
            }

        }









        public class CookieWebClient : WebClient
        {
            public CookieContainer cookieContainer = new CookieContainer();

            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest request = base.GetWebRequest(address);
                HttpWebRequest webRequest = request as HttpWebRequest;
                if (webRequest != null)
                {
                    webRequest.CookieContainer = cookieContainer;

                }
                return request;
            }
        }

        public delegate void Progress(string filename, long index, long total, int speed, long time);
        public delegate void Finish(NexCloudFileResult result = null);


        public async Task<NexCloudFileResult> UploadFileAsync(string file, string path = "", Progress progress = null, Finish finish = null)
        {


            smsactive = true;


            string files = $"{Host}index.php/apps/files/";
            string nombreArchivo = System.IO.Path.GetFileName(file);
            string uploadUrl = $"{Host}remote.php/webdav/";
            if (path != "")
                uploadUrl += $"{path}/{nombreArchivo}";
            else
                uploadUrl += $"{nombreArchivo}";



            var resp = await _httpClient.GetAsync(files);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                string html = await resp.Content.ReadAsStringAsync();

                WebScraping scr = new WebScraping(html);

                HtmlEtiq head = scr.Fin("head");
                string requesttoken = head.GetAttrib("data-requesttoken");

                FileInfo fi = new FileInfo(file);

                if (fi.Exists)

                    using (Stream stream = File.OpenRead(file))
                    {
                        HttpContent content = new StreamContent(stream);



                        //check
                        content.Headers.Add("requesttoken", requesttoken);
                        NexCloudFileResult result = null;
                        CookieWebClient wc = new CookieWebClient();

                        //valores de progreso
                        TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                        int timestart = (int)t.TotalSeconds;
                        int clockstart = (int)t.TotalSeconds;
                        int timetotal = 0;
                        int speed = 0;
                        long time = 0;
                        long lastsend = 0;
                        wc.cookieContainer = _cookieContainer;
                        wc.Proxy = pproxy;
                        wc.Headers.Add("requesttoken", requesttoken);
                        wc.UploadProgressChanged += (s, data) => {

                            //progreso ui
                            FileInfo fi = new FileInfo(file);
                            int sendlen = (int)(data.BytesSent - lastsend);
                            speed += sendlen;
                            lastsend = data.BytesSent;
                            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                            int tcurrent = (int)t.TotalSeconds - timestart;
                            timetotal += tcurrent;
                            timestart = (int)t.TotalSeconds;
                            if (timetotal >= 1)
                            {

                                //actualización x segundo
                                int clocktime;
                                if (speed != 0)
                                {
                                    clocktime = (timetotal - sendlen) / (speed); // timepo q va a demorar en subir (segundos)
                                }

                                else
                                {
                                    clocktime = 0;
                                }

                                //llamar delegado
                                if (progress != null) progress.Invoke(fi.Name, data.BytesSent, data.TotalBytesToSend, speed, clocktime);
                                timetotal = 0;
                                speed = 0;
                            }
                        };
                        wc.UploadFileCompleted += async (s, data) => {

                            //termina o da error la subida
                            if (data.Error != null)
                            {

                                //un error al subir 
                            }
                            else
                            {
                                if (progress != null) progress.Invoke(fi.Name, fi.Length, fi.Length, speed, 0);

                                //crear resultado
                                var linkresult = await get_share_linkAsync(nombreArchivo, requesttoken);
                                Tokenlogin = requesttoken;
                                result = new NexCloudFileResult(fi.Name, uploadUrl, fi.Length, NexCloudState.UploadFinish, linkresult);
                                if (finish != null)
                                    finish.Invoke(result);
                            }
                        };
                        var upt = await wc.UploadFileTaskAsync(uploadUrl, "PUT", file);
                        //resp = await _httpClient.PutAsync(uploadUrl, content);

                        return result;

                        if (resp.StatusCode == HttpStatusCode.Created)
                        {
                            Tokenlogin = requesttoken;

                            var linkresult = await get_share_linkAsync(nombreArchivo, requesttoken);


                            status = "s";
                            return new NexCloudFileResult(fi.Name, uploadUrl, fi.Length, NexCloudState.UploadFinish, linkresult);
                        }


                        if (resp.StatusCode == HttpStatusCode.NoContent)
                        {

                            var linkresult = await get_share_linkAsync(nombreArchivo, requesttoken);

                            Tokenlogin = requesttoken;
                            status = "s";
                            return new NexCloudFileResult(fi.Name, uploadUrl, fi.Length, NexCloudState.UploadFileExist, linkresult);


                        }
                    }
            }


            return null;
        }





        #region download


        public async Task DownloadFileAsync(string url, IProgress<double> progress, CancellationToken token)
        {
            var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
            }

            var total = response.Content.Headers.ContentLength.HasValue ? response.Content.Headers.ContentLength.Value : -1L;
            var canReportProgress = total != -1 && progress != null;

            using (var stream = await response.Content.ReadAsStreamAsync())
            {
                var totalRead = 0L;
                var buffer = new byte[4096];
                var isMoreToRead = true;

                do
                {
                    token.ThrowIfCancellationRequested();

                    var read = await stream.ReadAsync(buffer, 0, buffer.Length, token);

                    if (read == 0)
                    {
                        isMoreToRead = false;
                    }
                    else
                    {
                        var data = new byte[read];
                        buffer.ToList().CopyTo(0, data, 0, read);

                        // TODO: put here the code to write the file to disk

                        totalRead += read;

                        if (canReportProgress)
                        {
                            progress.Report((totalRead * 1d) / (total * 1d) * 100);
                        }
                    }
                } while (isMoreToRead);
            }
        }


        #endregion




        public async Task<string> TokenMakeAsync()
        {

            string files = $"{Host}index.php/apps/files/";


            var resp = await _httpClient.GetAsync(files);

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                string html = await resp.Content.ReadAsStringAsync();

                WebScraping scr = new WebScraping(html);

                HtmlEtiq head = scr.Fin("head");
                string requesttoken = head.GetAttrib("data-requesttoken");
                Tokenlogin = requesttoken;
                return requesttoken;
            }
            return "";
        }


    }




}
    