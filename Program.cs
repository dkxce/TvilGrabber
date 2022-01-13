using System;
using System.Collections.Generic;
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Windows;
using System.Windows.Forms;

using Newtonsoft.Json;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;

namespace TvilGrabber
{
    public static class Program
    {
        public static string version = "13.01.2022";

        // All is very simple
        // Plugin Must return fileName in last line (or in single line)
        // if last line (or single) is empty - file is not exists
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;      

            Console.WriteLine("Tvil Grabber by milokz@gmail.com");
            Console.WriteLine("** version " + version + " **");

            string outFile = null;
            TvilGrabber tg = TvilGrabber.Load();
            tg.Init();
            if ((tg.regions != null) && (tg.regions.Count > 0))
            {
                List<string> regions = new List<string>();
                for (int i = 0; i < tg.regions.Count; i++)
                    regions.Add(tg.regions[i].name);
                InputBox.defWidth = 450;
                int sel = 0;
                if (InputBox.Show("Tvil Grabber", "Выберите регион:", regions.ToArray(), ref sel) == DialogResult.OK)
                {
                    int reg = tg.regions[sel].id;
                    Response.Data[] objs = tg.GrabRegion(reg);
                    if ((objs != null) && (objs.Length > 0))
                    {
                        DialogResult dr = MessageBox.Show("Получено " + objs.Length.ToString() + " объектов без имен!\r\nПолучить имена для всех объектов?\r\n\r\nP.S: Получение имен объектов может занять продолжительное время!", "Tvil Grabber", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (dr != DialogResult.Cancel)
                        {
                            for (int j = 0; j < objs.Length; j++)
                            {
                                objs[j].name = objs[j].i.ToString();
                                objs[j].comm = String.Format(tg.name_url, objs[j].i);
                                objs[j].url = String.Format(tg.name_url, objs[j].i);
                            };
                            if (dr == DialogResult.Yes)
                            {
                                outFile = Response.Data.SaveKML(objs, tg.regions[sel].name);
                                try
                                {
                                    tg.GrabNames(ref objs);
                                }
                                catch (Exception ex) 
                                {
                                    Console.WriteLine("Error: " + ex.Message);
                                };
                            };
                            outFile = Response.Data.SaveKML(objs, tg.regions[sel].name);
                            if (outFile != null) outFile = Response.Data.SaveKMZ(outFile);
                        };                        
                    };
                };                
            };
            tg.DeInit();
            Console.WriteLine("Done");
            if (outFile != null)
            {
                Console.WriteLine("Data saved to file: ");
                Console.WriteLine(outFile);
            };
            System.Threading.Thread.Sleep(500);
        }
    }

    public class TvilGrabber
    {
        public string reg_url = "https://api.tvil.ru/mapEntities?page[limit]=1&filter[geo]={0}&format[minify]=1";
        public int reg_min = 2;
        public int reg_max = 400;
        public int reg_sleep = 20;

        public string objs_url = "https://api.tvil.ru/mapEntities?page[limit]={0}&filter[geo]={1}&format[minify]=1";
        public int objs_limit = 15000;
        
        public string name_url = "https://tvil.ru/city/all/hotels/{0}/";
        public int name_threads = 1;
        public int name_sleep = 2000;
        

        [XmlArray(ElementName = "regions")]
        [XmlArrayItem(ElementName = "r")]
        public List<RegionInfo> regions = new List<RegionInfo>();

        public void Init()
        {
            if ((regions == null) || (regions.Count == 0)) GrabRegions();
        }

        public void DeInit()
        {
            this.Save();
        }

