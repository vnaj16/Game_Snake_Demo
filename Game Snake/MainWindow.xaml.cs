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
using System.Windows.Threading;

namespace Game_Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            lista1.AddFirst(new Punto(0, 0));//La cabeza de la serpiente

            Timer.Tick += Timer_Tick;
            Timer.Interval = TimeSpan.FromMilliseconds(100);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //En la direccion en la que voy, agrego uno al principio y en RemoverCola quito uno del fondo para simular movimiento, ademas de agregar en cierta posicion para simular el movimiento izq,der,arriba o abajo
            if (Direccion == TDireccion.derecha)//Si mi direccion es derecha, cada segmento cuando llegue voltea a la derecha, pero siguiento en la misma posicion Y
                lista1.AddFirst(new Punto(lista1.First.Value.X + 1, lista1.First.Value.Y));
            if (Direccion == TDireccion.izquierda)
                lista1.AddFirst(new Punto(lista1.First.Value.X - 1, lista1.First.Value.Y));
            if (Direccion == TDireccion.arriba)
                lista1.AddFirst(new Punto(lista1.First.Value.X, lista1.First.Value.Y - 1));
            if (Direccion == TDireccion.abajo)
                lista1.AddFirst(new Punto(lista1.First.Value.X, lista1.First.Value.Y + 1));
            SaleMapa();
            SePisa();
            TocaFruta();
            RemoverCola();
            //Invalidate();
        }

        private enum TDireccion { izquierda, derecha, arriba, abajo };
        private TDireccion Direccion { get; set; } = TDireccion.derecha;
        private LinkedList<Punto> lista1 = new LinkedList<Punto>();//El cuerpo de la serpiente
        private Punto Fruta { get; set; } = new Punto(10, 10);
        private int Crecimiento { get; set; } = 0;

        private DispatcherTimer Timer = new DispatcherTimer();
      
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            foreach (var ele in lista1)
                e.Graphics.FillRectangle(new SolidBrush(Color.Red), ele.X * 10, ele.Y * 10, 9, 9);
            e.Graphics.FillRectangle(new SolidBrush(Color.Green), Fruta.X * 10, Fruta.Y * 10, 9, 9);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
                Direccion = TDireccion.derecha;
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
                Direccion = TDireccion.izquierda;
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W)
                Direccion = TDireccion.arriba;
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S)
                Direccion = TDireccion.abajo;
        }

        private void TocaFruta()
        {
            if (lista1.First.Value.X == Fruta.X && lista1.First.Value.Y == Fruta.Y)//Si la cabeza choca con la fruta
            {
                Crecimiento = new Random().Next(1, 5);//Aumenta en cierto numero su size
                Fruta.X = new Random().Next(1, 80);
                Fruta.Y = new Random().Next(1, 47);
            }
        }

        private void RemoverCola()
        {
            if (Crecimiento == 0)//Si no esta creciendo, voy quitando el ultimo para simular movimiento, ya que en la direccion se va agregando 1
                lista1.RemoveLast();
            else//Si estoy creciendo, dejo que agreguen 1 y mi crecimiento disminuye en 1 y asi
                Crecimiento--;
        }

        private void SaleMapa()
        {
            if (lista1.First.Value.X <= -1 || lista1.First.Value.Y <= -1 ||
                lista1.First.Value.X == 80 || lista1.First.Value.Y == 47)//816, 489
            {
                timer1.Enabled = false;
                MessageBox.Show("Perdio");
            }
        }

        private void SePisa()
        {
            foreach (var ele in lista1)
            {
                if (lista1.First.Value != ele)//Si el elemento es diferente a la cabeza
                    if (ele.X == lista1.First.Value.X &&
                        ele.Y == lista1.First.Value.Y)//Compruebo la posicion de la cabeza con cada elemento del cuerpo
                    {
                        timer1.Enabled = false;
                        MessageBox.Show("Perdio");
                    }
            }
        }


    }

    public class Punto
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Punto(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
}
