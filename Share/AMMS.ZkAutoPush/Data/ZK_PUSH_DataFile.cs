using System.Drawing;
using System.IO;
using System.Text;

namespace AMMS.ZkAutoPush.Data;

public class ZK_PUSH_DataFile
{
    private static string GetStr(string new_str, string key, char sp)
    {
        int quest_mark_pos = 0;
        string key_str = string.Empty;
        string value_str = string.Empty;

        string[] split_str = new_str.Split(new char[1] { sp });

        for (int loop = 0; loop < split_str.Length; loop++)
        {
            quest_mark_pos = split_str[loop].IndexOf("=");

            if (quest_mark_pos >= 0)
            {
                key_str = split_str[loop].Substring(0, quest_mark_pos);

                value_str = split_str[loop].Substring(quest_mark_pos + 1, split_str[loop].Length - quest_mark_pos - 1);

                if (string.Compare(key_str, key, true) == 0)
                {
                    return value_str;
                }
            }
        }

        return string.Empty;
    }

    // if (table == "ATTPHOTO")
    //            {
    //                byte[] bReceive;
    //                using (var memoryStream = new MemoryStream()) { await HttpContext.Request.Body.CopyToAsync(memoryStream); bReceive = memoryStream.ToArray(); }

    //                string strReceive = Encoding.GetEncoding("utf-8").GetString(bReceive, 0, bReceive.Length);

    //SaveAttPhoto(strReceive, bReceive);


    //return Retval_OK;
    //            }

    public static string SaveAttPhoto(string strReceive, byte[] bReceive)
    {
        FileStream fs = null;

        try
        {
            string sep = string.Empty;
            int sep_length = 0;
            int strReceive_mark_pos = 0;

            if ((strReceive_mark_pos = strReceive.IndexOf("CMD=uploadphoto")) > 0)
            {
                sep = "CMD=uploadphoto";
                sep_length = 15;
            }
            else if ((strReceive_mark_pos = strReceive.IndexOf("CMD=realupload")) > 0)
            {
                sep = "CMD=realupload";
                sep_length = 14;
            }

            string new_str = strReceive.Substring(0, strReceive_mark_pos);

            string user_pin = GetStr(new_str, "PIN", '\n');
            int strReceive_len = int.Parse(GetStr(new_str, "Size", '\n'));

            if (strReceive_len <= 0)
            {
                return "Return Data Error";
            }

            //string file_path = string.Empty;

            //if (user_pin.Split(new char[1] { 'B' }).Length > 1)
            //{
            //Att Failed
            //    file_path = @"bin\Debug\net6.0\Logs\" + user_pin;
            //}
            //else
            //{
            //Att Success
            //    file_path = @"bin\Debug\net6.0\Logs\" + user_pin;
            //}
            //if (File.Exists(file_path))
            //{
            //    File.Delete(file_path);
            //}
            //fs = new FileStream(file_path, FileMode.Create, FileAccess.Write);
            //fs.Write(bReceive, strReceive.IndexOf(sep) + sep_length + 1, strReceive_len);

            var des = new byte[strReceive_len];
            Array.Copy(bReceive, strReceive.IndexOf(sep) + sep_length + 1, des, 0, strReceive_len);
            using (MemoryStream ms = new MemoryStream(des))
            {
                var img = Image.FromStream(ms);
            }
            //return string.Format("Return Data OK, File Path={0}", file_path);
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (fs != null)
            {
                fs.Close();
            }
        }
    }

    public static string SaveUserPhoto(string sPostContent)
    {
        FileStream fs = null;
        try
        {
            int sPostContent_mark_pos = 0;
            string markdata = "\tContent=";

            sPostContent_mark_pos = sPostContent.IndexOf(markdata);

            if (sPostContent_mark_pos < 0)
            {
                return "Return Data Error";
            }

            string new_str = sPostContent.Substring(0, sPostContent_mark_pos);

            string user_pin = GetStr(new_str, "PIN", '\t');
            int sPostContent_len = int.Parse(GetStr(new_str, "Size", '\t'));

            if (sPostContent_len <= 0)
            {
                return "Return Data Error";
            }

            string file_path = @".\DataFiles\Download\UserPhotos\" + user_pin + ".jpg";

            if (File.Exists(file_path))
            {
                File.Delete(file_path);
            }

            byte[] imgedata = Convert.FromBase64String(sPostContent.Substring(sPostContent_mark_pos + markdata.Length, sPostContent_len));

            fs = new FileStream(file_path, FileMode.Create, FileAccess.Write);
            fs.Write(imgedata, 0, imgedata.Length);

            return string.Format("Return Data OK, File Path={0}", file_path);
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            if (fs != null)
            {
                fs.Close();
            }
        }
    }

}
