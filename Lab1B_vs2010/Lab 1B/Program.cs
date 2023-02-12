//Класс Monitor работает только в рамках одного процесса. Mutex'у же, можно задать идентификатор, который будет единственным для всей ОС.
//Например, это полезно, если у Вас запущено два разных приложения, но между ними должна происходить синхронизация (как пример, запись в файл и чтение).


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.IO.Pipes;

namespace MultiThread
{
    class Program
    {
       static Mutex mut = new Mutex();

       static object locker = new object();

       static bool stop = false;
       static string var_obm = "";
       static string vsinx = "";

       public static NamedPipeServerStream NamedPipeServer;
       public static NamedPipeClientStream NamedPipeClient;

       public static AnonymousPipeServerStream AnonPipeServer;
       public static AnonymousPipeClientStream AnonPipeClient;
       static public string pipeHandle;

       static object LockObj = new object();




       static bool Calculation(string command_name, string command_par, string str1, string str2, string str3)
       {
              char[] abc_eng = { 'a', 'A', 'b', 'B', 'c', 'C', 'd', 'D', 'e', 'E', 'f', 'F', 'g', 'G', 'h', 'H', 'i', 'I', 'j', 'J', 'k', 'K', 'l', 'L', 'm', 'M', 'n', 'N', 'o', 'O', 'p', 'P', 'q', 'Q', 'r', 'R', 's', 'S', 't', 'T', 'u', 'U', 'v', 'V', 'w', 'W', 'x', 'X', 'y', 'Y', 'z', 'Z' };
              bool flag;
              string[] words;
              int sum;
              int num;

              //Console.WriteLine("Calc");

              if (command_name == "s")
              {
                     if (command_par == "1")                   //Будет произведено объединение двух строк
                     {
                            Console.WriteLine("Результат: " + str1 + str2);
                     }
                     if (command_par == "2")                   //Будет произведен подсчет длины текста
                     {
                            Console.WriteLine("Результат: " + str1.Length);
                     }
                     if (command_par == "3")                   //Будет произведен подсчет средней длины слов строки (строка разделяется на слова с помощью звездочки)
                     {
                            words = str1.Split('*');
                            sum = 0;
                            foreach (var word in words)
                            {
                                   sum += word.Length;
                            }
                            Console.WriteLine("Количество слов:" + words.Count());
                            Console.WriteLine("Средняя длина:" + sum / words.Count());
                     }
                     if (command_par == "4")                   //Будет произведен подсчет количества слов из цифр (строка разделяется на слова с помощью двоеточия)
                     {
                            words = str1.Split(':');
                            sum = 0;
                            num = 0;
                            foreach (var word in words)
                            {
                                   bool success = int.TryParse(word, out num);
                                   if (success) sum++;
                            }
                            Console.WriteLine("Результат: " + sum);
                     }
                     if (command_par == "5")                   //Будет произведен подсчет суммы отрицательных чисел – слов строки (строка разделяется на слова с помощью запятой)
                     {
                            words = str1.Split(',');
                            sum = 0;
                            num = 0;
                            foreach (var word in words)
                            {
                                   bool success = int.TryParse(word, out num);
                                   if (success & num < 0) sum += num;
                            }
                            Console.WriteLine("Результат: " + sum);
                     }
                     if (command_par == "6")                   //Будет произведен подсчет числа 5-символьных слов из латинских букв в строке (строка разделяется на слова с помощью запятой)
                     {
                            words = str1.Split(',');
                            sum = 0;
                            foreach (var word in words)
                            {
                                   flag = true;
                                   foreach (char c in word)
                                   {
                                          if (!abc_eng.Contains(c))
                                          {
                                                 flag = false;
                                                 break;
                                          }
                                   }
                                   if (word.Length == 5 & flag) sum++;
                            }
                            Console.WriteLine("Результат: " + sum);
                     }
                     if (command_par == "7")                   //Будет произведено выделение подстроки заданной длины с начала строки.
                     {
                            num = Convert.ToInt32(str2);

                            Console.WriteLine("Результат: " + str1.Substring(0, num));
                     }
                     if (command_par == "8")                   //Будет произведен подсчет количества указанного символа в строке
                     {
                            num = 0;
                            foreach (char c in str1)
                            {
                                   if (c.ToString() == str2) num++;
                            }
                            Console.WriteLine("Результат: " + num);
                     }
                     if (command_par == "9")                   //Будет произведено сравнение двух строк и выдача количества несовпадающих символов
                     {
                            str3 = "";
                            num = 0;
                            foreach (char c in str1)
                            {
                                   if (!str3.Contains(c)) str3 = str3 + c;
                            }
                            foreach (char c in str2)
                            {
                                   if (!str3.Contains(c)) str3 = str3 + c;
                            }
                            foreach (char c in str3)
                            {
                                   if (str1.Contains(c) & str2.Contains(c)) { } else { num++; }
                            }
                            Console.WriteLine("Количество символов, которых нет в строке 1 и строке 2, равно: " + num);
                     }
                     if (command_par == "10")                  //Будет произведена вставка одной строки в другую с заданного символа
                     {
                            num = Convert.ToInt32(str3);

                            string s1_1 = str1.Substring(0, num);
                            string s1_2 = str1.Substring(num);
                            Console.WriteLine("Результат: " + s1_1 + str2 + s1_2);
                     }
              }
              if (command_name == "n")
              {
                     words = str1.Split(',');

                     if (command_par == "1")
                     {
                            Console.WriteLine("Будет произведен подсчет количества отрицательных элементов в массиве.");
                            sum = 0;
                            foreach (var word in words)
                            {
                                   int.TryParse(word, out num);
                                   if (num < 0) sum++;
                            }
                            Console.WriteLine("Результат: " + sum);
                     }
                     if (command_par == "2")
                     {
                            Console.WriteLine("Будет произведен подсчет количества неотрицательных элементов в массиве.");
                            sum = 0;
                            foreach (var word in words)
                            {
                                   int.TryParse(word, out num);
                                   if (num >= 0) sum++;
                            }
                            Console.WriteLine("Результат: " + sum);
                     }
                     if (command_par == "3")
                     {
                            Console.WriteLine("Будет произведен подсчет суммы положительных чисел массива.");
                            sum = 0;
                            foreach (var word in words)
                            {
                                   int.TryParse(word, out num);
                                   if (num >= 0) sum += num;
                            }
                            Console.WriteLine("Результат: " + sum);
                     }
                     if (command_par == "4")
                     {
                            Console.WriteLine("Будет произведен подсчет числа положительных элементов с четными индексами.");
                            sum = 0;
                            int i = 0;
                            foreach (var word in words)
                            {
                                   int.TryParse(word, out num);
                                   if (num >= 0 & (i % 2 == 0)) sum++;
                                   i++;
                            }
                            Console.WriteLine("Результат: " + sum);
                     }
                     if (command_par == "5")
                     {
                            Console.WriteLine("Будет произведена замена отрицательных элементов массива их квадратами.");
                            for (int j = 0; j <= words.Count() - 1; j++)
                            {
                                   num = Int16.Parse(words[j]);
                                   if (num < 0) words[j] = Convert.ToString(num * num);
                            }
                            Console.WriteLine("Результат: " + string.Join(",", words));
                     }
                     if (command_par == "6")
                     {
                            Console.WriteLine("Будет произведено удаление из массива четных элементов");
                            int i = 1;
                            str1 = "";
                            foreach (var word in words)
                            {
                                   if (i % 2 != 0)
                                   {
                                          if (str1 == "")
                                          {
                                                 str1 = word;
                                          }
                                          else
                                          {
                                                 str1 = str1 + "," + word;
                                          }
                                   }
                                   i++;
                            }
                            Console.WriteLine("Результат: " + str1);
                     }
              }
              
              return true;
       }
       
