using System;
using System.Net;
using System.Timers;
using System.Windows;

namespace subscribers_box{
  public partial class MainWindow : Window{
    private string channelID = "";

    public MainWindow(){
      InitializeComponent();
    }

    private void UpdateData(object source, ElapsedEventArgs e){
      string content = (new WebClient()).DownloadString($"https://www.youtube.com/channel/{channelID}");
      string toBeSearched = "subscriber-count";
      content = content.Substring(content.IndexOf(toBeSearched) + toBeSearched.Length);
      int from = content.IndexOf(">") + ">".Length;
      int to = content.IndexOf("<");
      content = content.Substring(from, to - from);

      Dispatcher.BeginInvoke(new Action(() => {
        Subscribers.Content = content;
        UpdateLayout();
      }));
    }

    private void Go_Click(object sender, RoutedEventArgs e){
      if(ChannelID.Text == ""){
        return;
      }

      channelID = ChannelID.Text;
      Start.Visibility = Visibility.Collapsed;

      UpdateData(null, null);
      Timer timer = new Timer();
      timer.Elapsed += new ElapsedEventHandler(UpdateData);
      timer.Interval = 5000;
      timer.Start();
    }
  }
}
