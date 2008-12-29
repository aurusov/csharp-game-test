using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace game
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			m_images = new List<SmartBMP>();
			m_keys   = new List<Keys>();
			m_car    = new Car(this, "C:/game/images/img1.png");
			m_repaintMode = RepaintMode.rm_none;
			m_images.Add(m_car);
		}
		private class SmartBMP
		{
			public SmartBMP( String filename )
			{
				m_bmp   = new Bitmap(filename);
				m_step  = 10;
				m_angel = 45;
			}
			public virtual void paint(Graphics g)
			{
				Point pos = new Point();
				pos.X = m_pos.X - m_bmp.Width / 2;
				pos.Y = m_pos.Y - m_bmp.Height / 2;
				g.TranslateTransform(pos.X, pos.Y);
				g.RotateTransform(m_angel);
				g.DrawImage(m_bmp, 0, 0);
				g.ResetTransform();
			}
			public void left () { m_pos.X -= m_step; setPos(m_pos); }
			public void right() { m_pos.X += m_step; setPos(m_pos); }
			public void up()    { m_pos.Y -= m_step; setPos(m_pos); }
			public void down()  { m_pos.Y += m_step; setPos(m_pos); }
			protected virtual void setPos( Point pos )
			{
				m_pos = pos;
			}

			protected Bitmap m_bmp;
			protected Point  m_pos;
			private   Int32  m_step;
			private   float  m_angel;
		};
		private class Car: SmartBMP
		{
			public Car(MainForm form, String filename)
				: base(filename)
			{
				m_form = form;
				m_fire = false;
			}
			public override void paint(Graphics g)
			{
				base.paint(g);
				if (m_fire)
				{
					g.DrawLine(new Pen(Color.Red, 5), new Point(m_pos.X, m_pos.Y), new Point(m_pos.X, m_pos.Y-100));
				}
			}
			protected override void setPos(Point pos)
			{
				base.setPos(pos);
				if (m_pos.X - m_bmp.Width / 2 < 0)
				{
					m_pos.X = m_bmp.Width / 2;
				}
				if (m_pos.Y - m_bmp.Height / 2 < 0)
				{
					m_pos.Y = m_bmp.Height / 2;
				}
				if (m_pos.X + m_bmp.Width / 2 > m_form.ClientSize.Width)
				{
					m_pos.X = m_form.ClientSize.Width - m_bmp.Width / 2;
				}
				if (m_pos.Y + m_bmp.Height / 2 > m_form.ClientSize.Height)
				{
					m_pos.Y = m_form.ClientSize.Height - m_bmp.Height / 2;
				}
			}
			public void keyDown(Keys keyCode)
			{
				if (keyCode == Keys.Left)
				{
					left();
					m_form.repaint();
				}
				if (keyCode == Keys.Right)
				{
					right();
					m_form.repaint();
				}
				if (keyCode == Keys.Up)
				{
					up();
					m_form.repaint();
				}
				if (keyCode == Keys.Down)
				{
					down();
					m_form.repaint();
				}
				if (keyCode == Keys.Space)
				{
					m_fire = true;
					m_form.repaint();
				}
			}
			public void keyUp( Keys keyCode )
			{
				if (keyCode == Keys.Space)
				{
					m_fire = false;
				}
			}
			private bool  m_fire;
			private MainForm m_form;
		}
		private Car m_car;
		private List<SmartBMP> m_images;
		private List<Keys>     m_keys;

		private enum RepaintMode
		{
			rm_none,
			rm_need_repaint
		};
		private RepaintMode m_repaintMode;
		private void repaint()
		{
			m_repaintMode = RepaintMode.rm_need_repaint;
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			for (int i = 0; i < m_images.Count; i++)
			{
				m_images[i].paint(g);
			}
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if (!m_keys.Contains(e.KeyCode))
			{
				m_keys.Add(e.KeyCode);
			}
		}

		private void Form1_KeyUp(object sender, KeyEventArgs e)
		{
			for (int i = 0; i < m_keys.Count; i++)
			{
				m_car.keyUp(e.KeyCode);
			}
			m_keys.Remove(e.KeyCode);
		}

		private void m_repaintTimer_Tick(object sender, EventArgs e)
		{
			if (m_repaintMode == RepaintMode.rm_need_repaint)
			{
				m_repaintMode = RepaintMode.rm_need_repaint;
				Refresh();
			}
		}

		private void m_stateTimer_Tick(object sender, EventArgs e)
		{
			for (int i = 0; i < m_keys.Count; i++)
			{
				m_car.keyDown(m_keys[i]);
			}
		}
	}
}
