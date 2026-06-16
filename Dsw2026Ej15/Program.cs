using Dsw2026Ej15.Data;
using Dsw2026Ej15.Data.Interfaces;
using System.Transactions;

namespace Dsw2026Ej15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IPersistence p = new PersistenceInMemory();
            Console.WriteLine(p.GetSpecialities().ToString());            
            
        }
    }
}
