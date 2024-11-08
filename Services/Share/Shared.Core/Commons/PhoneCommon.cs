namespace Shared.Core.Commons;

public class PhoneCommon
{
    /// <summary>
    /// Chuẩn hóa đinh dạng theo số điện thoại của Vietnam
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    public static bool ConvertToVietNam(ref string phone)
    {
        if (phone == null)
            phone = "";
        string newphone = "";
        foreach (var p in phone)
            if ("0123456789".Contains(p))
                newphone += p;
        return newphone.Length == 10;
    }

    /// <summary>
    /// Convert chuẩn hóa số điện thoại 0987765565
    /// </summary>
    /// <param name="phone"></param>
    /// <returns></returns>
    public static string ConvertFormatPhone(string phone)
    {
        string telephone = "";
        try
        {
            if (!string.IsNullOrWhiteSpace(phone))
            {
                phone = phone?.Trim().Replace(".", "").Replace("-", "").Replace(",", "").Replace(" ", "").Replace("+84", "0");

                foreach (var p in phone)
                {
                    if ("0123456789".Contains(p))
                    {
                        telephone += p;
                    }
                }
            }
        }
        catch (Exception ex) { }
        return telephone;
    }

    public static async Task<Result<string>> SendSMSToken(string phone, string sms_content)
    {
        try
        {
            //var client = new HttpClient();
            //var request = new HttpRequestMessage(HttpMethod.Post, "http://hnoffice.acs.vn:8009/api/ViettelSMS/Send");
            //var content = new StringContent("{\r\n  \"phone\": \"" + phone + "\",\r\n  \"smsContent\": \"" + sms_content + "\"\r\n}", null, "application/json");
            //request.Content = content;
            //var response = await client.SendAsync(request);
            //var retContent = await response.Content.ReadAsStringAsync();
            //if (response.StatusCode != System.Net.HttpStatusCode.OK)
            //    return new Result<string>($"Lỗi khi gửi SMS: {retContent}", false);

            //return new Result<string>($"Thành công!: {retContent}", true);

            return new Result<string>($"Thành công!:", true);
        }
        catch (Exception ex)
        {
            return new Result<string>($"Lỗi khi gửi SMS: {ex.Message}", false);
        }

    }
}

public class EmailCommon
{
    /// <summary>
    /// Kiểm tra email có đúng định dạng email không
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool Validate(ref string email)
    {
        if (email == null)
            return false;
        email = email.Replace(" ", "");
        var emailarr = email.Split('@');
        if (emailarr.Length != 2)
            return false;

        if (emailarr[1].Split('.').Length < 2)
            return false;

        return true;
    }
}
