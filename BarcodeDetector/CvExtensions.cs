namespace InitiatorProject.Barcode
{
 /// <remarks>
 /// Tested with Emgu.CV 4.2.0
 /// </remarks>
 public static class CvExtensions
 {
  public static System.Double CalculateContourArea(this Emgu.CV.Util.VectorOfPoint countourPoints,
                                                   System.Boolean orientationSigned = false)
  {
   if (countourPoints is null)
   {
    throw new System.ArgumentNullException(nameof(countourPoints));
   }

   return Emgu.CV.CvInvoke.ContourArea(contour: countourPoints,
                                       oriented: orientationSigned);
  }

  public static Emgu.CV.Image<TColor, TDepth> Clip<TColor, TDepth>(this Emgu.CV.Image<TColor, TDepth> image,
                                                                   System.Drawing.Rectangle boundingBox)
   where TColor : struct, Emgu.CV.IColor
   where TDepth : new()
  {
   if (image is null)
   {
    throw new System.ArgumentNullException(nameof(image));
   }

   return image.GetSubRect(rect: boundingBox);
  }

  public static Emgu.CV.Image<TColor, TDepth> DrawBoxOutline<TColor, TDepth>(this Emgu.CV.Image<TColor, TDepth> image,
                                                                             System.Drawing.Rectangle boundingBox,
                                                                             TColor? outlineColor = null)
   where TColor : struct, Emgu.CV.IColor
   where TDepth : new()
  {
   if (image is null)
   {
    throw new System.ArgumentNullException(nameof(image));
   }

   var outputImage = image.Clone();
   outputImage.Draw(rect: boundingBox,
                    color: outlineColor ?? (TColor)Settings.OutlineColor,
                    thickness: Settings.OutlineThickness);
   return outputImage;
  }

  public static Emgu.CV.Util.VectorOfVectorOfPoint FindContours(this Emgu.CV.IInputOutputArray image,
                                                                Emgu.CV.CvEnum.ChainApproxMethod method)
  {
   var contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
   Emgu.CV.CvInvoke.FindContours(image: image,
                                 contours: contours,
                                 hierarchy: null,
                                 mode: Emgu.CV.CvEnum.RetrType.External,
                                 method: method);
   return contours;
  }

  public static Emgu.CV.Structure.RotatedRect FindMinAreaRect(this Emgu.CV.Util.VectorOfPoint points)
  {
   if ((points is null) || (points.Size < 1))
   {
    return Emgu.CV.Structure.RotatedRect.Empty;
   }

   return Emgu.CV.CvInvoke.MinAreaRect(points: points);
  }

  public static System.Drawing.Rectangle GetBoundingBox(this Emgu.CV.Structure.RotatedRect rotatedRect,
                                                        System.Int32 maxWidth,
                                                        System.Int32 maxHeight)
  {
   var minAreaRect = rotatedRect.MinAreaRect();

   return new System.Drawing.Rectangle(x: System.Math.Max(0, minAreaRect.X - 5),
                                       y: System.Math.Max(0, minAreaRect.Y - 5),
                                       width: System.Math.Min(minAreaRect.Width + 10, maxWidth),
                                       height: System.Math.Min(minAreaRect.Height + 10, maxHeight));
  }

  public static Emgu.CV.Image<TColor, TDepth> GetGradient<TColor, TDepth>(this Emgu.CV.Image<TColor, TDepth> image)
   where TColor : struct, Emgu.CV.IColor
   where TDepth : new()
  {
   var gradientXImage = image.GetGradientX();

   var gradientYImage = image.GetGradientY();

   return gradientXImage.Sub(gradientYImage)
                        .ConvertScale<TDepth>(scale: 1,
                                              shift: 0)
                        .SmoothBlur(width: 9,
                                    height: 9);
  }

  public static Emgu.CV.Image<TColor, System.Single> GetGradientX<TColor, TDepth>(this Emgu.CV.Image<TColor, TDepth> image)
   where TColor : struct, Emgu.CV.IColor
   where TDepth : new()
  {
   if (image is null)
   {
    throw new System.ArgumentNullException(nameof(image));
   }

   return image.Sobel(xorder: 1, yorder: 0, apertureSize: -1);
  }

  public static Emgu.CV.Image<TColor, System.Single> GetGradientY<TColor, TDepth>(this Emgu.CV.Image<TColor, TDepth> image)
   where TColor : struct, Emgu.CV.IColor
   where TDepth : new()
  {
   if (image is null)
   {
    throw new System.ArgumentNullException(nameof(image));
   }

   return image.Sobel(xorder: 0, yorder: 1, apertureSize: -1);
  }

  public static Emgu.CV.Image<Emgu.CV.Structure.Gray, System.Byte> GetGrayscale(this Emgu.CV.Mat sourceMat)
  {
   if (sourceMat is null)
   {
    throw new System.ArgumentNullException(nameof(sourceMat));
   }

   return sourceMat.ToImage<Emgu.CV.Structure.Gray, System.Byte>();
  }

  public static Emgu.CV.Structure.RotatedRect GetLargestFeature(this Emgu.CV.IInputOutputArray image)
  {
   using (var contours = image.FindContours(method: Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple))
   {
    var bigSize = 0d;
    Emgu.CV.Util.VectorOfPoint? bigPoints = null;

    for (var x = 0; x < contours.Size; x++)
    {
     var points = contours[x];
     var size = points.CalculateContourArea();

     if (size > bigSize)
     {
      bigSize = size;
      bigPoints = points;
     }
    }

    return bigPoints?.FindMinAreaRect() ?? Emgu.CV.Structure.RotatedRect.Empty;
   }
  }

  public static Emgu.CV.Mat GetStructuringElement(this System.Drawing.Point anchorPoint,
                                                  Emgu.CV.CvEnum.ElementShape elementShape,
                                                  System.Drawing.Size elementSize)
  {
   return Emgu.CV.CvInvoke.GetStructuringElement(shape: elementShape,
                                                 ksize: elementSize,
                                                 anchor: anchorPoint);
  }

  public static Emgu.CV.Image<Emgu.CV.Structure.Gray, System.Byte> GetThreshold(this Emgu.CV.Image<Emgu.CV.Structure.Gray, System.Byte> image)
  {
   if (image is null)
   {
    throw new System.ArgumentNullException(nameof(image));
   }

   return image.ThresholdBinary(threshold: Settings.ThresholdMinGray,
                                maxValue: Settings.ThresholdMaxGray);
  }

  public static Emgu.CV.Image<TColor, TDepth> MorphClose<TColor, TDepth>(this Emgu.CV.Image<TColor, TDepth> image,
                                                                         Emgu.CV.IInputArray kernel,
                                                                         System.Drawing.Point? anchorPoint = null,
                                                                         System.Int32 iterations = 1,
                                                                         Emgu.CV.CvEnum.BorderType borderType = Emgu.CV.CvEnum.BorderType.Default,
                                                                         Emgu.CV.Structure.MCvScalar? borderValue = null)
   where TColor : struct, Emgu.CV.IColor
   where TDepth : new()
  {
   if (image is null)
   {
    throw new System.ArgumentNullException(nameof(image));
   }

   return image.MorphologyEx(operation: Emgu.CV.CvEnum.MorphOp.Close,
                             kernel: kernel,
                             anchor: anchorPoint ?? Settings.CenterAnchorPoint,
                             iterations: iterations,
                             borderType: borderType,
                             borderValue: borderValue ?? new Emgu.CV.Structure.MCvScalar());
  }

  public static Emgu.CV.Image<Emgu.CV.Structure.Gray, System.Byte> RemoveHoles(this Emgu.CV.Image<Emgu.CV.Structure.Gray, System.Byte> image)
  {
   if (image is null)
   {
    throw new System.ArgumentNullException(nameof(image));
   }

   var structuringElement = Settings.CenterAnchorPoint.GetStructuringElement(elementShape: Emgu.CV.CvEnum.ElementShape.Rectangle,
                                                                             elementSize: Settings.StructuringElementSize);
   return image.MorphClose(kernel: structuringElement);
  }

  public static Emgu.CV.Image<Emgu.CV.Structure.Gray, System.Byte> RemoveSpecks(this Emgu.CV.Image<Emgu.CV.Structure.Gray, System.Byte> image)
  {
   if (image is null)
   {
    throw new System.ArgumentNullException(nameof(image));
   }

   return image.Erode(iterations: 4)
               .Dilate(iterations: 4);
  }

  public static Emgu.CV.Image<Emgu.CV.Structure.Gray, System.Byte> SimplifyImage(this Emgu.CV.Image<Emgu.CV.Structure.Gray, System.Byte> image)
  {
   if (image is null)
   {
    throw new System.ArgumentNullException(nameof(image));
   }

   return image.GetThreshold()
               .RemoveHoles()
               .RemoveSpecks();
  }

  internal static class Settings
  {
   public static System.Drawing.Point CenterAnchorPoint { get; } = new System.Drawing.Point(-1, -1);

   public static Emgu.CV.IColor OutlineColor { get; } = new Emgu.CV.Structure.Bgr(0, 255, 0);

   public static System.Int32 OutlineThickness { get; } = 3;

   public static System.Drawing.Size StructuringElementSize { get; } = new System.Drawing.Size(21, 7);

   public static Emgu.CV.Structure.Gray ThresholdMaxGray { get; } = new Emgu.CV.Structure.Gray(255);

   public static Emgu.CV.Structure.Gray ThresholdMinGray { get; } = new Emgu.CV.Structure.Gray(225);
  }
 }
}