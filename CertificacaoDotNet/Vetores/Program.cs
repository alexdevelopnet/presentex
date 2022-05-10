using System;

namespace Vetores
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Vetor vetor = new Vetor(2);
            try
            {
                vetor.Adiciona("elemento 1");
                vetor.Adiciona("elemento 2");
                vetor.Adiciona("elemento 3");
            }
            catch (Exception e)
            { 
                Console.WriteLine("{0}" , e.Message );
            }
            
        }
    }
}
