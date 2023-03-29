// See https://aka.ms/new-console-template for more information

using FingerPrintMatch;
using SourceAFIS;



Console.WriteLine("Hello, World!");

//var encoded = File.ReadAllBytes("fingerprint.png");
//var decoded = new FingerprintImage(encoded);

FingerPrint.Process("belal");
FingerPrint.Process("naveed");
FingerPrint.Process("umar");
FingerPrint.Process("unknown");
FingerPrint.Process("uon");

Console.WriteLine("Done");
Console.ReadLine();