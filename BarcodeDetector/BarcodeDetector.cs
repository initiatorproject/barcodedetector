namespace InitiatorProject.Barcode
{
 public static class BarcodeDetector
 {
  /// <summary>
  /// Find a barcode in image. Returns a bounding box.
  /// </summary>
  /// <param name="file"></param>
  /// <returns></returns>
  public static System.Drawing.Rectangle FindBarcodeBoundingBox(System.IO.FileInfo file)
  {
   if (file is null)
   {
    throw new System.ArgumentNullException(nameof(file));
   }

   using (var sourceMat = new Emgu.CV.Mat(fileName: file.FullName))
   {
    return sourceMat.GetBarcodeBoundingBox();
   }
  }

  /// <summary>
  /// Find a barcode in image. Returns a bounding box.
  /// </summary>
  /// <param name="sourceMat"></param>
  /// <returns>bounding box</returns>
  public static System.Drawing.Rectangle GetBarcodeBoundingBox(this Emgu.CV.Mat sourceMat)
  {
   if (sourceMat is null)
   {
    throw new System.ArgumentNullException(nameof(sourceMat));
   }

   return sourceMat.GetGrayscale()
                   .GetGradient()
                   .SimplifyImage()
                   .GetLargestFeature()
                   .GetBoundingBox(maxWidth: sourceMat.Width,
                                   maxHeight: sourceMat.Height);
  }
 }
}