        public void GrabRegions()
        {
            regions = new List<RegionInfo>();
            Console.WriteLine("Grabbing {0}..{1} Regions with wGet ...", reg_min, reg_max);
            for (int i = reg_min; i <= reg_max; i++)
            {
                string url = String.Format(reg_url, i);
                Console.Write("Region {0} is ... ", i);

                HttpWGetRequest wGet = new HttpWGetRequest(url);
                wGet.RemoteEncoding = Encoding.UTF8;
                wGet.LocalEncoding = Encoding.UTF8;
                string body = wGet.GetResponseBody();
                if (String.IsNullOrEmpty(body))
                {
                    Console.Write("NOTHING RETURNED");
                }
                else
                {
                    Response obj = null;
                    try
                    {
                        obj = JsonConvert.DeserializeObject<Response>(body);
                        regions.Add(new RegionInfo(i, obj.meta.geo));
                        Console.Write(obj.meta.geo);
                    }
                    catch (Exception ex)
                    {
                        RespError err = null;
                        try
                        {
                            err = JsonConvert.DeserializeObject<RespError>(body);
                            Console.Write(err.errors[0].title);
                        }
                        catch (Exception ex2)
                        {
                            Console.Write(ex2.Message);
                        };
                        if (err == null)
                            Console.Write(ex.Message);
                    };                    
                };
                Console.WriteLine();
                System.Threading.Thread.Sleep(reg_sleep);
            };
            if (regions.Count > 1)
                regions.Sort(new RegionInfo.Comparer());
        }

        public Response.Data[] GrabRegion(int region)
        {
            string rName = "?";
            if (((regions != null) && (regions.Count > 0)))
                foreach (RegionInfo r in regions)
                    if (r.id == region)
                        rName = r.name;

            Console.WriteLine("Grabbing {0} - {1} with wGet ... ", region, rName);
            string url = String.Format(objs_url, objs_limit, region);

            HttpWGetRequest wGet = new HttpWGetRequest(url);
            wGet.RemoteEncoding = Encoding.UTF8;
            wGet.LocalEncoding = Encoding.UTF8;
            string body = wGet.GetResponseBody();
            if (String.IsNullOrEmpty(body))
            {
                Console.WriteLine("NOTHING RETURNED");
            }
            else
            {
                Response obj = null;
                try
                {
                    obj = JsonConvert.DeserializeObject<Response>(body);
                    Console.WriteLine("{0} objects returned", obj.data.Length);
                    return obj.data;
                }
                catch (Exception ex)
                {
                    RespError err = null;
                    try
                    {
                        err = JsonConvert.DeserializeObject<RespError>(body);
                        Console.Write(err.errors[0].title);
                    }
                    catch (Exception ex2)
                    {
                        Console.Write(ex2.Message);
                    };
                    if (err == null)
                        Console.Write(ex.Message);
                };
            };
            Console.WriteLine();
            return null;
        }

        private bool GrabName(long objId, out string objName, out string comment, out int cat, bool stdout)
        {
            objName = null;
            comment = null;
            cat = 0;

            if(stdout)
                Console.Write("Name of {0} is ", objId);
            string url = String.Format(name_url, objId);

            HttpWGetRequest wGet = new HttpWGetRequest(url);
            wGet.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_9_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.47 Safari/537.36 - " + DateTime.Now.ToString("mmssfff");
            wGet.RemoteEncoding = Encoding.UTF8;
            wGet.LocalEncoding = Encoding.UTF8;
            string body = wGet.GetResponseBody();
            if (String.IsNullOrEmpty(body))
            {
                if (stdout)
                    Console.Write("NOTHING RETURNED");
                return false;
            }
            else
            {
                int start_tag = body.IndexOf("<title>");
                if (start_tag < 0)
                {
                    if (stdout)
                        Console.Write("BAD RESULT");
                    return false;
                };
                start_tag += 7;
                int end_tag = body.IndexOf("</title>");
                if (end_tag < 0)
                {
                    if (stdout)
                        Console.Write("BAD RESULT");
                    return false;
                };
                string tag_data = body.Substring(start_tag, end_tag - start_tag);
                if (String.IsNullOrEmpty(tag_data))
                {
                    if (stdout)
                        Console.Write("BAD RESULT");
                    return false;
                };
                if (tag_data == "429 Too Many Requests")
                {
                    if (stdout)
                        Console.Write(tag_data);
                    return false;
                };
                objName = TrimAddress(objId, tag_data, out comment);                
                // get cat
                {
                    string cn = objName.ToLower();

                    if (cn.IndexOf("гостевой") >= 0) cat = 6;

                    if (cn.IndexOf("коттедж") >= 0) cat = 14;
                    if (cn.IndexOf("дом") >= 0) cat = 8;
                    if (cn.IndexOf("домик") >= 0) cat = 13;

                    if (cn.IndexOf("апартаменты") >= 0) cat = 12;                    
                    if (cn.IndexOf("студия") >= 0) cat = 10;
                    if (cn.IndexOf("квартира") >= 0) cat = 1;
                    if (cn.IndexOf("комната") >= 0) cat = 11;               

                    if (cn.IndexOf("комплекс") >= 0) cat = 9;
                    if (cn.IndexOf("кэмпинг") >= 0) cat = 7;                    
                    if (cn.IndexOf("база") >= 0) cat = 5;                    

                    if (cn.IndexOf("хостел") >= 0) cat = 4;                    
                    if (cn.IndexOf("отель") >= 0) cat = 3;
                    if (cn.IndexOf("гостиница") >= 0) cat = 2;                    
                };
                comment += "\r\nCat: " + cat.ToString();
                comment += "\r\nUrl: " + url;
                comment += "\r\nGrabbed: " + DateTime.Now.ToString("dd.MM.yyyy") + " by Tvil Grabber";
                if (stdout)
                    Console.Write(objName);
                return true;
            };
        }

