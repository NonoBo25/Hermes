using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Graphics;
using Java.IO;
using Java.Nio;
using Java.Nio.Channels;
using Android.Provider;

namespace Hermes
{


    public class TensorflowClassifier
    {
        const int FloatSize = 4;
        const int PixelSize = 3;

        public float[] Classify(byte[] image)
        {
            var mappedByteBuffer = GetModelAsMappedByteBuffer();
            var interpreter = new Xamarin.TensorFlow.Lite.Interpreter(mappedByteBuffer);
            var tensor = interpreter.GetInputTensor(0);
            var shape = tensor.Shape();
            var width = shape[1];
            var height = shape[2];
            var byteBuffer = GetPhotoAsByteBuffer(image, width, height);
            var outputLocations = new float[1][] { new float[2] };
            var outputs = Java.Lang.Object.FromArray(outputLocations);
            interpreter.Run(byteBuffer, outputs);
            var classificationResult = outputs.ToArray<float[]>();
            return classificationResult[0];
        }

        private MappedByteBuffer GetModelAsMappedByteBuffer()
        {
            var assetDescriptor = Application.Context.Assets.OpenFd("model.tflite");
            var inputStream = new FileInputStream(assetDescriptor.FileDescriptor);
            var mappedByteBuffer = inputStream.Channel.Map(FileChannel.MapMode.ReadOnly, assetDescriptor.StartOffset, assetDescriptor.DeclaredLength);
            return mappedByteBuffer;
        }

        private ByteBuffer GetPhotoAsByteBuffer(byte[] image, int width, int height)
        {
            var bitmap = BitmapFactory.DecodeByteArray(image, 0, image.Length);
            var resizedBitmap = Bitmap.CreateScaledBitmap(bitmap, width, height, true);
            var modelInputSize = FloatSize * height * width * PixelSize;
            var byteBuffer = ByteBuffer.AllocateDirect(modelInputSize);
            byteBuffer.Order(ByteOrder.NativeOrder());
            var pixels = new int[width * height];
            resizedBitmap.GetPixels(pixels, 0, resizedBitmap.Width, 0, 0, resizedBitmap.Width, resizedBitmap.Height);
            var pixel = 0;
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var pixelVal = pixels[pixel++];
                    float c1 = pixelVal >> 16 & 0xFF;
                    float c2 = pixelVal >> 8 & 0xFF;
                    float c3 = pixelVal & 0xFF;
                    float avg = (c1 + c2 + c3) / 3;
                    byteBuffer.PutFloat(avg);
                    byteBuffer.PutFloat(avg);
                    byteBuffer.PutFloat(avg);
                }
            }
            bitmap.Recycle();
            return byteBuffer;
        }

    }

}