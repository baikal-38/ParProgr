#include <QtConcurrent>
#include "mywidget.h"
#include "ui_mywidget.h"
#include "qmath.h"
#include <QMessageBox>

#include <ctime>
#include <random>

using namespace std;

bool ProverkaProstoeChislo(int number)
{
        if (number < 2)   return false;
        for (int i = 2; i < number; i++)
        {
            if (number % i == 0)  return false;
        }
        return true;
}

bool ProverkaKvadratChislo(int number)
{
        if (number < 0) return false;
        int n1 = qSqrt(number);
        if ((n1 * n1) == number)
        {
            return true;
        }
        else
        {
            return false;
        }
}

qreal perElementFunc1(const Task tsk) {
     qreal res = 0;

     if (tsk.task_num == 4) res = 1;

     for (int i=tsk.beginIndex; i<=tsk.endIndex; i++)
     {
         int v1 = tsk.Nabor1->at(i);


         if (tsk.task_num == 1)                                 //поиск поэлементно совпадающих элементов массивов А и С
         {
             int v2 = tsk.Nabor2->at(i);
             if (v1 == v2) res++;
         }
         if (tsk.task_num == 2)
         {
             for (int j = 0; j < tsk.nElem_for_shifr; j++)
             {
                 double k1 = tsk.list1->at(j);
                 double k2 = tsk.list2->at(j);

                 if (v1 == k1)
                 {
                     tsk.Nabor1->removeAt(i);
                     tsk.Nabor1->insert(i, k2);
                     res++;
                 }
             }
         }
         if (tsk.task_num == 3)
         {
             if (tsk.b_numb == v1)  res++;
         }
         if (tsk.task_num == 4)
         {
             /*
             QFile file("out.txt");
             if (!file.open(QIODevice::Append))
                 return 0;
             QTextStream out(&file);
             out << "The magic number is: " << tsk.beginIndex << "__" << tsk.endIndex << "__"<< res << "__" << v1 << "\r\n";
             */

             res = res * v1;             
         }
         if (tsk.task_num == 5)                                 //поиск максимального ai
         {
             if (i == tsk.beginIndex)  res = v1;
             if (res <= v1) res = v1;
         }
         if (tsk.task_num == 6)                                 //поиск минимального ai
         {
             if (i == tsk.beginIndex)  res = v1;
             if (res >= v1) res = v1;
         }
         if (tsk.task_num == 7)
         {
             if (ProverkaProstoeChislo(v1)) res++;
         }
         if (tsk.task_num == 8)
         {
             if (ProverkaKvadratChislo(v1)) res++;
         }
         if (tsk.task_num == 9)                                 //вычисление выражения a0-a1+a2-a3+a4-a5+...
         {
             if ((i % 2) == 0)
             {
                 res = res + v1;
             }
             else
             {
                 res = res - v1;
             }
         }
         if (tsk.task_num == 10)                                //поиск суммы четных чисел
         {
             if ((v1 % 2) == 0) res += v1;
         }
         //QThread::msleep(1);
     }
     return res;
}

void reduce1(qreal & sum1, const qreal semiSum1) {
    sum1 += semiSum1;
}
void reduce2(qreal & sum2, const qreal semiSum2) {
    sum2 += semiSum2;
}
void reduce3(qreal & sum3, const qreal semiSum3) {
    sum3 += semiSum3;
}
void reduce4(qreal & sum4, const qreal semiSum4) {
    if (sum4 == 0) sum4 = 1;
    sum4 = sum4 * semiSum4;
}
void reduce5(qreal & sum5, const qreal semiSum5) {
    if (semiSum5 >= sum5) sum5 = semiSum5;
}
void reduce6(qreal & sum6, const qreal semiSum6) {
    if (sum6 == 0) sum6 = semiSum6;
    if (semiSum6 <= sum6) sum6 = semiSum6;
}
void reduce7(qreal & sum7, const qreal semiSum7) {
    sum7 += semiSum7;
}
void reduce8(qreal & sum8, const qreal semiSum8) {
    sum8 += semiSum8;
}
void reduce9(qreal & sum9, const qreal semiSum9) {
    sum9 += semiSum9;
}
void reduce10(qreal & sum10, const qreal semiSum10) {
    sum10 += semiSum10;
}


