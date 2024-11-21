using Shared.Core.Loggers;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;


namespace Shared.Core.Commons;

public class Common
{
    public static string Follder { get; set; }

    public static bool getContentFromFileByUrl(string url, string fileName)
    {
        try
        {

            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(
                    new System.Uri(url), fileName
                );
                return true;
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    /// <summary>
    /// Hàm này trả về dạng double dựa vào key trong dynamic, đã có trycatch nên giá trị mặc định là 0
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double GetValueByKeyToDouble(string key, dynamic value)
    {
        double val = 0;
        dynamic outVal;
        if (value.TryGetValue(key, out outVal))
        {
            try
            {
                val = (double)outVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        return val;
    }
    /// <summary>
    /// Lấy đường dẫn đến foder bin
    /// </summary>
    /// <returns></returns>
    public static string GetCurentFolderBin()
    {
        var folder = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
        return CheckFolder(folder);
    }
    /// <summary>
    /// Tạo Folder Date
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static string GetDateFolder(DateTime date)
    {
        try
        {
            var dataFollder = string.Format("{0}\\{1}\\{2}\\", date.Year, date.Month, date.Day);
            return dataFollder;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    /// <summary>
    /// Lấy đường dẫn đến wwwroot
    /// </summary>
    /// <returns></returns>

    public static string GetCurentFolder()
    {
        var folder = Common.Follder + "\\";
        return CheckFolder(folder);
    }
    public static string GetExcelFolder()
    {
        try
        {
            var rootFolder = GetCurentFolder() + "Reports\\";
            rootFolder = CheckFolder(rootFolder);
            return rootFolder;
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public static string GetExcelDateFullFolder(DateTime date)
    {
        try
        {
            var dataFollder = string.Format("{0}{1}", GetCurentFolder() + "export_excel\\", GetDateFolder(date));
            dataFollder = CheckFolder(dataFollder);
            return dataFollder;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public static string GetExcelDatePathFolder(DateTime date)
    {
        try
        {
            var dataFollder = string.Format("{0}{1}", "export_excel\\", GetDateFolder(date));
            return dataFollder;
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public static string GetImageDateFullFolder(DateTime date)
    {
        try
        {
            var dataFollder = string.Format("{0}{1}", GetCurentFolder() + "images\\", GetDateFolder(date));
            dataFollder = CheckFolder(dataFollder);
            return dataFollder;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public static string GetImageDatePathFolder(DateTime date)
    {
        try
        {
            var dataFollder = string.Format("{0}{1}", "images\\", GetDateFolder(date));
            return dataFollder;
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public static string GetImageDateFullFolder(DateTime date, string folderName)
    {
        try
        {
            var dataFollder = string.Format("{0}{1}", GetCurentFolder() + $"{folderName}\\", GetDateFolder(date));
            dataFollder = CheckFolder(dataFollder);
            return dataFollder;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public static string GetImageDatePathFolder(DateTime date, string folderName)
    {
        try
        {
            var dataFollder = string.Format("{0}{1}", $"{folderName}\\", GetDateFolder(date));
            return dataFollder;
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public static string GetImagesFullFolder(string folderName)
    {
        try
        {
            var dataFollder = string.Format("{0}{1}", GetCurentFolder(), folderName);
            dataFollder = CheckFolder(dataFollder);
            return dataFollder;
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public static string GetImagesPathFolder(string folderName)
    {
        try
        {
            var dataFollder = string.Format("{0}", folderName);
            return dataFollder;
        }
        catch (Exception e)
        {
            throw e;
        }
    }



    /// <summary>
    /// Hàm này dùng để check tồn tại của Folder, Trả về chuỗi gồm "tên_folder\\"
    /// </summary>
    /// <param name="folder"></param>
    /// <returns></returns>
    public static string CheckFolder(string folder)
    {
        if (!folder.EndsWith("\\"))
            folder += "\\";
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        return folder;
    }
    public static string GetValueByKey(string key, dynamic values)
    {
        string val = "";
        foreach (var x in values)
        {
            if (x.Key == key)
            {
                val = x.Value != null ? x.Value.ToString() : "";
            }

        }
        return val;
    }

    public static string GetValueByKeyToString(string key, dynamic value)
    {
        string val = "";
        dynamic outVal;
        if (value.TryGetValue(key, out outVal))
        {
            try
            {
                val = outVal.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        return val;
    }

    public static int GetValueByKeyToInt2(string key, dynamic values)
    {
        int val = 0;
        try
        {
            foreach (var x in values)
            {
                if (x.Key == key)
                {
                    val = x.Value != null ? int.Parse(x.Value) : 0;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return val;
    }

    public static int GetValueByKeyToInt(string key, dynamic value)
    {
        int val = 0;
        dynamic outVal;
        if (value.TryGetValue(key, out outVal))
        {
            try
            {
                val = (int)outVal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        return val;
    }

    //public static string ConvertImageToBase64(string fileName)
    //{
    //    string base64String = "";
    //    if (File.Exists(fileName))
    //        using (Image image = Image.FromFile(fileName))
    //        {
    //            using (MemoryStream m = new MemoryStream())
    //            {
    //                image.Save(m, image.RawFormat);
    //                byte[] imageBytes = m.ToArray();

    //                // Convert byte[] to Base64 String
    //                base64String = Convert.ToBase64String(imageBytes);
    //            }
    //        }
    //    return base64String;

    //}

    public static dynamic StringToDynamic(dynamic values, string key)
    {
        dynamic val = null;
        try
        {
            values.TryGetValue(key, out val);
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return val;
    }

    public static dynamic GetSubDynamic(string key, dynamic values)
    {
        dynamic val = 0;
        try
        {
            //dynamic
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return val;
    }

    public static Guid ConvertStringToGuid(string val)
    {
        try
        {
            return Guid.Parse(val);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static string PropCase(string strText)
    {
        if (!string.IsNullOrEmpty(strText))
            return new CultureInfo("vi-VN").TextInfo.ToTitleCase(strText.ToLower());
        else
            return strText;
    }

    public static Image ConvertStringBase64ToImage(string image_base64)
    {
        try
        {
            byte[] bytes = Convert.FromBase64String(image_base64);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }
            return image;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static string ConvertFileImageToBase64(string file)
    {
        try
        {
            using (Image image = Image.FromFile(file))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
        catch (Exception) { return null; }
    }

    public static string ConvertImageToBase64(Image image)
    {
        try
        {
            var imageStream = new MemoryStream();
            image.Save(imageStream, System.Drawing.Imaging.ImageFormat.Bmp);
            imageStream.Position = 0;
            var imageBytes = imageStream.ToArray();
            var ImageBase64 = Convert.ToBase64String(imageBytes);
            return ImageBase64;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static double DateTimeToUnixTimestamp(DateTime dateTime)
    {
        return (TimeZoneInfo.ConvertTimeToUtc(dateTime) -
               new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
    }

    public static bool SaveImageBase64ToFile(string image_base64, string file)
    {
        Image image = ConvertStringBase64ToImage(image_base64);

        var bitmap = new Bitmap(image);

        bitmap.Save(file);

        return File.Exists(file);
    }

    public static int CreateHourMinuteTimeId(DateTime dateTime)
    {
        //return int.Parse(dateTime.Hour.ToString() + dateTime.Minute.ToString());
        return int.Parse(string.Format("{0}{1:00}", dateTime.Hour, dateTime.Minute));
    }
    public static DateTime GetDateMinute(DateTime dateTime)
    {
        //return dateTime.Date.AddHours(dateTime.Hour).AddMinutes(dateTime.Minute);
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, 0);
    }
    /// <summary>
    /// Lấy list file có quyền truy cập
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="fileExtension"></param>
    /// <param name="maxLength"></param>
    /// <returns></returns>
    //public static List<FileCanAccess> getFileInfo(DirectoryInfo dir, string fileExtension = ".txt", long maxLength = 300000)
    //{
    //    List<FileCanAccess> _listFileInfo = new List<FileCanAccess>();
    //    FileInfo[] fileInfoArray = dir.GetFiles().OrderBy(o => o.CreationTime).ToArray<FileInfo>();
    //    foreach (FileInfo fileInfo in fileInfoArray)
    //    {
    //        try
    //        {
    //            using (FileStream stream = File.Open(fileInfo.FullName, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
    //            {
    //                if (fileInfo.Extension == fileExtension && fileInfo.Length < maxLength)
    //                {
    //                    byte[] buffer = new byte[stream.Length];
    //                    stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
    //                    string content = System.Text.Encoding.UTF8.GetString(buffer);
    //                    _listFileInfo.Add(new FileCanAccess()
    //                    {
    //                        _canAccess = true,
    //                        fileInfo = fileInfo,
    //                        _content = content
    //                    });
    //                }
    //                stream.Close();
    //            }
    //        }
    //        catch
    //        {
    //            string exceptionString = string.Format("File {0} is using by another program: ", fileInfo.Name);
    //            //Logger.ShowLog(exceptionString);
    //            //_listFileInfo.Add(new FileCanAccess()
    //            //{
    //            //    _canAccess = false,
    //            //    fileInfo = fileInfo,
    //            //    _content = ""
    //            //});
    //        }
    //    }
    //    return _listFileInfo;
    //}

    public static HttpResponseMessage getSync(string apiUrl, string endPoint)
    {
        try
        {
            #region cấu hình request
            string url = apiUrl + endPoint;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Get;
            #endregion
            HttpClient client = new HttpClient();
            HttpResponseMessage result = client.SendAsync(request).Result;
            client.Dispose();
            return result;
        }
        catch (Exception e)
        {
            throw e;
        }

    }

    // StringContent, FormUrlEncodedContent StringContent(jsonData, Encoding.UTF8, "application/json")
    public static HttpResponseMessage postSync(string apiUrl, HttpContent contentBody, string accessToken)
    {
        try
        {
            string url = apiUrl;
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Post;
            request.Content = contentBody;
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            HttpClient client = new HttpClient();
            HttpResponseMessage result = client.SendAsync(request).Result;
            client.Dispose();
            return result;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static XmlDocument TryToReadXmlString(string xmlContent)
    {
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            return doc;
        }
        catch
        {
            throw new Exception("Can_not_read_xml_string");
        }
    }
    public static string TryToBase64Encode(string plainText)
    {
        try
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        catch
        {
            throw new Exception("Can_not_encode_base64");
        }

    }
    public static string TryToBase64Decode(string base64EncodedData)
    {
        try
        {
            byte[] base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        catch
        {
            throw new Exception("Can_not_decode_base64");
        }
    }
    /// <summary>
    /// Hàm này dùng để Lưu File hoặc đè nội dung vào File [tùy chọn overWrite]
    /// </summary>
    /// <param name="data"></param>
    /// <param name="fileName"></param>
    /// <param name="follder"></param>
    /// <param name="overWrite"></param>
    /// <returns></returns>
    public static bool SaveFile(string data, string fileName, string follder, bool overWrite = false)
    {
        try
        {
            follder = CheckFolder(follder);
            string FullFileName = follder + fileName;

            if (overWrite && File.Exists(FullFileName))
                File.Delete(FullFileName);

            using (FileStream fs = File.Create(FullFileName))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(data);
                fs.Write(info, 0, info.Length);
                fs.Close();
            }

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public static TimeSpan ConvertStringToTime(string time)
    {
        var endTime = time.Split(":");

        int endHour = int.Parse(endTime[0]);
        int endMinunte = int.Parse(endTime[1]);
        int endSecond = int.Parse(endTime[2]);

        var retVal = new TimeSpan(endHour, endMinunte, endSecond);
        return retVal;
    }

    public static T DictionaryToObject<T>(Dictionary<string, object> dict) where T : new()
    {
        T obj = new T();
        Type type = typeof(T);

        foreach (var kvp in dict)
        {
            // Get property by name
            PropertyInfo property = type.GetProperty(kvp.Key);

            // If the property exists and is writable, set the value
            if (property != null && property.CanWrite)
            {
                property.SetValue(obj, Convert.ChangeType(kvp.Value, property.PropertyType), null);
            }
        }

        return obj;
    }
    public static List<T> ConvertDictionaryListToObjectList<T>(List<Dictionary<string, object>> dictList) where T : new()
    {
        List<T> objectList = new List<T>();
        try
        {
            foreach (var dict in dictList)
            {
                T obj = new T();
                Type type = typeof(T);

                foreach (var kvp in dict)
                {
                    PropertyInfo property = type.GetProperty(kvp.Key);

                    if (property.PropertyType == typeof(int?))
                    {
                        try
                        { property.SetValue(obj, kvp.Value as int?); }
                        catch (Exception exx) { Logger.Error("Lỗi property1: " + exx); }
                    }
                    else
                    {
                        try
                        { property.SetValue(obj, Convert.ChangeType(kvp.Value, property.PropertyType)); }
                        catch (Exception exx) { Logger.Error("Lỗi property2: " + exx); }
                    }
                }
                objectList.Add(obj);
            }
        }
        catch (Exception ex)
        {
            Logger.Error("Lỗi ConvertDictionaryListToObjectList: " + ex);
        }
        return objectList;
    }


    public static Image Base64ToImage(string base64String)
    {

        // Convert Base64 String to byte[]
        byte[] imageBytes = Convert.FromBase64String(base64String);
        Bitmap tempBmp;
        using (MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
        {
            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            using (Image image = Image.FromStream(ms, true))
            {
                //Create another object image for dispose old image handler
                tempBmp = new Bitmap(image.Width, image.Height);
                Graphics g = Graphics.FromImage(tempBmp);
                g.DrawImage(image, 0, 0, image.Width, image.Height);
            }
        }
        return tempBmp;
    }

    // Use
    //    try
    //{
    //    Image img = Common.Base64ToImage(message.vehicle_snapshot);
    //  string fileName = imageFullFolder + imageName;
    //    if (System.IO.File.Exists(fileName))
    //        System.IO.File.Delete(fileName);
    //    //img.Save(fileName);

    //    Common.SaveJpeg(fileName, img, 30);
    //}
    //catch (Exception e)
    //{ }
    public static void SaveJpeg(string path, Image img, int quality)
    {
        //if (quality < 0 || quality > 100)
        //    throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");

        //// Encoder parameter for image quality 
        //EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
        //// JPEG image codec 
        //ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
        //EncoderParameters encoderParams = new EncoderParameters(1);
        //encoderParams.Param[0] = qualityParam;

        //img.Save(path, jpegCodec, encoderParams);



        int with = img.Width;
        int height = img.Height;
        if (with > 300)
        {
            with = 300;
            height = (int)((img.Height * with) / img.Width);
        }
        var resizedImg = ResizeImage(img, with, height);
        SaveJpegWithQuality(resizedImg, path, 50L);

    }

    public static void SaveJpeg1(string path, Image img, int quality)
    {
        if (quality < 0 || quality > 100)
            throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");

        // Encoder parameter for image quality 
        EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
        // JPEG image codec 
        ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
        EncoderParameters encoderParams = new EncoderParameters(1);
        encoderParams.Param[0] = qualityParam;

        img.Save(path, jpegCodec, encoderParams);
    }

    public static void SaveJpegWithQuality(Image img, string path, long quality)
    {
        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

        System.Drawing.Imaging.Encoder qualityEncoder = System.Drawing.Imaging.Encoder.Quality;
        EncoderParameters encoderParams = new EncoderParameters(1);
        EncoderParameter qualityParam = new EncoderParameter(qualityEncoder, quality);
        encoderParams.Param[0] = qualityParam;

        img.Save(path, jpgEncoder, encoderParams);
    }
    public static Image ResizeImage(Image img, int width, int height)
    {
        Bitmap b = new Bitmap(width, height);
        using (Graphics g = Graphics.FromImage(b))
        {
            g.DrawImage(img, 0, 0, width, height);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;

        }
        return (Image)b;
    }
    private static ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        // Get image codecs for all image formats 
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

        // Find the correct image codec 
        for (int i = 0; i < codecs.Length; i++)
            if (codecs[i].MimeType == mimeType)
                return codecs[i];

        return null;
    }
    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }



    /// <summary>
    /// Download ảnh từ link
    /// </summary>
    /// <param name="url"></param>
    /// <param name="savePath"></param>
    /// <returns></returns>
    public static async Task DownloadAndSaveImage(string url, string savePath)
    {
        using (HttpClient client = new HttpClient())
        {
            // Tải dữ liệu từ URL
            byte[] imageData = await client.GetByteArrayAsync(url);

            // Chuyển dữ liệu byte thành hình ảnh và lưu nó
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                using (Image img = Image.FromStream(ms))
                {
                    if (File.Exists(savePath))
                        File.Delete(savePath);
                    //img.Save(savePath);
                    SaveJpeg1(savePath, img, 90);
                }
            }
        }
    }





}
