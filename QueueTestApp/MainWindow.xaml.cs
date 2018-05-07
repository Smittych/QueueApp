using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace QueueTestApp
{
    public partial class MainWindow : Window
    {
        static AwaitQueue<int> aQueue = new AwaitQueue<int>();  
        private int pushIdx;  //инкрементируемый id объекта очереди (используется как сам объект очереди)

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnPush_Click(object sender, RoutedEventArgs e)
        {
            //создание отдельного потока для push 
            var threadPush = new Thread(Push);
            threadPush.Start(pushIdx);

            pushIdx++;
        }

        private void Push(object pushItem)
        {
            aQueue.push((int)pushItem);

            Action action = () =>
            {
                FillPushList();
            };
            Dispatcher.Invoke(action);
        }

        private void btnPop_Click(object sender, RoutedEventArgs e)
        {
            //создание отдельного потока для pop 
            var threadPop = new Thread(Pop);
            threadPop.Start();
        }

        private void Pop()
        {
            var a = aQueue.pop();

            Action action = () =>
            {
                PopList.Items.Add(a);
                FillPushList();
            };
            Dispatcher.Invoke(action);
        }

        //отображение списка элеметнов очереди
        public void FillPushList()
        {
            PushList.Items.Clear();
            List<int> queueList = aQueue.QueueList;

            for (int i = 0; i < queueList.Count; i++)
            {
                PushList.Items.Add(queueList[i]);
            }
        }
    }
}
