using SourceAFIS;

namespace FingerPrintMatch;

public class FingerPrint
{
    public static string DatabasePath = "D:\\Work\\Xs\\AIO_Edge\\Database\\";
    public static string TemplatePath = "D:\\Work\\Xs\\AIO_Edge\\FP2Match\\";
    public static int MatchThreshold = 12;

    private static FingerprintTemplate GetTemplate(string path)
    {
        var file = File.ReadAllBytes(path);
        var img = new FingerprintImage(file);
        return new FingerprintTemplate(img);
    }

    private static List<Subject> LoadDb(string username)
    {
        List<Subject> subjects = new();
        var directory = DatabasePath + username;
        var files = Directory.GetFiles(directory);
        if (files.Length > 0)
        {
            foreach (var file in files)
            {
                Subject subject = new Subject(0, username, GetTemplate(file), 0, false);
                subjects.Add(subject);
            }
        }
        return subjects;
    }

    private static Subject? Identify(FingerprintTemplate probe, List<Subject?> candidates)
    {
        var matcher = new FingerprintMatcher(probe);
        Subject? match = null;
        double max = double.NegativeInfinity;
        foreach (var candidate in candidates)
        {
            var similarity = matcher.Match(candidate?.Template);
            if (similarity > max)
            {
                max = similarity;
                match = candidate;
            }
        }
        double threshold = MatchThreshold; //12;
        if (match != null)
        {
            match.Confidence = ((int)max);
            if (max >= threshold)
            {
                match.Matched = true;
            }
            else
            {
                match.Matched = false;
            }
        }
        return match;
    }


    private static int LoadConfiguration(String filename)
    {
        //BufferedReader reader;
        //int count = 0;
        //int index = -1;
        //try
        //{
        //    reader = new BufferedReader(new FileReader(filename));
        //    while (1 == 1)
        //    {
        //        String line = reader.readLine();
        //        if (line == null) return count;
        //        if ((index = line.indexOf("DATABASE_PATH=")) >= 0)
        //        {
        //            count++;
        //            DATABASE_PATH = line.substring(index + 14);
        //            System.out.println("DATEBASE PATH=" + DATABASE_PATH);
        //        }
        //        else if ((index = line.indexOf("TEMPLATE_PATH=")) >= 0)
        //        {
        //            count++;
        //            TEMPLATE_PATH = line.substring(index + 14);
        //            System.out.println("TEMPLATE PATH=" + TEMPLATE_PATH);
        //        }
        //        else if ((index = line.indexOf("MATCH_THRESHOLD=")) >= 0)
        //        {
        //            count++;
        //            MATCH_THRESHOLD = Integer.parseInt(line.substring(index + 16));
        //            System.out.println("MATCH_THRESHOLD=" + MATCH_THRESHOLD);
        //        }
        //    }
        //}
        //catch (Exception e)
        //{
        //    System.out.println("ehem ehem");
        //}
        return 0;
    }


    //FingerPrint.Process("belal");
    //FingerPrint.Process("naveed");
    //FingerPrint.Process("umar");
    //FingerPrint.Process("unknown");
    //FingerPrint.Process("uon");
    public static bool Process(string userName)
    {
        var n = LoadConfiguration("jconfig.txt");
        try
        {
            Console.WriteLine("Loading Database username=" + userName);
            var candidates = LoadDb(userName);
            Console.WriteLine("Database loaded");
            
            var probe = File.ReadAllBytes(TemplatePath + "input.bmp");
            var probeImg = new FingerprintImage(probe);
            var probeTemplate = new FingerprintTemplate(probeImg);
            
            Subject subject = Identify(probeTemplate, candidates);
            if (subject != null)
            {
                Console.WriteLine("Match\tFilename: " + subject.Name + "\tConfidence: " + subject.Confidence +
                                  "\tMatched: " + subject.Matched);
                return subject.Matched;
            }
            else
            {
                Console.WriteLine("Couldn't Match");
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("error occurred: " + e.Message);
            return false;
        }
        finally
        {

        }
    }
}

public class Subject
{
    public Subject(int id, string name, FingerprintTemplate template, int confidence,
        bool matched)
    {
        Id = id;
        Name = name;
        Template = template;
        Confidence = confidence;
        Matched = matched;
    }

    public int Id { get; init; }
    public string Name { get; init; }
    public FingerprintTemplate Template { get; init; }
    public int Confidence { get; set; }
    public bool Matched { get; set; }

    public void Deconstruct(out int Id, out string Name, out FingerprintTemplate Template, out int Confidence, out bool Matched)
    {
        Id = this.Id;
        Name = this.Name;
        Template = this.Template;
        Confidence = this.Confidence;
        Matched = this.Matched;
    }
}