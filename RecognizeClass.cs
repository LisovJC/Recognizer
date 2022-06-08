using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using IronOcr;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Recognizer
{
    public interface IRecognizerF
    {
        string RecognizeArrF(List<string> paths, string data);
        string RecognizeF(string pathImage, string data);
    }

    public interface IRecognizerT
    {
        string RecognizeArrT(List<string> paths, string data);
        string RecognizeT(string pathImage, string data);
    }

    public interface IMedianFilter
    {
        string medianFilter(string pathImage);
    }

    public class RecognizerF : IRecognizerF
    {
        public string rus_modelPath = $"{Environment.CurrentDirectory}+\\Model_ru\\rus.traineddata";
        public string RecognizeArrF(List<string> paths, string data)
        {
            Tesseract tesseract = new Tesseract(rus_modelPath, "rus", OcrEngineMode.TesseractLstmCombined); //Используется кроме библиотеки скаченная модель языка
            for (int i = 0; i < paths.Count; i++)
            {
                tesseract.SetImage(new Image<Bgr, byte>(paths[i]));

                tesseract.Recognize();

                data += tesseract.GetUTF8Text();

                tesseract.Dispose();
            }
            return data;
        }

        public string RecognizeF(string pathImage, string data)
        {         
                Tesseract tesseract = new Tesseract(rus_modelPath, "rus", OcrEngineMode.TesseractLstmCombined); //Используется кроме библиотеки скаченная модель языка
                tesseract.SetImage(new Image<Bgr, byte>(pathImage));

                tesseract.Recognize();

                data = tesseract.GetUTF8Text();

                tesseract.Dispose();           
            return data;
        }
    }

    public class RecognizerT : IRecognizerT
    {
        public string RecognizeArrT(List<string> paths, string data)
        {
            try
            {
                IronTesseract IronOcr = new IronTesseract(); //Только библиотека nuGet
                for (int i = 0; i < paths.Count; i++)
                {
                    IronOcr.Language = OcrLanguage.RussianBest;

                    var Result = IronOcr.Read(paths[i]);
                    data += Result.Text;
                }
                return data;
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message);
                return data;
            }
        }

        public string RecognizeT(string pathImage, string data)
        {
            try
            {
                IronTesseract IronOcr = new IronTesseract(); //Только библиотека nuGet
                IronOcr.Language = OcrLanguage.RussianBest;

                var Result = IronOcr.Read(pathImage);
                data = Result.Text;
                return data;
            }
            catch (Exception e) 
            {
                Debug.Fail(e.Message);
                return data;
            }
        }
    }

    public class MedianFilter : IMedianFilter
    {
        public string medianFilter(string pathImage)
        {
            try
            {
                IplImage image, img;
                image = Cv.LoadImage(pathImage, LoadMode.Color);

                img = Cv.CreateImage(image.Size, BitDepth.U8, 3);
                Cv.Smooth(image, img, SmoothType.Median, 3, 3);
                Cv.SaveImage("filtering_img", img);
                return "filtering_img";
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message);
                return "";
            }
        }
    }
}
