using static InitiatorProject.Barcode.CvExtensions;

namespace TestHarness
{
 internal class Program
 {
  internal static void Main()
  {
   //System.Console.WriteLine ("Hello World!");
   RunExample(new System.IO.FileInfo("TEST.PNG"));
  }

  private static void RunExample(System.IO.FileInfo file)
  {
   if (file is null)
   {
    throw new System.ArgumentNullException(nameof(file));
   }

   var outputFilepath = System.IO.Path.ChangeExtension(file.FullName, $"OUT{file.Extension}");

   using (var sourceMat = new Emgu.CV.Mat(fileName: file.FullName))
   {
    var boundingBox = InitiatorProject.Barcode.BarcodeDetector.GetBarcodeBoundingBox(sourceMat: sourceMat);

    var outputImage = sourceMat.ToImage<Emgu.CV.Structure.Bgr, System.Single>();

    outputImage.DrawBoxOutline(boundingBox: boundingBox).Save(fileName: outputFilepath);
   }
  }
 }
}