//#include "stdafx.h"
#include <iostream>
#include <time.h>
#include <cmath>
#include <omp.h>
#include <math.h>
#include <string>
#include <random>
#include <ctime>

using namespace std;



bool ProverkaProstoeChislo(long long number)
{
        if (number < 2)   return false;
        for (long i = 2; i < number; i++)
        {
            if (number % i == 0)  return false;
        }
        return true;
}

bool ProverkaKvadratChislo(long double number)
{
        if (number < 0) return false;
        long n1 = (long) sqrt(number);
		
        if (n1 * n1 == number)
        {
            return true;
        }
        else
	    {
            return false;
	    }            
}
bool isNumber(const string& s)
{
    if (s.find_first_not_of("0123456789") == -1)	
	{
			if (s != "")
			{
				return true;
			}
			else
			{
				return false;
			}
	}
	else
	{
			return false;
	}
}



int main() {
		setlocale(LC_ALL,"Rus");


		mt19937 rng( std::time(0) ) ;
		uniform_int_distribution<long long> distr( 101, 9999999 ) ;
		
	
		const int kol_vo_elementov = 100000;
		const int kol_vo_kod_par = 100;
		long kol_vo_potokov;
		long b = 55434;
        

		static long long A[kol_vo_elementov];
		static long long C1[kol_vo_elementov];
		static long long C2[kol_vo_elementov];
		long long N111[2][kol_vo_kod_par];




		bool flag = false;
		string numberStr;
		long i, j;
		double timer;
		

		
		cout << "������� ���������� ������� (�� ����� " << kol_vo_elementov << "): ";
		do
		{
				getline(cin, numberStr);
				
				flag = isNumber(numberStr);
				if (!flag)
				{
					cout << "���������� ������� ������� �����������. ���������� ��� ���.";
				}
				else
				{
					kol_vo_potokov = stoi(numberStr);
					if (kol_vo_elementov > kol_vo_potokov)
					{
							break;
					}
					else
					{
							cout << "���������� ������� ������ ���� ������ ���������� ��������� �������. ���������� ��� ���." << endl;
					}
				}
		} while (5 == 5);
		cout << "���������� ������� = " << kol_vo_potokov << endl;
		
		
		
		omp_set_num_threads(kol_vo_potokov);
		srand((unsigned) time(NULL));



		//��������� ������������������ �����
		for (i = 0; i < kol_vo_elementov; i++)
		{
			A[i] = distr(rng);
			C1[i] = distr(rng);
			C2[i] = 1 + (rand() % 3);
			//cout << C1[i] << endl;
		}
		for (j = 0; j < kol_vo_kod_par; j++)
		{
			N111[0][j] = j;
			N111[1][j] = 2*j;
		}

		

		//=========================================================================================================================================
		cout << "\r\n\r\n������� � 1. �������� ��������� �� ����������� ������� � � �." << endl;
		long cnt1 = 0;
		timer = omp_get_wtime();		
		for (i = 0; i < kol_vo_elementov; i++)
		{
			cnt1 += (A[i] == C1[i] ? 1 : 0);
		}
		timer = omp_get_wtime() - timer;
		cout << "��� �������. � �������� � � � ������� ����������� ���������: " << cnt1 << " (�����: " << timer << " �) " << endl;
		

		cnt1 = 0;		
		timer = omp_get_wtime();
		#pragma omp parallel for reduction (+:cnt1)
		for (i = 0; i < kol_vo_elementov; i++)
		{
			cnt1 += (A[i] == C1[i] ? 1 : 0);
		}
		timer = omp_get_wtime() - timer;
		cout << "� ��������. � �������� � � � ������� ����������� ���������: " << cnt1 << " (�����: " << timer << " �) " << endl << endl;
		
		
		
		//=========================================================================================================================================            
		cout << "������� � 2. ����������� ��������� ������� �" << endl;
		timer = omp_get_wtime();
		for (i = 0; i < kol_vo_elementov; i++)
		{
			for (j = 0; j < kol_vo_kod_par; j++)
			{
				if (C1[i] == N111[0][j]) C1[i] = N111[1][j];
			}
		}
		timer = omp_get_wtime() - timer;
		cout << "��� �������. ����������� ����������� (�����: " << timer << " �) " << endl;


		timer = omp_get_wtime();
		#pragma omp for 
		for (i = 0; i < kol_vo_elementov; i++)
		{
			for (j = 0; j < kol_vo_kod_par; j++)
			{
				if (C1[i] == N111[0][j]) C1[i] = N111[1][j];
			}
		}
		timer = omp_get_wtime() - timer;
		cout << "� ��������. ����������� ����������� (�����: " << timer << " �) " << endl << endl;



		//=========================================================================================================================================
		cout << "������� � 3. ����������� ���������� ��������� ����� b � ������ �." << endl;
		timer = omp_get_wtime();
		long cnt3 = 0;
		for (i = 0; i < kol_vo_elementov; i++)
		{
			cnt3 += (C1[i] == b ?  1 : 0);
		} 
		timer = omp_get_wtime() - timer;
		cout << "��� �������. � ������� � ����� " << b << " ����������� " << cnt3 << " ��� (�����: " << timer << " �). " << endl;



		timer = omp_get_wtime();
		cnt3 = 0;
		#pragma omp parallel for reduction (+:cnt3)
		for (i = 0; i < kol_vo_elementov; i++)
		{
			cnt3 += (C1[i] == b ?  1 : 0);
		} 
		timer = omp_get_wtime() - timer;
		cout << "� ��������. � ������� � ����� " << b << " ����������� " << cnt3 << " ���. (�����: " << timer << " �). " << endl << endl;



		//=========================================================================================================================================
		cout << "������� � 4. ����� ������������ ���� ��������� �������." << endl;
		timer = omp_get_wtime();
		unsigned long long itog = 1;
		for (i = 0; i < kol_vo_elementov; i++)
		{
			itog = itog * C2[i];
		}
		timer = omp_get_wtime() - timer;
		cout << "��� �������. ��������� = " << itog << " (�����: " << timer << " �). " << endl;



		timer = omp_get_wtime();
		itog = 1;
		#pragma omp parallel for reduction (*:itog)
		for (i = 0; i < kol_vo_elementov; i++)
		{
			itog = itog * C2[i];
		}
		timer = omp_get_wtime() - timer;
		cout << "� ��������. ��������� = " << itog << " (�����: " << timer << " �). " << endl << endl;



		//=========================================================================================================================================
		cout << "������� � 5. ����� ������������� �������� �������." << endl;
		timer = omp_get_wtime();
		long long max = 0;
		for (i = 0; i < kol_vo_elementov; i++)
		{
			if (i == 0) max = C1[i];
			
			if (C1[i] > max) max = C1[i];
		}
		timer = omp_get_wtime() - timer;
		cout << "��� �������. ��������� = " << max << " (�����: " << timer << " �). " << endl;



		timer = omp_get_wtime();
		max = 0;
		#pragma omp for 
		for (i = 0; i < kol_vo_elementov; i++)
		{
			if (i == 0) max = C1[i];
			
			if (C1[i] > max) max = C1[i];
		}
		timer = omp_get_wtime() - timer;
		cout << "� ��������. ��������� = " << max << " (�����: " << timer << " �). " << endl << endl;



		//=========================================================================================================================================
		cout << "������� � 6. ����� ������������ �������� �������." << endl;
		timer = omp_get_wtime();
		long long min = 0;            
		for (i = 0; i < kol_vo_elementov; i++)
		{
			if (i == 0) min = C1[i];

			if (C1[i] < min) min = C1[i];
		}
		timer = omp_get_wtime() - timer;
		cout << "��� �������. ��������� = " << min << " (�����: " << timer << " �). " << endl;



		timer = omp_get_wtime();
		min = 0;
		#pragma omp for 
		for (i = 0; i < kol_vo_elementov; i++)
		{
			if (i == 0) min = C1[i];

			if (C1[i] < min) min = C1[i];
		}
		timer = omp_get_wtime() - timer;
		cout << "� ��������. ��������� = " << min << " (�����: " << timer << " �). " << endl << endl;


		
		//=========================================================================================================================================
		cout << "������� � 7. ����� ������� �����." << endl;
		timer = omp_get_wtime();
		int cnt7 = 0;
		for (i = 0; i < kol_vo_elementov; i++)
		{
				cnt7 += (ProverkaProstoeChislo(C1[i]) ? 1 : 0);
		}
		timer = omp_get_wtime() - timer;
		cout << "��� �������. ������� ������� ����� � �������: " << cnt7 << " (�����: " << timer << " �). " << endl;



		timer = omp_get_wtime();
		cnt7 = 0;
		#pragma omp parallel for reduction (+:cnt7)
		for (i = 0; i < kol_vo_elementov; i++)
		{
				cnt7 += (ProverkaProstoeChislo(C1[i]) ? 1 : 0);
		}
		timer = omp_get_wtime() - timer;
		cout << "� ��������. ������� ������� ����� � �������: " << cnt7 << " (�����: " << timer << " �). " << endl << endl;



		//=========================================================================================================================================
		cout << "������� � 8. ����� ��������� �������, ���������� ���������� ������ ������������ �����." << endl;
		timer = omp_get_wtime();
		long long cnt8 = 0;
		for (i = 0; i < kol_vo_elementov; i++)
		{
				cnt8 += (ProverkaKvadratChislo(C1[i]) ? 1 : 0);
		}
		timer = omp_get_wtime() - timer;
		cout << "��� �������. ��������� = " << cnt8 << " (�����: " << timer << " �). " << endl;



		timer = omp_get_wtime();
		cnt8 = 0;
		#pragma omp parallel for reduction (+:cnt8)
		for (i = 0; i < kol_vo_elementov; i++)
		{
				cnt8 += (ProverkaKvadratChislo(C1[i]) ? 1 : 0);
		}
		timer = omp_get_wtime() - timer;
		cout << "� ��������. ��������� = " << cnt8 << " (�����: " << timer << " �). " << endl << endl;
		


        //=========================================================================================================================================
        cout << "������� � 9. ����� ����� ��������� �0-�1 + �2-�3 + �4-�5 + ..." << endl;
        timer = omp_get_wtime();
        long long sum9 = 0;
        for (i = 0; i < kol_vo_elementov; i++)
        {
            sum9 += ((i % 2) == 0 ? C1[i] : -1* C1[i]);
			//cout << C1[i] << endl;
        }
        timer = omp_get_wtime() - timer;
		cout << "��� �������. ��������� = " << sum9 << " (�����: " << timer << " �). " << endl;



        timer = omp_get_wtime();
        sum9 = 0;
		#pragma omp parallel for reduction (+:sum9)
        for (i = 0; i < kol_vo_elementov; i++)
        {
			sum9 += ((i % 2) == 0 ? C1[i] : -1* C1[i]);
			//cout << C1[i] << endl;
        }
        timer = omp_get_wtime() - timer;
		cout << "� ��������. ��������� = " << sum9 << " (�����: " << timer << " �). " << endl << endl;
		

		
		//=========================================================================================================================================
		cout << "������� � 10. ����� ����� ���� ������ ����� �������" << endl;
		timer = omp_get_wtime();
		long long sum10 = 0;
		for (i = 0; i < kol_vo_elementov; i++)
		{
			sum10 += (C1[i] % 2 == 0 ? C1[i] : 0);
		}
		timer = omp_get_wtime() - timer;
		cout << "��� �������. ��������� = " << sum10 << " (�����: " << timer << " �). " << endl;



		timer = omp_get_wtime();
		sum10 = 0;
		#pragma omp parallel for reduction (+:sum10)
		for (i = 0; i < kol_vo_elementov; i++)
		{
			sum10 += (C1[i] % 2 == 0 ? C1[i] : 0);
		}
		timer = omp_get_wtime() - timer;
		cout << "� ��������. ��������� = " << sum10 << " (�����: " << timer << " �). " << endl << endl;
		/**/




		system("pause");	
}
