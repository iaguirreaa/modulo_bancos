﻿using CapaVista.Componentes.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista
{
    public partial class Navegador : UserControl
    {
        private utilidadesConsultasI utilConsultasI;
        public string operacion = "";
        public string tabla = "";
        public int filaActual = 0;
        public bool gridExiste = true;

        public Form parent;
        public Navegador()
        {
            InitializeComponent();
            this.parent = new Form();
            this.utilConsultasI = new utilidadesConsultasI();
            this.cambiarEstado(false);
        }

        public void config(string tabla, Form parent)
        {
            this.tabla = tabla;
            this.parent = parent;
            this.utilConsultasI.setTabla(this.tabla);
            DataGridView gd = GetDGV(this.parent);
            if (gd == null)
            {
                gridExiste = false;
                return;

            }
            gd.CellClick += this.data_Click;

        }

        void verificarDG()
        {
            if (!gridExiste)
            {

                return;
            }

        }

        private void data_Click(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dt = sender as DataGridView;
            if (dt.SelectedRows.Count > 0)
            {
                filaActual = dt.SelectedRows[0].Index;
            }
        }



        public void identificarFormulario(Form child, string operacion)
        {
            DataGridView dgvname = GetDGV(child);

            if (operacion.Equals("g")) this.utilConsultasI.guardar(child);
            if (operacion.Equals("m")) this.utilConsultasI.modificar(child);
            if (operacion.Equals("r")) this.utilConsultasI.refrescar(child);
            if (operacion.Equals("e")) this.utilConsultasI.eliminar(child, dgvname);
        }



        public DataGridView GetDGV(Form child)
        {
            foreach (Control c in child.Controls)
            {
                if (c is DataGridView dgv)
                {
                    return dgv;
                }
            }
            return null;
            throw new Exception("No se encontró un DataGridView en el formulario.");
        }
        private void btn_guardar_Click(object sender, EventArgs e)
        {
           
            this.identificarFormulario(this.parent, this.operacion);
            this.cambiarEstado(false);
        }

        public void cambiarEstado(bool estado)
        {
            foreach (Control control in this.panel.Controls)
            {
                if (control is Button)
                {
                    Button btn = (Button)control;
                    if (btn.Name.Equals("btn_guardar") || btn.Name.Equals("btn_cancelar"))
                    {
                        btn.Enabled = estado;
                    }
                    else
                    {
                        btn.Enabled = !estado;
                    }
                }
            }

        }

        private void btn_agregar_Click(object sender, EventArgs e)
        {
            this.cambiarEstado(true);
            this.operacion = "g";
        }

        public void limpiarControles()
        {
            foreach (Control control in this.parent.Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Clear();
                }
                else if (control is DateTimePicker)
                {
                    ((DateTimePicker)control).Value = DateTime.Now;
                }
                else if (control is ComboBox)
                {
                    ((ComboBox)control).SelectedIndex = 0;
                }
            }
        }

        private void btn_cancelar_Click(object sender, EventArgs e)
        {
            this.limpiarControles();
            this.cambiarEstado(false);
        }

        private void btn_ayuda_Click_1(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "Ayudas/AyudaSO2.chm", "NavAyuda.html");
        }

        private void btn_refrescar_Click(object sender, EventArgs e)
        {
            this.identificarFormulario(this.parent, "r");
        }

        private void btn_modificar_Click(object sender, EventArgs e)
        {

            try { 
            MessageBox.Show(" Modificar");
            this.utilConsultasI.cargarModificar(this.parent, GetDGV(this.parent));
            this.operacion = "m";
            this.cambiarEstado(true);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                MessageBox.Show("Error" + ex);
            }
          


        }

        private void btn_anterior_Click(object sender, EventArgs e)
        {
            verificarDG();
            try
            {
                DataGridView gd = GetDGV(this.parent);

                gd.ClearSelection();
                if (filaActual > 0)
                {

                    filaActual--;
                    gd.Rows[filaActual].Selected = true;
                }
                else if (filaActual <= 0)
                {
                    MessageBox.Show("No hay filas anteriores para seleccionar la anterior.");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No hay un DataGridView");
            }
        }

        private void btn_siguiente_Click(object sender, EventArgs e)
        {
            verificarDG();

            try
            {
                DataGridView gd = GetDGV(this.parent);
                gd.ClearSelection();
                if (filaActual < gd.Rows.Count - 1)
                {
                    filaActual++;
                    gd.Rows[filaActual].Selected = true;
                }
                else
                {

                    MessageBox.Show("No hay filas posteriores para seleccionar la siguiente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No hay un DataGridView");
            }

        }

        private void btn_inicio_Click(object sender, EventArgs e)
        {
            verificarDG();
            try
            {
                filaActual = 0;
                DataGridView gd = GetDGV(this.parent);
                gd.ClearSelection();
                gd.Rows[0].Selected = true;
                gd.FirstDisplayedScrollingRowIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show("No hay un DataGridView");
            }

        }

        private void btn_fin_Click(object sender, EventArgs e)
        {
            verificarDG();
            try
            {

                DataGridView gd = GetDGV(this.parent);
                gd.ClearSelection();
                gd.Rows[gd.Rows.Count - 1].Selected = true;
                gd.FirstDisplayedScrollingRowIndex = gd.Rows.Count - 1;
                filaActual = gd.Rows.Count - 1;

            }
            catch (Exception ex)
            {
                MessageBox.Show("No hay un DataGridView");
            }

        }

        //Carol Chuy
        private void btn_eliminar_Click(object sender, EventArgs e)
        {   
            this.identificarFormulario(this.parent, "e");
        }
    }
}
