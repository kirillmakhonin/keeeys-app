using Android.Graphics;
using ZXing;
using ZXing.Common;

namespace Keeeys.Droid.Helpers
{
    class QrBuilder
    {
        public static Bitmap BuildImage(string data, int width, int height)
        {
            EncodingOptions options = new EncodingOptions
            {
                Height = width,
                Width = height,
                PureBarcode = true,
                Margin = 0
            };
            options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");

            var writer = new ZXing.Mobile.BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = options

            };

            return writer.Write(data);
        }        
    }
}