        [XmlIgnore]
        System.Threading.Mutex mtx = new System.Threading.Mutex();
        [XmlIgnore]
        System.Threading.Thread[] thx = null;
        public void GrabNames(ref Response.Data[] data)
        {
            if (data == null) return;
            if (data.Length == 0) return;

            Console.WriteLine("Started at {0}", DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy") );
            Console.WriteLine("Grabbing names of {0} objects in {1} threads with wGet ... ", data.Length, name_threads);
            if (name_threads > 1)
                GrabNamesThreaded(ref data);
            else
                GrabNamesSingled(ref data);
            Console.WriteLine("Finished at {0}", DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy"));
        }

        private void GrabNamesSingled(ref Response.Data[] data)
        {
            thx = new System.Threading.Thread[name_threads];

            for (int i = 0; i < data.Length; i++)
            {
                string name, comm; int cat;
                bool ok = GrabName(data[i].i, out name, out comm, out cat, true);
                if (ok) 
                { 
                    data[i].name = name; 
                    data[i].comm = comm;
                    data[i].cat = cat;
                    data[i].url = String.Format(this.name_url, data[i].i);
                }
                else
                {
                    data[i].name = data[i].i.ToString();
                    data[i].comm = data[i].url = String.Format(this.name_url, data[i].i);
                };
                Console.WriteLine(" - {0}/{1}", i + 1, data.Length);
                System.Threading.Thread.Sleep(name_sleep);
            };
            Console.WriteLine("Grabbed done");
        }

        private void GrabNamesThreaded(ref Response.Data[] data)
        {
            thx = new System.Threading.Thread[name_threads];
            
            ThreadData thrd = new ThreadData();
            thrd.data = data;
            for (int i = 0; i < data.Length; i++) thrd.todo.Add(data[i].i);

            for (int i = 0; i < thx.Length; i++)
            {
                thx[i] = new System.Threading.Thread(GrabNameThread);
                thx[i].Start(thrd);
            };

            bool cont = true;
            int prcd = 0;
            int prct = data.Length;
            Console.Write("Already grabbed {0}/{1} ... ", prcd, prct);
            while (cont)
            {
                mtx.WaitOne();
                {
                    cont = thrd.threadsFinished < thx.Length;
                    prcd = thrd.objectsFinished;
                };
                mtx.ReleaseMutex();
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write("Already grabbed {0}/{1} ... ", prcd, prct);
                System.Threading.Thread.Sleep(1000);
            };
            cont = thrd.threadsFinished < thx.Length;
            prcd = thrd.objectsFinished;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine("Grabbed {0}/{1} names already done", prcd, prct);
        }

        private void GrabNameThread(object thrxo)
        {            
            while(true)
            {
                long id = -1;
                mtx.WaitOne();
                {
                    ThreadData thrd = (ThreadData)thrxo;
                    if (thrd.todo.Count > 0) { id = thrd.todo[0]; thrd.todo.RemoveAt(0); };                    
                };
                mtx.ReleaseMutex();
                if (id == -1)
                {
                    mtx.WaitOne();
                    {
                        ThreadData thrd = (ThreadData)thrxo;
                        thrd.threadsFinished++;
                    };
                    mtx.ReleaseMutex();
                    break;
                }
                else
                {
                    string name, comm; int cat;
                    bool ok = GrabName(id, out name, out comm, out cat, false);
                    mtx.WaitOne();
                    {
                        ThreadData thrd = (ThreadData)thrxo;
                        for (int i = 0; i < thrd.data.Length; i++)
                            if (thrd.data[i].i == id)
                            {
                                if (ok)
                                {
                                    thrd.data[i].name = name;
                                    thrd.data[i].comm = comm;
                                    thrd.data[i].cat = cat;
                                    thrd.data[i].url = String.Format(this.name_url, thrd.data[i].i);
                                }
                                else
                                {
                                    thrd.data[i].name = thrd.data[i].i.ToString();
                                    thrd.data[i].comm = thrd.data[i].url = String.Format(this.name_url, thrd.data[i].i);
                                };
                                break;
                            };
                        thrd.objectsFinished++;
                    };
                    mtx.ReleaseMutex();
                    System.Threading.Thread.Sleep(name_sleep);
                };
            };
        }

        public static TvilGrabber Load()
        {
            try
            {
                return XMLSaved<TvilGrabber>.Load();
            }
            catch
            {
                return new TvilGrabber();
            };
        }

        private void Save()
        {
            XMLSaved<TvilGrabber>.Save(this);
        }

        private string TrimAddress(long id, string line, out string comment)
        {
            string ln = line;
            comment = ln;
            if(!String.IsNullOrEmpty(ln))
            {
                ln = ln.Replace("цены и отзывы на официальном сайте Tvil.ru, бронирование отеля", "");
                ln = ln.Replace("цены и отзывы на официальном сайте Tvil.ru, бронирование базы отдыха", "");
                ln = ln.Replace("цены и отзывы на официальном сайте Tvil.ru", "");

                ln = ln.Replace("&quot;","\"");
                ln = ln.Replace("&#x27;","\"");
                int qsi = ln.IndexOf("\"");
                if (qsi > -1)
                {
                    int qse = ln.IndexOf("\"",qsi + 1);
                    if (qse > qsi)
                    {
                        string tmp = ln.Substring(qsi,qse-qsi + 1);
                        ln = tmp.Trim() + " " + (ln.Replace(tmp,"")).Trim();
                    };
                };                
                try
                {
                    int s_i = ln.IndexOf(",");
                    int s_e = ln.LastIndexOf("-") + 1;
                    if (s_i > 0)
                    {
                        string tmp = ln.Substring(0,s_i).Trim()+" ["+(ln.Substring(s_e)).Trim()+"]";
                        comment = tmp + "\r\nID: " + id.ToString() + "\r\nAddress: " + (ln.Substring(s_i + 1, s_e - 2 - s_i + 1)).Trim().Trim('-').Trim();
                        comment = comment.Replace("аренда посуточно", "\r\nNote: аренда посуточно");
                        ln = tmp;
                    };
                }
                catch (Exception ex)
                {
                    
                };
            };
            return ln;
        }

        private class ThreadData
        {
            public List<long> todo = new List<long>();
            public Response.Data[] data = null;
            public int threadsFinished = 0;
            public int objectsFinished = 0;
        }
    }

    public class RegionInfo
    {
        [XmlAttribute]
        public int id;
        [XmlText]
        public string name;
        public RegionInfo() { }
        public RegionInfo(int id, string name) { this.id = id; this.name = name; }

        public class Comparer : IComparer<RegionInfo>
        {
            public int Compare(RegionInfo a, RegionInfo b)
            {
                return a.name.CompareTo(b.name);
            }
        }
    }

    public class Response
    {
        public class Meta
        {
            public string geo;
        }

        public class Data
        {
            public long i;
            public int p;
            public int v;
            public double lt;
            public double lg;
            public int ne;
            public int r;

            /* non-json */
            public string name;
            public string comm;
            public string url;
            public int cat;

            public static string SaveKML(Data[] data, string layerName)
            {
                if ((data == null) || (data.Length == 0)) return "";
                string fileName = System.AppDomain.CurrentDomain.BaseDirectory + @"\TvilGrabber.kml";
                FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                StreamWriter sb = new StreamWriter(fs, Encoding.UTF8);

                sb.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                sb.WriteLine("<kml>");
                sb.WriteLine("<Document>");
                sb.WriteLine("<name>TvilGrabber</name>");
                sb.WriteLine("<createdby>TvilGrabber</createdby>");
                sb.WriteLine(String.Format("<Folder><name><![CDATA[{0} (Объектов: {1})]]></name>", layerName, data.Length));
                foreach (Data d in data)
                {
                    sb.WriteLine("<Placemark>");
                    sb.WriteLine(String.Format("<styleUrl>#cat{0}</styleUrl>", d.cat));
                    sb.WriteLine(String.Format("<name><![CDATA[{0}]]></name>", d.name));
                    sb.WriteLine(String.Format("<description><![CDATA[{0}]]></description>", d.comm));
                    sb.WriteLine(String.Format(System.Globalization.CultureInfo.InvariantCulture, "<Point><coordinates>{1},{0},0</coordinates></Point>", d.lt, d.lg));
                    sb.WriteLine("</Placemark>");
                };
                sb.WriteLine("</Folder>");
                for (int k = 0; k <= 14; k++)
                    sb.WriteLine(String.Format("<Style id=\"cat{0}\"><IconStyle><Icon><href>images/cat{0}.png</href></Icon></IconStyle></Style>", k));
                sb.WriteLine("</Document>");
                sb.WriteLine("</kml>");
                sb.Close();
                return fileName;
            }

            public static string SaveKMZ(string file)
            {
                string fileName = System.AppDomain.CurrentDomain.BaseDirectory + @"\TvilGrabber.kmz";
                FileStream fsOut = File.Create(fileName);
                ZipOutputStream zipStream = new ZipOutputStream(fsOut);
                zipStream.SetComment("Created by TvilGrabber");
                zipStream.SetLevel(3);
                // doc.kml
                {
                    FileInfo fi = new FileInfo(file);
                    ZipEntry newEntry = new ZipEntry("doc.kml");
                    newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity
                    newEntry.Size = fi.Length;
                    zipStream.PutNextEntry(newEntry);

                    byte[] buffer = new byte[4096];
                    using (FileStream streamReader = File.OpenRead(fi.FullName))
                        StreamUtils.Copy(streamReader, zipStream, buffer);
                    zipStream.CloseEntry();
                };
                // images
                {
                    string[] files = Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory + @"\images");
                    foreach (string filename in files)
                    {

                        FileInfo fi = new FileInfo(filename);

                        ZipEntry newEntry = new ZipEntry(@"images\" + fi.Name);
                        newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity
                        newEntry.Size = fi.Length;
                        zipStream.PutNextEntry(newEntry);

                        byte[] buffer = new byte[4096];
                        using (FileStream streamReader = File.OpenRead(filename))
                            StreamUtils.Copy(streamReader, zipStream, buffer);
                        zipStream.CloseEntry();
                    }
                };
                zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
                zipStream.Close();
                return fileName;
            }
        }

        public Data[] data;
        public Meta meta;
    }

    public class RespError
    {
        public class Error
        {
            public int status;
            public string title;
        }

        public Error[] errors;
    }

}
namespace System.Xml
{
    [Serializable]
    public class XMLSaved<T>
    {
        /// <summary>
        ///     Сохранение структуры в файл
        /// </summary>
        /// <param name="file">Полный путь к файлу</param>
        /// <param name="obj">Структура</param>
        public static void Save(T obj)
        {
            string fname = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString();
            fname = fname.Replace("file:///", "");
            fname = fname.Replace("/", @"\");
            fname = fname.Substring(0, fname.LastIndexOf(@"\") + 1);
            fname += @"configuration.xml";

            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
            System.IO.StreamWriter writer = System.IO.File.CreateText(fname);
            xs.Serialize(writer, obj);
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        ///     Подключение структуры из файла
        /// </summary>
        /// <param name="file">Полный путь к файлу</param>
        /// <returns>Структура</returns>
        public static T Load()
        {
            string fname = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString();
            fname = fname.Replace("file:///", "");
            fname = fname.Replace("/", @"\");
            fname = fname.Substring(0, fname.LastIndexOf(@"\") + 1);
            fname += @"configuration.xml";

            // if couldn't create file in temp - add credintals
            // http://support.microsoft.com/kb/908158/ru
            System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(typeof(T));
            System.IO.StreamReader reader = System.IO.File.OpenText(fname);
            T c = (T)xs.Deserialize(reader);
            reader.Close();
            return c;
        }
    }
}
