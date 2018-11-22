using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CPPClassMaker
{
    class Handler
    {

        string systemPath;
        string className;
        string cppFileName;
        string hFileName;
        string definedName;

        bool isRunning;
        uint state;

        public Handler()
        {
            state = 0;
            isRunning = true;
        }

        public void loop()
        {
            while (isRunning)
            {
                switch (state)
                {
                    case 0: // Show directory input message
                        Console.WriteLine("Enter the file path for the class:");
                        state++;
                        break;
                    case 1: // Get directory as input
                        string directory = Console.ReadLine();
                        if (validSystemPath(directory))
                        {
                            systemPath = directory;
                            state++;
                        }
                        else
                        {
                            Console.WriteLine("Please enter a valid system path!");
                        }
                        break;
                    case 2: // Class name input
                        Console.WriteLine("Enter your desired classname. You should try to use camelcase (I.E: Enemy or FileReader). Do not put file extensions here!");
                        className = Console.ReadLine();
                        state++;
                        break;
                    case 3: // Choice for advanced options
                        Console.WriteLine("Do you want to set custom define and file names? ('y' or 'n')");
                        string advanced = Console.ReadLine();
                        if (advanced == "y") // Loop through advanced states
                        {
                            state++;
                        }
                        else if (advanced == "n") // Skip a couple states and fill them in for the user
                        {
                            string baseFileName = className;
                            string firstLetter = baseFileName.Substring(0, 1);
                            firstLetter.ToUpper();
                            baseFileName = firstLetter + baseFileName.Remove(0,1);
                            setDefinedName(className.ToUpper() + "_H");
                            setFileNames(baseFileName);
                            state = 6;
                        }
                        else
                        {
                            Console.WriteLine("Please awnswer with 'y' or 'n'!");
                        }
                        break;
                    case 4: // Input for filename
                        Console.WriteLine("Enter the desired filename. Try to usel lower camel case. (I.E: thingDoer) There is no need for adding an extension just the name for the file. (I.E: thingDoer)");
                        setFileNames(Console.ReadLine());
                        state++;
                        break;
                    case 5: // Input for define name
                        Console.WriteLine("Enter the desired Define name: Try to use full capital letters and end with '_H'.");
                        setDefinedName(Console.ReadLine());
                        state++;
                        break;
                    case 6: // Creating of our wonderfull class
                        if (createFiles())
                        {
                            isRunning = false;
                        }
                        else
                        {
                            Console.WriteLine("File already exists! use 'q' to quit");
                            if (Console.ReadLine() == "q")
                            {
                                isRunning = false;
                            }
                        }
                        break;
                }
            }

            bool validSystemPath(string str)
            {
                if (Directory.Exists(str))
                {
                    return true;
                }
                return false;
            }

            void setFileNames(string baseFileName)
            {
                cppFileName = "\\" + baseFileName + ".cpp";
                hFileName = "\\" + baseFileName + ".h";
            }

            void setDefinedName(string inputDefinedName)
            {
                definedName = inputDefinedName;
            }

             bool createFiles()
            {
                if(File.Exists(systemPath + cppFileName) || File.Exists(systemPath + hFileName))
                {
                    return false;
                }
                string[] hLines =
                {
                    "#ifndef " + definedName,
                    "#define " + definedName,
                    "",
                    "Class " + className,
                    "{",
                    "public:",
                    "",
                    "   // " + className + " constructor",
                    "   " + className + "();",
                    "",
                    "   // " + className + " destructor",
                    "   ~" + className + "();",
                    "private:",
                    "};",
                    "",
                    "#endif // !" + definedName
                };

                string[] cppLines =
                {
                    "#include " + "\"" + hFileName + "\"",
                    "",
                    className + "::" + className + "()",
                    "{",
                    "   ",
                    "}",
                    "",
                    className + "::" + "~" + className + "()",
                    "{",
                    "   ",
                    "}"

                };

                File.Create(systemPath + hFileName).Close();
                File.WriteAllLines(systemPath + hFileName, hLines);
                File.Create(systemPath + cppFileName).Close();
                File.WriteAllLines(systemPath + cppFileName, cppLines);
                return true;
            }

        }

    }
}
