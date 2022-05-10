using System;
using System.Collections.Generic;
using System.Text;

namespace Vetores
{
    public class Vetor
    {
        private String[] elementos;
        private int tamanho;
        public Vetor(int capacidade)
        {
            this.elementos = new String[capacidade];
            this.tamanho = 0;
        }
        //public void Adiciona(string elemento)
        //{
        //    for (int i = 0; i < this.elementos.Length; i++)
        //    {
        //        if (this.elementos[i] == null)
        //            this.elementos[i] = elemento;
        //        break;
        //    }
        //}

        public void Adiciona(string elemento)
        {
            if (this.tamanho < this.elementos.Length)
            { 
                this.elementos[this.tamanho] = elemento;
                this.tamanho++;
            }
            else
            {
                throw new ArgumentException("Vetor j´´a está cheio");
            }
        }

        public int Tamanho()
        {
            return this.tamanho;
        }

       

    }
}