void mywidget::array_generation()
{
    mt19937 rng( std::time(0) ) ;
    uniform_int_distribution<long long> distr( 101, 99999 );


    for (int i=0; i<=nElem-1; i++)
    {
        qreal q1 = distr(rng);
        qreal q2 = distr(rng);
        //q1 = i+1;
        //q2 = i+1;
        arrA.append(q1);
        arrA_encode.append(q1);
        arrC.append(q2);
        if (ui->checkBox->isChecked()) ui->numbWindow1->append("n=" + QString::number(i, 'g', 35) + ", A[n]=" + QString::number(q1, 'g', 35) + ",  C[n]=" + QString::number(q2, 'g', 35) + "");
    }

    for (int i=0; i<=nElem_for_shifr-1; i++)
    {
        qreal k1 = distr(rng);
        qreal k2 = distr(rng);
        //k1 = i+1;
        //k2 = (i+1)*2;
        kodir1.append(k1);
        kodir2.append(k2);
        if (ui->checkBox->isChecked()) ui->numbWindow2->append("i=" + QString::number(i, 'g', 35) + ", ai=" + QString::number(k1, 'g', 35) + ",  bi=" + QString::number(k2, 'g', 35) + "");
    }
}


mywidget::mywidget(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::mywidget)
{
    ui->setupUi(this);


    watcher1 = new QFutureWatcher<qreal>();
    watcher2 = new QFutureWatcher<qreal>();
    watcher3 = new QFutureWatcher<qreal>();
    watcher4 = new QFutureWatcher<qreal>();
    watcher5 = new QFutureWatcher<qreal>();
    watcher6 = new QFutureWatcher<qreal>();
    watcher7 = new QFutureWatcher<qreal>();
    watcher8 = new QFutureWatcher<qreal>();
    watcher9 = new QFutureWatcher<qreal>();
    watcher10 = new QFutureWatcher<qreal>();


    timer = new QTimer();
    connect(timer, SIGNAL(timeout()), this, SLOT(slotTimerAlarm()));
    timer->start(1000); // И запустим таймер

    nElem = 1000000;
    nElem_for_shifr = 100;
}

mywidget::~mywidget()
{
    delete ui;
}

void mywidget::on_startButton_clicked()
{
    ui->startButton->setEnabled(false);
    ui->stopButton->setEnabled(true);


    if (arrA.count() == 0) array_generation();


    if (nElem < nElem_for_shifr)
    {
        QMessageBox::warning(this, "Внимание","Количество кодирующих элементов д.б. меньше набора чисел!\r\nДалее будет осуществлен выход из программы.");
        close();
    }

    nThreads = QThread::idealThreadCount();
    h = nElem / nThreads;



    ui->logWindow->append("\r\n\r\n===========================================================");
    ui->logWindow->append("Запуск процесса вычислений......");
    ui->logWindow->append("Количество потоков выбрано функцией idealThreadCount.");
    ui->logWindow->append("Количество потоков: " + QString::number(nThreads));
    ui->logWindow->append("Количество элементов: " + QString::number(nElem) + "\r\n");


    if (ui->checkBox->isChecked()) ui->numbWindow1->append("\r\n\r\n");
    if (ui->checkBox->isChecked()) ui->numbWindow2->append("\r\n\r\n");



    Task tsk;

    //задание 1
    ui->logWindow->append("Старт задания 1");
    time.start();
    for (int i=0; i<=nThreads-1; i++)
    {
        tsk.beginIndex = i * h;
        tsk.endIndex = i * h + h - 1;
        if (i == nThreads - 1) tsk.endIndex = nElem - 1;

        tsk.Nabor1 = &arrA;
        tsk.Nabor2 = &arrC;
        tsk.task_num = 1;

        tasks.append(tsk);
    }    
    connect(watcher1,SIGNAL(progressValueChanged(int)),this,SLOT(progressValueChanged(int)));
    connect(watcher1,SIGNAL(finished()),this,SLOT(finished1()));
    future1 = QtConcurrent::mappedReduced(tasks, perElementFunc1, reduce1);
    watcher1->setFuture(future1);
}

