using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Shared.Core.Images;

public class ImageUtil
{
    public static void CreateZoomImage(string originalFilePath, string resizedFilePath, int newWidth, float quality)
    {
        if (quality > 1)
        {
            throw new ArgumentException("Quality has to be between 0 and 1");
        }

        using (var originalImage = Image.FromFile(originalFilePath))
        {
            int newHeight = (originalImage.Width > originalImage.Height) ?
                (newWidth * originalImage.Height) / originalImage.Width :
                (newWidth * originalImage.Width) / originalImage.Height;

            using (var resizedImage = new Bitmap(newWidth, newHeight))
            {
                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.Clear(Color.White);
                    graphics.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                }

                var qualityParam = new EncoderParameter(Encoder.Quality, (long)(quality * 100));
                var jpegCodec = GetEncoder(ImageFormat.Jpeg);
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = qualityParam;

                resizedImage.Save(resizedFilePath, jpegCodec, encoderParams);
            }
        }
    }

    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        var codecs = ImageCodecInfo.GetImageDecoders();
        foreach (var codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }

}
