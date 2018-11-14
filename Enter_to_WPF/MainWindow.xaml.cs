using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;

namespace Enter_to_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        class Item
        {
            public string title { get; set; }
            public string docText { get; set; }
            public Item()
            {
                title = "";
            }
            public Item(string title, string docText)
            {
                this.title = title;
                this.docText = docText;
            }
        }
        List<Item> textList = new List<Item>();


        public MainWindow()
        {
            InitializeComponent();
        }
        private void Button_Get_Info_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string source = "";

                XmlDocument document = new XmlDocument();
                document.Load("https://habr.com/rss/interesting/");

                XmlNodeList xnList = document.SelectNodes("/rss/channel/item");

                foreach (XmlNode xn in xnList)
                {
                    string titleDoc = xn["title"].InnerText;
                    string textDoc = xn["description"].InnerText;
                    textList.Add(new Item(titleDoc, textDoc));
                }

                foreach (var Doc in textList)
                {
                    source += Doc.title + "\n";
                    source += Doc.docText + "\n\n";
                }
                TextBox1.Text = source;
                TextBox_Message.Text = "XML файл прочитан";
                TextBox_Message.Background = Brushes.Green;
            }
            catch (Exception ex)
            {
                TextBox_Message.Text = "Не удалось прочитать XML файл" + ex;
                TextBox_Message.Background = Brushes.Red;
            }
        }

        private void Button_Creat_XML_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Запись Xml файла
                string writePath = @"Xml_File.xml";

                XmlTextWriter textWritter = new XmlTextWriter(writePath, Encoding.UTF8);
                textWritter.WriteStartDocument();
                textWritter.WriteStartElement("document");
                textWritter.WriteEndElement();
                textWritter.Close();

                XmlDocument DocWritter = new XmlDocument();
                DocWritter.Load(writePath);

                foreach (var Doc in textList)
                {
                    XmlNode element1 = DocWritter.CreateElement("title");
                    DocWritter.DocumentElement.AppendChild(element1); // указываем родителя
                    element1.InnerText = Doc.title;

                    XmlNode element2 = DocWritter.CreateElement("description");
                    DocWritter.DocumentElement.AppendChild(element2); // указываем родителя
                    element2.InnerText = Doc.docText;
                }
                DocWritter.Save(writePath);
                TextBox_Message.Text = "XML файл записан";
                TextBox_Message.Background = Brushes.Green;
            }
            catch (Exception ex)
            {
                TextBox_Message.Text = "Не удалось записать XML файл" + ex;
                TextBox_Message.Background = Brushes.Red;
            }
        }
    }
}
