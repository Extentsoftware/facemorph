using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FaceMorph
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly IFaceMorpher _morpher;

        public MainWindow(IFaceMorpher morpher)
        {
            _morpher = morpher;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (await _morpher.Status(default))
                MessageBox.Show("Ok");
            else
                MessageBox.Show("Down");
        }

        private async void Morph3_Click(object sender, RoutedEventArgs e)
        {
            var cultures = Culture.Text.Split(",").Select(x => x.Trim()).ToList();
            foreach (var c in cultures)
            {
                string inTemplate = $"{c}_{{0}}.jpg";
                string dest= $"new_{c}_{{0}}.jpg";
                int iterations = int.Parse(Iterations.Text);
                int images = int.Parse(Images.Text);
                await _morpher.MultiMorph(SourceDirectory.Text, inTemplate, images, dest, iterations);
            }            
        }
    }
}
