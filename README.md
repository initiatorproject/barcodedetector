# BarcodeDetector

BarcodeDetector is a simple library for detecting barcodes in an image. It uses Emgu and OpenCV for the heavy lifting.
OpenCV can be obtuse. As such, one of the goals here is to provide code that is understandable without expertise in OpenCV or computer vision. To aid in this full type names are used and methods are called with explicit parameter names. Effort has been made to make method names self-explanatory.

Initially, it was felt that the program wasn't long enough to warrant a lot of structure so it was designed so that methods follow a functional pattern that is chaining friendly. Rather than try to wrap the multitude of types provided by Emgu and OpenCV we have instead made use of C#'s extension mechanism to glue our methods in where appropriate.

Please feel free to give feedback, to contribute code changes, and make use of this code in your own projects.
