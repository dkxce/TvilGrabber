//*************************//
//*************************//
//                         //
//       Writed By         //
//    milokz@gmail.com     //
//                         //
//*************************//
//*************************//
//                         //
//  Tested on              //
//  GNU Wget 1.19.4 built  //
//  on mingw32             //
//                         //
//*************************//
//*************************//

using System;

namespace System.Net
{
    /// <summary>
    ///     Using wGet.exe to HTTPRequest
    ///     Place wGet.exe to application exe folder
    ///     Home Page: http://www.gnu.org/software/wget/wget.html
    ///     Manual: http://www.gnu.org/software/wget/manual/
    /// </summary>
    public class HttpWGetRequest
    {
        #region private vars
        private string _Url = "http://localhost/";
        private string _UserAgent = "";
        private string _ExePath = AppDomain.CurrentDomain.BaseDirectory.Trim('\\') + @"\";
        private string _ExeName = "wget.exe";
        private string _HTTPMethod = "GET";
        private string _LogFile = null;
        private bool _Debug = false;
        private bool _Quiet = false;
        private bool _Verbose = true;
        private bool _ServerResponse = false;
        private int _Timeout = 900;
        private bool _NoProxy = false;
        private bool _NoDNSCache = false;
        private string _HTTPUser = null;
        private string _HTTPPassword = null;
        private System.Text.Encoding _LocalEncoding = System.Text.Encoding.UTF8;
        private System.Text.Encoding _RemoteEncoding = System.Text.Encoding.UTF8;
        private bool _NoHttpKeepAlive = false;
        private bool _NoCache = false;
        private bool _NoCookies = false;
        private string _LoadCookiesFile = null;
        private string _SaveCookiesFile = null;
        private bool _IgnoreLength = false;
        private string[] _Headers = null;
        private string _ProxyUser = null;
        private string _ProxyPassword = null;
        private string _Referer = null;
        private bool _SaveHeaders = true;

        private string _POSTDATA = null;
        private string _BODYDATA = null;
        private string _PostFile = null;
        private string _BodyFile = null;
        private bool _NoCheckCertificate = true;
        private bool _HTTPSOnly = false;
        private string _SecureProtocol = "auto";
        private string _OutputFile = "-";
        private bool _ExeWindow = false;
        private string _ExeUser = null;
        private string _ExePassword = null;
        private bool _ContentOnError = true;
        private string _CustomParameters = null;
        #endregion

        /// <summary>
        ///     Using wGet.exe to HTTPRequest
        ///     Place wGet.exe to application exe folder
        ///     Home Page: http://www.gnu.org/software/wget/wget.html
        ///     Manual: http://www.gnu.org/software/wget/manual/
        /// </summary>
        public HttpWGetRequest(){ }

        //// <summary>
        ///     Using wGet.exe to HTTPRequest
        ///     Place wGet.exe to application exe folder
        ///     Home Page: http://www.gnu.org/software/wget/wget.html
        ///     Manual: http://www.gnu.org/software/wget/manual/
        /// </summary>
        /// <param name="Url">Url</param>
        public HttpWGetRequest(string Url) { this._Url = Url; }

        /// <summary>
        ///     Url to call
        /// </summary>
        public string Url { get { return _Url; } set { _Url = value; } }

        /// <summary>
        ///     Custom Command Line Parameters:
        ///     --no-dns-cache, --execute cmd, --no-if-modified-since, --spider, --compression=gzip
        /// </summary>
        public string CustomParameters { get { return _CustomParameters; } set { _CustomParameters = value; } }

