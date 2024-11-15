using Shared.Core.Loggers;

namespace AMMS.ZkAutoPush.Applications.V1;

public class IclockCommand
{
    public IclockCommand()
    {

    }
    public double Id { get; set; }
    public string SerialNumber { get; set; } = "";
    public string Command { get; set; } = "";
    public bool IsRequest { get; set; } = false;
    public bool IsSuccessed { get; set; } = false;

    public string UserID { get; set; } = "";
    public bool IsSystemCommand { get; set; } = false;

    public IclockDataTable DataTable { get; set; }

    public string DataId { get; set; }
    public string? ParentId { get; set; }
    public string Action { get; set; } = "";
    public DateTime RevicedTime { get; set; } = DateTime.Now;
    public DateTime CommitTime { get; set; } = DateTime.Now;
    public int? returnCode { get; set; }
}

public enum IclockDataTable
{
    None, All, A2NguoiIclockSyn, A2VanTayIclockSyn, A2NguoiIclockUserPicSyn
}

public class IclockOperarion
{
    public const string POWER_ON = "0";
    public const string POWER_OFF = "1";
    public const string FAIL_THE_VERIFICATION = "2";
    public const string GENERATE_AN_ALARM = "3";
    public const string ENTER_THE_MENU = "4";
    public const string ENROLL_A_FINGERPRINT = "6";
    public const string ENROLL_A_PASSWORD = "7";
    public const string ENROLL_A_HID_CARD = "8";
    public const string DELETE_A_USER = "9";
    public const string DELETE_A_FINGERPRINT = "10";
    public const string DELETE_A_PASSWORD = "11";
    public const string DELETE_AN_RF_CARD = "12";
    /// <summary>
    /// Xóa dữ liệu
    /// </summary>
    public const string DELETE_A_PHOTO = "13";
    public const string CREATE_AN_MF_CARD = "14";
    public const string ENROLL_AN_MF_CARD = "15";
    public const string REGISTER_AN_MF_CARD = "16";
    public const string ENROLL_A_USER = "30";
    public const string ENROLL_MORE_FINGERPRINT = "36";


    public const string ENROLL_AN_PHOTO = "69";

    public const string CMD_DELETE_USER = "";
    public const string ActionDelete = "Delete";

    public const string ActionUpdate = "Update";


    public static IclockCommand GenerateCommand(double id,string sn, string cmd, bool isSystemCommand, IclockDataTable dataTable, string userId, string action = "")
    {
        
        return new IclockCommand()
        {
            SerialNumber = sn,
            Command = string.Format("C:{0}:{1}", id, cmd),
            Id = id,
            IsRequest = false,
            IsSystemCommand = isSystemCommand,
            DataTable = dataTable,
            UserID = userId,
            Action = action
        };
    }