void mywidget::progressValueChanged(int v) {
    //ui->logWindow->append(tr("Progress: ")+QString::number(v));
}

void mywidget::finished1() {
    qreal rez = watcher1->result();
    ui->logWindow->append("1. Количество поэлементно совпадающих элементов в массивах А и С: "          + QString::number(rez, 'g', 35) + ". "                              + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    Task tsk;
    //задание 2
    ui->logWindow->append("Старт задания 2");
    tasks.clear();
    time.restart();
    for (int i=0; i<=nThreads-1; i++)
    {
        tsk.beginIndex = i * h;
        tsk.endIndex = i * h + h - 1;
        if (i == nThreads - 1) tsk.endIndex = nElem - 1;

        tsk.Nabor1 = &arrA_encode;
        tsk.nElem_for_shifr = nElem_for_shifr;
        tsk.list1 = &kodir1;
        tsk.list2 = &kodir2;

        tsk.task_num = 2;

        tasks.append(tsk);
    }
    connect(watcher2,SIGNAL(progressValueChanged(int)),this,SLOT(progressValueChanged(int)));
    connect(watcher2,SIGNAL(finished()),this,SLOT(finished2()));
    future2 = QtConcurrent::mappedReduced(tasks, perElementFunc1, reduce2);
    watcher2->setFuture(future2);
}
void mywidget::finished2() {
    qreal rez = watcher2->result();
    ui->logWindow->append("2. Закодировано элементов: "                                                 + QString::number(rez, 'g', 35) + ". "                              + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");

    if (ui->checkBox->isChecked())
    {
        ui->numbWindow3->append("\r\n");
        for (int i=0; i<=nElem-1; i++)
        {
            qreal q1 = arrA_encode.at(i);
            ui->numbWindow3->append(QString::number(i, 'g', 35) + ") " + QString::number(q1, 'g', 35));
        }
    }


    Task tsk;
    //задание 3
    ui->logWindow->append("Старт задания 3");
    tasks.clear();
    time.restart();
    for (int i=0; i<=nThreads-1; i++)
    {
        tsk.beginIndex = i * h;
        tsk.endIndex = i * h + h - 1;
        if (i == nThreads - 1) tsk.endIndex = nElem - 1;

        tsk.Nabor1 = &arrA;
        tsk.task_num = 3;

        QString str = ui->lineEdit->text();
        bool ok;
        int num = str.toInt(&ok);
        if (ok)
        {
            tsk.b_numb = num;
        }
        else
        {
            // conversion failed
        }

        tasks.append(tsk);
    }
    connect(watcher3,SIGNAL(progressValueChanged(int)),this,SLOT(progressValueChanged(int)));
    connect(watcher3,SIGNAL(finished()),this,SLOT(finished3()));
    future3 = QtConcurrent::mappedReduced(tasks, perElementFunc1, reduce3);
    watcher3->setFuture(future3);
}
void mywidget::finished3() {
    qreal rez = watcher3->result();
    ui->logWindow->append("3. Число '" + ui->lineEdit->text() + "' входит "                             + QString::number(rez, 'g', 35) + " раз в последовательность A. "   + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    Task tsk;
    //задание 4
    ui->logWindow->append("Старт задания 4");
    tasks.clear();
    time.restart();
    for (int i=0; i<=nThreads-1; i++)
    {
        tsk.beginIndex = i * h;
        tsk.endIndex = i * h + h - 1;
        if (i == nThreads - 1) tsk.endIndex = nElem - 1;

        tsk.Nabor1 = &arrA;
        tsk.task_num = 4;

        tasks.append(tsk);
    }
    connect(watcher4,SIGNAL(progressValueChanged(int)),this,SLOT(progressValueChanged(int)));
    connect(watcher4,SIGNAL(finished()),this,SLOT(finished4()));
    future4 = QtConcurrent::mappedReduced(tasks, perElementFunc1, reduce4);
    watcher4->setFuture(future4);
}
void mywidget::finished4() {
    qreal rez = watcher4->result();
    ui->logWindow->append("4. Произведение чисел a0*a1*a2*...*aN: "                                     + QString::number(rez, 'g', 35) + ". "                              + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    Task tsk;
    //задание 5
    ui->logWindow->append("Старт задания 5");
    tasks.clear();
    time.restart();
    for (int i=0; i<=nThreads-1; i++)
    {
        tsk.beginIndex = i * h;
        tsk.endIndex = i * h + h - 1;
        if (i == nThreads - 1) tsk.endIndex = nElem - 1;

        tsk.Nabor1 = &arrA;
        tsk.task_num = 5;

        tasks.append(tsk);
    }
    connect(watcher5,SIGNAL(progressValueChanged(int)),this,SLOT(progressValueChanged(int)));
    connect(watcher5,SIGNAL(finished()),this,SLOT(finished5()));
    future5 = QtConcurrent::mappedReduced(tasks, perElementFunc1, reduce5);
    watcher5->setFuture(future5);
}
void mywidget::finished5() {
    qreal rez = watcher5->result();
    ui->logWindow->append("5. Максимальный элемент: "                                                   + QString::number(rez, 'g', 35) + ". "                              + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    Task tsk;
    //задание 6
    ui->logWindow->append("Старт задания 6");
    tasks.clear();
    time.restart();
    for (int i=0; i<=nThreads-1; i++)
    {
        tsk.beginIndex = i * h;
        tsk.endIndex = i * h + h - 1;
        if (i == nThreads - 1) tsk.endIndex = nElem - 1;

        tsk.Nabor1 = &arrA;
        tsk.task_num = 6;

        tasks.append(tsk);
    }
    connect(watcher6,SIGNAL(progressValueChanged(int)),this,SLOT(progressValueChanged(int)));
    connect(watcher6,SIGNAL(finished()),this,SLOT(finished6()));
    future6 = QtConcurrent::mappedReduced(tasks, perElementFunc1, reduce6);
    watcher6->setFuture(future6);
}
void mywidget::finished6() {
    qreal rez = watcher6->result();
    ui->logWindow->append("6. Минимальный элемент: "                                                    + QString::number(rez, 'g', 35) + ". "                              + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    Task tsk;
    //задание 7
    ui->logWindow->append("Старт задания 7");
    tasks.clear();
    time.restart();
    for (int i=0; i<=nThreads-1; i++)
    {
        tsk.beginIndex = i * h;
        tsk.endIndex = i * h + h - 1;
        if (i == nThreads - 1) tsk.endIndex = nElem - 1;

        tsk.Nabor1 = &arrA;
        tsk.task_num = 7;

        tasks.append(tsk);
    }
    connect(watcher7,SIGNAL(progressValueChanged(int)),this,SLOT(progressValueChanged(int)));
    connect(watcher7,SIGNAL(finished()),this,SLOT(finished7()));
    future7 = QtConcurrent::mappedReduced(tasks, perElementFunc1, reduce7);
    watcher7->setFuture(future7);
}
void mywidget::finished7() {
    qreal rez = watcher7->result();
    ui->logWindow->append("7. Количество элементов, являющихся простыми числами: "                      + QString::number(rez, 'g', 35) + ". "                              + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    Task tsk;
    //задание 8
    ui->logWindow->append("Старт задания 8");
    tasks.clear();
    time.restart();
    for (int i=0; i<=nThreads-1; i++)
    {
        tsk.beginIndex = i * h;
        tsk.endIndex = i * h + h - 1;
        if (i == nThreads - 1) tsk.endIndex = nElem - 1;

        tsk.Nabor1 = &arrA;
        tsk.task_num = 8;

        tasks.append(tsk);
    }
    connect(watcher8,SIGNAL(progressValueChanged(int)),this,SLOT(progressValueChanged(int)));
    connect(watcher8,SIGNAL(finished()),this,SLOT(finished8()));
    future8 = QtConcurrent::mappedReduced(tasks, perElementFunc1, reduce8);
    watcher8->setFuture(future8);
}
void mywidget::finished8() {
    qreal rez = watcher8->result();
    ui->logWindow->append("8. Количество элементов, являющихся квадратами любого натурального числа: "  + QString::number(rez, 'g', 35) + ". "                              + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    Task tsk;
    //задание 9
    ui->logWindow->append("Старт задания 9");
    tasks.clear();
    time.restart();
    for (int i=0; i<=nThreads-1; i++)
    {
        tsk.beginIndex = i * h;
        tsk.endIndex = i * h + h - 1;
        if (i == nThreads - 1) tsk.endIndex = nElem - 1;

        tsk.Nabor1 = &arrA;
        tsk.task_num = 9;

        tasks.append(tsk);
    }
    connect(watcher9,SIGNAL(progressValueChanged(int)),this,SLOT(progressValueChanged(int)));
    connect(watcher9,SIGNAL(finished()),this,SLOT(finished9()));
    future9 = QtConcurrent::mappedReduced(tasks, perElementFunc1, reduce9);
    watcher9->setFuture(future9);
}
void mywidget::finished9() {
    qreal rez = watcher9->result();
    ui->logWindow->append("9. Вычисление выражения a0-a1+a2-a3+a4-a5+...: "                             + QString::number(rez, 'g', 35) + ". "                              + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    Task tsk;
    //задание 10
    ui->logWindow->append("Старт задания 10");
    tasks.clear();
    time.restart();
    for (int i=0; i<=nThreads-1; i++)
    {
        tsk.beginIndex = i * h;
        tsk.endIndex = i * h + h - 1;
        if (i == nThreads - 1) tsk.endIndex = nElem - 1;

        tsk.Nabor1 = &arrA;
        tsk.task_num = 10;

        tasks.append(tsk);
    }
    connect(watcher10,SIGNAL(progressValueChanged(int)),this,SLOT(progressValueChanged(int)));
    connect(watcher10,SIGNAL(finished()),this,SLOT(finished10()));
    future10 = QtConcurrent::mappedReduced(tasks, perElementFunc1, reduce10);
    watcher10->setFuture(future10);
}
void mywidget::finished10() {
    qreal rez = watcher10->result();
    ui->logWindow->append("10. Сумма четных чисел: "                                                    + QString::number(rez, 'g', 35) + ". "                              + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");
}

void mywidget::on_stopButton_clicked()
{
    if (ui->stopButton->text() == "Пауза") {   ui->stopButton->setText("Запустить");  } else  {   ui->stopButton->setText("Пауза");    }

    if (watcher1->isPaused())   {   watcher1->resume();   } else  {   watcher1->pause();     }
    if (watcher2->isPaused())   {   watcher2->resume();   } else  {   watcher2->pause();     }
    if (watcher3->isPaused())   {   watcher3->resume();   } else  {   watcher3->pause();     }
    if (watcher4->isPaused())   {   watcher4->resume();   } else  {   watcher4->pause();     }
    if (watcher5->isPaused())   {   watcher5->resume();   } else  {   watcher5->pause();     }
    if (watcher6->isPaused())   {   watcher6->resume();   } else  {   watcher6->pause();     }
    if (watcher7->isPaused())   {   watcher7->resume();   } else  {   watcher7->pause();     }
    if (watcher8->isPaused())   {   watcher8->resume();   } else  {   watcher8->pause();     }
    if (watcher9->isPaused())   {   watcher9->resume();   } else  {   watcher9->pause();     }
    if (watcher10->isPaused())  {   watcher10->resume();  } else  {   watcher10->pause();    }
}


void mywidget::slotTimerAlarm()
{
    ui->label->setText(QTime::currentTime().toString("Время: hh:mm:ss"));

    if (watcher1->isStarted())   ui->label_2->setText("Задача №1: Стартовала");
    if (watcher1->isRunning())   ui->label_2->setText("Задача №1: Работает");
    if (watcher1->isPaused())    ui->label_2->setText("Задача №1: На паузе");
    if (watcher1->isFinished())  ui->label_2->setText("Задача №1: Завершена");

    if (watcher2->isStarted())   ui->label_3->setText("Задача №2: Стартовала");
    if (watcher2->isRunning())   ui->label_3->setText("Задача №2: Работает");
    if (watcher2->isPaused())    ui->label_3->setText("Задача №2: На паузе");
    if (watcher2->isFinished())  ui->label_3->setText("Задача №2: Завершена");

    if (watcher3->isStarted())   ui->label_4->setText("Задача №3: Стартовала");
    if (watcher3->isRunning())   ui->label_4->setText("Задача №3: Работает");
    if (watcher3->isPaused())    ui->label_4->setText("Задача №3: На паузе");
    if (watcher3->isFinished())  ui->label_4->setText("Задача №3: Завершена");

    if (watcher4->isStarted())   ui->label_5->setText("Задача №4: Стартовала");
    if (watcher4->isRunning())   ui->label_5->setText("Задача №4: Работает");
    if (watcher4->isPaused())    ui->label_5->setText("Задача №4: На паузе");
    if (watcher4->isFinished())  ui->label_5->setText("Задача №4: Завершена");

    if (watcher5->isStarted())   ui->label_6->setText("Задача №5: Стартовала");
    if (watcher5->isRunning())   ui->label_6->setText("Задача №5: Работает");
    if (watcher5->isPaused())    ui->label_6->setText("Задача №5: На паузе");
    if (watcher5->isFinished())  ui->label_6->setText("Задача №5: Завершена");

    if (watcher6->isStarted())   ui->label_7->setText("Задача №6: Стартовала");
    if (watcher6->isRunning())   ui->label_7->setText("Задача №6: Работает");
    if (watcher6->isPaused())    ui->label_7->setText("Задача №6: На паузе");
    if (watcher6->isFinished())  ui->label_7->setText("Задача №6: Завершена");

    if (watcher7->isStarted())   ui->label_8->setText("Задача №7: Стартовала");
    if (watcher7->isRunning())   ui->label_8->setText("Задача №7: Работает");
    if (watcher7->isPaused())    ui->label_8->setText("Задача №7: На паузе");
    if (watcher7->isFinished())  ui->label_8->setText("Задача №7: Завершена");

    if (watcher8->isStarted())   ui->label_9->setText("Задача №8: Стартовала");
    if (watcher8->isRunning())   ui->label_9->setText("Задача №8: Работает");
    if (watcher8->isPaused())    ui->label_9->setText("Задача №8: На паузе");
    if (watcher8->isFinished())  ui->label_9->setText("Задача №8: Завершена");

    if (watcher9->isStarted())   ui->label_10->setText("Задача №9: Стартовала");
    if (watcher9->isRunning())   ui->label_10->setText("Задача №9: Работает");
    if (watcher9->isPaused())    ui->label_10->setText("Задача №9: На паузе");
    if (watcher9->isFinished())  ui->label_10->setText("Задача №9: Завершена");

    if (watcher10->isStarted())   ui->label_11->setText("Задача №10: Стартовала");
    if (watcher10->isRunning())   ui->label_11->setText("Задача №10: Работает");
    if (watcher10->isPaused())    ui->label_11->setText("Задача №10: На паузе");
    if (watcher10->isFinished())  ui->label_11->setText("Задача №10: Завершена");
}








void mywidget::on_BezPotokovBtn_clicked()
{
    ui->logWindow->append("\r\n\r\n===========================================================");
    ui->logWindow->append("Запуск расчетов без потоков.");

    ui->BezPotokovBtn->setEnabled(false);


    if (ui->checkBox->isChecked()) ui->numbWindow1->append("\r\n");
    if (ui->checkBox->isChecked()) ui->numbWindow2->append("\r\n");



    if (arrA.count() == 0) array_generation();



    qreal rez1 = 0;
    qreal rez2 = 0;
    qreal rez3 = 0;
    qreal rez4 = 1;
    qreal rez5 = 0;
    qreal rez6 = 0;
    qreal rez7 = 0;
    qreal rez8 = 0;
    qreal rez9 = 0;
    qreal rez10 = 0;


    QString str = ui->lineEdit->text();
    bool ok;
    int b_numb;

    int num = str.toInt(&ok);
    if (ok)
    {
        b_numb = num;
    }
    else
    {
        // conversion failed
    }


    time.restart();
    for (int i=0; i<=nElem-1; i++)
    {
        int v1 = arrA.at(i);
        int v2 = arrC.at(i);

        if (v1 == v2) rez1++;
    }
    ui->logWindow->append("1. Количество поэлементно совпадающих элементов в массивах А и С: "          + QString::number(rez1, 'g', 35) + ". "                             + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    time.restart();
    for (int i=0; i<=nElem-1; i++)
    {
        int v1 = arrA.at(i);

        for (int j = 0; j < nElem_for_shifr; j++)
        {
            double k1 = kodir1.at(j);
            double k2 = kodir2.at(j);

            if (v1 == k1)
            {
                arrA_encode.removeAt(i);
                arrA_encode.insert(i, k2);
                rez2++;
            }
        }
    }
    ui->logWindow->append("2. Закодировано элементов: "                                                 + QString::number(rez2, 'g', 35) + ". "                             + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    time.restart();
    for (int i=0; i<=nElem-1; i++)
    {
        int v1 = arrA.at(i);

        if (b_numb == v1)  rez3++;
    }
    ui->logWindow->append("3. Число '" + ui->lineEdit->text() + "' входит "                             + QString::number(rez3, 'g', 35) + " раз в последовательность A. "  + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    time.restart();
    for (int i=0; i<=nElem-1; i++)
    {
        int v1 = arrA.at(i);

        rez4 = rez4 * v1;
    }
    ui->logWindow->append("4. Произведение чисел a0*a1*a2*...*aN: "                                     + QString::number(rez4, 'g', 35) + ". "                             + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    time.restart();
    for (int i=0; i<=nElem-1; i++)
    {
        int v1 = arrA.at(i);

        if (i == 0)  rez5 = v1;
        if (rez5 <= v1) rez5 = v1;
    }
    ui->logWindow->append("5. Максимальный элемент: "                                                   + QString::number(rez5, 'g', 35) + ". "                             + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    time.restart();
    for (int i=0; i<=nElem-1; i++)
    {
        int v1 = arrA.at(i);

        if (i == 0)  rez6 = v1;
        if (rez6 >= v1) rez6 = v1;
    }
    ui->logWindow->append("6. Минимальный элемент: "                                                    + QString::number(rez6, 'g', 35) + ". "                             + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    time.restart();
    for (int i=0; i<=nElem-1; i++)
    {
        int v1 = arrA.at(i);

        if (ProverkaProstoeChislo(v1)) rez7++;
    }
    ui->logWindow->append("7. Количество элементов, являющихся простыми числами: "                      + QString::number(rez7, 'g', 35) + ". "                             + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    time.restart();
    for (int i=0; i<=nElem-1; i++)
    {
        int v1 = arrA.at(i);

        if (ProverkaKvadratChislo(v1)) rez8++;
    }
    ui->logWindow->append("8. Количество элементов, являющихся квадратами любого натурального числа: "  + QString::number(rez8, 'g', 35) + ". "                             + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    time.restart();
    for (int i=0; i<=nElem-1; i++)
    {
        int v1 = arrA.at(i);

        if ((i % 2) == 0)
        {
                rez9 = rez9 + v1;
        }
        else
        {
                rez9 = rez9 - v1;
        }
    }
    ui->logWindow->append("9. Вычисление выражения a0-a1+a2-a3+a4-a5+...: "                             + QString::number(rez9, 'g', 35) + ". "                             + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");


    time.restart();
    for (int i=0; i<=nElem-1; i++)
    {
        int v1 = arrA.at(i);

        if ((v1 % 2) == 0) rez10 += v1;
    }
    ui->logWindow->append("10. Сумма четных чисел: "                                                    + QString::number(rez10,'g', 35) + ". "                             + tr("Затрачено времени: ") + " " + QString::number(time.elapsed())  + " мс.");



    if (ui->checkBox->isChecked())
    {
        ui->numbWindow3->append("\r\n");
        for (int i=0; i<=nElem-1; i++)
        {
            qreal q1 = arrA_encode.at(i);
            ui->numbWindow3->append(QString::number(i, 'g', 35) + ") " + QString::number(q1, 'g', 35));
        }
    }
}




