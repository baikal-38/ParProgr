using System;
using System.Linq;
using MPI;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class Program
    {
            static int kol_vo_elementov;
            static int kol_vo_potokov;
            static int b = 554234;
            static int[,] Nabor = new int[2,100];
            

            public static double calculate1(int[] C, int task)
            {
                    int i, j;
                    double var1 = 0;
                    
                    
                    for (i = 0; i < C.Length; i++)
                    {
                            if (task == 2)
                            {
                                for (j = 0; j < 100; j++)
                                {
                                        if (C[i] == Nabor[0, j])
                                        {
                                            C[i] = Nabor[1, j];
                                            var1++;
                                        }
                                }
                            }
                            if (task == 3)
                            {
                                if (C[i] == b) var1++;
                            }
                            if (task == 4)
                            {
                                var1 = var1 + Math.Sqrt(C[i]);
                            }
                            if (task == 5)
                            {
                                if (i == 0) var1 = C[i];
                                if (C[i] > var1) var1 = C[i];
                            }
                            if (task == 6)
                            {
                                if (i == 0) var1 = C[i];
                                if (C[i] < var1) var1 = C[i];
                            }
                            if (task == 7)
                            {
                                if (ProverkaProstoeChislo(C[i])) var1++;
                            }
                            if (task == 8)
                            {
                                if (ProverkaKvadratChislo(C[i])) var1++;
                            }
                            if (task == 9)
                            {
                                if ((i % 2) == 0)
                                {
                                    var1 = var1 + C[i];
                                }
                                else
                                {
                                    var1 = var1 - C[i];
                                }
                            }
                            if (task == 10)
                            {
                                if ((C[i] % 2) == 0) var1 = var1 + C[i];
                            }
                    }

                    return var1;
            }
            
            public static double calculate2(int[] A, int[] C, int task)
            {
                    int i;
                    double var1 = 0;
                    
                    for (i = 0; i < A.Length; i++)
                    {
                            if (task == 1)   if (A[i] == C[i]) var1++;
                    }

                    return var1;
            }



            static bool ProverkaProstoeChislo(int number)
            {
                    if (number < 2) return false;
                    for (int i = 2; i < number; i++)
                    {
                        if (number % i == 0) return false;
                    }
                    return true;
            }
            static bool ProverkaKvadratChislo(int number)
            {
                    if (number < 0) return false;
                    int n1 = (int)Math.Sqrt(number);
                    if (n1 * n1 == number)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }


        static void Main(string[] args)
            {
                    bool flag;

                    if (args.Count() == 0)
                    {
                            Console.Write("Введите количество элементов массива (от 100000 до 1000000): ");
                            do
                            {
                                    string numberStr = Console.ReadLine();

                                    flag = Int32.TryParse(numberStr, out kol_vo_elementov);
                                    if (!flag)
                                    {
                                        Console.WriteLine("Количество элементов массива указано некорректно. Попробуйте еще раз.");
                                    }
                                    else
                                    {
                                        if (kol_vo_elementov > 100000 & kol_vo_elementov < 1000000) { flag = true; } else { flag = false; }
                                        if (!flag) Console.WriteLine("Количество элементов массива быть > 100 и < 100000. Попробуйте еще раз.");
                                    }
                            } while (!flag);
                            Console.WriteLine("Количество элементов массива = " + kol_vo_elementov);
                            
                            
                            Console.Write("Введите количество потоков (не более 40): ", kol_vo_elementov);
                            do
                            {
                                    string numberStr = Console.ReadLine();

                                    flag = Int32.TryParse(numberStr, out kol_vo_potokov);
                                    if (!flag)
                                    {
                                            Console.WriteLine("Количество потоков указано некорректно. Попробуйте еще раз.");
                                    }
                                    else
                                    {
                                            if (kol_vo_potokov <= 40) { flag = true; } else { flag = false; }
                                            if (!flag) Console.WriteLine("Количество потоков должно быть меньше 40. Попробуйте еще раз.");
                                    }
                            }  while (!flag);
                            Console.WriteLine("Количество потоков = " + kol_vo_potokov);
                            
                            
                            Process.Start("CMD.exe", "/C cd "+ AppDomain.CurrentDomain.BaseDirectory + " && mpiexec -n " + kol_vo_potokov + " ConsoleApplication1.exe " + kol_vo_elementov);
                            Console.ReadLine();
                    }
                    else
                    {
                            Stopwatch sWatch = new Stopwatch();
                            sWatch.Start();

                            Random rnd1 = new Random();
                            
                            kol_vo_elementov = Convert.ToInt32(args[0]);
                            int N = kol_vo_elementov;

                            int[] A = new int[N];
                            int[] C = new int[N];
                            int[,] Nabor = new int[2,kol_vo_elementov];
                            
                            for (int i = 0; i < N; i++)
                            {
                                    A[i] = rnd1.Next(100, 10000000);
                                    C[i] = rnd1.Next(100, 10000000);
                            }
                            for (int j = 0; j < 100; j++)
                            {
                                    Nabor[0, j] = rnd1.Next(100, 10000000);
                                    Nabor[1, j] = rnd1.Next(100, 10000000);
                            }
                            
                            
                            using (new MPI.Environment(ref args))
                            {
                                Intracommunicator comm = Communicator.world;
                                int rank = comm.Rank;                           //номер текущего потока
                                int size = comm.Size;                           //общее количество потоков
                                
                                int h = kol_vo_elementov / size;
                                int start;
                                int end;

                                //comm.Barrier();
                                if (rank == 0)
                                {
                                        for (int i = 1; i < size; i++)                          //посылаем кол-во элементов в подмассиве i-му потоку c меткой 7
                                        {
                                                start = h * i;
                                                end = start + h - 1;
                                                if (i == size - 1) end = kol_vo_elementov - 1;

                                                comm.Send(end - start + 1, i, 1);
                                        }
                                        for (int i = 1; i < size; i++)                          //посылаем часть массива i-му потоку c меткой 3
                                        {
                                                start = h * i;
                                                end = start + h - 1;
                                                if (i == size - 1) end = kol_vo_elementov - 1;

                                                //Console.WriteLine("start=" + start + ", end=" + end);
                                                int[] array_part1 = new int[end - start + 1];
                                                int[] array_part2 = new int[end - start + 1];

                                                Array.Copy(A, start, array_part1, 0, end - start + 1);
                                                Array.Copy(C, start, array_part2, 0, end - start + 1);
                                                
                                                comm.Send(array_part1, i, 2);
                                                comm.Send(array_part2, i, 3);
                                        }
                                        
                                        int[] arr_part1 = new int[h];
                                        int[] arr_part2 = new int[h];
                                        Array.Copy(A, 0, arr_part1, 0, h);
                                        Array.Copy(C, 0, arr_part2, 0, h);
                                        

                                        
                                        double psum;
                                        
                                        //========================================================================================================================================= 
                                        Console.WriteLine("Задание № 1. Проверка совпадают ли поэлементно массивы А и С.");
                                        sWatch.Reset();
                                        sWatch.Start();
                                        double rez1 = calculate2(A, C, 1);                                        
                                        sWatch.Stop();
                                        Console.WriteLine("В одном потоке. В массивах А и С найдено совпадающих элементов: " + rez1 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");
                                        
                                        sWatch.Reset();
                                        sWatch.Start();
                                        rez1 =calculate2(arr_part1, arr_part2, 1);
                                        for (int i = 1; i < size; i++)
                                        {
                                            psum = 0;
                                            comm.Receive(i, 9, out psum);
                                            rez1 += psum;
                                        }
                                        sWatch.Stop();
                                        Console.WriteLine("N потоков. В массивах А и С найдено совпадающих элементов: " + rez1 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n\r\n");
                                        
                                        
                                        //========================================================================================================================================= 
                                        Console.WriteLine("Задание № 2. Кодирование элементов массива С.");
                                        sWatch.Reset();
                                        sWatch.Start();
                                        double rez2 =calculate1(C, 2);
                                        sWatch.Stop();
                                        Console.WriteLine("В одном потоке. Закодировано " + rez2 + " элементов. Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");
                                        
                                        sWatch.Reset();
                                        sWatch.Start();
                                        rez2 =calculate1(arr_part2, 2);
                                        for (int i = 1; i < size; i++)
                                        {
                                            psum = 0;
                                            comm.Receive(i, 9, out psum);
                                            rez2 += psum;
                                        }
                                        sWatch.Stop();
                                        Console.WriteLine("N потоков. Закодировано " + rez2 + " элементов. Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n\r\n");
                                        
                                        
                                        //========================================================================================================================================= 
                                        Console.WriteLine("Задание № 3. Определение количества вхождений числа b в массив С.");
                                        sWatch.Reset();
                                        sWatch.Start();
                                        double rez3 = calculate1(C, 3);
                                        sWatch.Stop();
                                        Console.WriteLine("В одном потоке. В массиве С число " + b + " встретилось " + rez3 + " раз. Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");
                                        
                                        sWatch.Reset();
                                        sWatch.Start();
                                        rez3 = calculate1(arr_part2, 3);
                                        for (int i = 1; i < size; i++)
                                        {
                                            psum = 0;
                                            comm.Receive(i, 9, out psum);
                                            rez3 += psum;
                                        }
                                        sWatch.Stop();
                                        Console.WriteLine("N потоков. В массиве С число " + b + " встретилось " + rez3 + " раз. Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n\r\n");
                                        
                                        
                                        //=========================================================================================================================================
                                        Console.WriteLine("Задание № 4. Поиск суммы корней квадратных из Ai.");
                                        sWatch.Reset();
                                        sWatch.Start();
                                        double rez4 = calculate1(C, 4);
                                        sWatch.Stop();
                                        Console.WriteLine("В одном потоке. Сумма корней квадратных из Ai = " + rez4 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");
                                        
                                        sWatch.Reset();
                                        sWatch.Start();
                                        rez4 = calculate1(arr_part2, 4);
                                        for (int i = 1; i < size; i++)
                                        {
                                            psum = 0;
                                            comm.Receive(i, 9, out psum);
                                            rez4 += psum;
                                        }
                                        sWatch.Stop();
                                        Console.WriteLine("N потоков. Сумма корней квадратных из Ai = " + rez4 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n\r\n");
                                        
                                        
                                        //=========================================================================================================================================
                                        Console.WriteLine("Задание № 5. Поиск максимального элемента массива.");
                                        sWatch.Reset();
                                        sWatch.Start();
                                        double rez5 = calculate1(C, 5);
                                        sWatch.Stop();
                                        Console.WriteLine("В одном потоке. Максимальный элемент в массиве = " + rez5 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");
                                        
                                        sWatch.Reset();
                                        sWatch.Start();
                                        rez5 = calculate1(arr_part2, 5);
                                        for (int i = 1; i < size; i++)
                                        {
                                            psum = 0;
                                            comm.Receive(i, 9, out psum);
                                            if (psum > rez5) rez5 = psum;
                                        }
                                        sWatch.Stop();
                                        Console.WriteLine("N потоков. Максимальный элемент в массиве = " + rez5 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n\r\n");
                        

                                        //=========================================================================================================================================
                                        Console.WriteLine("Задание № 6. Поиск минимального элемента массива.");
                                        sWatch.Reset();
                                        sWatch.Start();
                                        double rez6 = calculate1(C, 6);
                                        sWatch.Stop();
                                        Console.WriteLine("В одном потоке. Минимальный элемент в массиве = " + rez6 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");
                                        
                                        sWatch.Reset();
                                        sWatch.Start();
                                        rez6 = calculate1(arr_part2, 6);
                                        for (int i = 1; i < size; i++)
                                        {
                                            psum = 0;
                                            comm.Receive(i, 9, out psum);
                                            if (psum < rez6) rez6 = psum;
                                        }
                                        sWatch.Stop();
                                        Console.WriteLine("N потоков. Минимальный элемент в массиве = " + rez6 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n\r\n");
                        

                                        //=========================================================================================================================================
                                        Console.WriteLine("Задание № 7. Поиск простых чисел.");
                                        sWatch.Reset();
                                        sWatch.Start();
                                        double rez7 = calculate1(C, 7);
                                        sWatch.Stop();
                                        Console.WriteLine("В одном потоке. Найдено: " + rez7 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");
                                        
                                        sWatch.Reset();
                                        sWatch.Start();
                                        rez7 = calculate1(arr_part2, 7);
                                        for (int i = 1; i < size; i++)
                                        {
                                            psum = 0;
                                            comm.Receive(i, 9, out psum);
                                            rez7 += psum;
                                        }
                                        sWatch.Stop();
                                        Console.WriteLine("N потоков. Найдено: " + rez7 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n\r\n");
                        

                                        //=========================================================================================================================================
                                        Console.WriteLine("Задание № 8. Поиск элементов массива, являющихся квадратами любого натурального числа.");
                                        sWatch.Reset();
                                        sWatch.Start();
                                        double rez8 = calculate1(C, 8);
                                        sWatch.Stop();
                                        Console.WriteLine("В одном потоке. Найдено: " + rez8 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");
                                        
                                        sWatch.Reset();
                                        sWatch.Start();
                                        rez8 = calculate1(arr_part2, 8);
                                        for (int i = 1; i < size; i++)
                                        {
                                            psum = 0;
                                            comm.Receive(i, 9, out psum);
                                            rez8 += psum;
                                        }
                                        sWatch.Stop();
                                        Console.WriteLine("N потоков. Найдено: " + rez8 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n\r\n");
                        

                                        //=========================================================================================================================================
                                        Console.WriteLine("Задание № 9. Поиск суммы выражения а0-а1 + а2-а3 + а4-а5 + ...");
                                        sWatch.Reset();
                                        sWatch.Start();
                                        double rez9 = calculate1(C, 9);
                                        sWatch.Stop();
                                        Console.WriteLine("В одном потоке. Сумма а0-а1 + а2-а3 + а4-а5 + ... = " + rez9 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");

                                        sWatch.Reset();
                                        sWatch.Start();
                                        rez9 = calculate1(arr_part2, 9);
                                        for (int i = 1; i < size; i++)
                                        {
                                            psum = 0;
                                            comm.Receive(i, 9, out psum);
                                            rez9 += psum;
                                        }
                                        sWatch.Stop();
                                        Console.WriteLine("N потоков. Сумма а0-а1 + а2-а3 + а4-а5 + ... = " + rez9 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n\r\n");
                        

                                        //=========================================================================================================================================
                                        Console.WriteLine("Задание № 10. Поиск суммы всех четных чисел массива");
                                        sWatch.Reset();
                                        sWatch.Start();
                                        double rez10 = calculate1(C, 10);
                                        sWatch.Stop();
                                        Console.WriteLine("В одном потоке. Сумма всех четных чисел массива = " + rez10 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");
                                        
                                        sWatch.Reset();
                                        sWatch.Start();
                                        rez10 = calculate1(arr_part2, 10);
                                        for (int i = 1; i < size; i++)
                                        {
                                            psum = 0;
                                            comm.Receive(i, 9, out psum);
                                            rez10 += psum;
                                        }
                                        sWatch.Stop();
                                        Console.WriteLine("N потоков. Сумма всех четных чисел массива = " + rez10 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n\r\n");
                                }
                                else
                                {
                                        int N1 = 0;
                                        comm.Receive(0, 1, out N1);             //от нулевого потока получаем кол-во элементов в подмассиве
                                        int[] array1 = new int[N1];
                                        comm.Receive(0, 2, ref array1);         //от нулевого потока получаем подмассив
                                        int[] array2 = new int[N1];
                                        comm.Receive(0, 3, ref array2);         //от нулевого потока получаем подмассив
                                        
                                        double psum;
                                        
                                        psum = calculate2(array1, array2, 1);
                                        comm.Send(psum, 0, 9);

                                        psum = calculate1(array2, 2);
                                        comm.Send(psum, 0, 9);
                                        
                                        psum = calculate1(array2, 3);
                                        comm.Send(psum, 0, 9);

                                        psum = calculate1(array2, 4);
                                        comm.Send(psum, 0, 9);
                                        
                                        psum = calculate1(array2, 5);
                                        comm.Send(psum, 0, 9);

                                        psum = calculate1(array2, 6);
                                        comm.Send(psum, 0, 9);
                                        
                                        psum = calculate1(array2, 7);
                                        comm.Send(psum, 0, 9);

                                        psum = calculate1(array2, 8);
                                        comm.Send(psum, 0, 9);

                                        psum = calculate1(array2, 9);
                                        comm.Send(psum, 0, 9);

                                        psum = calculate1(array2, 10);
                                        comm.Send(psum, 0, 9);
                                }
                            }
                            Console.ReadLine();
                    }
        }
    }
}
