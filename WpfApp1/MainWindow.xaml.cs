using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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

namespace WpfApp1
{
    static class LegacyDataServiceExtensions
    {
        public static Task<IEnumerable<string>> GetNamesAsync(this LegacyDataService service)
        {
            var tcs = new TaskCompletionSource<IEnumerable<string>>();

            Task.Run(() =>
            {
                var names = service.GetNames();
                tcs.SetResult(names);
            });

            return tcs.Task;
        }
    }


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private async void clickBtn_Click7(object sender, RoutedEventArgs e)
        {
            var service = new LegacyDataService();
            var names = await service.GetNamesAsync();
            TextBox.Text = string.Join("\n", names);
        }


        public Task<IEnumerable<string>> GetFakeNamesAsync()
        {
            return Task.FromResult((IEnumerable<string>)new[]
            {
                "Mehemmed",
                "Nicat",
                "Ilqar",
                "Murad",
                "Nezrin",
                "Elesger"
            });
        }

        private async void clickBtn_Click6(object sender, RoutedEventArgs e)
        {
            var names = await GetFakeNamesAsync();
            // Thread.Sleep(5000);
            await Task.Delay(5000);
            TextBox.Text = string.Join("\n", names);
        }


        class BlablablaStateMachine : IAsyncStateMachine
        {
            DataService service;
            IEnumerable<string> names;
            TextBox TextBox;
            int state = -1;


            public BlablablaStateMachine(int state, TextBox textBox)
            {
                TextBox = textBox;
                this.state = state;
            }

            public void MoveNext()
            {
                if (state == 0)
                {
                    service = new DataService();
                    service.GetNamesAsync()
                        .ContinueWith(task =>
                        {
                            names = task.Result;
                            MoveNext();
                        }, TaskScheduler.FromCurrentSynchronizationContext());

                    state = 1;
                }
                else if (state == 1)
                {
                    TextBox.Text = string.Join("\n", names);
                    state = -1;
                }
            }

            public void SetStateMachine(IAsyncStateMachine stateMachine)
            {
                // throw new NotImplementedException();
            }
        }

        // after compiler
        private void clickBtn_Click5(object sender, RoutedEventArgs e)
        {
            var stateMachine = new BlablablaStateMachine(0, TextBox);
            stateMachine.MoveNext();
        }

        // before compiler
        private async void clickBtn_Click4(object sender, RoutedEventArgs e)
        {
            var service = new DataService();
            var names = await service.GetNamesAsync();
            TextBox.Text = string.Join("\n", names);
        }

        private async void clickBtn_Click3(object sender, RoutedEventArgs e)
        {
            var wc = new WebClient();
            var result = await wc.DownloadStringTaskAsync("https://www.google.com/");
            TextBox.Text = result;
        }


        private void clickBtn_Click2(object sender, RoutedEventArgs e)
        {
            var wc = new WebClient();
            var context = SynchronizationContext.Current;

            var result = wc.DownloadStringTaskAsync("https://www.google.com/")
                .ContinueWith(task =>
                {
                    context.Send(_ =>
                    {
                        TextBox.Text = task.Result;
                    }, null);
                });
        }

        private void clickBtn_Click1(object sender, RoutedEventArgs e)
        {
            var wc = new WebClient();
            var result = wc.DownloadStringTaskAsync("https://big.az/")
                .ContinueWith(task =>
                {
                    TextBox.Text = task.Result;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }


        private void clickBtn_Click0(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(SomeTaskWithDispatcher);
            thread.Start();
        }
        void SomeTaskWithDispatcher()
        {
            var wc = new WebClient();
            var result = wc.DownloadString("https://www.google.com/");

            Thread.Sleep(5000);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                TextBox.Text = result;
            }));
        }

    }
}