       static string[] AskInformation()
       {
              string[] comName = { "s", "n" };
              string[] arr_var1 = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
              string[] arr_var2 = { "1", "2", "3", "4", "5", "6" };
              string[] arr = arr_var1;

              string suggest_str = "";
              string err_str = "";
              string cname = "";
              string cpar = "";
              string s1 = "";
              string s2 = "";
              string s3 = "";
              string[] words;
              bool flag;
              int num;



              suggest_str = "Введите команду: s - обработка строк, n - обработка массива, q - выход.";
              err_str = "Введена некорректная команда. ";
              arr = comName;
              Console.WriteLine(suggest_str);
              do
              {
                     s1 = Console.ReadLine();
                     if (s1 == "q") Environment.Exit(0);
                     if (!arr.Contains(s1)) { Console.WriteLine(err_str + "\r\n" + suggest_str); } else { break; }
              }
              while (5 == 5);
              cname = s1;


              suggest_str = "Введите параметр команды (целое число от 1 до " + (cname == "s" ? "10" : "6") + ") или q для выхода.";
              err_str = "Введен некорректный параметр.";
              arr = (cname == "s" ? arr_var1 : arr_var2);
              Console.WriteLine(suggest_str);
              do
              {
                     s1 = Console.ReadLine();
                     if (s1 == "q") Environment.Exit(0);
                     if (!arr.Contains(s1)) { Console.WriteLine(err_str + "\r\n" + suggest_str); } else { break; }
              }
              while (5 == 5);
              cpar = s1;



              if (cname == "s")
              {
                     if (cpar == "1")
                     {
                            Console.WriteLine("Будет произведено объединение двух строк.");

                            do
                            {
                                   Console.WriteLine("Введите строку 1:");
                                   s1 = Console.ReadLine();
                            }
                            while (s1.Length == 0);

                            do
                            {
                                   Console.WriteLine("Введите строку 2:");
                                   s2 = Console.ReadLine();
                            }
                            while (s2.Length == 0);
                     }
                     if (cpar == "2")
                     {
                            Console.WriteLine("Будет произведен подсчет длины текста.");

                            do
                            {
                                   Console.WriteLine("Введите строку:");
                                   s1 = Console.ReadLine();
                            }
                            while (s1.Length == 0);
                     }
                     if (cpar == "3")
                     {
                            Console.WriteLine("Будет произведен подсчет средней длины слов строки (строка разделяется на слова с помощью звездочки).");

                            do
                            {
                                   Console.WriteLine("Введите строку:");
                                   s1 = Console.ReadLine();
                            }
                            while (s1.Length == 0);
                     }
                     if (cpar == "4")
                     {
                            Console.WriteLine("Будет произведен подсчет количества слов из цифр (строка разделяется на слова с помощью двоеточия).");

                            do
                            {
                                   Console.WriteLine("Введите строку:");
                                   s1 = Console.ReadLine();
                            }
                            while (s1.Length == 0);
                     }
                     if (cpar == "5")
                     {
                            Console.WriteLine("Будет произведен подсчет суммы отрицательных чисел – слов строки (строка разделяется на слова с помощью запятой).");

                            do
                            {
                                   Console.WriteLine("Введите строку:");
                                   s1 = Console.ReadLine();
                            }
                            while (s1.Length == 0);
                     }
                     if (cpar == "6")
                     {
                            Console.WriteLine("Будет произведен подсчет числа 5-символьных слов из латинских букв в строке (строка разделяется на слова с помощью запятой).");

                            do
                            {
                                   Console.WriteLine("Введите строку:");
                                   s1 = Console.ReadLine();
                            }
                            while (s1.Length == 0);
                     }
                     if (cpar == "7")
                     {
                            Console.WriteLine("Будет произведено выделение подстроки заданной длины с начала строки.");

                            do
                            {
                                   Console.WriteLine("Введите строку:");
                                   s1 = Console.ReadLine();
                            }
                            while (s1.Length == 0);

                            Console.WriteLine("Введите длину подстроки:");
                            do
                            {
                                   s2 = Console.ReadLine();

                                   flag = Int32.TryParse(s2, out num);
                                   if (!flag)
                                   {
                                          Console.WriteLine("Длина подстроки указана некорректно. Попробуйте еще раз.");
                                   }
                                   else
                                   {
                                          if (s1.Length >= num)
                                          {
                                                 break;
                                          }
                                          Console.WriteLine("Длина подстроки должна быть меньше длины строки. Попробуйте еще раз.");
                                   }
                            } while (5 == 5);
                     }
                     if (cpar == "8")
                     {
                            Console.WriteLine("Будет произведен подсчет количества указанного символа в строке.");

                            do
                            {
                                   Console.WriteLine("Введите строку:");
                                   s1 = Console.ReadLine();
                            }
                            while (s1.Length == 0);

                            do
                            {
                                   Console.WriteLine("Введите символ, количество вхождений которого необходиом посчитать:");
                                   s2 = Console.ReadLine();
                                   if (s2.Length != 1) Console.WriteLine("Необходимо ввести только один символ. Попробуйте еще раз.");
                            }
                            while (s2.Length != 1);
                     }
                     if (cpar == "9")
                     {
                            Console.WriteLine("Будет произведено сравнение двух строк и выдача количества несовпадающих символов.");

                            do
                            {
                                   Console.WriteLine("Введите строку1 :");
                                   s1 = Console.ReadLine();
                            }
                            while (s1.Length == 0);

                            do
                            {
                                   Console.WriteLine("Введите строку 2:");
                                   s2 = Console.ReadLine();
                            }
                            while (s2.Length == 0);
                     }
                     if (cpar == "10")
                     {
                            Console.WriteLine("Будет произведена вставка одной строки в другую с заданного символа.");

                            do
                            {
                                   Console.WriteLine("Введите строку 1:");
                                   s1 = Console.ReadLine();
                            }
                            while (s1.Length == 0);

                            do
                            {
                                   Console.WriteLine("Введите строку 2:");
                                   s2 = Console.ReadLine();
                            }
                            while (s2.Length == 0);


                            Console.WriteLine("Введите номер символа, после которого необходимо вставить строку 2 в строку 1:");
                            do
                            {
                                   s3 = Console.ReadLine();

                                   flag = Int32.TryParse(s3, out num);
                                   if (!flag)
                                   {
                                          Console.WriteLine("Номер символа указан некорректно. Попробуйте еще раз.");
                                   }
                                   else
                                   {
                                          if (s1.Length >= num)
                                          {
                                                 break;
                                          }
                                          Console.WriteLine("Номер символа должен быть меньше длины строки 1. Попробуйте еще раз.");
                                   }
                            } while (5 == 5);
                     }
              }
              if (cname == "n")
              {
                     do
                     {
                            Console.WriteLine("Введите массив чисел через запятую:");
                            s1 = Console.ReadLine();

                            words = s1.Split(',');
                            flag = true;
                            foreach (var word in words)
                            {
                                   flag = int.TryParse(word, out num);
                                   if (!flag) break;
                            }
                            if (!flag)
                            {
                                   Console.WriteLine("В ходе проверки выявлен некорректный элемент массива. Введите массив еще раз.");
                            }
                            else
                            {
                                   if (words.Count() < 2)
                                   {
                                          Console.WriteLine("Элементов массива должно быть не менее 2. Введите массив еще раз.");
                                   }
                                   else
                                   {
                                          break;
                                   }
                            }
                     } while (5 == 5);
              }

              string[] ret_arr = new string[] {cname, cpar, s1, s2, s3};

              return ret_arr;
       }
       
       
       public static void Second()
       {
              if (var_obm == "1")
              {
                     AnonPipeClient = new AnonymousPipeClientStream(PipeDirection.In, pipeHandle);
                     StreamReader Reader = new StreamReader(AnonPipeClient);
                     
                     Thread.Sleep(1000);
                     while (!stop)
                     {
                            //Console.WriteLine("Начал читать введенную информацию");
                            string command_name = Reader.ReadLine();
                            string command_par = Reader.ReadLine();
                            string str1 = Reader.ReadLine();
                            string str2 = Reader.ReadLine();
                            string str3 = Reader.ReadLine();
                            //Console.WriteLine("Закончил читать введенную информацию");


                            if (vsinx == "1")
                            {
                                   lock (LockObj)
                                   {
                                          Calculation(command_name, command_par, str1, str2, str3);
                                   }
                            }
                            else
                            {
                                   if (vsinx == "2") mut.WaitOne();
                                   if (vsinx == "3") Monitor.Enter(locker);
                                   Calculation(command_name, command_par, str1, str2, str3);
                                   if (vsinx == "2") mut.ReleaseMutex();
                                   if (vsinx == "3") Monitor.Exit(locker);
                            }
                            Thread.Sleep(100);
                     }
                     Reader.Close();
              }
              if (var_obm == "2")
              {
                     NamedPipeClient = new NamedPipeClientStream(".", "testpipe", PipeDirection.In);
                     NamedPipeClient.Connect();                     
                     StreamReader Reader = new StreamReader(NamedPipeClient);

                     Thread.Sleep(1000);
                     while (!stop)
                     {
                            //Console.WriteLine("Начал читать введенную информацию");
                            string command_name = Reader.ReadLine();
                            string command_par = Reader.ReadLine();
                            string str1 = Reader.ReadLine();
                            string str2 = Reader.ReadLine();
                            string str3 = Reader.ReadLine();
                            //Console.WriteLine("Закончил читать введенную информацию");


                            if (vsinx == "1")
                            {
                                   lock (LockObj)
                                   {
                                          Calculation(command_name, command_par, str1, str2, str3);
                                   }
                            }
                            else
                            {
                                   if (vsinx == "2") mut.WaitOne();
                                   if (vsinx == "3") Monitor.Enter(locker);
                                   Calculation(command_name, command_par, str1, str2, str3);
                                   if (vsinx == "2") mut.ReleaseMutex();
                                   if (vsinx == "3") Monitor.Exit(locker);
                            }
                            Thread.Sleep(100);
                     }
                     Reader.Close();
              }
              if (var_obm == "3")
              {
                     while (!stop)
                     {
                            if (vsinx == "1")
                            {
                                   lock (LockObj)
                                   {
                                          StreamReader Reader2 = new StreamReader("obmen.txt");

                                          string command_name2 = Reader2.ReadLine();
                                          string command_par2 = Reader2.ReadLine();
                                          string str1_2 = Reader2.ReadLine();
                                          string str2_2 = Reader2.ReadLine();
                                          string str3_2 = Reader2.ReadLine();

                                          Calculation(command_name2, command_par2, str1_2, str2_2, str3_2);
                                          Reader2.Close();
                                   }
                            }
                            else
                            {
                                   if (vsinx == "2") mut.WaitOne();
                                   if (vsinx == "3") Monitor.Enter(locker);

                                   StreamReader Reader = new StreamReader("obmen.txt");

                                   string command_name = Reader.ReadLine();
                                   string command_par = Reader.ReadLine();
                                   string str1 = Reader.ReadLine();
                                   string str2 = Reader.ReadLine();
                                   string str3 = Reader.ReadLine();

                                   Calculation(command_name, command_par, str1, str2, str3);
                                   Reader.Close();

                                   if (vsinx == "2") mut.ReleaseMutex();
                                   if (vsinx == "3") Monitor.Exit(locker);
                            }
                            Thread.Sleep(100);
                     }
              }
       }
       

       

