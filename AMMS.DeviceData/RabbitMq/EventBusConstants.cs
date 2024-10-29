namespace AMMS.DeviceData.RabbitMq;

public static class EventBusConstants
{
    public static string DataArea { get; private set; } = @"MQDevice";

    #region Web
    /// <summary>
    /// Phản hồi dữ liệu chấm công, ảnh chụp lên web
    /// </summary>
    public static string Data_Auto_Push_D2S { get; private set; } = @"_data-auto-push_d2s";
    /// <summary>
    /// Lệnh web đẩy xuống service thiết bị
    /// </summary>
    public static string Server_Auto_Push_S2D { get; private set; } = @"_server-auto-push_s2d";
    /// <summary>
    /// Phản hồi của sevice lên web sau khi thực hiện lệnh
    /// </summary>
    public static string Device_Auto_Push_D2S { get; private set; } = @"_device-auto-push_d2s";

    #endregion

    #region Zkteco
    /// <summary>
    /// Lệnh web đẩy xuống service thiết bị zkteco
    /// </summary>
    public static string ZK_Server_Push_S2D { get; private set; } = @"_zk-server-push_s2d";
    /// <summary>
    /// Phản hồi của thiết bị về service khi thực hiện lệnh
    /// </summary>
    public static string ZK_Response_Push_D2S { get; private set; } = @"_zk-response-push_d2s";
    /// <summary>
    /// Phản hồi của web khi có sự kiện từ máy chấm công : chấm công, ảnh
    /// </summary>
    public static string ZK_Auto_Push_D2S { get; private set; } = @"_zk-auto-push_d2s";

    #endregion

    #region Hanet
    /// <summary>
    /// Lệnh web đẩy xuống service thiết bị zkteco
    /// </summary>
    public static string Hanet_Server_Push_S2D { get; private set; } = @"_hanet-server-push_s2d";
    /// <summary>
    /// Phản hồi của thiết bị về service khi thực hiện lệnh
    /// </summary>
    public static string Hanet_Device_Push_D2S { get; private set; } = @"_hanet-device-push_d2s";
    /// <summary>
    /// Phản hồi của web khi có sự kiện từ máy chấm công : chấm công, ảnh
    /// </summary>
    public static string Hanet_Auto_Push_D2S { get; private set; } = @"_hanet-auto-push_d2s";


    #endregion
}
