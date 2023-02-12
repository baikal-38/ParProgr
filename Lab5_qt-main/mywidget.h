#ifndef MYWIDGET_H
#define MYWIDGET_H

#include <QMainWindow>
#include <QtWidgets>

struct Task {
    int beginIndex;
    int endIndex;
    int task_num;
    int b_numb;
    int nElem_for_shifr;
    QList<qreal>* Nabor1;
    QList<qreal>* Nabor2;
    QList<qreal>* list1;
    QList<qreal>* list2;
};




namespace Ui {
class mywidget;
}

class mywidget : public QMainWindow
{
    Q_OBJECT

public:
    explicit mywidget(QWidget *parent = 0);
    ~mywidget();    

private slots:
    void on_startButton_clicked();
    void progressValueChanged(int v);
    void finished1();
    void finished2();
    void finished3();
    void finished4();
    void finished5();
    void finished6();
    void finished7();
    void finished8();
    void finished9();
    void finished10();
    void on_stopButton_clicked();
    void slotTimerAlarm();
    void on_BezPotokovBtn_clicked();
    void array_generation();

private:
    QTextEdit *logWindow;
    Ui::mywidget *ui;

    int nThreads;
    int h;
    int nElem;
    int nElem_for_shifr;

    QList<qreal> arrA;
    QList<qreal> arrA_encode;
    QList<qreal> arrC;

    QList<qreal> kodir1;
    QList<qreal> kodir2;

    QTime time;
    QList<Task> tasks;
    QFutureWatcher<qreal> *watcher1;
    QFutureWatcher<qreal> *watcher2;
    QFutureWatcher<qreal> *watcher3;
    QFutureWatcher<qreal> *watcher4;
    QFutureWatcher<qreal> *watcher5;
    QFutureWatcher<qreal> *watcher6;
    QFutureWatcher<qreal> *watcher7;
    QFutureWatcher<qreal> *watcher8;
    QFutureWatcher<qreal> *watcher9;
    QFutureWatcher<qreal> *watcher10;
    QFuture<qreal> future1;
    QFuture<qreal> future2;
    QFuture<qreal> future3;
    QFuture<qreal> future4;
    QFuture<qreal> future5;
    QFuture<qreal> future6;
    QFuture<qreal> future7;
    QFuture<qreal> future8;
    QFuture<qreal> future9;
    QFuture<qreal> future10;


    QTimer *timer;
};

#endif // MYWIDGET_H
