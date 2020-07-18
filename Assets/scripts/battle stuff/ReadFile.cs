using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class ReadFile
{

    public static (List<float>, List<float>) readTextFile(string file_path)
    {
        List<float> kickBeats = new List<float>();
        List<float> snareBeats = new List<float>();

        StreamReader inp_stm = new StreamReader(file_path);

        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();
            // Do Something with the input. 
            switch (inp_ln[0])
            {
                case 'k':
                    string num = "";
                    for (int i = 1; i < inp_ln.Length; i++)
                    {
                        if (inp_ln[i] == '[')
                        {
                            //move forward one 
                            i++;
                            num += inp_ln[i];
                        }

                        else if (inp_ln[i] == ',' || inp_ln[i] == ']')
                        {

                            kickBeats.Add(float.Parse(num));
                            num = "";
                        }
                        else
                        {
                            num += inp_ln[i];
                        }
                    }
                    break;

                case 's':
                    string snum = "";
                    for (int i = 1; i < inp_ln.Length; i++)
                    {
                        if (inp_ln[i] == '[')
                        {
                            //move forward one 
                            i++;
                            snum += inp_ln[i];
                        }

                        else if (inp_ln[i] == ',' || inp_ln[i] == ']')
                        {
                            //end of a number 
                            //print(num);
                            snareBeats.Add(float.Parse(snum));
                            snum = "";
                        }
                        else
                        {
                            snum += inp_ln[i];
                        }
                    }
                    break;

                case 'l':
                    for (int i = 2; i < inp_ln.Length; i++)
                    {
                        // totalTimeStr += inp_ln[i];
                    }
                    // totalTime = float.Parse(totalTimeStr);
                    break;

            }

        }

        inp_stm.Close();

        return (kickBeats, snareBeats);

    }


}


