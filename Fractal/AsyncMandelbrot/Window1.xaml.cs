using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Threading.Tasks;


namespace AsyncMandelbrot
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        /// <summary>Размер изображения, ожидающего перерисовку или -1, если в очереди запросов нет</summary>
        private int pendingSize = -1;

        /// <summary>Работает ли в данный момент поток рисования фрактала</summary>
        private bool isRendering = false;

        public Window1()
        {
            InitializeComponent();
        }

        private async void BeginRendering(int size)
        {
            isRendering = true;

            // Thread pool / BeginInvoke approach (old)

            //ThreadPool.QueueUserWorkItem(new WaitCallback(dummy =>
            //{
            //    BitmapSource result = MandelbrotSet.Render(size, size, -2.5, -2, 1.5, 2);
            //    result.Freeze();

            //    this.Dispatcher.BeginInvoke(new Action(() =>
            //    {
            //        contents.Source = result;
            //        isRendering = false;
            //        if (pendingSize != -1)
            //        {
            //            BeginRendering(pendingSize);
            //            pendingSize = -1;
            //        }
            //    }));
            //}));

            //TPL - based approach
            var uis = TaskScheduler.FromCurrentSynchronizationContext();

            _ = Task.Factory.StartNew(() =>
            {
                BitmapSource result = MandelbrotSet.Render(size, size, -2.5, -2, 1.5, 2);
                result.Freeze();
                return result;
            }).ContinueWith(t =>
            {
                contents.Source = t.Result;
                isRendering = false;
                if (pendingSize != -1)
                {
                    BeginRendering(pendingSize);
                    pendingSize = -1;
                }
            }, CancellationToken.None, TaskContinuationOptions.None, uis);

            //var rt = Task<BitmapSource>.Factory.StartNew(() =>
            //{
            //    BitmapSource result = MandelbrotSet.Render(size, size, -2.5, -2, 1.5, 2);
            //    result.Freeze();
            //    return result;
            //});

            //contents.Source = await rt;

            //isRendering = false;
            //if (pendingSize != -1)
            //{
            //    BeginRendering(pendingSize);
            //    pendingSize = -1;
            //}
        }

        private void EnqueueRenderRequest(int size)
        {
            if (isRendering)
                pendingSize = size;
            else
                BeginRendering(size);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            int size = (int)Math.Max(e.NewSize.Width, e.NewSize.Height);
            EnqueueRenderRequest(size);
        }
    }
}
