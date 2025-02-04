﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;


namespace Redream
{
    internal class MyFunctions
    {
        public static int SearchValueInDatagridView(DataGridView grid, string searchValue, int cell = 0)
        {
            int rowIndex = -1;
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.Cells[cell].Value != null && row.Cells[cell].Value.ToString().ToLower() == searchValue.ToLower())
                {
                    rowIndex = row.Index;
                    break;
                }
            }
            return rowIndex;
        }

        public static int SearchValueInList(List<string> list, string searchValue)
        {
            int index = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == searchValue)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }



        public static string FindBestMatch(string input, string[] candidates)
        {
            int minDistance = int.MaxValue;
            string bestMatch = string.Empty;

            foreach (string candidate in candidates)
            {
                int distance = ComputeLevenshteinDistance(input, candidate);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestMatch = candidate;
                }
            }

            return bestMatch;
        }

        static int ComputeLevenshteinDistance(string source, string target)
        {
            int[,] distances = new int[source.Length + 1, target.Length + 1];

            for (int i = 0; i <= source.Length; i++)
                distances[i, 0] = i;

            for (int j = 0; j <= target.Length; j++)
                distances[0, j] = j;

            for (int i = 1; i <= source.Length; i++)
            {
                for (int j = 1; j <= target.Length; j++)
                {
                    int cost = source[i - 1] == target[j - 1] ? 0 : 1;

                    distances[i, j] = Math.Min(
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost
                    );
                }
            }

            return distances[source.Length, target.Length];
        }

        public static byte[] ImageToBytes(Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }



        public static Rectangle[] DetectAndApplyMask(Bitmap img)
        {
            Mat mat = img.ToMat();

            img.Save("temp.jpg");
            // Load the image using Emgu.CV
            Image<Bgr, byte> image = new Image<Bgr, byte>("temp.jpg");

            // Load the pre-trained face cascade classifier
            CascadeClassifier faceCascade = new CascadeClassifier("haarcascade_frontalface_alt.xml");
            //CascadeClassifier faceCascade = new CascadeClassifier("haarcascade_fullbody.xml");
            //CascadeClassifier faceCascade = new CascadeClassifier("haarcascade_upperbody.xml");


            Rectangle[] faces = new Rectangle[] { Rectangle.Empty };
            // Convert the image to grayscale for face detection
            using (var grayImage = image.Convert<Gray, byte>())
            {
                faces = faceCascade.DetectMultiScale(grayImage, 1.5, 3, Size.Empty);
            }
            //Bitmap maskOut = image.ToBitmap();

            return faces;
        }


    }
}