        /// <summary>
        ///   Filename with path of wGet.exe
        /// </summary>
        public string ExeFile { get { return _ExePath + _ExeName; } set { _ExePath = System.IO.Path.GetDirectoryName(value).Trim('\\') + @"\"; _ExeName = System.IO.Path.GetFileName(value); } }

        /// <summary>
        ///     Path without filename to wGet.exe
        /// </summary>
        public string ExePath { get { return _ExePath; } set { _ExePath = value.Trim('\\') + @"\"; } }

        /// <summary>
        ///     FileName without path of wGet.exe
        /// </summary>
        public string ExeName { get { return _ExeName; } set { _ExeName = System.IO.Path.GetFileName(value); } }

        /// <summary>
        ///     HTTP Method: GET/POST/PUT
        /// </summary>
        public string HTTPMethod { get { return _HTTPMethod; } set { _HTTPMethod = value; } }        

        /// <summary>
        ///     If this is set to on, wget will not skip the content when the server responds with a http status code that indicates error.
        /// </summary>
        public bool ContentOnError { get { return _ContentOnError; } set {_ContentOnError = value;} }

        /// <summary>
        ///     Log all messages to logfile. The messages are normally reported to standard error.
        /// </summary>
        public string LogFile { get { return _LogFile; } set { _LogFile = value; } }

        /// <summary>
        ///     Turn on debug output, meaning various information important to the developers of Wget if it does not work properly. Your system administrator may have chosen to compile Wget without debug support
        /// </summary>
        public bool Debug { get { return _Debug; } set { _Debug = value; } }

        /// <summary>
        ///     Turn off Wget’s output.
        /// </summary>
        public bool Quiet { get { return _Quiet; } set { _Quiet = value; } }

        /// <summary>
        ///     Turn on verbose output, with all the available data. The default output is verbose.
        /// </summary>
        public bool Verbose { get { return _Verbose; } set { _Verbose = true; } }

        /// <summary>
        ///     Print the headers sent by HTTP servers to StdOut
        /// </summary>
        public bool ServerResponse { get { return _ServerResponse; } set { _ServerResponse = value; } }

        /// <summary>
        ///     Set the network timeout to seconds seconds
        /// </summary>
        public int Timeout { get { return _Timeout; } set { _Timeout = value; } }

        /// <summary>
        ///     Don’t use proxies
        /// </summary>
        public bool NoProxy { get { return _NoProxy; } set { _NoProxy = value; } }

        /// <summary>
        ///     Do not use DNS cache
        /// </summary>
        public bool NoDNSCache { get { return _NoDNSCache; } set { _NoDNSCache = value; } }
        
        /// <summary>
        ///     Specify the username on an HTTP server
        /// </summary>
        public string HTTPUser { get { return _HTTPUser; } set { _HTTPUser = value; } }

        /// <summary>
        ///     Specify the password on an HTTP server
        /// </summary>
        public string HTTPPassword { get { return _HTTPPassword; } set { _HTTPPassword = value; } }

        /// <summary>
        ///     That affects how Wget converts URLs specified as arguments from locale to UTF−8 for IRI support.
        /// </summary>
        public System.Text.Encoding LocalEncoding { get { return _LocalEncoding; } set { _LocalEncoding = value; } }

        /// <summary>
        ///     Response Server Encoding
        /// </summary>
        public System.Text.Encoding RemoteEncoding { get { return _RemoteEncoding; } set { _RemoteEncoding = value; } }

        /// <summary>
        ///     Turn off the "keep-alive" feature for HTTP downloads.
        /// </summary>
        public bool NoHTTPKeepAlive { get { return _NoHttpKeepAlive; } set { _NoHttpKeepAlive = value; } }

        /// <summary>
        ///     Disable server-side cache
        /// </summary>
        public bool NoCache { get { return _NoCache; } set { _NoCache = value; } }

        /// <summary>
        ///     Disable the use of cookies.
        /// </summary>
        public bool NoCookies { get { return _NoCookies; } set { _NoCookies = value; } }

        /// <summary>
        ///     Load cookies from file before the first HTTP retrieval. file is a textual file in the format originally used by Netscape’s cookies.txt file.
        /// </summary>
        public string LoadCookiesFile { get { return _LoadCookiesFile; } set { _LoadCookiesFile = value; } }

        /// <summary>
        ///     Save cookies to file before exiting. This will not save cookies that have expired or that have no expiry time (so-called "session cookies")
        /// </summary>
        public string SaveCookiesFile { get { return _SaveCookiesFile; } set { _SaveCookiesFile = value; } }

        /// <summary>
        ///     Unfortunately, some HTTP servers ( CGI programs, to be more precise) send out bogus "Content−Length" headers, which makes Wget go wild, as it thinks not all the document was retrieved. You can spot this syndrome if Wget retries getting the same document again and again, each time claiming that the (otherwise normal) connection has closed on the very same byte.
        /// </summary>
        public bool IgnoreLength { get { return _IgnoreLength; } set { _IgnoreLength = value; } }

        /// <summary>
        ///     HTTP Request Headers 'Authorization: oauth'
        /// </summary>
        public string[] Headers { get { return _Headers; } set { _Headers = value; } }

        /// <summary>
        ///     Specify the username for authentication on a proxy server. 
        /// </summary>
        public string ProxyUser { get { return _ProxyUser; } set { _ProxyUser = value; } }

        /// <summary>
        ///     Specify the password for authentication on a proxy server. 
        /// </summary>
        public string ProxyPassword { get { return _ProxyPassword; } set { _ProxyPassword = value; } }

        /// <summary>
        ///     Include ‘Referer: url’ header in HTTP request. Useful for retrieving documents with server-side processing that assume they are always being retrieved by interactive web browsers and only come out properly when Referer is set to one of the pages that point to them.
        /// </summary>
        public string Referer { get { return _Referer; } set { _Referer = value; } }

        /// <summary>
        ///     Save the headers sent by the HTTP server to the file or stdout, preceding the actual contents, with an empty line as the separator.
        /// </summary>
        public bool SaveHeaders { get { return _SaveHeaders; } set { _SaveHeaders = value; } }

        /// <summary>
        ///     Identify as agent-string to the HTTP server.
        /// </summary>
        public string UserAgent { get { return _UserAgent; } set { _UserAgent = value;  } }

        /// <summary>
        ///     Use POST as the method for all HTTP requests and send the specified data in the request body.
        /// </summary>
        public string PostFile { get { return _PostFile; } set { _PostFile = value; } }

        /// <summary>
        ///     Must be set when additional data needs to be sent to the server along with the Method specified using −−method. −−body−data sends string as data, whereas −−body−file sends the contents of file. Other than that, they work in exactly the same way.
        /// </summary>
        public string BodyFile { get { return _BodyFile; } set { _BodyFile = value; } }

        /// <summary>
        ///     Don’t check the server certificate against the available certificate authorities. Also don’t require the URL host name to match the common name presented by the certificate.
        /// </summary>
        public bool NoCheckCertificate { get { return _NoCheckCertificate; } set { _NoCheckCertificate = value; } }

        /// <summary>
        ///     When in recursive mode, only HTTPS links are followed.
        /// </summary>
        public bool HTTPSOnly { get { return _HTTPSOnly; } set { _HTTPSOnly = value; } }

        /// <summary>
        ///     Choose the secure protocol to be used. Legal values are auto, SSLv2, SSLv3, TLSv1, TLSv1_1, TLSv1_2 and PFS . If auto is used, the SSL library is given the liberty of choosing the appropriate protocol automatically, which is achieved by sending a TLSv1 greeting. This is the default.
        /// </summary>
        public string SecureProtocol { get { return _SecureProtocol; } set { _SecureProtocol = value; } }

        /// <summary>
        ///     StdOut (-) or File to save servers response
        /// </summary>
        public string OutputFile { get { return _OutputFile; } set { _OutputFile = value; } }

        /// <summary>
        ///     Show wGet.exe console window StdOut
        /// </summary>
        public bool ExeWindow { get { return _ExeWindow; } set { _ExeWindow = value; } }

        /// <summary>
        ///     Start wGet.exe as user name
        /// </summary>
        public string ExeUser { get { return _ExeUser; } set { _ExeUser = value; } }

        /// <summary>
        ///     Start wGet.exe as user password
        /// </summary>
        public string ExePassword { get { return _ExePassword; } set { _ExePassword = value; } }

        

        /// <summary>
        ///     Use POST as the method for all HTTP requests and send the specified data in the request body. −−post−data sends string as data
        /// </summary>
        public string POSTDATA { get { return _POSTDATA; } set { _POSTDATA = value; } }

        /// <summary>
        ///     Must be set when additional data needs to be sent to the server along with the Method specified using −−method. −−body−data sends string as data
        /// </summary>
        public string BODYDATA { get { return _BODYDATA; } set { _BODYDATA = value; } }

        public string GetExeParams()
        {
            string pars = "--method=" + _HTTPMethod;

            if (!String.IsNullOrEmpty(_LogFile)) pars = " −o \"" + _LogFile + "\"";

            if (_Debug) pars = " -d"; // " −−debug";
            if (_Quiet) pars = " -q"; // " −−quiet";

            if (!_Verbose) pars = " -nv"; // " −−no−verbose";
            if ((String.IsNullOrEmpty(_OutputFile)) || (_OutputFile == "-"))
                pars += " -O -"; // -O or −−output−document=file // The documents will not be written to the appropriate files, but all will be concatenated together and written to file. If − is used as file, documents will be printed to standard output, disabling link conversion. (Use ./− to print to a file literally named −.)
            else if (!String.IsNullOrEmpty(_OutputFile))
                pars += " -O \"" + _OutputFile + "\"";

            if (_ServerResponse) pars += " -S"; // " −−server−response";
            if (_Timeout != 900) pars += " -T " + _Timeout.ToString(); //  " −−timeout=30"; // timeout in seconds

            if (_NoProxy) pars += " −−no−proxy"; // Don’t use proxies, even if the appropriate *_proxy environment variable is defined.
            if (_NoDNSCache) pars += " −−no−dns−cache";

            if (!String.IsNullOrEmpty(_HTTPUser)) pars += " --http-user=" + _HTTPUser;
            if ((!String.IsNullOrEmpty(_HTTPUser)) && (!String.IsNullOrEmpty(_HTTPPassword))) pars += " --http-password=" + _HTTPPassword;

            if ((_LocalEncoding != null) && (_LocalEncoding != System.Text.Encoding.UTF8)) pars += " −−local−encoding=" + _LocalEncoding.EncodingName;// // That affects how Wget converts URLs specified as arguments from locale to UTF−8 for IRI support.
            if ((_RemoteEncoding != null) && (_RemoteEncoding != System.Text.Encoding.UTF8)) pars += " −−remote−encoding=" + _RemoteEncoding.EncodingName; // // default remote server encoding.

            if (_NoHttpKeepAlive) pars += " −−no−http−keep−alive";
            if (_NoCache) pars += " −−no−cache"; // no cache
            if (_NoCookies) pars += " −−no−cookies"; // no cookies
            if (!String.IsNullOrEmpty(_LoadCookiesFile)) pars += " −−load−cookies \"" + _LoadCookiesFile + "\""; // cookies from file
            if (!String.IsNullOrEmpty(_SaveCookiesFile)) pars += " −−save−cookies \"" + SaveCookiesFile + "\""; // save cookies to file            

            if ((_Headers != null) && (_Headers.Length > 0))
                foreach (string header in _Headers)
                    pars += " --header=\"" + header.Replace("\"", "\\\"") + "\""; // headers
            if (!String.IsNullOrEmpty(_Referer)) pars += " −−referer=\"" + _Referer + "\"";
            if (_UserAgent != null) pars += " --user-agent=\"" + _UserAgent + "\""; // user-agent    

            if (!String.IsNullOrEmpty(_ProxyUser)) pars += " −−proxy−user=" + _ProxyUser; // proxy user
            if ((!String.IsNullOrEmpty(_ProxyUser)) && (!String.IsNullOrEmpty(_ProxyPassword))) pars += " −−proxy−password=" + ProxyPassword; // proxy pass

            if (_SaveHeaders) pars += " --save-headers"; // out response headers to stdout  
            if (_IgnoreLength) pars += " −−ignore−length";
            if (_ContentOnError) pars += " --content-on-error";

            if (!String.IsNullOrEmpty(POSTDATA)) pars += " −−post−data=\"" + POSTDATA + "\""; // POST data
            if ((!String.IsNullOrEmpty(_PostFile)) && System.IO.File.Exists(_PostFile)) pars += " −−post−file=\"" + _PostFile + "\"";// 
            if (!String.IsNullOrEmpty(BODYDATA)) pars += " −−body−data=\"" + BODYDATA + "\""; // POST data
            if ((!String.IsNullOrEmpty(_BodyFile)) && System.IO.File.Exists(_BodyFile)) pars += " −−body−file=\"" + _BodyFile + "\"";// 

            if (_NoCheckCertificate) pars += " --no-check-certificate"; // for HTTPS connection
            if (_HTTPSOnly) pars += " −−https−only"; //
            if ((!String.IsNullOrEmpty(_SecureProtocol)) && (_SecureProtocol != "auto")) pars += " −−secure−protocol=" + _SecureProtocol; // auto, SSLv2, SSLv3, TLSv1, TLSv1_1, TLSv1_2 and PFS 

            // pars += " −−certificate={file}"; //
            // pars += " −−certificate−type={type}"; // PEM / DER
            // pars += " −−ca−certificate={file}"; //  

            if (!String.IsNullOrEmpty(_CustomParameters)) pars += " " + _CustomParameters.Trim(' ');

            pars += " \"" + _Url + "\""; // pars

            return pars;
        }

        /// <summary>
        ///     Do all progress to StdOut
        /// </summary>
        public bool ProgressStdOut { get { return _ServerResponse && _ExeWindow && (!_Quiet); } set { _Quiet = !(_ServerResponse = _ExeWindow = value); } }

        /// <summary>
        ///     Call and get server's body response
        /// </summary>
        /// <returns></returns>
        public string GetResponseBody()
        {
            string h;
            return GetResponseBody(out h);         
        }

        /// <summary>
        ///     Call and get server's body response
        /// </summary>
        /// <param name="Headers">Server response headers</param>
        /// <returns></returns>
        public string GetResponseBody(out string Headers)
        {
            Headers = "";

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.StandardOutputEncoding = Text.Encoding.UTF8;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = ExeFile;
            p.StartInfo.Arguments = GetExeParams();
            p.StartInfo.CreateNoWindow = !_ExeWindow;
            if (!String.IsNullOrEmpty(_ExeUser)) p.StartInfo.UserName = _ExeUser;
            if ((!String.IsNullOrEmpty(_ExeUser)) && (!String.IsNullOrEmpty(_ExePassword))) p.StartInfo.Password = ConvertToSecureString(_ExePassword);
            p.Start();

            string output = "";
            Text.Encoding ttt = p.StandardOutput.CurrentEncoding;

            output += p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string[] ext = new string[] { "No Problem", "Generic Error Code", "Command Line Parse Error", "File I/O Error", "Network Failure", "SSL Certificate Failure", "Username/Password Authentification Failure", "Protocol Error", "Server Response Error" };
            int ExCode = p.ExitCode; // 0 - No problems,  1 - code error, 2 - parse error, 3 - IO error, 4 - Net failure, 5 - SSL failure, 6 - Auth failuer, 7 - proto error, 8 - error response

            string body = output;
            if ((!String.IsNullOrEmpty(output)) && (_SaveHeaders))
            {
                int bs = output.IndexOf("\r\n\r\n");
                if (bs > 0)
                {
                    body = output.Substring(bs + 4);
                    Headers = output.Substring(0, bs);
                };
            };

            if (ExCode == 0)
                return body;
            else
            {
                if (output != "")
                    return body;
                else
                    throw new Exception("Exit Code " + ExCode.ToString() + " - " + ext[ExCode]);
            };
        }

        /// <summary>
        ///     Call and get servers's response
        /// </summary>
        /// <returns></returns>
        public string GetResponse()
        {          
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = ExeFile;
            p.StartInfo.Arguments = GetExeParams();
            p.StartInfo.CreateNoWindow = !_ExeWindow;
            if(!String.IsNullOrEmpty(_ExeUser)) p.StartInfo.UserName = _ExeUser;
            if((!String.IsNullOrEmpty(_ExeUser)) && (!String.IsNullOrEmpty(_ExePassword))) p.StartInfo.Password = ConvertToSecureString(_ExePassword);
            p.Start();

            string output = "";
            output += p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string[] ext = new string[] { "No Problem", "Generic Error Code", "Command Line Parse Error", "File I/O Error", "Network Failure", "SSL Certificate Failure", "Username/Password Authentification Failure", "Protocol Error", "Server Response Error" };
            int ExCode = p.ExitCode; // 0 - No problems,  1 - code error, 2 - parse error, 3 - IO error, 4 - Net failure, 5 - SSL failure, 6 - Auth failuer, 7 - proto error, 8 - error response

            if (ExCode == 0)
               return output;
            else
            {
                if (output != "")
                    return output;
                else
                    throw new Exception("Exit Code " + ExCode.ToString() + " - " + ext[ExCode]);
            };
        }

        /// <summary>
        ///     wGET HTTP(S) REQUEST
        /// </summary>
        /// <param name="url">url</param>
        /// <param name="headers">http headers</param>
        /// <param name="return_headers">return server response headers</param>
        public static string SimpleRequest(string url, string[] headers)
        {
            string wget = AppDomain.CurrentDomain.BaseDirectory + @"\wget.exe";

            string pars = "--method=GET"; // GET or POST
            // pars += " −−server−response"; // Print the headers sent by HTTP servers and responses sent by FTP servers.
            // pars += " −−timeout=30"; // timeout in seconds
            pars += " --no-check-certificate"; // for HTTPS connection
            // pars += " −−https−only"; //
            // pars += " −−secure−protocol={protocol}"; // auto, SSLv2, SSLv3, TLSv1, TLSv1_1, TLSv1_2 and PFS 
            // pars += " −−certificate={file}"; //
            // pars += " −−certificate−type={type}"; // PEM / DER
            // pars += " −−ca−certificate={file}"; // 
            // pars += " −−no−proxy"; // Don’t use proxies, even if the appropriate *_proxy environment variable is defined.
            // pars += " −−proxy−user=user" ; // proxy user
            // pars += " −−proxy−password=password"; // proxy pass
            pars += " --content-on-error"; // get content if server returns != 200
            pars += " --save-headers"; // out response headers to stdout
            pars += " --user-agent=\"\""; // user-agent
            // pars += " −−no−cache"; // no cache
            // pars += " −−no−cookies"; // no cookies
            // pars += " −−load−cookies {file}"; // cookies from file
            // pars += " −−save−cookies {file}"; // save cookies to file
            // pars += " −−ignore−length"; // ingonre Content-Length
            // pars += " −−http−user={user}"; // USER
            // pars += " −−http-password={password}"; // password
            // pars += " −−local−encoding=encoding";// // That affects how Wget converts URLs specified as arguments from locale to UTF−8 for IRI support.
            // pars += " −−remote−encoding=encoding"; // // default remote server encoding.
            // pars += " −−post−data={string}"; // POST data
            // pars += " −−post−file={file}";// 
            // pars += " −−body−data={data-string}"; // POST data
            // pars += " −−body−file={body-file}";//

            // CUSTOM HEADERS
            if ((headers != null) && (headers.Length > 0))
                foreach (string header in headers)
                    pars += " --header=\"" + header.Replace("\"", "\\\"") + "\""; // headers
            // URL
            // pars += " −−input−file={file}"; // Read URLs from a local or external file. If − is specified as file, URLs are read from the standard input
            pars += " \"" + url + "\""; // pars

            // OUTPUT
            pars += " --quiet"; // Turn off Wget’s output.
            pars += " -O -"; // -O or −−output−document=file // The documents will not be written to the appropriate files, but all will be concatenated together and written to file. If − is used as file, documents will be printed to standard output, disabling link conversion. (Use ./− to print to a file literally named −.)

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = wget;
            p.StartInfo.Arguments = pars;
            p.StartInfo.CreateNoWindow = true;
            //p.StartInfo.UserName = "";
            //p.StartInfo.Password = ("");
            p.Start();

            string output = "";
            output += p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            string[] ext = new string[] { "No Problem", "Generic Error Code", "Command Line Parse Error", "File I/O Error", "Network Failure", "SSL Certificate Failure", "Username/Password Authentification Failure", "Protocol Error", "Server Response Error" };
            int ExCode = p.ExitCode; // 0 - No problems,  1 - code error, 2 - parse error, 3 - IO error, 4 - Net failure, 5 - SSL failure, 6 - Auth failuer, 7 - proto error, 8 - error response

            if (ExCode == 0)
                return output;
            else
            {
                if (output != "")
                    return output;
                else
                    throw new Exception("Exit Code " + ExCode.ToString() + " - " + ext[ExCode]);
            };
        }

        private static System.Security.SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            unsafe
            {
                fixed (char* passwordChars = password)
                {
                    System.Security.SecureString securePassword = new System.Security.SecureString(passwordChars, password.Length);
                    securePassword.MakeReadOnly();
                    return securePassword;
                }
            }
        }
    }
}