       public static void First()
       {
              StreamWriter writer;
              string[] arr;


              Thread th2 = new Thread(() => Second());
              if (var_obm == "2" || var_obm == "3") if (!th2.IsAlive) th2.Start();


              if (var_obm == "1")
              {
                     AnonPipeServer = new AnonymousPipeServerStream(PipeDirection.Out);
                     pipeHandle = AnonPipeServer.GetClientHandleAsString();

                     if (!th2.IsAlive) th2.Start();

                     writer = new StreamWriter(AnonPipeServer);
                     writer.AutoFlush = true;

                     while (!stop)
                     {
                            if (vsinx == "1")
                            {
                                   lock (LockObj)
                                   {
                                          arr = AskInformation();
                                   }
                            }
                            else
                            {
                                   if (vsinx == "2") mut.WaitOne();
                                   if (vsinx == "3") Monitor.Enter(locker);
                                   arr = AskInformation();
                                   if (vsinx == "2") mut.ReleaseMutex();
                                   if (vsinx == "3") Monitor.Exit(locker);
                            }

                            //Console.WriteLine("Начал писать введенную информацию");
                            writer.WriteLine(arr[0]);          //cname
                            writer.WriteLine(arr[1]);          //cpar
                            writer.WriteLine(arr[2]);          //s1
                            writer.WriteLine(arr[3]);          //s2
                            writer.WriteLine(arr[4]);          //s3
                            //Console.WriteLine("Закончил писать введенную информацию");

                            Thread.Sleep(100);
                     }
                     writer.Close();
              }
              if (var_obm == "2")
              {
                     NamedPipeServer = new NamedPipeServerStream("testpipe", PipeDirection.Out);        
                     NamedPipeServer.WaitForConnection();
                     writer = new StreamWriter(NamedPipeServer);
                     writer.AutoFlush = true;


                     while (!stop)
                     {
                            if (vsinx == "1")
                            {
                                   lock (LockObj)
                                   {
                                          arr = AskInformation();
                                   }
                            }
                            else
                            {
                                   if (vsinx == "2") mut.WaitOne();
                                   if (vsinx == "3") Monitor.Enter(locker);
                                   arr = AskInformation();
                                   if (vsinx == "2") mut.ReleaseMutex();
                                   if (vsinx == "3") Monitor.Exit(locker);
                            }

                            //Console.WriteLine("Начал писать введенную информацию");
                            writer.WriteLine(arr[0]);          //cname
                            writer.WriteLine(arr[1]);          //cpar
                            writer.WriteLine(arr[2]);          //s1
                            writer.WriteLine(arr[3]);          //s2
                            writer.WriteLine(arr[4]);          //s3
                            //Console.WriteLine("Закончил писать введенную информацию");
                            
                            Thread.Sleep(100);
                     }
                     writer.Close();
              }
              if (var_obm == "3")
              {
                     while (!stop)
                     {
                            if (vsinx == "1")
                            {
                                   lock (LockObj)
                                   {
                                          writer = new StreamWriter("obmen.txt", false);
                                          arr = AskInformation();
                                          writer.WriteLine(arr[0]);          //cname
                                          writer.WriteLine(arr[1]);          //cpar
                                          writer.WriteLine(arr[2]);          //s1
                                          writer.WriteLine(arr[3]);          //s2
                                          writer.WriteLine(arr[4]);          //s3
                                          writer.Close();
                                   }
                            }
                            else
                            {
                                   if (vsinx == "2") mut.WaitOne();
                                   if (vsinx == "3") Monitor.Enter(locker);

                                   writer = new StreamWriter("obmen.txt", false);
                                   arr = AskInformation();
                                   writer.WriteLine(arr[0]);          //cname
                                   writer.WriteLine(arr[1]);          //cpar
                                   writer.WriteLine(arr[2]);          //s1
                                   writer.WriteLine(arr[3]);          //s2
                                   writer.WriteLine(arr[4]);          //s3
                                   writer.Close();

                                   if (vsinx == "2") mut.ReleaseMutex();
                                   if (vsinx == "3") Monitor.Exit(locker);
                            }
                            Thread.Sleep(100);
                     }
              }
              th2.Join();
       }



