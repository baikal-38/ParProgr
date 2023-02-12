using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;


namespace MultiThread
{

    class Program
    {
        const int kol_vo_elementov = 500000;
        static int kol_vo_potokov;
        static int b = 554234;
        

        static int[] A = new int[kol_vo_elementov];
        static int[] C = new int[kol_vo_elementov];
        static int[,] N = new int[2,kol_vo_elementov];
        
        
        static bool ProverkaProstoeChislo(int number)
        {
                if (number < 2)   return false;
                for (int i = 2; i < number; i++)
                {
                    if (number % i == 0)  return false;
                }
                return true;
        }
        static bool ProverkaKvadratChislo(int number)
        {
                if (number < 0) return false;
                int n1 = (int) Math.Sqrt(number);
                if (n1 * n1 == number)
                {
                    return true;
                }
                else
	            {
                    return false;
	            }            
        }


        static double ThreadTask999(int nomer_potoka, int task)
        {
            int i, j, h, begin, end;
            double var1 = 0;
            if (task == 4) var1 = 1;


            h = kol_vo_elementov / kol_vo_potokov;
            begin = h * nomer_potoka;
            end = begin + h - 1;
            if (nomer_potoka == kol_vo_potokov - 1) end = kol_vo_elementov - 1;
            //Console.WriteLine("Поток {0} начало {1} конец {2}", nomer_potoka, begin, end);


            for (i = begin; i <= end; i++)
            {
                    if (task == 1)
                    {
                        if (A[i] == C[i]) var1++;
                    }
                    if (task == 2)
                    {
                        for (j = 0; j < 100; j++)
                        {
                            if (C[i] == N[0, j]) C[i] = N[1, j];
                        }
                    }
                    if (task == 3)
                    {
                        if (C[i] == b) var1++;
                    }
                    if (task == 4)
                    {
                        var1 = var1 * C[i];
                    }
                    if (task == 5)
                    {
                        if (i == begin) var1 = C[i];
                        if (C[i] > var1) var1 = C[i];
                    }
                    if (task == 6)
                    {
                        if (i == begin) var1 = C[i];
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
            /*
            if (task == 1) Console.WriteLine("В потоке {0} найдено совпадающих элементов: {1}", nomer_potoka, var1);
            if (task == 2) Console.WriteLine("В потоке {0} все элементы закодированы.", nomer_potoka);
            if (task == 3) Console.WriteLine("В потоке {0} найдено совпадающих элементов: {1}", nomer_potoka, var1);
            if (task == 4) Console.WriteLine("Произведение элементов потока {0} равно {1}", nomer_potoka, var1);
            if (task == 5) Console.WriteLine("В потоке {0} максимальный элемент = {1}", nomer_potoka, var1);
            if (task == 6) Console.WriteLine("В потоке {0} минимальный элемент = {1}", nomer_potoka, var1);
            if (task == 7) Console.WriteLine("В потоке {0} количество простых чисел = {1}", nomer_potoka, var1);
            if (task == 8) Console.WriteLine("В потоке {0} найдено элементов массива, являющихся квадратами любого натурального числа: = {1}", nomer_potoka, var1);
            if (task == 9) Console.WriteLine("В потоке {0} найдено элементов массива, являющихся квадратами любого натурального числа: = {1}", nomer_potoka, var1);
            if (task == 10) Console.WriteLine("В потоке {0} сумма четных чисел = {1}", nomer_potoka, var1);
            */


            return var1;
        }








        static void Main()
        {
            Stopwatch sWatch = new Stopwatch();


            bool flag = false;

            Console.Write("Введите количество потоков (не более {0}): ", kol_vo_elementov);
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
                        if (kol_vo_elementov > kol_vo_potokov) { flag = true; } else { flag = false; }
                        if (!flag) Console.WriteLine("Количество потоков должно быть меньше количества элементов массива. Попробуйте еще раз.");
                    }
            } while (!flag);
            Console.WriteLine("Количество потоков = " + kol_vo_potokov);


            
            Random rnd1 = new Random();
            int i;

            double rez = 0;
            double[] returns = new double[kol_vo_potokov];
            Thread[] thread = new Thread[kol_vo_potokov];


            

            //=========================================================================================================================================
            Console.WriteLine("\r\n\r\nЗадание № 1. Проверка совпадают ли поэлементно массивы А и С.");
            sWatch.Start();
            int cnt1 = 0;
            for (i = 0; i < kol_vo_elementov; i++)
            {
                A[i] = rnd1.Next(100, 10000000);
                C[i] = rnd1.Next(100, 10000000);

                if (A[i] == C[i]) cnt1++;
            }
            sWatch.Stop();
            Console.WriteLine("Без потоков. В массивах А и С найдено совпадающих элементов: " + cnt1 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");
            

            sWatch.Reset();
            sWatch.Start();
            for (i = 0; i < kol_vo_potokov; i++)
            {
                int tmp = i;
                returns[tmp] = 0;
                thread[i] = new Thread(() => { returns[tmp] = ThreadTask999(tmp, 1); });
                thread[i].Start();
            }
            for (i = 0; i < kol_vo_potokov; i++)
            {
                thread[i].Join();
                rez += returns[i];
            }
            sWatch.Stop();
            Console.WriteLine("С потоками. В массивах А и С найдено совпадающих элементов: " + rez + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n");



            //=========================================================================================================================================            
            Console.WriteLine("Задание № 2. Кодирование элементов массива С");
            sWatch.Reset();
            sWatch.Start();
            int j;
            for (i = 0; i < kol_vo_elementov; i++)
            {
                C[i] = rnd1.Next(100, 10000000);
            }
            for (j = 0; j < 100; j++)
            {
                N[0, j] = j;
                N[1, j] = 2*j;
            }
            for (i = 0; i < kol_vo_elementov; i++)
            {
                for (j = 0; j < 100; j++)
                {
                    if (C[i] == N[0, j]) C[i] = N[1, j];
                }
            }
            sWatch.Stop();
            Console.WriteLine("Без потоков. Кодирование произведено. Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");
            

            sWatch.Reset();
            sWatch.Start();
            for (i = 0; i < kol_vo_potokov; i++)
            {
                int tmp = i;
                thread[i] = new Thread(() => { ThreadTask999(tmp, 2); });
                thread[i].Start();
            }
            for (i = 0; i < kol_vo_potokov; i++)
            {
                thread[i].Join();
            }
            sWatch.Stop();
            Console.WriteLine("С потоками. Кодирование произведено. Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n");

            

            //=========================================================================================================================================
            Console.WriteLine("Задание № 3. Определение количества вхождений числа b в массив С.");
            sWatch.Reset();
            sWatch.Start();
            int cnt3 = 0;
            for (i = 0; i < kol_vo_elementov; i++)
            {
                C[i] = rnd1.Next(100, 10000000);
            }
            for (i = 0; i < kol_vo_elementov; i++)
            {
                if (C[i] == b) cnt3++;
            } 
            sWatch.Stop();
            Console.WriteLine("Без потоков. В массиве С число " + b + " встретилось " + cnt3 + " раз. Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");


            rez = 0;
            sWatch.Reset();
            sWatch.Start();
            for (i = 0; i < kol_vo_potokov; i++)
            {
                int tmp = i;
                returns[tmp] = 0;
                thread[i] = new Thread(() => { returns[tmp] = ThreadTask999(tmp, 3); });
                thread[i].Start();
            }
            for (i = 0; i < kol_vo_potokov; i++)
            {
                thread[i].Join();
                rez += returns[i];
            }
            sWatch.Stop();
            Console.WriteLine("С потоками. В массиве С число " + b + " встретилось " + rez + " раз. Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n");
            
           
            
            //=========================================================================================================================================
            Console.WriteLine("Задание № 4. Поиск произведения всех элементов массива.");
            sWatch.Reset();
            sWatch.Start();
            double itog = 1;
            for (i = 0; i < kol_vo_elementov; i++)
            {
                C[i] = rnd1.Next(1, 3);
                itog = itog * C[i];
            }
            sWatch.Stop();
            Console.WriteLine("Без потоков. Произведение = " + itog + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");


            double proizv = 1;
            sWatch.Reset();
            sWatch.Start();
            for (i = 0; i < kol_vo_potokov; i++)
            {
                int tmp = i;
                returns[tmp] = 0;
                thread[i] = new Thread(() => { returns[tmp] = ThreadTask999(tmp, 4); });
                thread[i].Start();
            }
            for (i = 0; i < kol_vo_potokov; i++)
            {
                thread[i].Join();
                proizv = proizv * returns[i];
            }
            sWatch.Stop();
            Console.WriteLine("С потоками. Произведение = " + proizv + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n");

            
            
            
            //=========================================================================================================================================
            Console.WriteLine("Задание № 5. Поиск максимального элемента массива.");
            sWatch.Reset();
            sWatch.Start();
            rez = 0;
            int max = 0;
            for (i = 0; i < kol_vo_elementov; i++)
            {
                C[i] = rnd1.Next(100, 10000000);
                if (i == 0) max = C[i];

                if (C[i] > max) max = C[i];
            }
            sWatch.Stop();
            Console.WriteLine("Без потоков. Максимальный элемент в массиве = " + max + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");


            sWatch.Reset();
            sWatch.Start();
            for (i = 0; i < kol_vo_potokov; i++)
            {
                int tmp = i;
                returns[tmp] = 0;
                thread[i] = new Thread(() => { returns[tmp] = ThreadTask999(tmp, 5); });
                thread[i].Start();
            }
            for (i = 0; i < kol_vo_potokov; i++)
            {
                thread[i].Join();
                if (returns[i] > rez) rez = returns[i];
            }
            sWatch.Stop();
            Console.WriteLine("С потоками. Максимальный элемент в массиве = " + rez + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n");



            //=========================================================================================================================================
            Console.WriteLine("Задание № 6. Поиск минимального элемента массива.");
            sWatch.Reset();
            sWatch.Start();
            int min = 0;            
            for (i = 0; i < kol_vo_elementov; i++)
            {
                C[i] = rnd1.Next(100, 10000000);
                if (i == 0) min = C[i];

                if (C[i] < min) min = C[i];
            }
            sWatch.Stop();
            Console.WriteLine("Без потоков. Минимальный элемент в массиве = " + min + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");


            sWatch.Reset();
            sWatch.Start();
            for (i = 0; i < kol_vo_potokov; i++)
            {
                int tmp = i;
                returns[tmp] = 0;
                thread[i] = new Thread(() => { returns[tmp] = ThreadTask999(tmp, 6); });
                thread[i].Start();
            }
            for (i = 0; i < kol_vo_potokov; i++)
            {
                thread[i].Join();
                if (i == 0) rez = returns[i];
                if (returns[i] < rez) rez = returns[i];
            }
            sWatch.Stop();
            Console.WriteLine("С потоками. Минимальный элемент в массиве = " + rez + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n");



            //=========================================================================================================================================
            Console.WriteLine("Задание № 7. Поиск простых чисел.");
            sWatch.Reset();
            sWatch.Start();
            int cnt2 = 0;
            for (i = 0; i < kol_vo_elementov; i++)
            {
                C[i] = rnd1.Next(100, 10000000);

                if (ProverkaProstoeChislo(C[i]))
                {
                    cnt2++;
                }
            }
            sWatch.Stop();
            Console.WriteLine("Без потоков. Найдено простых чисел в массиве: " + cnt2 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");


            sWatch.Reset();
            sWatch.Start();
            for (i = 0; i < kol_vo_potokov; i++)
            {
                int tmp = i;
                returns[tmp] = 0;
                thread[i] = new Thread(() => { returns[tmp] = ThreadTask999(tmp, 7); });
                thread[i].Start();
            }
            rez = 0;
            for (i = 0; i < kol_vo_potokov; i++)
            {
                thread[i].Join();
                rez += returns[i];
            }
            sWatch.Stop();
            Console.WriteLine("С потоками. Найдено простых чисел в массиве: " + rez + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n");



            //=========================================================================================================================================
            Console.WriteLine("Задание № 8. Поиск элементов массива, являющихся квадратами любого натурального числа.");
            sWatch.Reset();
            sWatch.Start();
            int cnt8 = 0;
            for (i = 0; i < kol_vo_elementov; i++)
            {
                C[i] = rnd1.Next(100, 10000000);
                //C[i] = i;

                if (ProverkaKvadratChislo(C[i]))
                {
                    //Console.WriteLine(C[i]);
                    cnt8++;
                }
            }
            sWatch.Stop();
            Console.WriteLine("Без потоков. Найдено: " + cnt8 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");


            sWatch.Reset();
            sWatch.Start();
            for (i = 0; i < kol_vo_potokov; i++)
            {
                int tmp = i;
                returns[tmp] = 0;
                thread[i] = new Thread(() => { returns[tmp] = ThreadTask999(tmp, 8); });
                thread[i].Start();
            }
            rez = 0;
            for (i = 0; i < kol_vo_potokov; i++)
            {
                thread[i].Join();
                rez += returns[i];
            }
            sWatch.Stop();
            Console.WriteLine("С потоками. Найдено: " + rez + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n");



            //=========================================================================================================================================
            Console.WriteLine("Задание № 9. Поиск суммы выражения а0-а1 + а2-а3 + а4-а5 + ...");
            sWatch.Reset();
            sWatch.Start();
            double sum1 = 0;
            for (i = 0; i < kol_vo_elementov; i++)
            {
                C[i] = rnd1.Next(100, 10000000);
                //C[i] = i;

                if ((i % 2) == 0)
                {
                    sum1 = sum1 + C[i];
                }
                else
                {
                    sum1 = sum1 - C[i];
                }
            }
            sWatch.Stop();
            Console.WriteLine("Без потоков. Сумма а0-а1 + а2-а3 + а4-а5 + ... = " + sum1 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");


            sWatch.Reset();
            sWatch.Start();
            for (i = 0; i < kol_vo_potokov; i++)
            {
                int tmp = i;
                returns[tmp] = 0;
                thread[i] = new Thread(() => { returns[tmp] = ThreadTask999(tmp, 9); });
                thread[i].Start();
            }
            rez = 0;
            for (i = 0; i < kol_vo_potokov; i++)
            {
                thread[i].Join();
                rez += returns[i];
            }
            sWatch.Stop();
            Console.WriteLine("С потоками. Сумма а0-а1 + а2-а3 + а4-а5 + ... = " + rez + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n");



            //=========================================================================================================================================
            Console.WriteLine("Задание № 10. Поиск суммы всех четных чисел массива");
            sWatch.Reset();
            sWatch.Start();
            double sum2 = 0;
            for (i = 0; i < kol_vo_elementov; i++)
            {
                C[i] = rnd1.Next(100, 10000000);
                //C[i] = i;

                if ((C[i] % 2) == 0)
                {
                    sum2 = sum2 + C[i];
                }
            }
            sWatch.Stop();
            Console.WriteLine("Без потоков. Сумма всех четных чисел массива = " + sum2 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.");


            sWatch.Reset();
            sWatch.Start();
            for (i = 0; i < kol_vo_potokov; i++)
            {
                int tmp = i;
                returns[tmp] = 0;
                thread[i] = new Thread(() => { returns[tmp] = ThreadTask999(tmp, 10); });
                thread[i].Start();
            }
            sum1 = 0;
            for (i = 0; i < kol_vo_potokov; i++)
            {
                thread[i].Join();
                sum1 += returns[i];
            }
            sWatch.Stop();
            Console.WriteLine("С потоками. Сумма всех четных чисел массива = " + sum1 + ". Время: " + sWatch.ElapsedMilliseconds.ToString() + " мс.\r\n");
            


            Console.ReadLine();
        }
    }
}