    public static IclockCommand CommandUploadUser(double id, string sn, string userId, string name, string privileges, string password, string card, string dataId)
    {
        try
        {

            long ucard = 0;
            try
            {
                ucard = long.Parse(card);
            }
            catch (Exception e)
            { }
            string cardConver = "";
            if (ucard > 0)
            {
                cardConver = ucard.ToString();
            }

            string cmd = string.Format("DATA UPDATE USERINFO PIN={0}\tName={1}\tPri={2}\tPasswd={3}\tCard={4}\tGrp=1\tTZ=0000000100000000\tVerify=-1\tViceCard=", userId, name, privileges, password, cardConver);

            return GenerateCommand(id,sn, cmd, false, IclockDataTable.A2NguoiIclockSyn, userId);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static IclockCommand CommandUploadUserFp(double id, string sn, string userId, string fpIndex, string templateFp)
    {
        try
        {
            int fpSize = templateFp.Length;
            int valid = 1;
            string cmd = string.Format("DATA FP PIN={0}\tFID={1}\tSize={2}\tValid={3}\tTMP={4}", userId, fpIndex, fpSize, valid, templateFp);
            return GenerateCommand(id,sn, cmd, false, IclockDataTable.A2VanTayIclockSyn, userId);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static IclockCommand CommandUploadUserPhoto(double id, string sn, string userId, string tmpUserPicBase64)
    {
        try
        {
            var bytearr = Convert.FromBase64String(tmpUserPicBase64);
            int size = bytearr.Count();
            string cmd = string.Format("DATA UPDATE USERPIC PIN={0}\tSize={1}\tContent={2}", userId, size, tmpUserPicBase64);
            //C:123:DATA UPDATE USERPIC PIN=3	Size={0}	Content={1}
            return GenerateCommand(id,sn, cmd, false, IclockDataTable.A2NguoiIclockUserPicSyn, userId);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static IclockCommand CommandUploadUserFace(double id, string sn, string userId, string tmpUserPicBase64)
    {
        try
        {
            var bytearr = System.Convert.FromBase64String(tmpUserPicBase64);
            int size = bytearr.Count();
            string cmd = string.Format("DATA UPDATE FACE PIN={0}\tFID={1}\tSIZE={2}\tVALID={3}\tTMP={4}", userId, 1, size, 1, tmpUserPicBase64);

            return GenerateCommand(id,sn, cmd, false, IclockDataTable.A2NguoiIclockUserPicSyn, userId);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static IclockCommand CommandUploadUserFaceV2(double id, string sn, string userId, string tmpUserPicBase64)
    {
        try
        {
            var bytearr = System.Convert.FromBase64String(tmpUserPicBase64);
            int size = bytearr.Count();
            string cmd = string.Format("DATA UPDATE BIODATA PIN={0}\tFID={1}\tNO=0\tDuress=0\tType=9\tMajorVer=5\tMinorVer=621\tVALID={3}\tFormat=0\tTmp={4}", userId, 0, size, 1, tmpUserPicBase64);

            return GenerateCommand(id,sn, cmd, false, IclockDataTable.A2NguoiIclockUserPicSyn, userId);
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public static IclockCommand CommandUploadUserFaceV3(double id, string sn, string userId, string tmpUserPicBase64)
    {
        try
        {
            var bytearr = System.Convert.FromBase64String(tmpUserPicBase64);
            int size = bytearr.Count();
            string cmd = $"DATA UPDATE BIOPHOTO PIN={userId}\tType={2}\tSize={size}\tContent={tmpUserPicBase64}";
            return GenerateCommand(id,sn, cmd, false, IclockDataTable.A2NguoiIclockUserPicSyn, userId);
        }
        catch (Exception e)
        {
            Logger.Warning(e.Message);
            return null;
        }
    }

    public static IclockCommand CommandDeleteUserPhoto(double id, string sn, string userId)
    {
        try
        {
            string cmd = string.Format("DATA DELETE USERPIC PIN={0}", userId);
            return GenerateCommand(id,sn, cmd, false, IclockDataTable.A2VanTayIclockSyn, userId, ActionDelete);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static IclockCommand CommandDeleteUser(double id, string sn, string userId)
    {
        try
        {
            string cmd = string.Format("DATA DEL_USER PIN={0}", userId);
            return GenerateCommand(id,sn, cmd, false, IclockDataTable.All, userId, ActionDelete);
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public static IclockCommand CommandGetUserInfo(double id, string sn, string userId)
    {
        try
        {
            //C: ${CmdID}: DATA${SP}QUERY${SP}USERINFO${SP}PIN=${XXX}
            string cmd = string.Format("DATA QUERY $ PIN={0}", userId);
            return GenerateCommand(id,sn, cmd, false, IclockDataTable.None, userId);
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public static IclockCommand CommandGetFp(double id, string sn, string userId, string fpIndex)
    {
        try
        {
            //C: ${CmdID}: DATA${SP}QUERY${SP}FINGERTMP${SP}PIN=${XXX}${HT}FingerID=${XXX}
            //Note:
            //PIN =${ XXX}: User ID
            //FingerID =${ XXX}: Finger number, valued from 0 – 9.

            string cmd = string.Format("DATA QUERY FINGERTMP PIN={0}\tFingerID={1}", userId, fpIndex);
            return GenerateCommand(id,sn, cmd, false, IclockDataTable.None, userId);
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public static IclockCommand CommandDeleteFp(double id, string sn, string userId, string fpIndex)
    {
        try
        {
            //string cmd = string.Format("DATA DEL_FP PIN ={0}\t{1}", userId, fpIndex);
            string cmd = string.Format("DATA DELETE FINGERTMP PIN={0}\t{1}", userId, fpIndex);
            return GenerateCommand(id,sn, cmd, false, IclockDataTable.A2VanTayIclockSyn, userId);
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public static IclockCommand CommandDeleteFace(double id, string sn, string userId)
    {
        try
        {
            string cmd = string.Format("DATA DELETE BIOPHOTO PIN={0}", userId);
            return GenerateCommand(id,sn, cmd, false, IclockDataTable.A2VanTayIclockSyn, userId);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static IclockCommand OnlineEnrollUserFp(double id, string sn, string userId, string fpIndex, int retry, int overwrite)
    {
        try
        {
            var cmd = string.Format("ENROLL_FP PIN={0}\tFID={1}\tRETRY={2}\tOVERWRITE={3}", userId, fpIndex, retry, overwrite);
            return GenerateCommand(id,sn, cmd, false, IclockDataTable.None, userId);
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public static IclockCommand OnlineEnrollUserCard(double id, string sn, string userId, int retry = 3)
    {
        try
        {
            var cmd = string.Format("ENROLL_MF PIN={0}\tRETRY={1}", userId, retry);
            return GenerateCommand(id,sn, cmd, false, IclockDataTable.None, userId);
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public static IclockCommand GetDeviceInfo(double id, string sn)
    {
        try
        {
            var cmd = "INFO";
            return GenerateCommand(id,sn, cmd, true, IclockDataTable.None, "");
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public static IclockCommand RebootDevice(double id, string sn)
    {
        try
        {
            var cmd = "REBOOT";
            return GenerateCommand(id,sn, cmd, true, IclockDataTable.None, "");
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public static IclockCommand ClearAttData(double id, string sn)
    {
        try
        {
            var cmd = "CLEAR LOG";
            return GenerateCommand(id,sn, cmd, true, IclockDataTable.None, "");
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    public static IclockCommand ClearPhotoData(double id, string sn)
    {
        try
        {
            var cmd = "CLEAR PHOTO";
            return GenerateCommand(id,sn, cmd, true, IclockDataTable.None, "");
        }
        catch (Exception e)
        {
            throw e;
        }
    }
    /// <summary>
    /// Xóa toàn bộ dữ kiệu trên thiết bị
    /// </summary>
    /// <param name="sn"></param>
    /// <returns></returns>
    public static IclockCommand ClearData(double id, string sn)
    {
        try
        {
            var cmd = "CLEAR DATA";
            return GenerateCommand(id,sn, cmd, true, IclockDataTable.None, "");
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static IclockCommand CheckingAndTransmittingNewData(double id, string sn)
    {
        try
        {
            var cmd = "LOG";
            return GenerateCommand(id,sn, cmd, true, IclockDataTable.None, "");
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public static IclockCommand CheckingDataUpdate(double id, string sn)
    {
        try
        {
            var cmd = "CHECK";
            return GenerateCommand(id,sn, cmd, true, IclockDataTable.None, "");
        }
        catch (Exception e)
        {
            throw e;
        }
    }


    public static string GetValueFromEqual(string key, List<string> sources)
    {
        try
        {
            foreach (var x in sources)
            {
                if (!string.IsNullOrEmpty(x))
                {
                    var arr = x.Split('=');
                    if (arr != null && arr.Length > 1)
                    {
                        if (key == arr[0])
                        {
                            return x.Replace(key + "=", "");
                        }
                    }
                }
            }
        }
        catch
        {
        }
        return "";
    }
}

 