       public static void Main()
       {
              string[] arr_var3 = { "1", "2", "3" };
              string[] arr_var4 = { "1", "2", "3" };
              string[] arr;

              string s1 = "";
              string suggest_str = "";
              string err_str = "";


              if (var_obm == "")
              {
                     suggest_str = "Введите вариант обмена данными между потоками (1 - неименованный канал, 2 - именованный канал, 3 - файл) или q для выхода.";
                     err_str = "Введен некорректный вариант.";
                     arr = arr_var3;
                     Console.WriteLine(suggest_str);
                     do
                     {
                            s1 = Console.ReadLine();
                            if (s1 == "q") Environment.Exit(0);
                            if (!arr.Contains(s1)) { Console.WriteLine(err_str + "\r\n" + suggest_str); } else { break; }
                     }
                     while (5 == 5);
                     var_obm = s1;


                     suggest_str = "Введите вариант синхронизации использования ресурсов (1 - lock, 2- mutex, 3 - monitor) или q для выхода.";
                     err_str = "Введен некорректный параметр.";
                     arr = arr_var4;
                     Console.WriteLine(suggest_str);
                     do
                     {
                            s1 = Console.ReadLine();
                            if (s1 == "q") Environment.Exit(0);
                            if (!arr.Contains(s1)) { Console.WriteLine(err_str + "\r\n" + suggest_str); } else { break; }
                     }
                     while (5 == 5);
                     vsinx = s1;
              }


              Thread th1 = new Thread(() => First());              
              if (!th1.IsAlive) th1.Start();
              th1.Join();
       }
    }
}
